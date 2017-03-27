using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslatorService.Models
{
    /// <summary> 
    /// Specifies the service for which to retrieve the supported languages. 
    /// </summary> 
    public enum LanguageServiceType
    {
        /// <summary> 
        /// Retrieves languages available for translation. 
        /// </summary> 
        Translation,
        /// <summary> 
        /// Retrieves languages available for speech.
        /// </summary> 
        Speech
    }
}
