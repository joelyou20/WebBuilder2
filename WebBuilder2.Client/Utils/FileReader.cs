using Microsoft.AspNetCore.Components.Forms;

namespace WebBuilder2.Client.Utils;

public static class FileHelper
{
    public static string ReadFile(string path)
    {
        string content = CheckIfFileIsImage(path) ? Convert.ToBase64String(File.ReadAllBytes(path)) : File.ReadAllText(path);
        return content;
    }

    public static async Task<string> ReadFileAsync(IBrowserFile file)
    {
        var reader = await new StreamReader(file.OpenReadStream()).ReadToEndAsync();

        return reader;
    }

    public static async Task WriteFileAsync(string path, string content)
    {
        var writer = new StreamWriter(path);
        await writer.WriteAsync(content);
    }

    private static bool CheckIfFileIsImage(string? extension) =>
        extension != null && (
        extension.ToLower().Equals(".jpg") ||
        extension.ToLower().Equals(".png"));
}
