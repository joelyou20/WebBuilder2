using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models.Projections;

public class GithubAuthenticationRequest
{
    public string PersonalAccessToken { get; set; } = string.Empty;

    public GithubAuthenticationRequest() { }

    public GithubAuthenticationRequest(string personalAccessToken)
    {
        PersonalAccessToken = personalAccessToken;
    }
}
