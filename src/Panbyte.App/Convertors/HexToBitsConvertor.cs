namespace Panbyte.App.Convertors;

public class HexToBitsConvertor : Convertor
{
    public HexToBitsConvertor(ConvertorOptions convertorOptions) : base(convertorOptions)
    {
    }

    public override void ConvertPart(byte[] source, Stream destination)
    {
        throw new NotImplementedException();
    }

    public override bool ValidateOptions(out string errorMessage)
    {
        throw new NotImplementedException();
    }
}
