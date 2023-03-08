namespace Panbyte.App.Convertors;

public class BytesToBitsConvertor : IConvertor
{
    private readonly char _delimeter;

    public BytesToBitsConvertor(char delimeter = '\n')
    {
        _delimeter = delimeter;
    }

    public Stream Convert(Stream source)
    {
        throw new NotImplementedException();
    }

    public bool ValidateOptions(out string errorMessage)
    {
        errorMessage = string.Empty;
        return true;
    }
}
