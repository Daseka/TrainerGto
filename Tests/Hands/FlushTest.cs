using Poker.GameReader.Hands;
using Poker.GameReader.Reporters;

namespace Tests.Hands
{
    public class FlushTest
    {
        [Fact]
        public void ThreeSuitedTwoCard()
        {
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.Jack, CardSuit.Hart),
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

            double chance = Flush.CalculateChance(gameData);

            Assert.Equal(0.04, chance);
        }

        [Fact]
        public void FourSuitedOneCard()
        {
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.Jack, CardSuit.Hart),
                    (CardSymbol.Five, CardSuit.Hart)
                ],

                CommunityCards =
                [
                    (CardSymbol.Ten, CardSuit.Hart),
                    (CardSymbol.Three, CardSuit.Club),
                    (CardSymbol.Two, CardSuit.Club),
                    (CardSymbol.Seven, CardSuit.Hart),
                    (CardSymbol.None, CardSuit.None),
                ]
            };

            double chance = Flush.CalculateChance(gameData);

            Assert.Equal(0.19, chance);
        }

        [Fact]
        public void FourSuitedTwoCard()
        {
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.Jack, CardSuit.Diamond),
                    (CardSymbol.Five, CardSuit.Diamond)
                ],

                CommunityCards =
                [
                    (CardSymbol.Ten, CardSuit.Diamond),
                    (CardSymbol.Three, CardSuit.Club),
                    (CardSymbol.Seven, CardSuit.Diamond),
                    (CardSymbol.None, CardSuit.None),
                    (CardSymbol.None, CardSuit.None),
                ]
            };

            double chance = Flush.CalculateChance(gameData);

            Assert.Equal(0.35, chance);
        }

        [Fact]
        public void SpadeFlushTwoCard()
        {
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.Jack, CardSuit.Spade),
                    (CardSymbol.Five, CardSuit.Spade)
                ],

                CommunityCards =
                [
                    (CardSymbol.Ten, CardSuit.Spade),
                    (CardSymbol.Three, CardSuit.Spade),
                    (CardSymbol.Seven, CardSuit.Spade),
                    (CardSymbol.None, CardSuit.None),
                    (CardSymbol.None, CardSuit.None),
                ]
            };

            double chance = Flush.CalculateChance(gameData);

            Assert.Equal(1, chance);
        }

        [Fact]
        public void DiamondFlushOneCard()
        {
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.Jack, CardSuit.Diamond),
                    (CardSymbol.Five, CardSuit.Diamond)
                ],

                CommunityCards =
                [
                    (CardSymbol.Ten, CardSuit.Diamond),
                    (CardSymbol.Three, CardSuit.Diamond),
                    (CardSymbol.Seven, CardSuit.Diamond),
                    (CardSymbol.Two, CardSuit.Hart),
                    (CardSymbol.None, CardSuit.None),
                ]
            };

            double chance = Flush.CalculateChance(gameData);

            Assert.Equal(1, chance);
        }

        [Fact]
        public void DrawingDeadTwoCard()
        {
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.Jack, CardSuit.Spade),
                    (CardSymbol.Five, CardSuit.Hart)
                ],

                CommunityCards =
                [
                    (CardSymbol.Ten, CardSuit.Diamond),
                    (CardSymbol.Three, CardSuit.Club),
                    (CardSymbol.Seven, CardSuit.Spade),
                    (CardSymbol.None, CardSuit.None),
                    (CardSymbol.None, CardSuit.None),
                ]
            };

            double chance = Flush.CalculateChance(gameData);

            Assert.Equal(0, chance);
        }

        [Fact]
        public void DrawingDeadOneCard()
        {
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.Jack, CardSuit.Spade),
                    (CardSymbol.Five, CardSuit.Hart)
                ],

                CommunityCards =
                [
                    (CardSymbol.Ten, CardSuit.Diamond),
                    (CardSymbol.Three, CardSuit.Club),
                    (CardSymbol.Seven, CardSuit.Spade),
                    (CardSymbol.Ace, CardSuit.Spade),
                    (CardSymbol.None, CardSuit.None),
                ]
            };

            double chance = Flush.CalculateChance(gameData);

            Assert.Equal(0, chance);
        }
    }
}