namespace AutomationApp.Domain.Actions;

public interface IAction
{
    Task ExecuteAsync(IServiceProvider serviceProvider);
}