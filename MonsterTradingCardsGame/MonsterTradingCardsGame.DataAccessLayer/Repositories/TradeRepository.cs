using MonsterTradingCardsGame.Common.Domain;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataAccessLayer.Repositories
{
    public class TradeRepository
    {
        public ICollection<Trade> GetAllTrades()
        {
            string query = $"SELECT * FROM \"Trade\"";

            using (var con = DBConnection.Connect())
            {
                using (var cmd = new NpgsqlCommand(query, con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        var trades = new List<Trade>();

                        while (reader.Read())
                        {
                            var trade = new Trade()
                            {
                                Id = reader["Id"] as string,
                                UserIdTradeCreator = (reader["UserIdTradeCreator"] as int?).GetValueOrDefault(),
                                CardToTrade = reader["CardToTrade"] as string,
                                Type = reader["WantedType"] as string,
                                MinimumDamage = (reader["WantedMinimumDamage"] as decimal?).GetValueOrDefault()
                            };
                            trades.Add(trade);
                        }

                        return trades;
                    }
                }
            }
        }

        public Trade GetTrade(string id)
        {
            string query = $"SELECT * FROM \"Trade\" WHERE Id = @Id LIMIT 1";

            using (var con = DBConnection.Connect())
            {
                using (var cmd = new NpgsqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("Id", id ?? "");

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();

                            var trade = new Trade()
                            {
                                Id = reader["Id"] as string,
                                UserIdTradeCreator = (reader["UserIdTradeCreator"] as int?).GetValueOrDefault(),
                                CardToTrade = reader["CardToTrade"] as string,
                                Type = reader["WantedType"] as string,
                                MinimumDamage = (reader["WantedMinimumDamage"] as decimal?).GetValueOrDefault()
                            };

                            return trade;
                        }

                        return null;
                    }
                }
            }
        }

        public bool InsertTrade(Trade trade)
        {
            var existingTrade = GetTrade(trade.Id);

            if(existingTrade == null)
            {
                string query = $"INSERT INTO \"Trade\" (Id, UserIdTradeCreator, CardToTrade, WantedType, WantedMinimumDamage) VALUES (@Id, @UserIdTradeCreator, @CardToTrade, @WantedType, @WantedMinimumDamage)";

                using (var con = DBConnection.Connect())
                {
                    using (var cmd = new NpgsqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("Id", trade.Id ?? "");
                        cmd.Parameters.AddWithValue("UserIdTradeCreator", trade.UserIdTradeCreator);
                        cmd.Parameters.AddWithValue("CardToTrade", trade.CardToTrade ?? "");
                        cmd.Parameters.AddWithValue("WantedType", trade.Type ?? "");
                        cmd.Parameters.AddWithValue("WantedMinimumDamage", trade.MinimumDamage ?? 0);

                        return cmd.ExecuteNonQuery() != -1;
                    }
                }
            }

            return false;
        }

        public bool DeleteTrade(Trade trade)
        {
            string query = $"DELETE FROM \"Trade\" WHERE Id = @Id";

            using (var con = DBConnection.Connect())
            {
                using (var cmd = new NpgsqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("Id", trade.Id ?? "");

                    return cmd.ExecuteNonQuery() != -1;
                }
            }
        }
    }
}
