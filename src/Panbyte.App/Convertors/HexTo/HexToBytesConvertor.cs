using System.Text.RegularExpressions;

namespace Panbyte.App.Convertors.HexTo;

public class HexToBytesConvertor : IConvertor
{
    private static readonly Regex WhiteSpaceRegex = new(@"\s+", RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));

    public HexToBytesConvertor()
    {
    }

    public void ConvertPart(byte[] source, Stream destination)
    {
        var sourceString = System.Text.Encoding.ASCII.GetString(source);
        // remove all whitespaces
        sourceString = WhiteSpaceRegex.Replace(sourceString, "");
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
    }
}
