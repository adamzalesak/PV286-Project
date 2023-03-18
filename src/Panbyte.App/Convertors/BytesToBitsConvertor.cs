using Panbyte.App.Validators;

namespace Panbyte.App.Convertors;

public class BytesToBitsConvertor : Convertor
{
    public BytesToBitsConvertor(ConvertorOptions convertorOptions, IByteValidator byteValidator) : base(convertorOptions, byteValidator)
    {
    }

    public override void ConvertPart(byte[] source, Stream destination)
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

    public override bool ValidateOptions(out string errorMessage)
    {
        errorMessage = string.Empty;
        return true;
    }
}