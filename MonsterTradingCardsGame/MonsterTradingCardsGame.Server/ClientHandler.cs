using MonsterTradingCardsGame.BusinessLogic;
using MonsterTradingCardsGame.Common;
using MonsterTradingCardsGame.Common.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using static MonsterTradingCardsGame.Common.ConstantsEnums;

namespace MonsterTradingCardsGame.Server
{
    public class ClientHandler
    {
        public IPAddress Address { get; set; }
        public int Port { get; set; }
        public User PlayerOne { get; set; }
        public User PlayerTwo { get; set; }

        public ClientHandler(IPAddress address, int port)
        {
            Address = address;
            Port = port;
        }

        public void Start()
        {
            TcpListener listener = new TcpListener(Address, Port);
            listener.Start();

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Task.Run(() =>
                {
                    using (StreamReader reader = new StreamReader(client.GetStream()))
                    using (StreamWriter writer = new StreamWriter(client.GetStream()) { AutoFlush = true })
                    {
                        // handle http request
                        var request = new Request(reader);

                        HandleRequest(request, writer);
                    }
                });
            }
        }

        public bool HandleRequest(Request request, StreamWriter writer)
        {
            var response = new Response();

            var pathParts = request.URL.Split("/");

            if(pathParts.Length <= 1)
            {
                response.HttpStatusCode = HttpStatusCode.BadRequest;
            }
            else if(pathParts[1] == PATHS_MAPPING[Paths.Users])
            {
                if (pathParts.Length == 2 && request.Method == HttpMethod.POST && request.ContentType == CONTENTTYPEJSON)
                {
                    response.HttpStatusCode = (RegisterUser(request.Content).Result == true) ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
                }
                else if(pathParts.Length == 3 && request.Method == HttpMethod.GET)
                {
                    var json = GetUser(pathParts[2], request.Autorization);
                    response.HttpStatusCode = (!string.IsNullOrEmpty(json)) ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
                    response.Content = json;
                }
                else if(pathParts.Length == 3 && request.Method == HttpMethod.PUT && request.ContentType == CONTENTTYPEJSON)
                {
                    response.HttpStatusCode = (UpdateUser(pathParts[2], request.Autorization, request.Content).Result == true) ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
                }
                else
                {
                    response.HttpStatusCode = HttpStatusCode.NotFound;
                }
            }
            else if(pathParts[1] == PATHS_MAPPING[Paths.Sessions])
            {
                if(pathParts.Length == 2 && request.Method == HttpMethod.POST && request.ContentType == CONTENTTYPEJSON)
                {
                    response.HttpStatusCode = (LoginUser(request.Content).Result == true) ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
                }
                else
                {
                    response.HttpStatusCode = HttpStatusCode.NotFound;
                }
            }
            else if (pathParts[1] == PATHS_MAPPING[Paths.Packages])
            {
                if(pathParts.Length == 2 && request.Method == HttpMethod.POST && request.ContentType == CONTENTTYPEJSON)
                {
                    response.HttpStatusCode = (AddPackage(request.Content, request.Autorization) == true) ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
                }
                else
                {
                    response.HttpStatusCode = HttpStatusCode.NotFound;
                }
            }
            else if (pathParts[1] == PATHS_MAPPING[Paths.Transactions])
            {
                if(pathParts.Length == 3 && request.Method == HttpMethod.POST && pathParts[2] == PATHS_MAPPING[Paths.Packages])
                {
                    response.HttpStatusCode = (BuyPackage(request.Autorization).Result == true) ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
                }
                else
                {
                    response.HttpStatusCode = HttpStatusCode.NotFound;
                }
            }
            else if (pathParts[1] == PATHS_MAPPING[Paths.Cards])
            {
                if(pathParts.Length == 2 && request.Method == HttpMethod.GET)
                {
                    var json = GetCardsByUser(request.Autorization);
                    response.HttpStatusCode = (!string.IsNullOrEmpty(json)) ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
                    response.Content = json;
                }
                else
                {
                    response.HttpStatusCode = HttpStatusCode.NotFound;
                }
            }
            else if (pathParts[1] == PATHS_MAPPING[Paths.Deck])
            {
                if(pathParts.Length == 2 && request.Method == HttpMethod.GET)
                {
                    var json = GetDeckByUser(request.Autorization);
                    response.HttpStatusCode = (!string.IsNullOrEmpty(json)) ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
                    response.Content = json;
                }
                else if(pathParts.Length == 2 && request.Method == HttpMethod.PUT && request.ContentType == CONTENTTYPEJSON)
                {
                    response.HttpStatusCode = (UpdateDeck(request.Content, request.Autorization).Result == true) ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
                }
                else
                {
                    response.HttpStatusCode = HttpStatusCode.NotFound;
                }
            }
            else if (pathParts[1] == PATHS_MAPPING[Paths.Stats])
            {
                if(pathParts.Length == 2 && request.Method == HttpMethod.GET)
                {
                    var json = GetUserStats(request.Autorization);
                    response.HttpStatusCode = (!string.IsNullOrEmpty(json)) ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
                    response.Content = json;
                }
                else
                {
                    response.HttpStatusCode = HttpStatusCode.NotFound;
                }
            }
            else if (pathParts[1] == PATHS_MAPPING[Paths.Score])
            {
                if(pathParts.Length == 2 && request.Method == HttpMethod.GET)
                {
                    var json = GetScoreboard(request.Autorization);
                    response.HttpStatusCode = (!string.IsNullOrEmpty(json)) ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
                    response.Content = json;
                }
                else
                {
                    response.HttpStatusCode = HttpStatusCode.NotFound;
                }
            }
            else if (pathParts[1] == PATHS_MAPPING[Paths.Battles])
            {
                if(pathParts.Length == 2 && request.Method == HttpMethod.POST)
                {
                    var json = RegisterForBattle(request.Autorization).Result;
                    response.HttpStatusCode = (!string.IsNullOrEmpty(json)) ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
                    response.Content = json;
                }
                else
                {
                    response.HttpStatusCode = HttpStatusCode.NotFound;
                }
            }
            else if (pathParts[1] == PATHS_MAPPING[Paths.Tradings])
            {
                if (pathParts.Length == 2 && request.Method == HttpMethod.GET)
                {
                    var json = GetAllTrades(request.Autorization);
                    response.HttpStatusCode = (!string.IsNullOrEmpty(json)) ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
                    response.Content = json;
                }
                else if (pathParts.Length == 2 && request.Method == HttpMethod.POST && request.ContentType == CONTENTTYPEJSON)
                {
                    response.HttpStatusCode = (CreateTrade(request.Content, request.Autorization) == true) ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
                }
                else if (pathParts.Length == 3 && request.Method == HttpMethod.DELETE)
                {
                    response.HttpStatusCode = (DeleteTrade(pathParts[2], request.Autorization) == true) ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
                }
                else if (pathParts.Length == 3 && request.Method == HttpMethod.POST && request.ContentType == CONTENTTYPEJSON)
                {
                    response.HttpStatusCode = (AcceptTradingDeal(pathParts[2], request.Content, request.Autorization).Result == true) ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
                }
                else
                {
                    response.HttpStatusCode = HttpStatusCode.NotFound;
                }
            }
            else
            {
                if (pathParts[1].Split('?', '=')[0] == PATHS_MAPPING[Paths.Deck] && pathParts[1].Split('?', '=')[1] == "format" && request.Method == HttpMethod.GET)
                {
                    if (pathParts[1].Split('?', '=')[2] == "plain")
                    {
                        var json = GetDeckByUser(request.Autorization);
                        response.HttpStatusCode = (!string.IsNullOrEmpty(json)) ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
                        var dicts = Newtonsoft.Json.JsonConvert.DeserializeObject<ICollection<Dictionary<string, string>>>(json);
                        
                        foreach(var dict in dicts)
                        {
                            response.Content += "\n" + string.Join(',', dict.Select(r => $"{r.Key}={r.Value}"));
                        }
                    }
                }
                else
                {
                    response.HttpStatusCode = HttpStatusCode.NotFound;
                }
            }

            response.Send(writer);

            return true;
        }

