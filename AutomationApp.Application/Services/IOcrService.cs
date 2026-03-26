namespace AutomationApp.Application.Services;

public interface IOcrService
{
    string ReadText(byte[] imageData);
}