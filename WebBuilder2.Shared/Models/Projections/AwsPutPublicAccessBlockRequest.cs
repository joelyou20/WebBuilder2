using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models.Projections;

public class AwsPublicAccessBlockRequest
{
    public Bucket Bucket { get; set; } = new();
    public bool BlockPublicAcls { get; set; } = true;
}
