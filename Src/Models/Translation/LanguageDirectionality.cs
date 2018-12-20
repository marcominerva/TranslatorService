using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
