using Blace.Editing;

namespace WebBuilder2.Client.Utils;

public static class FileExtensionHelper
{
    public static Syntax GetSyntax(string fileName)
    {
        var extension = Path.GetExtension(fileName);
        return extension switch
        {
            ".cs" => Syntax.Csharp,
            ".yaml" => Syntax.Yaml,
            ".css" => Syntax.Css,
            ".js" => Syntax.Javascript,
            ".ps1" => Syntax.Powershell,
            ".md" => Syntax.Markdown,
            _ => Syntax.Text,
        };
    }
}
