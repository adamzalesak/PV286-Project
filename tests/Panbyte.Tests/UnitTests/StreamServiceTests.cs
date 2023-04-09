using Panbyte.App;
using Panbyte.App.Services;
using Xunit;

namespace Panbyte.Tests.UnitTests;

public class StreamServiceTests
{
    private const string TestDataPath = "UnitTests/TestData/";
    private readonly StreamService streamService = new();

    [Fact]
    public void Exists_WhenFileExists_ReturnsTrue()
    {
        Assert.True(streamService.Exists(TestDataPath + "test.txt"));
    }

    [Fact]
    public void Exists_WhenFileDoesNotExists_ReturnsFalse()
    {
        Assert.False(streamService.Exists(TestDataPath + "test2.txt"));
    }

    [Fact]
    public void OpenInput_WhenFileExists_ReturnsReadableStream()
    {
        using var stream = streamService.OpenInputStream(TestDataPath + "test.txt");
        Assert.True(stream.CanRead);
        Assert.True(!stream.CanWrite);
    }

    [Fact]
    public void OpenInput_WhenFileDoesNotExists_Throws()
    {
        Assert.Throws<FileNotFoundException>(() => streamService.OpenInputStream(TestDataPath + "test2.txt"));
    }

    [Fact]
    public void OpenOutput_WhenFileExists_RewritesFileAndReturnsWriteableStream()
    {
        using var stream = streamService.OpenOutputStream(TestDataPath + "test.txt");
        Assert.True(stream.CanWrite);
        Assert.Equal(0, stream.Position);
    }

    [Fact]
    public void OpenOutput_WhenFileDoesNotExists_ReturnsCreatedFileStream()
    {
        using (var stream = streamService.OpenOutputStream(TestDataPath + "test123new.txt"))
        {
            Assert.True(stream.CanWrite);
        }
        File.Delete(TestDataPath + "test123new.txt");
    }

    [Fact]
    public void OpenOutput_WhenStdout_ReturnsWriteableStream()
    {
        using var stream = streamService.OpenOutputStream(Constants.Stdout);
        Assert.True(stream.CanWrite);
    }
}
