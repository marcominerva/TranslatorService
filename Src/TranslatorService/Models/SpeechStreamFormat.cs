using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslatorService.Models
{
    /// <summary> 
    /// Specifies the audio format of the retrieved audio stream. 
    /// </summary> 
    public enum SpeechStreamFormat
    {
        /// <summary> 
        /// Uses the WAVE file format. 
        /// </summary> 
        Wave,
        /// <summary> 
        /// Uses the MP3 file format. 
        /// </summary> 
        Mp3
    }
}
