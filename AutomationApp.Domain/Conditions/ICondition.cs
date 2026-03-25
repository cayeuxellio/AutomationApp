namespace AutomationApp.Domain.Conditions;
public interface ICondition
{
    Task<bool> EvaluateAsync(IServiceProvider serviceProvider);
}