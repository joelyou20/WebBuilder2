using Microsoft.AspNetCore.Components.Forms;

namespace WebBuilder2.Client.Utils;

public static class FileReader
{
    public static async Task<string> ReadFileAsync(IBrowserFile file)
    {
        var stream = file.OpenReadStream(file.Size);
        var bytes = new byte[file.Size]; 
        
        await stream.ReadAsync(bytes);

        return Convert.ToBase64String(bytes);
    }
}
