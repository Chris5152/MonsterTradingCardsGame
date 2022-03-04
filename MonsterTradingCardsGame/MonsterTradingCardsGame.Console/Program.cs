using MonsterTradingCardsGame.BusinessLogic;
using MonsterTradingCardsGame.DataAccessLayer;
using MonsterTradingCardsGame.Server;
using System;
using System.Net;
using System.Net.Sockets;

namespace MonsterTradingCardsGame.Console
{
    class Program
    {
        static void Main()
        {
            var start = new ClientHandler(IPAddress.Parse("127.0.0.1"), 10001);
            start.Start();
        }
    }
}
