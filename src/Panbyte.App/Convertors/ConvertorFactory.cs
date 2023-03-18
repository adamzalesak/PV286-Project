using Panbyte.App.Convertors.Copy;
using Panbyte.App.Validators;

namespace Panbyte.App.Convertors;

public static class ConvertorFactory
{
    public static IConvertor CreateConvertor(string from, string to, ConvertorOptions convertorOptions)
        => from switch
        {
            "bytes" => CreateFromBytesToXConvertor(to, convertorOptions),
            "hex" => CreateFromHexToXConvertor(to, convertorOptions),
            "int" => CreateFromIntToXConvertor(to, convertorOptions),
            "bits" => CreateFromBitsToXConvertor(to, convertorOptions),
            _ => throw new NotImplementedException(), // todo unknown format or invalid format
        };

    private static IConvertor CreateFromBytesToXConvertor(string to, ConvertorOptions convertorOptions) => to switch
    {
        "bits" => new BytesToBitsConvertor(convertorOptions, CreateByteValidator("bytes")),
        "hex" => new BytesToHexConvertor(convertorOptions, CreateByteValidator("bytes")),
        "bytes" => new CopyBytesConvertor(),
        _ => throw new NotImplementedException(),
    };

    private static IConvertor CreateFromBitsToXConvertor(string to, ConvertorOptions convertorOptions) => to switch
    {
        "bytes" => new BitsToBytesConvertor(convertorOptions, CreateByteValidator("bits")),
        "hex" => new BitsToHexConvertor(convertorOptions, CreateByteValidator("bits")),
        "bits" => new CopyBitsConvertor(convertorOptions, CreateByteValidator("bits")),
        _ => throw new NotImplementedException(),
    };

    private static IConvertor CreateFromIntToXConvertor(string to, ConvertorOptions convertorOptions) => to switch
    {
        _ => throw new NotImplementedException(),
    };

    private static IConvertor CreateFromHexToXConvertor(string to, ConvertorOptions convertorOptions) => to switch
    {
        "bytes" => new HexToBytesConvertor(convertorOptions, CreateByteValidator("hex")),
        "bits" => new HexToBitsConvertor(convertorOptions, CreateByteValidator("bits")),
        "hex" => new CopyHexConvertor(convertorOptions, CreateByteValidator("hex")),
        _ => throw new NotImplementedException(),
    };

    private static IConvertor CreateFromArrayToXConvertor(string to)
        => throw new NotImplementedException();

    private static IByteValidator CreateByteValidator(string from) => from switch
    {
        "bits" => new BitsValidator(),
        "hex" => new HexValidator(),
        "int" => new IntValidator(),
        _ => new DefaultValidator()
    };
}