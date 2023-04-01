using Panbyte.App.Convertors.BytesTo;
using Panbyte.Tests.Helpers;
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
            var convertor = new CopyBytesConvertor();
            using var stream = input.ToStream();
            using var memoryStream = new MemoryStream();
            convertor.Convert(stream, memoryStream);
            Assert.Equal(output, memoryStream.ToText());
        }
    }
}
