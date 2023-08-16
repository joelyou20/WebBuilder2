using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models.Projections;

public class AwsConfigureLoggingRequest
{
    public Bucket Bucket { get; set; } = new();
    public Bucket LogBucket { get; set; } = new();
    public string LogObjectKeyPrefix { get; set; } = string.Empty;
}
