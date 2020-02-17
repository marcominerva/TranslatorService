using System.Collections.Generic;

namespace TranslatorService.Models.Translation
{
    /// <summary>
    /// Strong type for Detected Language
    /// </summary>
    /// <seealso cref="ITranslatorClient.DetectLanguageAsync(string)"/>
    /// <seealso cref="ITranslatorClient.DetectLanguagesAsync(IEnumerable{string})"/>
    public class DetectedLanguage : DetectedLanguageBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether the detected language is one of the languages supported for text translation.
        /// </summary>
        public bool IsTranslationSupported { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the detected language is one of the languages supported for transliteration.
        /// </summary>
        public bool IsTransliterationSupported { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DetectedLanguage"/> class.
        /// </summary>
        /// <param name="language">The code of the detected language.</param>
        /// <param name="score">A float value indicating the confidence in the result. The score is between zero and one and a low score indicates a low confidence.</param>
        /// <param name="isTranslationSupported">A value indicating whether the detected language is one of the languages supported for text translation.</param>
        /// <param name="isTransliterationSupported">A value indicating whether the detected language is one of the languages supported for transliteration.</param>
        /// <seealso cref="DetectedLanguageBase"/>
        public DetectedLanguage(string language, float score, bool isTranslationSupported, bool isTransliterationSupported)
            : base(language, score)
        {
            IsTranslationSupported = isTranslationSupported;
            IsTransliterationSupported = isTransliterationSupported;
        }
    }
}
