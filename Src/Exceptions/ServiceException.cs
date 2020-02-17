using Newtonsoft.Json.Linq;
using System;

namespace TranslatorService
{
    /// <summary>
    /// The <strong>TranslatorServiceException</strong> class holds information about Exception related to <see cref="TranslatorClient"/> or <see cref="SpeechClient"/>.
    /// </summary>
    /// <seealso cref="TranslatorClient"/>
    /// <seealso cref="SpeechClient"/>
    public class ServiceException : Exception
    {
        /// <summary>
        /// Gets or sets the error status code.
        /// </summary>
        public int Code { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceException"/> class using the specified error message.
        /// </summary>
        /// <param name="code">The status code of the error.</param>
        /// <param name="message">Message that describes the error.</param>
        public ServiceException(int code, string message)
            : base(message) => Code = code;

        internal static ServiceException FromJson(string json)
        {
            try
            {
                var error = JToken.Parse(json)["error"];
                return new ServiceException((int)error["code"], error["message"].ToString());
            }
            catch
            {
                return new ServiceException(500, "Unknown error");
            }
        }
    }
}
