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
    Environment.Exit(1);
    return;
}

if (parser.IsHelpOptionProvided())
{
    PrintHelp();
    Environment.Exit(0);
}

var parserResult = parser.Parse();
if (!parserResult.Success)
{
    Console.WriteLine(parserResult.ErrorMessage);
    Environment.Exit(-2);
}

var (input, output) = parserResult.GetInputOutput();

if (streamService.Exists(input))
{
    Console.WriteLine($"Input file '{input}' was not found");
    Environment.Exit(-3);
}

if (streamService.Exists(output))
{
    Console.WriteLine($"Output file '{output}' was not found");
    Environment.Exit(-3);
}

var convertor = parserResult.CreateConvertorFromArguments();

if (!convertor.ValidateOptions(out var optionsError))
{
    Console.WriteLine(optionsError);
    Environment.Exit(-4);
}

using var sourceStream = streamService.Open(input);
using var outputStream = convertor.Convert(sourceStream);
streamService.Save(output, outputStream);


static void PrintHelp()
{
    Console.WriteLine("help");
}