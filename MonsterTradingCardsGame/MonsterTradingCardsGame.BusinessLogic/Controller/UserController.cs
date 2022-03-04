using MonsterTradingCardsGame.Common;
using MonsterTradingCardsGame.Common.Domain;
using MonsterTradingCardsGame.DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using static MonsterTradingCardsGame.Common.ConstantsEnums;

namespace MonsterTradingCardsGame.BusinessLogic
{
    public class UserController
    {
        private readonly UserRepository userRepository;

        public UserController()
        {
            userRepository = new UserRepository();
        }

        public bool RegisterUser(User user)
        {
            user.Coins = STARTCOINS;
            user.UserRole = (user.Username == "admin") ? UserRoles.Admin : UserRoles.User;

            return userRepository.InsertUser(user);
        }

        public bool LoginUser(User user)
        {
            // Fetch the stored value
            var savedUser = userRepository.GetUser(user.Username);
            // Extract the bytes
            byte[] hashBytes = Convert.FromBase64String(savedUser.Password);
            // Get the salt
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            // Compute the hash on the password the user entered
            var pbkdf2 = new Rfc2898DeriveBytes(user.Password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            // Compare the results
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i]) return false;
            }

            //update token
            string authToken = String.Format("Basic {0}-mtcgToken", savedUser.Username);
            savedUser.AuthToken = authToken;

            return userRepository.UpdateUser(savedUser);
        }

        public User GetUser(string username, string authToken)
        {
            if (validateUserToken(username, authToken))
            {
                return userRepository.GetUser(username);
            }

            return null;
        }

        public bool UpdateUser(string username, string authToken, User newUser)
        {
            if (validateUserToken(username, authToken))
            {
                var existingUser = userRepository.GetUser(username);

                existingUser.Username = !string.IsNullOrEmpty(newUser.Username) ? newUser.Username : existingUser.Username;
                existingUser.Coins = newUser.Coins;
                existingUser.ELO = newUser.ELO;
                existingUser.Wins = newUser.Wins;
                existingUser.Defeats = newUser.Defeats;
                existingUser.PlayedGames = newUser.PlayedGames;
                existingUser.AuthToken = newUser.AuthToken ?? existingUser.AuthToken;
                existingUser.UserRole = newUser.UserRole;
                existingUser.Bio = !string.IsNullOrEmpty(newUser.Bio) ? newUser.Bio : existingUser.Bio;
                existingUser.Image = !string.IsNullOrEmpty(newUser.Image) ? newUser.Image : existingUser.Image;

                return userRepository.UpdateUser(existingUser);
            }

            return false;
        }

        public User GetUserByToken(string authToken)
        {
            return userRepository.GetUserByToken(authToken);
        }

        public IEnumerable<UserScore> GetAllELO()
        {
            return userRepository.GetAllELO();
        }

        public bool DeleteUser(User user)
        {
            return userRepository.DeleteUser(user);
        }

        private bool validateUserToken(string username, string authToken)
        {
            return userRepository.GetUser(username)?.AuthToken == (authToken ?? "");
        }
    }
}
