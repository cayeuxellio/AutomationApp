using AutomationApp.Domain.Interfaces;

namespace AutomationApp.Domain.Actions;

public class ClickAction : IAction
{
    public int X { get; }
    public int Y { get; }

    public ClickAction(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Task ExecuteAsync(IServiceProvider serviceProvider)
    {
        var mouse = (IMouseService)serviceProvider.GetService(typeof(IMouseService))!;
        mouse.Click(X, Y);

        return Task.CompletedTask;
    }
}