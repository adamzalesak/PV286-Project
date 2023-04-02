namespace Panbyte.App.Convertors.IntTo;

public class IntToBytesConvertor : IConvertor
{
    private readonly bool _littleEndianInput;

    public IntToBytesConvertor(ConvertorOptions convertorOptions)
    {
        _littleEndianInput = convertorOptions.InputOption == "little";
    }

    public void ConvertPart(byte[] source, Stream destination)
    {
        var sourceString = System.Text.Encoding.ASCII.GetString(source);
        var bytes = BitConverter.GetBytes(uint.Parse(sourceString));
        if (!_littleEndianInput)
        {
            Array.Reverse(bytes);
        }

        destination.Write(bytes);
    }
}