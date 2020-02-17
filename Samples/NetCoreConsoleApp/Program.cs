using System;
using System.IO;
using System.Threading.Tasks;
using TranslatorService;
using TranslatorService.Models.Speech;

namespace NetCoreConsoleApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Initializes the speech client.
            var speechClient = new SpeechClient(ServiceKeys.SpeechRegion, ServiceKeys.SpeechSubscriptionKey);

            try
            {
                Console.WriteLine("Calling Speech Service for speech-to-text (using a sample file \"What's the weather like?\")...\n");

                using var fileStream = File.OpenRead(@"SpeechSample.wav");
                var recognitionResponse = await speechClient.RecognizeAsync(fileStream, "en-US", RecognitionResultFormat.Detailed);
                Console.WriteLine($"Recognition Result: {recognitionResponse.RecognitionStatus}");
                Console.WriteLine(recognitionResponse.DisplayText);

                var speakResponse = await speechClient.SpeakAsync(new TextToSpeechParameters
                {
                    Language = "en-US",
                    VoiceType = Gender.Female,
                    VoiceName = "Microsoft Server Speech Text to Speech Voice (en-US, ZiraRUS)",
                    Text = "Hello everyone! Today is really a beautiful day.",
                });
            }
            catch (ServiceException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            // Initializes the translator client.
            var translatorClient = new TranslatorClient(ServiceKeys.TranslatorRegion, ServiceKeys.TranslatorSubscriptionKey);

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
                catch (ServiceException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

            } while (true);

            translatorClient.Dispose();
        }
    }
}
