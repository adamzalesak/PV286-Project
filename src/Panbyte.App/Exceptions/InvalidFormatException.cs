namespace Panbyte.App.Exceptions;

public class InvalidFormatException : Exception
{
    public InvalidFormatException(string? message = null) : base(message ?? "Invalid input format")
    {
    }
}