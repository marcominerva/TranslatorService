using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicrosoftTranslation
{
    public interface ITranslatorService
    {
        string SubscriptionKey { get; set; }

        string Language { get; set; }

        Task<string> DetectLanguageAsync(string text);

        Task<IEnumerable<string>> GetLanguagesAsync();

        Task InitializeAsync();

        Task<string> TranslateAsync(string text, string from, string to);

        Task<string> TranslateAsync(string text, string to = null);
    }
}