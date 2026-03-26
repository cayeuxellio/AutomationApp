using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using AutomationApp.Application.Services;

namespace AutomationApp.Infrastructure.Services;
public class ScreenCaptureService : IScreenCaptureService
{
    public byte[] CaptureScreen()
    {
        var bounds = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
        return CaptureRegion(bounds.X, bounds.Y, bounds.Width, bounds.Height);
    }

    public byte[] CaptureRegion(int x, int y, int width, int height)
    {
        using var bitmap = new Bitmap(width, height);

        using (var g = Graphics.FromImage(bitmap))
        {
            g.CopyFromScreen(x, y, 0, 0, bitmap.Size);
        }

        using var ms = new MemoryStream();
        bitmap.Save(ms, ImageFormat.Png);

        return ms.ToArray();
    }
}