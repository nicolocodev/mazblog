using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using mazblog.Formatters;
using mazblog.MessageHandlers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace mazblog
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //Web Api Formatters
            config.Formatters.Add(new CategoryFormatter());
            config.Formatters.Add(new AtomFormatter());
            config.Formatters.Add(new ImageFormatter());
            config.Formatters.Add(new AtomImageFormatter());

            //Message Handlres
            config.MessageHandlers.Add(new WlwMessageHandler());

            //JSON Settings
            var jsonSettings = config.Formatters.JsonFormatter.SerializerSettings;
            jsonSettings.Formatting = Formatting.Indented;
            jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            

            //Services
            config.Services.Add(typeof(IExceptionLogger), new AzureTableLogging());
        }
    }
}
