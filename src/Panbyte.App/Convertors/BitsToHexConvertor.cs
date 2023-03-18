﻿using Panbyte.App.Validators;

namespace Panbyte.App.Convertors;

public class BitsToHexConvertor : Convertor
{
    private readonly string _padding;

    public BitsToHexConvertor(ConvertorOptions convertorOptions, IByteValidator byteValidator)
        : base(convertorOptions, byteValidator)
    {
        _padding = _convertorOptions.InputOption;
    }

    public override void ConvertPart(byte[] source, Stream destination)
    {
        source = HandlePadding(source);

        var bytes = GetBytes(source);

        var hex = BitConverter.ToString(bytes.ToArray());
        hex = hex.Replace("-", "").ToLower();
        var byteArray = System.Text.Encoding.ASCII.GetBytes(hex);

        destination.Write(byteArray, 0, byteArray.Length);
    }


    private byte[] HandlePadding(byte[] source)
    {
        if (source.Length % 8 != 0 && _padding == "left")
        {
            var padding = new byte[8 - (source.Length % 8)];
            padding = padding.Select(_ => (byte)'0').ToArray();
            source = padding.Concat(source).ToArray();
        }
        else if (source.Length % 8 != 0 && _padding == "right")
        {
            var padding = new byte[8 - (source.Length % 8)];
            padding = padding.Select(_ => (byte)'0').ToArray();
            source = source.Concat(padding).ToArray();
        }

        return source;
    }

    private static List<byte> GetBytes(byte[] source)
    {
        var bytes = new List<byte>();
        var bits = new List<byte>();

        foreach (var bit in source)
        {
            bits.Add(bit == '0' ? (byte)0 : (byte)1);
            if (bits.Count == 8)
            {
                var byteValue = 0;
                for (var i = 0; i < 8; i++)
                {
                    byteValue += bits[i] == 0 ? 0 : (int)Math.Pow(2, 7 - i);
                }

                bytes.Add((byte)byteValue);
                bits.Clear();
            }
        }

        return bytes;
    }
}