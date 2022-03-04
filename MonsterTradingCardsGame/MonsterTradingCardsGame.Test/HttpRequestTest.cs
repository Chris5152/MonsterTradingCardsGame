using MonsterTradingCardsGame.Common;
using NUnit.Framework;
using System;
using System.IO;

namespace MonsterTradingCardsGame.Test
{
    public class HttpRequestTest
    {
        private Request request;

        [SetUp]
        public void Setup()
        {
            Stream stream = new MemoryStream();
            StreamWriter sw = new StreamWriter(stream);
            sw.Write("POST /users/test HTTP/1.1\n" + "Host: localhost\n" + $"Authorization: Test\n" + $"Content-Length: {"{This is a Test!}".Length}\n" + "Content-Type: application/json\n" + "\n" + "{Test:This is a Test!}\n");
            sw.Flush();
            stream.Position = 0;

            StreamReader sr = new StreamReader(stream);

            request = new Request(sr);
        }

        [Test]
        public void TestRequestMethod()
        {
            var method = request.Method;
            var expectedMethod = ConstantsEnums.HttpMethod.POST;

            Assert.True(method == expectedMethod);
        }

        [Test]
        public void TestRequestPath()
        {
            var path = request.URL;
            var expectedPath = "/users/test";

            Assert.True(path == expectedPath);
        }

        [Test]
        public void TestRequestVersion()
        {
            var version = request.Version;
            var expectedVersion = "HTTP/1.1";

            Assert.True(version == expectedVersion);
        }

        [Test]
        public void TestRequestContentType()
        {
            var ct = request.ContentType;
            var expectedCt = "application/json";

            Assert.True(ct == expectedCt);
        }

        [Test]
        public void TestRequestAuthorization()
        {
            var auth = request.Autorization;
            var expectedAuth = "Test";

            Assert.True(auth == expectedAuth);
        }

        [Test]
        public void TestRequestContentLength()
        {
            var cl = request.ContentLength;
            var expectedCl = 17;

            Assert.True(cl == expectedCl);
        }

        [Test]
        public void TestRequestContent()
        {
            Assert.True(!string.IsNullOrEmpty(request.Content));
        }
    }
}
