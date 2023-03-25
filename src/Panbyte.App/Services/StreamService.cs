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

        if (!source.CanSeek)
        {
            var memoryStream = new MemoryStream();
            source.CopyTo(memoryStream);
            source = memoryStream;
            source.Seek(0, SeekOrigin.Begin);
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