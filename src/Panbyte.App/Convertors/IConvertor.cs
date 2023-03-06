namespace Panbyte.App.Convertors;

public interface IConvertor
{
    Stream Convert(Stream source);
    bool ValidateOptions(out string errorMessage);
}
