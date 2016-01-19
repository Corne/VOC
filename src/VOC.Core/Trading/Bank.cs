using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Boards;
using VOC.Core.Items.RawMaterials;
using VOC.Core.Players;

namespace VOC.Core.Trading
{
    public class Bank : IBank
    {
        private static readonly MaterialType[] VALID_RESOURCES = Enum.GetValues(typeof(MaterialType))
            .Cast<MaterialType>()
            .Except(new MaterialType[] { MaterialType.Unsourced, MaterialType.Sea })
            .ToArray();

        private readonly IBoard board;

        public Bank(IBoard board)
        {
            if (board == null)
                throw new ArgumentNullException("Board can't be null");
            this.board = board;
        }

        public void BuyResource(MaterialType request, MaterialType offer, IPlayer player)
        {
            if (request == offer)
                throw new ArgumentException("Can't request same material as you are offering");
            if (!VALID_RESOURCES.Contains(request))
                throw new ArgumentException($"Request ({request}) is not a valid material!");

            //GetInvestment also validates player != null, and offer is valid resource
            MaterialType[] investment = GetInvestmentCost(offer, player);
            if (!player.HasResources(investment))
                throw new InvalidOperationException("Player does not have the resources to buy");

            player.TakeResources(investment);
            player.AddResources(new RawMaterial(request));
        }

        public MaterialType[] GetInvestmentCost(MaterialType offer, IPlayer player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (!VALID_RESOURCES.Contains(offer))
                throw new ArgumentException($"Offer ({offer}) is not a valid material!");

            var harbors = board.GetHarbors(player);
            if (harbors.Any(h => h.Discount == offer))
                return new MaterialType[] { offer, offer };
            if (harbors.Any(h => h.Discount == MaterialType.Unsourced))
                return new MaterialType[] { offer, offer, offer };
            return new MaterialType[] { offer, offer, offer, offer };
        }


    }
}
