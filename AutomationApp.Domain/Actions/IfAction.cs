using AutomationApp.Domain.Conditions;

namespace AutomationApp.Domain.Actions;

public class IfAction : IAction
{
    private readonly ICondition _condition;
    private readonly List<IAction> _thenActions;
    private readonly List<IAction>? _elseActions;

    public IfAction(
        ICondition condition,
        List<IAction> thenActions,
        List<IAction>? elseActions)
    {
        _condition = condition;
        _thenActions = thenActions;
        _elseActions = elseActions;
    }

    public async Task ExecuteAsync(IServiceProvider serviceProvider)
    {
        var result = await _condition.EvaluateAsync(serviceProvider);

        var actionsToRun = result ? _thenActions : _elseActions;

        if (actionsToRun == null)
            return;

        foreach (var action in actionsToRun)
        {
            await action.ExecuteAsync(serviceProvider);
        }
    }
}