using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models;

public class NewFile
{
    public string Path { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public FileType FileType { get; set; }
    public bool IsImage { get; set; } = false;
    public string Mode => FileType switch
    {
        FileType.File => System.IO.Path.GetExtension(Path).Equals(".exe", StringComparison.InvariantCultureIgnoreCase) ? "100755" : "100644",
        FileType.Directory => "040000",
        FileType.Symlink => "120000",
        FileType.Submodule => "160000",
        _ => throw new NotImplementedException()
    };
}
