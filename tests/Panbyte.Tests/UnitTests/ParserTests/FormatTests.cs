using Panbyte.App.Parser;
using Xunit;

namespace Panbyte.Tests.UnitTests.ParserTests;

public class FormatTests
{
    [Theory]
    [InlineData(Format.Int, "big")]
    [InlineData(Format.Int, "little")]
    [InlineData(Format.Bits, "right")]
    [InlineData(Format.Bits, "left")]
    public void IsInputOptionValid_WhenValidInputOption_ReturnsTrue(Format format, string inputOption)
    {
        var result = format.IsInputOptionValid(inputOption);
        Assert.True(result);
    }

    [Theory]
    [InlineData(Format.Int, "huge")]
    [InlineData(Format.Int, "small")]
    [InlineData(Format.Int, "")]
    [InlineData(Format.Bits, "up")]
    [InlineData(Format.Bits, "down")]
    [InlineData(Format.Bits, "")]
    [InlineData(Format.Array, "test")]
    [InlineData(Format.Array, "")]
    [InlineData(Format.Bytes, "test")]
    [InlineData(Format.Bytes, "")]
    [InlineData(Format.Hex, "test")]
    [InlineData(Format.Hex, "")]
    public void IsInputOptionValid_WhenInvalidInputOption_ReturnsFalse(Format format, string inputOption)
    {
        var result = format.IsInputOptionValid(inputOption);
        Assert.False(result);
    }
    
    [Theory]
    [InlineData(Format.Int, "big")]
    [InlineData(Format.Int, "little")]
    [InlineData(Format.Array, "0x")]
    [InlineData(Format.Array, "0")]
    [InlineData(Format.Array, "a")]
    [InlineData(Format.Array, "0b")]
    [InlineData(Format.Array, "{")]
    [InlineData(Format.Array, "}")]
    [InlineData(Format.Array, "{}")]
    [InlineData(Format.Array, "[")]
    [InlineData(Format.Array, "]")]
    [InlineData(Format.Array, "[]")]
    [InlineData(Format.Array, "(")]
    [InlineData(Format.Array, ")")]
    [InlineData(Format.Array, "()")]
    public void IsOutputOptionValid_WhenValidOutputOption_ReturnsTrue(Format format, string outputOption)
    {
        var result = format.IsOutputOptionValid(outputOption);
        Assert.True(result);
    }

    [Theory]
    [InlineData(Format.Int, "huge")]
    [InlineData(Format.Int, "small")]
    [InlineData(Format.Int, "")]
    [InlineData(Format.Bits, "right")]
    [InlineData(Format.Bits, "left")]
    [InlineData(Format.Bits, "test")]
    [InlineData(Format.Bits, "")]
    [InlineData(Format.Bytes, "test")]
    [InlineData(Format.Bytes, "")]
    [InlineData(Format.Hex, "test")]
    [InlineData(Format.Hex, "")]
    [InlineData(Format.Array, "test")]
    [InlineData(Format.Array, "")]
    [InlineData(Format.Array, "OO")]
    [InlineData(Format.Array, "aa")]
    [InlineData(Format.Array, "0x0")]
    [InlineData(Format.Array, "0a")]
    [InlineData(Format.Array, "0bb")]
    [InlineData(Format.Array, "{{")]
    [InlineData(Format.Array, "{}}")]
    [InlineData(Format.Array, "{{}")]
    [InlineData(Format.Array, "((")]
    [InlineData(Format.Array, "())")]
    [InlineData(Format.Array, "(()")]
    [InlineData(Format.Array, "[[")]
    [InlineData(Format.Array, "[]]")]
    [InlineData(Format.Array, "[[]")]
    public void IsOutputOptionValid_WhenInvalidOutputOption_ReturnsFalse(Format format, string outputOption)
    {
        var result = format.IsOutputOptionValid(outputOption);
        Assert.False(result);
    }
}