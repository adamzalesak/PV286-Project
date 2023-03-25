namespace Panbyte.App.Exceptions;

public class InvalidFormatCharacter : Exception
{
    public InvalidFormatCharacter(byte byteValue) : base($"Invalid character: {byteValue}")
    {
    }
}
