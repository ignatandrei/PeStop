using PSAR.Export.Templates;
using PSARModels;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;

namespace PSAR.Export;

public record DisplayData(ValidationResult[] problems,string[] colNames, Dictionary<string, Dictionary<int,int> > colData, int year);
public class ExportPackages
{
    public async Task<byte[]> ExportLineStackLastYear(PackagesList? packages)
    {
        if (packages == null) return null;
        var validProblems = packages.Validate(new ValidationContext(this)).ToArray();
        var data = packages.ValidPackages();
        var maxYear = data.Max(it => it.year);
        data=data.Where(it=>it.year == maxYear).ToArray();
        if (data.Length == 0) return [];
        var colData= new Dictionary<string, Dictionary<int, int>>();
        var first = data[0];
        List<string> colNames = [];
        foreach(var item in first.Values)
        {
            colNames.Add(item.Key);
            colData.Add(item.Key,new Dictionary<int, int>()
            { 
                { 0,0} ,{1,0},{2,0 },{3,0 },{4,0},{5,0},{6,0 },{7,0 },{8,0},{9,0},{10,0 },{11,0},{12,0 } }
            )    ;
        }
        foreach ( var row in data)
        {
            int month = row.month-1;
            foreach(var item in row.Values)
            {
                colData[item.Key][month]= item.Value;
            }
        }

        var template= new PackagesLineStack(new DisplayData(validProblems,colNames.ToArray(), colData,maxYear));
        var text = await template.RenderAsync();
        return Encoding.ASCII.GetBytes(text);
    }

}
