using Panbyte.App.Parser;
using Xunit;

namespace Panbyte.Tests.UnitTests;

public class ArgumentParserTests
{
    [Theory]
    [InlineData("-h")]
    [InlineData("--help")]
    [InlineData("-f", "bits", "-t", "bytes", "--help")]
    [InlineData("-f", "bytes", "-h", "-t", "bytes")]
    [InlineData("-f", "int", "--to=bytes", "-h")]
    [InlineData("-f", "bits", "--from-options=left", "-t", "bytes", "-h")]
    [InlineData("-f", "hex", "-t", "int", "--to-options=big", "--help")]
    public void IsHelpOptionProvided_WhenHelpProvided_ReturnsTrue(params string[] args)
    {
        Assert.True(new ArgumentParser(args).IsHelpOptionProvided());
    }

    [Theory]
    [InlineData("")]
    [InlineData("-f", "bits", "-t", "bytes")]
    [InlineData("-f", "bytes", "-t", "bytes")]
    [InlineData("-f", "hex", "-t", "int")]
    [InlineData("--from=hex", "--to=int")]
    [InlineData("-f", "bits", "--from-options=left", "-t", "bytes")]
    [InlineData("-f", "bits", "-t", "int", "--to-options=big")]
    public void IsHelpOptionProvided_WhenHelpNotProvided_ReturnsFalse(params string[] args)
    {
        Assert.False(new ArgumentParser(args).IsHelpOptionProvided());
    }

    [Theory]
    [InlineData("")]
    [InlineData("-f")]
    [InlineData("--from=")]
    [InlineData("-t")]
    [InlineData("--to=")]
    [InlineData("-f", "hex", "-t")]
    [InlineData("-f", "-t", "hex")]
    [InlineData("--from=", "-t", "bits")]
    [InlineData("--from=int", "--to=")]
    [InlineData("-f", "bits", "-t", "bytes", "-i")]
    [InlineData("-f", "bits", "-o", "-t", "bytes")]
    [InlineData("-f", "bits", "-t", "bytes", "--delimeter=")]
    public void Parse_WhenMissingArguments_ReturnsFalse(params string[] args)
    {
        var parserResult = new ArgumentParser(args).Parse();
        Assert.False(parserResult.Success);
    }


    [Theory]
    [InlineData("-f", "bits", "-to=bits")]
    [InlineData("-from=bits", "-t", "bits")]
    [InlineData("-f", "hex", "-t", "hex", "-a")]
    [InlineData("--from=bits", "--to=bytes", "o")]
    [InlineData("--from=bits", "--to=bytes", "-x")]
    [InlineData("-f", "hex", "-f", "bits", "-t", "hex")]
    [InlineData("-f", "bits", "-t", "hex", "-from-options=right")]
    [InlineData("-f", "bits", "-t", "int", "-to-options=big")]
    [InlineData("-f", "int", "-t", "int", "-output=x")]
    [InlineData("-f", "hex", "-t", "array", "-input=x")]
    [InlineData("-f", "hex", "-t", "bytes", "-delimeter=a")]
    public void Parse_WhenInvalidArguments_ReturnsFalse(params string[] args)
    {
        var parserResult = new ArgumentParser(args).Parse();
        Assert.False(parserResult.Success);
    }

    [Theory]
    [InlineData("-f", "hex", "-t", "a")]
    [InlineData("-f", "a", "-t", "bits")]
    [InlineData("--from=a", "-t", "bits")]
    [InlineData("--from=bits", "--to=a")]
    [InlineData("-f", "bits", "-t", "int", "--to-options=left")]
    [InlineData("-f", "bits", "-t", "int", "--from-options=big")]
    public void Parse_WhenInvalidArgumentVales_ReturnsFalse(params string[] args)
    {
        var parserResult = new ArgumentParser(args).Parse();
        Assert.False(parserResult.Success);
    }


    [Theory]
    [InlineData("-f", "bits", "-t", "bytes")]
    [InlineData("-f", "hex", "-t", "bytes" )]
    [InlineData("-f", "int", "-t", "array")]
    [InlineData("-f", "int", "-t", "array", "-d", "aa")]
    [InlineData("-f", "bytes", "-t", "bytes", "--input=input.txt")]
    [InlineData("-f", "int", "-t", "array", "-i", "input.txt", "--output=out.txt", "-d", "-", "--to-options=0")]
    public void Parse_WhenValidArguments_EqualResults(params string[] args)
    {
        var parserResult = new ArgumentParser(args).Parse();
        Assert.True(parserResult.Success);
        if (parserResult.Success)
        {
            Assert.True(parserResult.Arguments.TryGetValue(ArgumentType.From, out var fromArg));
            Assert.Equal(args[1], fromArg.First());

            Assert.True(parserResult.Arguments.TryGetValue(ArgumentType.To, out var toArg));
            Assert.Equal(args[3], toArg.First());
        }
    }
}
