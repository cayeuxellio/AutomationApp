using System;
using AutomationApp.Application.Logging;
using AutomationApp.Application.GameModules;
namespace AutomationApp.Application.Automation;

public class AutomationEngine : IAutomationEngine
{
    private readonly IGameModule _gameModule;
    private readonly ILogger _logger;

    private CancellationTokenSource _cts;
    private Task _runningTask;

    public bool IsRunning => _cts != null && !_cts.IsCancellationRequested;

    public AutomationEngine(IGameModule gameModule, ILogger logger)
    {
        _gameModule = gameModule;
        _logger = logger;
    }

    public Task StartAsync()
    {
        if (IsRunning)
            return Task.CompletedTask;

        _cts = new CancellationTokenSource();

        _runningTask = Task.Run(() => RunLoopAsync(_cts.Token));

        return Task.CompletedTask;
    }

    public async Task StopAsync()
    {
        if (!IsRunning)
            return;

        _cts.Cancel();

        try
        {
            await _runningTask;
        }
        catch (OperationCanceledException)
        {
            // expected
        }

        _cts.Dispose();
        _cts = null;
    }

    private async Task RunLoopAsync(CancellationToken token)
    {
        _logger.Log("Automation started");

        await _gameModule.InitializeAsync();

        while (!token.IsCancellationRequested)
        {
            try
            {
                var state = await _gameModule.ReadStateAsync();

                await _gameModule.ExecuteAsync(state);
            }
            catch (Exception ex)
            {
                _logger.Log($"Error: {ex.Message}");
            }

            await Task.Delay(1000, token); // control CPU usage
        }

        _logger.Log("Automation stopped");
    }
}