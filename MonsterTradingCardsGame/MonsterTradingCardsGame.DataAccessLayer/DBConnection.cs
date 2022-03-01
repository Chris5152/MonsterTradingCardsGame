using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace MonsterTradingCardsGame.DataAccessLayer
{
    public class DBConnection
    {
        public static NpgsqlConnection Connect()
        {
            var con = new NpgsqlConnection(@"Server=localhost;Port=5432;User Id=postgres;Password=1234;Database=MTCG;");
            con.Open();

            return con;
        }

        public static void Disconnect(NpgsqlConnection con)
        {
            con.Close();
        }
    }
}
