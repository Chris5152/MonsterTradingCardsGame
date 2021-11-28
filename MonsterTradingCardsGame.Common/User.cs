using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Common
{
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Wins { get; set; }
        public int Defeats { get; set; }
        public int PlayedGames { get; set; }
        public ICollection<Card> Stack { get; set; }
        public ICollection<Card> Deck { get; set; }
    }
}
