using Panbyte.App.Exceptions;
using System.Text.RegularExpressions;

namespace Panbyte.App.Convertors.HexTo;

public class HexToBytesConvertor : IConvertor
{
    private static readonly Regex WhiteSpaceRegex = new(@"\s+", RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));

    public void ConvertPart(byte[] source, Stream destination)
    {
        var sourceString = System.Text.Encoding.ASCII.GetString(source);
        sourceString = WhiteSpaceRegex.Replace(sourceString, "");

        if (sourceString.Length % 2 != 0)
        {
            throw new InvalidFormatException();
        }

        for (int i = 0; i < sourceString.Length; i += 2)
        {
            var oneByte = Convert.ToByte(sourceString.Substring(i, 2), 16);
            destination.WriteByte(oneByte);
        }
    }
}
