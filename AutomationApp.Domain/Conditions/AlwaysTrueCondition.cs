namespace AutomationApp.Domain.Conditions;

public class AlwaysTrueCondition : ICondition
{
    public Task<bool> EvaluateAsync(IServiceProvider serviceProvider)
    {
        return Task.FromResult(true);
    }
}