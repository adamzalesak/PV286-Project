namespace Panbyte.App.Services;

public class StreamService : IStreamService
{
    public bool Exists(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return true;
        }
        return File.Exists(path);
    }

    public Stream OpenInputStream(string? path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return Console.OpenStandardInput();
        }
        return File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None);
    }

    public Stream OpenOutputStream(string? path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return Console.OpenStandardOutput();
        }
        return File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
    }

    public void Save(Stream stream)
    {
        stream.Flush();
    }
}