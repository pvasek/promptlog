namespace PromptLog.App.Services
{
    public class AppConfiguration
    {
        public string OpenaiApiBase { get; init; } = "https://api.openai.com/";
        public string ProxySecret { get; init; } = "empty";
        public bool AllowHttp { get; init; } = false;
    }
};