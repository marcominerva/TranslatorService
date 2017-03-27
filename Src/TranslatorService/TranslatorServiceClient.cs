using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml.Linq;
using TranslatorService.Models;
using System.IO;

namespace TranslatorService
{
    /// <summary>
    /// The <strong>TranslatorServiceClient</strong> class provides methods to translate text to various supported languages.
    /// </summary>
    /// <remarks>
    /// <para>To use this library, you must register Microsoft Translator on https://portal.azure.com/#create/Microsoft.CognitiveServices/apitype/TextTranslation to obtain the Subscription key.
    /// </para>
    /// </remarks>
    public class TranslatorServiceClient : ITranslatorServiceClient
    {
        private const string BaseUrl = "http://api.microsofttranslator.com/v2/Http.svc/";
        private const string TranslateLanguagesUri = "GetLanguagesForTranslate";
        private const string TranslateUri = "Translate?text={0}&to={1}&contentType=text/plain";
        private const string TranslateWithFromUri = "Translate?text={0}&from={1}&to={2}&contentType=text/plain";
        private const string SpeakLanguagesUri = "GetLanguagesForSpeak";
        private const string SpeakUri = "Speak?text={0}&language={1}&format={2}&options={3}";
        private const string DetectUri = "Detect?text={0}";
        private const string AuthorizationHeader = "Authorization";

        private const string AudioWavFormat = "audio/wav";
        private const string AudioMp3Format = "audio/mp3";
        private const string MinSizeFormat = "MinSize";
        private const string MaxQualityFormat = "MaxQuality";

        private const string ArrayNamespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays";

        private const int MaxTextLength = 1000;
        private const int MaxTextLengthForAutodetection = 100;

        private readonly AzureAuthToken authToken;
        private readonly HttpClient client;
        private string authorizationHeaderValue = string.Empty;

        /// <summary>
        /// Gets or sets the Subscription key that is necessary to use <strong>Microsoft Translator Service</strong>.
        /// </summary>
        /// <value>The Subscription Key.</value>
        /// <remarks>
        /// <para>You must register Microsoft Translator on https://portal.azure.com/#create/Microsoft.CognitiveServices/apitype/TextTranslation to obtain the Subscription key needed to use the service.</para>
        /// </remarks>
        public string SubscriptionKey
        {
            get { return authToken.SubscriptionKey; }
            set { authToken.SubscriptionKey = value; }
        }

        /// <summary>
        /// Gets or sets the string representing the supported language code to translate the text to.
        /// </summary>
        /// <value>The string representing the supported language code to translate the text to. The code must be present in the list of codes returned from the method <see cref="GetLanguagesAsync"/>.</value>
        /// <seealso cref="GetLanguagesAsync"/>
        public string Language { get; set; }

        /// <summary> 
        /// Gets or sets the audio format of the retrieved audio stream. Currently, <strong>Wav</strong> and <strong>MP3</strong> are supported. 
        /// </summary> 
        /// <value>The audio format of the retrieved audio stream. Currently, <strong>Wav</strong> and <strong>MP3</strong> are supported.</value> 
        /// <remarks>The default value is <strong>MP3</strong>.</remarks>         
        public SpeechStreamFormat AudioFormat { get; set; } = SpeechStreamFormat.Mp3;

        /// <summary> 
        /// Gets or sets the audio quality of the retrieved audio stream. Currently, <strong>MaxQuality</strong> and <strong>MinSize</strong> are supported. 
        /// </summary> 
        /// <value>The audio quality of the retrieved audio stream. Currently, <strong>MaxQuality</strong> and <strong>MinSize</strong> are supported.</value> 
        /// <remarks> 
        /// With <strong>MaxQuality</strong>, you can get the voice with the highest quality, and with <strong>MinSize</strong>, you can get the voices with the smallest size. The default value is <strong>MaxQuality</strong>. 
        /// </remarks> 
        public SpeechStreamQuality AudioQuality { get; set; } = SpeechStreamQuality.MaxQuality;

        /// <summary> 
        /// Gets or sets a value indicating whether the sentence to be spoken must be translated in the specified language. 
        /// </summary> 
        /// <value><strong>true</strong> if the sentence to be spoken must be translated in the specified language; otherwise, <strong>false</strong>.</value> 
        /// <remarks>If you don't need to translate to text to be spoken, you can speed-up the the library setting the <strong>AutomaticTranslation</strong> property to <strong>false</strong>. In this way, the specified text is passed as is to the other methods, without performing any translation. The default value is <strong>false</strong>.</remarks> 
        public bool AutomaticTranslation { get; set; } = false;

