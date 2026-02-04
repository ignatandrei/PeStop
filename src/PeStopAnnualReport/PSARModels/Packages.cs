using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PSARModels;

public record Package(int year, int month, ValuesPerLocalityInt Values);
[DebuggerDisplay("{ToDebugString}")]
public class PackageRead:IValidatableObject
{
    private readonly int row;

    private string ToDebugString
    {
        get
        {
            return $"Row {row} {Year} {Month}   {Values}";
        }
    }
    public PackageRead(int row)
    {
        this.row = row;
        Year = "";
        Month = "";
        Values = [];
    }
    public StringShouldBeNumber Year { get; set; }
    public StringShouldBeMonth Month { get; set; }
    
    public ValuesPerLocalityRead Values { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        string[] members = ["row :" + row];
        ValidationResult? validationResult;

        validationResult= null;
        
        StringShouldBeNumber.Validation(Year).Switch(
            val => validationResult = val,
            _ => { }
            );
        if (validationResult != null) yield return new ValidationResult(validationResult.ErrorMessage,members)  ;
        validationResult = null;
        StringShouldBeMonth.Validation(Month).Switch(
            val => validationResult = val,
            _ => { }
            );
        if (validationResult != null) yield return new ValidationResult(validationResult.ErrorMessage,members)  ;
        validationResult = null;
        var items=this.Values.Validate(new ValidationContext(this));
        foreach (var item in items) yield return new ValidationResult(item.ErrorMessage, members) ;

    }

}

public class PackagesList : List<PackageRead>, IValidatableObject
{
    //if does not have month and year
    private Predicate<PackageRead> Empty = (p) =>
    {
        if (p == null) return true;
        if (p.Year == null) return true;
        if (p.Month == null) return true;
        var valYear = p.Year.Value?.ToString()?.Trim();
        var valMonth = p.Month.Value?.ToString()?.Trim();
        return valYear?.Length == 0 && valMonth?.Length == 0;
    };
    public void RemoveEmpty()
    {
        //var nrEmpty = this.Where(it=>Empty(it)).ToArray();
        this.RemoveAll(Empty);
    }
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        this.RemoveEmpty();
        foreach(var item in this)
        {
            foreach (var vr in item.Validate(validationContext))
            {
                yield return vr;
            }
        }
    }
    public Package[] ValidPackages()
    {
        this.RemoveEmpty();
        var vc=new ValidationContext(this);
        var packagesValid = this.Where(it => !it.Validate(vc).Any()).ToArray();
        return packagesValid.Select(it =>
            {
                try
                {
                    return new Package(it.Year.TryGetNumber().number, it.Month.TryGetMonth().number, it.Values.ValidData());
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        ).ToArray();
    }
}

public record ProblemRow(int row, string problem);