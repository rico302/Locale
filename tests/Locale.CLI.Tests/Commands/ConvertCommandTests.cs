using Locale.CLI.Commands;

namespace Locale.CLI.Tests.Commands;

public class ConvertCommandTests
{
    [Fact]
    public void ConvertSettings_Properties_ShouldSetCorrectly()
    {
        // Arrange & Act
        ConvertSettings settings = new()
        {
            Source = "source.json",
            Destination = "dest.yaml",
            FromFormat = "json",
            ToFormat = "yaml",
            Force = true,
            Recursive = false,
            Culture = "tr-TR"
        };

        // Assert
        Assert.Equal("source.json", settings.Source);
        Assert.Equal("dest.yaml", settings.Destination);
        Assert.Equal("json", settings.FromFormat);
        Assert.Equal("yaml", settings.ToFormat);
        Assert.True(settings.Force);
        Assert.False(settings.Recursive);
        Assert.Equal("tr-TR", settings.Culture);
    }

    [Fact]
    public void ConvertSettings_DefaultValues_ShouldBeSet()
    {
        // Arrange & Act
        ConvertSettings settings = new()
        {
            Source = "source.json",
            Destination = "dest.yaml",
            ToFormat = "yaml"
        };

        // Assert
        Assert.False(settings.Force);
        Assert.True(settings.Recursive);
    }
}