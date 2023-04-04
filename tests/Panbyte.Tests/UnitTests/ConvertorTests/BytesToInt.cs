using Panbyte.App.Convertors;
using Panbyte.App.Convertors.BytesTo;
using Panbyte.Tests.Helpers;
using Xunit;

namespace Panbyte.Tests.UnitTests.ConvertorTests;

public class BytesToInt
{
    [Theory]
    [InlineData(new byte[] { 0, 0, 0, 0 }, "0")]
    [InlineData(new byte[] { 0, 0, 0, 0 }, "0", "little")]
    [InlineData(new byte[] { 0, 0, 0, 1 }, "1")]
    [InlineData(new byte[] { 1, 0, 0, 0 }, "1", "little")]
    [InlineData(new byte[] { 0, 0, 0, 50 }, "50")]
    [InlineData(new byte[] { 50, 0, 0, 0 }, "50", "little")]
    [InlineData(new byte[] { 0, 0, 0, 64 }, "64")]
    [InlineData(new byte[] { 64, 0, 0, 0 }, "64", "little")]
    [InlineData(new byte[] { 255, 255, 255, 255 }, "4294967295")]
    [InlineData(new byte[] { 255, 255, 255, 255 }, "4294967295", "little")]
    public void Convert_WhenValidInput_ReturnsValidOutput(byte[] input, string output, string? endian = "big")
    {
        var convertor = new BytesToIntConvertor(new ConvertorOptions("", endian!));
        using var memoryStream = new MemoryStream();
        convertor.ConvertPart(input, memoryStream);
        Assert.Equal(output, memoryStream.ToText());
    }
}