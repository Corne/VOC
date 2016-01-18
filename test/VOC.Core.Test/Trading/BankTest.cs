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

        //test validate check based harbors
        //test succes
        //test remove based on harbors
    }
}
