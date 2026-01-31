using LightBDD.Framework;
using LightBDD.TUnit;
using PSARReadData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TUnit.Core.Exceptions;

namespace PSARTests;

partial class TestLoadExcel : FeatureFixture
{
    ReadExcel readExcel = new();
    ResultExcel result = default!;
    async Task Given_The_Excel(string excel)
    {
        //await Assert.That(excel).Satisfies(File.Exists);
        await Assert.That(excel).IsNotEmpty();
        //var fullPath = Path.GetFullPath(excel);
        //var dir = Path.GetDirectoryName(fullPath)??"";
        //foreach (var item in Directory.GetFiles(dir,"*.xlsx"))
        //{
        //    StepExecution.Current.Comment($"in the folder exists file : {item}");
        //}

    }
    async Task Then_Should_Obtain_Type(Type type)
    {
        var res = result.Value;
        var typeRes= res.GetType();
        if (typeof(ResultPackages) != typeRes)
        {
            await Assert.That(type).IsEqualTo(typeRes);
            return;
        }
        ResultPackages? rp=res as ResultPackages;
        ArgumentNullException.ThrowIfNull(rp);
        res = result.Value;
        typeRes = res.GetType();
        Assert.Equals(type, typeRes);

    }
    async Task And_Data_Obtained_Contains_Validation_nr(int nr)
    {
        var res = result.Value as DataObtained;
        await Assert.That(res).IsNotNull();
        var valid = res.Validate(new ValidationContext(this)).ToArray();
        await Assert.That(valid).HasCount(nr);
        foreach (var (index,item) in valid.Index())
        {
            StepExecution.Current.Comment($"{index+1})  {item.ErrorMessage} {item.MemberNames.FirstOrDefault()}");
        }
    }
    async Task When_Read_The_Excel(string excel)
    {
        result = await readExcel.ReadExcelData(excel);
        StepExecution.Current.Comment($"the result type of reading excel is {result.Value?.GetType().ToString()}");
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