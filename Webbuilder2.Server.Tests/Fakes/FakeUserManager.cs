using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBuilder2.Shared.Models;

namespace Webbuilder2.Server.Tests.Fakes;

public class FakeUserManager : UserManager<ApplicationUser>
{
    public FakeUserManager()
        : base(new Mock<IUserStore<ApplicationUser>>().Object,
              new Mock<IOptions<IdentityOptions>>().Object,
              new Mock<IPasswordHasher<ApplicationUser>>().Object,
              Array.Empty<IUserValidator<ApplicationUser>>(),
              Array.Empty<IPasswordValidator<ApplicationUser>>(),
              new Mock<ILookupNormalizer>().Object,
              new Mock<IdentityErrorDescriber>().Object,
              new Mock<IServiceProvider>().Object,
              new Mock<ILogger<UserManager<ApplicationUser>>>().Object)
    { }

}
