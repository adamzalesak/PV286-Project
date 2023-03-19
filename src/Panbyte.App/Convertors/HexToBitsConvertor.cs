using Panbyte.App.Validators;

namespace Panbyte.App.Convertors;

public class HexToBitsConvertor : Convertor
{
    public HexToBitsConvertor(ConvertorOptions convertorOptions, IByteValidator byteValidator) : base(convertorOptions, byteValidator)
    {
    }

    public override void ConvertPart(byte[] source, Stream destination)
    {
        throw new NotImplementedException();
    }
}
