using Panbyte.App.Parser;
using Panbyte.App.Services;

ArgumentParser parser;
StreamService streamService = new();

try
{
    parser = new ArgumentParser(args);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    return -1;
}

if (parser.IsHelpOptionProvided())
{
    PrintHelp();
    return 1;
}

var parserResult = parser.Parse();
if (!parserResult.Success)
{
    Console.WriteLine(parserResult.ErrorMessage);
    return -2;
}

var (input, output) = parserResult.GetInputOutput();

if (!streamService.Exists(input))
{
    Console.WriteLine($"Input file '{input}' was not found");
    return -3;
}

if (!streamService.Exists(output))
{
    Console.WriteLine($"Output file '{output}' was not found");
    return -3;
}

var convertor = parserResult.CreateConvertorFromArguments();

if (!convertor.ValidateOptions(out var optionsError))
{
    Console.WriteLine(optionsError);
    return -4;
}

try
{
    using var sourceStream = streamService.OpenInputStream(input);
    using var outputStream = streamService.OpenOutputStream(output);
    convertor.Convert(sourceStream, outputStream);
    streamService.Save(outputStream);
}
//todo catch custom exception
catch
{
    Console.WriteLine("Unknown error");
    return -5;
}

return 0;

static void PrintHelp()
{
    Console.WriteLine("help");
}
