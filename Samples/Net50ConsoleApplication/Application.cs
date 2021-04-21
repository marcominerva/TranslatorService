using System;
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
            do
            {
                try
                {
                    Console.Write("\nWrite the sentence you want to translate: ");
                    var sentence = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(sentence))
                    {
                        break;
                    }

                    Console.Write("Specify the target language: ");
                    var language = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(language))
                    {
                        break;
                    }

                    Console.WriteLine("Translating...\n");

                    var response = await translatorClient.TranslateAsync(sentence, to: language);

                    Console.WriteLine($"Detected source language: {response.DetectedLanguage.Language} ({response.DetectedLanguage.Score:P2})");
                    Console.WriteLine(response.Translation.Text);
                }
                catch (TranslatorServiceException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

            } while (true);
        }
    }
}
