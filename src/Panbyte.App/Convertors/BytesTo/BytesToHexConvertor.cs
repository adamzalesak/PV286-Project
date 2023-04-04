using System.Text.RegularExpressions;

namespace Panbyte.App.Convertors.BytesTo;

public class BytesToHexConvertor : IConvertor
{
    private readonly bool lowerCase;
    private static readonly Regex trailingZero = new(@"0+$", RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
    private static readonly Regex leftTrailingZero = new(@"^0+", RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));

    public BytesToHexConvertor(bool lowerCase = false)
    {
        this.lowerCase = lowerCase;
    }

    public void ConvertPart(byte[] source, Stream destination)
    {
        var hex = BitConverter.ToString(source);
        hex = hex.Replace("-", "");
        if (lowerCase)
        {
            hex = hex.ToLowerInvariant();
        }

        var tmp = trailingZero.Replace(leftTrailingZero.Replace(
            hex, i => (hex.Length - i.Length) % 2 == 0 ? "" : "0"),
            i => (hex.Length - i.Length) % 2 == 0 ? "" : "0");

        var bytes = System.Text.Encoding.ASCII.GetBytes(tmp);

        destination.Write(bytes, 0, bytes.Length);
    }
}