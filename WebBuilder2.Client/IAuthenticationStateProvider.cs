using WebBuilder2.Client.Models;

namespace WebBuilder2.Client;

public interface IAuthenticationStateProvider
{
    void GoogleLogin(GoogleResponse googleResponse);
}
