using MiniExcelLibs;
using OneOf;
using OneOf.Types;
using PSARModels;
using System.IO;
using System.Security.Cryptography.X509Certificates;
namespace PSARReadData;

public class ReadExcel
{
    static string[] sheets = ["pachete", "cursuri", "voluntari"];
    static string[] colsPachete = ["an", "luna","nr_pac_buc","nr_pac_tulcea","nr_pac_vaslui"];
    public async Task<ResultExcel> ReadExcelData(string excel)
    {
        if (!File.Exists(excel))
            return new MissingExcel(excel);

        var sheetsInExcel = MiniExcel.GetSheetNames(excel);
        var missing = sheets.Where(it => !sheetsInExcel.Contains(it)).ToArray();
        if (missing.Length > 0)
            return new ExcelMissingSheet(missing);

        var res = await ReadPachete(excel, sheets[0]);
        if (!res.IsT0)
            return res;
        var packages = res.AsT0;
        return new Result<bool>(true);
    }
    public async Task<ResultPachete> ReadPachete(string excel,string nameSheet)
    {
        var columns = MiniExcel.GetColumns(excel,sheetName:nameSheet,useHeaderRow:true);
        var missing =colsPachete.Where(it=> !columns.Contains(it)).ToArray();
        if(missing.Length > 0)
        {
            return new NotFoundHeader(missing);
        }
        var pachete = MiniExcel.Query(path: excel, useHeaderRow: true, sheetName: nameSheet).Cast<IDictionary<string, object>>();
        List<Packages> packages = [];
        foreach(var row in pachete)
        {
            var x = row as IDictionary<string,object>;
            ArgumentNullException.ThrowIfNull(x);
            Packages p = new();            
            packages.Add(p);
        }
        return new Result<Packages[]>([.. packages]);
    }
}

[GenerateOneOf]
public partial class ResultPachete : OneOfBase<
    Result<Packages[]>,
    NotFoundHeader,
    ProblemWithRow
    >
{

}
public record NotFoundHeader(string[] name);
public record ProblemWithRow(int nrRow,int nrCol);

[GenerateOneOf]
public partial class ResultExcel : OneOfBase<
    Result<bool>,
    MissingExcel,
    ExcelMissingSheet,
    ResultPachete> { 

}

public record MissingExcel(string fileName);
public record ExcelMissingSheet(string[] names);