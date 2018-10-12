using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using CursoBackend.Util;

namespace CursoBackend
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuración y servicios de API web
            var corsAttr = new EnableCorsAttribute("*", "Accept,Origin,content-type,authtoken", "*");
            config.EnableCors(corsAttr);

            // Rutas de API web
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "ApiWithAction",
            //    routeTemplate: "WebApi/{controller}/{action}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "WebApi/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "ActionsApi",
                routeTemplate: "WebApi/Proceso/{controller}/{action}/{path}",
                defaults: new { path = RouteParameter.Optional }
            );

            // Evitar k__BackingField class [Serializable]
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new DefaultContractResolver { IgnoreSerializableAttribute = true };

            config.Formatters.Add(new FormMultipartEncodedMediaTypeFormatter());
        }
    }
}