namespace Panbyte.App.Convertors;

public class HexToBitsConvertor : Convertor
{
    public HexToBitsConvertor(ConvertorOptions convertorOptions) : base(convertorOptions)
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
