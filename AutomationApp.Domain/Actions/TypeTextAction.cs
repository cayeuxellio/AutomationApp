using AutomationApp.Domain.Interfaces;

namespace AutomationApp.Domain.Actions;

public class TypeTextAction : IAction
{
    public string Text { get; }

    public TypeTextAction(string text)
    {
        Text = text;
    }

    public Task ExecuteAsync(IServiceProvider serviceProvider)
    {
        var keyboard = (IKeyboardService)serviceProvider.GetService(typeof(IKeyboardService))!;
        keyboard.TypeText(Text);

        return Task.CompletedTask;
    }
}