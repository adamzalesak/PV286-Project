using Panbyte.App.Convertors.BitsTo;
using Panbyte.App.Validators;
using Panbyte.Tests.Helpers;
using Xunit;

namespace Panbyte.Tests.UnitTests.ConvertorTests
{
    public class BitsToBytesTests
    {
        [Theory]
        [InlineData("0", "\0")]
        [InlineData("1000001", "A")]
        [InlineData("100 1111 0100 1011", "OK")]
        [InlineData("100111101001011", "OK")]
        [InlineData("01110100011001010111001101110100", "test")]
        [InlineData("1101100  01101111 0110111001100111 00100000 01101001 01101110 01110000 01110101 01110100", "long input")]
        public void Convert_WhenValidInput_ReturnsValidOutput(string input, string output)
        {
            var convertor = new BitsToBytesConvertor(new(""), new BitsValidator());
            using var stream = input.ToStream();
            using var memoryStream = new MemoryStream();
            convertor.Convert(stream, memoryStream);
            Assert.Equal(output, memoryStream.ToText());
        }

        [Theory]
        [InlineData("0", "\0")]
        [InlineData("0100001", "B")]
        [InlineData("0100 1111 0100 1011", "OK")]
        [InlineData("01110100011001010111001101110100", "test")]
        [InlineData("011101000110010101110011011101", "test")]
        [InlineData("011011000110111101101110011001110010000001101001011011100111000001110101011101", "long input")]
        public void Convert_WhenValidRightPaddedInput_ReturnsValidOutput(string input, string output)
        {
            var convertor = new BitsToBytesConvertor(new("", "right"), new BitsValidator());
            using var stream = input.ToStream();
            using var memoryStream = new MemoryStream();
            convertor.Convert(stream, memoryStream);
            Assert.Equal(output, memoryStream.ToText());
        }
    }
}
