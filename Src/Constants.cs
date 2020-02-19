namespace TranslatorService
{
    internal static class Constants
    {
        public const string GlobalAuthorizationUrl = "https://api.cognitive.microsoft.com/sts/v1.0/issueToken";
        public const string RegionAuthorizationUrl = "https://{0}.api.cognitive.microsoft.com/sts/v1.0/issueToken";

        public const string AuthorizationHeader = "Authorization";

        public const string JsonMediaType = "application/json";
        public const string WavAudioMediaType = "audio/wav";
    }
}
