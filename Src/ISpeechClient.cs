using System;
using System.IO;
using System.Threading.Tasks;
using TranslatorService.Models.Speech;

namespace TranslatorService
{
    /// <summary>
    /// The <strong>ISpeechClient</strong> interface specifies properties and methods for text-to-speech and speech-to-text
    /// </summary>
    public interface ISpeechClient : IDisposable
    {
        /// <summary>
        /// Gets or sets the Authentication URI for the Speech service.
        /// </summary>
        string AuthenticationUri { get; set; }

        /// <summary>
        /// Gets or sets the request URI for the Text-to-speech service.
        /// </summary>
        string TextToSpeechRequestUri { get; set; }

        /// <summary>
        /// Gets or sets the request URI for the Speech-to-text service.
        /// </summary>
        string SpeechToTextRequestUri { get; set; }

        /// <summary>
        /// Gets or sets the Subscription key that is necessary to use <strong>Microsoft Translator Service</strong>.
        /// </summary>
        /// <value>The Subscription Key.</value>
        /// <remarks>
        /// <para>You must register Speech Service on https://portal.azure.com/#create/Microsoft.CognitiveServicesSpeechServices to obtain the Speech Uri, Authentication Uri and Subscription key needed to use the service.</para>
        /// </remarks>
        string SubscriptionKey { get; set; }

        /// <summary>
        /// Initializes the <see cref="SpeechClient"/> class by getting an access token for the service.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents the initialize operation.</returns>
        /// <exception cref="ArgumentNullException">The <see cref="SubscriptionKey"/> property hasn't been set.</exception>
        /// <exception cref="ServiceException">The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</exception>
        /// <remarks>Calling this method isn't mandatory, because the token is get/refreshed everytime is needed. However, it is called at startup, it can speed-up subsequest requests.</remarks>
        Task InitializeAsync();

        /// <summary>
        /// Initializes the <see cref="SpeechClient"/> class by getting an access token for the service.
        /// </summary>
        /// <param name="region">The Azure region of the the Speech service. This value is used to automatically set the <see cref="AuthenticationUri"/>, <see cref="TextToSpeechRequestUri"/> and <see cref="SpeechToTextRequestUri"/> properties.</param>
        /// <param name="subscriptionKey">The subscription key for the Microsoft Translator Service on Azure.</param>
        /// <returns>A <see cref="Task"/> that represents the initialize operation.</returns>
        /// <exception cref="ArgumentNullException">The <see cref="SubscriptionKey"/> property hasn't been set.</exception>
        /// <exception cref="ServiceException">The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</exception>
        /// <remarks>
        /// <para>Calling this method isn't mandatory, because the token is get/refreshed everytime is needed. However, it is called at startup, it can speed-up subsequest requests.</para>
        /// <para>You must register Speech Service on https://portal.azure.com/#create/Microsoft.CognitiveServicesSpeechServices to obtain the Speech Uri, Authentication Uri and Subscription key needed to use the service.</para>
        /// </remarks>
        Task InitializeAsync(string region, string subscriptionKey);

        /// <summary>
        /// Sends the specified text to be spoken to the TTS service and saves the response audio to a file.
        /// </summary>
        /// <param name="input">A class holding information to generate the audio file.</param>
        /// <returns>The stream containing the audio of the input text</returns>
        /// <exception cref="ArgumentNullException">
        /// <list type="bullet">
        /// <term>The <see cref="SubscriptionKey"/> property hasn't been set.</term>
        /// <term>The <see cref="AuthenticationUri"/> property hasn't been set.</term>
        /// <term>The <see cref="TextToSpeechRequestUri"/> property hasn't been set.</term>
        /// <term>The <paramref name="input"/> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic).</term>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentException">The text is longer than 800 characters.</exception>
        /// <exception cref="ServiceException">
        /// <list type="bullet">
        /// <term>The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</term>
        /// <term>The call to the method has encountered an unexpected error.</term>
        /// </list>
        /// </exception>
        /// <remarks><para>This method performs a non-blocking request for language detection.</para>
        /// <para>For more information, go to https://docs.microsoft.com/azure/cognitive-services/speech-service/rest-apis#text-to-speech.
        /// </para></remarks>
        /// <seealso cref="TextToSpeechParameters"/>
        Task<Stream> SpeakAsync(TextToSpeechParameters input);

        /// <summary>
        /// Recognize text in a speech audio file.
        /// </summary>
        /// <param name="audioStream">The stream containing the audio to be recognized.</param>
        /// <param name="language">The language of the audio.</param>
        /// <param name="recognitionFormat">The result format.</param>
        /// <param name="profanity">The profanity in recognition results.</param>
        /// <returns>A <see cref="SpeechRecognitionResponse"/> object containing information about the recognized text.</returns>
        /// <exception cref="ArgumentNullException">
        /// <list type="bullet">
        /// <term>The <see cref="SubscriptionKey"/> property hasn't been set.</term>
        /// <term>The <see cref="AuthenticationUri"/> property hasn't been set.</term>
        /// <term>The <see cref="TextToSpeechRequestUri"/> property hasn't been set.</term>
        /// <term>The <paramref name="audioStream"/> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic).</term>
        /// <term>The <paramref name="language"/> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic) or empty.</term>
        /// </list>
        /// </exception>
        /// <exception cref="ServiceException">
        /// <list type="bullet">
        /// <term>The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</term>
        /// <term>The call to the method has encountered an unexpected error.</term>
        /// </list>
        /// </exception>
        /// <remarks>
        /// <para>The currently supported format for recognition is single-channel (mono) WAV / PCM with a sampling rate of 16 kHz.</para>
        /// <para>For more information, go to https://docs.microsoft.com/azure/cognitive-services/speech-service/rest-apis#speech-to-text
        /// </para>
        /// </remarks>
        /// <seealso cref="SpeechRecognitionResponse"/>
        /// <seealso cref="RecognitionResultFormat"/>
        /// <seealso cref=" SpeechProfanityMode"/>
        Task<SpeechRecognitionResponse> RecognizeAsync(Stream audioStream, string language, RecognitionResultFormat recognitionFormat = RecognitionResultFormat.Simple, SpeechProfanityMode profanity = SpeechProfanityMode.Masked);
    }
}