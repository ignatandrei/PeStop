using PSAR.Export.Templates;
using PSARModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace PSAR.Export;

public record DisplayDataCursuri(CursuriValide cursuri);
public class ExportCursuri
{
    public async Task<bool> ExportAllDatacursuri(CursuriList? cursuri, string folderExport)
    {
        if (cursuri == null) return false;
        var validProblems = cursuri.Validate(new ValidationContext(this)).ToArray();
        var data = cursuri.ValidCursuri();
        if (data.Length == 0) return false;
        
        var displayData = new DisplayDataCursuri(data);
        var template = new CursuriRadar(displayData);
        var ret = await template.RenderAsync();
        var htmlFile = Path.Combine(folderExport, "cursuriRadar.js");
        await File.WriteAllTextAsync(htmlFile, ret);

        var jsonFile = Path.Combine(folderExport, "cursuri.json");
        var options = new JsonSerializerOptions { WriteIndented = true };
        await File.WriteAllTextAsync(jsonFile, JsonSerializer.Serialize(data, options));
        return true;
    }

}
