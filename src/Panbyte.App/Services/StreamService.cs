namespace Panbyte.App.Services;

public class StreamService : IStreamService
{
    public bool Exists(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return false;
        }

        if (path.IsStdinOrStdout())
        {
            return true;
        }

        return File.Exists(path);
    }

    public Stream OpenInputStream(string path)
    {
        var source = path == Constants.Stdin
            ? Console.OpenStandardInput()
            : File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None);

        if (!source.CanSeek && Console.IsInputRedirected)
        {
            var memoryStream = new MemoryStream();
            source.CopyTo(memoryStream);
            source.Dispose();
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        if (!source.CanSeek)
        {
            var memoryStream = new MemoryStream();

            ConsoleKeyInfo keyInfo = new();
            while (keyInfo.Key != ConsoleKey.Escape)
            {
                keyInfo = Console.ReadKey(true);
                if ((keyInfo.Modifiers & ConsoleModifiers.Control) != 0 && keyInfo.Key == ConsoleKey.D)
                {
                    break;
                }
                memoryStream.WriteByte((byte)keyInfo.KeyChar);
                Console.Write(keyInfo.KeyChar);
            }

            Console.WriteLine();
            source.Dispose();
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        return source;
    }

    public Stream OpenOutputStream(string path)
    {
        if (path == Constants.Stdout)
        {
            return Console.OpenStandardOutput();
        }

        return File.Open(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
    }

    public void Save(Stream stream)
    {
        stream.Flush();
    }
}