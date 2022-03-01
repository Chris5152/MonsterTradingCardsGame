using MonsterTradingCardsGame.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MonsterTradingCardsGame.Common.ConstantsEnums;

namespace MonsterTradingCardsGame.DataAccessLayer.Repositories
{
    public class CardRepository
    {
        public bool InsertCard(Card card)
        {
            var existingCard = GetCard(card.Id);

            if(existingCard == null)
            {
                string query = $"INSERT INTO Card (Name, Type, Element, Damage) VALUES (@Name, @Type, @Element, @Damage)";

                using (var con = DBConnection.Connect())
                {
                    using (var cmd = new NpgsqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("Name", card.Name);
                        cmd.Parameters.AddWithValue("Type", card.Type);
                        cmd.Parameters.AddWithValue("Element", card.Element);
                        cmd.Parameters.AddWithValue("Damage", card.Damage);

                        return cmd.ExecuteNonQuery() != -1;
                    }
                }
            }

            return false;
        }

        public Card GetCard(string id)
        {
            string query = $"SELECT * FROM Card WHERE Id = @Id LIMIT 1";

            using (var con = DBConnection.Connect())
            {
                using (var cmd = new NpgsqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("Id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();

                            var card = new Card()
                            {
                                Id = reader["Id"] as string,
                                Name = reader["Name"] as string,
                                Type = (reader["Type"] as CardTypes?).Value,
                                Element = (reader["Element"] as Elements?).Value,
                                Damage = (reader["Damage"] as int?).Value
                            };

                            return card;
                        }

                        return null;
                    }
                }
            }
        }

        public int CreatePackage()
        {
            string query = $"INSERT INTO Package RETURNING Id";

            using (var con = DBConnection.Connect())
            {
                using (var cmd = new NpgsqlCommand(query, con))
                {
                    return (cmd.ExecuteScalar() as int?).Value;
                }
            }
        }

        public bool AddCardToPackage(Card card, int packageId)
        {
            string query = $"INSERT INTO PackageHasCard (PackageId, CardId) VALUES (@PackageId, @CardId)";

            using (var con = DBConnection.Connect())
            {
                using (var cmd = new NpgsqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("PackageId", packageId);
                    cmd.Parameters.AddWithValue("CardId", card.Id);

                    return cmd.ExecuteNonQuery() != -1;
                }
            }
        }

        public ICollection<int> GetAllPackages()
        {
            string query = $"SELECT Id FROM Packages";

            using (var con = DBConnection.Connect())
            {
                using (var cmd = new NpgsqlCommand(query, con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        var ids = new List<int>();

                        while (reader.Read())
                        {
                            ids.Add((reader["Id"] as int?).Value);
                        }

                        return ids;
                    }
                }
            }
        }

        public ICollection<Card> GetCardsFromPackage(int packageId)
        {
            string query = $"SELECT * FROM PackageHasCard WHERE PackageId = @PackageId";

            using (var con = DBConnection.Connect())
            {
                using (var cmd = new NpgsqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("PackageId", packageId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        var cards = new List<Card>();

                        while (reader.Read())
                        {
                            var card = new Card()
                            {
                                Id = reader["Id"] as string,
                                Name = reader["Name"] as string,
                                Type = (reader["Type"] as CardTypes?).Value,
                                Element = (reader["Element"] as Elements?).Value,
                                Damage = (reader["Damage"] as int?).Value
                            };

                            cards.Add(card);
                        }

                        return cards;
                    }
                }
            }
        }

        public bool AddCardToUser(int userId, string cardId)
        {
            string query = $"INSERT INTO UserHasCard (UserId, CardId) VALUES (@UserId, @CardId)";

            using (var con = DBConnection.Connect())
            {
                using (var cmd = new NpgsqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("UserId", userId);
                    cmd.Parameters.AddWithValue("CardId", cardId);

                    return cmd.ExecuteNonQuery() != -1;
                }
            }
        }

        public bool RemoveCardFromUser(int userId, string cardId)
        {
            string queryGet = $"SELECT * FROM UserHasCard WHERE UserId = @UserId AND CardId = @CardId LIMIT 1";
            string queryRemove = $"DELETE FROM UserHasCard WHERE Id = @Id";
            var userHasCardId = 0;

            using (var con = DBConnection.Connect())
            {
                using (var cmd = new NpgsqlCommand(queryGet, con))
                {
                    cmd.Parameters.AddWithValue("UserId", userId);
                    cmd.Parameters.AddWithValue("CardId", cardId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();

                            var test = (reader["Id"] as int?).Value;
                            userHasCardId = test;
                        }
                    }
                }
                
                if(userHasCardId != 0)
                {
                    using (var cmd = new NpgsqlCommand(queryRemove, con))
                    {
                        cmd.Parameters.AddWithValue("Id", userHasCardId);

                        return cmd.ExecuteNonQuery() != -1;
                    }
                }

                return false;
            }
        }

        public ICollection<Card> GetCardsByUserId(int userId)
        {
            string query = $"SELECT * FROM Card JOIN UserHasCard ON Card.Id = UserHasCard.CardId WHERE UserHasCard.UserId = @UserId";

            using (var con = DBConnection.Connect())
            {
                using (var cmd = new NpgsqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("UserId", userId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        var cards = new List<Card>();

                        while (reader.Read())
                        {
                            var card = new Card()
                            {
                                Id = reader["Id"] as string,
                                Name = reader["Name"] as string,
                                Type = (reader["Type"] as CardTypes?).Value,
                                Element = (reader["Element"] as Elements?).Value,
                                Damage = (reader["Damage"] as int?).Value
                            };

                            cards.Add(card);
                        }

                        return cards;
                    }
                }
            }
        }

        public ICollection<Card> GetDeckByUserId(int userId)
        {
            string query = $"SELECT * FROM Card JOIN UserHasCardInDeck ON Card.Id = UserHasCardInDeck.CardId WHERE UserHasCardInDeck.UserId = @UserId";

            using (var con = DBConnection.Connect())
            {
                using (var cmd = new NpgsqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("UserId", userId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        var cards = new List<Card>();

                        while (reader.Read())
                        {
                            var card = new Card()
                            {
                                Id = reader["Id"] as string,
                                Name = reader["Name"] as string,
                                Type = (reader["Type"] as CardTypes?).Value,
                                Element = (reader["Element"] as Elements?).Value,
                                Damage = (reader["Damage"] as int?).Value
                            };

                            cards.Add(card);
                        }

                        return cards;
                    }
                }
            }
        }

        public bool RemoveDeckByUserId(int userId)
        {
            string query = $"DELETE FROM UserHasCardInDeck WHERE UserId = @UserId";

            using (var con = DBConnection.Connect())
            {
                using (var cmd = new NpgsqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("UserId", userId);

                    return cmd.ExecuteNonQuery() != -1;
                }
            }
        }

        public bool AddCardToDeck(int userId, string cardId)
        {
            string query = $"INSERT INTO UserHasCardInDeck (UserId, CardId) VALUES (@UserId, @CardId)";

            using (var con = DBConnection.Connect())
            {
                using (var cmd = new NpgsqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("UserId", userId);
                    cmd.Parameters.AddWithValue("CardId", cardId);

                    return cmd.ExecuteNonQuery() != -1;
                }
            }
        }
    }
}
