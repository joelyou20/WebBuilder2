using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models.Projections;

public class AwsNewSSLCertificateRequest
{
    public string DomainName { get; set; } = string.Empty;
    public List<string> AlternativeNames { get; set; } = new List<string>();
}
