using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslatorService.Models
{
    /// <summary> 
    /// Specifies the audio quality of the retrieved audio stream. 
    /// </summary> 
    public enum SpeechStreamQuality
    {
        /// <summary> 
        /// Uses the max available quality. 
        /// </summary> 
        MaxQuality,
        /// <summary> 
        /// Retrieves audio file with the minimum size. 
        /// </summary> 
        MinSize
    }
}
