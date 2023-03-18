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
            "array" => throw new NotImplementedException(),
            _ => throw new NotImplementedException(),
        };

    private static IConvertor CreateFromBytesToXConvertor(string to, ConvertorOptions convertorOptions) => to switch
    {
        "bits" => new BytesToBitsConvertor(convertorOptions),
        "hex" => new CommonToTargetConvertor(new BytesToBitsConvertor(convertorOptions), new BitsToHexConvertor(convertorOptions), convertorOptions),
        "int" => new CommonToTargetConvertor(new BytesToBitsConvertor(convertorOptions), new BitsToIntConvertor(convertorOptions), convertorOptions),
        "array" => throw new NotImplementedException(),
        _ => throw new NotImplementedException(),
    };

    private static IConvertor CreateFromBitsToXConvertor(string to, ConvertorOptions convertorOptions) => to switch
    {
        "bytes" => new BitsToBytesConvertor(convertorOptions),
        "hex" => new BitsToHexConvertor(convertorOptions),
        "int" => new BitsToIntConvertor(convertorOptions),
        "array" => throw new NotImplementedException(),
        _ => throw new NotImplementedException(),
    };

    private static IConvertor CreateFromIntToXConvertor(string to, ConvertorOptions convertorOptions) => to switch
    {
        "bytes" => new CommonToTargetConvertor(new IntToBitsConvertor(convertorOptions), new BitsToBytesConvertor(convertorOptions), convertorOptions),
        "hex" => new CommonToTargetConvertor(new IntToBitsConvertor(convertorOptions), new BitsToHexConvertor(convertorOptions), convertorOptions),
        "bits" => new IntToBitsConvertor(convertorOptions),
        "array" => throw new NotImplementedException(),
        _ => throw new NotImplementedException(),
    };

    private static IConvertor CreateFromHexToXConvertor(string to, ConvertorOptions convertorOptions) => to switch
    {
        "bytes" => new HexToBytesConvertor(convertorOptions),
        "int" => new CommonToTargetConvertor(new HexToBitsConvertor(convertorOptions), new BitsToIntConvertor(convertorOptions), convertorOptions),
        "bits" => new HexToBitsConvertor(convertorOptions),
        "array" => throw new NotImplementedException(),
        _ => throw new NotImplementedException(),
    };

    public static IConvertor CreateFromArrayToXConvertor(string to)
        => throw new NotImplementedException();
}