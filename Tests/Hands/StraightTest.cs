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
                    (CardRank.Ace, CardSuit.Hart),
                    (CardRank.Queen, CardSuit.Hart )
                ],

                CommunityCards =
                [
                    (CardRank.Jack, CardSuit.Club),
                    (CardRank.Ten, CardSuit.Club),
                    (CardRank.Six, CardSuit.Club),
                    (CardRank.None, CardSuit.None),
                    (CardRank.None, CardSuit.None),
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
                    (CardRank.Jack, CardSuit.Hart),
                    (CardRank.Five, CardSuit.Hart )
                ],

                CommunityCards =
                [
                    (CardRank.Ten, CardSuit.Club),
                    (CardRank.Three, CardSuit.Club),
                    (CardRank.Two, CardSuit.Club),
                    (CardRank.None, CardSuit.None),
                    (CardRank.None, CardSuit.None),
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
                    (CardRank.Jack, CardSuit.Hart),
                    (CardRank.Five, CardSuit.Hart)
                ],

                CommunityCards =
                [
                    (CardRank.Ten, CardSuit.Club),
                    (CardRank.Four, CardSuit.Club),
                    (CardRank.Six, CardSuit.Club),
                    (CardRank.None, CardSuit.None),
                    (CardRank.None, CardSuit.None),
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
                    (CardRank.Jack, CardSuit.Hart),
                    (CardRank.Five, CardSuit.Hart)
                ],

                CommunityCards =
                [
                    (CardRank.None, CardSuit.None),
                    (CardRank.Ten, CardSuit.Club),
                    (CardRank.Three, CardSuit.Club),
                    (CardRank.Two, CardSuit.Club),
                    (CardRank.Six, CardSuit.Club),
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
                    (CardRank.Jack, CardSuit.Hart),
                    (CardRank.Five, CardSuit.Hart)
                ],

                CommunityCards =
                [
                    (CardRank.None, CardSuit.None),
                    (CardRank.None, CardSuit.None),
                    (CardRank.Three, CardSuit.Club),
                    (CardRank.Two, CardSuit.Club),
                    (CardRank.Six, CardSuit.Club),
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
                    (CardRank.Six, CardSuit.Hart),
                    (CardRank.Five, CardSuit.Hart)
                ],

                CommunityCards =
                [
                    (CardRank.None, CardSuit.None),
                    (CardRank.None, CardSuit.None),
                    (CardRank.None, CardSuit.None),
                    (CardRank.None, CardSuit.None),
                    (CardRank.None, CardSuit.None),
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
                    (CardRank.Nine, CardSuit.Hart),
                    (CardRank.Five, CardSuit.Hart)
                ],

                CommunityCards =
                [
                    (CardRank.None, CardSuit.None),
                    (CardRank.None, CardSuit.None),
                    (CardRank.None, CardSuit.None),
                    (CardRank.None, CardSuit.None),
                    (CardRank.None, CardSuit.None),
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
                    (CardRank.Two, CardSuit.Hart),
                    (CardRank.Nine, CardSuit.Hart)
                ],

                CommunityCards =
                [
                    (CardRank.Three, CardSuit.Club),
                    (CardRank.Ten, CardSuit.Club),
                    (CardRank.Eight, CardSuit.Club),
                    (CardRank.Jack, CardSuit.Club)
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
                    (CardRank.Two, CardSuit.Hart),
                    (CardRank.Nine, CardSuit.Hart)
                ],

                CommunityCards =
                [
                    (CardRank.Eight, CardSuit.Club),
                    (CardRank.Ten, CardSuit.Club),
                    (CardRank.Jack, CardSuit.Club)
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
                    (CardRank.Four, CardSuit.Hart),
                    (CardRank.Five, CardSuit.Hart)
                ],

                CommunityCards =
                [
                    (CardRank.Three, CardSuit.Club),
                    (CardRank.Two, CardSuit.Club),
                    (CardRank.Six, CardSuit.Club),
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
                    (CardRank.Four, CardSuit.Hart),
                    (CardRank.Five, CardSuit.Hart)
                ],

                CommunityCards =
                [
                    (CardRank.Three, CardSuit.Club),
                    (CardRank.Two, CardSuit.Club),
                    (CardRank.Six, CardSuit.Club),
                ]
            };

            double chance = Straight.CalculateChance(gameData);

            Assert.Equal(1, chance);
        }
    }
}