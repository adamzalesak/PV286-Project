namespace Panbyte.App.Convertors;

public class CommonConvertor : IConvertor
{
    private readonly IConvertor inputConvertor;
    private readonly IConvertor outputConvertor;

    public CommonConvertor(IConvertor inputConvertor, IConvertor outputConvertor)
    {
        this.inputConvertor = inputConvertor;
        this.outputConvertor = outputConvertor;
    }

    public void ConvertPart(byte[] source, Stream destination)
    {
        using var tmpStream = new MemoryStream();
        inputConvertor.ConvertPart(source, tmpStream);
        outputConvertor.ConvertPart(tmpStream.ToArray(), destination);
    }
}
