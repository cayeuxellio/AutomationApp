namespace AutomationApp.Infrastructure.Vision;

using System.Drawing;

public class SimpleOcrService
{
    public string ReadText(byte[] imageBytes)
    {
        return $"Captured {imageBytes.Length} bytes";
    }
}