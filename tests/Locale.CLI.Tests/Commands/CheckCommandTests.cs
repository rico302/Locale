using Locale.CLI.Commands;

namespace Locale.CLI.Tests.Commands;

public class CheckCommandTests
{
    [Fact]
    public void CheckSettings_Properties_ShouldSetCorrectly()
    {
        // Arrange & Act
        CheckSettings settings = new()
        {
            Path = "/test/path",
            Rules = "no-empty-values,no-duplicate-keys",
            BaseCulture = "en",
            CiMode = true,
            Output = "check-report.json",
            Recursive = false
        };

        // Assert
        Assert.Equal("/test/path", settings.Path);
        Assert.Equal("no-empty-values,no-duplicate-keys", settings.Rules);
        Assert.Equal("en", settings.BaseCulture);
        Assert.True(settings.CiMode);
        Assert.Equal("check-report.json", settings.Output);
        Assert.False(settings.Recursive);
    }

    [Fact]
    public void CheckSettings_DefaultValues_ShouldBeSet()
    {
        // Arrange & Act
        CheckSettings settings = new()
        {
            Path = "/test"
        };

        // Assert
        Assert.True(settings.Recursive);
        Assert.False(settings.CiMode);
    }
}