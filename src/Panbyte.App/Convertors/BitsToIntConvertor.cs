namespace Panbyte.App.Convertors;

public class BitsToIntConvertor : Convertor
{
    public BitsToIntConvertor(ConvertorOptions convertorOptions) : base(convertorOptions)
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