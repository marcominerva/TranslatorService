using System.Collections.Generic;
using System.Threading.Tasks;

namespace BingTranslation
{
    public interface ITranslatorService
    {
        string ClientId { get; set; }

        string ClientSecret { get; set; }

        string Language { get; set; }

        Task<string> DetectLanguageAsync(string text);

        Task<IEnumerable<string>> GetLanguagesAsync();

        Task InitializeAsync();

        Task<string> TranslateAsync(string text, string from, string to);

        Task<string> TranslateAsync(string text, string to = null);

        bool IsInitialized { get; }
    }
}