using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslatorService.Models.Speech
{
    /// <summary>
    /// The <strong>SpeechProfanityMode</strong> enum defines the possible modes in which the service can handle profanity.
    /// </summary>
    public enum SpeechProfanityMode
    {
        /// <summary>
        /// The service masks profanity with an asterisk. This is the default.
        /// </summary>
        Masked,

        /// <summary>
        /// The service removes profanity.
        /// </summary>
        Removed,

        /// <summary>
        /// The service does not remove or mask profanity.
        /// </summary>
        Raw
    }
}
