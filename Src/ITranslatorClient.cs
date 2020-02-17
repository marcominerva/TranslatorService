using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TranslatorService.Models.Translation;

namespace TranslatorService
{
    /// <summary>
    /// The <strong>ITranslatorClient</strong> interface specifies properties and methods to translate text in various supported languages.
    /// </summary>
    public interface ITranslatorClient : IDisposable
    {
        /// <summary>
        /// Gets or sets the Authentication URI for the Translator service.
        /// </summary>
        string AuthenticationUri { get; set; }

        /// <summary>
        /// Gets or sets the Subscription key that is necessary to use <strong>Microsoft Translator Service</strong>.
        /// </summary>
        /// <value>The Subscription Key.</value>
        /// <remarks>
        /// <para>You must register Microsoft Translator on https://portal.azure.com/#create/Microsoft.CognitiveServicesTextTranslation to obtain the Subscription key needed to use the service.</para>
        /// </remarks>
        string SubscriptionKey { get; set; }

        /// <summary>
        /// Gets or sets the the Azure region of the the Translator service.
        /// </summary>
        /// <remarks>This value is used to automatically set the <see cref="AuthenticationUri"/> property. If the paramter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic), the global service is used.
        /// </remarks>
        string Region { get; set; }

        /// <summary>
        /// Gets or sets the string representing the supported language code to translate the text in.
        /// </summary>
        /// <value>The string representing the supported language code to translate the text in. The code must be present in the list of codes returned from the method <see cref="GetLanguagesAsync"/>.</value>
        /// <seealso cref="GetLanguagesAsync"/>
        string Language { get; set; }

        /// <summary>
        /// Initializes the <see cref="TranslatorClient"/> class by getting an access token for the global (non region-dependent) service, using the current language.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents the initialize operation.</returns>
        /// <exception cref="ArgumentNullException">The <see cref="SubscriptionKey"/> property hasn't been set.</exception>
        /// <exception cref="ServiceException">The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</exception>
        /// <remarks>Calling this method isn't mandatory, because the token is get/refreshed everytime is needed. However, it is called at startup, it can speed-up subsequest requests.</remarks>
        Task InitializeAsync();

        /// <summary>
        /// Initializes the <see cref="TranslatorClient"/> class by getting an access token for a specified region service, using the given language.
        /// </summary>
        /// <param name="region">The Azure region of the the Speech service. This value is used to automatically set the <see cref="AuthenticationUri"/> property. If the <em>region</em> paramter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic), the global service is used.</param>
        /// <param name="subscriptionKey">The subscription key for the Microsoft Translator Service on Azure.</param>
        /// <param name="language">A string representing the supported language code to speak the text in. The code must be present in the list of codes returned from the method <see cref="GetLanguagesAsync"/>. If the <em>language</em> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic), the current language is used.</param>
        /// <returns>A <see cref="Task"/> that represents the initialize operation.</returns>
        /// <exception cref="ArgumentNullException">The <see cref="SubscriptionKey"/> property hasn't been set.</exception>
        /// <exception cref="ServiceException">The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</exception>
        /// <remarks>
        /// <para>Calling this method isn't mandatory, because the token is get/refreshed everytime is needed. However, it is called at startup, it can speed-up subsequest requests.</para>
        /// <para>You must register Microsoft Translator on https://portal.azure.com/#create/Microsoft.CognitiveServicesTextTranslation to obtain the Subscription key needed to use the service.</para>
        /// </remarks>
        Task InitializeAsync(string region, string subscriptionKey, string language = null);

        /// <summary>
        /// Detects the language of a text.
        /// </summary>
        /// <param name="input">A string representing the text whose language must be detected.</param>
        /// <returns>A <see cref="DetectedLanguageResponse"/> object containing information about the detected language.</returns>
        /// <exception cref="ArgumentNullException">
        /// <list type="bullet">
        /// <term>The <see cref="SubscriptionKey"/> property hasn't been set.</term>
        /// <term>The <paramref name="input"/> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic) or empty.</term>
        /// </list>
        /// </exception>
        /// <exception cref="ServiceException">The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</exception>
        /// <remarks><para>This method performs a non-blocking request for language detection.</para>
        /// <para>For more information, go to https://docs.microsoft.com/azure/cognitive-services/translator/reference/v3-0-detect.
        /// </para></remarks>
        /// <seealso cref="GetLanguagesAsync"/>
        /// <seealso cref="Language"/>
        Task<DetectedLanguageResponse> DetectLanguageAsync(string input);

