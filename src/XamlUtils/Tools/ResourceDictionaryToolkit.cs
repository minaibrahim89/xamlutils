using System.Xml.Linq;

namespace XamlUtils.Tools;

public static class ResourceDictionaryToolkit
{
    private const string XamlNamespace = "http://schemas.microsoft.com/winfx/2006/xaml";
    public static List<string> FindDuplicateKeys(string xaml)
    {
        if (xaml == null)
            throw new ArgumentNullException(nameof(xaml));

        var xml = ParseResourceDictionary(xaml);

        return xml.Descendants()
            .Select(element => element.Attribute(XName.Get("Key", XamlNamespace))?.Value)
            .Where(key => key != null)
            .GroupBy(key => key!)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToList();
    }

    public static List<string> FindKeysDiff(string xaml1, string xaml2)
    {
        if (xaml1 == null)
            throw new ArgumentNullException(nameof(xaml1));

        if (xaml2 == null)
            throw new ArgumentNullException(nameof(xaml2));

        var xml1 = ParseResourceDictionary(xaml1);
        var xml2 = ParseResourceDictionary(xaml2);

        var keys1 = xml1.Descendants()
            .Select(element => element.Attribute(XName.Get("Key", XamlNamespace))?.Value)
            .Where(key => key != null);

        var keys2 = xml2.Descendants()
            .Select(element => element.Attribute(XName.Get("Key", XamlNamespace))?.Value)
            .Where(key => key != null);

        var removedKeys = keys1.Except(keys2).Select(key => $"-{key}");
        var addedKeys = keys2.Except(keys1).Select(key => $"+{key}");

        return removedKeys.Concat(addedKeys).ToList();
    }

    private static XElement ParseResourceDictionary(string xaml)
    {
        var xml = XElement.Parse(xaml);

        if (xml.Name.LocalName != "ResourceDictionary")
            throw new InvalidOperationException("File is not a ResourceDictionary");

        return xml;
    }
}