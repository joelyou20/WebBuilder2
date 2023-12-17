using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Shared.Models.Projections;

public class AwsNewSSLCertificateRequest
{
    [SimpleUrlValidator]
    public string DomainName { get; set; } = string.Empty;
    public List<string> AlternativeNames { get; set; } = new List<string>();
}
