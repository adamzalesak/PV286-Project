using System.Text;

namespace Panbyte.App.Convertors;

public abstract class Convertor : IConvertor
{
    private readonly ConvertorOptions _convertorOptions;

    protected Convertor(ConvertorOptions convertorOptions)
    {
        _convertorOptions = convertorOptions;
    }

    public abstract Stream ConvertPart(Stream source);
    public abstract bool ValidateOptions(out string errorMessage);

    public Stream Convert(Stream stream)
    {
        // separate the stream into parts by the _convertorOptions.Delimiter and convert each part by ConvertPart method. Add the delimiter to the end of each part.
        var sb = new StringBuilder();
        using var sr = new StreamReader(stream);
        
        var content = sr.ReadToEnd();
        var parts = content.Split(_convertorOptions.Delimiter);
        for (var i = 0; i < parts.Length; i++)
        {
            var partStream = new MemoryStream(Encoding.UTF8.GetBytes(parts[i]));
            var convertedPartStream = ConvertPart(partStream);
            var convertedPart = new StreamReader(convertedPartStream).ReadToEnd();
            sb.Append(convertedPart);
            if (i != parts.Length - 1)
            {
                sb.Append(_convertorOptions.Delimiter);
            }
        }

        return new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString()));
    }
}