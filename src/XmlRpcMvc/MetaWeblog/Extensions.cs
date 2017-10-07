﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace XmlRpcMvc.MetaWeblog
{
    public static class MetaWeblogExtensions
    {
        /// <summary>
        /// Register services
        /// </summary>
        /// <typeparam name="TXmlRpcService"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMetaWeblog<TXmlRpcService, TMetaWeblogEndpointService>(this IServiceCollection services) where TXmlRpcService : class, IXmlRpcService where TMetaWeblogEndpointService : class, IMetaWeblogEndpointProvider
        {
            // register handlers
            services.AddSingleton<IEnumerable<IXmlRpcHandler>>(new List<IXmlRpcHandler>()
            {
                new XmlRpcOverviewHandler(),
                new XmlRpcHandler(),
                new XmlRpcRsdHandler()
            }
            );

            // XmlRpc services
            services.AddScoped<IXmlRpcService, TXmlRpcService>().AddScoped<TXmlRpcService>();

            // Provider
            services.AddScoped<IMetaWeblogEndpointProvider, TMetaWeblogEndpointService>();

            return services;

        }

        /// <summary>
        /// Use Meta Weblog
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="apiUri"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMetaWeblog(this IApplicationBuilder builder, IOptions<XmlRpcOptions> options)
        {
            return builder.UseMiddleware<MetaWeblogMiddleware>(options);
        }
    }
}
