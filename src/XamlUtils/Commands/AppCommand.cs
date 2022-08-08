using System.CommandLine;
using XamlUtils.Commands.ResourceDictionary;

namespace XamlUtils.Commands;

public class AppCommand : RootCommand
{
    public AppCommand()
        : base("Useful tools for dealing with XAML.")
    {
        AddCommand(new ResourceDictionaryCommand());
    }
}