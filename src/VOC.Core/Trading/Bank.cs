using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using VOC.Core.Boards;
using VOC.Core.Games.Turns;
using VOC.Core.Items.Achievements;
using VOC.Core.Items.Cards;
using VOC.Core.Items.RawMaterials;
using VOC.Core.Players;

namespace VOC.Core.Trading
{
    public class Bank : IBank
    {
        public static readonly MaterialType[] DEVELOPMENTCARD_COST = { MaterialType.Ore, MaterialType.Wool, MaterialType.Grain };
        private static ILog logger = LogManager.GetLogger(nameof(Bank));
        private static readonly MaterialType[] VALID_RESOURCES = Enum.GetValues(typeof(MaterialType))
                .Cast<MaterialType>()
                .Except(new MaterialType[] { MaterialType.Unsourced, MaterialType.Sea })
                .ToArray();

        private readonly IBoard board;
        private readonly IEnumerable<IAchievement> achievements;
        private readonly DevelopmentCardDeck deck = new DevelopmentCardDeck();


        public IEnumerable<IAchievement> ActiveAchievements
        {
            get { return achievements.Where(p => p.Owner != null); }
        }

        public Bank(IBoard board, IEnumerable<IAchievement> achievements)
        {
            if (board == null)
                throw new ArgumentNullException(nameof(board));
            if (achievements == null)
                throw new ArgumentNullException(nameof(achievements));
            this.board = board;
            this.achievements = achievements;
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

            logger.Info($"{player.Name} bought {request} for {offer}({investment.Count()})");
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

        // 25 Development Cards (14 Knight/Soldier Cards, 6 Progress Cards, 5 Victory Point Cards)
        //ore, wool and grain
        public void BuyDevelopmentCard(IPlayer player, ITurn turn)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (turn == null)
                throw new ArgumentNullException(nameof(turn));
            if (!player.HasResources(DEVELOPMENTCARD_COST))
                throw new InvalidOperationException("Player does not have the resources to buy a Development Card");

            player.TakeResources(DEVELOPMENTCARD_COST);
            player.AddCard(new DevelopmentCard(deck.Pop(), turn));
        }

        public void UpdateAchievements(IPlayer player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            foreach (var achievement in achievements)
                achievement.Update(player);
        }

        public bool VerifyWinCondition(IPlayer player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            int cards = player.Cards.Where(c => c.Type == DevelopmentCardType.VictoryPoint).Count();
            int achievementCount = achievements.Where(a => a.Owner == player).Select(a => a.VictoryPoints).Sum();
            int establishmentCount = board.GetEstablishments(player).Select(e => e.VictoryPoints).Sum();

            return cards + achievementCount + establishmentCount >= 10;
        }
    }
}
