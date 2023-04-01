using Panbyte.App.Validators;
using Xunit;

namespace Panbyte.Tests.UnitTests.ValidatorTests
{
    public class IntValidatorTests
    {
        [Theory]
        [InlineData('0')]
        [InlineData('1')]
        [InlineData('4')]
        [InlineData('7')]
        [InlineData('9')]
        public void ValidateInt_WhenValidInput_ReturnsValidOutput(byte b)
        {
            var validator = new IntValidator();
            var valid = validator.ValidateByte(b);
            Assert.Equal(ByteValidation.Valid, valid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(3)]
        [InlineData(15)]
        [InlineData('+')]
        [InlineData('B')]
        [InlineData('a')]
        [InlineData('x')]
        [InlineData('}')]
        [InlineData(255)]
        public void ValidateInt_WhenInvalidInput_ReturnsErrorOutput(byte b)
        {
            var validator = new IntValidator();
            var valid = validator.ValidateByte(b);
            Assert.Equal(ByteValidation.Error, valid);
        }
    }
}
