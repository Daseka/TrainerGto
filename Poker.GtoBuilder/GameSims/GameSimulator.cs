using Poker.Common;

namespace Poker.GtoBuilder.GameSims;

public class GameSimulator
{
    private const int PositionCount = 7;
    private readonly IDeckBuilder _deckBuilder;
    private readonly IHandSimulator _handSimulator;

    public GameSimulator(IDeckBuilder deckBuilder, IHandSimulator handSimulator)
    {
        //IStrategy strategy = strategyBuilder.Build(gameData);
        _deckBuilder = deckBuilder;
        _handSimulator = handSimulator;
    }

    public async Task<double> SimulateGame(GameState gameState, int currentPlayerIndex)
    {
        var deck = _deckBuilder.Build(0);
        RemoveKnownCards(gameState, deck);
        var playerPlayed = new bool[gameState.TotalPlayersPlaying.Length];

        while (true)
        {
            //Get hand for current player
            (Rank, Suit)[] hand;
            if (currentPlayerIndex == 0)
            {
                //If hero use HeroCards from game state
                hand = gameState.HeroCards;
            }
            else
            {
                //Else if villain generate acceptable hand for current gamestate
                // hand = GetVillainCards(deck, gameState);
                hand = deck.Peek(2);
            }

            // Get win chance for the current hand
            var (win, draw, loss) = await _handSimulator.SimulateWinChanceOld(
                hand,
                new int[gameState.TotalPlayersPlaying.Count(x => x) - 1],
                gameState.CommunityCards);

            /// get available actions for current player
            var availableActions = GetAvailableActions(gameState, currentPlayerIndex);

            //Get Position of current player
            Position currentPlayerPosition = GetCurrentPlayerPosition(gameState, currentPlayerIndex);

            // Select action based on EV 
            GameActions action = SelectHighEvAction(gameState, availableActions, hand, currentPlayerPosition, win);
            
            switch (action)
            {
                case GameActions.Call:
                    gameState.Bets[currentPlayerIndex] = gameState.Bets.Max();
                    break;

                case GameActions.Fold:
                    gameState.TotalPlayersPlaying[currentPlayerIndex] = false;
                    break;

                case GameActions.Check:
                    gameState.Bets[currentPlayerIndex] = 0;
                    break;

                case GameActions.Bet:
                    gameState.Bets[currentPlayerIndex] = GetBet(gameState, currentPlayerPosition, win, draw, loss);
                    break;

                case GameActions.Raise:
                    gameState.Bets[currentPlayerIndex] = GetRaise(gameState, currentPlayerPosition, win, draw, loss);
                    break;
            }

            // Mark player as having played
            playerPlayed[currentPlayerIndex] = true;

            // Add the bet to the pot
            gameState.PotTotal += gameState.Bets[currentPlayerIndex];

            // set next player
            var nextPlayerIndex = (currentPlayerIndex + 1) % gameState.TotalPlayersPlaying.Length;

            // if not all players have played or not all bets are made, continue to next player
            if (!HasPlayerPlayed(playerPlayed, nextPlayerIndex) || !IsAllBetsMade(gameState, nextPlayerIndex))
            {
                currentPlayerIndex = nextPlayerIndex;
                continue;
            }

            // if not all cards are dealt, deal next cards
            if (!IsAllCardsDealt(gameState))
            {
                DealNextCards(gameState, deck);
                playerPlayed = new bool[gameState.TotalPlayersPlaying.Length];
                currentPlayerIndex = SetSmallBlindAsCurrent(gameState);
                continue;
            }

            // if all players have played and all bets are made, end the game
            break;
        }

        return 0;
    }

    private static GameActions SelectHighEvAction(
        GameState gameState, 
        IList<GameActions> availableActions,
        (Rank, Suit)[] hand,
        Position currentPlayerPosition,
        double winChance)
    {
        if (gameState.CommunityCards.Length == 0)
        {
            //choose action based on chart
        }

        double maxBet = gameState.Bets.Max();
        double ev = (winChance / 100 * (gameState.PotTotal + maxBet)) - ( maxBet * (1 - winChance) / 100);

        // Calculate if should raise/bet
        double minimumRaiselEv = maxBet * 2;
        if (ev > minimumRaiselEv)
        {
            return availableActions.Contains(GameActions.Raise)
                ? GameActions.Raise
                : GameActions.Bet;
        }

        // Calculate if should call
        double minimumCallEv = maxBet * 0.8;
        if (ev > minimumCallEv)
        {
            return availableActions.Contains(GameActions.Call)
                ? GameActions.Call
                : GameActions.Bet;
        }
        
        return availableActions.Contains(GameActions.Fold) 
                ? GameActions.Fold 
                : GameActions.Check;
    }

    private static void DealNextCards(GameState gameState, IDeck deck)
    {
        if (gameState.CommunityCards.Length == 0)
        {
            // Deal the flop
            (Rank, Suit)[] cardsToDeal = deck.Peek(3);
            gameState.CommunityCards = new (Rank, Suit)[3];
            deck.TryDeal(cardsToDeal[0], out gameState.CommunityCards[0]);
            deck.TryDeal(cardsToDeal[1], out gameState.CommunityCards[1]);
            deck.TryDeal(cardsToDeal[2], out gameState.CommunityCards[2]);
        }
        else if (gameState.CommunityCards.Length == 3)
        {
            // Deal the turn
            deck.TryDeal(deck.Peek(1).First(), out (Rank, Suit) cardDealt);
            gameState.CommunityCards = [.. gameState.CommunityCards, cardDealt];
        }
        else if (gameState.CommunityCards.Length == 4)
        {
            // Deal the river
            deck.TryDeal(deck.Peek(1).First(), out (Rank, Suit) cardDealt);
            gameState.CommunityCards = [.. gameState.CommunityCards, cardDealt];
        }
    }

