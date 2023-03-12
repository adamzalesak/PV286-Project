namespace Panbyte.App.Convertors;

public class CommonToTargetConvertor : Convertor
{
    private readonly IConvertor _commonFormatConvertor;
    private readonly IConvertor _targetFormatConvertor;

    public CommonToTargetConvertor(IConvertor commonFormatConvertor, IConvertor targetFormatConvertor,
        ConvertorOptions convertorOptions) : base(convertorOptions)
    {
        _commonFormatConvertor = commonFormatConvertor;
        _targetFormatConvertor = targetFormatConvertor;
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