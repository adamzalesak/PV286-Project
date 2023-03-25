namespace Panbyte.App.Extensions;

public static class StreamExtensions
{
    public static void SkipBOMHeaders(this Stream source)
    {
        var headerBytes = new byte[5];
        source.Read(headerBytes, 0, headerBytes.Length);

        var seekPosition = headerBytes switch
        {
            [239, 187, 191, ..] => 3,
            [254, 255, ..] => 2,
            [255, 254, 0, 0, ..] => 4,
            [255, 254, ..] => 2,
            [0, 0, 254, 255, ..] => 4,
            [43, 47, 118, ..] => 3,
            [247, 100, 76, ..] => 3,
            _ => 0,
        };
        source.Position = seekPosition;
    }
}
