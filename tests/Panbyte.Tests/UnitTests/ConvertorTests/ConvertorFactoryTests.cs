using Panbyte.App.Convertors;
using Panbyte.App.Convertors.ArrayTo;
using Panbyte.App.Convertors.BytesTo;
using Panbyte.App.Convertors.HexTo;
using Panbyte.App.Parser;
using Xunit;

namespace Panbyte.Tests.UnitTests.ConvertorTests
{
    public class ConvertorFactoryTests
    {
        [Theory]
        [InlineData(Format.Hex, Format.Bytes, typeof(HexToBytesConvertor))]
        [InlineData(Format.Int, Format.Array, typeof(XToArrayConvertor))]
        [InlineData(Format.Bytes, Format.Bits, typeof(BytesToBitsConvertor))]
        [InlineData(Format.Bits, Format.Bits, typeof(CopyConvertor))]
        [InlineData(Format.Hex, Format.Int, typeof(CommonConvertor))]
        public void Create_WhenValidFormats_ReturnsValidConvertor(Format from, Format to, Type type)
        {
            var convertor = ConvertorFactory.Create(from, to, new List<string>(), new List<string>());
            Assert.True(convertor.GetType() == type);
        }
    }
}
