using Panbyte.App.Services;
using Xunit;

namespace Panbyte.Tests.UnitTests;

public class StreamServiceTests
{
    private readonly StreamService streamService = new();

    [Fact]
    public void Exists_WhenFileExists_ReturnsTrue()
    {
        Assert.True(streamService.Exists("TestData/test.txt"));
    }

    [Fact]
    public void Open_WhenFileExists_ReturnsStream()
    {
        using var reader = new StreamReader(streamService.OpenInputStream("TestData/test.txt"));
        var text = reader.ReadToEnd();
        Assert.StartsWith("test", text);
    }
}
