namespace Panbyte.App.Convertors;

public class BitsToHexConvertor : Convertor
{
    public BitsToHexConvertor(ConvertorOptions convertorOptions) : base(convertorOptions)
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