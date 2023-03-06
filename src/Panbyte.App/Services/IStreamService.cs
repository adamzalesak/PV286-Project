namespace Panbyte.App.Services;

public interface IStreamService
{
    bool Exists(string path);
    Stream Open(string path);
    void Save(string path, Stream stream);
}
