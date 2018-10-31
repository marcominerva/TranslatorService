using System.Collections.Generic;

namespace TranslatorService.Models
{
    /// <summary>
    /// Strong type for Detect Language Response
    /// </summary>
    /// <seealso cref="ITranslatorClient.DetectLanguageAsync(string)"/>
    /// <seealso cref="ITranslatorClient.DetectLanguagesAsync(IEnumerable{string})"/>
    public class DetectedLanguageResponse : DetectedLanguage
    {
        /// <summary>
        /// Gets or sets an array of other possible languages.
        /// </summary>
        public IEnumerable<DetectedLanguage> Alternatives { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DetectedLanguageResponse"/> class.
        /// </summary>
        /// <param name="language">The code of the detected language.</param>
        /// <param name="score">A float value indicating the confidence in the result. The score is between zero and one and a low score indicates a low confidence.</param>
        /// <param name="isTranslationSupported">A value indicating whether the detected language is one of the languages supported for text translation.</param>
        /// <param name="isTransliterationSupported">A value indicating whether the detected language is one of the languages supported for transliteration.</param>
        /// <param name="alternatives">An array of other possible languages</param>
        /// <seealso cref="DetectedLanguage"/>
        public DetectedLanguageResponse(string language, float score, bool isTranslationSupported, bool isTransliterationSupported, IEnumerable<DetectedLanguage> alternatives)
            : base(language,score,isTranslationSupported,isTransliterationSupported)
        {
            Alternatives = alternatives;
        }
    }
}
