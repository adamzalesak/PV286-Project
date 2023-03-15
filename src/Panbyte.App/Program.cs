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

if (input is not null && !streamService.Exists(input))
{
    Console.WriteLine($"Input file '{input}' was not found");
    Environment.Exit(-3);
}

if (output is not null && !streamService.Exists(output))
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
    Console.Write(
        "./panbyte [ARGS...]\n\n" +
        "ARGS:\n" +
        "-f FORMAT     --from=FORMAT           Set input data format\n" +
        "              --from-options=OPTIONS  Set input options\n" +
        "-t FORMAT     --to=FORMAT             Set output data format\n" +
        "              --to-options=OPTIONS    Set output options\n" +
        "-i FILE       --input=FILE            Set input file (default stdin)\n" +
        "-o FILE       --output=FILE           Set output file (default stdout)\n" +
        "-d DELIMITER  --delimiter=DELIMITER   Record delimiter (default newline)\n" +
        "-h            --help                  Print help\n\n" +
        "FORMATS:\n" +
        "bytes         Raw bytes\n" +
        "hex           Hex-encoded string\n" +
        "int           Integer\n" +
        "bits          0,1-represented bits\n" +
        "array         Byte array\n"
    );
}