using System.Collections.Generic;

namespace TranslatorService.Models.Translation
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
        public IEnumerable<DetectedLanguage> Alternatives { get; set; }
    }
}
