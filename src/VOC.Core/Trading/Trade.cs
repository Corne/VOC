using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Items.RawMaterials;
using VOC.Core.Players;

namespace VOC.Core.Trading
{
    public class Trade : ITrade
    {
        private object tradeLock = new object();
        private static readonly MaterialType[] INVALID_MATERIALS = { MaterialType.Sea, MaterialType.Unsourced };

        public Trade(MaterialType[] offer, MaterialType[] request, IPlayer owner)
        {
            if (offer == null)
                throw new ArgumentNullException(nameof(offer));
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (owner == null)
                throw new ArgumentNullException(nameof(owner));

            if (!offer.Any() && !request.Any())
                throw new ArgumentException("You can't create a trade without at least an offer or a request");
            if (offer.Any(m => INVALID_MATERIALS.Contains(m)) || request.Any(m => INVALID_MATERIALS.Contains(m)))
                throw new ArgumentException($"Can't create a trade with invalid material resources ({string.Join(", ", INVALID_MATERIALS)})");
            if (!owner.HasResources(offer))
                throw new InvalidOperationException("Can't offer materials if you don't have them");

            Offer = offer;
            Request = request;
            Owner = owner;
            State = TradeState.Open;
        }

        public IPlayer Owner { get; }
        public MaterialType[] Offer { get; }
        public MaterialType[] Request { get; }
        public TradeState State { get; private set; }

        public void Accept(IPlayer player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (player == Owner)
                throw new ArgumentException("Can't accept a trade if owner == player");
            if (!player.HasResources(Request))
                throw new InvalidOperationException("Player should have the requested resources");
            if (!Owner.HasResources(Offer))
                throw new InvalidOperationException("Owner doesn't have the required resources (anymore)!");

            lock (tradeLock)
            {
                if (State != TradeState.Open)
                    throw new InvalidOperationException("Trade can no longer be accepted, because it's no longer open!");

                IEnumerable<IRawMaterial> requested = player.TakeResources(Request);
                IEnumerable<IRawMaterial> offered = Owner.TakeResources(Offer);

                foreach(var material in requested)
                {
                    Owner.AddResource(material);
                }

                foreach(var material in offered)
                {
                    player.AddResource(material);
                }

                State = TradeState.Processed;
            }
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public void Counter(ITrade trade)
        {
            throw new NotImplementedException();
        }
    }
}
