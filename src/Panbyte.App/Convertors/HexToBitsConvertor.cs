namespace Panbyte.App.Convertors;

public class HexToBitsConvertor : Convertor
{
    public HexToBitsConvertor(ConvertorOptions convertorOptions, IByteValidator byteValidator) : base(convertorOptions, byteValidator)
    {
    }

    public override void ConvertPart(byte[] source, Stream destination)
    {
        var sourceString = System.Text.Encoding.ASCII.GetString(source);
        // remove all whitespaces
        sourceString = Regex.Replace(sourceString, @"\s+", "");

        if (sourceString.Length % 2 != 0)
        {
            throw new ArgumentException("Invalid input value");
        }

        byte hexByte;
        string bitString;

        for (int i = 0; i < sourceString.Length; i += 2)
        {
            hexByte = System.Convert.ToByte(sourceString.Substring(i, 2), 16);
            bitString = System.Convert.ToString(hexByte, 2).PadLeft(8, '0');
            byte[] bitBytes = System.Text.Encoding.ASCII.GetBytes(bitString);

            destination.Write(bitBytes);
        }

        destination.Flush();
    }
}
