namespace Panbyte.App.Convertors;

public interface IConvertor
{
    void ConvertPart(byte[] source, Stream destination);
}
