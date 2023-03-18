using Panbyte.App.Convertors.Copy;
using Panbyte.App.Parser;
using Panbyte.App.Validators;

namespace Panbyte.App.Convertors;

public static class ConvertorFactory
{
    private static readonly Dictionary<(Format From, Format To), Func<string, ICollection<string>, ICollection<string>, IConvertor>> factory = new()
    {
        // bits to x
        { (Format.Bits, Format.Bits), (del, inputs, _) => new CopyBitsConvertor(new ConvertorOptions(del, inputs.FirstOrDefault("left")), CreateByteValidator(Format.Bits))},
        { (Format.Bits, Format.Bytes), (del, inputs, _) => new BitsToBytesConvertor(new ConvertorOptions(del, inputs.FirstOrDefault("left")), CreateByteValidator(Format.Bits))},
        { (Format.Bits, Format.Hex), (del, inputs, _) => new BitsToHexConvertor(new ConvertorOptions(del, inputs.FirstOrDefault("left")), CreateByteValidator(Format.Bits))},
        // bytes to x
        { (Format.Bytes, Format.Bytes), (del, _, _) => new CopyBytesConvertor()},
        { (Format.Bytes, Format.Hex), (del, _, _) => new BytesToHexConvertor(new ConvertorOptions(del), CreateByteValidator(Format.Bytes))},
        { (Format.Bytes, Format.Bits), (del, _, _) => new BytesToBitsConvertor(new ConvertorOptions(del), CreateByteValidator(Format.Bytes))},
        // hex to x
        { (Format.Hex, Format.Hex), (del, _, _) => new CopyHexConvertor(new ConvertorOptions(del), CreateByteValidator(Format.Hex))},
        { (Format.Hex, Format.Bytes), (del, _, _) => new HexToBytesConvertor(new ConvertorOptions(del), CreateByteValidator(Format.Hex))},
        { (Format.Hex, Format.Bits), (del, _, _) => new HexToBitsConvertor(new ConvertorOptions(del), CreateByteValidator(Format.Hex))},
    };

    public static IConvertor Create(Format from, Format to, string del, ICollection<string> inputs, ICollection<string> outputs)
    {
        if (factory.TryGetValue((from, to), out var factoryMethod))
        {
            return factoryMethod(del, inputs, outputs);
        }
        throw new NotImplementedException();
    }

    private static IByteValidator CreateByteValidator(Format from) => from switch
    {
        Format.Bits => new BitsValidator(),
        Format.Hex => new HexValidator(),
        Format.Int => new IntValidator(),
        _ => new DefaultValidator()
    };
}