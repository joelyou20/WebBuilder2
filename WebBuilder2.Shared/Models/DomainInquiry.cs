using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models;

public class DomainInquiry
{
    public string Name { get; set; } = string.Empty;
    public PriceWithCurrency Price { get; set; } = new();
    public DomainAvailability Availability { get; set; }
}
