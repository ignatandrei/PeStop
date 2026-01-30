using System.Numerics;

namespace PSARModels;

public abstract class ValuesPerLocality<T>: Dictionary<string, T>
    where T:INumber<T>
{
}


public class ValuesPerLocalityInt : ValuesPerLocality<int>
{
    
}