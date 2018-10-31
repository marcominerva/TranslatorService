using System;

namespace TranslatorService.Models
{
    /// <summary>
    /// The <strong>TranslatorServiceException</strong> class holds information about Exception related to <see cref="TranslatorClient"/> or <see cref="SpeechClient"/>.
    /// </summary>
    /// <seealso cref="TranslatorClient"/>
    public class ServiceException : Exception
    {
        /// <summary>
        /// Gets or sets the HTTP status code.
        /// </summary>
        public int StatusCode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceException"/> class using the specified error message.
        /// </summary>
        /// <param name="message">Message that describes the error.</param>
        /// <param name="statusCode">The HTTP status code of the error.</param>
        public ServiceException(string message, int statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
