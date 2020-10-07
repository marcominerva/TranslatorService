using System.Text.Json;
using System.Text.Json.Serialization;

namespace TranslatorService
{
    internal static class JsonOptions
    {
        public static JsonSerializerOptions JsonSerializerOptions { get; }

        static JsonOptions()
        {
            JsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };

            JsonSerializerOptions.Converters.Add(new StringEnumMemberConverter());
        }
    }
}
