using Panbyte.App.Validators;

namespace Panbyte.App.Convertors;

public class BytesToHexConvertor : Convertor
{
    public BytesToHexConvertor(ConvertorOptions convertorOptions, IByteValidator byteValidator) : base(convertorOptions,
        byteValidator)
    {
    }

    public override void ConvertPart(byte[] source, Stream destination)
    {
        var hex = BitConverter.ToString(source);
        hex = hex.Replace("-", "");
        var bytes = System.Text.Encoding.ASCII.GetBytes(hex);

        destination.Write(bytes, 0, bytes.Length);

        destination.Flush();
    }
}