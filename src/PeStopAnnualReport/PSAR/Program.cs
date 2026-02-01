// See https://aka.ms/new-console-template for more information
using PSAR.Export;
using PSARReadData;

string fileName = @"D:\eu\GitHub\PeStop\src\PeStopAnnualReport\data\centralizare_pe_stop.xlsx";
Console.WriteLine("Hello, World!");
ReadExcel readExcel = new();
var result = await readExcel.ReadExcelData(fileName);
var obtain = result.AsT0;

ExportPackages export= new ExportPackages();

var res= await export.ExportLineStackLastYear(obtain.packages);
ArgumentNullException.ThrowIfNull(res);
string folderExport = @"D:\eu\GitHub\PeStop\docs\export";

await ExportPackages.SaveFiles(folderExport);
var filePath = Path.Combine(folderExport, "packages_last_year.js");    
await File.WriteAllBytesAsync(filePath, res);