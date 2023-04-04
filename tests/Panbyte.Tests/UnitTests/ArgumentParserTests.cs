using Panbyte.App.Parser;
using Xunit;

namespace Panbyte.Tests.UnitTests;

public class ArgumentParserTests
{
    [Theory]
    [InlineData("-h", true)]
    [InlineData("-f", false)]
    [InlineData("--help", true)]
    public void IsHelpOptionProvided_WhenHelpProvided_ReturnsTrue(string help, bool result)
    {
        var tmp = new string[] { "a", help, "c" };
        Assert.Equal(result, new ArgumentParser(tmp).IsHelpOptionProvided());
    }

    [Theory]
    [InlineData(new string[] { "-f", "idk", "-t", "idk" }, false)]
    [InlineData(new string[] { "-f", "bits", "-t", "bytes" }, true)]
    [InlineData(new string[] { "-f", "bits", "-from-options=", "-t", "bytes" }, false)]
    public void Parse_Tests(string[] args, bool result)
    {
        var parserResult = new ArgumentParser(args).Parse();
        Assert.Equal(result, parserResult.Success);
        if (parserResult.Success)
        {
            Assert.True(parserResult.Arguments.TryGetValue(ArgumentType.From, out var fromArg));
            Assert.Equal(args[1], fromArg.First());
        }
    }
}
