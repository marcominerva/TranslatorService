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
        public bool IsTranslationSupported { get; init; }

        /// <summary>
        /// Gets or sets a value indicating whether the detected language is one of the languages supported for transliteration.
        /// </summary>
        public bool IsTransliterationSupported { get; init; }
    }
}
