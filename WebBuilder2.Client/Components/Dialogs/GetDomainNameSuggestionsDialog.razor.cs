using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Components.Dialogs;

public partial class GetDomainNameSuggestionsDialog
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = default!;
}
