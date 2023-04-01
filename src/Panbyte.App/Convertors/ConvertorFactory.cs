using Panbyte.App.Convertors.ArrayTo;
using Panbyte.App.Convertors.BitsTo;
using Panbyte.App.Convertors.BytesTo;
using Panbyte.App.Convertors.HexTo;
using Panbyte.App.Parser;
using Panbyte.App.Validators;

namespace Panbyte.App.Convertors;

public static class ConvertorFactory
{
    private static readonly Dictionary<(Format From, Format To), Func<ICollection<string>, ICollection<string>, IConvertor>> factory = new()
    {
        // bits to x
        { (Format.Bits, Format.Bits), (inputs, _) => new CopyConvertor()},
        { (Format.Bits, Format.Bytes), (inputs, _) => new BitsToBytesConvertor(new ConvertorOptions(inputs.FirstOrDefault("left")))},
        { (Format.Bits, Format.Hex), (inputs, _) => new CommonConvertor(new BitsToBytesConvertor(new ConvertorOptions(inputs.FirstOrDefault("left"))), new BytesToHexConvertor(true))},
        // bytes to x
        { (Format.Bytes, Format.Bytes), (_, _) => new CopyConvertor()},
        { (Format.Bytes, Format.Hex), (_, _) => new BytesToHexConvertor()},
        { (Format.Bytes, Format.Bits), (_, _) => new BytesToBitsConvertor()},
        // hex to x
        { (Format.Hex, Format.Hex), (_, _) => new CopyConvertor()},
        { (Format.Hex, Format.Bytes), (_, _) => new HexToBytesConvertor()},
        { (Format.Hex, Format.Bits), (_, _) => new CommonConvertor(new HexToBytesConvertor(), new BytesToBitsConvertor())},
        { (Format.Hex, Format.Array), (_, _) => new HexToArrayConvertor()},
        // array to x
        { (Format.Array, Format.Array), (_, outputs) => new ArrayToArrayConvertor(new ArrayConvertorOptions(outputs.ToArray(), "", "", Format.Array))},
        { (Format.Array, Format.Hex), (inputs, _) => new ArrayToXConvertor(new ArrayConvertorOptions(Array.Empty<string>(), inputs.FirstOrDefault("left"), "", Format.Hex))},
    };

    public static IConvertor Create(Format from, Format to, ICollection<string> inputs, ICollection<string> outputs)
    {
        if (factory.TryGetValue((from, to), out var factoryMethod))
        {
            return factoryMethod(inputs, outputs);
        }
        throw new NotImplementedException();
    }

    private static string GetStringIfNotEmptyOrNewLine(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return Environment.NewLine;
        }
        return value;
    }

    private static IByteValidator CreateByteValidator(Format from) => from switch
    {
        Format.Bits => new BitsValidator(),
        Format.Hex => new HexValidator(),
        Format.Int => new IntValidator(),
        _ => new DefaultValidator()
    };
}