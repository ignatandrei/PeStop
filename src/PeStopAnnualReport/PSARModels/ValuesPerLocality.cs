using System.Numerics;

namespace PSARModels;

public abstract class ValuesPerLocality<T>: Dictionary<string, T>
    
{
}
public class ValuesPerLocalityInt : ValuesPerLocality<int>
{

}

public class ValuesPerLocalityRead : ValuesPerLocality<StringOrNumber>
{
    
}