namespace AutomationApp.Application.GameModules;

public interface IGameModule
{
    string Name { get; }

    Task InitializeAsync();

    Task<GameState> ReadStateAsync();

    Task ExecuteAsync(GameState state);
}