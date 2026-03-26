using System;
using AutomationApp.Application.Logging;
namespace AutomationApp.Infrastructure.Logging;

public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");
    }
}