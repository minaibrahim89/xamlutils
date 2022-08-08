using System.Xml.Linq;

namespace XamlUtils.Tools;

public static class ResourceDictionaryToolkit
{
    private const string XamlNamespace = "http://schemas.microsoft.com/winfx/2006/xaml";
    public static List<string> FindDuplicateKeys(string xaml)
    {
        if (xaml == null)
            throw new ArgumentNullException(nameof(xaml));

        var xml = XElement.Parse(xaml);

        if (xml.Name.LocalName != "ResourceDictionary")
            throw new InvalidOperationException("File is not a ResourceDictionary");

        return xml.Descendants()
            .Select(element => element.Attribute(XName.Get("Key", XamlNamespace))?.Value)
            .Where(key => key != null)
            .GroupBy(key => key!)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToList();
    }
}