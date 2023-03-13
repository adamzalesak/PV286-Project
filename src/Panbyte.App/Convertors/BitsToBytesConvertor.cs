namespace Panbyte.App.Convertors;

public class BitsToBytesConvertor : Convertor
{
    public BitsToBytesConvertor(ConvertorOptions convertorOptions) : base(convertorOptions)
    {
    }

    public override Stream ConvertPart(byte[] source)
    {   
        var sourceString = System.Text.Encoding.ASCII.GetString(source);
        var remainder = source.Length % 8;
        var stream = new MemoryStream();
        byte oneByte;

        if (remainder != 0)
        {
            oneByte = System.Convert.ToByte(sourceString[..remainder], 2);
            stream.WriteByte(oneByte);
        }

        for (int i = remainder; i < source.Length; i += 8)
        {
            oneByte = System.Convert.ToByte(sourceString.Substring(i, 8), 2);
            stream.WriteByte(oneByte);
        }

        stream.Flush();
        stream.Position = 0;
        return stream;
    }

    public override bool ValidateOptions(out string errorMessage)
    {
        throw new NotImplementedException();
    }
}
