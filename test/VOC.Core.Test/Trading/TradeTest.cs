using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Items.RawMaterials;
using VOC.Core.Players;
using VOC.Core.Trading;
using Xunit;

namespace VOC.Core.Test.Trading
{
    public class TradeTest
    {
        [Fact]
        public void ConstructionOfferCantBeNull()
        {
            var request = new MaterialType[] { MaterialType.Brick };
            var player = new Mock<IPlayer>();

            Assert.Throws<ArgumentNullException>(() => new Trade(null, request, player.Object));
        }

        [Fact]
        public void CosntructionRequestCantBeNull()
        {
            var offer = new MaterialType[] { MaterialType.Brick };
            var player = new Mock<IPlayer>();

            Assert.Throws<ArgumentNullException>(() => new Trade(offer, null, player.Object));
        }

        [Fact]
        public void ConstructionPlayerCantBeNull()
        {
            var request = new MaterialType[] { MaterialType.Lumber };
            var offer = new MaterialType[] { MaterialType.Wool };

            Assert.Throws<ArgumentNullException>(() => new Trade(offer, request, null));
        }

        [Fact]
        public void ConstructionBothRequestAndOfferCantBeEmpty()
        {
            var request = new MaterialType[] { };
            var offer = new MaterialType[] { };
            var player = new Mock<IPlayer>();

            Assert.Throws<ArgumentException>(() => new Trade(offer, request, player.Object));
        }

        [Theory]
        [InlineData(new MaterialType[] { MaterialType.Sea }, new MaterialType[] { MaterialType.Wool })]
        [InlineData(new MaterialType[] { MaterialType.Unsourced }, new MaterialType[] { MaterialType.Wool })]
        [InlineData(new MaterialType[] { MaterialType.Brick }, new MaterialType[] { MaterialType.Sea })]
        [InlineData(new MaterialType[] { MaterialType.Brick }, new MaterialType[] { MaterialType.Unsourced })]
        public void ConstructionCantTradeSeaAndDessert(MaterialType[] request, MaterialType[] offer)
        {
            var player = new Mock<IPlayer>();

            Assert.Throws<ArgumentException>(() => new Trade(offer, request, player.Object));

        }

        [Fact]
        public void ConstructionPlayerShouldHaveResourcessHeOffers()
        {
            var request = new MaterialType[] { MaterialType.Lumber };
            var offer = new MaterialType[] { MaterialType.Wool };
            var player = new Mock<IPlayer>();
            player.Setup(p => p.HasResources(offer)).Returns(false);

            Assert.Throws<InvalidOperationException>(() => new Trade(offer, request, player.Object));
            player.Verify(p => p.HasResources(offer), Times.AtLeastOnce());
        }

        [Theory]
        [InlineData(new MaterialType[] { MaterialType.Brick }, new MaterialType[] { MaterialType.Wool })]
        [InlineData(new MaterialType[] { }, new MaterialType[] { MaterialType.Wool })]
        [InlineData(new MaterialType[] { MaterialType.Brick }, new MaterialType[] { })]
        [InlineData(new MaterialType[] { MaterialType.Brick, MaterialType.Brick }, new MaterialType[] { MaterialType.Wool, MaterialType.Grain })]
        [InlineData(new MaterialType[] { MaterialType.Brick, MaterialType.Brick, MaterialType.Ore }, new MaterialType[] { MaterialType.Wool })]
        [InlineData(new MaterialType[] { MaterialType.Brick }, new MaterialType[] { MaterialType.Wool, MaterialType.Grain, MaterialType.Ore })]
        public void ConstructionTest(MaterialType[] request, MaterialType[] offer)
        {
            var player = new Mock<IPlayer>();
            player.Setup(p => p.HasResources(offer)).Returns(true);

            var trade = new Trade(offer, request, player.Object);

            Assert.Equal(request, trade.Request);
            Assert.Equal(offer, trade.Offer);
            Assert.Equal(player.Object, trade.Owner);
            Assert.Equal(TradeState.Open, trade.State);
        }

        private Trade CreateValid()
        {
            var offer = new MaterialType[] { MaterialType.Brick, MaterialType.Brick };
            var request = new MaterialType[] { MaterialType.Wool, MaterialType.Grain };
            var player1 = new Mock<IPlayer>();
            player1.Setup(p => p.HasResources(offer)).Returns(true);

            return new Trade(offer, request, player1.Object);
        }

        [Fact]
        public void AcceptPlayerCantBeNull()
        {
            var trade = CreateValid();
            Assert.Throws<ArgumentNullException>(() => trade.Accept(null));
        }

