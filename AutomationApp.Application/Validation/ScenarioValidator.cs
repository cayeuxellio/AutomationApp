using AutomationApp.Shared.DTOs;

namespace AutomationApp.Application.Validation;

public class ScenarioValidator
{
    public ValidationResult Validate(List<ScenarioStepDto>? steps)
    {
        var result = new ValidationResult();

        if (steps == null || steps.Count == 0)
        {
            result.Errors.Add("Scenario is empty or invalid JSON.");
            return result;
        }

        for (int i = 0; i < steps.Count; i++)
        {
            var step = steps[i];
            var prefix = $"Step {i + 1}";

            if (string.IsNullOrWhiteSpace(step.Type))
            {
                result.Errors.Add($"{prefix}: Missing 'type'.");
                continue;
            }

            switch (step.Type.ToLower())
            {
                case "click":
                    if (step.X == null || step.Y == null)
                        result.Errors.Add($"{prefix}: Click requires 'x' and 'y'.");
                    break;

                case "wait":
                    if (step.Ms == null || step.Ms <= 0)
                        result.Errors.Add($"{prefix}: Wait requires 'ms' > 0.");
                    break;

                case "type":
                    if (string.IsNullOrWhiteSpace(step.Text))
                        result.Errors.Add($"{prefix}: Type requires 'text'.");
                    break;
                
                case "repeat":
                    if (step.Count == null || step.Count <= 0)
                        result.Errors.Add($"{prefix}: Repeat requires 'count' > 0.");

                    if (step.Actions == null || step.Actions.Count == 0)
                        result.Errors.Add($"{prefix}: Repeat requires non-empty 'actions'.");
                    else
                    {
                        // 🔥 recursive validation
                        var nestedResult = Validate(step.Actions);
                        foreach (var err in nestedResult.Errors)
                        {
                            result.Errors.Add($"{prefix} -> {err}");
                        }
                    }
                    break;
                
                case "if":
                    if (step.Condition == "template")
                    {
                        if (string.IsNullOrWhiteSpace(step.Template))
                            result.Errors.Add($"{prefix}: Template condition requires 'template'.");

                        if (step.Threshold != null && (step.Threshold < 0 || step.Threshold > 1))
                            result.Errors.Add($"{prefix}: Threshold must be between 0 and 1.");
                    }
                    if (string.IsNullOrWhiteSpace(step.Condition))
                        result.Errors.Add($"{prefix}: If requires 'condition'.");

                    if (step.Then == null || step.Then.Count == 0)
                        result.Errors.Add($"{prefix}: If requires non-empty 'then'.");

                    if (step.Then != null)
                    {
                        var thenValidation = Validate(step.Then);
                        foreach (var err in thenValidation.Errors)
                            result.Errors.Add($"{prefix} -> THEN -> {err}");
                    }

                    if (step.Else != null)
                    {
                        var elseValidation = Validate(step.Else);
                        foreach (var err in elseValidation.Errors)
                            result.Errors.Add($"{prefix} -> ELSE -> {err}");
                    }

                    break;
                

                default:
                    result.Errors.Add($"{prefix}: Unknown type '{step.Type}'.");
                    break;
            }
        }

        return result;
    }
}