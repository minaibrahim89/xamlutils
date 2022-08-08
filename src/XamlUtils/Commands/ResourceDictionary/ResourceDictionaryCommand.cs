using System.CommandLine;

namespace XamlUtils.Commands.ResourceDictionary;

public class ResourceDictionaryCommand : Command
{
    public ResourceDictionaryCommand()
        : base("ResourceDictionary", "Utilities dealing with ResourceDictionary XAML files.")
    {
        AddCommand(new DuplicateKeysCommand());
    }
}