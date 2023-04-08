using Panbyte.App.Convertors;
using Panbyte.App.Convertors.BitsTo;
using Panbyte.App.Convertors.BytesTo;
using Panbyte.Tests.Helpers;
using System.Text;
using Xunit;

namespace Panbyte.Tests.UnitTests.ConvertorTests
{
    public class CommonConvertorTests
    {
        [Fact]
        public void Convert_WhenCombinedConvertors_ReturnsValidOutput()
        {
            var inConvertor = new BitsToBytesConvertor(new("right"));
            var outConvertor = new BytesToHexConvertor();
            var commonConvertor = new CommonConvertor(inConvertor, outConvertor);

            var bytes = Encoding.ASCII.GetBytes("100111101001011");
            using var memoryStream = new MemoryStream();
            commonConvertor.ConvertPart(bytes, memoryStream);
            Assert.Equal("9E96", memoryStream.ToText());
        }
    }
}
