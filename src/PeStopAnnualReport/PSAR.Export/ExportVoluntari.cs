using PSAR.Export.Templates;
using PSARModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PSAR.Export;

record DataDisplayVoluntari(string[] colNames, Voluntari[] voluntari);

public class ExportVoluntari
{
    public async Task<bool> ExportChartVoluntari(VoluntariList? voluntari, string folderExport)
    {
        if (voluntari == null) return false;
        var validProblems = voluntari.Validate(new ValidationContext(this)).ToArray();
        var data = voluntari.ValidVoluntaris();
        if (data.Length == 0) return false;
        var colNames = data[0].Values.Select(v => v.Key).ToList();

        var template = new VoluntariDatasetSeriesLayoutBy(new DataDisplayVoluntari(colNames.ToArray(), data));
        var text = await template.RenderAsync();
        var bytes = Encoding.ASCII.GetBytes(text);
        var filePath = Path.Combine(folderExport, "voluntari_dataset_series_layout_by.js");
        await File.WriteAllBytesAsync(filePath, bytes);
        return true; 


    }
}
