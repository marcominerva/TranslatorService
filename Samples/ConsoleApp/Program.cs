using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        private const string clientId = "";
        private const string clientSecret = "";

        static void Main(string[] args)
        {
            // Initializes the translator service.
            var translatorService = new BingTranslation.TranslatorService(clientId, clientSecret);

            do
            {
                Console.Write("\nWrite the sentect you want to translate: ");
                var sentence = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(sentence))
                    break;

                Console.Write("Specify the target language: ");
                var language = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(language))
                    break;

                Console.WriteLine("Translating...");

                var translatedText = translatorService.TranslateAsync(sentence, language).Result;
                Console.WriteLine(translatedText);

            } while (true);
        }
    }
}
