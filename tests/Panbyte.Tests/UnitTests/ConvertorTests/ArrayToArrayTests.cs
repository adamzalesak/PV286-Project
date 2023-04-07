using System.Text;
using Panbyte.App.Convertors.ArrayTo;
using Panbyte.App.Parser;
using Panbyte.Tests.Helpers;
using Xunit;

namespace Panbyte.Tests.UnitTests.ConvertorTests;

public class ArrayToArrayTests
{
    [Theory]
    [InlineData(new[] { "0" }, "(0x01, 2, 0b11, '\x04')", "{1, 2, 3, 4}")]
    [InlineData(new[] { "0", "{}" }, "(0x01, 2, 0b11, '\x04')", "{1, 2, 3, 4}")]
    [InlineData(new[] { "0", "[]" }, "(0x01, 2, 0b11, '\x04')", "[1, 2, 3, 4]")]
    [InlineData(new[] { "0", "()" }, "(0x01, 2, 0b11, '\x04')", "(1, 2, 3, 4)")]
    [InlineData(new[] { "0x", "()" }, "(0x01, 2, 0b11, '\x04')", "(0x1, 0x2, 0x3, 0x4)")]
    [InlineData(new[] { "0b", "[]" }, "(0x01, 2, 0b11, '\x04')", "[0b1, 0b10, 0b11, 0b100]")]
    [InlineData(new[] { "a", "{}" }, "{0x01, 2, 0b11, '\x04'}", "{'\\x01', '\\x02', '\\x03', '\\x04'}")]
    public void Convert_WhenValidInput_ReturnsValidOutput(
        string[] outputOptions,
        string input,
        string output
    )
    {
        var convertorOptions = new ArrayConvertorOptions(outputOptions, "", "", Format.Array);
        var convertor = new ArrayToArrayConvertor(convertorOptions);
        var bytes = Encoding.ASCII.GetBytes(input);
        using var memoryStream = new MemoryStream();
        convertor.ConvertPart(bytes, memoryStream);
        Assert.Equal(output, memoryStream.ToText());
    }
}