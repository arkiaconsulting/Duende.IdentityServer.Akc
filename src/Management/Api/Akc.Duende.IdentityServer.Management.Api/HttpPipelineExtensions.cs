﻿// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Akc.Duende.IdentityServer.Management.Api
{
    public static class HttpPipelineExtensions
    {
        public static IdentityServerClientApiEndpointBuilder UseIdentityServerClientApi(this IEndpointRouteBuilder app)
        {
            var options = app.ServiceProvider.GetRequiredService<IOptions<ManagementApiOptions>>().Value;
            var builders = new List<RouteHandlerBuilder>
            {
                app.MapGet(options.BasePath, ClientMiddleware.GetAll),
                app.MapGet($"{options.BasePath}/{{clientId}}", ClientMiddleware.Get),
                app.MapPut($"{options.BasePath}/{{clientId}}", ClientMiddleware.Create),
                app.MapPost($"{options.BasePath}/{{clientId}}", ClientMiddleware.Update),
                app.MapDelete($"{options.BasePath}/{{clientId}}", ClientMiddleware.Delete),

                app.MapPut($"{options.BasePath}/{{clientId}}/secrets", ClientMiddleware.AddSecret),
                app.MapPost($"{options.BasePath}/{{clientId}}/secrets", ClientMiddleware.UpdateSecret),
                app.MapDelete($"{options.BasePath}/{{clientId}}/secrets/{{id}}", ClientMiddleware.DeleteSecret)
            };

            return new(app, builders);
        }

        public static IdentityServerClientApiEndpointBuilder RequireAuthorization(
            this IdentityServerClientApiEndpointBuilder builder,
            string policy)
        {
            builder.RouteHandlerBuilders.ToList().ForEach(builder => builder.RequireAuthorization(policy));

            return builder;
        }

        public class IdentityServerClientApiEndpointBuilder
        {
            public IEndpointRouteBuilder App { get; }
            public IEnumerable<RouteHandlerBuilder> RouteHandlerBuilders { get; } = new List<RouteHandlerBuilder>();

            public IdentityServerClientApiEndpointBuilder(
                IEndpointRouteBuilder app,
                IEnumerable<RouteHandlerBuilder> routeHandlerBuilders)
            {
                App = app;
                RouteHandlerBuilders = routeHandlerBuilders;
            }
        }
    }
}