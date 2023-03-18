namespace Panbyte.App.Convertors;

public interface IConvertor
{
    void Convert(Stream source, Stream destination);
    bool ValidateOptions(out string errorMessage);
}
