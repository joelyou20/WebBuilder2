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
        public bool ConfigureForWebsiteHosting { get; set; } = false;
        public bool ConfigureForLogging { get; set; } = false;
        public string? RedirectTarget { get; set; }

        public Bucket() { }

        public Bucket(string name, Region region, bool configureForWebSiteHosting = false, bool configureForLogging = false, string? redirectTarget = null)
        {
            Name = name;
            Region = region;
            ConfigureForWebsiteHosting = configureForWebSiteHosting;
            ConfigureForLogging = configureForLogging;
            RedirectTarget = redirectTarget;
        }
    }
}
