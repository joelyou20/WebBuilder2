﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models.Projections;

public class AwsAddBucketPolicyRequest
{
    public Bucket Bucket { get; set; } = new();
    public string Policy { get; set; } = string.Empty;
}
