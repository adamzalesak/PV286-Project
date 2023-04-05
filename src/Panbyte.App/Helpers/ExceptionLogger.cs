namespace Panbyte.App.Helpers;

public static class ExceptionLogger
{
    public static void LogToFile(Exception exception, string[] strings)
    {
        try
        {
            using var logFile = File.Open("panbyte.log", FileMode.Append);
            using var writer = new StreamWriter(logFile);
            writer.WriteLine($"[{DateTime.Now}]");
            writer.WriteLine($"Application error: {exception.Message}");
            writer.WriteLine($"Arguments: {string.Join(' ', strings)}");
            writer.WriteLine($"Exception: {exception}");
            writer.WriteLine($"Stack trace: {exception.StackTrace}");
            writer.WriteLine();
            logFile.Close();
        }
        catch
        {
            // ignored - we don't want to throw exception here
        }
    }
}

