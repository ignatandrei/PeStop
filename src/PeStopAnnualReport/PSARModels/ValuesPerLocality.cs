using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace PSARModels;

public abstract class ValuesPerLocality<T>: Dictionary<string, T>
    
{
}
public class ValuesPerLocalityInt : ValuesPerLocality<int>
{

}

public class ValuesPerLocalityRead : ValuesPerLocality<StringShouldBeNumber>, IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach(var item in this)
        {
            var name = item.Key;
            var value = item.Value;
            ValidationResult? result = null;
            StringShouldBeNumber.Validation(value, name).Switch(
                val => result = val,
                _ => { }
                );
            if(result != null ) yield return result;
        }
    }
}