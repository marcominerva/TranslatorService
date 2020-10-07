using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

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

        internal static async Task<ServiceException> FromResponseAsync(HttpResponseMessage response)
        {
            using var responseStream = await response.Content.ReadAsStreamAsync();

            try
            {
                using var jsonDocument = await JsonDocument.ParseAsync(responseStream);
                var error = jsonDocument.RootElement.GetProperty("error");
                return new ServiceException(Convert.ToInt32(error.GetProperty("code").GetString()), error.GetProperty("message").GetString());
            }
            catch
            {
                responseStream.Position = 0;
                using var reader = new StreamReader(responseStream);
                return new ServiceException(500, await reader.ReadToEndAsync() ?? "Unknown error");
            }
        }
    }
}
