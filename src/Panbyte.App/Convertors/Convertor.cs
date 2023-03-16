using System.Text;

namespace Panbyte.App.Convertors;

public abstract class Convertor : IConvertor
{
    protected readonly ConvertorOptions _convertorOptions;
    private const int _bufferSize = 4096;
    private readonly byte[] _rawBytesDelimeter;

    protected Convertor(ConvertorOptions convertorOptions)
    {
        _convertorOptions = convertorOptions;
        _rawBytesDelimeter = Encoding.UTF8.GetBytes(_convertorOptions.Delimiter);
    }

    public abstract void ConvertPart(byte[] source, Stream destination);
    public abstract bool ValidateOptions(out string errorMessage);
    public virtual bool ValidateByte(byte b) => true;
    public virtual bool IsDelimeterEntered() => false; // todo zatim nepodporovat _rawBytesDelimeter.Any();

    public void Convert(Stream source, Stream destination)
    {
        var bytes = new List<byte>();
        int readByte;

        while ((readByte = source.ReadByte()) != -1)
        {
            var byteValue = (byte)readByte;

            if (!ValidateByte(byteValue))
            {
                throw new ArgumentOutOfRangeException(nameof(source));
            }

            if (IsDelimeterEntered() && byteValue == _rawBytesDelimeter.First())
            {
                var startPos = source.Position;
                if (!TryReadDelimeter(source))
                {
                    bytes.Add(byteValue);
                    source.Seek(startPos, SeekOrigin.Begin);
                    continue;
                }

                ConvertInternal(bytes, destination);
                destination.Write(_rawBytesDelimeter);
                continue;
            }
            bytes.Add(byteValue);

            if (bytes.Count >= _bufferSize)
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

    private bool TryReadDelimeter(Stream source)
    {
        source.Seek(source.Position == 0 ? 0 : -1, SeekOrigin.Current);
        var delimeter = new byte[_rawBytesDelimeter.Length];
        try
        {
            source.ReadExactly(delimeter, 0, _rawBytesDelimeter.Length);
        }
        catch
        {
            return false;
        }
        return _rawBytesDelimeter.SequenceEqual(delimeter);
    }
}