        /// <summary>
        /// Detects the language of a text.
        /// </summary>
        /// <param name="input">A string array containing the sentences whose language must be detected.</param>
        /// <returns>A <see cref="DetectedLanguageResponse"/> array with one result for each string in the input array. Each object contains information about the detected language.</returns>
        /// <exception cref="ArgumentException">
        /// <list type="bullet">
        /// <term>The <paramref name="input"/> parameter doesn't contain any element.</term>
        /// <term>The <paramref name="input"/> array contains more than 100 elements.</term>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <list type="bullet">
        /// <term>The <see cref="SubscriptionKey"/> property hasn't been set.</term>
        /// <term>The <paramref name="input"/> array is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic).</term>
        /// </list>
        /// </exception>
        /// <exception cref="ServiceException">
        /// <list type="bullet">
        /// <term>The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</term>
        /// <term>The call to the method has encountered an unexpected error.</term>
        /// </list>
        /// </exception>        /// <remarks><para>This method performs a non-blocking request for language detection.</para>
        /// <para>For more information, go to https://docs.microsoft.com/azure/cognitive-services/translator/reference/v3-0-detect.
        /// </para></remarks>
        /// <seealso cref="GetLanguagesAsync"/>
        /// <seealso cref="Language"/>
        Task<IEnumerable<DetectedLanguageResponse>> DetectLanguagesAsync(IEnumerable<string> input);

        /// <summary>
        /// Retrieves friendly names for the languages available for text translation.
        /// </summary>
        /// <param name="language">The language used to localize the language names. If the parameter is set to <strong>null</strong>, the language specified in the <seealso cref="Language"/> property will be used.</param>
        /// <returns>An array of <see cref="ServiceLanguage"/> containing the language codes and names supported for translation by <strong>Microsoft Translator Service</strong>.</returns>
        /// <exception cref="ArgumentNullException">The <see cref="SubscriptionKey"/> property hasn't been set.</exception>
        /// <exception cref="ServiceException">
        /// <list type="bullet">
        /// <term>The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</term>
        /// <term>The call to the method has encountered an unexpected error.</term>
        /// </list>
        /// </exception>
        /// <remarks><para>This method performs a non-blocking request for language names.</para>
        /// <para>For more information, go to https://docs.microsoft.com/azure/cognitive-services/translator/reference/v3-0-languages.
        /// </para>
        /// </remarks>
        Task<IEnumerable<ServiceLanguage>> GetLanguagesAsync(string language = null);

        /// <summary>
        /// Translates a text string into the specified language.
        /// </summary>
        /// <returns>A <see cref="TranslationResponse"/> object containing translated text and information.</returns>
        /// <param name="input">A string representing the text to translate.</param>
        /// <param name="to">A string representing the language code to translate the text into. The code must be present in the list of codes returned from the <see cref="GetLanguagesAsync"/> method. If the parameter is set to <strong>null</strong>, the language specified in the <seealso cref="Language"/> property will be used.</param>
        /// <exception cref="ArgumentNullException">
        /// <list type="bullet">
        /// <term>The <see cref="SubscriptionKey"/> property hasn't been set.</term>
        /// <term>The <paramref name="input"/> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic) or empty.</term>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentException">The <paramref name="input"/> parameter is longer than 1000 characters.</exception>
        /// <exception cref="ServiceException">
        /// <list type="bullet">
        /// <term>The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</term>
        /// <term>The call to the method has encountered an unexpected error.</term>
        /// </list>
        /// </exception>
        /// <remarks><para>This method perform a non-blocking request for text translation.</para>
        /// <para>For more information, go to https://docs.microsoft.com/azure/cognitive-services/translator/reference/v3-0-translate.
        /// </para>
        /// </remarks>
        /// <seealso cref="Language"/>
        /// <seealso cref="GetLanguagesAsync"/>
        Task<TranslationResponse> TranslateAsync(string input, string to = null);

        /// <summary>
        /// Translates a text string into the specified languages.
        /// </summary>
        /// <returns>A <see cref="TranslationResponse"/> object containing translated text and information.</returns>
        /// <param name="input">A string representing the text to translate.</param>
        /// <param name="from">A string representing the language code of the original text. The code must be present in the list of codes returned from the <see cref="GetLanguagesAsync"/> method. If the parameter is set to <strong>null</strong>, the language specified in the <seealso cref="Language"/> property will be used.</param>
        /// <param name="to">A string representing the language code to translate the text into. The code must be present in the list of codes returned from the <see cref="GetLanguagesAsync"/> method. If the parameter is set to <strong>null</strong>, the language specified in the <seealso cref="Language"/> property will be used.</param>
        /// <exception cref="ArgumentNullException">
        /// <list type="bullet">
        /// <term>The <see cref="SubscriptionKey"/> property hasn't been set.</term>
        /// <term>The <paramref name="input"/> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic) or empty.</term>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentException">The <paramref name="input"/> parameter is longer than 1000 characters.</exception>
        /// <exception cref="ServiceException">
        /// <list type="bullet">
        /// <term>The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</term>
        /// <term>The call to the method has encountered an unexpected error.</term>
        /// </list>
        /// </exception>
        /// <remarks><para>This method perform a non-blocking request for text translation.</para>
        /// <para>For more information, go to https://docs.microsoft.com/azure/cognitive-services/translator/reference/v3-0-translate.
        /// </para>
        /// </remarks>
        /// <seealso cref="Language"/>
        /// <seealso cref="GetLanguagesAsync"/>
        Task<TranslationResponse> TranslateAsync(string input, string from, string to);

