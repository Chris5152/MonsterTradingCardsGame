using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MonsterTradingCardsGame.Common.ConstantsEnums;

namespace MonsterTradingCardsGame.Common
{
    public class Card
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public CardTypes Type { get; set; }
        public Elements Element { get; set; }
        public decimal Damage { get; set; }
    }
}