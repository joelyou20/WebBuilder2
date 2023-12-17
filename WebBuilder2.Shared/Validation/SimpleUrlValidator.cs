using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace WebBuilder2.Shared.Validation;

public partial class SimpleUrlValidator : ValidationAttribute
{ 
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if(value?.GetType() != typeof(string) ) return new ValidationResult("Value is not a string", new List<string> { validationContext.MemberName! });

        Regex regex = UrlRegex();

        string? url = value.ToString();

        if(string.IsNullOrEmpty(url)) return new ValidationResult("Value is empty", new List<string> { validationContext.MemberName! });

        if (regex.IsMatch(url.ToString()))
        {
            return null;
        }

        return new ValidationResult($"URL must be of format www.example.com", new List<string> { validationContext.MemberName! });
    }

    [GeneratedRegex("^www\\..*\\.com$")]
    private static partial Regex UrlRegex();
}
