using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Common.Domain
{
    public class UserStats
    {
        public UserStats(User user)
        {
            Username = user.Username;
            Coins = user.Coins;
            ELO = user.ELO;
            Wins = user.Wins;
            Defeats = user.Defeats;
            PlayedGames = user.PlayedGames;
            WinLoseRatio = Defeats != 0 ? Wins / Defeats : 0;
        }

        public string Username { get; set; }
        public int Coins { get; set; }
        public int ELO { get; set; }
        public int Wins { get; set; }
        public int Defeats { get; set; }
        public int PlayedGames { get; set; }
        public decimal WinLoseRatio { get; set; }
    }
}
