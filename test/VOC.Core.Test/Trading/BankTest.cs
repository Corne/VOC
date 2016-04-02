using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Boards;
using VOC.Core.Establishments;
using VOC.Core.Games.Turns;
using VOC.Core.Items.Achievements;
using VOC.Core.Items.Cards;
using VOC.Core.Items.RawMaterials;
using VOC.Core.Players;
using VOC.Core.Trading;
using Xunit;

namespace VOC.Core.Test.Trading
{
    public class BankTest
    {
        public static IEnumerable<object> NullConstruction
        {
            get
            {
                yield return new object[] { null, new IAchievement[0] };
                yield return new object[] { new Mock<IBoard>().Object, null };
            }
        }

        [Theory, MemberData(nameof(NullConstruction))]
        public void CantBeConstructedWithoutABoard(IBoard board, IEnumerable<IAchievement> achievements)
        {
            Assert.Throws<ArgumentNullException>(() => new Bank(board, achievements));
        }


        [Fact]
        public void CantBuyFromBankWithoutPlayer()
        {
            var board = new Mock<IBoard>();
            var achievements = new IAchievement[0];
            var bank = new Bank(board.Object, achievements);

            Assert.Throws<ArgumentNullException>(() => bank.BuyResource(MaterialType.Brick, MaterialType.Grain, null));
        }

        [Fact]
        public void CantBuyAndOfferSameResource()
        {
            var board = new Mock<IBoard>();
            var player = new Mock<IPlayer>();

            var achievements = new IAchievement[0];
            var bank = new Bank(board.Object, achievements);
            Assert.Throws<ArgumentException>(() => bank.BuyResource(MaterialType.Grain, MaterialType.Grain, player.Object));
        }

        [Theory]
        [InlineData(MaterialType.Unsourced)]
        [InlineData(MaterialType.Sea)]
        [InlineData((MaterialType)33)]
        public void CantBuyInvalidResource(MaterialType material)
        {
            var board = new Mock<IBoard>();
            var player = new Mock<IPlayer>();

            var achievements = new IAchievement[0];
            var bank = new Bank(board.Object, achievements);
            Assert.Throws<ArgumentException>(() => bank.BuyResource(material, MaterialType.Grain, player.Object));
        }

        [Theory]
        [InlineData(MaterialType.Unsourced)]
        [InlineData(MaterialType.Sea)]
        [InlineData((MaterialType)33)]
        public void CantOfferInvalidResource(MaterialType material)
        {
            var board = new Mock<IBoard>();
            var player = new Mock<IPlayer>();

            var achievements = new IAchievement[0];
            var bank = new Bank(board.Object, achievements);
            Assert.Throws<ArgumentException>(() => bank.BuyResource(MaterialType.Grain, material, player.Object));
        }

        [Fact]
        public void BuyFailsIfPlayerHasNotTheOfferedResources()
        {
            var board = new Mock<IBoard>();
            board.Setup(b => b.GetHarbors(It.IsAny<IPlayer>())).Returns(new IHarbor[] { });

            var player = new Mock<IPlayer>();
            player.Setup(p => p.HasResources(It.IsAny<MaterialType[]>())).Returns(false);

            var achievements = new IAchievement[0];
            var bank = new Bank(board.Object, achievements);
            Assert.Throws<InvalidOperationException>(() => bank.BuyResource(MaterialType.Grain, MaterialType.Lumber, player.Object));
        }

        [Fact]
        public void BuyResourceWithoutHarborTest()
        {
            var board = new Mock<IBoard>();
            board.Setup(b => b.GetHarbors(It.IsAny<IPlayer>())).Returns(new IHarbor[] { });

            var player = new Mock<IPlayer>();
            var offer = new MaterialType[] { MaterialType.Lumber, MaterialType.Lumber, MaterialType.Lumber, MaterialType.Lumber };
            player.Setup(p => p.HasResources(offer)).Returns(true);

            var achievements = new IAchievement[0];
            var bank = new Bank(board.Object, achievements);
            bank.BuyResource(MaterialType.Grain, MaterialType.Lumber, player.Object);

            player.Verify(p => p.TakeResources(offer), Times.Once);
            player.Verify(p => p.AddResources(It.Is<IRawMaterial>(r => r.Type == MaterialType.Grain)));
        }

        [Fact]
        public void BuyResourceWithUnsourcedHarbor()
        {
            var board = new Mock<IBoard>();
            var harbor = new Mock<IHarbor>();
            harbor.Setup(h => h.Discount).Returns(MaterialType.Unsourced);
            board.Setup(b => b.GetHarbors(It.IsAny<IPlayer>())).Returns(new IHarbor[] { harbor.Object });

            var player = new Mock<IPlayer>();
            var offer = new MaterialType[] { MaterialType.Brick, MaterialType.Brick, MaterialType.Brick };
            player.Setup(p => p.HasResources(offer)).Returns(true);

            var achievements = new IAchievement[0];
            var bank = new Bank(board.Object, achievements);
            bank.BuyResource(MaterialType.Wool, MaterialType.Brick, player.Object);

            player.Verify(p => p.TakeResources(offer), Times.Once);
            player.Verify(p => p.AddResources(It.Is<IRawMaterial>(r => r.Type == MaterialType.Wool)));
        }

