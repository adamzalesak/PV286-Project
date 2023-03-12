namespace Panbyte.App.Convertors;

public interface IConvertor
{
    Stream Convert(Stream stream);
    Stream ConvertPart(byte[] source);
    bool ValidateOptions(out string errorMessage);
}
