namespace TranslatorService.Models.Translation
{
    /// <summary>
    /// Strong type for Translation
    /// </summary>
    /// <seealso cref="ITranslatorClient.TranslateAsync(string, string, string)"/>
    public class Translation
    {
        /// <summary>
        /// Gets or sets a string giving the translated text.
        /// </summary>
        public string Text { get; init; } = null!;

        /// <summary>
        /// Gets or sets a string representing the language code of the target language.
        /// </summary>
        public string To { get; init; } = null!;
    }
}
