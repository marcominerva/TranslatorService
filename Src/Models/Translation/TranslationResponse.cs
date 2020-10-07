using System.Collections.Generic;
using System.Linq;

namespace TranslatorService.Models.Translation
{
    /// <summary>
    /// Strong type for Translate Response
    /// </summary>
    /// <seealso cref="ITranslatorClient.TranslateAsync(string, string, string)"/>
    public class TranslationResponse
    {
        /// <summary>
        /// Gets or sets a <see cref="DetectedLanguageBase"/> object describing the detected language.
        /// </summary>
        /// <remarks>This property has a value only when the <see cref="ITranslatorClient.TranslateAsync(string, string)"/> method is invoked without the <strong>from</strong> parameter, so that automatic language detection is applied to determine the source language.
        /// </remarks>
        public DetectedLanguageBase DetectedLanguage { get; set; } = null!;

        /// <summary>
        /// Gets or sets an array of <see cref="Translation"/> results.
        /// </summary>
        public IEnumerable<Translation> Translations { get; set; } = new List<Translation>();

        /// <summary>
        /// Gets the first translation result.
        /// </summary>
        public Translation? Translation => Translations?.FirstOrDefault();
    }
}
