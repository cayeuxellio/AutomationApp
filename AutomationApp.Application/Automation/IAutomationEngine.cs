namespace AutomationApp.Application.Automation;

public interface IAutomationEngine
{
    Task StartAsync();
    Task StopAsync();
    bool IsRunning { get; }
}