        /// <summary> 
        /// Gets or sets a value indicating whether the language of the text must be automatically detected before text-to-speech. 
        /// </summary> 
        /// <value><strong>true</strong> if the language of the text must be automatically detected; otherwise, <strong>false</strong>.</value> 
        /// <remarks>The <strong>AutoDetectLanguage</strong> property is used when the following methods are invoked: 
        /// <list type="bullet"> 
        /// <term><see cref="GetSpeechStreamAsync"/></term> 
        /// </list> 
        /// <para>When these methods are called, if the <strong>AutoDetectLanguage</strong> property is set to <strong>true</strong>, the language of the text is auto-detected before speech stream request. Otherwise, the language specified in the <seealso cref="Language"/> property is used.</para> 
        /// <para>If the language to use is explicitly specified, using the versions of the methods that accept it, no auto-detection is performed.</para> 
        /// <para>The default value is <strong>false</strong>.</para> 
        /// </remarks> 
        /// <seealso cref="Language"/> 
        public bool AutoDetectLanguage { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslatorServiceClient"/> class, using the current system language.
        /// </summary>
        /// <remarks>
        /// <para>You must register Microsoft Translator on https://portal.azure.com/#create/Microsoft.CognitiveServices/apitype/TextTranslation to obtain the Subscription key needed to use the service.</para>
        /// </remarks>
        /// <seealso cref="SubscriptionKey"/>
        /// <seealso cref="Language"/>
        public TranslatorServiceClient()
            : this(null, CultureInfo.CurrentCulture.Name.ToLower())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslatorServiceClient"/> class, using the specified Subscription key and the desired language.
        /// </summary>
        /// <param name="subscriptionKey">The subscription key for the Microsoft Translator Service on Azure
        /// </param>
        /// <param name="language">A string representing the supported language code to translate the text to. The code must be present in the list of codes returned from the method <see cref="GetLanguagesAsync"/>. If a null value is provided, the current system language is used.
        /// </param>
        /// <remarks>
        /// <para>You must register Microsoft Translator on https://portal.azure.com to obtain the Subscription key needed to use the service.</para>
        /// </remarks>
        /// <seealso cref="SubscriptionKey"/>
        /// <seealso cref="Language"/>
        public TranslatorServiceClient(string subscriptionKey, string language = null)
        {
            authToken = new AzureAuthToken(subscriptionKey);
            client = new HttpClient { BaseAddress = new Uri(BaseUrl) };

            SubscriptionKey = subscriptionKey;
            Language = language ?? CultureInfo.CurrentCulture.Name.ToLower();
        }

        /// <summary>
        /// Retrieves the languages available for translation.
        /// </summary>
        /// <param name="serviceType">The service type for which to retrieve supported languages.</param>
        /// <returns>A string array containing the language codes supported for translation by <strong>Microsoft Translator Service</strong>.</returns>
        /// <exception cref="ArgumentNullException">The <see cref="SubscriptionKey"/> property hasn't been set.</exception>
        /// <exception cref="TranslatorServiceException">The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</exception>
        /// <remarks><para>This method performs a non-blocking request for language codes.</para>
        /// <para>For more information, go to http://msdn.microsoft.com/en-us/library/ff512415.aspx.
        /// </para>
        /// </remarks>
        /// <seealso cref="LanguageServiceType"/>

        public async Task<IEnumerable<string>> GetLanguagesAsync(LanguageServiceType serviceType = LanguageServiceType.Translation)
        {
            // Check if it is necessary to obtain/update access token.
            await CheckUpdateTokenAsync().ConfigureAwait(false);

            var uri = serviceType == LanguageServiceType.Speech ? SpeakLanguagesUri : TranslateLanguagesUri;
            var content = await client.GetStringAsync(uri).ConfigureAwait(false);

            XNamespace ns = ArrayNamespace;
            var doc = XDocument.Parse(content);

            var languages = doc.Root.Elements(ns + "string").Select(s => s.Value);
            return languages;
        }

        /// <summary>
        /// Translates a text string into the specified language.
        /// </summary>
        /// <returns>A string representing the translated text.</returns>
        /// <param name="text">A string representing the text to translate.</param>
        /// <param name="to">A string representing the language code to translate the text into. The code must be present in the list of codes returned from the <see cref="GetLanguagesAsync"/> method. If the parameter is set to <strong>null</strong>, the language specified in the <seealso cref="Language"/> property will be used.</param>
        /// <exception cref="ArgumentNullException">
        /// <list type="bullet">
        /// <term>The <see cref="SubscriptionKey"/> property hasn't been set.</term>
        /// <term>The <paramref name="text"/> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic) or empty.</term>
        /// </list>
        /// </exception>        
        /// <exception cref="ArgumentException">The <paramref name="text"/> parameter is longer than 1000 characters.</exception>
        /// <exception cref="TranslatorServiceException">The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</exception>
        /// <remarks><para>This method performs a non-blocking request for text translation.</para>
        /// <para>For more information, go to http://msdn.microsoft.com/en-us/library/ff512421.aspx.
        /// </para>
        /// </remarks>
        /// <seealso cref="Language"/>
        public Task<string> TranslateAsync(string text, string to = null) => TranslateAsync(text, null, to);

        /// <summary>
        /// Translates a text string into the specified language.
        /// </summary>
        /// <returns>A string representing the translated text.</returns>
        /// <param name="text">A string representing the text to translate.</param>
        /// <param name="from">A string representing the language code of the original text. The code must be present in the list of codes returned from the <see cref="GetLanguagesAsync"/> method. If the parameter is set to <strong>null</strong>, the language specified in the <seealso cref="Language"/> property will be used.</param>
        /// <param name="to">A string representing the language code to translate the text into. The code must be present in the list of codes returned from the <see cref="GetLanguagesAsync"/> method. If the parameter is set to <strong>null</strong>, the language specified in the <seealso cref="Language"/> property will be used.</param>
        /// <exception cref="ArgumentNullException">
        /// <list type="bullet">
        /// <term>The <see cref="SubscriptionKey"/> property hasn't been set.</term>
        /// <term>The <paramref name="text"/> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic) or empty.</term>
        /// </list>
        /// </exception>        
        /// <exception cref="ArgumentException">The <paramref name="text"/> parameter is longer than 1000 characters.</exception>
        /// <exception cref="TranslatorServiceException">The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</exception>
        /// <remarks><para>This method performs a non-blocking request for text translation.</para>
        /// <para>For more information, go to http://msdn.microsoft.com/en-us/library/ff512421.aspx.
        /// </para>
        /// </remarks>
        /// <seealso cref="Language"/>
        public async Task<string> TranslateAsync(string text, string from, string to)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentNullException(nameof(text));

            if (text.Length > MaxTextLength)
                throw new ArgumentException($"{nameof(text)} parameter cannot be longer than {MaxTextLength} characters");

            // Checks if it is necessary to obtain/update access token.
            await CheckUpdateTokenAsync().ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(to))
            { 
                to = Language;
            }

