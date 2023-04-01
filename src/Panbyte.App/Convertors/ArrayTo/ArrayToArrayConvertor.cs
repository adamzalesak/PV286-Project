using Panbyte.App.Exceptions;
using Panbyte.App.Parser;

namespace Panbyte.App.Convertors.ArrayTo;

public class ArrayToArrayConvertor : IConvertor
{
    protected virtual bool WritingToDestinationEnabled => true;
    protected virtual bool SupportNesting => true;
    protected readonly ArrayConvertorOptions convertorOptions;

    private static readonly List<byte> leftBrackets = new() { (byte)'[', (byte)'(', (byte)'{' };
    private static readonly List<byte> rightBrackets = new() { (byte)']', (byte)')', (byte)'}' };

    private readonly byte outputLeftBracket;
    private readonly byte outputRightBracket;
    private readonly Format toFormat;
    private readonly string arrayFormat;
    private readonly byte[] prefix;
    private readonly byte[] suffix;

    public ArrayToArrayConvertor(ArrayConvertorOptions convertorOptions)
    {
        this.convertorOptions = convertorOptions;
        (outputLeftBracket, outputRightBracket) = SelectBracket(convertorOptions.OutputOptions.Where(i => !i.StartsWith('0') || !i.StartsWith('a')).LastOrDefault(""));
        arrayFormat = convertorOptions.OutputOptions.Where(i => i.StartsWith('0') || i.StartsWith('a')).LastOrDefault("0x");
        toFormat = arrayFormat switch
        {
            "0b" => Format.Bits,
            "0" => Format.Int,
            "a" => Format.Bytes,
            _ => Format.Hex
        };
        (prefix, suffix) = GetPrefixAndSuffix(arrayFormat);
    }

    public void ConvertPart(byte[] source, Stream destination)
    {
        source = source.Where(i => !char.IsWhiteSpace((char)i)).ToArray();

        Stack<byte> brackets = new();
        List<byte> bytesToProcess = new();
        State state = State.Start;

        if (source[0] != '"' || source[^1] != '"' || !rightBrackets.Contains(source[^2]))
        {
            throw new NotImplementedException();
        }

        for (int i = 1; i < source.Length - 1; i++)
        {
            switch (state)
            {
                case State.Start:
                    if (!leftBrackets.Contains(source[i]))
                    {
                        throw new NotImplementedException();
                    }
                    PushToStack(brackets, source[i]);
                    state = State.LeftBracket;
                    WriteByte(destination, outputLeftBracket);
                    break;
                case State.LeftBracket:
                    if (leftBrackets.Contains(source[i]))
                    {
                        PushToStack(brackets, source[i]);
                        WriteByte(destination, outputLeftBracket);
                        break;
                    }
                    if (rightBrackets.Contains(source[i]))
                    {
                        if (CheckRightBracket((char)source[i], brackets))
                        {
                            WriteByte(destination, outputRightBracket);
                            if (bytesToProcess.Any())
                            {
                                ConvertInputPart(bytesToProcess.ToArray(), destination);
                                bytesToProcess.Clear();
                            }
                            state = State.RightBracket;
                            break;
                        }
                        throw new InvalidFormatCharacter(source[i]);
                    }
                    if (source[i] != ',')
                    {
                        bytesToProcess.Add(source[i]);
                        state = State.FormattedInput;
                        break;
                    }
                    throw new NotImplementedException();
                case State.FormattedInput:
                    if (bytesToProcess.Count >= 10)
                    {
                        throw new NotImplementedException();
                    }
                    if (source[i] == ',')
                    {
                        if (bytesToProcess.Any())
                        {
                            ConvertInputPart(bytesToProcess.ToArray(), destination);
                            WriteByte(destination, source[i]);
                            WriteByte(destination, 32);
                            bytesToProcess.Clear();
                        }
                        state = State.Comma;
                        break;
                    }
                    if (rightBrackets.Contains(source[i]))
                    {
                        if (CheckRightBracket((char)source[i], brackets))
                        {
                            if (bytesToProcess.Any())
                            {
                                ConvertInputPart(bytesToProcess.ToArray(), destination);
                                bytesToProcess.Clear();
                            }
                            WriteByte(destination, outputRightBracket);
                            state = State.RightBracket;
                            break;
                        }
                        throw new InvalidFormatCharacter(source[i]);
                    }
                    bytesToProcess.Add(source[i]);
                    break;
                case State.Comma:
                    if (rightBrackets.Contains(source[i]))
                    {
                        throw new NotImplementedException();
                    }
                    if (leftBrackets.Contains(source[i]))
                    {
                        PushToStack(brackets, source[i]);
                        WriteByte(destination, outputLeftBracket);
                        state = State.LeftBracket;
                        break;
                    }
                    bytesToProcess.Add(source[i]);
                    state = State.FormattedInput;
                    break;
                case State.RightBracket:
                    if (rightBrackets.Contains(source[i]))
                    {
                        if (CheckRightBracket((char)source[i], brackets))
                        {
                            WriteByte(destination, outputRightBracket);
                            state = State.RightBracket;
                            break;
                        }
                        throw new InvalidFormatCharacter(source[i]);
                    }
                    if (source[i] == ',')
                    {
                        WriteByte(destination, source[i]);
                        WriteByte(destination, 32);
                        state = State.Comma;
                        break;
                    }
                    throw new NotImplementedException();
            }
        }
    }

