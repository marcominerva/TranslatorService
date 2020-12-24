using System.Collections.Generic;

namespace TranslatorService.Models.Speech
{
    /// <summary>
    /// Inputs Options for the TTS Service.
    /// </summary>
    public class TextToSpeechParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextToSpeechParameters"/> class.
        /// </summary>
        /// <seealso cref="SpeechClient"/>
        public TextToSpeechParameters()
        {
            Language = "en-us";
            VoiceName = "en-US-AriaNeural";
            VoiceType = Gender.Female;
            // Default to Riff16Khz16BitMonoPcm output format.
            OutputFormat = AudioOutputFormat.Riff16Khz16BitMonoPcm;
        }

        /// <summary>
        /// Gets or sets the audio output format.
        /// </summary>
        public AudioOutputFormat OutputFormat { get; init; }

        /// <summary>
        /// Gets or sets the language of the text.
        /// </summary>
        public string Language { get; init; }

        /// <summary>
        /// Gets or sets the type of the voice: male/female.
        /// </summary>
        public Gender VoiceType { get; init; }

        /// <summary>
        /// Gets or sets the name of the voice.
        /// </summary>
        /// <remarks>Voices list is available at https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/supported-languages#text-to-speech.
        /// </remarks>
        public string VoiceName { get; init; }

        /// <summary>
        /// Gets or sets the text to speech.
        /// </summary>
        public string Text { get; init; } = string.Empty;

        /// <summary>
        /// Gets or sets the headers.
        /// </summary>
        internal IEnumerable<KeyValuePair<string, string>> Headers
        {
            get
            {
                var toReturn = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("Content-Type", "application/ssml+xml")
                };

                var outputFormat = OutputFormat switch
                {
                    AudioOutputFormat.Raw16Khz16BitMonoPcm => "raw-16khz-16bit-mono-pcm",
                    AudioOutputFormat.Raw8Khz8BitMonoMULaw => "raw-8khz-8bit-mono-mulaw",
                    AudioOutputFormat.Riff16Khz16BitMonoPcm => "riff-16khz-16bit-mono-pcm",
                    AudioOutputFormat.Riff8Khz8BitMonoMULaw => "riff-8khz-8bit-mono-mulaw",
                    AudioOutputFormat.Ssml16Khz16BitMonoSilk => "ssml-16khz-16bit-mono-silk",
                    AudioOutputFormat.Raw16Khz16BitMonoTrueSilk => "raw-16khz-16bit-mono-truesilk",
                    AudioOutputFormat.Ssml16Khz16BitMonoTts => "ssml-16khz-16bit-mono-tts",
                    AudioOutputFormat.Audio16Khz128KBitRateMonoMp3 => "audio-16khz-128kbitrate-mono-mp3",
                    AudioOutputFormat.Audio16Khz64KBitRateMonoMp3 => "audio-16khz-64kbitrate-mono-mp3",
                    AudioOutputFormat.Audio16Khz32KBitRateMonoMp3 => "audio-16khz-32kbitrate-mono-mp3",
                    AudioOutputFormat.Audio16Khz16KbpsMonoSiren => "audio-16khz-16kbps-mono-siren",
                    AudioOutputFormat.Riff16Khz16KbpsMonoSiren => "riff-16khz-16kbps-mono-siren",
                    AudioOutputFormat.Raw24Khz16BitMonoPcm => "raw-24khz-16bit-mono-pcm",
                    AudioOutputFormat.Riff24Khz16BitMonoPcm => "riff-24khz-16bit-mono-pcm",
                    AudioOutputFormat.Audio24Khz48KBitRateMonoMp3 => "audio-24khz-48kbitrate-mono-mp3",
                    AudioOutputFormat.Audio24Khz96KBitRateMonoMp3 => "audio-24khz-96kbitrate-mono-mp3",
                    AudioOutputFormat.Audio24Khz160KBitRateMonoMp3 => "audio-24khz-160kbitrate-mono-mp3",
                    _ => "riff-16khz-16bit-mono-pcm",
                };

                toReturn.Add(new KeyValuePair<string, string>("X-Microsoft-OutputFormat", outputFormat));
                // The software originating the request
                toReturn.Add(new KeyValuePair<string, string>("User-Agent", "TranslatorService.SpeechClient"));

                return toReturn;
            }
            init
            {
                Headers = value;
            }
        }
    }
}