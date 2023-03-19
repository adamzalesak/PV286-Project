namespace Panbyte.App.Validators;

public class DefaultValidator : IByteValidator
{
    public ByteValidation ValidateByte(byte b)
    {
        return b == 10 || b == 13 ? ByteValidation.Ignore : ByteValidation.Valid;
    }
}
