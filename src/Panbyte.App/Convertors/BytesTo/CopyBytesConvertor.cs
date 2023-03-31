namespace Panbyte.App.Convertors.BytesTo;

public class CopyBytesConvertor : IConvertor
{
    public void Convert(Stream source, Stream destination) => source.CopyTo(destination);

    public void ConvertPart(byte[] source, Stream destination) => destination.Write(source, 0, source.Length);
}
