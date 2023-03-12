namespace Panbyte.App.Convertors;

public class BitsToHexConvertor : Convertor
{
    public BitsToHexConvertor(ConvertorOptions convertorOptions) : base(convertorOptions)
    {
    }
    public override Stream ConvertPart(byte[] source)
    {
        throw new NotImplementedException();
    }

    public override bool ValidateOptions(out string errorMessage)
    {
        throw new NotImplementedException();
    }
}