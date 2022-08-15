// See https://aka.ms/new-console-template for more information

using System.CommandLine;
using XamlUtils.Commands;

namespace XamlUtils;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var exitCode = await new AppCommand().InvokeAsync(args);
        Console.WriteLine($"Exit code: {exitCode}");

        return exitCode;
    }
}
