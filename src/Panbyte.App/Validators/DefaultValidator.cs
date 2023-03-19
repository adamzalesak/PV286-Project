namespace Panbyte.App.Validators;

public class DefaultValidator : IByteValidator
{
    public ByteValidation ValidateByte(byte b)
    {
        char c = (char)b;
        if (char.IsWhiteSpace(c))
        {
            return ByteValidation.Ignore;
        }
        return ByteValidation.Valid;
    }
}
