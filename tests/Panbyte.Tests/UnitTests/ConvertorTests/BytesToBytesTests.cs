using Panbyte.App.Convertors;
using Panbyte.Tests.Helpers;
using System.Text;
using Xunit;

namespace Panbyte.Tests.UnitTests.ConvertorTests
{
    public class BytesToBytesTests
    {
        [Theory]
        [InlineData("0", "0")]
        [InlineData("123", "123")]
        [InlineData("test", "test")]
        [InlineData("test\n", "test\n")]
        [InlineData("testtest   test", "testtest   test")]
        [InlineData("\t testinput$*@\n testinput", "\t testinput$*@\n testinput")]
        public void Convert_WhenValidInput_ReturnsValidOutput(string input, string output)
        {
            var convertor = new CopyConvertor();
            var bytes = Encoding.ASCII.GetBytes(input);
            using var memoryStream = new MemoryStream();
            convertor.ConvertPart(bytes, memoryStream);
            Assert.Equal(output, memoryStream.ToText());
        }
    }
}
