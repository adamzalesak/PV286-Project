namespace Panbyte.App.Convertors;

public class CommonToTargetConvertor : IConvertor
{
    private readonly IConvertor _commonFormatConvertor;
    private readonly IConvertor _targetFormatConvertor;

    public CommonToTargetConvertor(IConvertor commonFormatConvertor, IConvertor targetFormatConvertor)
    {
        _commonFormatConvertor = commonFormatConvertor;
        _targetFormatConvertor = targetFormatConvertor;
    }

    public Stream Convert(Stream source)
    {
        throw new NotImplementedException();
    }

    public bool ValidateOptions(out string errorMessage)
    {
        throw new NotImplementedException();
    }
}