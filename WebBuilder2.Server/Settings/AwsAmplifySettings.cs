namespace WebBuilder2.Server.Settings
{
    public class AwsAmplifySettings
    {
        public string AccessToken { get; set; } = string.Empty;
    }

    public static class AwsAmplifySettingsStore
    {
        public static string AccessToken { get; } = $"{nameof(AwsAmplifySettings)}:{nameof(AwsAmplifySettings.AccessToken)}";
    }
}
