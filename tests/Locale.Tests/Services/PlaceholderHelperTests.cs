using Locale.Services;
using System.Text.RegularExpressions;

namespace Locale.Tests.Services;

public class PlaceholderHelperTests
{
    [Fact]
    public void ExtractPlaceholders_WithDefaultPattern_ShouldExtractPlaceholders()
    {
        // Arrange
        string value = "Hello {name}, welcome to {place}!";

        // Act
        List<string> placeholders = PlaceholderHelper.ExtractPlaceholders(value);

        // Assert
        Assert.Equal(2, placeholders.Count);
        Assert.Contains("{name}", placeholders);
        Assert.Contains("{place}", placeholders);
    }

    [Fact]
    public void ExtractPlaceholders_WithDoubleBraces_ShouldExtract()
    {
        // Arrange
        string value = "Value: {{count}}";

        // Act
        List<string> placeholders = PlaceholderHelper.ExtractPlaceholders(value);

        // Assert
        Assert.Single(placeholders);
        Assert.Contains("{{count}}", placeholders);
    }

    [Fact]
    public void ExtractPlaceholders_WithNullValue_ShouldReturnEmpty()
    {
        // Arrange
        string? value = null;

        // Act
        List<string> placeholders = PlaceholderHelper.ExtractPlaceholders(value);

        // Assert
        Assert.Empty(placeholders);
    }

    [Fact]
    public void ExtractPlaceholders_WithEmptyValue_ShouldReturnEmpty()
    {
        // Arrange
        string value = "";

        // Act
        List<string> placeholders = PlaceholderHelper.ExtractPlaceholders(value);

        // Assert
        Assert.Empty(placeholders);
    }

    [Fact]
    public void ExtractPlaceholders_WithNoPlaceholders_ShouldReturnEmpty()
    {
        // Arrange
        string value = "Just plain text without any placeholders";

        // Act
        List<string> placeholders = PlaceholderHelper.ExtractPlaceholders(value);

        // Assert
        Assert.Empty(placeholders);
    }

    [Fact]
    public void ExtractPlaceholders_WithMultipleSamePlaceholder_ShouldReturnDistinct()
    {
        // Arrange
        string value = "Hello {name}, goodbye {name}";

        // Act
        List<string> placeholders = PlaceholderHelper.ExtractPlaceholders(value);

        // Assert
        Assert.Equal(2, placeholders.Count);
        Assert.Equal("{name}", placeholders[0]);
        Assert.Equal("{name}", placeholders[1]);
    }

    [Fact]
    public void ExtractPlaceholders_ShouldReturnSorted()
    {
        // Arrange
        string value = "{zebra} {apple} {banana}";

        // Act
        List<string> placeholders = PlaceholderHelper.ExtractPlaceholders(value);

        // Assert
        Assert.Equal(3, placeholders.Count);
        Assert.Equal("{apple}", placeholders[0]);
        Assert.Equal("{banana}", placeholders[1]);
        Assert.Equal("{zebra}", placeholders[2]);
    }

    [Fact]
    public void ExtractPlaceholders_WithCustomRegex_ShouldUseCustomPattern()
    {
        // Arrange
        string value = "Price: $100, Discount: $20";
        Regex customRegex = new(@"\$\d+", RegexOptions.Compiled);

        // Act
        List<string> placeholders = PlaceholderHelper.ExtractPlaceholders(value, customRegex);

        // Assert
        Assert.Equal(2, placeholders.Count);
        Assert.Contains("$100", placeholders);
        Assert.Contains("$20", placeholders);
    }

    [Fact]
    public void GetRegex_WithNullPattern_ShouldReturnDefaultRegex()
    {
        // Arrange
        string? pattern = null;

        // Act
        Regex regex = PlaceholderHelper.GetRegex(pattern);

        // Assert
        Assert.NotNull(regex);
        Assert.Equal(PlaceholderHelper.DefaultPlaceholderRegex(), regex);
    }

    [Fact]
    public void GetRegex_WithEmptyPattern_ShouldReturnDefaultRegex()
    {
        // Arrange
        string pattern = "";

        // Act
        Regex regex = PlaceholderHelper.GetRegex(pattern);

        // Assert
        Assert.NotNull(regex);
        Assert.Equal(PlaceholderHelper.DefaultPlaceholderRegex(), regex);
    }

    [Fact]
    public void GetRegex_WithDefaultPattern_ShouldReturnDefaultRegex()
    {
        // Arrange
        string pattern = PlaceholderHelper.DefaultPlaceholderPattern;

        // Act
        Regex regex = PlaceholderHelper.GetRegex(pattern);

        // Assert
        Assert.NotNull(regex);
        Assert.Equal(PlaceholderHelper.DefaultPlaceholderRegex(), regex);
    }

    [Fact]
    public void GetRegex_WithCustomPattern_ShouldReturnCustomRegex()
    {
        // Arrange
        string pattern = @"\{\{\w+\}\}";

        // Act
        Regex regex = PlaceholderHelper.GetRegex(pattern);

        // Assert
        Assert.NotNull(regex);
        Assert.NotEqual(PlaceholderHelper.DefaultPlaceholderRegex(), regex);
    }

    [Fact]
    public void DefaultPlaceholderPattern_ShouldBeCorrect()
    {
        // Assert
        Assert.Equal(@"\{+\w+\}+", PlaceholderHelper.DefaultPlaceholderPattern);
    }

    [Fact]
    public void DefaultPlaceholderRegex_ShouldMatchSingleBraces()
    {
        // Arrange
        Regex regex = PlaceholderHelper.DefaultPlaceholderRegex();

        // Act
        bool matches = regex.IsMatch("{test}");

        // Assert
        Assert.True(matches);
    }

    [Fact]
    public void DefaultPlaceholderRegex_ShouldMatchDoubleBraces()
    {
        // Arrange
        Regex regex = PlaceholderHelper.DefaultPlaceholderRegex();

        // Act
        bool matches = regex.IsMatch("{{test}}");

        // Assert
        Assert.True(matches);
    }

    [Fact]
    public void DefaultPlaceholderRegex_ShouldNotMatchIncompleteBraces()
    {
        // Arrange
        Regex regex = PlaceholderHelper.DefaultPlaceholderRegex();

        // Act
        bool matches = regex.IsMatch("{test");

        // Assert
        Assert.False(matches);
    }
}