namespace Panbyte.App.Convertors;

public class BytesToBitsConvertor : Convertor
{
    public BytesToBitsConvertor(ConvertorOptions convertorOptions) : base(convertorOptions)
    {
    }

    public override Stream ConvertPart(Stream source)
    {
        var bits = new List<bool>();
        var buffer = new byte[1];
        while (source.Read(buffer, 0, buffer.Length) > 0)
        {
            var byteValue = buffer[0];
            for (var i = 0; i < 8; i++)
            {
                bits.Add((byteValue & 0x80) != 0);
                byteValue <<= 1;
            }
        }

        var stream = new MemoryStream();
        foreach (var b in bits)
        {
            var bitChar = b ? '1' : '0';
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