using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models;

public class ApplicationUser : IdentityUser
{
    public ApplicationUser() { }

    public ApplicationUser(string userName, string passwordHash)
    {
        UserName = userName;
        PasswordHash = passwordHash;
    }
}
