namespace PSARModels;

public class PackagesRead
{
    private readonly int row;

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

public record ProblemRow(int row, string problem);