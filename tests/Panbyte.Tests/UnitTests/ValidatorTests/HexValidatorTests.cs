using Panbyte.App.Validators;
using Xunit;

namespace Panbyte.Tests.UnitTests.ValidatorTests
{
    public class HexValidatorTests
    {
        [Theory]
        [InlineData('0')]
        [InlineData('3')]
        [InlineData('9')]
        [InlineData('a')]
        [InlineData('c')]
        [InlineData('f')]
        [InlineData('A')]
        [InlineData('D')]
        [InlineData('F')]
        public void ValidateHex_WhenValidInput_ReturnsValidOutput(byte b)
        {
            var validator = new HexValidator();
            var valid = validator.ValidateByte(b);
            Assert.Equal(ByteValidation.Valid, valid);
        }

        [Theory]
        [InlineData(' ')]
        [InlineData('\n')]
        [InlineData('\t')]
        public void ValidateHex_WhenValidInput_ReturnsIgnoredOutput(byte b)
        {
            var validator = new HexValidator();
            var valid = validator.ValidateByte(b);
            Assert.Equal(ByteValidation.Ignore, valid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(7)]
        [InlineData(15)]
        [InlineData('/')]
        [InlineData('g')]
        [InlineData('@')]
        [InlineData('G')]
        [InlineData('x')]
        public void ValidateHex_WhenInvalidInput_ReturnsErrorOutput(byte b)
        {
            var validator = new HexValidator();
            var valid = validator.ValidateByte(b);
            Assert.Equal(ByteValidation.Error, valid);
        }
    }
}