        [Fact]
        public void AcceptPlayerCantBeSameAsOwner()
        {
            var trade = CreateValid();
            Assert.Throws<ArgumentException>(() => trade.Accept(trade.Owner));
        }

        [Fact]
        public void AcceptPlayerShouldHaveRequestedMaterials()
        {
            var trade = CreateValid();
            var player = new Mock<IPlayer>();
            player.Setup(p => p.HasResources(trade.Request)).Returns(false);

            Assert.Throws<InvalidOperationException>(() => trade.Accept(player.Object));
            player.Verify(p => p.HasResources(trade.Request), Times.AtLeastOnce());
        }

        [Fact]
        public void CantAcceptIfNoRequestMaterials()
        {
            var offer = new MaterialType[] { MaterialType.Brick, MaterialType.Brick };
            var request = new MaterialType[] { };
            var player1 = new Mock<IPlayer>();
            var player2 = new Mock<IPlayer>();
            player1.Setup(p => p.HasResources(offer)).Returns(true);
            player2.Setup(p => p.HasResources(request)).Returns(false);

            var trade = new Trade(offer, request, player1.Object);

            Assert.Throws<InvalidOperationException>(() => trade.Accept(player2.Object));
        }

        [Fact]
        public void CantAcceptIfNoOfferMaterials()
        {
            var offer = new MaterialType[] {  };
            var request = new MaterialType[] { MaterialType.Wool, MaterialType.Grain };
            var player1 = new Mock<IPlayer>();
            var player2 = new Mock<IPlayer>();
            player1.Setup(p => p.HasResources(offer)).Returns(true);
            player2.Setup(p => p.HasResources(request)).Returns(true);

            var trade = new Trade(offer, request, player1.Object);
            //canged to false after trade created
            player1.Setup(p => p.HasResources(offer)).Returns(false);

            Assert.Throws<InvalidOperationException>(() => trade.Accept(player2.Object));
        }

        [Fact]
        public void CantAcceptAfterCancel()
        {
            var trade = CreateValid();
            var player = new Mock<IPlayer>();
            player.Setup(p => p.HasResources(trade.Request)).Returns(true);

            trade.Cancel();

            Assert.Throws<InvalidOperationException>(() => trade.Accept(player.Object));
        }

        [Fact]
        public void CantAcceptTwice()
        {
            var trade = CreateValid();
            var player = new Mock<IPlayer>();
            player.Setup(p => p.HasResources(trade.Request)).Returns(true);

            trade.Accept(player.Object);

            Assert.Throws<InvalidOperationException>(() => trade.Accept(player.Object));
        }

        [Fact]
        public void AcceptTest()
        {
            var offer = new MaterialType[] { MaterialType.Brick, MaterialType.Brick };
            var request = new MaterialType[] { MaterialType.Wool, MaterialType.Grain };
            var offeredMaterials = offer.Select(m => new RawMaterial(m)).ToArray();
            var requestedMaterrials = request.Select(m => new RawMaterial(m)).ToArray();

            var player1 = new Mock<IPlayer>();
            player1.Setup(p => p.HasResources(offer)).Returns(true);
            player1.Setup(p => p.TakeResources(offer)).Returns(offeredMaterials);

            var player2 = new Mock<IPlayer>();
            player2.Setup(p => p.HasResources(request)).Returns(true);
            player2.Setup(p => p.TakeResources(request)).Returns(requestedMaterrials);

            var trade = new Trade(offer, request, player1.Object);
            trade.Accept(player2.Object);

            player1.Verify(p => p.TakeResources(offer), Times.Once);
            player2.Verify(p => p.TakeResources(request), Times.Once);
            player1.Verify(p => p.AddResources(requestedMaterrials), Times.Once);
            player2.Verify(p => p.AddResources(offeredMaterials), Times.Once);

            Assert.Equal(TradeState.Processed, trade.State);
        }


        [Fact]
        public void CantCancelAfterAccepted()
        {
            var trade = CreateValid();
            var player2 = new Mock<IPlayer>();
            player2.Setup(p => p.HasResources(It.IsAny<MaterialType[]>())).Returns(true);
            trade.Accept(player2.Object);

            Assert.Throws<InvalidOperationException>(() => trade.Cancel());
        }

        [Fact]
        public void CantCancelTwice()
        {
            var trade = CreateValid();
            trade.Cancel();
            Assert.Throws<InvalidOperationException>(() => trade.Cancel());
        }

        [Fact]
        public void CancelTest()
        {
            var trade = CreateValid();
            trade.Cancel();

            Assert.Equal(TradeState.Canceled, trade.State);
        }
    }
}
