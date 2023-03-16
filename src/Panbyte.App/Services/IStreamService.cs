namespace Panbyte.App.Services;

public interface IStreamService
{
    bool Exists(string path);
    Stream OpenInputStream(string path);
    Stream OpenOutputStream(string path);
    void Save(Stream stream);
}
