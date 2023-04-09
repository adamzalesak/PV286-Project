namespace Panbyte.App.Convertors;

public class CopyConvertor : IConvertor
{
    public void ConvertPart(byte[] source, Stream destination) => destination.Write(source);
}
