
using OneOf;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace PSARModels;

public record Voluntari(int year, ValuesPerLocalityInt Values);


[DebuggerDisplay("{ToDebugString}")]
public class VoluntarRead : IValidatableObject
{
    private readonly int row;

    private string ToDebugString
    {  
        get
        {
            return $"Row {row} {Year}  {Values}";
        }
    }
    public VoluntarRead(int row)
    {
        this.row = row;
        Year = "";
        Values = [];
    }
    public StringShouldBeNumber Year { get; set; }


    public ValuesPerLocalityRead Values { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        string[] members = ["row :" + row];
        ValidationResult? validationResult;

        validationResult = null;

        StringShouldBeNumber.Validation(Year).Switch(
            val => validationResult = val,
            _ => { }
            );
        if (validationResult != null) yield return new ValidationResult(validationResult.ErrorMessage, members);
        validationResult = null;
        var items = this.Values.Validate(new ValidationContext(this));
        foreach (var item in items) yield return new ValidationResult(item.ErrorMessage, members);

    }

}

public class VoluntariList : List<VoluntarRead>, IValidatableObject
{
    //if does not have month and year
    private Predicate<VoluntarRead> Empty = (p) =>
    {
        if (p == null) return true;
        if (p.Year == null) return true;
        var valYear = p.Year.Value?.ToString()?.Trim();
        return valYear?.Length == 0 ;
    };
    public void RemoveEmpty()
    {
        //var nrEmpty = this.Where(it=>Empty(it)).ToArray();
        this.RemoveAll(Empty);
    }
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        this.RemoveEmpty();
        foreach (var item in this)
        {
            foreach (var vr in item.Validate(validationContext))
            {
                yield return vr;
            }
        }
    }
    public Voluntari[] ValidVoluntaris()
    {
        this.RemoveEmpty(); 
        var vc = new ValidationContext(this);
        var VoluntarisValid = this.Where(it => !it.Validate(vc).Any()).ToArray();
        return VoluntarisValid.Select(it =>
        {
            try
            {
                return new Voluntari(it.Year.TryGetNumber().number,  it.Values.ValidData());
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        ).ToArray();
    }
}

