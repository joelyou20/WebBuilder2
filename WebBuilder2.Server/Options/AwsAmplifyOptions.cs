namespace WebBuilder2.Server.Options
{
    public class AwsAmplifyOptions
    {
        public string AccessToken { get; set; } = string.Empty;
    }

    public static class AwsAmplifyOptionsStore
    {
        public static string AccessToken { get; } = $"{nameof(AwsAmplifyOptions)}:{nameof(AwsAmplifyOptions.AccessToken)}";
    }
}
