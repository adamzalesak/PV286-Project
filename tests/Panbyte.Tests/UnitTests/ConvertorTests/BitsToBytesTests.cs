using Panbyte.App.Convertors.BitsTo;
using Panbyte.App.Validators;
using Panbyte.Tests.Helpers;
using System.Text;
using Xunit;

namespace Panbyte.Tests.UnitTests.ConvertorTests
{
    public class BitsToBytesTests
    {
        [Theory]
        [InlineData("0", "\0")]
        [InlineData("1000001", "A")]
        [InlineData("100111101001011", "OK")]
        [InlineData("01110100011001010111001101110100", "test")]
        [InlineData("1101100011011110110111001100111001000000110100101101110011100000111010101110100", "long input")]
        public void Convert_WhenValidInput_ReturnsValidOutput(string input, string output)
        {
            var convertor = new BitsToBytesConvertor(new("left"));
            var bytes = Encoding.ASCII.GetBytes(input);
            using var memoryStream = new MemoryStream();
            convertor.ConvertPart(bytes, memoryStream);
            Assert.Equal(output, memoryStream.ToText());
        }

        [Theory]
        [InlineData("0", "\0")]
        [InlineData("0100001", "B")]
        [InlineData("0100111101001011", "OK")]
        [InlineData("01110100011001010111001101110100", "test")]
        [InlineData("011101000110010101110011011101", "test")]
        [InlineData("011011000110111101101110011001110010000001101001011011100111000001110101011101", "long input")]
        public void Convert_WhenValidRightPaddedInput_ReturnsValidOutput(string input, string output)
        {
            var convertor = new BitsToBytesConvertor(new("right"));
            var bytes = Encoding.ASCII.GetBytes(input);
            using var memoryStream = new MemoryStream();
            convertor.ConvertPart(bytes, memoryStream);
            Assert.Equal(output, memoryStream.ToText());
        }
    }
}