    protected virtual void ConvertInputPart(byte[] bytes, Stream destination)
    {
        var (fromFormat, bytesToConvert) = GetBytes(bytes);

        var convertor = ConvertorFactory.Create(fromFormat, toFormat, Array.Empty<string>(), Array.Empty<string>());
        if (arrayFormat == "a")
        {
            convertor.ConvertPart(bytesToConvert, destination);
            return;
        }

        destination.Write(prefix);

        // trim zeros
        using var tmpStream = new MemoryStream();
        convertor.ConvertPart(bytesToConvert, tmpStream);
        var tmpArr = tmpStream.ToArray();
        var stop = false;
        for (int i = 0; i < tmpArr.Length; i++)
        {
            if (!stop && tmpArr[i] == '0')
            {
                continue;
            }
            stop = true;
            destination.WriteByte(tmpArr[i]);
        }

        destination.Write(suffix);
    }

    protected static (Format, byte[]) GetBytes(byte[] bytes)
    {
        return bytes switch
        {
            [48, 98, .. var left] => (Format.Bits, left),
            [48, 120, (>= 48 and <= 57) or (>= 65 and <= 90), (>= 48 and <= 57) or (>= 65 and <= 90)] => (Format.Hex, bytes[2..]),
            [39, 92, 120, (>= 48 and <= 57) or (>= 65 and <= 90), (>= 48 and <= 57) or (>= 65 and <= 90), 39] => (Format.Hex, bytes[3..^1]),
            [39, _, 39] => (Format.Bytes, bytes[1..2]),
            [>= 48 and <= 57, ..] => (Format.Int, bytes),
            _ => throw new NotImplementedException()
        };
    }

    private void WriteByte(Stream destination, byte value)
    {
        if (WritingToDestinationEnabled)
        {
            destination.WriteByte(value);
        }
    }

    private void PushToStack(Stack<byte> brackets, byte value)
    {
        if (!SupportNesting && brackets.Count > 1)
        {
            throw new NotImplementedException();
        }
        brackets.Push(value);
    }

    private static (byte[] Prefix, byte[] Suffix) GetPrefixAndSuffix(string arrayFormat)
    {
        return arrayFormat switch
        {
            "0" => (Array.Empty<byte>(), Array.Empty<byte>()),
            "a" => (new byte[] { 39, 92, 120 }, new byte[] { 39 }),
            "0b" => (new byte[] { 48, 98 }, Array.Empty<byte>()),
            "0x" => (new byte[] { 48, 120 }, Array.Empty<byte>()),
            _ => throw new NotSupportedException()
        };
    }

    private static bool CheckRightBracket(char actual, Stack<byte> brackets)
    {
        var leftBracket = actual switch
        {
            ']' => '[',
            ')' => '(',
            '}' => '{',
            _ => throw new InvalidFormatCharacter((byte)actual)
        };
        if (brackets.TryPop(out var bracket) && bracket == leftBracket)
        {
            return true;
        }
        return false;
    }

    private static (byte Left, byte Right) SelectBracket(string value) => value switch
    {
        "[]" or "[" or "]" => ((byte)'[', (byte)']'),
        "()" or "(" or ")" => ((byte)'(', (byte)')'),
        _ => ((byte)'{', (byte)'}'),
    };

    enum State
    {
        Start,
        End,
        LeftBracket,
        RightBracket,
        FormattedInput,
        Comma
    }
}
