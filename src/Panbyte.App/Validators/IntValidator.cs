namespace Panbyte.App.Validators;

public class IntValidator : IByteValidator
{
    public ByteValidation ValidateByte(byte b)
    {
        return b >= 48 && b <= 57 ? ByteValidation.Valid : ByteValidation.Ignore;
    }
}