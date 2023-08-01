using Microsoft.AspNetCore.Components.Forms;

namespace WebBuilder2.Client.Utils;

public static class FileReader
{
    public static async Task<string> ReadFileAsync(IBrowserFile file)
    {
        var reader = await new StreamReader(file.OpenReadStream()).ReadToEndAsync();

        return reader;
    }
}
