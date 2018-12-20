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
        public string Text { get; }

        /// <summary>
        /// Gets or sets a string representing the language code of the target language.
        /// </summary>
        public string To { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Translation"/> class.
        /// </summary>
        /// <param name="text">A string giving the translated text.</param>
        /// <param name="to">a string representing the language code of the target language.</param>
        public Translation(string text, string to)
        {
            Text = text;
            To = to;
        }
    }
}
