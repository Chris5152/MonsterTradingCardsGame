using MonsterTradingCardsGame.Common;
using MonsterTradingCardsGame.Common.Domain;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataAccessLayer.Repositories
{
    public class UserRepository
    {
        public bool InsertUser(User user)
        {
            var existingUser = GetUser(user.Username);

            if(existingUser == null)
            {
                string query = $"INSERT INTO \"User\" (Username, Password, Coins, ELO, Wins, Defeats, PlayedGames, AuthToken, UserRole, Bio, Image) VALUES (@Username, @Password, @Coins, @ELO, @Wins, @Defeats, @PlayedGames, @AuthToken, @UserRole, @Bio, @Image)";

                byte[] salt;
                new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

                var pbkdf2 = new Rfc2898DeriveBytes(user.Password, salt, 100000);
                byte[] hash = pbkdf2.GetBytes(20);

                byte[] hashBytes = new byte[36];
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);

                string savedPasswordHash = Convert.ToBase64String(hashBytes);

                using (var con = DBConnection.Connect())
                {
                    using (var cmd = new NpgsqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("Username", user.Username);
                        cmd.Parameters.AddWithValue("Password", savedPasswordHash);
                        cmd.Parameters.AddWithValue("Coins", user.Coins);
                        cmd.Parameters.AddWithValue("ELO", user.ELO);
                        cmd.Parameters.AddWithValue("Wins", user.Wins);
                        cmd.Parameters.AddWithValue("Defeats", user.Defeats);
                        cmd.Parameters.AddWithValue("PlayedGames", user.PlayedGames);
                        cmd.Parameters.AddWithValue("AuthToken", user.AuthToken);
                        cmd.Parameters.AddWithValue("UserRole", user.UserRole);
                        cmd.Parameters.AddWithValue("Bio", user.Bio);
                        cmd.Parameters.AddWithValue("Image", user.Image);

                        return cmd.ExecuteNonQuery() != -1;
                    }
                }
            }

            return false;
        }

        public User GetUser(string username)
        {
            string query = $"SELECT * FROM \"User\" WHERE Username = @Username LIMIT 1";

            using (var con = DBConnection.Connect())
            {
                using (var cmd = new NpgsqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("Username", username);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();

                            var user = new User()
                            {
                                Id = (reader["Id"] as int?).Value,
                                Username = reader["Username"] as string,
                                Password = reader["Password"] as string,
                                Coins = (reader["Coins"] as int?).Value,
                                ELO = (reader["ELO"] as int?).Value,
                                Wins = (reader["Wins"] as int?).Value,
                                Defeats = (reader["Defeats"] as int?).Value,
                                PlayedGames = (reader["PlayedGames"] as int?).Value,
                                AuthToken = reader["AuthToken"] as string
                            };

                            return user;
                        }

                        return null;
                    }
                }
            }
        }

        public bool UpdateUser(User user)
        {
            string query = $"UPDATE \"User\" SET Username = @Username, Coins = @Coins, ELO = @ELO, Wins = @Wins, Defeats = @Defeats, PlayedGames = @PlayedGames, AuthToken = @AuthToken, UserRole = @UserRole, Bio = @Bio, Image = @Image WHERE Id = @Id";

            using (var con = DBConnection.Connect())
            {
                using (var cmd = new NpgsqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("Id", user.Id);
                    cmd.Parameters.AddWithValue("Username", user.Username);
                    cmd.Parameters.AddWithValue("Coins", user.Coins);
                    cmd.Parameters.AddWithValue("ELO", user.ELO);
                    cmd.Parameters.AddWithValue("Wins", user.Wins);
                    cmd.Parameters.AddWithValue("Defeats", user.Defeats);
                    cmd.Parameters.AddWithValue("PlayedGames", user.PlayedGames);
                    cmd.Parameters.AddWithValue("AuthToken", user.AuthToken);
                    cmd.Parameters.AddWithValue("UserRole", user.UserRole);
                    cmd.Parameters.AddWithValue("Bio", user.Bio);
                    cmd.Parameters.AddWithValue("Image", user.Image);

                    return cmd.ExecuteNonQuery() != -1;
                }
            }
        }

        public bool DeleteUser(User user)
        {
            string query = $"DELETE FROM \"User\" WHERE Id = @Id";

            using (var con = DBConnection.Connect())
            {
                using (var cmd = new NpgsqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("Id", user.Id);

                    return cmd.ExecuteNonQuery() != -1;
                }
            }
        }

        public User GetUserByToken(string authToken)
        {
            string query = $"SELECT * FROM \"User\" WHERE AuthToken = @AuthToken LIMIT 1";

            using (var con = DBConnection.Connect())
            {
                using (var cmd = new NpgsqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("AuthToken", authToken);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();

                            var user = new User()
                            {
                                Id = (reader["Id"] as int?).Value,
                                Username = reader["Username"] as string,
                                Password = reader["Password"] as string,
                                Coins = (reader["Coins"] as int?).Value,
                                ELO = (reader["ELO"] as int?).Value,
                                Wins = (reader["Wins"] as int?).Value,
                                Defeats = (reader["Defeats"] as int?).Value,
                                PlayedGames = (reader["PlayedGames"] as int?).Value,
                                AuthToken = reader["AuthToken"] as string
                            };

                            return user;
                        }

                        return null;
                    }
                }
            }
        }

        public ICollection<UserScore> GetAllELO()
        {
            string query = $"SELECT Username, ELO FROM \"User\"";

            using (var con = DBConnection.Connect())
            {
                using (var cmd = new NpgsqlCommand(query, con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        var userScores = new List<UserScore>();

                        while (reader.Read())
                        {
                            var userScore = new UserScore()
                            {
                                Username = reader["Username"] as string,
                                ELO = (reader["ELO"] as int?).Value
                            };
                            userScores.Add(userScore);
                        }

                        return (ICollection<UserScore>)userScores.OrderByDescending(x => x.ELO);
                    }
                }
            }
        }
    }
}
