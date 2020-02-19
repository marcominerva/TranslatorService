# Translator Service
A lightweight library that uses Cognitive Translator Service for text translation and Cognitive Speech Service for text-to-speech and speech-to-text.

To use this library, you must register an [All-In-One Cognitive Service](https://portal.azure.com/#create/Microsoft.CognitiveServicesAllInOne) or [Translator Service](https://portal.azure.com/#create/Microsoft.CognitiveServicesTextTranslation) and [Speech Service](https://portal.azure.com/#create/Microsoft.CognitiveServicesSpeechServices) on Azure to obtain the Subscription keys. Keep in mind that Regional endpoints are available, so you need to pass the region name to the library according to your registration.

**Installation**

The library is available on [NuGet](https://www.nuget.org/packages/TranslatorService/). Just search *TranslatorService* in the **Package Manager GUI** or run the following command in the **Package Manager Console**:    

    Install-Package TranslatorService
    
**Usage**

**Translation**

It's usage is straightforward. For example, if you want to translate text:

    // Use a Global Translator Service
    var translatorClient = new TranslatorService.TranslatorClient("<subscription_key>");

    // Use a Regional Translation Service
    var translatorClient = new TranslatorService.TranslatorClient("<region>", "subscription_key>");
    
    var response = await translatorClient.TranslateAsync("Today is really a beautiful day.", to: "it");
    Console.WriteLine(
        $"Detected source language: {response.DetectedLanguage.Language} ({response.DetectedLanguage.Score:P2})");
    
    Console.WriteLine(response.Translation.Text);

**Speech Recognition**

    var speechClient = new SpeechClient("<region>", "<subscription_key>");

    var response = await speechClient.RecognizeAsync(audioStream, "en-US", RecognitionResultFormat.Detailed);
    Console.WriteLine($"Recognition Result: {response.RecognitionStatus}");
    Console.WriteLine(response.DisplayText);

**Speech Synthesis**

    var speechClient = new SpeechClient("<region>", "<subscription_key>");

    var responseStream = await speechClient.SpeakAsync(new TextToSpeechParameters
        {
            Language = "en-US",
            VoiceType = Gender.Female,
            VoiceName = "Microsoft Server Speech Text to Speech Voice (en-US, ZiraRUS)",
            Text = "Hello everyone! Today is really a beautiful day.",
        });

In the [Samples](https://github.com/marcominerva/TranslatorService/tree/master/Samples) folder are available samples for .NET Core and the Universal Windows Platform.

**Contribute**

The project is continually evolving. We welcome contributions. Feel free to file issues and pull requests on the repo and we'll address them as we can. 