        public async Task<bool> RegisterUser(string json)
        {
            var userController = new UserController();

            var user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(json);

            return await Task.Run(() => userController.RegisterUser(user));
        }

        public string GetUser(string username, string authToken)
        {
            var userController = new UserController();

            var user = userController.GetUser(username, authToken);

            if(user != null)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(user, Newtonsoft.Json.Formatting.Indented);
            }

            return "";
        }

        public async Task<bool> UpdateUser(string username, string authToken, string json)
        {
            var userController = new UserController();

            var user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(json);

            return await Task.Run(() => userController.UpdateUser(username, authToken, user));
        }

        public async Task<bool> LoginUser(string json)
        {
            var userController = new UserController();

            var user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(json);

            return await Task.Run (() => userController.LoginUser(user));
        }

        public bool AddPackage(string json, string authToken)
        {
            if(authToken == ADMINTOKEN)
            {
                var cardController = new CardController();

                var cards = Newtonsoft.Json.JsonConvert.DeserializeObject<ICollection<Card>>(json);

                return cardController.InsertPackage(cards);
            }

            return false;
        }

        public async Task<bool> BuyPackage(string authToken)
        {
            var userController = new UserController();
            var cardController = new CardController();

            var user = userController.GetUserByToken(authToken);

            if(user != null && user.Coins >= DEFAULTPACKAGECOSTS)
            {
                if (cardController.BuyPackage(user.Id))
                {
                    user.Coins -= DEFAULTPACKAGECOSTS;
                    return await Task.Run(() => userController.UpdateUser(user.Username, authToken, user));
                }
            }

            return false;
        }

        public string GetCardsByUser(string authToken)
        {
            var userController = new UserController();
            var cardController = new CardController();

            var user = userController.GetUserByToken(authToken);

            if(user != null) 
            { 
                var cards = cardController.GetCardsByUserId(user.Id);

                return Newtonsoft.Json.JsonConvert.SerializeObject(cards, Newtonsoft.Json.Formatting.Indented);
            }

            return "";
        }

        public string GetDeckByUser(string authToken)
        {
            var userController = new UserController();
            var cardController = new CardController();

            var user = userController.GetUserByToken(authToken);

            if (user != null)
            {
                var cards = cardController.GetDeckByUserId(user.Id);

                return Newtonsoft.Json.JsonConvert.SerializeObject(cards, Newtonsoft.Json.Formatting.Indented);
            }

            return "";
        }

        public async Task<bool> UpdateDeck(string json, string authToken)
        {
            var userController = new UserController();
            var cardController = new CardController();

            var cardIds = Newtonsoft.Json.JsonConvert.DeserializeObject<ICollection<string>>(json);

            if(cardIds.Count == 4) 
            {
                var user = userController.GetUserByToken(authToken);
                var cards = new List<Card>();
                
                foreach(var cardId in cardIds)
                {
                    var card = cardController.GetCardById(cardId);
                    if (card == null) return false;
                    cards.Add(card);
                }

                return await Task.Run(() => cardController.UpdateDeckByUserId(user.Id, cards));
            }

            return false;
        }

        public string GetUserStats(string authToken)
        {
            var userController = new UserController();

            var user = userController.GetUserByToken(authToken);
            var stats = new UserStats(user);

            return Newtonsoft.Json.JsonConvert.SerializeObject(stats, Newtonsoft.Json.Formatting.Indented);
        }

        public string GetScoreboard(string authToken)
        {
            var userController = new UserController();

            if (userController.GetUserByToken(authToken) != null)
            {
                var scores = userController.GetAllELO();

                return Newtonsoft.Json.JsonConvert.SerializeObject(scores, Newtonsoft.Json.Formatting.Indented);
            }

            return "";
        }

        public string GetAllTrades(string authToken)
        {
            var userController = new UserController();
            var tradeController = new TradeController();

            if (userController.GetUserByToken(authToken) != null)
            {
                var trades = tradeController.GetAllTrades();

                return Newtonsoft.Json.JsonConvert.SerializeObject(trades, Newtonsoft.Json.Formatting.Indented);
            }

            return "";
        }

        public bool CreateTrade(string json, string authToken)
        {
            var userController = new UserController();
            var tradeController = new TradeController();
            var cardController = new CardController();

            var newTrade = Newtonsoft.Json.JsonConvert.DeserializeObject<Trade>(json);
            var user = userController.GetUserByToken(authToken);

            if (user != null)
            {
                if (cardController.GetCardById(newTrade.CardToTrade) != null)
                {
                    newTrade.UserIdTradeCreator = user.Id;

                    return tradeController.CreateTrade(newTrade);
                }
            }

            return false;
        }

        public bool DeleteTrade(string tradeId, string authToken)
        {
            var userController = new UserController();
            var tradeController = new TradeController();

            var user = userController.GetUserByToken(authToken);
            var trade = tradeController.GetTradeById(tradeId);

            if (user != null && user.Id == trade.UserIdTradeCreator)
            {
                return tradeController.DeleteTrade(trade);
            }

            return false;
        }

        public async Task<bool> AcceptTradingDeal(string tradeId, string cardId, string authToken)
        {
            var userController = new UserController();
            var cardController = new CardController();
            var tradeController = new TradeController();

            var user = userController.GetUserByToken(authToken);
            var trade = tradeController.GetTradeById(tradeId);

            if (user != null && trade != null)
            {
                var userCards = cardController.GetCardsByUserId(user.Id);

                foreach (var card in userCards)
                {
                    if(card.Id == cardId && CARDTYPE_MAPPING[card.Type].ToLower() == trade.Type && card.Damage >= trade.MinimumDamage && trade.UserIdTradeCreator != user.Id)
                    {
                        return await Task.Run(() => cardController.AddCardToUser(user.Id, trade.CardToTrade) &&
                                cardController.AddCardToUser(trade.UserIdTradeCreator, cardId) &&
                                cardController.RemoveCardFromUser(user.Id, cardId) &&
                                cardController.RemoveCardFromUser(trade.UserIdTradeCreator, trade.CardToTrade) &&
                                tradeController.DeleteTrade(trade));
                    }
                }
            }

            return false;
        }

        public async Task<string> RegisterForBattle(string authToken)
        {
            var userController = new UserController();
            var cardController = new CardController();

            var user = userController.GetUserByToken(authToken);

            if(user != null)
            {
                var deck = cardController.GetDeckByUserId(user.Id);
                var stack = cardController.GetCardsByUserId(user.Id);

                if (PlayerOne == null)
                {
                    await Task.Run(() => PlayerOne = user);
                    await Task.Run(() => PlayerOne.Deck = deck);
                    await Task.Run(() => PlayerOne.Stack = stack);
                    return "Waiting for second player...\n";
                }
                else if(PlayerTwo == null)
                {
                    await Task.Run(() => PlayerTwo = user);
                    await Task.Run(() => PlayerTwo.Deck = deck);
                    await Task.Run(() => PlayerTwo.Stack = stack);
                    var battle = new BattleLogic(PlayerOne, PlayerTwo);
                    PlayerOne = null;
                    PlayerTwo = null;

                    return await Task.Run (() => battle.StartBattle());
                }
            }

            return "";
        }
    }
}
