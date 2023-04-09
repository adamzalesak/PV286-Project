namespace Panbyte.App.Convertors.BytesTo;

public class BytesToBitsConvertor : IConvertor
{
    public void ConvertPart(byte[] source, Stream destination)
    {
        var bits = new List<bool>();
        foreach (var b in source)
        {
            var byteValue = b;
            for (var j = 0; j < 8; j++)
            {
                bits.Add((byteValue & 0x80) != 0);
                byteValue <<= 1;
            }
        }

        foreach (var bitChar in bits.Select(b => b ? '1' : '0'))
        {
            destination.WriteByte((byte)bitChar);
        }
    }
}