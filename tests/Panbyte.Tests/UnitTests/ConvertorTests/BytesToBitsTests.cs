using Panbyte.App.Convertors.BytesTo;
using Panbyte.Tests.Helpers;
using System.Text;
using Xunit;

namespace Panbyte.Tests.UnitTests.ConvertorTests;

public class BytesToBitsTests
{
    [Theory]
    [InlineData("test", "01110100011001010111001101110100")]
    public void Convert_WhenValidInput_ReturnsValidOutput(string input, string output)
    {
        var convertor = new BytesToBitsConvertor();
        using var memoryStream = new MemoryStream();
        convertor.ConvertPart(Encoding.ASCII.GetBytes(input), memoryStream);
        Assert.Equal(output, memoryStream.ToText());
    }
}