            string uri = null;
            if (string.IsNullOrWhiteSpace(from))
            { 
                uri = string.Format(TranslateUri, Uri.EscapeDataString(text), to);
            }
            else
            { 
                uri = string.Format(TranslateWithFromUri, Uri.EscapeDataString(text), from, to);
            }

            var content = await client.GetStringAsync(uri).ConfigureAwait(false);

            var doc = XDocument.Parse(content);
            var translatedText = doc.Root.Value;

            return translatedText;
        }

        /// <summary>
        /// Detects the language of a text.
        /// </summary>
        /// <param name="text">A string represeting the text whose language must be detected.</param>
        /// <returns>A string containing a two-character Language code for the given text.</returns>
        /// <exception cref="ArgumentNullException">
        /// <list type="bullet">
        /// <term>The <see cref="SubscriptionKey"/> property hasn't been set.</term>
        /// <term>The <paramref name="text"/> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic) or empty.</term>
        /// </list>
        /// </exception>        
        /// <exception cref="TranslatorServiceException">The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</exception>
        /// <remarks><para>This method performs a non-blocking request for language detection.</para>
        /// <para>For more information, go to http://msdn.microsoft.com/en-us/library/ff512427.aspx.
        /// </para></remarks>
        /// <seealso cref="GetLanguagesAsync"/>
        /// <seealso cref="Language"/>
        public async Task<string> DetectLanguageAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            { 
                throw new ArgumentNullException(nameof(text));
            }

            text = text.Substring(0, Math.Min(text.Length, MaxTextLengthForAutodetection));

            // Checks if it is necessary to obtain/update access token.
            await CheckUpdateTokenAsync().ConfigureAwait(false);

