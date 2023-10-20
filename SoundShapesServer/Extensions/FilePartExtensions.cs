using HttpMultipartParser;
using SoundShapesServer.Types;

namespace SoundShapesServer.Extensions;

public static class FilePartExtensions
{
    public static byte[] ToByteArray(this FilePart filePart)
    {
        MemoryStream memoryStream = new();
        filePart.Data.CopyTo(memoryStream);
        byte[] bytes = memoryStream.ToArray();

        return bytes;
    }
    public static FileType SSFileType(this FilePart file)
    {
        if (file.ContentType == "image/png") 
            return FileType.Image;
        // ReSharper disable StringLiteralTypo
        if (file.ContentType == "application/vnd.soundshapes.level") 
            return FileType.Level;
        if (file.ContentType == "application/vnd.soundshapes.sound") 
            return FileType.Sound;
        // ReSharper restore StringLiteralTypo
            
        return FileType.Unknown;
    }
}