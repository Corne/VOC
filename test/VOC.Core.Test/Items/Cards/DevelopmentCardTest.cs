using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Games.Turns;
using VOC.Core.Items.Cards;
using Xunit;

namespace VOC.Core.Test.Items.Cards
{
    public class DevelopmentCardTest
    {

        [Theory]
        [InlineData((DevelopmentCardType)(-1))]
        [InlineData((DevelopmentCardType)5)]
        public void CardCantBeCreatedWithInvalidCardType(DevelopmentCardType type)
        {
            var turn = new Mock<ITurn>();
            Assert.Throws<ArgumentException>(() => new DevelopmentCard(type, turn.Object));
        }

        [Fact]
        public void CardCantBeCreatedWithoutTurn()
        {
            Assert.Throws<ArgumentNullException>(() => new DevelopmentCard(DevelopmentCardType.Monopoly, null));
        }


        [Theory]
        [InlineData(DevelopmentCardType.Knight)]
        [InlineData(DevelopmentCardType.Monopoly)]
        [InlineData(DevelopmentCardType.RoadBuilding)]
        [InlineData(DevelopmentCardType.YearOfPlenty)]
        public void ExpectCardNotToBePlayableIfTurnNotEnded(DevelopmentCardType type)
        {
            var turn = new Mock<ITurn>();
            var card = new DevelopmentCard(type, turn.Object);

            Assert.False(card.Playable);
        }

        [Theory]
        [InlineData(DevelopmentCardType.Knight)]
        [InlineData(DevelopmentCardType.Monopoly)]
        [InlineData(DevelopmentCardType.RoadBuilding)]
        [InlineData(DevelopmentCardType.YearOfPlenty)]
        public void ExpectCardToBePlayableAfterTurnEnded(DevelopmentCardType type)
        {
            var turn = new Mock<ITurn>();
            var card = new DevelopmentCard(type, turn.Object);
            turn.Raise(t => t.Ended += null, EventArgs.Empty);

            Assert.True(card.Playable);
        }

        [Fact]
        public void ExpectVictoryPointAlwasyToBePlayable()
        {
            var turn = new Mock<ITurn>();
            var card = new DevelopmentCard(DevelopmentCardType.VictoryPoint, turn.Object);

            Assert.True(card.Playable);
        }
    }
}
