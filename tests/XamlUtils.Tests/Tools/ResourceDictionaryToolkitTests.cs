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

    [Test]
    public void FindKeysDiff_Returns_removed_keys()
    {
        // Arrange
        var xaml1 = @"<ResourceDictionary xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                                          xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
                                          xmlns:s=""clr-namespace:System;assembly=mscorlib"">
                        <s:String x:Key=""Key"">Value</s:String>
                   </ResourceDictionary>";

        var xaml2 = @"<ResourceDictionary xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                                          xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
                                          xmlns:s=""clr-namespace:System;assembly=mscorlib"">
                   </ResourceDictionary>";

        // Act
        List<string> diff = ResourceDictionaryToolkit.FindKeysDiff(xaml1, xaml2);

        // Assert
        Assert.That(diff, Contains.Item("-Key"));
    }

    [Test]
    public void FindKeysDiff_Returns_added_keys()
    {
        // Arrange
        var xaml1 = @"<ResourceDictionary xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                                          xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
                                          xmlns:s=""clr-namespace:System;assembly=mscorlib"">
                   </ResourceDictionary>";

        var xaml2 = @"<ResourceDictionary xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                                          xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
                                          xmlns:s=""clr-namespace:System;assembly=mscorlib"">
                        <s:String x:Key=""Key"">Value</s:String>
                   </ResourceDictionary>";

        // Act
        List<string> diff = ResourceDictionaryToolkit.FindKeysDiff(xaml1, xaml2);

        // Assert
        Assert.That(diff, Contains.Item("+Key"));
    }

    [Test]
    public void FindKeysDiff_Returns_removed_and_added_keys()
    {
        // Arrange
        var xaml1 = @"<ResourceDictionary xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                                          xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
                                          xmlns:s=""clr-namespace:System;assembly=mscorlib"">
                        <s:String x:Key=""Key1"">Value</s:String>
                        <s:String x:Key=""Key2"">Value</s:String>
                   </ResourceDictionary>";

        var xaml2 = @"<ResourceDictionary xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                                          xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
                                          xmlns:s=""clr-namespace:System;assembly=mscorlib"">
                        <s:String x:Key=""Key2"">Value</s:String>
                        <s:String x:Key=""Key3"">Value</s:String>
                   </ResourceDictionary>";

        // Act
        List<string> diff = ResourceDictionaryToolkit.FindKeysDiff(xaml1, xaml2);

        // Assert
        Assert.That(diff, Contains.Item("-Key1"));
        Assert.That(diff, Contains.Item("+Key3"));
    }

    [Test]
    public void FindKeysDiff_Returns_empty_list_when_identical_keys()
    {
        // Arrange
        var xaml1 = @"<ResourceDictionary xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                                          xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
                                          xmlns:s=""clr-namespace:System;assembly=mscorlib"">
                        <s:String x:Key=""Key"">Value</s:String>
                   </ResourceDictionary>";

        var xaml2 = @"<ResourceDictionary xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                                          xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
                                          xmlns:s=""clr-namespace:System;assembly=mscorlib"">
                        <s:String x:Key=""Key"">Value</s:String>
                   </ResourceDictionary>";

        // Act
        List<string> diff = ResourceDictionaryToolkit.FindKeysDiff(xaml1, xaml2);

        // Assert
        Assert.That(diff.Count, Is.EqualTo(0));
    }

    [Theory]
    [TestCase("<Root/>", "<ResourceDictionary/>")]
    [TestCase("<ResourceDictionary/>", "<Root/>")]
    [TestCase("<Root/>", "<Root/>")]
    [TestCase("<Root/>", "<Root/>")]
    public void FindKeysDiff_Throws_InvalidOperationException_if_any_argument_is_not_resource_dictionary(string xaml1, string xaml2)
    {
        // Act - Assert        
        Assert.Throws<InvalidOperationException>(() => ResourceDictionaryToolkit.FindKeysDiff(xaml1, xaml2));
    }

    [Theory]
    [TestCase(null, "<ResourceDictionary/>")]
    [TestCase("<ResourceDictionary/>", null)]
    [TestCase(null, null)]
    public void FindKeysDiff_Throws_ArgumentNullException_if_any_argument_is_null(string xaml1, string xaml2)
    {
        // Act - Assert        
        var exception = Assert.Throws<ArgumentNullException>(() => ResourceDictionaryToolkit.FindKeysDiff(xaml1, xaml2));
        if (xaml1 == null)
            Assert.That(exception!.ParamName, Is.EqualTo("xaml1"));
        if (xaml1 != null && xaml2 == null)
            Assert.That(exception!.ParamName, Is.EqualTo("xaml2"));
    }
}