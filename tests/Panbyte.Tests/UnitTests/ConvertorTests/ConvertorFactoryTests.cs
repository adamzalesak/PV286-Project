using Panbyte.App.Convertors;
using Panbyte.App.Convertors.HexTo;
using Panbyte.App.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Panbyte.Tests.UnitTests.ConvertorTests
{
    public class ConvertorFactoryTests
    {
        [Fact]
        public void Create_WhenValidFormats_ReturnsValidConvertor()
        {
            var convertor = ConvertorFactory.Create(Format.Hex, Format.Bytes, new List<string>(), new List<string>());
            Assert.True(convertor.GetType() == typeof(HexToBytesConvertor));
        }
    }
}
