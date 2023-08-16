using WebBuilder2.Client.Services;
using WebBuilder2.Shared.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using WebBuilder2.Shared.Validation;
using Microsoft.AspNetCore.Components;
using WebBuilder2.Client.Services.Contracts;

namespace WebBuilder2.Client.Components;

public partial class DomainSuggesterPanel
{
    [Inject] public IAwsService AwsService { get; set; } = default!;

    [Parameter] public Domain Value { get; set; } = new();
    [Parameter] public EventCallback<Domain> ValueChanged { get; set; } = new();

    private string _text = string.Empty;
    private List<DomainInquiry> _domains = new();
    private bool _loading;
    private List<ApiError> _errors = new();

    private async Task OnGetSuggestionsButtonClick()
    {
        _loading = true;
        StateHasChanged();
        ValidationResponse<DomainInquiry> result = await AwsService.GetSuggestedDomainNamesAsync(_text);

        if (!result.IsSuccessful)
        {
            _errors.AddRange(result.Errors);
        }

        _domains = result.GetValues();
        _loading = false;
        StateHasChanged();
    }

    private async Task OnSelectedOptionChanged(DomainInquiry domain)
    {
        if (domain == null) throw new ArgumentNullException("Selected domain value is null.");

        Value = new Domain { Name = domain.Name };
        await ValueChanged.InvokeAsync(Value);
    }
}
