using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static MonsterTradingCardsGame.Common.ConstantsEnums;

namespace MonsterTradingCardsGame.Common
{
    public class Response
    {
        public string Version { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public string HttpStatus { get; set; }
        public string ServerName { get; set; }
        public string Content { get; set; }

        public void Send(StreamWriter writer)
        {
            HttpStatus = HTTPSTATUSCODE_MAPPING[HttpStatusCode];

            writer.WriteLine($"{Version} {HttpStatusCode.ToString()} {HttpStatus}");
            writer.WriteLine($"Server: {ServerName}");
            writer.WriteLine($"Current Time: {DateTime.Now}");
            writer.WriteLine($"Content-Length: {Content.Length}");
            writer.WriteLine("Content-Type: text/html; charset=utf-8");
            writer.WriteLine("");
            writer.WriteLine(Content);
        }
    }
}
