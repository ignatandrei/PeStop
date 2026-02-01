using PSAR.Export.Templates;
using PSARModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PSAR.Export;

public record DisplayData(ValidationResult[] problems,string[] colNames, Dictionary<string, Dictionary<int,int> > colData, int year);
public class ExportPackages
{
    public static async Task SaveFiles(string path) {
        
        await SaveEmb(EmbeddedResourceresources.echarts_min_js, path);
        await SaveEmb(EmbeddedResourceresources.index_html, path);
    }
    private static async Task SaveEmb(EmbeddedResourceresources res, string path)
    {
        var stream = EmbeddedResources.GetStream(res);
        string fileName = res.ToString().Replace("_",".");
        using var fileStream = File.Create(Path.Combine(path, fileName));
        await stream.CopyToAsync(fileStream);
    }
    private async Task ExportMapRoPackages(Package[]? packages)
    {
        var reverse=ReverseColLinesPackage(packages);
        if (reverse.Count == 0) return;
        var map= EmbeddedResources.GetReader(EmbeddedResourceresources.RO_svg);
        var text = await map.ReadToEndAsync();

        List<string> colNames = [];
        foreach (var item in packages[0].Values)
        {
            colNames.Add(item.Key);
        }
            var xml = new XmlDocument();
        xml.LoadXml(text);
        //gn_name
        var nodes = xml.SelectNodes("//*[local-name()='path'");
        foreach(XmlNode node in nodes)
        {
            if (node == null) continue;
            var name = node.Attributes?["gn_name"]?.Value;
            if(name == null) continue;
            if(!colNames.Contains(name)) continue;
            var values = reverse[name];
            int total = 0;
            foreach(var val in values)
            {
                total += val.Value;
            }
            //var color = GetColor(total);
            //node.Attributes["fill"]!.Value = color;
        }
    }
    public static Dictionary<string, Dictionary<int, int>> ReverseColLinesPackage(Package[]? data)
    {
        if (data == null) return [];
        if (data.Length == 0) return [];

        var colData = ReverseColLinesPackage(data);
        if(colData.Count ==0) return [];
        var first = data[0];
        List<string> colNames = [];
        foreach (var item in first.Values)
        {
            colNames.Add(item.Key);
            colData.Add(item.Key, new Dictionary<int, int>()
            {
                { 0,0} ,{1,0},{2,0 },{3,0 },{4,0},{5,0},{6,0 },{7,0 },{8,0},{9,0},{10,0 },{11,0},{12,0 } }
            );
        }
        foreach (var row in data)
        {
            int month = row.month - 1;
            foreach (var item in row.Values)
            {
                colData[item.Key][month] = item.Value;
            }
        }
        return colData;
    }
    public async Task<byte[]> ExportLineStackLastYear(PackagesList? packages)
    {
        if (packages == null) return null;
        var validProblems = packages.Validate(new ValidationContext(this)).ToArray();
        var data = packages.ValidPackages();
        var maxYear = data.Max(it => it.year);
        data=data.Where(it=>it.year == maxYear).ToArray();
        if (data.Length == 0) return [];
        var colData= new Dictionary<string, Dictionary<int, int>>();
        List<string> colNames = [];
        foreach (var item in first.Values)
        {
            colNames.Add(item.Key);
        }
         
        var template= new PackagesLineStack(new DisplayData(validProblems,colNames.ToArray(), colData,maxYear));
        var text = await template.RenderAsync();
        return Encoding.ASCII.GetBytes(text);
    }

}
