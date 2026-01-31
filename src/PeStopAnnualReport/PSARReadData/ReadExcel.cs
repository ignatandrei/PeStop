using MiniExcelLibs;
using OneOf;
using OneOf.Types;
using PSARModels;
using System.IO;
using System.Linq;
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
    public async Task<ResultPackages> ReadPachete(string excel,string nameSheet)
    {
        var columns = MiniExcel.GetColumns(excel,sheetName:nameSheet,useHeaderRow:true);
        var missing =colsPachete.Where(it=> !columns.Contains(it)).ToArray();
        if(missing.Length > 0)
        {
            return new NotFoundHeader(missing);
        }
        var pachete = MiniExcel.Query(path: excel, useHeaderRow: true, sheetName: nameSheet).Cast<IDictionary<string, object>>();
        PackagesList packages = new();
        foreach(var (number,row) in pachete.Index())
        {
            IDictionary<string, object> x = row as IDictionary<string,object>;
            ArgumentNullException.ThrowIfNull(x);
            PackagesRead p = new(number+2);//first is header     
            p.Year = x["an"]?.ToString()??"";
            p.Month = x["month"].ToString() ?? "";
            for(int i = 2; i < colsPachete.Length; i++)
            {
                string col = colsPachete[i];
                p.Values.Add(col,x[col]?.ToString()??"");
            }
            packages.Add(p);
        }
        return new Result<PackagesList>(packages);
    }
}

[GenerateOneOf]
public partial class ResultPackages : OneOfBase<
    Result<PackagesList>,
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
    ResultPackages> { 

}

public record MissingExcel(string fileName);
public record ExcelMissingSheet(string[] names);