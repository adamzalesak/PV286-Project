namespace Panbyte.App.Convertors;

public interface IConvertor
{
    Stream Convert(Stream stream);
    Stream ConvertPart(Stream source);
    bool ValidateOptions(out string errorMessage);
}
