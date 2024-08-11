using Microsoft.AspNetCore.Mvc;

namespace WebBuilder2.Server.Controllers
{
    public class CustomControllerBase : ControllerBase
    {
        public void ValidateRequest<T>(IEnumerable<T> request) where T : class
        {
            ArgumentNullException.ThrowIfNull(request);

            if (!request.Any()) throw new Exception($"{nameof(request)} is empty");
        }
    }
}
