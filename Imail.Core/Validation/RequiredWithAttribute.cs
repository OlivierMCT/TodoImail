using System.ComponentModel.DataAnnotations;

namespace Imail.Core.Validation; 
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class RequiredWithAttribute(string otherPropertyName) : ValidationAttribute {
    private readonly string _otherPropertyName = otherPropertyName;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
        if (value != null) return ValidationResult.Success;

        var otherProperty = validationContext.ObjectType.GetProperty(_otherPropertyName) ?? throw new InvalidOperationException();
        var otherValue = otherProperty.GetValue(validationContext.ObjectInstance);

        return otherValue is null ? ValidationResult.Success : 
            new ValidationResult(ErrorMessage, [validationContext.MemberName+""]);
    }
}
