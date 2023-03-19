using Panbyte.App.Validators;

namespace Panbyte.App.Convertors.BitsTo;

public class BitsToBytesConvertor : Convertor
{
    private readonly string _padding;

    public BitsToBytesConvertor(ConvertorOptions convertorOptions, IByteValidator byteValidator) : base(
        convertorOptions, byteValidator)
    {
        _padding = _convertorOptions.InputOption;
    }

    public override void ConvertPart(byte[] source, Stream destination)
    {
        source = HandlePadding(source);

        var sourceString = System.Text.Encoding.ASCII.GetString(source);
        var remainder = source.Length % 8;
        byte oneByte;

        if (remainder != 0)
        {
            oneByte = System.Convert.ToByte(sourceString[..remainder], 2);
            destination.WriteByte(oneByte);
        }

        for (int i = remainder; i < source.Length; i += 8)
        {
            oneByte = System.Convert.ToByte(sourceString.Substring(i, 8), 2);
            destination.WriteByte(oneByte);
        }

        destination.Flush();
    }


    private byte[] HandlePadding(byte[] source)
    {
        if (source.Length % 8 != 0 && _padding == "left")
        {
            var padding = new byte[8 - source.Length % 8];
            padding = padding.Select(_ => (byte)'0').ToArray();
            source = padding.Concat(source).ToArray();
        }
        else if (source.Length % 8 != 0 && _padding == "right")
        {
            var padding = new byte[8 - source.Length % 8];
            padding = padding.Select(_ => (byte)'0').ToArray();
            source = source.Concat(padding).ToArray();
        }

        return source;
    }
}