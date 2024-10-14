using Poker.GameReader.Hands;
using Poker.GameReader.Reporters;

namespace Tests.Hands
{
    public class StraightTest
    {
        [Fact]
        public void bla()
        {
            // 2,3,x,5,x,x,x,x,10,11
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.Ace, CardSuit.Hart),
                    (CardSymbol.Queen, CardSuit.Hart )
                ],

                CommunityCards =
                [
                    (CardSymbol.Jack, CardSuit.Club),
                    (CardSymbol.Ten, CardSuit.Club),
                    (CardSymbol.Six, CardSuit.Club),
                    (CardSymbol.None, CardSuit.None),
                    (CardSymbol.None, CardSuit.None),
                ]
            };

            double chance = Straight.CalculateChance(gameData);

            Assert.Equal(0.17, chance);
        }

        [Fact]
        public void BackdoorGutShotTwoCard()
        {
            // 2,3,x,5,x,x,x,x,10,11
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.Jack, CardSuit.Hart),
                    (CardSymbol.Five, CardSuit.Hart )
                ],

                CommunityCards =
                [
                    (CardSymbol.Ten, CardSuit.Club),
                    (CardSymbol.Three, CardSuit.Club),
                    (CardSymbol.Two, CardSuit.Club),
                    (CardSymbol.None, CardSuit.None),
                    (CardSymbol.None, CardSuit.None),
                ]
            };

            double chance = Straight.CalculateChance(gameData);

            Assert.Equal(0.02, chance);
        }

        [Fact]
        public void BackdoorOpenEndedTwoCard()
        {
            // x,x,4,5,6,x,x,x,10,11
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.Jack, CardSuit.Hart),
                    (CardSymbol.Five, CardSuit.Hart)
                ],

                CommunityCards =
                [
                    (CardSymbol.Ten, CardSuit.Club),
                    (CardSymbol.Four, CardSuit.Club),
                    (CardSymbol.Six, CardSuit.Club),
                    (CardSymbol.None, CardSuit.None),
                    (CardSymbol.None, CardSuit.None),
                ]
            };

            double chance = Straight.CalculateChance(gameData);

            Assert.Equal(0.04, chance);
        }

        [Fact]
        public void GutshotOneCard()
        {
            // 2,3,x,5,6,x,x,x,10,11
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.Jack, CardSuit.Hart),
                    (CardSymbol.Five, CardSuit.Hart)
                ],

                CommunityCards =
                [
                    (CardSymbol.None, CardSuit.None),
                    (CardSymbol.Ten, CardSuit.Club),
                    (CardSymbol.Three, CardSuit.Club),
                    (CardSymbol.Two, CardSuit.Club),
                    (CardSymbol.Six, CardSuit.Club),
                ]
            };

            double chance = Straight.CalculateChance(gameData);

            Assert.Equal(0.09, chance);
        }

        [Fact]
        public void GutshotTwoCard()
        {
            // 2,3,x,5,6,x,x,x,x,11
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.Jack, CardSuit.Hart),
                    (CardSymbol.Five, CardSuit.Hart)
                ],

                CommunityCards =
                [
                    (CardSymbol.None, CardSuit.None),
                    (CardSymbol.None, CardSuit.None),
                    (CardSymbol.Three, CardSuit.Club),
                    (CardSymbol.Two, CardSuit.Club),
                    (CardSymbol.Six, CardSuit.Club),
                ]
            };

            double chance = Straight.CalculateChance(gameData);

            Assert.Equal(0.17, chance);
        }

        [Fact]
        public void OnlyTwoCardsConnected()
        {
            // x,x,x,5,6,x,x,x,x,x
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.Six, CardSuit.Hart),
                    (CardSymbol.Five, CardSuit.Hart)
                ],

                CommunityCards =
                [
                    (CardSymbol.None, CardSuit.None),
                    (CardSymbol.None, CardSuit.None),
                    (CardSymbol.None, CardSuit.None),
                    (CardSymbol.None, CardSuit.None),
                    (CardSymbol.None, CardSuit.None),
                ]
            };

            double chance = Straight.CalculateChance(gameData);

            Assert.Equal(0, chance);
        }

        [Fact]
        public void OnlyTwoCardsUNConnected()
        {
            // x,x,x,5,x,x,x,9,x,x
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.Nine, CardSuit.Hart),
                    (CardSymbol.Five, CardSuit.Hart)
                ],

                CommunityCards =
                [
                    (CardSymbol.None, CardSuit.None),
                    (CardSymbol.None, CardSuit.None),
                    (CardSymbol.None, CardSuit.None),
                    (CardSymbol.None, CardSuit.None),
                    (CardSymbol.None, CardSuit.None),
                ]
            };

            double chance = Straight.CalculateChance(gameData);

            Assert.Equal(0, chance);
        }

        [Fact]
        public void OpenEndedOneCard()
        {
            // 2,3,x,x,x,x,8,9,10,11
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.Two, CardSuit.Hart),
                    (CardSymbol.Nine, CardSuit.Hart)
                ],

                CommunityCards =
                [
                    (CardSymbol.Three, CardSuit.Club),
                    (CardSymbol.Ten, CardSuit.Club),
                    (CardSymbol.Eight, CardSuit.Club),
                    (CardSymbol.Jack, CardSuit.Club)
                ]
            };

            double chance = Straight.CalculateChance(gameData);

            Assert.Equal(0.17, chance);
        }

        [Fact]
        public void OpenEndedTwoCard()
        {
            // 2,x,x,x,x,x,8,9,10,11
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.Two, CardSuit.Hart),
                    (CardSymbol.Nine, CardSuit.Hart)
                ],

                CommunityCards =
                [
                    (CardSymbol.Eight, CardSuit.Club),
                    (CardSymbol.Ten, CardSuit.Club),
                    (CardSymbol.Jack, CardSuit.Club)
                ]
            };

            double chance = Straight.CalculateChance(gameData);

            Assert.Equal(0.32, chance);
        }

        [Fact]
        public void StraightOneCard()
        {
            // 2,3,4,5,6,x,x,x,x,x
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.Four, CardSuit.Hart),
                    (CardSymbol.Five, CardSuit.Hart)
                ],

                CommunityCards =
                [
                    (CardSymbol.Three, CardSuit.Club),
                    (CardSymbol.Two, CardSuit.Club),
                    (CardSymbol.Six, CardSuit.Club),
                ]
            };

            double chance = Straight.CalculateChance(gameData);

            Assert.Equal(1, chance);
        }

        [Fact]
        public void StraightTwoCard()
        {
            // 2,3,4,5,6,x,x,x,x,x1
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.Four, CardSuit.Hart),
                    (CardSymbol.Five, CardSuit.Hart)
                ],

                CommunityCards =
                [
                    (CardSymbol.Three, CardSuit.Club),
                    (CardSymbol.Two, CardSuit.Club),
                    (CardSymbol.Six, CardSuit.Club),
                ]
            };

            double chance = Straight.CalculateChance(gameData);

            Assert.Equal(1, chance);
        }
    }
}