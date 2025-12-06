using Locale.CLI.Commands;

namespace Locale.CLI.Tests.Commands;

public class DiffCommandTests
{
    [Fact]
    public void DiffSettings_Properties_ShouldSetCorrectly()
    {
        // Arrange & Act
        DiffSettings settings = new()
        {
            First = "first.json",
            Second = "second.json",
            Output = "diff.json",
            CheckPlaceholders = false
        };

        // Assert
        Assert.Equal("first.json", settings.First);
        Assert.Equal("second.json", settings.Second);
        Assert.Equal("diff.json", settings.Output);
        Assert.False(settings.CheckPlaceholders);
    }

    [Fact]
    public void DiffSettings_DefaultValues_ShouldBeSet()
    {
        // Arrange & Act
        DiffSettings settings = new()
        {
            First = "first.json",
            Second = "second.json"
        };

        // Assert
        Assert.True(settings.CheckPlaceholders);
    }
}