using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Common
{
    public class Card
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int CardType { get; set; }
        public int ElementType { get; set; }
        public int Damage { get; set; }
    }
}
