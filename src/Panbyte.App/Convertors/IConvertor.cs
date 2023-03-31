namespace Panbyte.App.Convertors;

public interface IConvertor
{
    void Convert(Stream source, Stream destination);
    void ConvertPart(byte[] source, Stream destination);
}
