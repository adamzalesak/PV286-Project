namespace Panbyte.App.Exceptions;

public class InvalidFormatCharacterException : Exception
{
    public InvalidFormatCharacterException(byte byteValue) : base($"\nInvalid character: {byteValue}")
    {
    }
}
