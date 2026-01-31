using PSARModels;
using System.ComponentModel.DataAnnotations;

namespace PSAR.Export;

public record Display(ValidationResult[] problems)
public class ExportPackages
{
    public async Task<Stream> Export(PackagesList packages)
    {
        var validProblems = packages.Validate(new ValidationContext(this)).ToArray();

        using var ms = new MemoryStream();

        return ms;
    }

}
