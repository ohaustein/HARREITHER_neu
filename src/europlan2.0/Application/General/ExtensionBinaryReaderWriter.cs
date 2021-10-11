using System;
using System.Collections.Generic;
using System.Linq;

public static class ExtensionBinaryReaderWriter
{


    private static String Decode(IEnumerable<Byte> bytes)
    {
        var byteArray = bytes.ToArray();
        var code = (Byte)85;
        for (var index = 0; index < byteArray.Length; index++)
        {
            byteArray[index] ^= code;
        }
        var text = System.Text.Encoding.UTF7.GetString(byteArray);
        return text;
    }

    private static IEnumerable<Byte> Encode(String text)
    {
        var byteArray = System.Text.Encoding.UTF7.GetBytes(text);
        var code = (Byte)85;
        for (var index = 0; index < byteArray.Length; index++)
        {
            byteArray[index] ^= code;
        }
        return byteArray.AsEnumerable();
    }

    public static String ReadDecodedString(this System.IO.BinaryReader binaryReader)
    {
        if (binaryReader == null) throw new ArgumentNullException("binaryReader");
        var byteBufferLength = binaryReader.ReadInt32();
        var byteBuffer = binaryReader.ReadBytes(byteBufferLength);
        var text = Decode(byteBuffer);
        return text;
    }

    public static void WriteEncodedString(this System.IO.BinaryWriter binaryWriter, String text)
    {
        if (binaryWriter == null) throw new ArgumentNullException("binaryWriter");
        var byteBuffer = Encode(text).ToArray();
        binaryWriter.Write(byteBuffer.Length);
        binaryWriter.Write(byteBuffer);
    }


}
