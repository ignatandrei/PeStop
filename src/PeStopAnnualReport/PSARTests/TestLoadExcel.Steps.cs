using LightBDD.TUnit;
using PSARReadData;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSARTests;

partial class TestLoadExcel : FeatureFixture
{
    ReadExcel readExcel = new();
    async Task Given_The_Excel(string excel)
    {
        await Assert.That(excel).Satisfies(File.Exists);

    }
    async Task Read_The_Excel(string excel)
    {
        var data = await readExcel.ReadExcelData(excel);
        var result = true;
        data.Switch(
            res => result = res.Value,
            missingExcel => throw new FileNotFoundException(nameof(missingExcel.fileName)),
            missingSheet => throw new ArgumentOutOfRangeException(string.Join(",", missingSheet.names)),
            problemPachete => problemPachete.Switch(
                                pachete=> { },
                                notFoundHeader => throw new ArgumentOutOfRangeException(string.Join(",",notFoundHeader.name)),
                                problemWithRow => throw new ArgumentException($"row {problemWithRow.nrRow} col {problemWithRow.nrCol}")
                                )
            );
    }
}