using Panbyte.App.Convertors;
using Panbyte.App.Validators;

namespace Panbyte.App.Parser;

public record ParserResult(bool Success, string ErrorMessage = "")
{
    public IReadOnlyDictionary<ArgumentType, List<string>> Arguments { get; }
        = new Dictionary<ArgumentType, List<string>>();

    private readonly string delimiter = string.Empty;
    private readonly Format fromArg;
    private readonly Format toArg;

    public ParserResult(IReadOnlyDictionary<ArgumentType, List<string>> arguments) : this(true)
    {
        Arguments = arguments;

        Arguments.TryGetValue(ArgumentType.Delimiter, out var del);
        delimiter = del?.FirstOrDefault() ?? "";

        Arguments.TryGetValue(ArgumentType.From, out var tmp);
        fromArg = tmp?.FirstOrDefault("bytes").ToFormatType() ?? Format.Bytes;

        Arguments.TryGetValue(ArgumentType.To, out tmp);
        toArg = tmp?.FirstOrDefault("bytes").ToFormatType() ?? Format.Bytes;
    }

    public ParserResult(string errorMessage) : this(false, errorMessage) { }

    public (string Input, string Output) GetInputOutput()
    {
        Arguments.TryGetValue(ArgumentType.Input, out var input);
        Arguments.TryGetValue(ArgumentType.Output, out var output);
        return (input?.FirstOrDefault() ?? Constants.Stdin, output?.FirstOrDefault() ?? Constants.Stdout);
    }

    public string GetDelimiter()
    {
        if (delimiter == @"\n")
        {
            return Environment.NewLine;
        }
        if (string.IsNullOrEmpty(delimiter) && fromArg is Format.Bytes)
        {
            return string.Empty;
        }
        if (string.IsNullOrEmpty(delimiter))
        {
            return Environment.NewLine;
        }
        return delimiter;
    }

    public IByteValidator TryCreateValidator()
        => fromArg switch
        {
            Format.Bits => new BitsValidator(),
            Format.Hex => new HexValidator(),
            Format.Int => new IntValidator(),
            _ => new DefaultValidator()
        };

    public IConvertor TryCreateConvertor()
    {
        var inputOptions = GetInputOptions(Arguments, fromArg);
        var outputOptions = GetOutputOptions(Arguments, fromArg);

        return ConvertorFactory.Create(fromArg, toArg, inputOptions, outputOptions);
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
