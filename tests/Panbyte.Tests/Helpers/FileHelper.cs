namespace Panbyte.Tests.Helpers;

public static class FileHelper
{
    public static string ReadFile(string filePath)
    {
        var path = Path.IsPathRooted(filePath)
            ? filePath
            : Path.GetRelativePath(Directory.GetCurrentDirectory(), filePath);

        if (!File.Exists(path))
        {
            throw new ArgumentException($"Could not find file at path: {path}");
        }

        return File.ReadAllText(path);
    }
}