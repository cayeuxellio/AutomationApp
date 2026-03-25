using AutomationApp.Domain.Vision;
using OpenCvSharp;
using System.Drawing;
using System.Threading.Tasks;

namespace AutomationApp.Infrastructure.Vision;

public class OpenCvTemplateMatcher : ITemplateMatcher
{
    public Task<bool> ExistsAsync(string templatePath, double threshold)
    {
        // Take screenshot
        using var screenBmp = CaptureScreen();
        using var screenMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(screenBmp);

        using var template = Cv2.ImRead(templatePath);
        

        using var result = new Mat();
        Cv2.MatchTemplate(screenMat, template, result, TemplateMatchModes.CCoeffNormed);

        Cv2.MinMaxLoc(result, out _, out double maxVal, out _, out _);

        return Task.FromResult(maxVal >= threshold);
    }

    private Bitmap CaptureScreen()
    {
        var bounds = System.Windows.Forms.Screen.PrimaryScreen.Bounds;

        var bmp = new Bitmap(bounds.Width, bounds.Height);

        using var g = Graphics.FromImage(bmp);
        g.CopyFromScreen(0, 0, 0, 0, bmp.Size);

        return bmp;
    }
}