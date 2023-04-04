using Panbyte.App.Parser;

namespace Panbyte.App.Convertors.ArrayTo;

public class XToArrayConvertor : IConvertor
{
    private static readonly byte[] arrayPrefix = "\"{"u8.ToArray();
    private static readonly byte[] arraySuffix = "}\""u8.ToArray();
    private readonly ToArrayConvertorOptions options;

    public XToArrayConvertor(ToArrayConvertorOptions options)
    {
        this.options = options;
    }

    public void ConvertPart(byte[] source, Stream destination)
    {
        source = ApplyOptionalConvertor(source);
        var bytes = GetBytes(source);
        var arrayConvertor = ConvertorFactory.Create(Format.Array, Format.Array, Array.Empty<string>(), options.OutputOptions);
        arrayConvertor.ConvertPart(bytes, destination);
    }

    private byte[] ApplyOptionalConvertor(byte[] source)
    {
        if (options.Convertor is null)
        {
            return source;
        }
        using var tmpStream = new MemoryStream();
        options.Convertor.ConvertPart(source, tmpStream);
        return tmpStream.ToArray();
    }

    private byte[] GetBytes(byte[] source)
    {
        List<byte> bytes = new(arrayPrefix);
        var (delLenght, prefix, suffix) = GetFormatArrayItemInfo();
        for (int i = 0; i < source.Length; i += delLenght)
        {
            var toAdd = prefix.Concat(source.Skip(i).Take(delLenght)).Concat(suffix).ToList();
            if (i < source.Length - delLenght)
            {
                toAdd.Add((byte)',');
            }
            bytes.AddRange(toAdd);
        }
        bytes.AddRange(arraySuffix);
        return bytes.ToArray();
    }

    private (int Lenght, byte[] Prefix, byte[] Suffix) GetFormatArrayItemInfo() => options.FromFormat switch
    {
        Format.Int => (3, Array.Empty<byte>(), Array.Empty<byte>()),
        Format.Hex => (2, "0x"u8.ToArray(), Array.Empty<byte>()),
        Format.Bits => (8, "0b"u8.ToArray(), Array.Empty<byte>()),
        Format.Bytes => (1, "'"u8.ToArray(), "'"u8.ToArray()),
        _ => throw new NotImplementedException(),
    };
}
