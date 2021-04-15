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

            services.AddHttpClient<ITokenProvider, DefaultTokenProvider>();
            services.AddHttpClient<ITranslatorClient, TranslatorClient>();

            return services;
        }
    }
}
