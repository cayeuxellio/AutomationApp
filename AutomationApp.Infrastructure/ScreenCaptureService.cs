using AutomationApp.Domain.Vision;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace AutomationApp.Infrastructure.Vision;
public class ScreenCaptureService : IScreenCaptureService
{
    public byte[] CaptureRegion(ScreenRegion region)
    {
        using var bmp = new Bitmap(region.Width, region.Height);

        using var g = Graphics.FromImage(bmp);
        g.CopyFromScreen(region.X, region.Y, 0, 0, bmp.Size);

        using var ms = new MemoryStream();
        bmp.Save(ms, ImageFormat.Png);

        return ms.ToArray();
    }
}