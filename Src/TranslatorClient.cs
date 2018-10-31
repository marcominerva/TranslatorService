using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TranslatorService.Models;

namespace TranslatorService
{
    /// <summary>
    /// The <strong>TranslatorClient</strong> class provides methods to translate text in various supported languages.
    /// </summary>
    /// <remarks>
    /// <para>To use this class, you must register Translator Service on https://portal.azure.com/#create/Microsoft.CognitiveServicesTextTranslation to obtain the Subscription key.
    /// </para>
    /// </remarks>
    public class TranslatorClient : ITranslatorClient
    {
        private const string BaseUrl = "https://api.cognitive.microsofttranslator.com/";
        private const string ApiVersion = "api-version=3.0";
        private const string AuthorizationHeader = "Authorization";
        private const string JsonMediaType = "application/json";

        private const int MaxArrayLengthForTranslation = 25;
        private const int MaxTextLengthForTranslation = 5000;
        private const int MaxArrayLengthForDetection = 100;
        private const int MaxTextLengthForDetection = 10000;

        private static HttpClient client = new HttpClient();
        private static TranslatorClient instance;

        /// <summary>
        /// Gets public singleton property.
        /// </summary>
        public static TranslatorClient Instance => instance ?? (instance = new TranslatorClient());

        private AzureAuthToken authToken;
        private string authorizationHeaderValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslatorClient"/> class.
        /// </summary>
        /// <param name="subscriptionKey">The Subscription Key to use the service.</param>
        /// <param name="language">The string representing the supported language code to translate the text in. The code must be present in the list of codes returned from the method <see cref="GetLanguagesAsync"/>.</param>
        /// <remarks>
        /// <para>You must register Microsoft Translator on https://portal.azure.com/#create/Microsoft.CognitiveServicesTextTranslation to obtain the Subscription key needed to use the service.</para>
        /// </remarks>
        /// <seealso cref="ITranslatorClient"/>
        public TranslatorClient(string subscriptionKey = null, string language = null)
        {
            Initialize(subscriptionKey, language);
        }

        /// <inheritdoc/>
        public string SubscriptionKey
        {
            get { return authToken.SubscriptionKey; }
            set { authToken.SubscriptionKey = value; }
        }

        /// <inheritdoc/>
        public string Language { get; set; }

