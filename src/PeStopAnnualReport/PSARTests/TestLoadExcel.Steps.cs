using LightBDD.TUnit;
using PSARReadData;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSARTests;

partial class TestLoadExcel : FeatureFixture
{
    ReadExcel readExcel = new();
    ResultExcel result = default!;
    async Task Given_The_Excel(string excel)
    {
        //await Assert.That(excel).Satisfies(File.Exists);
        await Assert.That(excel).IsNotEmpty();
    }
    async Task Then_Should_Obtain_Type(Type type)
    {
        var res = result.Value;
        var typeRes= res.GetType();
        if (typeof(ResultPackages) != typeRes)
        {
            Assert.Equals(type, typeRes);
            return;
        }
        ResultPackages? rp=res as ResultPackages;
        ArgumentNullException.ThrowIfNull(rp);
        res = result.Value;
        typeRes = res.GetType();
        Assert.Equals(type, typeRes);

    }
    async Task When_Read_The_Excel(string excel)
    {
        result = await readExcel.ReadExcelData(excel);
        //data.Switch(
        //    res => result = res.Value,
        //    missingExcel => throw new FileNotFoundException(nameof(missingExcel.fileName)),
        //    missingSheet => throw new ArgumentOutOfRangeException(string.Join(",", missingSheet.names)),
        //    problemPachete => problemPachete.Switch(
        //                        pachete=> { },
        //                        notFoundHeader => throw new ArgumentOutOfRangeException(string.Join(",",notFoundHeader.name)),
        //                        problemWithRow => throw new ArgumentException($"row {problemWithRow.nrRow} col {problemWithRow.nrCol}")
        //                        )
        //    );
    }
}