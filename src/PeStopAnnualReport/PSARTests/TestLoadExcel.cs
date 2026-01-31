using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using PSARModels;
using PSARReadData;
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
    [Label("SCENARIO-1-LoadData-problems")]
    [Scenario]
    //[Arguments("Data/centralizare_pe_stop.xlsx")]
    [Arguments("Data/notexists.xlsx", typeof(MissingExcel))]
    [Arguments("Data/error_sheet_names.xlsx", typeof(ExcelMissingSheet))]
    [Arguments("Data/error_sheet__header_packages.xlsx", typeof(NotFoundHeader))]
    public async Task BasicVerifyProblems(string excel,Type type)
    {
        
        await Runner.RunScenarioAsync(
            _ => Given_The_Excel(excel), 
            _ => When_Read_The_Excel(excel),
            _ => Then_Should_Obtain_Type(type)
            );
    }
    [Label("SCENARIO-2-LoadData-validations")]
    [Scenario]
    [Arguments("Data/invalid_package_year_sheet.xlsx", typeof(DataObtained))]
    public async Task BasicVerifyValidation(string excel, Type type)
    {

        await Runner.RunScenarioAsync(
            _ => Given_The_Excel(excel),
            _ => When_Read_The_Excel(excel),
            _ => Then_Should_Obtain_Type(type)
            );
    }

}
