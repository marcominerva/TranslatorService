using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace TranslatorService.Models.Speech
{
    /// <summary>
    /// The <strong>RecognitionSpeechResponse</strong> class contains information about a successfull recognition operation.
    /// </summary>
    /// <seealso cref="SpeechClient"/>
    public class SpeechRecognitionResponse
    {
        /// <summary>
        /// A string indicating the result status.  Successful requests will return "Success"
        /// </summary>
        public RecognitionStatus RecognitionStatus { get; set; }

        /// <summary>
        /// Gets or sets the offset.
        /// The Offset element specifies the offset (in 100-nanosecond units) at which the phrase was recognized, relative to the start of the audio stream
        /// </summary>
        public long Offset { get; set; }

        /// <summary>
        /// The duration of speech.
        /// The Duration element specifies the duration (in 100-nanosecond units) of this speech phrase.
        /// </summary>
        public long Duration { get; set; }

        private string displayText;
        /// <summary>
        /// Gets or sets the top result (by confidence), returned in Display Form.
        /// </summary>
        /// <remarks>The display form adds punctuation and capitalization to recognition results, making it the most appropriate form for applications that display the spoken text.</remarks>
        public string DisplayText
        {
            get => displayText ?? Alternatives?.FirstOrDefault()?.Display;
            set => displayText = value;
        }

        /// <summary>
        /// A list of alternative interpretations of the same speech recognition result. These results are ranked from most likely to least likely The first entry is the same as the main recognition result.
        /// </summary>
        [JsonProperty("NBest")]
        public IEnumerable<RecognitionAlternative> Alternatives { get; set; }
    }
}
