using Panbyte.App.Validators;

namespace Panbyte.App.Convertors;

public class BitsToBytesConvertor : Convertor
{
    public BitsToBytesConvertor(ConvertorOptions convertorOptions, IByteValidator byteValidator) : base(convertorOptions, byteValidator)
    {
    }

    public override void ConvertPart(byte[] source, Stream destination)
    {
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

    public override bool ValidateOptions(out string errorMessage)
    {
        throw new NotImplementedException();
    }
}
