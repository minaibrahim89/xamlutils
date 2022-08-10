using System.CommandLine;
using XamlUtils.Tools;

namespace XamlUtils.Commands.ResourceDictionary;

public class KeysDiffCommand : Command
{
    public KeysDiffCommand()
        : base("keysDiff", "Finds diff between two ResourceDictionary files.")
    {
        var srcFileOption = new Option<FileInfo?>(name: "-src", description: "The source (left) XAML file.");
        var targetFileOption = new Option<FileInfo?>(name: "-target", description: "The target (right) XAML file.");

        AddOption(srcFileOption);
        AddOption(targetFileOption);
        this.SetHandler(FindKeysDiff, srcFileOption, targetFileOption);
    }

    private static void FindKeysDiff(FileInfo? src, FileInfo? dest)
    {
        if (src == null || dest == null)
        {
            Console.WriteLine("Please provide paths to two XAML files to compare.");
            return;
        }

        if (!src.Exists)
        {
            Console.WriteLine($"File with '{src.FullName}' is not found.");
            return;
        }

        if (!dest.Exists)
        {
            Console.WriteLine($"File with '{dest.FullName}' is not found.");
            return;
        }

        var xaml1 = File.ReadAllText(src.FullName);
        var xaml2 = File.ReadAllText(dest.FullName);
        var keysDiff = ResourceDictionaryToolkit.FindKeysDiff(xaml1, xaml2);

        Console.WriteLine();

        if (!keysDiff.Any())
        {
            Console.WriteLine("No keys diff found.");
        }
        else
        {
            Console.WriteLine("The following keys are missing from destination:");
            Console.WriteLine(string.Join(Environment.NewLine, keysDiff.Where(key => key.StartsWith('-'))));

            Console.WriteLine();

            Console.WriteLine("The following keys are added to destination:");
            Console.WriteLine(string.Join(Environment.NewLine, keysDiff.Where(key => key.StartsWith('+'))));
        }
    }
}