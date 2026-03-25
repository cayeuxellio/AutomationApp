using AutomationApp.Domain.Vision;

namespace AutomationApp.Domain.Conditions;

public class TemplateCondition : ICondition
{
    private readonly string _template;
    private readonly double _threshold;

    public TemplateCondition(string template, double threshold)
    {
        _template = template;
        _threshold = threshold;
    }

    private readonly ITemplateMatcher _matcher;

    public TemplateCondition(string template, double threshold, ITemplateMatcher matcher)
    {
        _template = template;
        _threshold = threshold;
        _matcher = matcher;
    }

    public Task<bool> EvaluateAsync(IServiceProvider serviceProvider)
    {
        return _matcher.ExistsAsync(_template, _threshold);
    }
}