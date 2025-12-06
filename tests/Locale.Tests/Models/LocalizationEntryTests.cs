using Locale.Models;

namespace Locale.Tests.Models;

public class LocalizationEntryTests
{
    [Fact]
    public void Properties_ShouldSetAndGetCorrectly()
    {
        // Arrange & Act
        LocalizationEntry entry = new()
        {
            Key = "TestKey",
            Value = "TestValue",
            Comment = "Test Comment",
            Source = "Source Text"
        };

        // Assert
        Assert.Equal("TestKey", entry.Key);
        Assert.Equal("TestValue", entry.Value);
        Assert.Equal("Test Comment", entry.Comment);
        Assert.Equal("Source Text", entry.Source);
    }

    [Fact]
    public void IsEmpty_WithNullValue_ShouldReturnTrue()
    {
        // Arrange
        LocalizationEntry entry = new()
        {
            Key = "Key",
            Value = null
        };

        // Act & Assert
        Assert.True(entry.IsEmpty);
    }

    [Fact]
    public void IsEmpty_WithEmptyValue_ShouldReturnTrue()
    {
        // Arrange
        LocalizationEntry entry = new()
        {
            Key = "Key",
            Value = ""
        };

        // Act & Assert
        Assert.True(entry.IsEmpty);
    }

    [Fact]
    public void IsEmpty_WithWhitespaceValue_ShouldReturnTrue()
    {
        // Arrange
        LocalizationEntry entry = new()
        {
            Key = "Key",
            Value = "   \t\n"
        };

        // Act & Assert
        Assert.True(entry.IsEmpty);
    }

    [Fact]
    public void IsEmpty_WithNonEmptyValue_ShouldReturnFalse()
    {
        // Arrange
        LocalizationEntry entry = new()
        {
            Key = "Key",
            Value = "Value"
        };

        // Act & Assert
        Assert.False(entry.IsEmpty);
    }

    [Fact]
    public void ToString_ShouldReturnFormattedString()
    {
        // Arrange
        LocalizationEntry entry = new()
        {
            Key = "Welcome",
            Value = "Hello World"
        };

        // Act
        string result = entry.ToString();

        // Assert
        Assert.Equal("Welcome = Hello World", result);
    }

    [Fact]
    public void Equals_WithSameKeyAndValue_ShouldReturnTrue()
    {
        // Arrange
        LocalizationEntry entry1 = new()
        {
            Key = "Key",
            Value = "Value"
        };

        LocalizationEntry entry2 = new()
        {
            Key = "Key",
            Value = "Value"
        };

        // Act & Assert
        Assert.True(entry1.Equals(entry2));
        Assert.True(entry2.Equals(entry1));
    }

    [Fact]
    public void Equals_WithDifferentKey_ShouldReturnFalse()
    {
        // Arrange
        LocalizationEntry entry1 = new()
        {
            Key = "Key1",
            Value = "Value"
        };

        LocalizationEntry entry2 = new()
        {
            Key = "Key2",
            Value = "Value"
        };

        // Act & Assert
        Assert.False(entry1.Equals(entry2));
    }

    [Fact]
    public void Equals_WithDifferentValue_ShouldReturnFalse()
    {
        // Arrange
        LocalizationEntry entry1 = new()
        {
            Key = "Key",
            Value = "Value1"
        };

        LocalizationEntry entry2 = new()
        {
            Key = "Key",
            Value = "Value2"
        };

        // Act & Assert
        Assert.False(entry1.Equals(entry2));
    }

    [Fact]
    public void Equals_WithNull_ShouldReturnFalse()
    {
        // Arrange
        LocalizationEntry entry = new()
        {
            Key = "Key",
            Value = "Value"
        };

        // Act & Assert
        Assert.False(entry.Equals(null));
    }

    [Fact]
    public void Equals_WithSameReference_ShouldReturnTrue()
    {
        // Arrange
        LocalizationEntry entry = new()
        {
            Key = "Key",
            Value = "Value"
        };

        // Act & Assert
        Assert.True(entry.Equals(entry));
    }

    [Fact]
    public void Equals_WithObjectType_ShouldWork()
    {
        // Arrange
        LocalizationEntry entry1 = new()
        {
            Key = "Key",
            Value = "Value"
        };

        object entry2 = new LocalizationEntry
        {
            Key = "Key",
            Value = "Value"
        };

        // Act & Assert
        Assert.True(entry1.Equals(entry2));
    }

    [Fact]
    public void GetHashCode_WithSameKeyAndValue_ShouldReturnSameHash()
    {
        // Arrange
        LocalizationEntry entry1 = new()
        {
            Key = "Key",
            Value = "Value"
        };

        LocalizationEntry entry2 = new()
        {
            Key = "Key",
            Value = "Value"
        };

        // Act
        int hash1 = entry1.GetHashCode();
        int hash2 = entry2.GetHashCode();

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void GetHashCode_WithDifferentKeyOrValue_ShouldReturnDifferentHash()
    {
        // Arrange
        LocalizationEntry entry1 = new()
        {
            Key = "Key1",
            Value = "Value"
        };

        LocalizationEntry entry2 = new()
        {
            Key = "Key2",
            Value = "Value"
        };

        // Act
        int hash1 = entry1.GetHashCode();
        int hash2 = entry2.GetHashCode();

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void OperatorEquals_WithEqualEntries_ShouldReturnTrue()
    {
        // Arrange
        LocalizationEntry entry1 = new()
        {
            Key = "Key",
            Value = "Value"
        };

        LocalizationEntry entry2 = new()
        {
            Key = "Key",
            Value = "Value"
        };

        // Act & Assert
        Assert.True(entry1 == entry2);
    }

    [Fact]
    public void OperatorEquals_WithDifferentEntries_ShouldReturnFalse()
    {
        // Arrange
        LocalizationEntry entry1 = new()
        {
            Key = "Key1",
            Value = "Value"
        };

        LocalizationEntry entry2 = new()
        {
            Key = "Key2",
            Value = "Value"
        };

        // Act & Assert
        Assert.False(entry1 == entry2);
    }

    [Fact]
    public void OperatorEquals_WithBothNull_ShouldReturnTrue()
    {
        // Arrange
        LocalizationEntry? entry1 = null;
        LocalizationEntry? entry2 = null;

        // Act & Assert
        Assert.True(entry1 == entry2);
    }

    [Fact]
    public void OperatorEquals_WithOneNull_ShouldReturnFalse()
    {
        // Arrange
        LocalizationEntry? entry1 = new()
        {
            Key = "Key",
            Value = "Value"
        };
        LocalizationEntry? entry2 = null;

        // Act & Assert
        Assert.False(entry1 == entry2);
        Assert.False(entry2 == entry1);
    }

    [Fact]
    public void OperatorNotEquals_WithEqualEntries_ShouldReturnFalse()
    {
        // Arrange
        LocalizationEntry entry1 = new()
        {
            Key = "Key",
            Value = "Value"
        };

        LocalizationEntry entry2 = new()
        {
            Key = "Key",
            Value = "Value"
        };

        // Act & Assert
        Assert.False(entry1 != entry2);
    }

    [Fact]
    public void OperatorNotEquals_WithDifferentEntries_ShouldReturnTrue()
    {
        // Arrange
        LocalizationEntry entry1 = new()
        {
            Key = "Key1",
            Value = "Value"
        };

        LocalizationEntry entry2 = new()
        {
            Key = "Key2",
            Value = "Value"
        };

        // Act & Assert
        Assert.True(entry1 != entry2);
    }
}