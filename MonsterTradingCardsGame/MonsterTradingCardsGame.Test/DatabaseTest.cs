using MonsterTradingCardsGame.BusinessLogic;
using MonsterTradingCardsGame.Common;
using MonsterTradingCardsGame.Common.Domain;
using NUnit.Framework;

namespace MonsterTradingCardsGame.Test
{
    public class DatabaseTest
    {
        private CardController cC;
        private UserController uC;
        private TradeController tC;

        private Card card;
        private User user;
        private Trade trade;

        [SetUp]
        public void Setup()
        {
            cC = new CardController();
            uC = new UserController();
            tC = new TradeController();

            card = new Card()
            {
                Id = "test",
                Name = "test",
                Type = ConstantsEnums.CardTypes.Monster,
                Element = ConstantsEnums.Elements.Fire,
                Damage = 10
            };

            user = new User()
            {
                Id = 10000,
                Username = "test",
                Password = "test",
                Coins = 20,
                ELO = 10,
                Wins = 100,
                Defeats = 10,
                PlayedGames = 120,
                AuthToken = "test",
                UserRole = ConstantsEnums.UserRoles.Admin,
                Bio = "test",
                Image = "test"
            };

            trade = new Trade()
            {

            };
        }

        [Test]
        public void TestInsertUser()
        {
            var result = uC.RegisterUser(user);
            uC.DeleteUser(user);

            Assert.IsTrue(result);
        }

        [Test]
        public void TestGetUser()
        {
            uC.RegisterUser(user);
            var result = uC.GetUser(user.Username, user.AuthToken);
            uC.DeleteUser(user);

            Assert.IsTrue(result == user);
        }

        [Test]
        public void TestGetUserByToken()
        {
            uC.RegisterUser(user);
            var result = uC.GetUserByToken(user.AuthToken);
            uC.DeleteUser(user);

            Assert.IsTrue(result == user);
        }

        [Test]
        public void TestDeleteUser()
        {
            uC.RegisterUser(user);
            var result = uC.DeleteUser(user);

            Assert.IsTrue(result);
        }

        [Test]
        public void TestLoginUser()
        {
            uC.RegisterUser(user);
            var result = uC.LoginUser(user);

            Assert.IsTrue(result);
        }

        [Test]
        public void TestInsertCard()
        {
            var result = cC.InsertCard(card);
            cC.DeleteCard(card);

            Assert.IsTrue(result);
        }

        [Test]
        public void TestDeleteCard()
        {
            cC.InsertCard(card);
            var result = cC.DeleteCard(card);

            Assert.IsTrue(result);
        }
    }
}
