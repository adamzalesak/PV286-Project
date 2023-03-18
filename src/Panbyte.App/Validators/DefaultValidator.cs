namespace Panbyte.App.Validators;

public class DefaultValidator : IByteValidator
{
    public ByteValidation ValidateByte(byte b)
    {
        return ByteValidation.Valid;
    }
}
