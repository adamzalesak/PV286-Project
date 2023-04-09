namespace Panbyte.App.Parser;

public enum Format
{
    Int,
    Hex,
    Bits,
    Bytes,
    Array
}

public static class FormatTypeExtensions
{
    public static Format ToFormatType(this string value) => value switch
    {
        "bytes" => Format.Bytes,
        "bits" => Format.Bits,
        "hex" => Format.Hex,
        "array" => Format.Array,
        "int" => Format.Int,
        _ => throw new NotSupportedException()
    };

    public static bool IsInputOptionValid(this Format formatType, string inputOption) => formatType switch
    {
        Format.Int => inputOption == "big" || inputOption == "little",
        Format.Hex => false,
        Format.Bits => inputOption == "right" || inputOption == "left",
        Format.Bytes => false,
        Format.Array => false,
        _ => false
    };

    public static bool IsOutputOptionValid(this Format formatType, string outputOption) => formatType switch
    {
        Format.Int => outputOption == "big" || outputOption == "little",
        Format.Hex => false,
        Format.Bits => false,
        Format.Bytes => false,
        Format.Array => ArgumentTypeExtensions.IsArrayToOptionValid(outputOption),
        _ => false
    };

}