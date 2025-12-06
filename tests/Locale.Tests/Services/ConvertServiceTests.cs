using Locale.Formats;
using Locale.Services;

namespace Locale.Tests.Services;

public class ConvertServiceTests : IDisposable
{
    private readonly string _testDir;
    private readonly ConvertService _service;

    public ConvertServiceTests()
    {
        _testDir = Path.Combine(Path.GetTempPath(), $"locale_convert_test_{Guid.NewGuid():N}");
        Directory.CreateDirectory(_testDir);
        _service = new ConvertService();
    }

    public void Dispose()
    {
        if (Directory.Exists(_testDir))
        {
            Directory.Delete(_testDir, true);
        }
        GC.SuppressFinalize(this);
    }

    [Fact]
    public void Constructor_WithRegistry_ShouldWork()
    {
        // Arrange & Act
        ConvertService service = new(FormatRegistry.Default);

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public void Constructor_Default_ShouldWork()
    {
        // Arrange & Act
        ConvertService service = new();

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public void Convert_JsonToYaml_ShouldSucceed()
    {
        // Arrange
        string sourceJson = Path.Combine(_testDir, "test.json");
        string destYaml = Path.Combine(_testDir, "test.yaml");

        string jsonContent = """
            {
                "Hello": "World",
                "Goodbye": "World"
            }
            """;
        File.WriteAllText(sourceJson, jsonContent);

        ConvertOptions options = new()
        {
            ToFormat = "yaml"
        };

        // Act
        ConvertResult result = _service.Convert(sourceJson, destYaml, options);

        // Assert
        Assert.True(result.Success, result.ErrorMessage);
        Assert.Equal(sourceJson, result.SourcePath);
        Assert.Equal(destYaml, result.DestinationPath);
        Assert.True(File.Exists(destYaml));
    }

    [Fact]
    public void Convert_WithExplicitFormat_ShouldSucceed()
    {
        // Arrange
        string sourceJson = Path.Combine(_testDir, "test.json");
        string destResx = Path.Combine(_testDir, "test.resx");

        string jsonContent = """
            {
                "Title": "My Application"
            }
            """;
        File.WriteAllText(sourceJson, jsonContent);

        ConvertOptions options = new()
        {
            FromFormat = "json",
            ToFormat = "resx"
        };

        // Act
        ConvertResult result = _service.Convert(sourceJson, destResx, options);

        // Assert
        Assert.True(result.Success, result.ErrorMessage);
        Assert.True(File.Exists(destResx));
    }

    [Fact]
    public void Convert_WithCultureOverride_ShouldSucceed()
    {
        // Arrange
        string sourceJson = Path.Combine(_testDir, "test.json");
        string destResx = Path.Combine(_testDir, "test.resx");

        string jsonContent = """
            {
                "Key": "Value"
            }
            """;
        File.WriteAllText(sourceJson, jsonContent);

        ConvertOptions options = new()
        {
            ToFormat = "resx",
            Culture = "tr-TR"
        };

        // Act
        ConvertResult result = _service.Convert(sourceJson, destResx, options);

        // Assert
        Assert.True(result.Success, result.ErrorMessage);
        Assert.True(File.Exists(destResx));
    }

    [Fact]
    public void Convert_WithoutForce_WhenFileExists_ShouldFail()
    {
        // Arrange
        string sourceJson = Path.Combine(_testDir, "source.json");
        string destJson = Path.Combine(_testDir, "dest.json");

        File.WriteAllText(sourceJson, "{}");
        File.WriteAllText(destJson, "{}");

        ConvertOptions options = new()
        {
            ToFormat = "json",
            Force = false
        };

        // Act
        ConvertResult result = _service.Convert(sourceJson, destJson, options);

        // Assert
        Assert.False(result.Success);
        Assert.Contains("already exists", result.ErrorMessage);
    }

    [Fact]
    public void Convert_WithForce_WhenFileExists_ShouldSucceed()
    {
        // Arrange
        string sourceJson = Path.Combine(_testDir, "source.json");
        string destJson = Path.Combine(_testDir, "dest.json");

        File.WriteAllText(sourceJson, "{\"Key\": \"NewValue\"}");
        File.WriteAllText(destJson, "{\"Key\": \"OldValue\"}");

        ConvertOptions options = new()
        {
            ToFormat = "json",
            Force = true
        };

        // Act
        ConvertResult result = _service.Convert(sourceJson, destJson, options);

        // Assert
        Assert.True(result.Success, result.ErrorMessage);
        Assert.True(File.Exists(destJson));
    }

    [Fact]
    public void Convert_WithUnsupportedSourceFormat_ShouldFail()
    {
        // Arrange
        string source = Path.Combine(_testDir, "test.unknown");
        string dest = Path.Combine(_testDir, "test.json");

        File.WriteAllText(source, "content");

        ConvertOptions options = new()
        {
            ToFormat = "json"
        };

        // Act
        ConvertResult result = _service.Convert(source, dest, options);

        // Assert
        Assert.False(result.Success);
        Assert.Contains("Cannot determine format", result.ErrorMessage);
    }

    [Fact]
    public void Convert_WithUnsupportedTargetFormat_ShouldFail()
    {
        // Arrange
        string source = Path.Combine(_testDir, "test.json");
        string dest = Path.Combine(_testDir, "test.unknown");

        File.WriteAllText(source, "{}");

        ConvertOptions options = new()
        {
            ToFormat = "unknownformat"
        };

        // Act
        ConvertResult result = _service.Convert(source, dest, options);

        // Assert
        Assert.False(result.Success);
        Assert.Contains("Unsupported target format", result.ErrorMessage);
    }

    [Fact]
    public void Convert_WithInvalidSourceFile_ShouldFail()
    {
        // Arrange
        string source = Path.Combine(_testDir, "nonexistent.json");
        string dest = Path.Combine(_testDir, "test.yaml");

        ConvertOptions options = new()
        {
            ToFormat = "yaml"
        };

        // Act
        ConvertResult result = _service.Convert(source, dest, options);

        // Assert
        Assert.False(result.Success);
        Assert.NotNull(result.ErrorMessage);
    }

    [Fact]
    public void ConvertDirectory_WithValidFiles_ShouldConvertAll()
    {
        // Arrange
        string sourceDir = Path.Combine(_testDir, "source");
        string destDir = Path.Combine(_testDir, "dest");
        Directory.CreateDirectory(sourceDir);
        Directory.CreateDirectory(destDir);

        // Create test files
        File.WriteAllText(Path.Combine(sourceDir, "file1.json"), "{\"Key1\": \"Value1\"}");
        File.WriteAllText(Path.Combine(sourceDir, "file2.json"), "{\"Key2\": \"Value2\"}");

        ConvertOptions options = new()
        {
            ToFormat = "yaml",
            Recursive = false
        };

        // Act
        List<ConvertResult> results = _service.ConvertDirectory(sourceDir, destDir, options);

        // Assert
        Assert.Equal(2, results.Count);
        Assert.All(results, r => Assert.True(r.Success, r.ErrorMessage));
    }

    [Fact]
    public void ConvertDirectory_WithRecursive_ShouldConvertSubdirectories()
    {
        // Arrange
        string sourceDir = Path.Combine(_testDir, "source");
        string subDir = Path.Combine(sourceDir, "sub");
        string destDir = Path.Combine(_testDir, "dest");
        Directory.CreateDirectory(subDir);
        Directory.CreateDirectory(destDir);

        File.WriteAllText(Path.Combine(sourceDir, "file1.json"), "{\"Key1\": \"Value1\"}");
        File.WriteAllText(Path.Combine(subDir, "file2.json"), "{\"Key2\": \"Value2\"}");

        ConvertOptions options = new()
        {
            ToFormat = "yaml",
            Recursive = true
        };

        // Act
        List<ConvertResult> results = _service.ConvertDirectory(sourceDir, destDir, options);

        // Assert
        Assert.Equal(2, results.Count);
        Assert.All(results, r => Assert.True(r.Success, r.ErrorMessage));
    }

    [Fact]
    public void ConvertDirectory_WithNonRecursive_ShouldNotConvertSubdirectories()
    {
        // Arrange
        string sourceDir = Path.Combine(_testDir, "source");
        string subDir = Path.Combine(sourceDir, "sub");
        string destDir = Path.Combine(_testDir, "dest");
        Directory.CreateDirectory(subDir);
        Directory.CreateDirectory(destDir);

        File.WriteAllText(Path.Combine(sourceDir, "file1.json"), "{\"Key1\": \"Value1\"}");
        File.WriteAllText(Path.Combine(subDir, "file2.json"), "{\"Key2\": \"Value2\"}");

        ConvertOptions options = new()
        {
            ToFormat = "yaml",
            Recursive = false
        };

        // Act
        List<ConvertResult> results = _service.ConvertDirectory(sourceDir, destDir, options);

        // Assert
        Assert.Single(results);
    }

    [Fact]
    public void ConvertDirectory_WithNonexistentDirectory_ShouldReturnError()
    {
        // Arrange
        string sourceDir = Path.Combine(_testDir, "nonexistent");
        string destDir = Path.Combine(_testDir, "dest");

        ConvertOptions options = new()
        {
            ToFormat = "yaml"
        };

        // Act
        List<ConvertResult> results = _service.ConvertDirectory(sourceDir, destDir, options);

        // Assert
        Assert.Single(results);
        Assert.False(results[0].Success);
        Assert.Contains("does not exist", results[0].ErrorMessage);
    }

    [Fact]
    public void ConvertOptions_Properties_ShouldWorkAsExpected()
    {
        // Arrange & Act
        ConvertOptions options = new()
        {
            FromFormat = "json",
            ToFormat = "yaml",
            Force = true,
            Recursive = false,
            Culture = "en-US"
        };

        // Assert
        Assert.Equal("json", options.FromFormat);
        Assert.Equal("yaml", options.ToFormat);
        Assert.True(options.Force);
        Assert.False(options.Recursive);
        Assert.Equal("en-US", options.Culture);
    }

    [Fact]
    public void ConvertResult_Properties_ShouldWorkAsExpected()
    {
        // Arrange & Act
        ConvertResult result = new()
        {
            SourcePath = "source.json",
            DestinationPath = "dest.yaml",
            Success = true,
            ErrorMessage = null
        };

        result.Warnings.Add("Warning 1");
        result.Warnings.Add("Warning 2");

        // Assert
        Assert.Equal("source.json", result.SourcePath);
        Assert.Equal("dest.yaml", result.DestinationPath);
        Assert.True(result.Success);
        Assert.Null(result.ErrorMessage);
        Assert.Equal(2, result.Warnings.Count);
    }
}