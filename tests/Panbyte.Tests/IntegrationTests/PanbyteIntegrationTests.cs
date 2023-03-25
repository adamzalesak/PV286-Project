using CliWrap;
using Panbyte.Tests.Helpers;
using Xunit;

namespace Panbyte.Tests.IntegrationTests;

public class PanbyteIntegrationTests
{
    private const string TestDataPath = "IntegrationTests/TestData/";
    private static readonly string exePath = GetAppExePath();

    [Theory]
    [InlineData("-h", "", "", 0)]
    [InlineData("--help", "", "", 0)]
    [InlineData("-f bytes -t bytes", "test", "test", 0)]
    [InlineData("-f bytes --to=bits", "a", "01100001", 0)]
    [InlineData("-f hex -t hex", "AC19", "AC19", 0)]
    [InlineData("-f hex -t hex", "AF 12", "AF12", 0)]
    [InlineData("-f hex -t hex", "AG", "", 6)]
    [InlineData("-f bits -t bits", "AG", "", 6)]
    [InlineData("-f bits -t bits", "1001", "1001", 0)]
    [InlineData("-f bits -t bits", "10 01", "1001", 0)]
    [InlineData("-f bytes --to=bits", "test", "01110100011001010111001101110100", 0)]
    [InlineData("-f hex -t bytes", "74657374", "test", 0)]
    [InlineData("-f hex -t bytes", "74 65 73 74", "test", 0)]
    [InlineData("-f hex -t bits", "11", "00010001", 0)]
    [InlineData("-f hex -t bits", " fe  24", "1111111000100100", 0)]
    [InlineData("-f hex -t bits", "5", "", 4)]
    [InlineData("-f hex -t bits", "01100001", "00000001000100000000000000000001", 0)]
    [InlineData("-f bytes -t hex", "test", "74657374", 0)]
    [InlineData("-f bits --from-options=left -t bytes", "100 1111 0100 1011 ", "OK", 0)]
    [InlineData("-f bits --from-options=right -t hex", "100111101001011", "9e96", 0)]
    [InlineData("-f bytes -t bits", "OK", "0100111101001011", 0)]
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

        var outputText = outputStream.ToText();
        Assert.Equal(errCode, app.ExitCode);

        if (!string.IsNullOrEmpty(output))
        {
            Assert.Equal(output, outputText);
        }
    }

    [Theory]
    [FileData(TestDataPath + "result1.txt", $"-f hex -t bytes --delimiter=-- --input={TestDataPath}/test1.txt --output=result1.txt", "result1.txt", 0)]
    [FileData(TestDataPath + "result2.txt", $"-f bytes -t bytes --delimiter=kk --input={TestDataPath}/test2.txt --output=result2.txt", "result2.txt", 0)]
    [FileData(TestDataPath + "result3.txt", $"-f bits --from-options=right -t hex -d *a* --input={TestDataPath}/test3.txt --output=result3.txt", "result3.txt", 0)]
    [InlineData("", "-f hex -t bytes -delimiter=--", "", 2)]
    public async Task TestsWithFiles(string validOutputFileContent, string arguments, string resultFilePath, int errCode)
    {
        using var debugOutputStream = new MemoryStream();

        var app = await Cli.Wrap(exePath)
            .WithArguments(arguments)
            .WithValidation(CommandResultValidation.None)
            .WithStandardOutputPipe(PipeTarget.ToStream(debugOutputStream))
            .ExecuteAsync();

        var debugText = debugOutputStream.ToText();
        Assert.Equal(errCode, app.ExitCode);

        if (!string.IsNullOrEmpty(validOutputFileContent))
        {
            var outputText = FileHelper.ReadFile(resultFilePath);
            Assert.Equal(validOutputFileContent, outputText);
        }
    }

    private static string GetAppExePath()
    {
        var releaseExePath = "../../../../../src/Panbyte.App/bin/Release/net7.0/panbyte.exe";
        if (!File.Exists(releaseExePath))
        {
            releaseExePath = "../../../../../src/Panbyte.App/bin/Debug/net7.0/panbyte.exe";
        }
        return Environment.GetEnvironmentVariable("ACTIONS_APP_PATH") ?? releaseExePath;
    }
}
