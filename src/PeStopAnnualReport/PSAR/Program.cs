// See https://aka.ms/new-console-template for more information
using PSAR.Export;
using PSARReadData;

string rootProject = @"D:\eu\GitHub\PeStop\";

string fileName =Path.Combine(rootProject, @"src\PeStopAnnualReport\data\centralizare_pe_stop.xlsx");
Console.WriteLine("start processing data");
ReadExcel readExcel = new();
var result = await readExcel.ReadExcelData(fileName);
var obtain = result.AsT0;


string folderExport = Path.Combine(rootProject, @"docs\export");

var exportPackages = new ExportPackages();

var res = await exportPackages.ExportAllDataPackages(obtain.packages,folderExport);
Console.WriteLine($"Exported file: {res}");

var exportVol = new ExportVoluntari();
res = await exportVol.ExportAllDataVoluntari(obtain.voluntari, folderExport);
Console.WriteLine($"Exported file: {res}");
await ExportHTML.SaveFiles(folderExport);

var exportCurs = new ExportCursuri();
res = await exportCurs.ExportAllDatacursuri(obtain.cursuri, folderExport);
Console.WriteLine($"Exported file: {res}");
await ExportHTML.SaveFiles(folderExport);
