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
            Version = "HTTP/1.1";
            ServerName = "MTCG Server";

            WriteLine(writer, $"{Version} {(int)HttpStatusCode} {HttpStatus}");
            WriteLine(writer, $"Server: {ServerName}");
            WriteLine(writer, $"Current Time: {DateTime.Now}");

            if (!String.IsNullOrEmpty(Content))
            {
                WriteLine(writer, $"Content-Length: {Content.Length}");
                WriteLine(writer, "Content-Type: text/html; charset=utf-8");
                WriteLine(writer, "");
                WriteLine(writer, Content);
            }
        }

        private void WriteLine(StreamWriter writer, string s)
        {
            Console.WriteLine(s);
            writer.WriteLine(s);
        }
    }
}
