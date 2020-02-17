using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace TranslatorService.Models.Translation
{
    /// <summary>
    /// Language directionality, which is rtl for right-to-left languages or ltr for left-to-right languages.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum LanguageDirectionality
    {
        /// <summary>
        /// Left to right language
        /// </summary>
        [EnumMember(Value = "ltr")]
        LeftToRight,

        /// <summary>
        /// Right to left language
        /// </summary>
        [EnumMember(Value = "rtl")]
        RightToLeft
    }
}
