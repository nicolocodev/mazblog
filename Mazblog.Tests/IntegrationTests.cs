using System.Net.Http;
using System.Threading;
using System.Web.Http;
using mazblog.Controllers.ApiControllers;
using NUnit.Framework;

namespace Mazblog.Tests
{
    [TestFixture]
    public class IntegrationTests
    {
        [Test]
        [Ignore]

        //TODO: End integration tests
        public void ShouldRespondDiscoveryAtomFile()
        {
            var config = new HttpConfiguration();
            var httproute = config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional, }
            );
            httproute.DataTokens["Namespaces"] = new[] {typeof (DiscoveryController).Namespace};
            var server = new HttpServer(config);
            using (var client = new HttpMessageInvoker(server))
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, "http://dev.mazblog.net/api/discovery/"))
                using (HttpResponseMessage response = client.SendAsync(request, CancellationToken.None).Result)
                {
                    //Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                }
            };
        }
    }
}
