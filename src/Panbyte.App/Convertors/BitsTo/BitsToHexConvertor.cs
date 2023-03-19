using Panbyte.App.Extensions;
using Panbyte.App.Validators;

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
        source = source.HandlePadding(_leftPadding);

        var bytes = GetBytes(source);

        var hex = BitConverter.ToString(bytes.ToArray());
        hex = hex.Replace("-", "").ToLower();
        var byteArray = System.Text.Encoding.ASCII.GetBytes(hex);

        destination.Write(byteArray, 0, byteArray.Length);
    }

    private static List<byte> GetBytes(byte[] source)
    {
        var bytes = new List<byte>();
        var bits = new List<byte>();

        foreach (var bit in source)
        {
            bits.Add(bit == '0' ? (byte)0 : (byte)1);
            if (bits.Count == 8)
            {
                var byteValue = 0;
                for (var i = 0; i < 8; i++)
                {
                    byteValue += bits[i] == 0 ? 0 : (int)Math.Pow(2, 7 - i);
                }

                bytes.Add((byte)byteValue);
                bits.Clear();
            }
        }

        return bytes;
    }
}