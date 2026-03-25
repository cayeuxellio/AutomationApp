using AutomationApp.Domain.Actions;

namespace AutomationApp.Application.UseCases;

public class RunAutomationUseCase
{
    private readonly IServiceProvider _serviceProvider;

    public RunAutomationUseCase(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task ExecuteAsync(IEnumerable<IAction> actions)
    {
        foreach (var action in actions)
        {
            await action.ExecuteAsync(_serviceProvider);
        }
    }
}