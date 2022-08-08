namespace XamlUtils.Tests.Tools;

public class ResourceDictionaryToolkitTests
{
    [Test]
    public void FindDuplicateKeys_Returns_duplicate_keys()
    {
        // Arrange
        var xaml = @"<ResourceDictionary xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                                         xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
                                         xmlns:s=""clr-namespace:System;assembly=mscorlib"">
                        <s:String x:Key=""Key1"">Value</s:String>
                        <s:String x:Key=""Key2"">Value</s:String>
                        <s:String x:Key=""Key3"">Value</s:String>
                        <s:String x:Key=""Key3"">Value</s:String>
                        <s:String x:Key=""Key1"">Value</s:String>
                        <s:String x:Key=""Key2"">Value</s:String>
                        <s:String x:Key=""Key4"">Value</s:String>
                    </ResourceDictionary>";

        // Act        
        List<string> keys = ResourceDictionaryToolkit.FindDuplicateKeys(xaml);

        // Assert
        Assert.That(keys.Count, Is.EqualTo(3));
        Assert.Contains("Key1", keys);
        Assert.Contains("Key2", keys);
        Assert.Contains("Key3", keys);
    }

    [Test]
    public void FindDuplicateKeys_Throws_InvalidOperationException_if_not_resource_dictionary()
    {
        // Arrange
        var xaml = @"<Root></Root>";

        // Act - Assert        
        Assert.Throws<InvalidOperationException>(() => ResourceDictionaryToolkit.FindDuplicateKeys(xaml));
    }

    [Test]
    public void FindDuplicateKeys_Throws_ArgumentNullException_if_argument_is_null()
    {
        // Act - Assert        
        var exception = Assert.Throws<ArgumentNullException>(() => ResourceDictionaryToolkit.FindDuplicateKeys(null!));
        Assert.That(exception!.ParamName, Is.EqualTo("xaml"));
    }
}