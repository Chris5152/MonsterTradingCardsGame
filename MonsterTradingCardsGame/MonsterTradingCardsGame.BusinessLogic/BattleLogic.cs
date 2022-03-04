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

        private ICollection<Card> initialDeckP1;
        private ICollection<Card> initialDeckP2;

        public BattleLogic(User playerOne, User playerTwo)
        {
            this.playerOne = playerOne;
            this.playerTwo = playerTwo;
            log = new StringBuilder();

            initialDeckP1 = playerOne.Deck;
            initialDeckP2 = playerTwo.Deck;
        }

        public string StartBattle()
        {
            var roundCount = 0;
            var rnd = new Random();

            while(playerOne.Deck.Count > 0 && playerTwo.Deck.Count > 0 && roundCount < 100)
            {
                var cardP1 = getRandomCard(playerOne.Deck, rnd);
                var cardP2 = getRandomCard(playerTwo.Deck, rnd);

                if(checkIfLoseDueSpecialties(cardP1, cardP2))
                {
                    moveCard(playerTwo.Deck, playerOne.Deck, cardP1);
                    createSpecialtiesLogMessage(cardP2.Name, cardP1, cardP2);
                }
                else if(checkIfLoseDueSpecialties(cardP2, cardP1))
                {
                    moveCard(playerOne.Deck, playerTwo.Deck, cardP2);
                    createSpecialtiesLogMessage(cardP1.Name, cardP1, cardP2);
                }
                else
                {
                    if(cardP1.Type == ConstantsEnums.CardTypes.Monster && cardP2.Type == ConstantsEnums.CardTypes.Monster)
                    {
                        calculateWinnerNoElements(cardP1, cardP2);
                    }
                    else
                    {
                        calculateWinnerIncludingElements(cardP1, cardP2);
                    }
                }

                ++roundCount;

                checkWinner(roundCount);
                //Console.WriteLine("--- " + playerOne.Deck.Count + " /// " + playerTwo.Deck.Count + " ---");
            }

            return log.ToString();
        }

        public Card getRandomCard(ICollection<Card> deck, Random rnd)
        {
            var index = rnd.Next(deck.Count);

            return deck.ElementAt(index);
        }

        public void moveCard(ICollection<Card> deckGetCard, ICollection<Card> deckRemoveCard, Card card)
        {
            deckGetCard.Add(card);
            deckRemoveCard.Remove(card);
        }

        public bool checkIfLoseDueSpecialties(Card cardToCheck, Card enemyCard)
        {
            var cardName = cardToCheck.Name.ToLower();
            var enemyCardName = enemyCard.Name.ToLower();

            if(cardName.Contains("goblin") && enemyCardName.Contains("dragon"))
            {
                return true;
            }
            else if(cardName.Contains("ork") && enemyCardName.Contains("wizzard"))
            {
                return true;
            }
            else if (cardName.Contains("knight") && enemyCardName.Contains("waterspell"))
            {
                return true;
            }
            else if (cardName.Contains("spell") && enemyCardName.Contains("kraken"))
            {
                return true;
            }
            else if (cardName.Contains("dragon") && enemyCardName.Contains("fireelve"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void createSpecialtiesLogMessage(string cardWon, Card cardP1, Card cardP2)
        {
            log.Append(String.Format("{0}: {1} ({2} Damage) vs {3}: {4} ({5} Damage) => {6} wins due to specialty\n", playerOne.Username, cardP1.Name, cardP1.Damage, playerTwo.Username, cardP2.Name, cardP2.Damage, cardWon));
        }

        public void checkWinner(int roundCount)
        {
            if(playerOne.Deck.Count == 0)
            {
                log.Append(String.Format("{0} is the winner!\n", playerTwo.Username));
                updateStackOfWinner(playerTwo);
                updateStackOfLoser(playerOne, initialDeckP1);
                updateStatsWinner(playerTwo);
            }
            else if(playerTwo.Deck.Count == 0)
            {
                log.Append(String.Format("{0} is the winner!\n", playerOne.Username));
                updateStackOfWinner(playerOne);
                updateStackOfLoser(playerTwo, initialDeckP2);
                updateStatsLoser(playerOne);
            }
            else if(roundCount >= 100)
            {
                log.Append("It is a draw!\n");
            }
        }

        public void calculateWinnerIncludingElements(Card cardP1, Card cardP2)
        {
            var damageP1 = getDamageIncludingElements(cardP1, cardP2);
            var damageP2 = getDamageIncludingElements(cardP2, cardP1);

            if (damageP1 > damageP2)
            {
                moveCard(playerOne.Deck, playerTwo.Deck, cardP2);
                log.Append(String.Format("{0}: {1} ({2} Damage) vs {3}: {4} ({5} Damage) => {6} vs {7} -> {8} vs {9} => {10} wins\n", playerOne.Username, cardP1.Name, cardP1.Damage, playerTwo.Username, cardP2.Name, cardP2.Damage, cardP1.Damage, cardP2.Damage, damageP1, damageP2, cardP1.Name));
            }
            else if (damageP1 < damageP2)
            {
                moveCard(playerTwo.Deck, playerOne.Deck, cardP1);
                log.Append(String.Format("{0}: {1} ({2} Damage) vs {3}: {4} ({5} Damage) => {6} vs {7} -> {8} vs {9} => {10} wins\n", playerOne.Username, cardP1.Name, cardP1.Damage, playerTwo.Username, cardP2.Name, cardP2.Damage, cardP1.Damage, cardP2.Damage, damageP1, damageP2, cardP2.Name));
            }
            else
            {
                log.Append(String.Format("{0}: {1} ({2} Damage) vs {3}: {4} ({5} Damage) => {6} vs {7} -> {8} vs {9} => Draw\n", playerOne.Username, cardP1.Name, cardP1.Damage, playerTwo.Username, cardP2.Name, cardP2.Damage, cardP1.Damage, cardP2.Damage, damageP1, damageP2));
            }
        }

        public decimal getDamageIncludingElements(Card cardPlayer, Card cardEnemy)
        {
            switch (cardPlayer.Element)
            {
                case ConstantsEnums.Elements.Fire:
                    return (cardEnemy.Element == ConstantsEnums.Elements.Water) ? cardPlayer.Damage / 2 : (cardEnemy.Element == ConstantsEnums.Elements.Normal) ? cardPlayer.Damage * 2 : cardPlayer.Damage;
                case ConstantsEnums.Elements.Water:
                    return (cardEnemy.Element == ConstantsEnums.Elements.Normal) ? cardPlayer.Damage / 2 : (cardEnemy.Element == ConstantsEnums.Elements.Fire) ? cardPlayer.Damage * 2 : cardPlayer.Damage;
                case ConstantsEnums.Elements.Normal:
                    return (cardEnemy.Element == ConstantsEnums.Elements.Fire) ? cardPlayer.Damage / 2 : (cardEnemy.Element == ConstantsEnums.Elements.Water) ? cardPlayer.Damage * 2 : cardPlayer.Damage;
                default:
                    return cardPlayer.Damage;
            }
        }

        public void calculateWinnerNoElements(Card cardP1, Card cardP2) 
        {
            if(cardP1.Damage > cardP2.Damage)
            {
                moveCard(playerOne.Deck, playerTwo.Deck, cardP2);
                log.Append(String.Format("{0}: {1} ({2} Damage) vs {3}: {4} ({5} Damage) => {6} defeats {7}\n", playerOne.Username, cardP1.Name, cardP1.Damage, playerTwo.Username, cardP2.Name, cardP2.Damage, cardP1.Name, cardP2.Name));
            }
            else if(cardP1.Damage < cardP2.Damage)
            {
                moveCard(playerTwo.Deck, playerOne.Deck, cardP1);
                log.Append(String.Format("{0}: {1} ({2} Damage) vs {3}: {4} ({5} Damage) => {6} defeats {7}\n", playerOne.Username, cardP1.Name, cardP1.Damage, playerTwo.Username, cardP2.Name, cardP2.Damage, cardP2.Name, cardP1.Name));
            }
            else
            {
                log.Append(String.Format("{0}: {1} ({2} Damage) vs {3}: {4} ({5} Damage) => Draw\n", playerOne.Username, cardP1.Name, cardP1.Damage, playerTwo.Username, cardP2.Name, cardP2.Damage));
            }
        }

        public void updateStackOfWinner(User user)
        {
            var cardController = new CardController();

            foreach(var card in user.Deck)
            {
                if(user.Stack.Any(x => x.Id == card.Id))
                {
                    user.Deck.Remove(card);
                }
            }

            foreach(var card in user.Deck)
            {
                cardController.AddCardToUser(user.Id, card.Id);
            }
        }

        public void updateStackOfLoser(User user, ICollection<Card> cardsToBeRemoved) 
        {
            var cardController = new CardController();

            foreach(var card in cardsToBeRemoved)
            {
                cardController.RemoveCardFromUser(user.Id, card.Id);
            }
        }

        public void updateStatsWinner(User user)
        {
            var userController = new UserController();

            ++user.ELO;
            ++user.Wins;
            ++user.PlayedGames;

            userController.UpdateUser(user.Username, user.AuthToken, user);
        }

        public void updateStatsLoser(User user)
        {
            var userController = new UserController();

            user.ELO = user.ELO - 1 < 0 ? 0 : --user.ELO;
            ++user.Defeats;
            ++user.PlayedGames;

            userController.UpdateUser(user.Username, user.AuthToken, user);
        }
    }
}
