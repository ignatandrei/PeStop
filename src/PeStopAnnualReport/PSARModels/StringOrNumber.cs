using OneOf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace PSARModels;

[GenerateOneOf]
public class StringShouldBeNumber : OneOfBase< int, string>
{
    StringShouldBeNumber(OneOf< int, string> _) : base(_) { }

    // optionally, define implicit conversions
    // you could also make the constructor public
    public static implicit operator StringShouldBeNumber(string _) => new StringShouldBeNumber(_);
    public static implicit operator StringShouldBeNumber(int _) => new StringShouldBeNumber(_);

    public (bool isNumber, int number) TryGetNumber() =>
        Match(
            i => (true, i),
            s => (int.TryParse(s, out var n), n)
            
        );
    public bool IsNumber() => this.IsT0;
    public static SuccessOrInvalid Validation(StringShouldBeNumber value, [CallerArgumentExpression("value")] string name = "")
    {
        if (!value.IsNumber()) return new ValidationResult($"value {name} =  {value.Value?.ToString()??""} is not a number");
        return default(OneOf.Types.Success);
       
    }
}



[GenerateOneOf]
public partial class SuccessOrInvalid : OneOfBase<ValidationResult, OneOf.Types.Success>
{

}


