namespace Panbyte.App.Convertors;

public abstract class Convertor : IConvertor
{
    private readonly ConvertorOptions _convertorOptions;
    private const int _bufferSize = 4096;

    protected Convertor(ConvertorOptions convertorOptions)
    {
        _convertorOptions = convertorOptions;
    }

    public abstract void ConvertPart(byte[] source, Stream destination);
    public abstract bool ValidateOptions(out string errorMessage);
    public virtual bool ValidateByte(byte b) => true;

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

            //todo zde se jeste uvidi
            if ((char)byteValue == _convertorOptions.Delimiter.First())
            {
                ConvertPart(bytes.ToArray(), destination);
                bytes.Clear();
                continue;
            }

            bytes.Add(byteValue);

            if (bytes.Count >= _bufferSize)
            {
                ConvertPart(bytes.ToArray(), destination);
                bytes.Clear();
            }
        }

        if (readByte == -1 && bytes.Count != 0)
        {
            ConvertPart(bytes.ToArray(), destination);
        }
    }
}