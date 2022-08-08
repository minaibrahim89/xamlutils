using System.CommandLine;
using XamlUtils.Tools;

namespace XamlUtils.Commands.ResourceDictionary;

public class DuplicateKeysCommand : Command
{
    public DuplicateKeysCommand()
        : base("duplicateKeys", "Finds duplicate keys in a ResourceDictionary.")
    {
        var fileOption = new Option<FileInfo?>(name: "-f", description: "The XAML file to check.");

        AddOption(fileOption);
        this.SetHandler(FindDuplicateKeys, fileOption);
    }

    private static void FindDuplicateKeys(FileInfo? file)
    {
        if (file == null || !file.Exists)
        {
            Console.WriteLine("File not found");
            return;
        }

        var xaml = File.ReadAllText(file.FullName);
        var duplicateKeys = ResourceDictionaryToolkit.FindDuplicateKeys(xaml);

        if (!duplicateKeys.Any())
        {
            Console.WriteLine("No duplicate keys are found.");
        }
        else
        {
            Console.WriteLine("The following duplicate keys are found:");
            Console.WriteLine(string.Join(Environment.NewLine, duplicateKeys));
        }
    }
}