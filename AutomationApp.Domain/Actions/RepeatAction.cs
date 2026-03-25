namespace AutomationApp.Domain.Actions;

public class RepeatAction : IAction
{
    private readonly int _count;
    private readonly List<IAction> _actions;

    public RepeatAction(int count, List<IAction> actions)
    {
        _count = count;
        _actions = actions;
    }

    public async Task ExecuteAsync(IServiceProvider serviceProvider)
    {
        for (int i = 0; i < _count; i++)
        {
            foreach (var action in _actions)
            {
                await action.ExecuteAsync(serviceProvider);
            }
        }
    }
}