            var uri = string.Format(DetectUri, Uri.EscapeDataString(text));
            var content = await client.GetStringAsync(uri).ConfigureAwait(false);

            var doc = XDocument.Parse(content);
            var detectedLanguage = doc.Root.Value;

            return detectedLanguage;
        }

        /// <summary> 
        /// Returns a stream of a file speaking the passed-in text in the desired language. If <paramref name="language"/> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic) and the <see cref="AutoDetectLanguage"/> property is set to <strong>true</strong>, the <see cref="DetectLanguageAsync(string)"/> method is used to detect the language of the speech stream. Otherwise, the language specified in the <see cref="Language"/> property is used. 
        /// </summary> 
        /// <param name="text">A string containing the sentence to be spoken.</param> 
        /// <param name="language">A string representing the language code to speak the text in. The code must be present in the list of codes returned from the method <see cref="GetLanguagesAsync"/>.</param> 
        /// <returns>A <see cref="Stream"/> object that contains a file speaking the passed-in text in the desired language.</returns> 
        /// <exception cref="ArgumentNullException">
        /// <list type="bullet">
        /// <term>The <see cref="SubscriptionKey"/> property hasn't been set.</term>
        /// <term>The <paramref name="text"/> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic) or empty.</term>
        /// </list>
        /// </exception>  
        /// <exception cref="ArgumentException">The <paramref name="text"/> parameter is longer than 1000 characters.</exception>
        /// <exception cref="TranslatorServiceException">The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</exception>
        /// <remarks> 
        /// <para>This method performs a non-blocking request for speak stream.</para> 
        /// <para>For more information, go to http://msdn.microsoft.com/en-us/library/ff512420.aspx. 
        /// </para></remarks> 
        /// <seealso cref="Stream"/> 
        /// <seealso cref="Language"/> 
        /// <seealso cref="GetLanguagesAsync"/> 
        public async Task<Stream> GetSpeechStreamAsync(string text, string language = null)
        {
            if (string.IsNullOrWhiteSpace(text))
            { 
                throw new ArgumentNullException(nameof(text));
            }

            if (text.Length > MaxTextLength)
            {
                throw new ArgumentException($"{nameof(text)} parameter cannot be longer than {MaxTextLength} characters");
            }

            var languageDetected = false;
            if (string.IsNullOrEmpty(language))
            {
                if (AutoDetectLanguage)
                {
                    language = await DetectLanguageAsync(text).ConfigureAwait(false);
                    languageDetected = true;
                }
                else
                {
                    language = Language;
                }
            }

            if (!languageDetected && AutomaticTranslation)
            { 
                text = await TranslateAsync(text, language).ConfigureAwait(false);
            }

            // Checks if it is necessary to obtain/update access token.
            await CheckUpdateTokenAsync().ConfigureAwait(false);

            var audioFormat = AudioFormat == SpeechStreamFormat.Wave ? AudioWavFormat : AudioMp3Format;
            var audioQuality = AudioQuality == SpeechStreamQuality.MinSize ? MinSizeFormat : MaxQualityFormat;
            var uri = string.Format(SpeakUri, Uri.EscapeDataString(text), language, Uri.EscapeDataString(audioFormat), audioQuality);

            var content = await client.GetByteArrayAsync(uri).ConfigureAwait(false);

            var speakStream = new MemoryStream(content);            
            return speakStream;
        }

        private async Task CheckUpdateTokenAsync()
        {
            var token = await authToken.GetAccessTokenAsync().ConfigureAwait(false);
            if (token != authorizationHeaderValue)
            {
                // Updates the access token.
                authorizationHeaderValue = token;
                var headers = client.DefaultRequestHeaders;

                if (headers.Contains(AuthorizationHeader))
                    headers.Remove(AuthorizationHeader);

                headers.Add(AuthorizationHeader, authorizationHeaderValue);
            }
        }

        /// <summary>
        /// Initializes the <see cref="TranslatorServiceClient"/> class by getting an access token for the service.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents the initialize operation.</returns>
        /// <exception cref="ArgumentNullException">The <see cref="SubscriptionKey"/> property hasn't been set.</exception>
        /// <exception cref="TranslatorServiceException">The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</exception>
        /// <remarks>Calling this method isn't mandatory, because the token is get/refreshed everytime is needed. However, it is called at startup, it can speed-up subsequest requests.</remarks>
        public Task InitializeAsync() => CheckUpdateTokenAsync();

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            authToken.Dispose();
            client.Dispose();
        }
    }
}