using Poker.GameReader.Hands;
using Poker.GameReader.Reporters;

namespace Tests.Hands
{
    public class FourOfAKindTest
    {
        [Fact]
        public void TwoOfAKindTwoCard()
        {
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardRank.Five, CardSuit.Club),
                    (CardRank.Five, CardSuit.Hart)
                ],

                CommunityCards =
                [
                    (CardRank.Ten, CardSuit.Hart),
                    (CardRank.Three, CardSuit.Club),
                    (CardRank.Two, CardSuit.Club),
                    (CardRank.None, CardSuit.None),
                    (CardRank.None, CardSuit.None),
                ]
            };

            double chance = FourOfAKind.CalculateChance(gameData);

            Assert.Equal(0.001, chance);
        }

        [Fact]
        public void ThreeOfAKindTwoCard()
        {
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardRank.Five, CardSuit.Club),
                    (CardRank.Five, CardSuit.Hart)
                ],

                CommunityCards =
                [
                    (CardRank.Five, CardSuit.Spade),
                    (CardRank.Three, CardSuit.Club),
                    (CardRank.Two, CardSuit.Club),
                    (CardRank.None, CardSuit.None),
                    (CardRank.None, CardSuit.None),
                ]
            };

            double chance = FourOfAKind.CalculateChance(gameData);

            Assert.Equal(0.02, chance);
        }

        [Fact]
        public void ThreeOfAKindOneCard()
        {
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardRank.Five, CardSuit.Club),
                    (CardRank.Five, CardSuit.Hart)
                ],

                CommunityCards =
                [
                    (CardRank.Five, CardSuit.Spade),
                    (CardRank.Three, CardSuit.Club),
                    (CardRank.Two, CardSuit.Club),
                    (CardRank.Ten, CardSuit.Diamond),
                    (CardRank.None, CardSuit.None),
                ]
            };

            double chance = FourOfAKind.CalculateChance(gameData);

            Assert.Equal(0.02, chance);
        }

        [Fact]
        public void TwoOfAKindOneCard()
        {
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardRank.Five, CardSuit.Club),
                    (CardRank.Five, CardSuit.Hart)
                ],

                CommunityCards =
                [
                    (CardRank.King, CardSuit.Spade),
                    (CardRank.Three, CardSuit.Club),
                    (CardRank.Two, CardSuit.Club),
                    (CardRank.Ten, CardSuit.Diamond),
                    (CardRank.None, CardSuit.None),
                ]
            };

            double chance = FourOfAKind.CalculateChance(gameData);

            Assert.Equal(0, chance);
        }

        [Fact]
        public void FourOfAKindOneCard()
        {
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardRank.Five, CardSuit.Club),
                    (CardRank.Five, CardSuit.Hart)
                ],

                CommunityCards =
                [
                    (CardRank.Five, CardSuit.Spade),
                    (CardRank.Three, CardSuit.Club),
                    (CardRank.Two, CardSuit.Club),
                    (CardRank.Five, CardSuit.Diamond),
                    (CardRank.None, CardSuit.None),
                ]
            };

            double chance = FourOfAKind.CalculateChance(gameData);

            Assert.Equal(1, chance);
        }

        [Fact]
        public void FourOfAKindTwoCard()
        {
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardRank.Five, CardSuit.Club),
                    (CardRank.Five, CardSuit.Hart)
                ],

                CommunityCards =
                [
                    (CardRank.Five, CardSuit.Spade),
                    (CardRank.Three, CardSuit.Club),
                    (CardRank.None, CardSuit.None),
                    (CardRank.Five, CardSuit.Diamond),
                    (CardRank.None, CardSuit.None),
                ]
            };

            double chance = FourOfAKind.CalculateChance(gameData);

            Assert.Equal(1, chance);
        }
    }
}