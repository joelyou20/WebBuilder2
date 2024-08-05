using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using WebBuilder2.Client.Managers.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Managers
{
    public class CustomUserManager<T>(
        IUserStore<T> store, 
        IOptions<IdentityOptions> optionsAccessor, 
        IPasswordHasher<T> passwordHasher, 
        IEnumerable<IUserValidator<T>> userValidators, 
        IEnumerable<IPasswordValidator<T>> passwordValidators, 
        ILookupNormalizer keyNormalizer, 
        IdentityErrorDescriber errors, 
        IServiceProvider services, 
        ILogger<UserManager<T>> logger) : UserManager<T>(
            store, 
            optionsAccessor, 
            passwordHasher, 
            userValidators, 
            passwordValidators, 
            keyNormalizer, 
            errors, 
            services, 
            logger), ICustomUserManager where T : class
    {
    }
}
