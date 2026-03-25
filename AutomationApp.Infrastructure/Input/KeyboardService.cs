using System.Windows.Forms;
using AutomationApp.Domain.Interfaces;

namespace AutomationApp.Infrastructure.Input;

public class KeyboardService : IKeyboardService
{
    public void TypeText(string text)
    {
        SendKeys.SendWait(text);
    }
}