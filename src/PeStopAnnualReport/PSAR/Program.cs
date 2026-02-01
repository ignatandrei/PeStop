using PSAR.Export;
using PSARReadData;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Diagnostics;
partial class Program
{
    static async Task<int> Main(string[] args)
    {

        Option<FileInfo> fileOption = new("--file", "-f")
        {
            Description = "The file to read and display on the console"
        };
        Option<bool?> doNotUse = new("--andrei")
        {
            Description = "Do not use,just test"
        };
        RootCommand rootCommand = new("PeStop Generator");

        rootCommand.Options.Add(fileOption);
        rootCommand.Options.Add(doNotUse);
        
        ParseResult parseResult = rootCommand.Parse(args);
        if (parseResult.Errors.Count > 0)
        {
            foreach (ParseError parseError in parseResult.Errors)
            {
                Console.Error.WriteLine(parseError.Message);
            }
            rootCommand.Parse("-h").Invoke();
            return 0;
        }
        if (parseResult.GetValue(fileOption) is FileInfo parsedFile)
        {
            await ReadFile(parsedFile);
            return 0;
        }
        if (parseResult.GetValue(doNotUse) is not null)
        {
            await DoTest();
            return 0;
        }
        rootCommand.Parse("-h").Invoke();
        return 1;
    }

    static async Task ReadFile(FileInfo file)
    {
        ReadExcel readExcel = new();
        Console.WriteLine($"Reading file: {file.FullName}");
        var result = await readExcel.ReadExcelData(file.FullName);
        result.Switch(
            dataObtained => { },
            missingExcel => Console.WriteLine($"Error: Excel file not found - {file.FullName}"),
            missingSheet => Console.WriteLine($"Error: Sheet not found - {string.Join(",", missingSheet.names)} in file {file.FullName}"),
            resultPackages => ProcessInvalidPackages(resultPackages)

            );

        if(!result.IsT0)
        {
            Console.WriteLine("Processing finished with errors.");
            return;
        }
        await ProcessObtainedData(result.AsT0);
        Console.WriteLine("Processing finished.");
    }

    private static void ProcessInvalidPackages(ResultPackages resultPackages)
    {
        resultPackages.Switch(
            validPackages => { },
            notFoundHeader => Console.WriteLine($"Error: Not found header in sheet packages {string.Join(",",notFoundHeader.name)}")
            );
    }

    private static async Task ProcessObtainedData(DataObtained obtain)
    {
        var folderExport = Path.Combine(Path.GetTempPath(),"PSAR"+Path.GetFileName( Path.GetTempFileName()));
        if(!Directory.Exists(folderExport))
        {
            Directory.CreateDirectory(folderExport);
        }
        var export = new ExportPackages();
        
        var res = await export.ExportLineStackLastYear(obtain.packages, folderExport);
        Console.WriteLine($"Exported file: {res}");

        await ExportPackages.SaveFiles(folderExport);
        var p = new Process();
        p.StartInfo = new ProcessStartInfo(Path.Combine(folderExport,"index.html"))
        {
            UseShellExecute = true
        };
        p.Start();

    }

    static async Task DoTest()
    {
        string rootProject = @"D:\eu\GitHub\PeStop\";

        string fileName = Path.Combine(rootProject, @"src\PeStopAnnualReport\data\centralizare_pe_stop.xlsx");
        Console.WriteLine("start processing data");
        ReadExcel readExcel = new();
        var result = await readExcel.ReadExcelData(fileName);
        var obtain = result.AsT0;

        var export = new ExportPackages();
        string folderExport = Path.Combine(rootProject, @"docs\export");

        var res = await export.ExportLineStackLastYear(obtain.packages, folderExport);
        Console.WriteLine($"Exported file: {res}");

        await ExportPackages.SaveFiles(folderExport);

    }
}