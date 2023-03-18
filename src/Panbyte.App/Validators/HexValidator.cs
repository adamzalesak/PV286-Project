namespace Panbyte.App.Validators;

public class HexValidator : IByteValidator
{
    public ByteValidation ValidateByte(byte b)
    {
        char c = (char)b;
        if (char.IsWhiteSpace(c))
        {
            return ByteValidation.Ignore;
        }
        return ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f')) ? ByteValidation.Valid : ByteValidation.Error;
    }
}
