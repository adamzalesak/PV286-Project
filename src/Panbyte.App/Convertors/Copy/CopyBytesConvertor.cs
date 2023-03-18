namespace Panbyte.App.Convertors.Copy;

public class CopyBytesConvertor : IConvertor
{
    public void Convert(Stream source, Stream destination) => source.CopyTo(destination);

    public bool ValidateOptions(out string errorMessage)
    {
        errorMessage = "";
        return true;
    }
}
