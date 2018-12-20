using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslatorService.Models.Speech
{
    /// <summary>
    /// The <strong>RecognitionResultFormat</strong> enum specifies the result format.
    /// </summary>
    public enum RecognitionResultFormat
    {
        /// <summary>
        /// The service returns only the top level information of the recognized text. This is the default.
        /// </summary>
        Simple,

        /// <summary>
        /// The service returns detailed recognition result.
        /// </summary>
        Detailed,
    }
}
