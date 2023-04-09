using Panbyte.App.Validators;
using Xunit;

namespace Panbyte.Tests.UnitTests.ValidatorTests
{
    public class DefaultValidatorTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData('a')]
        [InlineData('.')]
        [InlineData(255)]
        public void ValidateByte_WhenValidInput_ReturnsValidOutput(byte b)
        {
            var validator = new DefaultValidator();
            var valid = validator.ValidateByte(b);
            Assert.Equal(ByteValidation.Valid, valid);
        }
    }
}
