using System.Text;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Newtonsoft.Json.Linq;

namespace SoundShapesServer.Extensions;

public static class ByteArrayExtensions
{
    public static bool IsPng(this byte[] bytes)
    {
        if (bytes.Length > 7 &&
            bytes[0] == 137 &&
            bytes[1] == 80 &&
            bytes[2] == 78 &&
            bytes[3] == 71 &&
            bytes[4] == 13 &&
            bytes[5] == 10 &&
            bytes[6] == 26 &&
            bytes[7] == 10)
            return true;

        return false;
    }
}