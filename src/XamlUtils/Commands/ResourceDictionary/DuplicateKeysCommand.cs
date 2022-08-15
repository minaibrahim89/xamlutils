using System.CommandLine;
using System.CommandLine.Invocation;
using XamlUtils.Tools;

namespace XamlUtils.Commands.ResourceDictionary;

public class DuplicateKeysCommand : Command
{
    public const int ReturnCodeNoDuplicatesFound = 0;
    public const int ReturnCodeDuplicatesFound = 11;
    public const int ReturnCodeFileNotFound = 12;

    private readonly Option<FileInfo> _fileOption;

    public DuplicateKeysCommand()
          : base("duplicateKeys", "Finds duplicate keys in a ResourceDictionary.")
    {
        _fileOption = new Option<FileInfo>(name: "-f", description: "The XAML file to check.") { IsRequired = true };

        AddOption(_fileOption);

        this.SetHandler(FindDuplicateKeys);
    }

    private void FindDuplicateKeys(InvocationContext context)
    {
        var file = context.ParseResult.GetValueForOption(_fileOption)!;

        if (!file.Exists)
        {
            Console.WriteLine("File not found.");
            context.ExitCode = ReturnCodeFileNotFound;
            return;
        }

        var xaml = File.ReadAllText(file.FullName);
        var duplicateKeys = ResourceDictionaryToolkit.FindDuplicateKeys(xaml);

        if (!duplicateKeys.Any())
        {
            Console.WriteLine("No duplicate keys are found.");
            context.ExitCode = ReturnCodeNoDuplicatesFound;
        }
        else
        {
            Console.WriteLine("The following duplicate keys are found:");
            Console.WriteLine(string.Join(Environment.NewLine, duplicateKeys));
            context.ExitCode = ReturnCodeDuplicatesFound;
        }
    }
}