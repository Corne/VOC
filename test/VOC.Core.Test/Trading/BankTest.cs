using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Boards;
using VOC.Core.Items.RawMaterials;
using VOC.Core.Players;
using VOC.Core.Trading;
using Xunit;

namespace VOC.Core.Test.Trading
{
    public class BankTest
    {
        [Fact]
        public void CantBeConstructedWithoutABoard()
        {
            Assert.Throws<ArgumentNullException>(() => new Bank(null));
        }

        [Fact]
        public void BankConstructionTest()
        {
            var board = new Mock<IBoard>();
            var bank = new Bank(board.Object);

            //kinda useless, but nothing to test at the moment...
            Assert.NotNull(bank);
        }

        [Fact]
        public void CantBuyFromBankWithoutPlayer()
        {
            var board = new Mock<IBoard>();
            var bank = new Bank(board.Object);

            Assert.Throws<ArgumentNullException>(() => bank.BuyResource(MaterialType.Brick, MaterialType.Grain, null));
        }

        [Fact]
        public void CantBuyAndOfferSameResource()
        {
            var board = new Mock<IBoard>();
            var player = new Mock<IPlayer>();

            var bank = new Bank(board.Object);
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

            var bank = new Bank(board.Object);
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

            var bank = new Bank(board.Object);
            Assert.Throws<ArgumentException>(() => bank.BuyResource(MaterialType.Grain, material, player.Object));
        }

        [Fact]
        public void BuyFailsIfPlayerHasNotTheOfferedResources()
        {
            var board = new Mock<IBoard>();
            board.Setup(b => b.GetHarbors(It.IsAny<IPlayer>())).Returns(new IHarbor[] { });

            var player = new Mock<IPlayer>();
            player.Setup(p => p.HasResources(It.IsAny<MaterialType[]>())).Returns(false);

            var bank = new Bank(board.Object);
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

            var bank = new Bank(board.Object);
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

            var bank = new Bank(board.Object);
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

            var bank = new Bank(board.Object);
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

            var bank = new Bank(board.Object);
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

            var bank = new Bank(board.Object);
            bank.BuyResource(MaterialType.Ore, MaterialType.Wool, player.Object);

            player.Verify(p => p.TakeResources(offer), Times.Once);
            player.Verify(p => p.AddResources(It.Is<IRawMaterial>(r => r.Type == MaterialType.Ore)));
        }

        [Fact]
        public void GetInvestmentCostNeedsPlayer()
        {
            var board = new Mock<IBoard>();
            var bank = new Bank(board.Object);

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
            var bank = new Bank(board.Object);

            Assert.Throws<ArgumentException>(() => bank.GetInvestmentCost(material, player.Object));
        }

        [Fact]
        public void GetInvestmentCostDefaultReturns4()
        {
            var board = new Mock<IBoard>();
            var player = new Mock<IPlayer>();
            var bank = new Bank(board.Object);

            MaterialType[] result = bank.GetInvestmentCost(MaterialType.Grain, player.Object);
            MaterialType[] expected = { MaterialType.Grain, MaterialType.Grain, MaterialType.Grain, MaterialType.Grain };
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetInvestestemtCostIs3OnUnsourcedHarbor()
        {
            var board = new Mock<IBoard>();
            var player = new Mock<IPlayer>();
            var bank = new Bank(board.Object);
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
            var bank = new Bank(board.Object);
            var harbor = new Mock<IHarbor>();
            harbor.Setup(h => h.Discount).Returns(MaterialType.Grain);
            board.Setup(b => b.GetHarbors(player.Object)).Returns(new IHarbor[] { harbor.Object });

            MaterialType[] result = bank.GetInvestmentCost(MaterialType.Grain, player.Object);
            MaterialType[] expected = { MaterialType.Grain, MaterialType.Grain};
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetInvestmentCostUsesCheapestHarbor()
        {
            var board = new Mock<IBoard>();
            var player = new Mock<IPlayer>();
            var bank = new Bank(board.Object);

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
            var bank = new Bank(board.Object);
            var harbor = new Mock<IHarbor>();
            harbor.Setup(h => h.Discount).Returns(MaterialType.Wool);
            board.Setup(b => b.GetHarbors(player.Object)).Returns(new IHarbor[] { harbor.Object });

            MaterialType[] result = bank.GetInvestmentCost(MaterialType.Grain, player.Object);
            MaterialType[] expected = { MaterialType.Grain, MaterialType.Grain, MaterialType.Grain, MaterialType.Grain };
            Assert.Equal(expected, result);
        }

    }
}
