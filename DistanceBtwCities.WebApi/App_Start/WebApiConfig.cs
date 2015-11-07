﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DistanceBtwCities.WebApi.Handlers;

namespace DistanceBtwCities.WebApi
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

            // добавляем общие обработчики сообщений для всех HttpRoute
            config.MessageHandlers.Add(new MessageHandlerForCaching());
        }
    }
}
