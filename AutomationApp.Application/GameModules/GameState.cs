namespace AutomationApp.Application.GameModules;

public class GameState
{
    public double CurrentMoney { get; set; }

    public double RebirthCost { get; set; }

    public TimeSpan TimeSinceLastRebirth { get; set; }
}