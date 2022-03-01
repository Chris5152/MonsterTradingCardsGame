using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MonsterTradingCardsGame.Common.ConstantsEnums;

namespace MonsterTradingCardsGame.Common
{
    public class Request
    {
        public HttpMethod Method { get; set; }
        public string URL { get; set; }
        public string Version { get; set; }
        public string ContentType { get; set; }
        public string Autorization { get; set; }
        public int ContentLength { get; set; }
        public string Content { get; set; }

        public Request(StreamReader reader)
        {
            // read method
            string line = reader.ReadLine();
            if (line == null) return;
            string[] parts = line.Split(" ");

            Method = HTTPMETHOD_MAPPING[parts[0]]; // exit if no supported method
            URL = parts[1];
            Version = parts[2];

            // read header
            while ((line = reader.ReadLine()) != null && line != "")
            {
                var currentHeader = line.Split(": ");

                if (currentHeader[0] == HTTPHEADER_MAPPING[HttpHeader.ContentType])
                {
                    ContentType = currentHeader[1];
                }
                else if (currentHeader[0] == HTTPHEADER_MAPPING[HttpHeader.Authorization])
                {
                    Autorization = currentHeader[1];
                }
                else if (currentHeader[0] == HTTPHEADER_MAPPING[HttpHeader.ContentLength])
                {
                    ContentLength = Convert.ToInt32(currentHeader[1]);
                }
            }

            // read content
            char[] buffer = new char[ContentLength];
            reader.Read(buffer, 0, ContentLength);
            Content = new string(buffer);
        }
    }
}
