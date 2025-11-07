using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace UtilityLayer
{
    public static class ImageReader
    {
        public static string GetMimeType(ImageFormat format)
        {
            if (format.Equals(ImageFormat.Jpeg))
                return "image/jpeg";
            else if (format.Equals(ImageFormat.Png))
                return "image/png";
            else if (format.Equals(ImageFormat.Gif))
                return "image/gif";
            else if (format.Equals(ImageFormat.Bmp))
                return "image/bmp";
            else if (format.Equals(ImageFormat.Icon))
                return "image/x-icon";
            else
                return "application/octet-stream";  // fallback mime
        }
    }
}
