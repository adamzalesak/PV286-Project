using Panbyte.App.Extensions;
using Panbyte.App.Validators;

namespace Panbyte.App.Convertors.BitsTo;

public class BitsToBytesConvertor : Convertor
{
    private readonly bool _leftPadding;

    public BitsToBytesConvertor(ConvertorOptions convertorOptions, IByteValidator byteValidator) : base(
        convertorOptions, byteValidator)
    {
        _leftPadding = _convertorOptions.InputOption == "left";
    }

    public override void ConvertPart(byte[] source, Stream destination)
    {
        source = source.HandlePadding(_leftPadding);

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
}