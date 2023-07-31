using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models;

public class GitTreeItem
{
    public string Path { get; set; } = string.Empty;
    public string Sha { get; set; } = string.Empty;
    public bool IsExpanded { get; set; } = false;
    public string Mode { get; set; } = string.Empty;
    public GitTreeType Type { get; set; }
    public FileExtension Extension { get; set; }
    public IEnumerable<GitTreeItem>? Items { get; set; } = Enumerable.Empty<GitTreeItem>();

    public GitTreeItem() { }

    public GitTreeItem(string path, string sha, string mode, FileExtension extension, GitTreeType type, IEnumerable<GitTreeItem>? items = null)
    {
        Path = path;
        Sha = sha;
        Mode = mode;
        Extension = extension;
        Type = type;
        Items = items;
    }
}
