using Microsoft.AspNetCore.Components;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Components;

public partial class ErrorContainer
{
    [Parameter] public IEnumerable<ApiError> Errors { get; set; } = Enumerable.Empty<ApiError>();
}
