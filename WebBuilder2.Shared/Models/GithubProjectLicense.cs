using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models;

public class GithubProjectLicense
{
    public string Name { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public bool Featured { get; set; } = false;
    public string Url { get; set; } = string.Empty;
}
