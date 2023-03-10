namespace Panbyte.App.Convertors;

public record ConvertorOptions(
    ICollection<string> FromOptions,
    ICollection<string> ToOptions,
    string Delimiter);
