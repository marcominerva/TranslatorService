using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TranslatorService.Models;

namespace TranslatorService
{
    /// <summary>
    /// The <strong>ITranslatorServiceClient</strong> interface specifies properties and methods to translate text in various supported languages.
    /// </summary>
    public interface ITranslatorServiceClient : IDisposable
    {
        /// <summary>
        /// Gets or sets the Subscription key that is necessary to use <strong>Microsoft Translator Service</strong>.
        /// </summary>
        /// <value>The Subscription Key.</value>
        /// <remarks>
        /// <para>You must register Microsoft Translator on https://portal.azure.com/#create/Microsoft.CognitiveServices/apitype/TextTranslation to obtain the Subscription key needed to use the service.</para>
        /// </remarks>
        string SubscriptionKey { get; set; }

        /// <summary>
        /// Gets or sets the string representing the supported language code to translate the text in.
        /// </summary>
        /// <value>The string representing the supported language code to translate the text in. The code must be present in the list of codes returned from the method <see cref="GetLanguagesAsync"/>.</value>
        /// <seealso cref="GetLanguagesAsync"/>
        string Language { get; set; }

        /// <summary> 
        /// Gets or sets the audio format of the retrieved audio stream. Currently, <strong>Wav</strong> and <strong>MP3</strong> are supported. 
        /// </summary> 
        /// <value>The audio format of the retrieved audio stream. Currently, <strong>Wav</strong> and <strong>MP3</strong> are supported.</value> 
        /// <remarks>The default value is <strong>Mp3</strong>.</remarks>         
        SpeechStreamFormat AudioFormat { get; set; }

        /// <summary> 
        /// Gets or sets the audio quality of the retrieved audio stream. Currently, <strong>MaxQuality</strong> and <strong>MinSize</strong> are supported. 
        /// </summary> 
        /// <value>The audio quality of the retrieved audio stream. Currently, <strong>MaxQuality</strong> and <strong>MinSize</strong> are supported.</value> 
        /// <remarks> 
        /// With <strong>MaxQuality</strong>, you can get the voice with the highest quality, and with <strong>MinSize</strong>, you can get the voices with the smallest size. The default value is <strong>MaxQuality</strong>. 
        /// </remarks> 
        SpeechStreamQuality AudioQuality { get; set; }

        /// <summary> 
        /// Gets or sets a value indicating whether the sentence to be spoken must be translated in the specified language. 
        /// </summary> 
        /// <value><strong>true</strong> if the sentence to be spoken must be translated in the specified language; otherwise, <strong>false</strong>.</value> 
        /// <remarks>If you don't need to translate to text to be spoken, you can speed-up the the library setting the <strong>AutomaticTranslation</strong> property to <strong>false</strong>. In this way, the specified text is passed as is to the other methods, without performing any translation. The default value is <strong>true</strong>.</remarks> 
        bool AutomaticTranslation { get; set; }

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
        /// <para>The default value is <strong>true</strong>.</para> 
        /// </remarks> 
        /// <seealso cref="Language"/> 
        bool AutoDetectLanguage { get; set; }

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
        Task<string> DetectLanguageAsync(string text);

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
        Task<IEnumerable<string>> GetLanguagesAsync(LanguageServiceType serviceType = LanguageServiceType.Translation);

        /// <summary>
        /// Initializes the <see cref="TranslatorServiceClient"/> class by getting an access token for the service.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents the initialize operation.</returns>
        /// <exception cref="ArgumentNullException">The <see cref="SubscriptionKey"/> property hasn't been set.</exception>
        /// <exception cref="TranslatorServiceException">The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</exception>
        /// <remarks>Calling this method isn't mandatory, because the token is get/refreshed everytime is needed. However, it is called at startup, it can speed-up subsequest requests.</remarks>
        Task InitializeAsync();

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
        Task<string> TranslateAsync(string text, string to = null);

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
        Task<string> TranslateAsync(string text, string from, string to);

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
        Task<Stream> GetSpeechStreamAsync(string text, string language = null);
    }
}