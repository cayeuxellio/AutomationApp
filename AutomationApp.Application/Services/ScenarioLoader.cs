using System.Text.Json;
using AutomationApp.Shared.DTOs;
using AutomationApp.Domain.Actions;
using AutomationApp.Application.Factories;
using AutomationApp.Application.Validation;

namespace AutomationApp.Application.Services;

public class ScenarioLoader
{
    private readonly ActionFactory _factory;
    private readonly ScenarioValidator _validator;

    public ScenarioLoader(ActionFactory factory, ScenarioValidator validator)
    {
        _factory = factory;
        _validator = validator;
    }

    public List<IAction> LoadFromJson(string json)
    {
        var steps = JsonSerializer.Deserialize<List<ScenarioStepDto>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        var validation = _validator.Validate(steps);

        if (!validation.IsValid)
        {
            var message = string.Join("\n", validation.Errors);
            throw new Exception($"Scenario validation failed:\n{message}");
        }

        return steps!.Select(_factory.Create).ToList();
    }
}