using Panbyte.App.Validators;

namespace Panbyte.App.Convertors.ArrayTo;

public class ArrayToXConvertor : ArrayToArrayConvertor
{
    protected override bool SupportNesting => false;
    protected override bool WritingToDestinationEnabled => false;

    public ArrayToXConvertor(ArrayConvertorOptions convertorOptions, IByteValidator byteValidator) : base(convertorOptions, byteValidator)
    {
    }

    protected override void ConvertInputPart(byte[] bytes, Stream destination)
    {
        var (fromFormat, bytesToConvert) = GetBytes(bytes);
        var convertor = ConvertorFactory.Create(fromFormat, _convertorOptions.OutputFormat, "", new[] { _convertorOptions.InputOption }, new[] { _convertorOptions.OutputOption });
        convertor.ConvertPart(bytesToConvert, destination);
    }
}
