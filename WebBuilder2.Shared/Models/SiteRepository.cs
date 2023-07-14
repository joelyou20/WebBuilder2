using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models;

public class SiteRepository
{
    public Site Site { get; set; } = default!;
    public Repository Repository { get; set; } = default!;
}
