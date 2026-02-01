// See https://aka.ms/new-console-template for more information
using PSAR.Export;
using PSARReadData;

string rootProject = @"D:\eu\GitHub\PeStop\";

string fileName =Path.Combine(rootProject, @"src\PeStopAnnualReport\data\centralizare_pe_stop.xlsx");
Console.WriteLine("start processing data");
ReadExcel readExcel = new();
var result = await readExcel.ReadExcelData(fileName);
var obtain = result.AsT0;

var export= new ExportPackages();
string folderExport = Path.Combine(rootProject ,@"docs\export");

var res = await export.ExportLineStackLastYear(obtain.packages,folderExport);
Console.WriteLine($"Exported file: {res}");

await ExportPackages.SaveFiles(folderExport);

