using System.CommandLine;
using System.CommandLine.Invocation;
using XamlUtils.Tools;

namespace XamlUtils.Commands.ResourceDictionary;

public class KeysDiffCommand : Command
{
    public const int ReturnCodeNoDiffFound = 0;
    public const int ReturnCodeDiffFound = 11;
    public const int ReturnCodeSrcFileNotFound = 121;
    public const int ReturnCodeTargetFileNotFound = 122;

    private readonly Option<FileInfo> _srcFileOption;
    private readonly Option<FileInfo> _targetFileOption;

    public KeysDiffCommand()
        : base("keysDiff", "Finds diff between two ResourceDictionary files.")
    {
        _srcFileOption = new Option<FileInfo>(name: "-src", description: "The source (left) XAML file.") { IsRequired = true };
        _targetFileOption = new Option<FileInfo>(name: "-target", description: "The target (right) XAML file.") { IsRequired = true };

        AddOption(_srcFileOption);
        AddOption(_targetFileOption);
        this.SetHandler(FindKeysDiff);
    }

    private void FindKeysDiff(InvocationContext context)
    {
        var src = context.ParseResult.GetValueForOption(_srcFileOption)!;
        var target = context.ParseResult.GetValueForOption(_targetFileOption)!;

        if (!src.Exists)
        {
            Console.WriteLine($"File with '{src.FullName}' is not found.");
            context.ExitCode = ReturnCodeSrcFileNotFound;
            return;
        }

        if (!target.Exists)
        {
            Console.WriteLine($"File with '{target.FullName}' is not found.");
            context.ExitCode = ReturnCodeTargetFileNotFound;
            return;
        }

        var xaml1 = File.ReadAllText(src.FullName);
        var xaml2 = File.ReadAllText(target.FullName);
        var keysDiff = ResourceDictionaryToolkit.FindKeysDiff(xaml1, xaml2);

        if (!keysDiff.Any())
        {
            Console.WriteLine("No keys diff found.");
            context.ExitCode = ReturnCodeNoDiffFound;
        }
        else
        {
            Console.WriteLine("The following keys are missing from destination:");
            Console.WriteLine(string.Join(Environment.NewLine, keysDiff.Where(key => key.StartsWith('-'))));

            Console.WriteLine();

            Console.WriteLine("The following keys are added to destination:");
            Console.WriteLine(string.Join(Environment.NewLine, keysDiff.Where(key => key.StartsWith('+'))));

            context.ExitCode = ReturnCodeDiffFound;
        }
    }
}