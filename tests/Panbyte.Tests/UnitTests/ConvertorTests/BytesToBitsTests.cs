using Panbyte.App.Convertors;
using Panbyte.App.Validators;
using Panbyte.Tests.Helpers;
using Xunit;

namespace Panbyte.Tests.UnitTests.ConvertorTests;

public class BytesToBitsTests
{
    [Theory]
    [InlineData("test", "01110100011001010111001101110100")]
    public void Convert_WhenValidInput_ReturnsValidOutput(string input, string output)
    {
        var convertor = new BytesToBitsConvertor(new(""), new DefaultValidator());
        using var stream = input.ToStream();
        using var memoryStream = new MemoryStream();
        convertor.Convert(stream, memoryStream);
        Assert.Equal(output, memoryStream.ToText());
    }
}
