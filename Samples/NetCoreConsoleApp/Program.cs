using NetCoreConsoleApp;
using System;
using TranslatorService;

//// Initializes the speech client.
//using var speechClient = new SpeechClient(ServiceKeys.SpeechSubscriptionKey, ServiceKeys.SpeechRegion);

//try
//{
//    Console.WriteLine("Calling Speech Service for speech-to-text (using a sample file \"What's the weather like?\")...\n");

//    using var fileStream = File.OpenRead(@"SpeechSample.wav");
//    var recognitionResponse = await speechClient.RecognizeAsync(fileStream, "en-US", RecognitionResultFormat.Detailed);
//    Console.WriteLine($"Recognition Result: {recognitionResponse.RecognitionStatus}");
//    Console.WriteLine(recognitionResponse.DisplayText);

//    var speakResponse = await speechClient.SpeakAsync(new TextToSpeechParameters
//    {
//        Language = "en-US",
//        VoiceType = Gender.Female,
//        VoiceName = "en-US-AriaNeural",
//        Text = "Hello everyone! Today is really a nice day."
//    });

//    // speakResponse contains the stream of the audio.
//}
//catch (TranslatorServiceException ex)
//{
//    Console.WriteLine($"Error: {ex.Message}");
//}

// Initializes the translator client.
using var translatorClient = new TranslatorClient(ServiceKeys.TranslatorSubscriptionKey, ServiceKeys.TranslatorRegion);

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
