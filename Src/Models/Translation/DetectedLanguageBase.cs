using System.Collections.Generic;

namespace TranslatorService.Models.Translation
{
    /// <summary>
    /// Strong type for Base Detected Language
    /// </summary>
    /// <seealso cref="ITranslatorClient.DetectLanguageAsync(string)"/>
    /// <seealso cref="ITranslatorClient.DetectLanguagesAsync(IEnumerable{string})"/>
    /// <seealso cref="ITranslatorClient.TranslateAsync(IEnumerable{string}, IEnumerable{string})"/>
    /// <seealso cref="ITranslatorClient.TranslateAsync(IEnumerable{string}, string, IEnumerable{string})"/>
    /// <seealso cref="ITranslatorClient.TranslateAsync(IEnumerable{string}, string, string)"/>
    /// <seealso cref="ITranslatorClient.TranslateAsync(string, IEnumerable{string})"/>
    /// <seealso cref="ITranslatorClient.TranslateAsync(string, string)"/>
    /// <seealso cref="ITranslatorClient.TranslateAsync(string, string, IEnumerable{string})"/>
    /// <seealso cref="ITranslatorClient.TranslateAsync(string, string, string)"/>
    public class DetectedLanguageBase
    {
        /// <summary>
        /// Gets or sets the code of the detected language.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets a float value indicating the confidence in the result. The score is between zero and one and a low score indicates a low confidence.
        /// </summary>
        public float Score { get; set; }

        /// <inheritdoc/>
        public override string ToString() => Language;
    }
}
