using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models
{
    public class Bucket
    {
        // TODO: Enforce AWS bucket naming rules https://docs.aws.amazon.com/AmazonS3/latest/userguide/bucketnamingrules.html
        public string Name { get; set; } = string.Empty;
        public Region Region { get; set; }

        public Bucket() { }

        public Bucket(string name, Region region)
        {
            Name = name;
            Region = region;
        }
    }
}
