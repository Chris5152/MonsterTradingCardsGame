using MonsterTradingCardsGame.Common.Domain;
using MonsterTradingCardsGame.DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.BusinessLogic
{
    public class TradeController
    {
        private readonly TradeRepository tradeRepository;

        public TradeController()
        {
            tradeRepository = new TradeRepository();
        }

        public ICollection<Trade> GetAllTrades()
        {
            return tradeRepository.GetAllTrades();
        }

        public Trade GetTradeById(string tradeId)
        {
            return tradeRepository.GetTrade(tradeId);
        }

        public bool CreateTrade(Trade trade)
        {
            return tradeRepository.InsertTrade(trade);
        }

        public bool DeleteTrade(Trade trade)
        {
            if(tradeRepository.GetTrade(trade.Id) != null)
            {
                return tradeRepository.DeleteTrade(trade);
            }

            return false;
        }
    }
}
