using Panbyte.App;
using Panbyte.App.Convertors;
using Panbyte.App.Exceptions;
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
    return 1;
}

if (parser.IsHelpOptionProvided())
{
    PrintHelp();
    return 0;
}

var parserResult = parser.Parse();
if (!parserResult.Success)
{
    Console.WriteLine(parserResult.ErrorMessage);
    return 2;
}

var (input, output) = parserResult.GetInputOutput();

if (!streamService.Exists(input))
{
    Console.WriteLine($"Input file '{input}' was not found");
    return 3;
}

var convertor = parserResult.TryCreateConvertor();
var validator = parserResult.TryCreateValidator();
var director = new ConvertorDirector(convertor, validator, parserResult.GetDelimiter());

try
{
    using var sourceStream = streamService.OpenInputStream(input);
    using var outputStream = streamService.OpenOutputStream(output);
    director.Convert(sourceStream, outputStream);
    streamService.Save(outputStream);
}
catch (Exception ex)
{
    var message = ex switch
    {
        InvalidFormatException or InvalidFormatCharacterException or NotSupportedException or NotImplementedException => ex.Message,
        _ => "Application error"
    };

    if (output != Constants.Stdout)
    {
        File.Delete(output);
    }
    Console.WriteLine(message);
    return 6;
}

return 0;

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
        "-d delimiter  --delimiter=delimiter   Record delimiter (default newline - for newline enter \\n)\n" +
        "-h            --help                  Print help\n\n" +
        "FORMATS:\n" +
        "bytes         Raw bytes\n" +
        "hex           Hex-encoded string\n" +
        "int           Integer\n" +
        "bits          0,1-represented bits\n" +
        "array         Byte array\n"
    );
}
