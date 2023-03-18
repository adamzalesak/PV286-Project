using Panbyte.App.Validators;

namespace Panbyte.App.Convertors.Copy;

public class CopyBitsConvertor : Convertor
{
    public CopyBitsConvertor(ConvertorOptions convertorOptions, IByteValidator byteValidator) : base(convertorOptions, byteValidator)
    {
    }

    public override void ConvertPart(byte[] source, Stream destination)
    {
        destination.Write(source);
    }
}
