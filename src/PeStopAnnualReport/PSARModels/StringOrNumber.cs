using OneOf;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PSARModels;

[GenerateOneOf]
public class StringOrNumber : OneOfBase<string, int>
{
    StringOrNumber(OneOf<string, int> _) : base(_) { }

    // optionally, define implicit conversions
    // you could also make the constructor public
    public static implicit operator StringOrNumber(string _) => new StringOrNumber(_);
    public static implicit operator StringOrNumber(int _) => new StringOrNumber(_);

    public (bool isNumber, int number) TryGetNumber() =>
        Match(
            s => (int.TryParse(s, out var n), n),
            i => (true, i)
        );
}


