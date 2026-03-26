
namespace AutomationApp.Application.Services;

public interface IScreenCaptureService
{
    byte[] CaptureScreen();
    byte[] CaptureRegion(int x, int y, int width, int height);
}