using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace PSARModels;

public record Curs(int year, string tematica, ValuesPerLocalityInt Values);
[DebuggerDisplay("{ToDebugString}")]
public class CursRead : IValidatableObject
{
    private readonly int row;

    private string ToDebugString
    {
        get
        {
            return $"Row {row} {Year} {Tematica} {Values}";
        }
    }
    public CursRead(int row)
    {
        this.row = row;
        Year = "";
        Values = [];
    }
    public StringShouldBeNumber Year { get; set; }

    public string Tematica { get; set; } = "";

    public ValuesPerLocalityRead Values { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        string[] members = ["row :" + row];
        ValidationResult? validationResult;

        validationResult = null;

        StringShouldBeNumber.Validation(Year).Switch(
            val => validationResult = val,
            _ => { }
            );
        if (validationResult != null) yield return new ValidationResult(validationResult.ErrorMessage, members);
        
        if(string.IsNullOrWhiteSpace(Tematica))
        {
            yield return new ValidationResult("Tematica is required", members);
        }

        validationResult = null;
        var items = this.Values.Validate(new ValidationContext(this));
        foreach (var item in items) yield return new ValidationResult(item.ErrorMessage, members);

    }

}

public class CursuriList : List<CursRead>, IValidatableObject
{
    //if does not have month and year
    private Predicate<CursRead> Empty = (p) =>
    {
        if (p == null) return true;
        if (p.Year == null) return true;
        var valYear = p.Year.Value?.ToString()?.Trim();
        return valYear?.Length == 0;
    };
    public void RemoveEmpty()
    {
        //var nrEmpty = this.Where(it=>Empty(it)).ToArray();
        this.RemoveAll(Empty);
    }
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        this.RemoveEmpty();
        foreach (var item in this)
        {
            foreach (var vr in item.Validate(validationContext))
            {
                yield return vr;
            }
        }
    }
    public CursuriValide ValidCursuri()
    {
        this.RemoveEmpty();
        var vc = new ValidationContext(this);
        var valid = this.Where(it => !it.Validate(vc).Any()).ToArray();
        var arr=  valid.Select(it =>
        {
            try
            {
                return new Curs(it.Year.TryGetNumber().number,it.Tematica, it.Values.ValidData());
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        ).ToArray();
        return new CursuriValide(arr);
    }
}

public record CursuriValide(Curs[] Cursuri)
{
    public int Length => Cursuri?.Length??0;

    public Dictionary<int, Dictionary<string, int>> CursuriPerYearAndTematics()
    {
        Dictionary<int, Dictionary<string, int>> ret = new();
        var years = Cursuri.Select(it => it.year).Distinct() .ToArray();
        foreach (var year in years)
        {
            var cursuriForYear = Cursuri.Where(it => it.year == year)
                .Select(it => new { it.tematica, vals = it.Values.ToArray() })
                
                .ToArray();
            Dictionary<string, int> perYear = new();
            foreach (var curs in cursuriForYear)
            {
                var key = curs.tematica;
                foreach(var item in curs.vals)
                {
                    string keyItem = key + "_" + item.Key;
                    if (perYear.ContainsKey(keyItem))
                    {
                        throw new Exception($"Duplicate key {keyItem} for year {year} Prev Value : {perYear[keyItem]}, actual {item.Value}");
                    }               
                    
                    perYear.Add(keyItem, item.Value);
                }
            }
            ret.Add(year, perYear);
        }
        return ret;
    }
    public Dictionary<int,int>   CursuriPerYear()
    {
        
        var ret= this.Cursuri.GroupBy(it=>it.year, it => it.Values["durata_minute"])
            .ToDictionary(it=>it.Key, it=> it.ToArray().Sum());

        return ret;
    }
}