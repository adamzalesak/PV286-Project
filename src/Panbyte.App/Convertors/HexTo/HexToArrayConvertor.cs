using Panbyte.App.Parser;

namespace Panbyte.App.Convertors.HexTo;

public class HexToArrayConvertor : IConvertor
{
    private static readonly byte[] prefix = new[] { (byte)'"', (byte)'{' };
    private static readonly byte[] suffix = new[] { (byte)'}', (byte)'"' };
    private static readonly byte[] hexPrefix = new[] { (byte)'0', (byte)'x' };

    public HexToArrayConvertor()
    {
    }

    public void ConvertPart(byte[] source, Stream destination)
    {
        List<byte> bytes = new(prefix);
        for (int i = 0; i < source.Length; i += 2)
        {
            var toAdd = hexPrefix.Concat(source.Skip(i).Take(2)).ToList();
            if (i < source.Length - 2)
            {
                toAdd.Add((byte)',');
            }
            bytes.AddRange(toAdd);
        }
        bytes.AddRange(suffix);

        var arrayConvertor = ConvertorFactory.Create(Format.Array, Format.Array, Array.Empty<string>(), Array.Empty<string>());
        arrayConvertor.ConvertPart(bytes.ToArray(), destination);
    }
}
