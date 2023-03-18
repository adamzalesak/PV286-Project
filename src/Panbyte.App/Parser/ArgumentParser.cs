﻿using System.Text.RegularExpressions;

namespace Panbyte.App.Parser;

public class ArgumentParser
{
    const int MaxArgumentLenght = 500;
    const int MaxArguments = 20;

    private readonly static Dictionary<string, ArgumentType> ArgumentAliases = new()
    {
        { "-t", ArgumentType.To},
        { "-f", ArgumentType.From},
        { "-i", ArgumentType.Input},
        { "-o", ArgumentType.Output},
        { "-d", ArgumentType.Delimiter},
    };
    private readonly static Dictionary<string, ArgumentType> ArgumentNames = new()
    {
        { "to", ArgumentType.To},
        { "from", ArgumentType.From},
        { "to-options", ArgumentType.ToOptions},
        { "from-options", ArgumentType.FromOptions},
        { "input", ArgumentType.Input},
        { "output", ArgumentType.Output},
        { "delimiter", ArgumentType.Delimiter},
    };

    private static readonly Regex longArgumentRegex = new(
        @"^--(?<name>input|output|from|from-options|to|to-options|delimiter)=(?<value>.*)$",
        RegexOptions.Compiled,
        TimeSpan.FromMilliseconds(250));

    private readonly string[] _args;
    private readonly Dictionary<ArgumentType, List<string>> _arguments = new();

    public ArgumentParser(string[] args)
    {
        if (args.Length > MaxArguments)
        {
            throw new ArgumentException("Too many arguments");
        }

        if (args.Any(i => i.Length > MaxArgumentLenght))
        {
            throw new ArgumentException("Too large argument");
        }
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
                    return new($"Argument {arg} can not have this value '{argValue}'");
                }
                if (!TryAddArgument(type, argValue))
                {
                    return new($"Duplicite argument: '{arg}'");
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
                return new($"Argument {arg} can not have this value '{argumentValue}'");
            }
            if (!TryAddArgument(type, argumentValue))
            {
                return new($"Duplicite argument: '{arg}'");
            }
        }

        //todo ArgumentValueValidator class?
        if (!_arguments.ContainsKey(ArgumentType.To))
        {
            return new("Missing 'to' argument");
        }
        if (!_arguments.ContainsKey(ArgumentType.From))
        {
            return new("Missing 'from' argument");
        }

        if (_arguments.TryGetValue(ArgumentType.Input, out var input) && input.First().IsStdinOrStdout())
        {
            return new($"'{input}' is invalid value for 'input' argument");
        }
        if (_arguments.TryGetValue(ArgumentType.Input, out var output) && output.First().IsStdinOrStdout())
        {
            return new($"'{output}' is invalid value for 'output' argument");
        }

        return new ParserResult(_arguments);
    }

    private bool TryAddArgument(ArgumentType argumentType, string value)
    {
        if (!_arguments.TryAdd(argumentType, new List<string>() { value }))
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
            var match = longArgumentRegex.Match(arg);
            if (match is null || !match.Success)
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
