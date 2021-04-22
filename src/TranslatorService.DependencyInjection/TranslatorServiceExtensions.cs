using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TranslatorService.Settings;

namespace TranslatorService.DependencyInjection
{
    public static class TranslatorServiceExtensions
    {
        public static IServiceCollection AddTranslatorClient(this IServiceCollection services, Action<TranslatorSettings>? configuration, bool useCache = true)
        {
            services.Configure(configuration);

            /*
             * Classic (explicit) services registrations
             * If needed, we can change the lifetime of the services
             */

            //services.AddHttpClient();

            //services.AddScoped<ITokenProvider, DefaultTokenProvider>(provider =>
            //{
            //    var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient();
            //    return new DefaultTokenProvider(httpClient, settings);
            //});

            //services.AddScoped<ITranslatorClient, TranslatorClient>(provider =>
            //{
            //    var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient();
            //    var tokenProvider = provider.GetRequiredService<ITokenProvider>();
            //    return new TranslatorClient(httpClient, settings, tokenProvider);
            //});

            /* 
             * Registrations with AddHttpClient (more concise approach).
             * Remember that AddHttpClient registers the implementations as transient
             */

            if (useCache)
            {
                services.AddMemoryCache();

                services.AddHttpClient<ITokenProvider, CacheableTokenProvider>((httpClient, provider) =>
                {
                    var cache = provider.GetRequiredService<IMemoryCache>();
                    var settings = provider.GetRequiredService<IOptions<TranslatorSettings>>().Value;
                    return new CacheableTokenProvider(httpClient, settings, cache);
                });
            }
            else
            {
                services.AddHttpClient<ITokenProvider, DefaultTokenProvider>((httpClient, provider) =>
                {
                    var settings = provider.GetRequiredService<IOptions<TranslatorSettings>>().Value;
                    return new DefaultTokenProvider(httpClient, settings);
                });
            }

            services.AddHttpClient<ITranslatorClient, TranslatorClient>((httpClient, provider) =>
            {
                var tokenProvider = provider.GetRequiredService<ITokenProvider>();
                var settings = provider.GetRequiredService<IOptions<TranslatorSettings>>().Value;
                return new TranslatorClient(httpClient, settings, tokenProvider);
            });

            return services;
        }
    }
}
