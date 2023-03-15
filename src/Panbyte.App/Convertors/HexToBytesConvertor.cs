namespace Panbyte.App.Convertors;

public class HexToBytesConvertor : Convertor
{
    public HexToBytesConvertor(ConvertorOptions convertorOptions) : base(convertorOptions)
    {
    }

    public override Stream ConvertPart(byte[] source)
    {
        var sourceString = System.Text.Encoding.ASCII.GetString(source);
        var stream = new MemoryStream();
        byte oneByte;

        if (source.Length % 2 != 0)
        {
            throw new ArgumentException("Invalid input value");
        }

        for (int i = 0; i < source.Length; i += 2)
        {
            oneByte = System.Convert.ToByte(sourceString.Substring(i, 2), 16);
            stream.WriteByte(oneByte);
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
