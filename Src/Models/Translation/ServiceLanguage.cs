using System.Text.Json.Serialization;

namespace TranslatorService.Models.Translation
{
    /// <summary>
    /// Holds information about langagues supported for text translation and speech synthesis.
    /// </summary>
    /// <seealso cref="ITranslatorClient.GetLanguagesAsync(string)"/>
    public class ServiceLanguage
    {
        /// <summary>
        /// Gets or sets the language code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the language friendly name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the display name of the language in the locale native for this language.
        /// </summary>
        public string NativeName { get; set; }

        /// <summary>
        /// Gets or sets the directionality, which is rtl for right-to-left languages or ltr for left-to-right languages.
        /// </summary>
        [JsonPropertyName("dir")]
        public LanguageDirectionality Directionality { get; set; }

        /// <summary>
        /// Returns the language friendly name.
        /// </summary>
        /// <returns>The language friendly name.</returns>
        public override string ToString() => Name;
    }
}