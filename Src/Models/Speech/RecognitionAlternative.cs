using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslatorService.Models.Speech
{
    /// <summary>
    /// The <see cref="RecognitionAlternative"/> class contains information about a recognition result.
    /// </summary>
    /// <seealso cref="ISpeechClient.RecognizeAsync(Stream, string, RecognitionResultFormat, SpeechProfanityMode)"/>
    public class RecognitionAlternative
    {
        /// <summary>
        /// The confidence score of the entry from 0.0 (no confidence) to 1.0 (full confidence)
        /// </summary>
        public float Confidence { get; set; }

        /// <summary>
        /// The lexical form of the recognized text: the actual words recognized.
        /// </summary>
        [JsonProperty("Lexical")]
        public string LexicalForm { get; set; }

        /// <summary>
        /// The inverse-text-normalized ("canonical") form of the recognized text, with phone numbers, numbers, abbreviations ("doctor smith" to "dr smith"), and other transformations applied.
        /// </summary>
        [JsonProperty("ITN")]
        public string CanonicalForm { get; set; }

        /// <summary>
        /// The ITN form with profanity masking applied, if requested.
        /// </summary>
        [JsonProperty("MaskedITN")]
        public string MaskedCanonicalForm { get; set; }

        /// <summary>
        /// The display form of the recognized text, with punctuation and capitalization added. This parameter is the same as <seealso cref="SpeechRecognitionResponse.DisplayText"/> provided when <seealso cref="RecognitionResultFormat"/> is set to <seealso cref="RecognitionResultFormat.Simple"/>.
        /// </summary>
        public string Display { get; set; }
    }
}
