using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models;

public class RepoContent
{
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsExpanded { get; set; } = false;
    public FileType FileType { get; set; }

    public RepoContent() { }
    public RepoContent(string name, string path, string content, FileType fileType)
    {
        Name = name;
        Path = path;
        Content = content;
        FileType = fileType;
    }
}
