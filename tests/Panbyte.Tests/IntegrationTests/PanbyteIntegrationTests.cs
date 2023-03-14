using CliWrap;
using Panbyte.Tests.Helpers;
using Xunit;

namespace Panbyte.Tests.IntegrationTests;

public class PanbyteIntegrationTests
{
    private static readonly string exePath = GetAppExePath();

    [Theory]
    [InlineData("-h", "", "", 1)]
    [InlineData("--help", "", "", 1)]
    [InlineData("-f bytes --to=bits", "test", "01110100011001010111001101110100", 0)]
    public async Task TestsWithPipes(string arguments, string input, string output, int errCode)
    {
        using var outputStream = new MemoryStream();
        var source = string.IsNullOrEmpty(input) ? PipeSource.Null : PipeSource.FromString(input);

        var app = await Cli.Wrap(exePath)
            .WithArguments(arguments)
            .WithStandardInputPipe(source)
            .WithStandardOutputPipe(PipeTarget.ToStream(outputStream))
            .WithValidation(CommandResultValidation.None)
            .ExecuteAsync();

        Assert.Equal(errCode, app.ExitCode);

        if (!string.IsNullOrEmpty(output))
        {
            Assert.Equal(output, outputStream.ToText());
        }
    }

    private static string GetAppExePath()
    {
        var releaseExePath = "../../../../../src/Panbyte.App/bin/Release/net7.0/Panbyte.App.exe";
        if (!File.Exists(releaseExePath))
        {
            return "../../../../../src/Panbyte.App/bin/Debug/net7.0/Panbyte.App.exe";
        }
        return releaseExePath;
    }
    //todo TestsWithFiles
}
