using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using TranslatorService.Models.Translation;
using TranslatorService.Settings;

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

        private const int MaxArrayLengthForTranslation = 25;
        private const int MaxTextLengthForTranslation = 5000;
        private const int MaxArrayLengthForDetection = 100;
        private const int MaxTextLengthForDetection = 10000;

        private HttpClient httpClient = null!;
        private bool useInnerHttpClient = false;

        private ITokenProvider tokenProvider = null!;
        private string authorizationHeaderValue = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslatorClient"/> class, using the global (non region-dependent) service and the current language.
        /// </summary>
        /// <remarks>
        /// <para>You must register Microsoft Translator on https://portal.azure.com/#create/Microsoft.CognitiveServicesTextTranslation to obtain the Subscription key needed to use the service.</para>
        /// </remarks>
        /// <seealso cref="ITranslatorClient"/>
        public TranslatorClient()
            => Initialize(null, null, null, null, null);

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslatorClient"/> class, using an existing <see cref="System.Net.Http.HttpClient"/>, the global (non region-dependent) service and the current language.
        /// </summary>
        /// <param name="httpClient">An instance of the <see cref="HttpClient"/> object to use to network communication.</param>
        /// <remarks>
        /// <para>You must register Microsoft Translator on https://portal.azure.com/#create/Microsoft.CognitiveServicesTextTranslation to obtain the Subscription key needed to use the service.</para>
        /// </remarks>
        /// <seealso cref="ITranslatorClient"/>
        /// <seealso cref="HttpClient"/>
        public TranslatorClient(HttpClient httpClient)
            => Initialize(httpClient, null, null, null, null);

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslatorClient"/> class, using the global (non region-dependent) service and the current language.
        /// </summary>
        /// <param name="subscriptionKey">The Subscription Key to use the service.</param>
        /// <remarks>
        /// <para>You must register Microsoft Translator on https://portal.azure.com/#create/Microsoft.CognitiveServicesTextTranslation to obtain the Subscription key needed to use the service.</para>
        /// </remarks>
        /// <seealso cref="ITranslatorClient"/>
        public TranslatorClient(string subscriptionKey)
            => Initialize(null, subscriptionKey, null, null, null);

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslatorClient"/> class, using an existing <see cref="System.Net.Http.HttpClient"/>, the global (non region-dependent) service and the current language.
        /// </summary>
        /// <param name="httpClient">An instance of the <see cref="HttpClient"/> object to use to network communication.</param>
        /// <param name="subscriptionKey">The Subscription Key to use the service.</param>
        /// <remarks>
        /// <para>You must register Microsoft Translator on https://portal.azure.com/#create/Microsoft.CognitiveServicesTextTranslation to obtain the Subscription key needed to use the service.</para>
        /// </remarks>
        /// <seealso cref="ITranslatorClient"/>
        /// <seealso cref="HttpClient"/>
        public TranslatorClient(HttpClient httpClient, string subscriptionKey)
            => Initialize(httpClient, subscriptionKey, null, null, null);

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslatorClient"/> class for a specified region service, using the given language.
        /// </summary>
        /// <param name="subscriptionKey">The Subscription Key to use the service.</param>
        /// <param name="region">The Azure region of the the Speech service. This value is used to automatically set the <see cref="AuthenticationUri"/> property. If the <em>region</em> paramter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic), the global service is used.</param>
        /// <param name="language">The string representing the supported language code to translate the text in. The code must be present in the list of codes returned from the method <see cref="GetLanguagesAsync"/>. If the <em>language</em> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic), the current language is used.</param>
        /// <remarks>
        /// <para>You must register Microsoft Translator on https://portal.azure.com/#create/Microsoft.CognitiveServicesTextTranslation to obtain the Subscription key needed to use the service.</para>
        /// </remarks>
        /// <seealso cref="ITranslatorClient"/>
        public TranslatorClient(string subscriptionKey, string? region, string? language = null)
            => Initialize(null, subscriptionKey, region, language, null);

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslatorClient"/> class for a specified region service, using an existing <see cref="HttpClient"/> and the given language.
        /// </summary>
        /// <param name="httpClient">An instance of the <see cref="HttpClient"/> object to use to network communication.</param>
        /// <remarks>
        /// <para>You must register Microsoft Translator on https://portal.azure.com/#create/Microsoft.CognitiveServicesTextTranslation to obtain the Subscription key needed to use the service.</para>
        /// </remarks>
        /// <seealso cref="ITranslatorClient"/>
        /// <seealso cref="HttpClient"/>
        public TranslatorClient(HttpClient httpClient, TranslatorSettings settings, ITokenProvider tokenProvider)
            => Initialize(httpClient, settings.SubscriptionKey, settings.Region, settings.Language, tokenProvider);

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslatorClient"/> class for a specified region service, using an existing <see cref="HttpClient"/> and the given language.
        /// </summary>
        /// <param name="httpClient">An instance of the <see cref="HttpClient"/> object to use to network communication.</param>
        /// <param name="subscriptionKey">The Subscription Key to use the service.</param>
        /// <param name="region">The Azure region of the the Speech service. This value is used to automatically set the <see cref="AuthenticationUri"/> property. If the <em>region</em> paramter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic), the global service is used.</param>
        /// <param name="language">The string representing the supported language code to translate the text in. The code must be present in the list of codes returned from the method <see cref="GetLanguagesAsync"/>. If the <em>language</em> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic), the current language is used.</param>
        /// <remarks>
        /// <para>You must register Microsoft Translator on https://portal.azure.com/#create/Microsoft.CognitiveServicesTextTranslation to obtain the Subscription key needed to use the service.</para>
        /// </remarks>
        /// <seealso cref="ITranslatorClient"/>
        /// <seealso cref="HttpClient"/>
        public TranslatorClient(HttpClient httpClient, string subscriptionKey, string? region, string? language = null, ITokenProvider? tokenProvider = null)
            => Initialize(httpClient, subscriptionKey, region, language, tokenProvider);

        /// <inheritdoc/>
        public string? SubscriptionKey
        {
            get => tokenProvider.SubscriptionKey;
            set => tokenProvider.SubscriptionKey = value;
        }

        /// <inheritdoc/>
        public string? Region
        {
            get => tokenProvider.Region;
            set => tokenProvider.Region = value;
        }

        /// <inheritdoc/>
        public string Language { get; set; } = null!;

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
            using var request = CreateHttpRequest(uriString, HttpMethod.Post, input.Select(t => new
            {
                Text = t.Substring(0, Math.Min(t.Length, MaxTextLengthForDetection))
            }));

            using var response = await httpClient.SendAsync(request).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadFromJsonAsync<IEnumerable<DetectedLanguageResponse>>(JsonOptions.JsonSerializerOptions).ConfigureAwait(false);
                return responseContent!;
            }

            throw await TranslatorServiceException.ReadFromResponseAsync(response).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ServiceLanguage>> GetLanguagesAsync(string? language = null)
        {
            // Check if it is necessary to obtain/update access token.
            await CheckUpdateTokenAsync().ConfigureAwait(false);

            var uriString = $"{BaseUrl}languages?scope=translation&{ApiVersion}";
            using var request = CreateHttpRequest(uriString);

            language ??= Language;
            if (!string.IsNullOrWhiteSpace(language))
            {
                // If necessary, adds the Accept-Language header in order to get localized language names.
                request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(language));
            }

            using var response = await httpClient.SendAsync(request).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                using var jsonDocument = JsonDocument.Parse(contentStream);
                var jsonContent = jsonDocument.RootElement.GetProperty("translation");
                var responseContent = JsonSerializer.Deserialize<Dictionary<string, ServiceLanguage>>(jsonContent.ToString()!, JsonOptions.JsonSerializerOptions).ToList();
                responseContent.ForEach(r => r.Value.Code = r.Key);

                return responseContent.Select(r => r.Value).OrderBy(r => r.Name).ToList();
            }

            throw await TranslatorServiceException.ReadFromResponseAsync(response).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public Task<TranslationResponse> TranslateAsync(string input, string? to = null)
            => TranslateAsync(input, null, to ?? Language);

        /// <inheritdoc/>
        public async Task<TranslationResponse> TranslateAsync(string input, string? from, string to)
        {
            var response = await TranslateAsync(new string[] { input }, from, new string[] { to }).ConfigureAwait(false);
            return response.FirstOrDefault();
        }

        /// <inheritdoc/>
        public Task<IEnumerable<TranslationResponse>> TranslateAsync(IEnumerable<string> input, string? from, string to)
            => TranslateAsync(input, from, new string[] { to });

        /// <inheritdoc/>
        public Task<TranslationResponse> TranslateAsync(string input, IEnumerable<string> to)
            => TranslateAsync(input, null, to);

        /// <inheritdoc/>
        public async Task<TranslationResponse> TranslateAsync(string input, string? from, IEnumerable<string> to)
        {
            var response = await TranslateAsync(new string[] { input }, from, to).ConfigureAwait(false);
            return response.FirstOrDefault();
        }

        /// <inheritdoc/>
        public Task<IEnumerable<TranslationResponse>> TranslateAsync(IEnumerable<string> input, IEnumerable<string>? to = null)
            => TranslateAsync(input, null, to);

        /// <inheritdoc/>
        public async Task<IEnumerable<TranslationResponse>> TranslateAsync(IEnumerable<string> input, string? from, IEnumerable<string>? to)
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
            using var request = CreateHttpRequest(uriString, HttpMethod.Post, input.Select(t => new { Text = t }));

            using var response = await httpClient.SendAsync(request).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadFromJsonAsync<IEnumerable<TranslationResponse>>(JsonOptions.JsonSerializerOptions).ConfigureAwait(false);
                return responseContent!;
            }

            throw await TranslatorServiceException.ReadFromResponseAsync(response).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public Task InitializeAsync()
            => CheckUpdateTokenAsync();

        /// <inheritdoc/>

        public Task InitializeAsync(string? subscriptionKey, string? region, string? language = null)
            => InitializeAsync(null, region, subscriptionKey, language);

        /// <inheritdoc/>
        public Task InitializeAsync(HttpClient? httpClient, string? region, string? subscriptionKey, string? language = null)
        {
            Initialize(httpClient, subscriptionKey, region, language, null);
            return InitializeAsync();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (useInnerHttpClient)
            {
                httpClient.Dispose();
            }
        }

        private void Initialize(HttpClient? httpClient, string? subscriptionKey, string? region, string? language, ITokenProvider? tokenProvider)
        {
            if (httpClient == null)
            {
                this.httpClient = new HttpClient();
                useInnerHttpClient = true;
            }
            else
            {
                this.httpClient = httpClient;
                useInnerHttpClient = false;
            }

            this.tokenProvider = tokenProvider ?? new DefaultTokenProvider(this.httpClient, subscriptionKey, region);
            Language = language ?? CultureInfo.CurrentCulture.Name.ToLower();
        }

        private async Task CheckUpdateTokenAsync()
        {
            // If necessary, updates the access token.
            authorizationHeaderValue = await tokenProvider.GetAccessTokenAsync().ConfigureAwait(false);
        }

        private HttpRequestMessage CreateHttpRequest(string uriString)
            => CreateHttpRequest(uriString, HttpMethod.Get);

        private HttpRequestMessage CreateHttpRequest(string uriString, HttpMethod method, object? content = null)
        {
            var request = new HttpRequestMessage(method, new Uri(uriString))
            {
                Content = content != null ? JsonContent.Create(content, content.GetType(), options: JsonOptions.JsonSerializerOptions) : null
            };

            request.Headers.Add(Constants.AuthorizationHeader, authorizationHeaderValue);
            return request;
        }
    }
}