using AutomationApp.Domain.Common;

namespace AutomationApp.Domain.Interfaces
{
    public interface IScreenCaptureService
    {
        byte[] CaptureRegionAsBytes(ScreenRegion region);
    }
}