namespace Panbyte.App.Convertors.BytesTo;

public class BytesToHexConvertor : IConvertor
{
    private readonly bool lowerCase;

    public BytesToHexConvertor(bool lowerCase = false)
    {
        this.lowerCase = lowerCase;
    }

    public void ConvertPart(byte[] source, Stream destination)
    {
        var hex = BitConverter.ToString(source);
        hex = hex.Replace("-", "");
        if (lowerCase)
        {
            hex = hex.ToLowerInvariant();
        }
        var bytes = System.Text.Encoding.ASCII.GetBytes(hex);

        destination.Write(bytes, 0, bytes.Length);
    }
}