using System.Threading.Tasks;

namespace AutomationApp.Application.Services;

public interface IMinersHavenOcrService
{
    Task<decimal> GetCurrentMoneyAsync();
    Task<bool> IsRebirthButtonVisibleAsync();
    Task<string> GetRebirthRewardTextAsync();
}