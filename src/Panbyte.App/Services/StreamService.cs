namespace Panbyte.App.Services;

public class StreamService : IStreamService
{
    public bool Exists(string path)
    {
        if (path == "stdin" || path == "stdout")
        {
            return true;
        }

        throw new NotImplementedException();
    }

    public Stream Open(string? path)
    {
        if (path is null)
        {
            return Console.OpenStandardInput();
        }

        throw new NotImplementedException();
    }


    public void Save(string? path, Stream stream)
    {
        if (path is null)
        {
            var stdout = Console.OpenStandardOutput();
            using var writer = new StreamWriter(stdout);
            stream.Position = 0;
            using var reader = new StreamReader(stream);
            writer.Write(reader.ReadToEnd());
            writer.Flush();

            return;
        }

        throw new NotImplementedException();
    }
}