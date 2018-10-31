# Translator Service
A lightweight library that uses Cognitive Translator Service for text translation and Cognitive Speech Service for text-to-speech and spech-to-text

To use this library, you must register [Translator Service](https://portal.azure.com/#create/Microsoft.CognitiveServicesTextTranslation) and [Speech Service](https://portal.azure.com/#create/Microsoft.CognitiveServicesSpeechServices) on Azure to obtain the Subscription keys. Keep in mind that for Speech service, Regional endpoints are available, so you must use a subscription key that corresponds to the endpoint you're using.

**Installation**

The library is available on [NuGet](https://www.nuget.org/packages/TranslatorService/). Just search *TranslatorService* in the **Package Manager GUI** or run the following command in the **Package Manager Console**:    

    Install-Package TranslatorService
    
It's usage is straightforward. For example, if you want to translate text:

    var translatorClient = new TranslatorService.TranslatorClient(ServiceKeys.TranslatorSubscriptionKey);

    var response = await translatorClient.TranslateAsync("Today is really a beauttiful day.", to: "it");
    Console.WriteLine($"Detected source language: {response.DetectedLanguage.Language} ({response.DetectedLanguage.Score:P2})");
    Console.WriteLine(response.Translation.Text);

In the [Samples](https://github.com/marcominerva/TranslatorService/tree/master/Samples) folder are available samples for .NET Core and the Universal Windows Platform.

**Contribute**

The project is continually evolving. We welcome contributions. Feel free to file issues and pull requests on the repo and we'll address them as we can. 
