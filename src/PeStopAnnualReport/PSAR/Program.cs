// See https://aka.ms/new-console-template for more information
using PSAR.Export;
using PSARReadData;



string fileName = @"D:\eu\GitHub\PeStop\src\PeStopAnnualReport\data\centralizare_pe_stop.xlsx";
Console.WriteLine("Hello, World!");
ReadExcel readExcel = new();
var result = await readExcel.ReadExcelData(fileName);
var obtain = result.AsT0;

var export= new ExportPackages();
string folderExport = @"D:\eu\GitHub\PeStop\docs\export";

var res = await export.ExportLineStackLastYear(obtain.packages,folderExport);
Console.WriteLine($"Exported file: {res}");

await ExportPackages.SaveFiles(folderExport);

