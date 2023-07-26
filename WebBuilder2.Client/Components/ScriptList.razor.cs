using Microsoft.AspNetCore.Components;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Components;

public partial class ScriptList
{
    [Parameter] public List<ScriptModel> Scripts { get; set; } = new();
}
