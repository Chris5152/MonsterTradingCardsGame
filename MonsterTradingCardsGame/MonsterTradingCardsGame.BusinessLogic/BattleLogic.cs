using MonsterTradingCardsGame.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.BusinessLogic
{
    public class BattleLogic
    {
        private User playerOne;
        private User playerTwo;
        private StringBuilder log;
        public BattleLogic(User playerOne, User playerTwo)
        {
            this.playerOne = playerOne;
            this.playerTwo = playerTwo;
            log = new StringBuilder();
        }

        //implement battle logic
    }
}
