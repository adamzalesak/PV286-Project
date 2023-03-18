using System.Text.RegularExpressions;

namespace Panbyte.App.Convertors;

public class HexToBytesConvertor : Convertor
{
    public HexToBytesConvertor(ConvertorOptions convertorOptions) : base(convertorOptions)
    {
    }

    public override void ConvertPart(byte[] source, Stream destination)
    {
        var sourceString = System.Text.Encoding.ASCII.GetString(source);
        // remove all whitespaces
        sourceString = Regex.Replace(sourceString, @"\s+", "");
        byte oneByte;

        if (sourceString.Length % 2 != 0)
        {
            throw new ArgumentException("Invalid input value");
        }

        for (int i = 0; i < sourceString.Length; i += 2)
        {
            oneByte = System.Convert.ToByte(sourceString.Substring(i, 2), 16);
            destination.WriteByte(oneByte);
        }

        destination.Flush();
    }

    public override bool ValidateOptions(out string errorMessage)
    {
        errorMessage = string.Empty;
        return true;
    }
}
