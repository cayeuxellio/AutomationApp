namespace AutomationApp.Domain.Actions;

public class WaitAction : IAction
{
    public int Milliseconds { get; }

    public WaitAction(int milliseconds)
    {
        Milliseconds = milliseconds;
    }

    public async Task ExecuteAsync(IServiceProvider serviceProvider)
    {
        await Task.Delay(Milliseconds);
    }
}