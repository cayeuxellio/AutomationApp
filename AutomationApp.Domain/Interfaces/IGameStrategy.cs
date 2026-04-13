// AutomationApp.Domain/Interfaces/IGameStrategy.cs
using System.Drawing;
using System.Threading.Tasks;
using AutomationApp.Domain.Common;

namespace AutomationApp.Domain.Interfaces;

public interface IGameStrategy
{
    string GameName { get; }
    ScreenRegion MoneyRegion { get; }
    ScreenRegion RebirthButtonRegion { get; }
        
    Task<decimal> GetCurrentMoneyAsync();
    Task<bool> IsRebirthAvailableAsync();
    Task<bool> PerformRebirthAsync();
}