using Panbyte.App.Parser;

namespace Panbyte.App.Convertors.ArrayTo;

public record ToArrayConvertorOptions(Format FromFormat, ICollection<string> OutputOptions, IConvertor? Convertor = null);
