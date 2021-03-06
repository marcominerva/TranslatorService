﻿using System.IO;
using System.Text.Json.Serialization;

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
        public float Confidence { get; init; }

        /// <summary>
        /// The lexical form of the recognized text: the actual words recognized.
        /// </summary>
        [JsonPropertyName("Lexical")]
        public string LexicalForm { get; init; } = null!;

        /// <summary>
        /// The inverse-text-normalized ("canonical") form of the recognized text, with phone numbers, numbers, abbreviations ("doctor smith" to "dr smith"), and other transformations applied.
        /// </summary>
        [JsonPropertyName("ITN")]
        public string CanonicalForm { get; init; } = null!;

        /// <summary>
        /// The ITN form with profanity masking applied, if requested.
        /// </summary>
        [JsonPropertyName("MaskedITN")]
        public string MaskedCanonicalForm { get; init; } = null!;

        /// <summary>
        /// The display form of the recognized text, with punctuation and capitalization added. This parameter is the same as <seealso cref="SpeechRecognitionResponse.DisplayText"/> provided when <seealso cref="RecognitionResultFormat"/> is set to <seealso cref="RecognitionResultFormat.Simple"/>.
        /// </summary>
        public string Display { get; init; } = null!;
    }
}
