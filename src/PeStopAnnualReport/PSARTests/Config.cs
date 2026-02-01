using LightBDD.Core.Configuration;
using LightBDD.Framework.Configuration;
using LightBDD.Framework.Reporting.Formatters;
using LightBDD.TUnit;
using PSARTests;


[assembly: ConfiguredLightBddScope]

namespace PSARTests;


internal class ConfiguredLightBddScopeAttribute : LightBddScopeAttribute
{
    protected override void OnConfigure(LightBddConfiguration configuration)
    {
        configuration.ReportWritersConfiguration()
            .AddFileWriter<HtmlReportFormatter>("LightBDD_Report.html")
            .AddFileWriter<MarkdownReportFormatter>("LightBDD_Report.md")
            ;

    }

    protected override async ValueTask OnSetUp()
    {

        Console.WriteLine("!!!!before all");
        // code executed before any scenarios
    }

    protected override async ValueTask OnTearDown()
    {
        // code executed after all scenarios
    }
}
