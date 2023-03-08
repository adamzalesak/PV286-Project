namespace Panbyte.App.Convertors;

public static class ConvertorFactory
{
    public static IConvertor CreateConvertor(string from, string to, ConvertorOptions convertorOptions)
        => from switch
        {
            "bytes" => CreateFromBytesToXConvertor(to),
            "hex" => CreateFromHexToXConvertor(to),
            "int" => CreateFromIntToXConvertor(to),
            "bits" => CreateFromBitsToXConvertor(to),
            "array" => throw new NotImplementedException(),
            _ => throw new NotImplementedException(),
        };

    public static IConvertor CreateFromBytesToXConvertor(string to) => to switch
    {
        "bits" => new BytesToBitsConvertor(),
        "hex" => new CommonToTargetConvertor(new BytesToBitsConvertor(), new BitsToHexConvertor()),
        "int" => new CommonToTargetConvertor(new BytesToBitsConvertor(), new BitsToIntConvertor()),
        "array" => throw new NotImplementedException(),
        _ => throw new NotImplementedException(),
    };

    public static IConvertor CreateFromBitsToXConvertor(string to) => to switch
    {
        "bytes" => new BitsToBytesConvertor(),
        "hex" => new BitsToHexConvertor(),
        "int" => new BitsToIntConvertor(),
        "array" => throw new NotImplementedException(),
        _ => throw new NotImplementedException(),
    };

    public static IConvertor CreateFromIntToXConvertor(string to) => to switch
    {
        "bytes" => new CommonToTargetConvertor(new IntToBitsConvertor(), new BitsToBytesConvertor()),
        "hex" => new CommonToTargetConvertor(new IntToBitsConvertor(), new BitsToHexConvertor()),
        "bits" => new IntToBitsConvertor(),
        "array" => throw new NotImplementedException(),
        _ => throw new NotImplementedException(),
    };

    public static IConvertor CreateFromHexToXConvertor(string to) => to switch
    {
        "bytes" => new CommonToTargetConvertor(new HexToBitsConvertor(), new BitsToBytesConvertor()),
        "int" => new CommonToTargetConvertor(new HexToBitsConvertor(), new BitsToIntConvertor()),
        "bits" => new HexToBitsConvertor(),
        "array" => throw new NotImplementedException(),
        _ => throw new NotImplementedException(),
    };

    public static IConvertor CreateFromArrayToXConvertor(string to)
        => throw new NotImplementedException();
}