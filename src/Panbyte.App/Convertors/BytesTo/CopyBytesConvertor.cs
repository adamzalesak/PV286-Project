namespace Panbyte.App.Convertors.BytesTo;

public class CopyBytesConvertor : IConvertor
{
    public void Convert(Stream source, Stream destination) => source.CopyTo(destination);
}
