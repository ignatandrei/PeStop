using System;
using System.Collections.Generic;
using System.Text;

namespace PSAR.Export;

public class ExportHTML
{
    public static async Task SaveFiles(string path)
    {

        await SaveEmb(EmbeddedResourceresources.echarts_min_js, path);
        await SaveEmb(EmbeddedResourceresources.index_html, path);
        await SaveEmb(EmbeddedResourceresources.RO_svg, path);
    }
    private static async Task SaveEmb(EmbeddedResourceresources res, string path)
    {
        var stream = EmbeddedResources.GetStream(res);
        string fileName = res.ToString().Replace("_", ".");
        using var fileStream = File.Create(Path.Combine(path, fileName));
        await stream.CopyToAsync(fileStream);
    }
}
