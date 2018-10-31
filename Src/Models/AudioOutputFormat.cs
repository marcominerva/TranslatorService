namespace TranslatorService.Models
{
    /// <summary>
    /// Voice output formats.
    /// </summary>
    public enum AudioOutputFormat
    {
        /// <summary>
        /// raw-8khz-8bit-mono-mulaw request output audio format type.
        /// </summary>
        Raw8Khz8BitMonoMULaw,

        /// <summary>
        /// raw-16khz-16bit-mono-pcm request output audio format type.
        /// </summary>
        Raw16Khz16BitMonoPcm,

        /// <summary>
        /// riff-8khz-8bit-mono-mulaw request output audio format type.
        /// </summary>
        Riff8Khz8BitMonoMULaw,

        /// <summary>
        /// riff-16khz-16bit-mono-pcm request output audio format type.
        /// </summary>
        Riff16Khz16BitMonoPcm,

        /// <summary>
        /// ssml-16khz-16bit-mono-silk request output audio format type.
        /// It is a SSML with audio segment, with audio compressed by SILK codec
        /// </summary>
        Ssml16Khz16BitMonoSilk,

        /// <summary>
        /// raw-16khz-16bit-mono-truesilk request output audio format type.
        /// Audio compressed by SILK codec
        /// </summary>
        Raw16Khz16BitMonoTrueSilk,

        /// <summary>
        /// ssml-16khz-16bit-mono-tts request output audio format type.
        /// It is a SSML with audio segment, and it needs tts engine to play out
        /// </summary>
        Ssml16Khz16BitMonoTts,

        /// <summary>
        /// audio-16khz-128kbitrate-mono-mp3 request output audio format type.
        /// </summary>
        Audio16Khz128KBitRateMonoMp3,

        /// <summary>
        /// audio-16khz-64kbitrate-mono-mp3 request output audio format type.
        /// </summary>
        Audio16Khz64KBitRateMonoMp3,

        /// <summary>
        /// audio-16khz-32kbitrate-mono-mp3 request output audio format type.
        /// </summary>
        Audio16Khz32KBitRateMonoMp3,

        /// <summary>
        /// audio-16khz-16kbps-mono-siren request output audio format type.
        /// </summary>
        Audio16Khz16KbpsMonoSiren,

        /// <summary>
        /// riff-16khz-16kbps-mono-siren request output audio format type.
        /// </summary>
        Riff16Khz16KbpsMonoSiren,

        /// <summary>
        /// raw-24khz-16bit-mono-truesilk request output audio format type.
        /// </summary>
        Raw24Khz16BitMonoTrueSilk,

        /// <summary>
        /// raw-24khz-16bit-mono-pcm request output audio format type.
        /// </summary>
        Raw24Khz16BitMonoPcm,

        /// <summary>
        /// riff-24khz-16bit-mono-pcm request output audio format type.
        /// </summary>
        Riff24Khz16BitMonoPcm,

        /// <summary>
        /// audio-24khz-48kbitrate-mono-mp3 request output audio format type.
        /// </summary>
        Audio24Khz48KBitRateMonoMp3,

        /// <summary>
        /// audio-24khz-96kbitrate-mono-mp3 request output audio format type.
        /// </summary>
        Audio24Khz96KBitRateMonoMp3,

        /// <summary>
        /// audio-24khz-160kbitrate-mono-mp3 request output audio format type.
        /// </summary>
        Audio24Khz160KBitRateMonoMp3
    }
}