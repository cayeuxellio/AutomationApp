namespace AutomationApp.Shared.DTOs;

public class ScenarioStepDto
{
    public string Type { get; set; } = string.Empty;

    public int? X { get; set; }
    public int? Y { get; set; }

    public int? Ms { get; set; }

    public string? Text { get; set; }
    
    public int? Count { get; set; }
    
    public List<ScenarioStepDto>? Actions { get; set; }
    public string? Condition { get; set; }
    public List<ScenarioStepDto>? Then { get; set; }
    public List<ScenarioStepDto>? Else { get; set; }
    
    public string? Template { get; set; }
    public double? Threshold { get; set; }
}