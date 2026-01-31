using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace PSARModels;

public class PackageRead:IValidatableObject
{
    private readonly int row;

    public PackageRead(int row)
    {
        this.row = row;
        Year = "";
        Month = "";
        Values = [];
    }
    public StringShouldBeNumber Year { get; set; }
    public StringShouldBeNumber Month { get; set; }
    
    public ValuesPerLocalityRead Values { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        ValidationResult? validationResult;

        validationResult= null;
        
        StringShouldBeNumber.Validation(Year).Switch(
            val => validationResult = val,
            _ => { }
            );
        if (validationResult != null) yield return validationResult;

        StringShouldBeNumber.Validation(Month).Switch(
            val => validationResult = val,
            _ => { }
            );
        if (validationResult != null) yield return validationResult;
        var items=this.Values.Validate(new ValidationContext(this));
        foreach (var item in items) yield return item;

    }

}

public class PackagesList : List<PackageRead>, IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach(var item in this)
        {
            foreach (var vr in item.Validate(validationContext))
            {
                yield return vr;
            }
        }
    }
}

public record ProblemRow(int row, string problem);