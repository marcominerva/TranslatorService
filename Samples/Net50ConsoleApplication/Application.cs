using System.Threading.Tasks;
using TranslatorService;

namespace Net50ConsoleApplication
{
    public class Application
    {
        private readonly ITranslatorClient translatorClient;

        public Application(ITranslatorClient translatorClient)
        {
            this.translatorClient = translatorClient;
        }

        public async Task RunAsync()
        {
            var result = await translatorClient.TranslateAsync("Ciao Mondo!", "en");
        }
    }
}
