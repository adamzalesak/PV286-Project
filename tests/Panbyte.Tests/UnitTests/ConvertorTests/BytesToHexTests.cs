using Panbyte.App.Convertors.BytesTo;
using Panbyte.Tests.Helpers;
using System.Text;
using Xunit;

namespace Panbyte.Tests.UnitTests.ConvertorTests
{
    public class BytesToHexTests
    {
        [Theory]
        [InlineData("", "")]
        [InlineData("   ", "202020")]
        [InlineData("A", "41")]
        [InlineData("OK", "4F4B")]
        [InlineData("test", "74657374")]
        [InlineData("\nabcd", "0A61626364")]
        [InlineData("testtesttest", "746573747465737474657374")]
        [InlineData("long byte input to hex output", "6C6F6E67206279746520696E70757420746F20686578206F7574707574")]
        public void Convert_WhenValidInput_ReturnsValidOutput(string input, string output)
        {
            var convertor = new BytesToHexConvertor();
            var bytes = Encoding.ASCII.GetBytes(input);
            using var memoryStream = new MemoryStream();
            convertor.ConvertPart(bytes, memoryStream);
            Assert.Equal(output, memoryStream.ToText());
        }
    }
}
