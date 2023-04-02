using Panbyte.App.Convertors.ArrayTo;
using Panbyte.App.Convertors.BitsTo;
using Panbyte.App.Convertors.BytesTo;
using Panbyte.App.Convertors.HexTo;
using Panbyte.App.Convertors.IntTo;
using Panbyte.App.Parser;

namespace Panbyte.App.Convertors;

public static class ConvertorFactory
{
    private static readonly Dictionary<(Format From, Format To), Func<ICollection<string>, ICollection<string>, IConvertor>> factory = new()
    {
        // bits to x
        { (Format.Bits, Format.Bits), (_, _) => new CopyConvertor()},
        { (Format.Bits, Format.Bytes), (inputs, _) => new BitsToBytesConvertor(new ConvertorOptions(inputs.FirstOrDefault("left")))},
        { (Format.Bits, Format.Hex), (inputs, _) => new CommonConvertor(new BitsToBytesConvertor(new ConvertorOptions(inputs.FirstOrDefault("left"))), new BytesToHexConvertor(true))},
        // bytes to x
        { (Format.Bytes, Format.Bytes), (_, _) => new CopyConvertor()},
        { (Format.Bytes, Format.Hex), (_, _) => new BytesToHexConvertor()},
        { (Format.Bytes, Format.Bits), (_, _) => new BytesToBitsConvertor()},
        { (Format.Bytes, Format.Int), (_, outputs) => new BytesToIntConvertor(new ConvertorOptions("", outputs.FirstOrDefault("big")))},
        // hex to x
        { (Format.Hex, Format.Hex), (_, _) => new CopyConvertor()},
        { (Format.Hex, Format.Bytes), (_, _) => new HexToBytesConvertor()},
        { (Format.Hex, Format.Bits), (_, _) => new CommonConvertor(new HexToBytesConvertor(), new BytesToBitsConvertor())},
        { (Format.Hex, Format.Array), (_, _) => new HexToArrayConvertor()},
        { (Format.Hex, Format.Int), (_, outputs) => new CommonConvertor(new HexToBytesConvertor(), new BytesToIntConvertor(new ConvertorOptions("", outputs.FirstOrDefault("big"))))},
        // int to x
        { (Format.Int, Format.Int), (inputs, outputs) => new CommonConvertor(new IntToBytesConvertor(new ConvertorOptions(inputs.FirstOrDefault("big"))), new BytesToIntConvertor(new ConvertorOptions("", outputs.FirstOrDefault("big"))))},
        { (Format.Int, Format.Bytes), (inputs, _) => new IntToBytesConvertor(new ConvertorOptions(inputs.FirstOrDefault("big")))},
        { (Format.Int, Format.Bits), (inputs, _) => new CommonConvertor(new IntToBytesConvertor(new ConvertorOptions(inputs.FirstOrDefault("big"))), new BytesToBitsConvertor())},
        { (Format.Int, Format.Hex), (inputs, _) => new CommonConvertor(new IntToBytesConvertor(new ConvertorOptions(inputs.FirstOrDefault("big"))), new BytesToHexConvertor())},
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
}