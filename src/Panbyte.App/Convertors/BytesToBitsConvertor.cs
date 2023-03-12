namespace Panbyte.App.Convertors;

public class BytesToBitsConvertor : Convertor
{
    public BytesToBitsConvertor(ConvertorOptions convertorOptions) : base(convertorOptions)
    {
    }

    public override Stream ConvertPart(byte[] source)
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

        var stream = new MemoryStream();
        foreach (var bitChar in bits.Select(b => b ? '1' : '0'))
        {
            stream.WriteByte((byte)bitChar);
        }

        stream.Flush();
        stream.Position = 0;
        return stream;
    }

    public override bool ValidateOptions(out string errorMessage)
    {
        errorMessage = string.Empty;
        return true;
    }
}