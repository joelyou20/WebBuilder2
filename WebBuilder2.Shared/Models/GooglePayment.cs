﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models;

public class GooglePayment
{
    public string Name { get; set; } = string.Empty;
    public DateTime? Date { get; set; }
    public string Amount { get; set; } = string.Empty;
}
