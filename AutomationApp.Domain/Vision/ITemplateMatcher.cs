namespace AutomationApp.Domain.Vision;

public interface ITemplateMatcher
{
    Task<bool> ExistsAsync(string templatePath, double threshold);
}
