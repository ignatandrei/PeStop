using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;

namespace PSARModels;

public abstract class ValuesPerLocality<T>: Dictionary<string, T>
    
{
}
public class ValuesPerLocalityInt : ValuesPerLocality<int>
{

}

public class ValuesPerLocalityRead : ValuesPerLocality<StringShouldBeNumber>, IValidatableObject
{
    public ValuesPerLocalityInt ValidData()
    {
        ValuesPerLocalityInt ret = new();
        foreach (var item in this)
        {
            var result = ValidateItem(item);
            if (result != null) continue;
            ret.Add(item.Key, item.Value.TryGetNumber().number);
        }
        return ret;
    }
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach(var item in this)
        {
            var result = ValidateItem(item);
            if(result != null ) yield return result;
        }
    }
    private ValidationResult? ValidateItem(KeyValuePair<string,StringShouldBeNumber> item)
    {
        var name = item.Key;
        var value = item.Value;
        ValidationResult? result = null;
        StringShouldBeNumber.Validation(value, name).Switch(
            val => result = val,
            _ => { }
            );
        return result;
    }
}