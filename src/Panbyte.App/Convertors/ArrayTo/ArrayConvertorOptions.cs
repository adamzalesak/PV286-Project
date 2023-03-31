using Panbyte.App.Parser;

namespace Panbyte.App.Convertors.ArrayTo;

public record ArrayConvertorOptions(string Delimiter, string[] OutputOptions, string InputOption, Format OutputFormat) : ConvertorOptions(Delimiter, InputOption);
