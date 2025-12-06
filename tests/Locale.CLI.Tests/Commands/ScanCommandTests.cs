using Locale.CLI.Commands;

namespace Locale.CLI.Tests.Commands;

public class ScanCommandTests
{
    [Fact]
    public void ScanSettings_Properties_ShouldSetCorrectly()
    {
        // Arrange & Act
        ScanSettings settings = new()
        {
            Path = "/test/path",
            BaseCulture = "en",
            Targets = "tr,de",
            Recursive = true,
            Output = "report.json",
            Ignore = "*.bak",
            CheckPlaceholders = true
        };

        // Assert
        Assert.Equal("/test/path", settings.Path);
        Assert.Equal("en", settings.BaseCulture);
        Assert.Equal("tr,de", settings.Targets);
        Assert.True(settings.Recursive);
        Assert.Equal("report.json", settings.Output);
        Assert.Equal("*.bak", settings.Ignore);
        Assert.True(settings.CheckPlaceholders);
    }

    [Fact]
    public void ScanSettings_DefaultValues_ShouldBeSet()
    {
        // Arrange & Act
        ScanSettings settings = new()
        {
            Path = "/test"
        };

        // Assert
        Assert.Equal("en", settings.BaseCulture);
        Assert.True(settings.Recursive);
        Assert.True(settings.CheckPlaceholders);
    }
}