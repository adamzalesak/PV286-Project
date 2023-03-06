using Panbyte.App.Convertors;

namespace Panbyte.App.Parser;

public record ParserResult(bool Success, string ErrorMessage = "")
{
    public IReadOnlyDictionary<ArgumentType, List<string>> Arguments { get; } = new Dictionary<ArgumentType, List<string>>();

    public ParserResult(IReadOnlyDictionary<ArgumentType, List<string>> arguments) : this(true)
    {
        Arguments = arguments;
    }

    public ParserResult(string errorMessage) : this(false, errorMessage) { }

    public (string Input, string Output) GetInputOutput()
    {
        Arguments.TryGetValue(ArgumentType.Input, out var input);
        Arguments.TryGetValue(ArgumentType.Output, out var output);
        return (input?.FirstOrDefault() ?? "", output?.FirstOrDefault() ?? "");
    }

    public IConvertor CreateConvertorFromArguments()
    {
        Arguments.TryGetValue(ArgumentType.From, out var tmp);
        var fromArg = tmp?.FirstOrDefault()!;

        Arguments.TryGetValue(ArgumentType.To, out tmp);
        var toArg = tmp?.FirstOrDefault()!;
        return fromArg switch
        {
            "bytes" => ConvertorFactory.CreateBytesToConvertor(toArg),
            _ => throw new NotImplementedException(),
        };
    }
}

public static class ConvertorFactory
{
    public static IConvertor CreateBytesToConvertor(string to) => to switch
    {
        "bits" => new BytesToBitsConvertor(),
        "hex" => new CommonToTargetConvertor(new BytesToBitsConvertor(), new BitsToHexConvertor()),
        _ => throw new NotImplementedException(),
    };
}
