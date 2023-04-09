namespace Panbyte.App.Extensions;

public static class ByteArrayExtensions
{
    public static byte[] HandlePadding(this byte[] source, bool left = true)
    {
        if (source.Length % 8 != 0 && left)
        {
            var padding = new byte[8 - source.Length % 8];
            padding = padding.Select(_ => (byte)'0').ToArray();
            source = padding.Concat(source).ToArray();
        }
        else if (source.Length % 8 != 0 && !left)
        {
            var padding = new byte[8 - source.Length % 8];
            padding = padding.Select(_ => (byte)'0').ToArray();
            source = source.Concat(padding).ToArray();
        }

        return source;
    }
}
