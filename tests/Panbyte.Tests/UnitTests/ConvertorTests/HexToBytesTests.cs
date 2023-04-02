using Panbyte.App.Convertors.HexTo;
using Panbyte.App.Validators;
using Panbyte.Tests.Helpers;
using System.Text;
using Xunit;

namespace Panbyte.Tests.UnitTests.ConvertorTests
{
    public class HexToBytesTests
    {
        [Theory]
        [InlineData("00", "\0")]
        [InlineData("41", "A")]
        [InlineData("4f 4b", "OK")]
        [InlineData("74657374", "test")]
        [InlineData("74  65 7374", "test")]
        [InlineData("68657820746f2062696e617279", "hex to binary")]
        [InlineData("6A6B6C6D6E6f70", "jklmnop")]
        public void Convert_WhenValidInput_ReturnsValidOutput(string input, string output)
        {
            var convertor = new HexToBytesConvertor();
            var bytes = Encoding.ASCII.GetBytes(input);
            using var memoryStream = new MemoryStream();
            convertor.ConvertPart(bytes, memoryStream);
            Assert.Equal(output, memoryStream.ToText());
        }
    }
}
