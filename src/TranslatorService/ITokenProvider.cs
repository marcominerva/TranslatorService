using System.Threading.Tasks;

namespace TranslatorService
{
    public interface ITokenProvider
    {
        string? Region { get; set; }

        string? SubscriptionKey { get; set; }

        Task<string> GetAccessTokenAsync();
    }
}