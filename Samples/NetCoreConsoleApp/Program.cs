using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TranslatorService.Models;

namespace NetCoreConsoleApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Initializes the speech client.
            var speechClient = new TranslatorService.SpeechClient(ServiceKeys.SpeechRegion, ServiceKeys.SpeechSubscriptionKey);

            Console.WriteLine("Calling Speech Service for speech-to-text (using a sample file)...\n");
            using (var fileStream = File.OpenRead(@"SpeechSample.wav"))
            {
                var result = await speechClient.RecognizeAsync(fileStream, "en-US");
                Console.WriteLine($"Recognition Result: {result.RecognitionStatus}");
                Console.WriteLine(result.DisplayText);
            }

            //var result = await speechService.SpeakAsync(new TextToSpeechParameters
            //{
            //    Language = "en-US",
            //    VoiceType = Gender.Female,
            //    VoiceName = "Microsoft Server Speech Text to Speech Voice (en-US, ZiraRUS)",
            //    Text = "Hello everyone! Today is really a beautiful day.",
            //});

            // Initializes the translator client.
            var translatorClient = new TranslatorService.TranslatorClient(ServiceKeys.TranslatorSubscriptionKey);

            do
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

            } while (true);

            translatorClient.Dispose();
        }
    }
}
