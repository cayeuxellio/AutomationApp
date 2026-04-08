// AutomationApp.Domain/Interfaces/IOcrService.cs
using System.Threading.Tasks;
using AutomationApp.Domain.Common;

namespace AutomationApp.Domain.Interfaces;

public interface IOcrService
{
    Task<string> ReadTextFromRegionAsync(ScreenRegion region);
    Task<decimal> ReadNumberFromRegionAsync(ScreenRegion region);
}