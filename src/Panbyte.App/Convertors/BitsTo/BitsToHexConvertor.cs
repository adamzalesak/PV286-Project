using Panbyte.App.Extensions;
using Panbyte.App.Validators;
using System.Text;

namespace Panbyte.App.Convertors.BitsTo;

public class BitsToHexConvertor : Convertor
{
    private readonly bool _leftPadding;

    public BitsToHexConvertor(ConvertorOptions convertorOptions, IByteValidator byteValidator)
        : base(convertorOptions, byteValidator)
    {
        _leftPadding = _convertorOptions.InputOption == "left";
    }

    public override void ConvertPart(byte[] source, Stream destination)
    {
        source = source.HandlePadding(4, _leftPadding);

        var bytes = GetBytes(source);
        var hex = GetHexString(bytes).ToLower();
        var byteArray = Encoding.ASCII.GetBytes(hex);

        destination.Write(byteArray, 0, byteArray.Length);
    }

    private static List<byte> GetBytes(byte[] source)
    {
        var bytes = new List<byte>();
        var bits = new List<byte>();

        foreach (var bit in source)
        {
            bits.Add(bit == '0' ? (byte)0 : (byte)1);
            if (bits.Count == 4)
            {
                var byteValue = 0;
                for (var i = 0; i < 4; i++)
                {
                    byteValue += bits[i] == 0 ? 0 : (int)Math.Pow(2, 3 - i);
                }

                bytes.Add((byte)byteValue);
                bits.Clear();
            }
        }

        return bytes;
    }

    private static string GetHexString(List<byte> bytes)
    {
        var result = new StringBuilder();
        foreach (var byteValue in bytes)
        {
            result.Append(string.Format("{0:X}", byteValue));
        }
        return result.ToString();
    }
}