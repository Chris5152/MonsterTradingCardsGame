using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Common
{
    public class ConstantsEnums
    {
        public enum HttpMethod { 
            POST, 
            GET, 
            PUT, 
            DELETE
        };

        public static Dictionary<string, HttpMethod> HTTPMETHOD_MAPPING = new Dictionary<string, HttpMethod>()
        {
            { "POST", HttpMethod.POST },
            { "GET", HttpMethod.GET },
            { "PUT", HttpMethod.PUT },
            { "DELETE", HttpMethod.DELETE }
        };

        public enum HttpHeader
        {
            ContentType,
            Authorization,
            ContentLength
        }

        public enum ContentType
        {
            Json
        }

        public static Dictionary<HttpHeader, string> HTTPHEADER_MAPPING = new Dictionary<HttpHeader, string>()
        {
            { HttpHeader.ContentType, "Content-Type" },
            { HttpHeader.Authorization, "Authorization" },
            { HttpHeader.ContentLength, "Content-Length" }
        };

        public enum Paths
        {
            Users,
            Sessions,
            Packages,
            Transactions,
            Cards,
            Deck,
            Stats,
            Score,
            Battles,
            Tradings
        }

        public static Dictionary<Paths, string> PATHS_MAPPING = new Dictionary<Paths, string>()
        {
            { Paths.Users, "users" },
            { Paths.Sessions, "sessions" },
            { Paths.Packages, "packages" },
            { Paths.Transactions, "transactions" },
            { Paths.Cards, "cards" },
            { Paths.Deck, "deck" },
            { Paths.Stats, "stats" },
            { Paths.Score, "score" },
            { Paths.Battles, "battles" },
            { Paths.Tradings, "tradings" }
        };

        public static Dictionary<HttpStatusCode, string> HTTPSTATUSCODE_MAPPING = new Dictionary<HttpStatusCode, string>()
        {
            { HttpStatusCode.OK, "OK" },
            { HttpStatusCode.Created, "Created" },
            { HttpStatusCode.BadRequest, "Bad Request" },
            { HttpStatusCode.Unauthorized, "Unauthorized" },
            { HttpStatusCode.NotFound, "Not Found" },
            { HttpStatusCode.InternalServerError, "Internal Server Error" }
        };

        public enum CardTypes
        {
            Monster,
            Spell
        }

        public static Dictionary<CardTypes, string> CARDTYPE_MAPPING = new Dictionary<CardTypes, string>()
        {
            { CardTypes.Monster, "Monster" },
            { CardTypes.Spell, "Spell" }
        };

        public enum Elements
        {
            Fire,
            Water,
            Normal
        }

        public static Dictionary<Elements, string> ELEMENT_MAPPING = new Dictionary<Elements, string>()
        {
            { Elements.Fire, "Fire" },
            { Elements.Water, "Water" },
            { Elements.Normal, "Normal" }
        };

        public static string CONTENTTYPEJSON = "application/json";

        public enum UserRoles
        {
            Admin,
            User
        }

        public static string ADMINTOKEN = "Basic admin-mtcgToken";

        public static int STARTCOINS = 20;

        public static int DEFAULTPACKAGECOSTS = 5;
    }
}
