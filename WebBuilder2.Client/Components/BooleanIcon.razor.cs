using Microsoft.AspNetCore.Components;

namespace WebBuilder2.Client.Components;

public partial class BooleanIcon
{
    [Parameter] public bool Value { get; set; } = false;
}
