using WebBuilder2.Client.Models;

namespace WebBuilder2.Client.Utils.Providers.Contracts;

public interface IAuthenticationStateProvider
{
    void GoogleLogin(GoogleResponse googleResponse);
}
