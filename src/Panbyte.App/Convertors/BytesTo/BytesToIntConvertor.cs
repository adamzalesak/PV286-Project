using Panbyte.App.Exceptions;

namespace Panbyte.App.Convertors.BytesTo;

public class BytesToIntConvertor : IConvertor
{
    private readonly bool _littleEndianOutput;

    public BytesToIntConvertor(ConvertorOptions convertorOptions)
    {
        _littleEndianOutput = convertorOptions.OutputOption == "little";
    }

    public void ConvertPart(byte[] source, Stream destination)
    {
        if (source.Length > 4)
        {
            throw new InvalidFormatException("Input is too long to be converted to an integer.");
        }

        if (source.Length < 4)
        {
            var newBytes = new byte[4];
            Array.Copy(source, newBytes, source.Length);
            source = newBytes;
        }

        if (!_littleEndianOutput)
        {
            Array.Reverse(source);
        }

        var result = BitConverter.ToUInt32(source);
        var resultString = result.ToString();
        var resultBytes = System.Text.Encoding.ASCII.GetBytes(resultString);

        destination.Write(resultBytes);
    }
}