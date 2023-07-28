using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models.Projections;

public class GithubAuthenticationResponse
{
    public bool IsAuthenticated { get; set; } = false;

    public GithubAuthenticationResponse() { }

    public GithubAuthenticationResponse(bool isAuthenticated) { IsAuthenticated = isAuthenticated; }
}
