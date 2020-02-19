using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace TranslatorService.Models.Translation
{
    /// <summary>
    /// Language directionality, which is rtl for right-to-left languages or ltr for left-to-right languages.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
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
