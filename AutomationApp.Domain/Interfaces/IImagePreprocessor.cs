namespace AutomationApp.Domain.Interfaces
{
    public interface IImagePreprocessor
    {
        byte[] Preprocess(byte[] imageData);
    }
}