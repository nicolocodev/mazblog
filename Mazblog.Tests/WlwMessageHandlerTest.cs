using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using mazblog.MessageHandlers;
using NUnit.Framework;

namespace Mazblog.Tests
{
    public class TestableHandler : DelegatingHandler
    {
        private readonly Func<HttpRequestMessage,
            CancellationToken, Task<HttpResponseMessage>> _handlerFunc;

        public TestableHandler()
        {
            _handlerFunc = (r, c) => Return200();
        }

        public TestableHandler(Func<HttpRequestMessage,
            CancellationToken, Task<HttpResponseMessage>> handlerFunc)
        {
            _handlerFunc = handlerFunc;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return _handlerFunc(request, cancellationToken);
        }

        public static Task<HttpResponseMessage> Return200()
        {
            return Task.Factory.StartNew(
                () => new HttpResponseMessage(HttpStatusCode.OK));
        }
    }

    [TestFixture]
    public class WlwMessageHandlerTest
    {
        [Test]
        public async Task ShouldAddAnAtomAcceptHeaderWhenWlwUserAgentCommentIsPresent()
        {
            var testhandler = new TestableHandler((r, c) => TestableHandler.Return200());
            var handler = new WlwMessageHandler {InnerHandler = testhandler};
            var request = new HttpRequestMessage(HttpMethod.Get, "http://test.net/");
            var wlw = new ProductInfoHeaderValue("(compatible; MSIE 9.11; Windows NT 6.2; Windows Live Writer 1.0)");
            request.Headers.UserAgent.Add(wlw);
            
            var client = new HttpClient(handler);
            await  client.SendAsync(request, new CancellationToken());
            Assert.IsTrue(request.Headers.Accept.Contains(new MediaTypeWithQualityHeaderValue("application/atom+xml")));
        }
    }
}