        /// <inheritdoc/>
        public async Task<DetectedLanguageResponse> DetectLanguageAsync(string input)
        {
            var response = await DetectLanguagesAsync(new string[] { input }).ConfigureAwait(false);
            return response.FirstOrDefault();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<DetectedLanguageResponse>> DetectLanguagesAsync(IEnumerable<string> input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (!input.Any())
            {
                throw new ArgumentException($"{nameof(input)} array must contain at least 1 element");
            }

            if (input.Count() > MaxArrayLengthForDetection)
            {
                throw new ArgumentException($"{nameof(input)} array can have at most {MaxArrayLengthForDetection} elements");
            }

            // Checks if it is necessary to obtain/update access token.
            await CheckUpdateTokenAsync().ConfigureAwait(false);

            var uriString = $"{BaseUrl}detect?{ApiVersion}";
            using (var request = CreateHttpRequest(uriString, HttpMethod.Post, input.Select(t => new { Text = t.Substring(0, Math.Min(t.Length, MaxTextLengthForDetection)) })))
            {
                var response = await client.SendAsync(request).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var responseContent = JsonConvert.DeserializeObject<IEnumerable<DetectedLanguageResponse>>(content);
                return responseContent;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ServiceLanguage>> GetLanguagesAsync(string language = null)
        {
            // Check if it is necessary to obtain/update access token.
            await CheckUpdateTokenAsync().ConfigureAwait(false);

            var uriString = $"{BaseUrl}languages?scope=translation&{ApiVersion}";
            using (var request = CreateHttpRequest(uriString))
            {
                language = language ?? Language;
                if (!string.IsNullOrWhiteSpace(language))
                {
                    // If necessary, adds the Accept-Language header in order to get localized language names.
                    request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(language));
                }

                var response = await client.SendAsync(request).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var jsonContent = JToken.Parse(content)["translation"];
                var responseContent = JsonConvert.DeserializeObject<Dictionary<string, ServiceLanguage>>(jsonContent.ToString()).ToList();
                responseContent.ForEach(r => r.Value.Code = r.Key);

                return responseContent.Select(r => r.Value).OrderBy(r => r.Name).ToList();
            }
        }

        /// <inheritdoc/>
        public Task<TranslationResponse> TranslateAsync(string input, string to = null) => TranslateAsync(input, null, to ?? Language);

        /// <inheritdoc/>
        public async Task<TranslationResponse> TranslateAsync(string input, string from, string to)
        {
            var response = await TranslateAsync(new string[] { input }, from, new string[] { to }).ConfigureAwait(false);
            return response.FirstOrDefault();
        }

        /// <inheritdoc/>
        public Task<IEnumerable<TranslationResponse>> TranslateAsync(IEnumerable<string> input, string from, string to) => TranslateAsync(input, from, new string[] { to });

        /// <inheritdoc/>
        public Task<TranslationResponse> TranslateAsync(string input, IEnumerable<string> to) => TranslateAsync(input, null, to);

        /// <inheritdoc/>
        public async Task<TranslationResponse> TranslateAsync(string input, string from, IEnumerable<string> to)
        {
            var response = await TranslateAsync(new string[] { input }, from, to).ConfigureAwait(false);
            return response.FirstOrDefault();
        }

        /// <inheritdoc/>
        public Task<IEnumerable<TranslationResponse>> TranslateAsync(IEnumerable<string> input, IEnumerable<string> to = null) => TranslateAsync(input, null, to);

        /// <inheritdoc/>
        public async Task<IEnumerable<TranslationResponse>> TranslateAsync(IEnumerable<string> input, string from, IEnumerable<string> to)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (input.Count() > MaxArrayLengthForTranslation)
            {
                throw new ArgumentException($"{nameof(input)} array can have at most {MaxArrayLengthForTranslation} elements");
            }

            if (input.Any(str => string.IsNullOrWhiteSpace(str) || str.Length > MaxTextLengthForTranslation))
            {
                throw new ArgumentException($"Each sentence cannot be null or longer than {MaxTextLengthForTranslation} characters");
            }

            if (to == null || !to.Any())
            {
                to = new string[] { Language };
            }

            // Checks if it is necessary to obtain/update access token.
            await CheckUpdateTokenAsync().ConfigureAwait(false);

            var toQueryString = string.Join("&", to.Select(t => $"to={t}"));
            var uriString = (string.IsNullOrWhiteSpace(from) ? $"{BaseUrl}translate?{toQueryString}" : $"{BaseUrl}translate?from={from}&{toQueryString}") + $"&{ApiVersion}";
            using (var request = CreateHttpRequest(uriString, HttpMethod.Post, input.Select(t => new { Text = t })))
            {
                var response = await client.SendAsync(request).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var responseContent = JsonConvert.DeserializeObject<IEnumerable<TranslationResponse>>(content);
                return responseContent;
            }
        }

        /// <inheritdoc/>
        public Task InitializeAsync() => CheckUpdateTokenAsync();

        /// <inheritdoc/>
        public Task InitializeAsync(string subscriptionKey, string language = null)
        {
            Initialize(subscriptionKey, language);
            return InitializeAsync();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            authToken.Dispose();
            client.Dispose();
        }

        private void Initialize(string subscriptionKey, string language)
        {
            authToken = new AzureAuthToken(subscriptionKey);
            Language = language ?? CultureInfo.CurrentCulture.Name.ToLower();
        }

        private async Task CheckUpdateTokenAsync()
        {
            // If necessary, updates the access token.
            authorizationHeaderValue = await authToken.GetAccessTokenAsync().ConfigureAwait(false);
        }

        private HttpRequestMessage CreateHttpRequest(string uriString)
            => CreateHttpRequest(uriString, HttpMethod.Get);

        private HttpRequestMessage CreateHttpRequest(string uriString, HttpMethod method, object content = null)
        {
            var request = new HttpRequestMessage(method, new Uri(uriString));
            request.Headers.Add(AuthorizationHeader, authorizationHeaderValue);

            if (content != null)
            {
                var jsonRequest = JsonConvert.SerializeObject(content);
                var requestContent = new StringContent(jsonRequest, System.Text.Encoding.UTF8, JsonMediaType);
                request.Content = requestContent;
            }

            return request;
        }
    }
}