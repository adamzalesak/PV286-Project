namespace Panbyte.App.Extensions;

public static class ByteArrayExtensions
{
    public static byte[] HandlePadding(this byte[] source, int padTo, bool left = true)
    {
        if (source.Length % padTo != 0 && left)
        {
            var padding = new byte[padTo - source.Length % padTo];
            padding = padding.Select(_ => (byte)'0').ToArray();
            source = padding.Concat(source).ToArray();
        }
        else if (source.Length % padTo != 0 && !left)
        {
            var padding = new byte[padTo - source.Length % padTo];
            padding = padding.Select(_ => (byte)'0').ToArray();
            source = source.Concat(padding).ToArray();
        }

        return source;
    }
}
