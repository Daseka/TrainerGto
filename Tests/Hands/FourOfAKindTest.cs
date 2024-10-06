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
                    (CardSymbol.Five, CardSuit.Club),
                    (CardSymbol.Five, CardSuit.Hart)
                ],

                CommunityCards =
                [
                    (CardSymbol.Ten, CardSuit.Hart),
                    (CardSymbol.Three, CardSuit.Club),
                    (CardSymbol.Two, CardSuit.Club),
                    (CardSymbol.None, CardSuit.None),
                    (CardSymbol.None, CardSuit.None),
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
                    (CardSymbol.Five, CardSuit.Club),
                    (CardSymbol.Five, CardSuit.Hart)
                ],

                CommunityCards =
                [
                    (CardSymbol.Five, CardSuit.Spade),
                    (CardSymbol.Three, CardSuit.Club),
                    (CardSymbol.Two, CardSuit.Club),
                    (CardSymbol.None, CardSuit.None),
                    (CardSymbol.None, CardSuit.None),
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
                    (CardSymbol.Five, CardSuit.Club),
                    (CardSymbol.Five, CardSuit.Hart)
                ],

                CommunityCards =
                [
                    (CardSymbol.Five, CardSuit.Spade),
                    (CardSymbol.Three, CardSuit.Club),
                    (CardSymbol.Two, CardSuit.Club),
                    (CardSymbol.Ten, CardSuit.Diamond),
                    (CardSymbol.None, CardSuit.None),
                ]
            };

            double chance = FourOfAKind.CalculateChance(gameData);

            Assert.Equal(0.02, chance);
        }
    }
}