using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Common.Domain
{
    public class Trade
    {
        public string Id { get; set; }
        public int UserIdTradeCreator { get; set; }
        public string CardToTrade { get; set; }
        public string Type { get; set; }
        public decimal? MinimumDamage { get; set; }
    }
}
