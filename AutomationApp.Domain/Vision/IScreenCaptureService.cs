using System.Drawing;

namespace AutomationApp.Domain.Vision;
public interface IScreenCaptureService
{
    byte[] CaptureRegion(ScreenRegion region);
}