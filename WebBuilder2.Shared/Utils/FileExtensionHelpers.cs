using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Shared.Utils;

public static class FileExtensionHelpers
{
    public static FileExtension GetFileExtensionFromPath(string path)
    {
        var extension = Path.GetExtension(path);
        return extension switch
        {
            ".pdf" => FileExtension.PDF,
            ".png" => FileExtension.PNG,
            ".jpg" => FileExtension.JPG,
            ".jpeg" => FileExtension.JPEG,
            ".cs" => FileExtension.CSharp,
            ".mp3" => FileExtension.MP3,
            ".json" => FileExtension.Json,
            ".html" => FileExtension.Html,
            _ => FileExtension.Text
        };
    }
}
