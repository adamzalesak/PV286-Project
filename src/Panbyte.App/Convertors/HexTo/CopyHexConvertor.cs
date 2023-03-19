using Panbyte.App.Validators;

namespace Panbyte.App.Convertors.HexTo;

public class CopyHexConvertor : Convertor
{
    public CopyHexConvertor(ConvertorOptions convertorOptions, IByteValidator byteValidator) : base(convertorOptions, byteValidator)
    {
    }


    public override void ConvertPart(byte[] source, Stream destination)
    {
        destination.Write(source);
    }
}
