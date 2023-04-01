using Panbyte.App.Validators;
using Xunit;

namespace Panbyte.Tests.UnitTests.ValidatorTests
{
    public class BitsValidatorTests
    {
        [Theory]
        [InlineData('0')]
        [InlineData('1')]
        public void ValidateBit_WhenValidInput_ReturnsValidOutput(byte b)
        {
            var validator = new BitsValidator();
            var valid = validator.ValidateByte(b);
            Assert.Equal(ByteValidation.Valid, valid);
        }

        [Theory]
        [InlineData(' ')]
        [InlineData('\n')]
        [InlineData('\t')]
        public void ValidateBit_WhenValidInput_ReturnsIgnoredOutput(byte b)
        {
            var validator = new BitsValidator();
            var valid = validator.ValidateByte(b);
            Assert.Equal(ByteValidation.Ignore, valid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData('a')]
        [InlineData('2')]
        [InlineData('x')]
        public void ValidateBit_WhenInvalidInput_ReturnsErrorOutput(byte b)
        {
            var validator = new BitsValidator();
            var valid = validator.ValidateByte(b);
            Assert.Equal(ByteValidation.Error, valid);
        }
    }
}
