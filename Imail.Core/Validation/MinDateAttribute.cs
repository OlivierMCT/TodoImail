using System.ComponentModel.DataAnnotations;

namespace Imail.Core.Validation; 
[AttributeUsage(AttributeTargets.Property,  AllowMultiple = false)]
public class MinDateAttribute(string strDate) : ValidationAttribute {
    public const string Today = "today";
    public const string Now = "now";

    private readonly DateTime _dateToCompare = strDate switch {
        Today => DateTime.Today,
        Now => DateTime.Now,
        _ => DateTime.Parse(strDate),
    };

    public override bool IsValid(object? value) {
        if (value is null) return true;

        DateTime? dateValue = null;
        if(value is DateTime date) dateValue = date;
        else if(value is DateOnly dateonly) dateValue = dateonly.ToDateTime(TimeOnly.MinValue);
        if (dateValue is null) throw new InvalidOperationException();

        return dateValue >= _dateToCompare;
    }
}
