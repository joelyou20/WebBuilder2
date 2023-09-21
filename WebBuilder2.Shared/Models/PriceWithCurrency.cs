using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models;

public class PriceWithCurrency
{
    public decimal Price { get; set; }
    public string Currency { get; set; } = string.Empty;
}
