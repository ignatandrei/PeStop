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
        await SaveEmb(EmbeddedResourceresources.RO_svg, path);
    }
    private static async Task SaveEmb(EmbeddedResourceresources res, string path)
    {
        var stream = EmbeddedResources.GetStream(res);
        string fileName = res.ToString().Replace("_",".");
        using var fileStream = File.Create(Path.Combine(path, fileName));
        await stream.CopyToAsync(fileStream);
    }
    private async Task<XmlDocument?> ExportMapRoPackages(Package[]? packages)
    {
        var map = EmbeddedResources.GetReader(EmbeddedResourceresources.RO_svg);
        var text = await map.ReadToEndAsync();

        var reverse = ReverseColLinesPackage(packages);
        if (reverse.Count == 0) return null;
        List<string> colNames = [];
        foreach (var item in packages[0].Values)
        {
            colNames.Add(item.Key);
        }
        Dictionary<string, string> translate = new()
        {
            { "Bucharest","nr_pac_buc"},
            
                { "Tulcea","nr_pac_tulcea"},
                { "Vaslui","nr_pac_vaslui"}

        };
    
        var xml = new XmlDocument();
        xml.LoadXml(text);
        //gn_name
        var nodes = xml.SelectNodes("//*[local-name()='path']");
        var nr= nodes?.Count;
        Console.WriteLine(nr);
        foreach(XmlNode node in nodes)
        {
            if (node == null) continue;
            var name = node.Attributes?["name"]?.Value;
            if(name == null) continue;
            string colName = name;
            if (!colNames.Contains(name))
            {
                if (!translate.ContainsKey(name))
                {
                    continue;
                }
                colName= translate[name];
            }

            var values = reverse[colName];
            int total = 0;
            foreach(var val in values)
            {
                total += val.Value;
            }
            var title = xml.CreateNode(XmlNodeType.Element, "title",xml.DocumentElement!.NamespaceURI );
            title.InnerText= $"{name} Total pachete: {total}";
            node.AppendChild(title);
            var attr=node.Attributes["fill"];
            if(attr == null)
            {
                attr = xml.CreateAttribute("fill");
                node.Attributes.Append(attr);
            }
            attr.Value = "red";
        }
        return xml;
    }
    public static Dictionary<string, Dictionary<int, int>> ReverseColLinesPackage(Package[]? data)
    {
        if (data == null) return [];
        if (data.Length == 0) return [];
        Dictionary<string, Dictionary<int, int>> colData = [];
        var first = data[0];
        List<string> colNames = [];
        foreach (var item in first.Values)
        {
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
    public async Task<bool> ExportLineStackLastYear(PackagesList? packages, string folderExport)
    {
        if (packages == null) return false;
        var validProblems = packages.Validate(new ValidationContext(this)).ToArray();
        var data = packages.ValidPackages();
        var maxYear = data.Max(it => it.year);
        data=data.Where(it=>it.year == maxYear).ToArray();
        if (data.Length == 0) return false;
        var xml = await ExportMapRoPackages(data);
        string filePath = Path.Combine(folderExport, "packages_map_ro.svg");
        if(xml ==null) return false;
        xml.Save(filePath);
        var colData= ReverseColLinesPackage(data);
        List<string> colNames = [];
        foreach (var item in packages[0].Values)
        {
            colNames.Add(item.Key);
        }
         
        var template= new PackagesLineStack(new DisplayData(validProblems,colNames.ToArray(), colData,maxYear));
        var text = await template.RenderAsync();
        var bytes= Encoding.ASCII.GetBytes(text);
        filePath = Path.Combine(folderExport, "packages_last_year.js");
        await File.WriteAllBytesAsync(filePath, bytes);
        return true;
    }

}
