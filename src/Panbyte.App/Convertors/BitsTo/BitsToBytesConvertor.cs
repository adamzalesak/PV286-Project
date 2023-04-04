using Panbyte.App.Extensions;

namespace Panbyte.App.Convertors.BitsTo;

public class BitsToBytesConvertor : IConvertor
{
    private readonly bool _leftPadding;

    public BitsToBytesConvertor(ConvertorOptions convertorOptions)
    {
        _leftPadding = convertorOptions.InputOption == "left";
    }

    public void ConvertPart(byte[] source, Stream destination)
    {
        source = source.HandlePadding(_leftPadding);

        var sourceString = System.Text.Encoding.ASCII.GetString(source);
        var remainder = source.Length % 8;

        if (remainder != 0)
        {
            var oneByte = Convert.ToByte(sourceString[..remainder], 2);
            destination.WriteByte(oneByte);
        }

        for (int i = remainder; i < source.Length; i += 8)
        {
            var oneByte = Convert.ToByte(sourceString.Substring(i, 8), 2);
            destination.WriteByte(oneByte);
        }
    }
}