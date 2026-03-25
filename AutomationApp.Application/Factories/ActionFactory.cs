using AutomationApp.Domain.Actions;
using AutomationApp.Shared.DTOs;
using AutomationApp.Domain.Conditions;
using AutomationApp.Domain.Vision;

namespace AutomationApp.Application.Factories;

public class ActionFactory
{
    private readonly ITemplateMatcher _matcher;

    public ActionFactory(ITemplateMatcher matcher)
    {
        _matcher = matcher;
    }
    public IAction Create(ScenarioStepDto step)
    {
        if (string.IsNullOrWhiteSpace(step.Type))
            throw new Exception("Action type is missing.");

        switch (step.Type.ToLower())
        {
            case "click":
                return new ClickAction(step.X!.Value, step.Y!.Value);

            case "wait":
                return new WaitAction(step.Ms!.Value);

            case "type":
                return new TypeTextAction(step.Text!);

            case "repeat":
                if (step.Count == null || step.Actions == null)
                    throw new Exception("Repeat requires 'count' and 'actions'.");

                var nested = step.Actions.Select(Create).ToList(); // 🔥 recursion

                return new RepeatAction(step.Count.Value, nested);
            
            case "if":
                if (string.IsNullOrWhiteSpace(step.Condition) || step.Then == null)
                    throw new Exception("If requires 'condition' and 'then'.");

                var condition = CreateCondition(step);

                var thenActions = step.Then.Select(Create).ToList();
                var elseActions = step.Else?.Select(Create).ToList();

                return new IfAction(condition, thenActions, elseActions);

            default:
                throw new Exception($"Unknown action type: {step.Type}");
        }
    }
    private ICondition CreateCondition(ScenarioStepDto step)
    {
        switch (step.Condition?.ToLower())
        {
            case "always":
                return new AlwaysTrueCondition();

            case "template":
                if (string.IsNullOrWhiteSpace(step.Template))
                    throw new Exception("Template condition requires 'template'.");

                var threshold = step.Threshold ?? 0.8;

                return new TemplateCondition(step.Template, threshold, _matcher);

            default:
                throw new Exception($"Unknown condition: {step.Condition}");
        }
    }
}