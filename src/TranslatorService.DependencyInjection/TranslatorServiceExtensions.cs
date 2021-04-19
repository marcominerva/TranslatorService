using Microsoft.Extensions.DependencyInjection;
using System;
using TranslatorService.Settings;

namespace TranslatorService.DependencyInjection
{
    public static class TranslatorServiceExtensions
    {
        public static IServiceCollection AddTranslatorClient(this IServiceCollection services, Action<TranslatorSettings>? configuration)
        {
            var settings = new TranslatorSettings();
            configuration?.Invoke(settings);

            services.AddSingleton(settings);

            /*
             * Classic (explicit) objects registrations
             * If needed, we can change the lifetime of the registrations
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
             * Registrations with AddHttpClient and ActivatorUtilities (more concise approach).
             * Remember that AddHttpClient registers the implementations as transient
             */

            services.AddHttpClient<ITokenProvider, DefaultTokenProvider>((_, provider) =>
            {
                return ActivatorUtilities.GetServiceOrCreateInstance<DefaultTokenProvider>(provider);
            });

            services.AddHttpClient<ITranslatorClient, TranslatorClient>((httpClient, provider) =>
            {
                var tokenProvider = provider.GetRequiredService<ITokenProvider>();
                return new TranslatorClient(httpClient, settings, tokenProvider);
            });

            return services;
        }
    }
}
