using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using AutomationApp.Domain.Common;
using AutomationApp.Domain.Interfaces;

namespace AutomationApp.Infrastructure.Services
{
    public class ScreenCaptureService : IScreenCaptureService
    {
        public byte[] CaptureRegionAsBytes(ScreenRegion region)
        {
            using var bitmap = new Bitmap(region.Width, region.Height);
            using var graphics = Graphics.FromImage(bitmap);
            graphics.CopyFromScreen(region.X, region.Y, 0, 0, new Size(region.Width, region.Height));
            
            using var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }
    }
}