namespace Panbyte.App.Parser;

public enum ArgumentType
{
    To,
    ToOptions,
    From,
    FromOptions,
    Input,
    Output,
    Delimiter,
}

public static class ArgumentTypeExtensions
{
    public static bool IsArgumentValueValid(this ArgumentType argumentType, string value) =>
        argumentType switch
        {
            ArgumentType.To or ArgumentType.From => value == "bytes" || value == "hex" || value == "int" || value == "bits" || value == "array",
            ArgumentType.FromOptions => value == "big" || value == "little" || value == "left" || value == "right",
            ArgumentType.ToOptions => IsToOptionValid(value),
            ArgumentType.Input or ArgumentType.Output => true,
            ArgumentType.Delimiter => true,
            _ => throw new NotImplementedException(),
        };

    public static bool IsToOptionValid(string value) =>
           value == "big"
        || value == "little"
        || value == "0x"
        || value == "0"
        || value == "0b"
        || value == "{"
        || value == "}"
        || value == "{}"
        || value == "["
        || value == "]"
        || value == "[]"
        || value == "("
        || value == ")"
        || value == "()";
}
