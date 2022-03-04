using MonsterTradingCardsGame.Common;
using MonsterTradingCardsGame.DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.BusinessLogic
{
    public class CardController
    {
        private readonly CardRepository cardRepository;

        public CardController()
        {
            cardRepository = new CardRepository();
        }

        public bool InsertPackage(ICollection<Card> cards)
        {
            var result = true;
            var packageId = cardRepository.CreatePackage();

            foreach(var card in cards)
            {
                if(cardRepository.GetCard(card.Id) == null)
                {
                    cardRepository.InsertCard(card);
                }

                result = cardRepository.AddCardToPackage(card, packageId) && result;
            }

            return result;
        }

        public bool InsertCard(Card card)
        {
            return cardRepository.InsertCard(card);
        }

        public bool DeleteCard(Card card)
        {
            return cardRepository.DeleteCard(card);
        }

        public Card GetCardById(string cardId)
        {
            return cardRepository.GetCard(cardId);
        }

        public bool BuyPackage(int userId)
        {
            var result = true;
            var packages = cardRepository.GetAllPackages();

            var rnd = new Random();
            var index = rnd.Next(0, packages.Count);

            var packageId = packages.ElementAt(index);

            var cardIds = cardRepository.GetCardsFromPackage(packageId);

            foreach(var cardId in cardIds)
            {
                result = cardRepository.AddCardToUser(userId, cardId) && result;
            }

            return result;
        }

        public ICollection<Card> GetCardsByUserId(int userId)
        {
            return cardRepository.GetCardsByUserId(userId);
        }

        public ICollection<Card> GetDeckByUserId(int userId)
        {
            return cardRepository.GetDeckByUserId(userId);
        }

        public bool UpdateDeckByUserId(int userId, ICollection<Card> cards)
        {
            var result = true;

            result = cardRepository.RemoveDeckByUserId(userId);

            foreach(var card in cards)
            {
                result = cardRepository.AddCardToDeck(userId, card.Id) && result;
            }

            return result;
        }

        public bool AddCardToUser(int userId, string cardId)
        {
            return cardRepository.AddCardToUser(userId, cardId);
        }

        public bool RemoveCardFromUser(int userId, string cardId)
        {
            return cardRepository.RemoveCardFromUser(userId, cardId);
        }
    }
}
