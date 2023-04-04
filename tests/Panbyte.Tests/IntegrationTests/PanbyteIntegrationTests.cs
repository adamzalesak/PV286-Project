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
    [InlineData("-f hex -t bits", "5", "", 6)]
    [InlineData("-f hex -t bits", "01100001", "00000001000100000000000000000001", 0)]
    [InlineData("-f bytes -t hex", "test", "74657374", 0)]
    [InlineData("-f bits --from-options=left -t bytes", "100 1111 0100 1011 ", "OK", 0)]
    [InlineData("-f bits --from-options=right -t hex", "100111101001011", "9e96", 0)]
    [InlineData("-f bytes -t bits", "OK", "0100111101001011", 0)]
    //int tests:
    [InlineData("-f int -t hex", "1234567890", "499602D2", 0)]
    [InlineData("-f int -t hex --from-options=big", "1234567890", "499602D2", 0)]
    [InlineData("-f int -t hex --from-options=little", "1234567890", "D2029649", 0)]
    [InlineData("-f hex -t int", "499602D2", "1234567890", 0)]
    [InlineData("-f hex -t int --to-options=big", "499602D2", "1234567890", 0)]
    [InlineData("-f hex -t int --to-options=little", "D2029649", "1234567890", 0)]
    //array tests:
    [InlineData("-f hex -t array", "01020304", "{0x1, 0x2, 0x3, 0x4}", 0)]
    [InlineData("-f array -t hex", "\"{0x01, 2, 0b11, '\\x04'}\"", "01020304", 0)]
    [InlineData("-f array -t array", "\"{0x01,2,0b11 ,'\\x04' }\"", "{0x1, 0x2, 0x3, 0x4}", 0)]
    [InlineData("-f array -t array --to-options=0x", "\"[0x01, 2, 0b11, '\\x04']\"", "{0x1, 0x2, 0x3, 0x4}", 0)]
    [InlineData("-f array -t array --to-options=0", "\"(0x01, 2, 0b11, '\\x04')\"", "{1, 2, 3, 4}", 0)]
    [InlineData("-f array -t array --to-options=a", "\"{0x01, 2, 0b11, '\\x04'}\"", "{'\\x01', '\\x02', '\\x03', '\\x04'}", 0)]
    [InlineData("-f array -t array --to-options=0b", "\"[0x01, 2, 0b11, '\\x04']\"", "{0b1, 0b10, 0b11, 0b100}", 0)]
    [InlineData("-f array -t array --to-options=\"(\"", "\"(0x1, 0x2, 0x3, 0x4)\"", "(0x1, 0x2, 0x3, 0x4)", 0)]
    [InlineData("-f array -t array --to-options=0 --to-options=\"[\"", "\"{0x01, 2, 0b11, '\\x04'}\"", "[1, 2, 3, 4]", 0)]
    [InlineData("-f array -t array", "\"[[1, 2], [3, 4], [5, 6]]\"", "{{0x1, 0x2}, {0x3, 0x4}, {0x5, 0x6}}", 0)]
    [InlineData("-f array -t array --to-options=\"{\" --to-options=0", "\"[[1, 2], [3, 4], [5, 6]]\"", "{{1, 2}, {3, 4}, {5, 6}}", 0)]
    [InlineData("-f array -t array --to-options=0 --to-options=\"[\"", "\"{{0x01, (2), [3, 0b100, 0x05], '\\x06'}}\"", "[[1, [2], [3, 4, 5], 6]]", 0)]
    [InlineData("-f array -t array", "\"()\"", "{}", 0)]
    [InlineData("-f array -t array --to-options=\"[\"", "\"([],{})\"", "[[], []]", 0)]
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
    [FileData(TestDataPath + "result4.txt", $"-d , -f bits -t array --input={TestDataPath}/test4.txt --output=result4.txt", "result4.txt", 0)]
    [InlineData("", "-f hex -t bytes -delimiter=--", "", 2)]
    public async Task TestsWithFiles(string validOutputFileContent, string arguments, string resultFilePath, int errCode)
    {
        using var debugOutputStream = new MemoryStream();

        var app = await Cli.Wrap(exePath)
            .WithArguments(arguments)
            .WithValidation(CommandResultValidation.None)
            .WithStandardOutputPipe(PipeTarget.ToStream(debugOutputStream))
            .ExecuteAsync();

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
