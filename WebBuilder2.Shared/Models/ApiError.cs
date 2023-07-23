﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models;

public class ApiError
{
    public string Message { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Resource { get; set; } = string.Empty;
    public string Field { get; set; } = string.Empty; 
    public Exception? Exception { get; set; } 
}