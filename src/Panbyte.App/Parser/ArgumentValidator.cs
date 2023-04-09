namespace Panbyte.App.Parser;

public static class ArgumentValidator
{
    public static bool Validate(Dictionary<ArgumentType, List<string>> arguments, out string errorMessage)
    {
        errorMessage = "";
        if (!arguments.TryGetValue(ArgumentType.To, out var to))
        {
            errorMessage = "Missing 'to' argument";
            return false;
        }
        if (!arguments.TryGetValue(ArgumentType.From, out var from))
        {
            errorMessage = "Missing 'from' argument";
            return false;
        }

        if (arguments.TryGetValue(ArgumentType.Input, out var input) && input.First().IsStdinOrStdout())
        {
            errorMessage = $"'{input}' is invalid value for 'input' argument";
            return false;
        }
        if (arguments.TryGetValue(ArgumentType.Output, out var output) && output.First().IsStdinOrStdout())
        {
            errorMessage = $"'{output}' is invalid value for 'output' argument";
            return false;
        }

        var toFormat = to.FirstOrDefault("bytes").ToFormatType();
        if (arguments.TryGetValue(ArgumentType.ToOptions, out var toOptions)
            && toOptions.Any(i => !toFormat.IsOutputOptionValid(i)))
        {
            errorMessage = "Invalid output options";
            return false;
        }

        var fromFormat = from.FirstOrDefault("bytes").ToFormatType();
        if (arguments.TryGetValue(ArgumentType.FromOptions, out var fromOptions)
            && fromOptions.Any(i => !fromFormat.IsInputOptionValid(i)))
        {
            errorMessage = "Invalid input options";
            return false;
        }
        return true;
    }
}
