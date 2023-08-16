using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models.Projections;

public class AwsCreateBucketRequest
{
    public IEnumerable<Bucket> Buckets { get; set; } = Enumerable.Empty<Bucket>();
}
