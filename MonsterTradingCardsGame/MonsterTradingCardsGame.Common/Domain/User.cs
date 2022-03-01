using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MonsterTradingCardsGame.Common.ConstantsEnums;

namespace MonsterTradingCardsGame.Common
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Coins { get; set; }
        public int ELO { get; set; }
        public int Wins { get; set; }
        public int Defeats { get; set; }
        public int PlayedGames { get; set; }
        public string AuthToken { get; set; }
        public UserRoles UserRole { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }
        public ICollection<Card> Stack { get; set; }
        public ICollection<Card> Deck { get; set; }
    }
}
