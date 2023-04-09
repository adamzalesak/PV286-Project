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
        (outputLeftBracket, outputRightBracket) = SelectBracket(convertorOptions.OutputOptions.Where(i => !i.StartsWith('0') && !i.StartsWith('a')).LastOrDefault(""));
        arrayFormat = convertorOptions.OutputOptions.Where(i => i.StartsWith('0') || i.StartsWith('a')).LastOrDefault("0x");
        toFormat = arrayFormat switch
        {
            "0b" => Format.Bits,
            "0" => Format.Int,
            _ => Format.Hex
        };
        (prefix, suffix) = GetPrefixAndSuffix(arrayFormat);
    }

    public void ConvertPart(byte[] source, Stream destination)
    {
        source = source.Where(i => !char.IsWhiteSpace((char)i) && i != 0).ToArray();

        Stack<byte> brackets = new();
        List<byte> bytesToProcess = new();
        State state = State.Start;

        if (!leftBrackets.Contains(source[0]) || !rightBrackets.Contains(source[^1]))
        {
            throw new InvalidFormatException("Array does not begin or end with bracket.");
        }

        for (int i = 0; i < source.Length; i++)
        {
            switch (state)
            {
                case State.Start:
                    if (!leftBrackets.Contains(source[i]))
                    {
                        throw new InvalidFormatException("Array does not begin with left bracket");
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
                        throw new InvalidFormatCharacterException(source[i]);
                    }
                    if (source[i] != ',')
                    {
                        bytesToProcess.Add(source[i]);
                        state = State.FormattedInput;
                        break;
                    }
                    throw new InvalidFormatException();
                case State.FormattedInput:
                    if (bytesToProcess.Count > 10)
                    {
                        // everything else is surely an error
                        throw new InvalidFormatException();
                    }
                    if (source[i] == ',')
                    {
                        if (bytesToProcess.Any())
                        {
                            ConvertInputPart(bytesToProcess.ToArray(), destination);
                            WriteByte(destination, source[i]);
                            WriteByte(destination, (byte)' ');
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
                        throw new InvalidFormatCharacterException(source[i]);
                    }
                    bytesToProcess.Add(source[i]);
                    break;
                case State.Comma:
                    if (rightBrackets.Contains(source[i]))
                    {
                        throw new InvalidFormatCharacterException(source[i]);
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
                        throw new InvalidFormatCharacterException(source[i]);
                    }
                    if (source[i] == ',')
                    {
                        WriteByte(destination, source[i]);
                        WriteByte(destination, (byte)' ');
                        state = State.Comma;
                        break;
                    }
                    throw new InvalidFormatException();
                default:
                    throw new NotImplementedException();
            }
        }
    }

    protected virtual void ConvertInputPart(byte[] bytes, Stream destination)
    {
        var (fromFormat, bytesToConvert) = GetBytes(bytes);

        var options = toFormat == Format.Int && fromFormat != Format.Int ? new[] { "little" } : Array.Empty<string>();
        var convertor = ConvertorFactory.Create(fromFormat, toFormat, Array.Empty<string>(), options);

        destination.Write(prefix);
        ConvertAndTrimLeadingZeros(convertor, bytesToConvert, destination);
        destination.Write(suffix);
    }

    private void ConvertAndTrimLeadingZeros(IConvertor convertor, byte[] bytesToConvert, Stream destination)
    {
        if (arrayFormat == "a")
        {
            convertor.ConvertPart(bytesToConvert, destination);
            return;
        }

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
    }

    protected static (Format, byte[]) GetBytes(byte[] bytes)
    {
        return bytes switch
        {
            [48, 98, .. var left] => (Format.Bits, left),
            [48, 120, (>= 48 and <= 57) or (>= 65 and <= 90) or (>= 97 and <= 122)] => (Format.Hex, bytes[2..]),
            [48, 120, (>= 48 and <= 57) or (>= 65 and <= 90) or (>= 97 and <= 122), (>= 48 and <= 57) or (>= 65 and <= 90) or (>= 97 and <= 122)] => (Format.Hex, bytes[2..]),
            [39, 92, 120, (>= 48 and <= 57) or (>= 65 and <= 90) or (>= 97 and <= 122), (>= 48 and <= 57) or (>= 65 and <= 90) or (>= 97 and <= 122), 39] => (Format.Hex, bytes[3..^1]),
            [39, _, 39] => (Format.Bytes, bytes[1..2]),
            [39, 39] => (Format.Bytes, Array.Empty<byte>()),
            [>= 48 and <= 57, ..] => (Format.Int, bytes),
            _ => throw new InvalidFormatException()
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
            throw new InvalidFormatException("Format has nested brackets");
        }
        brackets.Push(value);
    }

    private static (byte[] Prefix, byte[] Suffix) GetPrefixAndSuffix(string arrayFormat) => arrayFormat switch
    {
        "0" => (Array.Empty<byte>(), Array.Empty<byte>()),
        "a" => ("'\\x"u8.ToArray(), "'"u8.ToArray()),
        "0b" => ("0b"u8.ToArray(), Array.Empty<byte>()),
        "0x" => ("0x"u8.ToArray(), Array.Empty<byte>()),
        _ => throw new InvalidFormatException()
    };

    private static bool CheckRightBracket(char actual, Stack<byte> brackets)
    {
        var leftBracket = actual switch
        {
            ']' => '[',
            ')' => '(',
            '}' => '{',
            _ => throw new InvalidFormatCharacterException((byte)actual)
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
