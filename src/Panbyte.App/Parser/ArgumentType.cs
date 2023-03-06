namespace Panbyte.App.Parser;

public enum ArgumentType
{
    To,
    ToOptions,
    From,
    FromOptions,
    Input,
    Output,
    Delimeter,
}

public static class ArgumentTypeExtensions
{
    public static bool IsArgumentValueValid(this ArgumentType argumentType, string value) =>
        argumentType switch
        {
            ArgumentType.To or ArgumentType.From => value == "bytes" || value == "hex" || value == "int" || value == "bits" || value == "array",
            ArgumentType.FromOptions => value == "big" || value == "little" || value == "left" || value == "right",
            ArgumentType.ToOptions =>
                 value == "big" || value == "little" || value == "0x" || value == "0" || value == "a" || value == "0b" || value == "{}", //todo
            ArgumentType.Input or ArgumentType.Output => true,
            ArgumentType.Delimeter => value.Length == 1,
            _ => throw new NotImplementedException(),
        };
}
