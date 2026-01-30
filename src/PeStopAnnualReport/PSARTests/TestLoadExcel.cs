using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSARTests;

[Label("FEAT-1")]
[FeatureDescription(
@"In order to verify excel
As a developer
I want to verify that can be loaded")]

public partial class TestLoadExcel
{
    [Label("SCENARIO-1-LoadData")]
    [Scenario]
    [Arguments("centralizare_pe_stop.xlsx")]
    
    public async Task BasicVerifyThrow(string excel)
    {
        await Runner.RunScenarioAsync(
            _ => Given_The_Excel(excel),
            _ => Read_The_Excel(excel)
            );
    }
}
