using Panbyte.App.Exceptions;
using Panbyte.App.Validators;
using System.Text;

namespace Panbyte.App.Convertors;

public abstract class Convertor : Convertor<ConvertorOptions>
{
    protected Convertor(ConvertorOptions convertorOptions, IByteValidator byteValidator) : base(convertorOptions, byteValidator)
    {
    }
}


public abstract class Convertor<TOptions> : IConvertor
    where TOptions : ConvertorOptions
{
    private readonly IByteValidator _byteValidator;

    protected virtual int BufferSize { get; } = 4096;
    protected readonly ConvertorOptions _convertorOptions;
    private readonly byte[] _rawBytesDelimiter;

    protected Convertor(TOptions convertorOptions, IByteValidator byteValidator)
    {
        _convertorOptions = convertorOptions;
        _byteValidator = byteValidator;
        _rawBytesDelimiter = Encoding.UTF8.GetBytes(_convertorOptions.Delimiter);
    }

    public abstract void ConvertPart(byte[] source, Stream destination);
    public virtual bool InputDelimiterEnabled() => _rawBytesDelimiter.Any();

    public void Convert(Stream source, Stream destination)
    {
        var bytes = new List<byte>();
        int readByte;

        while ((readByte = source.ReadByte()) != -1)
        {
            var byteValue = (byte)readByte;

            switch (_byteValidator.ValidateByte(byteValue))
            {
                case ByteValidation.Ignore:
                    continue;
                case ByteValidation.Error:
                    throw new InvalidFormatCharacter();
            }

            if (InputDelimiterEnabled() && byteValue == _rawBytesDelimiter.First())
            {
                var startPos = source.Position;
                if (!TryReadDelimiter(source))
                {
                    bytes.Add(byteValue);
                    source.Seek(startPos, SeekOrigin.Begin);
                    continue;
                }

                ConvertInternal(bytes, destination);
                destination.Write(_rawBytesDelimiter);
                continue;
            }
            bytes.Add(byteValue);

            if (bytes.Count >= BufferSize)
            {
                ConvertInternal(bytes, destination);
            }
        }

        if (readByte == -1 && bytes.Count != 0)
        {
            ConvertInternal(bytes, destination);
        }
    }

    private void ConvertInternal(IList<byte> bytes, Stream destination)
    {
        ConvertPart(bytes.ToArray(), destination);
        bytes.Clear();
    }

    private bool TryReadDelimiter(Stream source)
    {
        source.Seek(source.Position == 0 ? 0 : -1, SeekOrigin.Current);
        var delimiter = new byte[_rawBytesDelimiter.Length];
        try
        {
            source.ReadExactly(delimiter, 0, _rawBytesDelimiter.Length);
        }
        catch
        {
            return false;
        }
        return _rawBytesDelimiter.SequenceEqual(delimiter);
    }
}