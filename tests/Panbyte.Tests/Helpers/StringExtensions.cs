namespace Panbyte.Tests.Helpers;

public static class StringExtensions
{
    public static Stream ToStream(this string text)
    {
        MemoryStream stream = new();
        StreamWriter writer = new(stream);
        writer.Write(text);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }
}

