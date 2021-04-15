using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Net50ConsoleApplication;
using NetCoreConsoleApp;
using TranslatorService.DependencyInjection;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(ConfigureServices)
    .Build();

var application = host.Services.GetService<Application>();
await application.RunAsync();

void ConfigureServices(HostBuilderContext hostingContext, IServiceCollection services)
{
    services.AddTranslatorClient(options =>
    {
        options.SubscriptionKey = ServiceKeys.TranslatorSubscriptionKey;
        options.Region = ServiceKeys.TranslatorRegion;
    });

    services.AddSingleton<Application>();
}