using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

public static class ImagePreprocessor
{
    public static byte[] Preprocess(byte[] input)
    {
        using var ms = new MemoryStream(input);
        using var bitmap = new Bitmap(ms);

        // (for now, no processing)
        
        using var output = new MemoryStream();
        bitmap.Save(output, ImageFormat.Png);

        return output.ToArray();
    }
}