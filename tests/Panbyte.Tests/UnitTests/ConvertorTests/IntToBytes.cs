using System.Text;
using Panbyte.App.Convertors;
using Panbyte.App.Convertors.IntTo;
using Panbyte.Tests.Helpers;
using Xunit;

namespace Panbyte.Tests.UnitTests.ConvertorTests;

public class IntToBytes
{
    [Theory]
    [InlineData("0", new byte[] { 0, 0, 0, 0 })]
    [InlineData("0", new byte[] { 0, 0, 0, 0 }, "little")]
    [InlineData("1", new byte[] { 0, 0, 0, 1 })]
    [InlineData("1", new byte[] { 1, 0, 0, 0 }, "little")]
    [InlineData("50", new byte[] { 0, 0, 0, 50 })]
    [InlineData("50", new byte[] { 50, 0, 0, 0 }, "little")]
    [InlineData("64", new byte[] { 0, 0, 0, 64 })]
    [InlineData("64", new byte[] { 64, 0, 0, 0 }, "little")]
    [InlineData("4294967295", new byte[] { 255, 255, 255, 255 })]
    [InlineData("4294967295", new byte[] { 255, 255, 255, 255 }, "little")]
    public void Convert_WhenValidInput_ReturnsValidOutput(string input, byte[] output, string? endian = "big")
    {
        var convertor = new IntToBytesConvertor(new ConvertorOptions(endian));
        using var memoryStream = new MemoryStream();
        convertor.ConvertPart(Encoding.ASCII.GetBytes(input), memoryStream);
        Assert.Equal(output, memoryStream.ToArray());
    }
}