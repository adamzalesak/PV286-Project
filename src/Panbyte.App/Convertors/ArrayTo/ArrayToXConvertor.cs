namespace Panbyte.App.Convertors.ArrayTo;

public class ArrayToXConvertor : ArrayToArrayConvertor
{
    protected override bool SupportNesting => false;
    protected override bool WritingToDestinationEnabled => false;

    public ArrayToXConvertor(ArrayConvertorOptions convertorOptions) : base(convertorOptions)
    {
    }

    protected override void ConvertInputPart(byte[] bytes, Stream destination)
    {
        var (fromFormat, bytesToConvert) = GetBytes(bytes);
        var convertor = ConvertorFactory.Create(fromFormat, convertorOptions.OutputFormat, new[] { convertorOptions.InputOption }, new[] { convertorOptions.OutputOption });
        convertor.ConvertPart(bytesToConvert, destination);
    }
}
