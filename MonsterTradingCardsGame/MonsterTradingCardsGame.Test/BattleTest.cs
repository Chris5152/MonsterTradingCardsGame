using MonsterTradingCardsGame.BusinessLogic;
using MonsterTradingCardsGame.Common;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace MonsterTradingCardsGame.Test
{
    public class BattleTest
    {
        private BattleLogic battle;

        private User p1;
        private User p2;

        private Card c1;
        private Card c2;
        private Card c3;
        private Card c4;
        private Card c5;

        [SetUp]
        public void Setup()
        {
            c1 = new Card()
            {
                Id = "1",
                Name = "Goblin",
                Type = ConstantsEnums.CardTypes.Monster,
                Element = ConstantsEnums.Elements.Fire,
                Damage = 10
            };

            c2 = new Card()
            {
                Id = "2",
                Name = "WaterSpell",
                Type = ConstantsEnums.CardTypes.Spell,
                Element = ConstantsEnums.Elements.Water,
                Damage = 15
            };

            c3 = new Card()
            {
                Id = "3",
                Name = "Dragon",
                Type = ConstantsEnums.CardTypes.Monster,
                Element = ConstantsEnums.Elements.Normal,
                Damage = 20
            };

            c4 = new Card
            {
                Id = "4",
                Name = "Knight",
                Type = ConstantsEnums.CardTypes.Monster,
                Element = ConstantsEnums.Elements.Normal,
                Damage = 13
            };

            c5 = new Card()
            {
                Id = "5",
                Name = "FireSpell",
                Type = ConstantsEnums.CardTypes.Spell,
                Element = ConstantsEnums.Elements.Fire,
                Damage = 17
            };

            p1 = new User()
            {
                Id = 1,
                Username = "PlayerA",
                Password = "1234",
                Coins = 20,
                ELO = 5,
                Wins = 5,
                Defeats = 5,
                AuthToken = "abc",
                UserRole = ConstantsEnums.UserRoles.User,
                Bio = "bio",
                Image = "i",
                Deck = new List<Card>()
                {
                    c1, c2, c3, c4
                },
                Stack = new List<Card>()
                {
                    c1, c2, c3, c4, c5
                }
            };

            p2 = new User()
            {
                Id = 2,
                Username = "PlayerB",
                Password = "1234",
                Coins = 15,
                ELO = 3,
                Wins = 3,
                Defeats = 3,
                AuthToken = "abc",
                UserRole = ConstantsEnums.UserRoles.User,
                Bio = "bio",
                Image = "i",
                Deck = new List<Card>()
                {
                    c2, c3, c4, c5
                },
                Stack = new List<Card>()
                {
                    c2, c3, c4, c5
                }
            };

            battle = new BattleLogic(p1, p2);
        }

        [Test]
        public void TestMoveCardCount() // check count of the decks after card has been transferred
        {
            battle.moveCard(p1.Deck, p2.Deck, c5);

            Assert.IsTrue(p1.Deck.Count > p2.Deck.Count);
        }


        [Test]
        public void TestCheckIfLoseDueSpecialtiesGoblin() // check if goblin dies to dragon due to specialty
        {
            Assert.IsTrue(battle.checkIfLoseDueSpecialties(c1, c3));
        }

        [Test]
        public void TestCheckIfLoseDueSpecialtiesKnight() // check if knight dies to waterspell due to specialty
        {
            Assert.IsTrue(battle.checkIfLoseDueSpecialties(c4, c2));
        }

        [Test]
        public void TestGetDamageIncludingElementsNormal() // check if normal vs fire is half damage
        {
            var damage = battle.getDamageIncludingElements(c4, c5);
            var expectedDamage = (decimal)6.5;

            Assert.IsTrue(damage == expectedDamage);
        }

        [Test]
        public void TestGetDamageIncludingElementsWater() // check if water vs water is same damage
        {
            var damage = battle.getDamageIncludingElements(c2, c2);
            var expectedDamage = c2.Damage;

            Assert.IsTrue(damage == expectedDamage);
        }

        [Test]
        public void TestGetDamageIncludingElementsFire() // check if fire vs normal is doubled damage
        {
            var damage = battle.getDamageIncludingElements(c5, c4);
            var expectedDamage = (decimal)34;

            Assert.IsTrue(damage == expectedDamage);
        }

        [Test]
        public void TestMoveCard() // check if card has been moved from deck to deck
        {
            battle.moveCard(p2.Deck, p1.Deck, c1);

            Assert.IsTrue(p2.Deck.ElementAt(4) == c1);
        }
    }
}