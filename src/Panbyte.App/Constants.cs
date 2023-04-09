namespace Panbyte.App;

public static class Constants
{
    public static readonly string Stdin = "stdin";
    public static readonly string Stdout = "stdout";

    private static readonly IReadOnlyList<string> supportedFormats = new List<string> { "bytes", "hex", "int", "bits" };

    public static bool IsStdinOrStdout(this string path) => path == Stdin || path == Stdout;

    public static bool IsFormatSupported(this string format) => supportedFormats.Contains(format);
}