        /// <summary>
        /// Translates a list of sentences into the specified language.
        /// </summary>
        /// <returns>A <see cref="TranslationResponse"/> array with one result for each language code in the <paramref name="to"/> array. Each object contains translated text and information.</returns>
        /// <param name="input">A string array containing the sentences to translate.</param>
        /// <param name="from">A string representing the language code of the original text. The code must be present in the list of codes returned from the <see cref="GetLanguagesAsync"/> method. If the parameter is set to <strong>null</strong>, the language specified in the <seealso cref="Language"/> property will be used.</param>
        /// <param name="to">A string representing the language code to translate the text into. The code must be present in the list of codes returned from the <see cref="GetLanguagesAsync"/> method. If the parameter is set to <strong>null</strong>, the language specified in the <seealso cref="Language"/> property will be used.</param>
        /// <exception cref="ArgumentNullException">
        /// <list type="bullet">
        /// <term>The <see cref="SubscriptionKey"/> property hasn't been set.</term>
        /// <term>The <paramref name="input"/> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic).</term>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <list type="bullet">
        /// <term>The <paramref name="input"/> parameter is longer than 1000 characters.</term>
        /// <term>The <paramref name="input"/> array contains more than 25 elements.</term>
        /// </list>
        /// </exception>
        /// <exception cref="ServiceException">
        /// <list type="bullet">
        /// <term>The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</term>
        /// <term>The call to the method has encountered an unexpected error.</term>
        /// </list>
        /// </exception>
        /// <remarks><para>This method perform a non-blocking request for text translation.</para>
        /// <para>For more information, go to https://docs.microsoft.com/azure/cognitive-services/translator/reference/v3-0-translate.
        /// </para>
        /// </remarks>
        /// <seealso cref="Language"/>
        /// <seealso cref="GetLanguagesAsync"/>
        Task<IEnumerable<TranslationResponse>> TranslateAsync(IEnumerable<string> input, string from, string to);

        /// <summary>
        /// Translates a text into the specified languages.
        /// </summary>
        /// <returns>A <see cref="TranslationResponse"/> object containing translated text and information.</returns>
        /// <param name="input">A string representing the text to translate.</param>
        /// <param name="from">A string representing the language code of the original text. The code must be present in the list of codes returned from the <see cref="GetLanguagesAsync"/> method. If the parameter is set to <strong>null</strong>, the language specified in the <seealso cref="Language"/> property will be used.</param>
        /// <param name="to">A string array representing the language codes to translate the text into. The code must be present in the list of codes returned from the <see cref="GetLanguagesAsync"/> method. If the parameter is set to <strong>null</strong>, the language specified in the <seealso cref="Language"/> property will be used.</param>
        /// <exception cref="ArgumentNullException">
        /// <list type="bullet">
        /// <term>The <see cref="SubscriptionKey"/> property hasn't been set.</term>
        /// <term>The <paramref name="input"/> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic) or empty.</term>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <list type="bullet">
        /// <term>The <paramref name="input"/> parameter is longer than 1000 characters.</term>
        /// <term>The <paramref name="to"/> array contains more than 25 elements.</term>
        /// </list>
        /// </exception>
        /// <exception cref="ServiceException">
        /// <list type="bullet">
        /// <term>The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</term>
        /// <term>The call to the method has encountered an unexpected error.</term>
        /// </list>
        /// </exception>
        /// <remarks><para>This method perform a non-blocking request for text translation.</para>
        /// <para>For more information, go to https://docs.microsoft.com/azure/cognitive-services/translator/reference/v3-0-translate.
        /// </para>
        /// </remarks>
        /// <seealso cref="Language"/>
        /// <seealso cref="GetLanguagesAsync"/>
        Task<TranslationResponse> TranslateAsync(string input, string from, IEnumerable<string> to);

