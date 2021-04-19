using Microsoft.Extensions.Caching.Memory;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TranslatorService.Settings;

namespace TranslatorService
{
    /// <summary>
    /// Client to call Cognitive Services Azure Auth Token service in order to get an access token.
    /// </summary>
    public class CacheableTokenProvider : ITokenProvider
    {
        /// <summary>
        /// Name of header used to pass the subscription key to the token service
        /// </summary>
        private const string OcpApimSubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";
        private const string OcpApimSubscriptionRegionHeader = "Ocp-Apim-Subscription-Region";

        private const string GlobalAuthorizationUrl = "https://api.cognitive.microsoft.com/sts/v1.0/issueToken";
        private const string RegionAuthorizationUrl = "https://{0}.api.cognitive.microsoft.com/sts/v1.0/issueToken";

        /// Gets or sets the URL of the token service.
        public Uri ServiceUrl { get; set; }

        public string? Region { get; set; }

        /// <summary>
        /// After obtaining a valid token, this class will cache it for this duration.
        /// Use a duration of 8 minutes, which is less than the actual token lifetime of 10 minutes.
        /// </summary>
        private static readonly TimeSpan TokenCacheDuration = new TimeSpan(0, 8, 0);

        private readonly IMemoryCache cache;
        private readonly HttpClient httpClient;

        private string storedTokenValue = string.Empty;
        private string? subscriptionKey;

        /// <summary>
        /// Gets or sets the Service Subscription Key.
        /// </summary>
        public string? SubscriptionKey
        {
            get
            {
                return subscriptionKey;
            }
            set
            {
                if (subscriptionKey != value)
                {
                    // If the subscription key is changed, the token is no longer valid.
                    subscriptionKey = value;
                    storedTokenValue = string.Empty;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultTokenProvider"/> class, that is used to obtain access token
        /// </summary>
        /// <param name="httpClient">The instance of <see cref="HttpClient"/> used by the service.</param>
        public CacheableTokenProvider(HttpClient httpClient, TranslatorSettings settings, IMemoryCache cache)
            : this(httpClient, settings.SubscriptionKey, settings.Region, cache)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultTokenProvider"/> class, that is used to obtain access token
        /// </summary>
        /// <param name="httpClient">The instance of <see cref="HttpClient"/> used by the service.</param>
        /// <param name="subscriptionKey">Subscription key to use to get an authentication token.</param>
        /// <param name="region">The Azure region of the the Translator service, if any.</param>
        /// <exception cref="ArgumentNullException">The <em>serviceUrl</em> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic) or empty.</exception>
        public CacheableTokenProvider(HttpClient httpClient, string? subscriptionKey, string? region = null, IMemoryCache cache = null)
        {
            this.httpClient = httpClient;
            SubscriptionKey = subscriptionKey;
            ServiceUrl = new Uri(!string.IsNullOrWhiteSpace(region) ? string.Format(RegionAuthorizationUrl, region) : GlobalAuthorizationUrl);
            Region = region;
            this.cache = cache;
        }

        /// <summary>
        /// Gets a token for the specified subscription.
        /// </summary>
        /// <returns>The encoded JWT token prefixed with the string "Bearer ".</returns>
        /// <remarks>
        /// This method uses a cache to limit the number of request to the token service.
        /// A fresh token can be re-used during its lifetime of 10 minutes. After a successful
        /// request to the token service, this method caches the access token. Subsequent
        /// invocations of the method return the cached token for the next 8 minutes. After
        /// 8 minutes, a new token is fetched from the token service and the cache is updated.
        /// </remarks>
        public async Task<string> GetAccessTokenAsync()
        {
            if (string.IsNullOrWhiteSpace(subscriptionKey))
            {
                throw new ArgumentNullException(nameof(SubscriptionKey), "A subscription key is required. Go to Azure Portal and sign up for Microsoft Translator: https://portal.azure.com/#create/Microsoft.CognitiveServices/apitype/TextTranslation");
            }

            storedTokenValue = await cache.GetOrCreateAsync(subscriptionKey, async (entry) =>
            {
                try
                {
                    using var request = new HttpRequestMessage(HttpMethod.Post, ServiceUrl);
                    request.Headers.Add(OcpApimSubscriptionKeyHeader, SubscriptionKey);
                    request.Headers.Add(OcpApimSubscriptionRegionHeader, Region);

                    using var response = await httpClient.SendAsync(request).ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        storedTokenValue = $"Bearer {content}";

                        entry.AbsoluteExpirationRelativeToNow = TokenCacheDuration;
                        return storedTokenValue;
                    }

                    throw await TranslatorServiceException.ReadFromResponseAsync(response).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    throw new TranslatorServiceException(500, ex.Message);
                }
            });

            return storedTokenValue;
        }
    }
}
