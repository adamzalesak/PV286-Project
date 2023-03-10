namespace Panbyte.App.Convertors;

public class IntToBitsConvertor : Convertor
{
    public IntToBitsConvertor(ConvertorOptions convertorOptions) : base(convertorOptions)
    {
    }
    
    public override Stream ConvertPart(Stream source)
    {
        throw new NotImplementedException();
    }

    public override bool ValidateOptions(out string errorMessage)
    {
        throw new NotImplementedException();
    }
}