        /// <summary>
        /// Translates a text string into the specified languages.
        /// </summary>
        /// <returns>A <see cref="TranslationResponse"/> object containing translated text and information.</returns>
        /// <param name="input">A string representing the text to translate.</param>
        /// <param name="to">A string array representing the language codes to translate the text into. The code must be present in the list of codes returned from the <see cref="GetLanguagesAsync"/> method. If the parameter is set to <strong>null</strong>, the language specified in the <seealso cref="Language"/> property will be used.</param>
        /// <exception cref="ArgumentNullException">
        /// <list type="bullet">
        /// <term>The <see cref="SubscriptionKey"/> property hasn't been set.</term>
        /// <term>The <paramref name="input"/> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic) or empty.</term>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <list type="bullet">
        /// <term>The <paramref name="input"/> parameter is longer than 1000 characters.</term>
        /// <term>The <paramref name="to"/> array contains more than 25 elements.</term>
        /// </list>
        /// </exception>
        /// <exception cref="ServiceException">
        /// <list type="bullet">
        /// <term>The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</term>
        /// <term>The call to the method has encountered an unexpected error.</term>
        /// </list>
        /// </exception>
        /// <remarks><para>This method perform a non-blocking request for text translation.</para>
        /// <para>For more information, go to https://docs.microsoft.com/azure/cognitive-services/translator/reference/v3-0-translate.
        /// </para>
        /// </remarks>
        /// <seealso cref="Language"/>
        /// <seealso cref="GetLanguagesAsync"/>
        Task<TranslationResponse> TranslateAsync(string input, IEnumerable<string> to);

        /// <summary>
        /// Translates a list of sentences into the specified languages.
        /// </summary>
        /// <returns>A <see cref="TranslationResponse"/> array with one result for each language code in the <paramref name="to"/> array. Each object contains translated text and information.</returns>
        /// <param name="input">A string array containing the sentences to translate.</param>
        /// <param name="to">A string array representing the language codes to translate the text into. The code must be present in the list of codes returned from the <see cref="GetLanguagesAsync"/> method. If the parameter is set to <strong>null</strong>, the language specified in the <seealso cref="Language"/> property will be used.</param>
        /// <exception cref="ArgumentNullException">
        /// <list type="bullet">
        /// <term>The <see cref="SubscriptionKey"/> property hasn't been set.</term>
        /// <term>The <paramref name="input"/> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic).</term>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <list type="bullet">
        /// <term>The <paramref name="input"/> parameter is longer than 1000 characters.</term>
        /// <term>The <paramref name="input"/> array contains more than 25 elements.</term>
        /// </list>
        /// </exception>
        /// <exception cref="ServiceException">
        /// <list type="bullet">
        /// <term>The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</term>
        /// <term>The call to the method has encountered an unexpected error.</term>
        /// </list>
        /// </exception>
        /// <remarks><para>This method perform a non-blocking request for text translation.</para>
        /// <para>For more information, go to https://docs.microsoft.com/azure/cognitive-services/translator/reference/v3-0-translate.
        /// </para>
        /// </remarks>
        /// <seealso cref="Language"/>
        /// <seealso cref="GetLanguagesAsync"/>
        Task<IEnumerable<TranslationResponse>> TranslateAsync(IEnumerable<string> input, IEnumerable<string> to = null);

        /// <summary>
        /// Translates a list of sentences into the specified languages.
        /// </summary>
        /// <returns>A <see cref="TranslationResponse"/> array with one result for each language code in the <paramref name="to"/> array. Each object contains translated text and information.</returns>
        /// <param name="input">A string array containing the sentences to translate.</param>
        /// <param name="from">A string representing the language code of the original text. The code must be present in the list of codes returned from the <see cref="GetLanguagesAsync"/> method. If the parameter is set to <strong>null</strong>, the language specified in the <seealso cref="Language"/> property will be used.</param>
        /// <param name="to">A string array representing the language codes to translate the text into. The code must be present in the list of codes returned from the <see cref="GetLanguagesAsync"/> method. If the parameter is set to <strong>null</strong>, the language specified in the <seealso cref="Language"/> property will be used.</param>
        /// <exception cref="ArgumentNullException">
        /// <list type="bullet">
        /// <term>The <see cref="SubscriptionKey"/> property hasn't been set.</term>
        /// <term>The <paramref name="input"/> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic).</term>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <list type="bullet">
        /// <term>The <paramref name="input"/> parameter is longer than 1000 characters.</term>
        /// <term>The <paramref name="input"/> array contains more than 25 elements.</term>
        /// </list>
        /// </exception>
        /// <exception cref="ServiceException">
        /// <list type="bullet">
        /// <term>The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</term>
        /// <term>The call to the method has encountered an unexpected error.</term>
        /// </list>
        /// </exception>
        /// <remarks><para>This method perform a non-blocking request for text translation.</para>
        /// <para>For more information, go to https://docs.microsoft.com/azure/cognitive-services/translator/reference/v3-0-translate.
        /// </para>
        /// </remarks>
        /// <seealso cref="Language"/>
        /// <seealso cref="GetLanguagesAsync"/>
        Task<IEnumerable<TranslationResponse>> TranslateAsync(IEnumerable<string> input, string from, IEnumerable<string> to);
    }
}