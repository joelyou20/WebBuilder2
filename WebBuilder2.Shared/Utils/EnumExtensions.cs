using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Shared.Utils;

public static class EnumExtensions
{
    public static string EnumValue(this Region e) => e switch
    {
        Region.USEast1 => "us-east-1",
        Region.USEast2 => "us-east-2",
        Region.USWest1 => "us-west-1",
        Region.USWest2 => "us-west-2",
        _ => "Region not supported"
    };
}