        [Fact]
        public void BuyResourceWitResourceHarbor()
        {
            var board = new Mock<IBoard>();
            var harbor = new Mock<IHarbor>();
            harbor.Setup(h => h.Discount).Returns(MaterialType.Wool);
            board.Setup(b => b.GetHarbors(It.IsAny<IPlayer>())).Returns(new IHarbor[] { harbor.Object });

            var player = new Mock<IPlayer>();
            var offer = new MaterialType[] { MaterialType.Wool, MaterialType.Wool };
            player.Setup(p => p.HasResources(offer)).Returns(true);

            var achievements = new IAchievement[0];
            var bank = new Bank(board.Object, achievements);
            bank.BuyResource(MaterialType.Ore, MaterialType.Wool, player.Object);

            player.Verify(p => p.TakeResources(offer), Times.Once);
            player.Verify(p => p.AddResources(It.Is<IRawMaterial>(r => r.Type == MaterialType.Ore)));
        }

        [Fact]
        public void BuyResourceIgnoresDifferentResourceHarbor()
        {
            var board = new Mock<IBoard>();
            var harbor = new Mock<IHarbor>();
            harbor.Setup(h => h.Discount).Returns(MaterialType.Ore);
            board.Setup(b => b.GetHarbors(It.IsAny<IPlayer>())).Returns(new IHarbor[] { harbor.Object });

            var player = new Mock<IPlayer>();
            var offer = new MaterialType[] { MaterialType.Wool, MaterialType.Wool, MaterialType.Wool, MaterialType.Wool };
            player.Setup(p => p.HasResources(offer)).Returns(true);

            var achievements = new IAchievement[0];
            var bank = new Bank(board.Object, achievements);
            bank.BuyResource(MaterialType.Ore, MaterialType.Wool, player.Object);

            player.Verify(p => p.TakeResources(offer), Times.Once);
            player.Verify(p => p.AddResources(It.Is<IRawMaterial>(r => r.Type == MaterialType.Ore)));
        }

        [Fact]
        public void BuyResourceUsesCheapestHarbor()
        {
            var board = new Mock<IBoard>();
            var resourceHarbor = new Mock<IHarbor>();
            resourceHarbor.Setup(h => h.Discount).Returns(MaterialType.Wool);
            var unsourcedHarbor = new Mock<IHarbor>();
            unsourcedHarbor.Setup(h => h.Discount).Returns(MaterialType.Unsourced);
            board.Setup(b => b.GetHarbors(It.IsAny<IPlayer>())).Returns(new IHarbor[] { unsourcedHarbor.Object, resourceHarbor.Object });

            var player = new Mock<IPlayer>();
            var offer = new MaterialType[] { MaterialType.Wool, MaterialType.Wool };
            player.Setup(p => p.HasResources(offer)).Returns(true);

            var achievements = new IAchievement[0];
            var bank = new Bank(board.Object, achievements);
            bank.BuyResource(MaterialType.Ore, MaterialType.Wool, player.Object);

            player.Verify(p => p.TakeResources(offer), Times.Once);
            player.Verify(p => p.AddResources(It.Is<IRawMaterial>(r => r.Type == MaterialType.Ore)));
        }

        [Fact]
        public void GetInvestmentCostNeedsPlayer()
        {
            var board = new Mock<IBoard>();
            var achievements = new IAchievement[0];
            var bank = new Bank(board.Object, achievements);

            Assert.Throws<ArgumentNullException>(() => bank.GetInvestmentCost(MaterialType.Grain, null));
        }

        [Theory]
        [InlineData(MaterialType.Unsourced)]
        [InlineData(MaterialType.Sea)]
        [InlineData((MaterialType)33)]
        public void GetInvestmentFailsOnInvalidResource(MaterialType material)
        {
            var board = new Mock<IBoard>();
            var player = new Mock<IPlayer>();
            var achievements = new IAchievement[0];
            var bank = new Bank(board.Object, achievements);

            Assert.Throws<ArgumentException>(() => bank.GetInvestmentCost(material, player.Object));
        }

