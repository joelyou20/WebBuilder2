using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using WebBuilder2.Shared.Models;

namespace Webbuilder2.Server.Tests.Fakes;

public class FakeSignInManager : SignInManager<ApplicationUser>
{
    public FakeSignInManager()
        : base(new Mock<FakeUserManager>().Object,
              new HttpContextAccessor(),
              new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
              new Mock<IOptions<IdentityOptions>>().Object,
              new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
              new Mock<IAuthenticationSchemeProvider>().Object,
              new Mock<IUserConfirmation<ApplicationUser>>().Object)
    { }
}
