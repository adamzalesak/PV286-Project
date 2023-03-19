using Panbyte.App.Convertors;

namespace Panbyte.App.Parser;

public record ParserResult(bool Success, string ErrorMessage = "")
{
    public IReadOnlyDictionary<ArgumentType, List<string>> Arguments { get; }
        = new Dictionary<ArgumentType, List<string>>();

    public ParserResult(IReadOnlyDictionary<ArgumentType, List<string>> arguments) : this(true)
    {
        Arguments = arguments;
    }

    public ParserResult(string errorMessage) : this(false, errorMessage) { }

    public (string Input, string Output) GetInputOutput()
    {
        Arguments.TryGetValue(ArgumentType.Input, out var input);
        Arguments.TryGetValue(ArgumentType.Output, out var output);
        return (input?.FirstOrDefault() ?? Constants.Stdin, output?.FirstOrDefault() ?? Constants.Stdout);
    }

    public IConvertor TryCreateConvertor()
    {
        Arguments.TryGetValue(ArgumentType.From, out var tmp);
        var fromArg = tmp?.FirstOrDefault()?.ToFormatType() ?? throw new NotImplementedException();

        Arguments.TryGetValue(ArgumentType.To, out tmp);
        var toArg = tmp?.FirstOrDefault()?.ToFormatType() ?? throw new NotImplementedException();

        Arguments.TryGetValue(ArgumentType.del, out var del);
        var delArg = del?.FirstOrDefault() ?? Environment.NewLine;

        var inputOptions = GetInputOptions(Arguments, fromArg);
        var outputOptions = GetOutputOptions(Arguments, fromArg);

        return ConvertorFactory.Create(fromArg, toArg, delArg, inputOptions, outputOptions);
    }

    private static string[] GetInputOptions(IReadOnlyDictionary<ArgumentType, List<string>> arguments, Format format)
    {
        if (arguments.TryGetValue(ArgumentType.FromOptions, out var fromOptions))
        {
            return fromOptions.Where(i => format.IsInputOptionValid(i)).ToArray();
        }
        return Array.Empty<string>();
    }

    private static string[] GetOutputOptions(IReadOnlyDictionary<ArgumentType, List<string>> arguments, Format format)
    {
        if (arguments.TryGetValue(ArgumentType.ToOptions, out var fromOptions))
        {
            return fromOptions.Where(i => format.IsOutputOptionValid(i)).ToArray();
        }
        return Array.Empty<string>();
    }
}


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
    };

    public static bool IsOutputOptionValid(this Format formatType, string outputOption) => formatType switch
    {
        Format.Int => throw new NotImplementedException(),
        Format.Hex => throw new NotImplementedException(),
        Format.Bits => throw new NotImplementedException(),
        Format.Bytes => throw new NotImplementedException(),
        Format.Array => throw new NotImplementedException(),
    };
}