        [Fact]
        public void GetInvestmentCostDefaultReturns4()
        {
            var board = new Mock<IBoard>();
            var player = new Mock<IPlayer>();
            var achievements = new IAchievement[0];
            var bank = new Bank(board.Object, achievements);

            MaterialType[] result = bank.GetInvestmentCost(MaterialType.Grain, player.Object);
            MaterialType[] expected = { MaterialType.Grain, MaterialType.Grain, MaterialType.Grain, MaterialType.Grain };
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetInvestestemtCostIs3OnUnsourcedHarbor()
        {
            var board = new Mock<IBoard>();
            var player = new Mock<IPlayer>();
            var achievements = new IAchievement[0];

            var bank = new Bank(board.Object, achievements);
            var harbor = new Mock<IHarbor>();
            harbor.Setup(h => h.Discount).Returns(MaterialType.Unsourced);
            board.Setup(b => b.GetHarbors(player.Object)).Returns(new IHarbor[] { harbor.Object });

            MaterialType[] result = bank.GetInvestmentCost(MaterialType.Grain, player.Object);
            MaterialType[] expected = { MaterialType.Grain, MaterialType.Grain, MaterialType.Grain };
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetInvestestemtCostIs2OnResourceHarbor()
        {
            var board = new Mock<IBoard>();
            var player = new Mock<IPlayer>();
            var achievements = new IAchievement[0];

            var bank = new Bank(board.Object, achievements);
            var harbor = new Mock<IHarbor>();
            harbor.Setup(h => h.Discount).Returns(MaterialType.Grain);
            board.Setup(b => b.GetHarbors(player.Object)).Returns(new IHarbor[] { harbor.Object });

            MaterialType[] result = bank.GetInvestmentCost(MaterialType.Grain, player.Object);
            MaterialType[] expected = { MaterialType.Grain, MaterialType.Grain };
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetInvestmentCostUsesCheapestHarbor()
        {
            var board = new Mock<IBoard>();
            var player = new Mock<IPlayer>();
            var achievements = new IAchievement[0];
            var bank = new Bank(board.Object, achievements);

            var unsourcedHarbor = new Mock<IHarbor>();
            unsourcedHarbor.Setup(h => h.Discount).Returns(MaterialType.Unsourced);
            var resourceHarbor = new Mock<IHarbor>();
            resourceHarbor.Setup(h => h.Discount).Returns(MaterialType.Grain);
            board.Setup(b => b.GetHarbors(player.Object)).Returns(new IHarbor[] { unsourcedHarbor.Object, resourceHarbor.Object });

            MaterialType[] result = bank.GetInvestmentCost(MaterialType.Grain, player.Object);
            MaterialType[] expected = { MaterialType.Grain, MaterialType.Grain };
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DifferentResourceHarborGetsIgnored()
        {
            var board = new Mock<IBoard>();
            var player = new Mock<IPlayer>();
            var achievements = new IAchievement[0];

            var bank = new Bank(board.Object, achievements);
            var harbor = new Mock<IHarbor>();
            harbor.Setup(h => h.Discount).Returns(MaterialType.Wool);
            board.Setup(b => b.GetHarbors(player.Object)).Returns(new IHarbor[] { harbor.Object });

            MaterialType[] result = bank.GetInvestmentCost(MaterialType.Grain, player.Object);
            MaterialType[] expected = { MaterialType.Grain, MaterialType.Grain, MaterialType.Grain, MaterialType.Grain };
            Assert.Equal(expected, result);
        }


        public static IEnumerable<object> DevelopmentNullParams
        {
            get
            {
                yield return new object[] { null, new Mock<ITurn>().Object };
                yield return new object[] { new Mock<IPlayer>().Object, null };
            }
        }

        [Theory, MemberData(nameof(DevelopmentNullParams))]
        public void BuyDevelopmentCantBeCalledWithoutPlayer(IPlayer player, ITurn turn)
        {
            var board = new Mock<IBoard>();
            var achievements = new IAchievement[0];
            var bank = new Bank(board.Object, achievements);

            Assert.Throws<ArgumentNullException>(() => bank.BuyDevelopmentCard(player, turn));
        }

        [Fact]
        public void BuyDevelopmentCardFailsIfPlayerHasNoResources()
        {
            var board = new Mock<IBoard>();
            var player = new Mock<IPlayer>();
            var turn = new Mock<ITurn>();
            player.Setup(p => p.HasResources(Bank.DEVELOPMENTCARD_COST)).Returns(false);

            var achievements = new IAchievement[0];
            var bank = new Bank(board.Object, achievements);

            Assert.Throws<InvalidOperationException>(() => bank.BuyDevelopmentCard(player.Object, turn.Object));
        }

        [Fact]
        public void BuyDevelopmentCardTest()
        {
            var board = new Mock<IBoard>();
            var player = new Mock<IPlayer>();
            var turn = new Mock<ITurn>();

            player.Setup(p => p.HasResources(Bank.DEVELOPMENTCARD_COST)).Returns(true);

            var achievements = new IAchievement[0];
            var bank = new Bank(board.Object, achievements);

            bank.BuyDevelopmentCard(player.Object, turn.Object);

            player.Verify(p => p.TakeResources(Bank.DEVELOPMENTCARD_COST));
            player.Verify(p => p.AddCard(It.IsAny<IDevelopmentCard>()));
        }

        [Fact]
        public void BuyCardFailsWhenNoMoreCardsLeft()
        {
            var board = new Mock<IBoard>();
            var player = new Mock<IPlayer>();
            var turn = new Mock<ITurn>();

            player.Setup(p => p.HasResources(Bank.DEVELOPMENTCARD_COST)).Returns(true);

            var achievements = new IAchievement[0];
            var bank = new Bank(board.Object, achievements);

            for (int i = 0; i < 25; i++)
            {
                bank.BuyDevelopmentCard(player.Object, turn.Object);
            }
            Assert.Throws<InvalidOperationException>(() => bank.BuyDevelopmentCard(player.Object, turn.Object));
        }

        [Fact]
        public void UpdateAchievementsFailsIfPlayerNull()
        {
            var board = new Mock<IBoard>();
            var mocks = new[] { new Mock<IAchievement>(), new Mock<IAchievement>(), new Mock<IAchievement>() };
            var achievements = mocks.Select(m => m.Object).ToList();
            var bank = new Bank(board.Object, achievements);

            Assert.Throws<ArgumentNullException>(() => bank.UpdateAchievements(null));
        }

        [Fact]
        public void UpdateAchievementUpdatesAllAchievements()
        {
            var board = new Mock<IBoard>();
            var mocks = new[] { new Mock<IAchievement>(), new Mock<IAchievement>(), new Mock<IAchievement>() };
            var achievements = mocks.Select(m => m.Object).ToList();
            var bank = new Bank(board.Object, achievements);

            var player = new Mock<IPlayer>();
            bank.UpdateAchievements(player.Object);

            foreach (var mock in mocks)
                mock.Verify(m => m.Update(player.Object));
        }

        [Fact]
        public void CantVerifyWinConditionOnNullPlayer()
        {
            var board = new Mock<IBoard>();
            var achievements = new IAchievement[0];

            var bank = new Bank(board.Object, achievements);
            Assert.Throws<ArgumentNullException>(() => bank.VerifyWinCondition(null));
        }

        private IEnumerable<IDevelopmentCard> CreateVictoryCards(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var mock = new Mock<IDevelopmentCard>();
                mock.Setup(m => m.Type).Returns(DevelopmentCardType.VictoryPoint);
                yield return mock.Object;
            }
        }

        private IEnumerable<IEstablishment> CreateEstablishments(int count, int value)
        {
            for(int i =0; i<count; i++)
            {
                var mock = new Mock<IEstablishment>();
                mock.Setup(m => m.VictoryPoints).Returns(value);
                yield return mock.Object;
            }
        }

        private IAchievement CreateAchievement(IPlayer player)
        {
            var achievement = new Mock<IAchievement>();
            achievement.Setup(l => l.Owner).Returns(player);
            achievement.Setup(a => a.VictoryPoints).Returns(2);
            return achievement.Object;
        }

        //settlement = 1
        //city = 2
        //victory point card =1 
        //longest road = 2
        //biggest army = 2
        [Theory]
        [InlineData(0, 0, 0, false, false, false)]//total 0
        [InlineData(1, 1, 1, false, false, false)]//total 4
        [InlineData(10, 0, 0, false, false, true)]//total 10
        [InlineData(0, 5, 0, false, false, true)]//total 10
        [InlineData(0, 0, 10, false, false, true)]//total 10
        [InlineData(3, 1, 2, true, false, false)]//total 9
        [InlineData(2, 1, 3, true, true, true)]//total 11
        [InlineData(0, 4, 2, false, true, true)]//total 12
        public void VerifyWinConditionTest(int settlements, int cities, int victoryCards, bool longestRoad, bool biggestArmy, bool expected)
        {
            var player = new Mock<IPlayer>();
            player.Setup(p => p.Cards).Returns(CreateVictoryCards(victoryCards));

            var board = new Mock<IBoard>();
            board.Setup(b => b.GetEstablishments(player.Object)).Returns(CreateEstablishments(settlements, 1).Concat(CreateEstablishments(cities, 2)));

            var roadAchievement = CreateAchievement(longestRoad ? player.Object : new Mock<IPlayer>().Object);
            var armhyAchievement = CreateAchievement(biggestArmy ? player.Object : new Mock<IPlayer>().Object);

            var bank = new Bank(board.Object, new[] { roadAchievement, armhyAchievement });

            bool result = bank.VerifyWinCondition(player.Object);
            Assert.Equal(expected, result);
        }
    }
}
