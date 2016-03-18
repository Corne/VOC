using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Core.Items.Cards
{
    public class DevelopmentCardDeck
    {
        //CvB Todo: not really sure about using type here, would be nice to have instance of DevelopmentCard???
        private readonly Stack<DevelopmentCardType> cards;
        private static readonly Dictionary<DevelopmentCardType, int> cardcounts =
            new Dictionary<DevelopmentCardType, int>() {
                { DevelopmentCardType.Knight, 14 },
                { DevelopmentCardType.VictoryPoint, 5 },
                { DevelopmentCardType.Monopoly, 2 },
                { DevelopmentCardType.RoadBuilding, 2 },
                { DevelopmentCardType.YearOfPlenty, 2 },
            };

        public DevelopmentCardDeck()
        {
            IEnumerable<DevelopmentCardType> cards = cardcounts
                .SelectMany(c => Enumerable.Range(0, c.Value).Select(i => c.Key))
                .OrderBy(c => Guid.NewGuid());
            this.cards = new Stack<DevelopmentCardType>(cards);
        }

        public bool IsEmpty
        {
            get { return !cards.Any(); }
        }

        public DevelopmentCardType Pop()
        {
            if (IsEmpty)
                throw new InvalidOperationException("No cards left");
            return cards.Pop();
        }
    }
}