    private static IList<GameActions> GetAvailableActions(GameState gameState, int currentPlayer)
    {
        if (!gameState.TotalPlayersPlaying[currentPlayer])
        {
            throw new ArgumentException($"Player {currentPlayer} is not playing");
        }

        IList<GameActions> actions = new List<GameActions>();
        double maxBet = gameState.Bets.Max();
        if (maxBet == gameState.Bets[currentPlayer])
        {
            actions.Add(GameActions.Check);
        }

        if (maxBet == 0)
        {
            actions.Add(GameActions.Bet);
        }

        if (maxBet != gameState.Bets[currentPlayer])
        {
            actions.Add(GameActions.Fold);
        }
        
        actions.Add(GameActions.Call);
        actions.Add(GameActions.Raise);

        return actions;
    }

    private static double GetBet(
        GameState gameState, 
        Position position, 
        double winChance, 
        double drawChance, 
        double lossChance)
    {
        // Bet size based on position
        if (gameState.CommunityCards.Length <= 0)
        {
            return position switch
            {
                Position.UnderTheGun => gameState.BigBlind * 5,
                Position.HighJack => gameState.BigBlind * 4,
                Position.CutOff => gameState.BigBlind * 3,
                Position.Button => gameState.BigBlind * 2,
                Position.SmallBlind => gameState.BigBlind * 3,
                Position.BigBlind => gameState.BigBlind * 4,
                _ => throw new ArgumentOutOfRangeException(nameof(position), position, null)
            };
        }

        // Bet size bassed on EV calculation
        var win = winChance + Math.Round(drawChance / 2, 0);
        var loss = lossChance + Math.Round(drawChance / 2, 0);

        return (win / 100 * gameState.PotTotal) - (gameState.Bets.Max() * loss / 100);
    }

    private static Position GetCurrentPlayerPosition(GameState gameState, int currentPlayerIndex)
    {
        return (Position)(((int)gameState.HeroPosition + currentPlayerIndex) % PositionCount);
    }

    private static double GetRaise(
        GameState gameState,
        Position currentPlayerPosition,
        double winChance,
        double drawChance,
        double lossChance)
    {
        var maxBet = gameState.Bets.Max();

        // Bet size based on position
        if (gameState.CommunityCards.Length <= 0)
        {
            var playersInGame = gameState.TotalPlayersPlaying.Count(x => x);
            var betValue = currentPlayerPosition switch
            {
                Position.UnderTheGun => gameState.BigBlind * 5,
                Position.HighJack => gameState.BigBlind * 4,
                Position.CutOff => gameState.BigBlind * 3,
                Position.Button => gameState.BigBlind * 2,
                Position.SmallBlind => gameState.BigBlind * 3,
                Position.BigBlind => gameState.BigBlind * 4,
                _ => throw new ArgumentOutOfRangeException(nameof(currentPlayerPosition), currentPlayerPosition, null)
            };

            return betValue + maxBet + (playersInGame * gameState.BigBlind);
        }

        // Bet size bassed on EV calculation
        var win = winChance + Math.Round(drawChance / 2, 0);
        var loss = lossChance + Math.Round(drawChance / 2, 0);
        var minRaise = maxBet + gameState.BigBlind;

        return Math.Max(minRaise, (win / 100 * gameState.PotTotal) - (gameState.Bets.Max() * loss / 100));
    }

    private static bool HasPlayerPlayed(bool[] playerPlayed, int nextPlayerIndex)
    {
        return playerPlayed[nextPlayerIndex];
    }

    private static bool IsAllBetsMade(GameState gameState, int nextPlayerIndex)
    {
        return gameState.Bets[nextPlayerIndex] == gameState.Bets.Max();
    }

    private static bool IsAllCardsDealt(GameState gameState)
    {
        return gameState.CommunityCards.Length == 5;
    }

    private static void RemoveKnownCards(GameState gameState, IDeck deck)
    {
        foreach (var card in gameState.CommunityCards)
        {
            if (!deck.TryDeal(card, out _))
            {
                throw new ArgumentException($"Card {card} is not in the deck");
            }
        }

        foreach (var card in gameState.HeroCards)
        {
            if (!deck.TryDeal(card, out _))
            {
                throw new ArgumentException($"Card {card} is not in the deck");
            }
        }
    }

    private static int SetSmallBlindAsCurrent(GameState gameState)
    {
        return gameState.HeroPosition switch
        {
            Position.UnderTheGun => 4,
            Position.HighJack => 3,
            Position.CutOff => 2,
            Position.Button => 1,
            Position.SmallBlind => 0,
            Position.BigBlind => 5,
            _ => throw new ArgumentOutOfRangeException(nameof(gameState.HeroPosition), gameState.HeroPosition, null)
        };
    }
}