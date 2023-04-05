using Panbyte.App.Exceptions;
using Panbyte.App.Extensions;
using Panbyte.App.Validators;
using System.Text;

namespace Panbyte.App.Convertors;

public class ConvertorDirector
{
    private readonly IConvertor _convertor;
    private readonly IByteValidator _byteValidator;
    private readonly byte[] _rawBytesDelimiter;
    private static readonly int _bufferSize = 4096;

    public ConvertorDirector(IConvertor convertor, IByteValidator byteValidator, string delimiter)
    {
        _convertor = convertor;
        _byteValidator = byteValidator;
        _rawBytesDelimiter = Encoding.UTF8.GetBytes(delimiter);
    }

    public void Convert(Stream source, Stream destination)
    {
        var bytes = new List<byte>();
        int readByte;

        source.SkipBOMHeaders();

        while ((readByte = source.ReadByte()) != -1)
        {
            var byteValue = (byte)readByte;

            // check delimiter
            if (_rawBytesDelimiter.Any() && byteValue == _rawBytesDelimiter.First())
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

            // check or ignore invalid chars
            var byteValidationResult = _byteValidator.ValidateByte(byteValue);
            if (byteValidationResult == ByteValidation.Ignore)
            {
                continue;
            }
            if (byteValidationResult == ByteValidation.Error)
            {
                throw new InvalidFormatCharacterException(byteValue);
            }

            // convert
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
        var last2Bytes = bytes.TakeLast(2).ToArray();
        var bytesToConvert = last2Bytes switch
        {
            [13, 10] => bytes.SkipLast(2).ToArray(),
            [_, 10] => bytes.SkipLast(1).ToArray(),
            _ => bytes.ToArray()
        };
        _convertor.ConvertPart(bytesToConvert, destination);
        bytes.Clear();
    }

    private bool TryReadDelimiter(Stream source)
    {
        source.Seek(source.Position == 0 ? 0 : -1, SeekOrigin.Current);
        var delimiter = new byte[_rawBytesDelimiter.Length];
        source.Read(delimiter, 0, _rawBytesDelimiter.Length);
        return _rawBytesDelimiter.SequenceEqual(delimiter);
    }
}