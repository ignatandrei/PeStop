using OneOf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace PSARModels;

[GenerateOneOf]
public class StringShouldBeMonth : OneOfBase<int, string>
{
    readonly Dictionary<string, int> months =new()
    { 
        {"jan",1},    {"feb",2},  {"mar",3},  {"apr",4},  {"may",5},  {"jun",6},  {"jul",7},  {"aug",8},  {"sep",9},  {"oct",10}, {"nov",11}, {"dec",12},
    };
    StringShouldBeMonth(OneOf<int, string> _) : base(_) { }
    public static implicit operator StringShouldBeMonth(string _) => new StringShouldBeMonth(_);
    public (bool isMonth, int number) TryGetMonth() =>
        Match(
            i => (true, i),
            s => (months.ContainsKey(s) ? (true, months[s]):(false,-1))
        );

    public bool IsMonth() => TryGetMonth().isMonth;

    public static SuccessOrInvalid Validation(StringShouldBeMonth value, [CallerArgumentExpression("value")] string name = "")
    {
        if (!value.IsMonth()) return new ValidationResult($"value {name} =  {value.Value?.ToString() ?? ""} is not a month");
        return default(OneOf.Types.Success);
    }

}
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
    public bool IsNumber() => TryGetNumber().isNumber;
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


