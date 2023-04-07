using System.Text.RegularExpressions;

namespace Panbyte.App.Parser;

public class ArgumentParser
{
    private static readonly Dictionary<string, ArgumentType> ArgumentAliases = new()
    {
        { "-t", ArgumentType.To },
        { "-f", ArgumentType.From },
        { "-i", ArgumentType.Input },
        { "-o", ArgumentType.Output },
        { "-d", ArgumentType.Delimiter },
    };

    private static readonly Dictionary<string, ArgumentType> ArgumentNames = new()
    {
        { "to", ArgumentType.To },
        { "from", ArgumentType.From },
        { "to-options", ArgumentType.ToOptions },
        { "from-options", ArgumentType.FromOptions },
        { "input", ArgumentType.Input },
        { "output", ArgumentType.Output },
        { "delimiter", ArgumentType.Delimiter },
    };

    private static readonly Regex LongArgumentRegex = new(
        @"^--(?<name>input|output|from|from-options|to|to-options|delimiter)=(?<value>.+)$",
        RegexOptions.Compiled,
        TimeSpan.FromMilliseconds(250));

    private readonly string[] _args;
    private readonly Dictionary<ArgumentType, List<string>> _arguments = new();

    public ArgumentParser(string[] args)
    {
        _args = args;
    }

    public bool IsHelpOptionProvided() => _args.Contains("-h") || _args.Contains("--help");

    public ParserResult Parse()
    {
        for (int i = 0; i < _args.Length; i++)
        {
            var arg = _args[i];
            if (ArgumentAliases.TryGetValue(arg, out var type))
            {
                i++;
                var argValue = _args.ElementAtOrDefault(i);
                if (string.IsNullOrEmpty(argValue))
                {
                    return new($"Argument: '{arg}' has no value");
                }
                if (!type.IsArgumentValueValid(argValue))
                {
                    return new($"Argument {arg} cannot have this value '{argValue}'");
                }
                if (!TryAddArgument(type, argValue))
                {
                    return new($"Duplicate argument: '{arg}'");
                }
                continue;
            }
            if (!TryMatchArgument(arg, out var argumentName, out var argumentValue))
            {
                return new($"Parsing failed: '{arg}'");
            }
            if (!ArgumentNames.TryGetValue(argumentName, out type))
            {
                return new($"Unknown argument: '{arg}'");
            }
            if (!type.IsArgumentValueValid(argumentValue))
            {
                return new($"Argument {arg} cannot have this value '{argumentValue}'");
            }
            if (!TryAddArgument(type, argumentValue))
            {
                return new($"Duplicate argument: '{arg}'");
            }
        }

        if (!ArgumentValidator.Validate(_arguments, out var validationError))
        {
            return new(validationError);
        }

        return new ParserResult(_arguments);
    }

    private bool TryAddArgument(ArgumentType argumentType, string value)
    {
        if (!_arguments.TryAdd(argumentType, new List<string> { value }))
        {
            if (argumentType == ArgumentType.ToOptions || argumentType == ArgumentType.FromOptions)
            {
                _arguments[argumentType].Add(value);
                return true;
            }
            return false;
        }
        return true;
    }

    private static bool TryMatchArgument(string arg, out string argumentName, out string argumentValue)
    {
        argumentName = string.Empty;
        argumentValue = string.Empty;
        try
        {
            var match = LongArgumentRegex.Match(arg);
            if (!match.Success)
            {
                return false;
            }
            argumentName = match.Groups["name"].Value;
            argumentValue = match.Groups["value"].Value;
            return true;
        }
        catch
        {
            return false;
        }
    }
}
