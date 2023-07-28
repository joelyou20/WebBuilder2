using System.ComponentModel.DataAnnotations;

namespace WebBuilder2.Shared.Validation;

public class GithubMergeStrategyValidator : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        bool booleanValue = value is bool;

        return booleanValue ? null : new ValidationResult($"One merge strategy must be selected.", new[] { validationContext.MemberName }!);
    }
}