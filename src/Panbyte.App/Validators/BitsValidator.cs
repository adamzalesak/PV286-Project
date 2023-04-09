namespace Panbyte.App.Validators;

public class BitsValidator : IByteValidator
{
    public ByteValidation ValidateByte(byte b)
    {
        char c = (char)b;
        if (char.IsWhiteSpace(c))
        {
            return ByteValidation.Ignore;
        }
        return b == 48 || b == 49 ? ByteValidation.Valid : ByteValidation.Error;
    }
}
