namespace PSARModels;

public class PackagesRead
{
    public readonly int row;

    public PackagesRead(int row)
    {
        this.row = row;
        Year = "";
        Month = "";
        Values = [];
    }
    public StringOrNumber Year { get; set; }
    public StringOrNumber Month { get; set; }
    
    public ValuesPerLocalityRead Values { get; set; }
}

public class PackagesList : List<PackagesRead>
{

}