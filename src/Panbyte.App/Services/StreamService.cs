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
        if (path == Constants.Stdin)
        {
            return Console.OpenStandardInput();
        }
        return File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None);
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