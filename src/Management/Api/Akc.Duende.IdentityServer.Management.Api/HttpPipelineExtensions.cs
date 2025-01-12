﻿// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using static Akc.Duende.IdentityServer.Management.Api.Constants.Paths;

namespace Akc.Duende.IdentityServer.Management.Api
{
    public static class HttpPipelineExtensions
    {
        public static IdentityServerClientApiEndpointBuilder UseIdentityServerManagementApi(this IEndpointRouteBuilder app)
        {
            var options = app.ServiceProvider.GetRequiredService<IOptions<ManagementApiOptions>>().Value;
            var builders = new List<RouteHandlerBuilder>
            {
                app.MapGet($"{options.BasePath}/{SubPaths.Clients}", ClientMiddleware.GetAll),
                app.MapGet($"{options.BasePath}/{SubPaths.Clients}/{{clientId}}", ClientMiddleware.Get),
                app.MapPut($"{options.BasePath}/{SubPaths.Clients}/{{clientId}}", ClientMiddleware.Create),
                app.MapPost($"{options.BasePath}/{SubPaths.Clients}/{{clientId}}", ClientMiddleware.Update),
                app.MapDelete($"{options.BasePath}/{SubPaths.Clients}/{{clientId}}", ClientMiddleware.Delete),

                app.MapGet($"{options.BasePath}/{SubPaths.Clients}/{{clientId}}/{SubPaths.ClientSecrets}/{{name}}", ClientMiddleware.GetSecret),
                app.MapPut($"{options.BasePath}/{SubPaths.Clients}/{{clientId}}/{SubPaths.ClientSecrets}/{{name}}", ClientMiddleware.AddSecret),
                app.MapPost($"{options.BasePath}/{SubPaths.Clients}/{{clientId}}/{SubPaths.ClientSecrets}/{{name}}", ClientMiddleware.UpdateSecret),
                app.MapDelete($"{options.BasePath}/{SubPaths.Clients}/{{clientId}}/{SubPaths.ClientSecrets}/{{name}}", ClientMiddleware.DeleteSecret),

                app.MapGet($"{options.BasePath}/{SubPaths.ApiScopes}/{{name}}", ApiScopeMiddleware.Get),
                app.MapPut($"{options.BasePath}/{SubPaths.ApiScopes}/{{name}}", ApiScopeMiddleware.Create),
                app.MapPost($"{options.BasePath}/{SubPaths.ApiScopes}/{{name}}", ApiScopeMiddleware.Update),
                app.MapDelete($"{options.BasePath}/{SubPaths.ApiScopes}/{{name}}", ApiScopeMiddleware.Delete),

                app.MapPut($"{options.BasePath}/{SubPaths.ApiResources}/{{name}}", ApiResourceMiddleware.Create),
                app.MapGet($"{options.BasePath}/{SubPaths.ApiResources}/{{name}}", ApiResourceMiddleware.Get),
                app.MapPost($"{options.BasePath}/{SubPaths.ApiResources}/{{name}}", ApiResourceMiddleware.Update),
                app.MapDelete($"{options.BasePath}/{SubPaths.ApiResources}/{{name}}", ApiResourceMiddleware.Delete),
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