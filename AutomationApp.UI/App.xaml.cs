using System;
using System.Windows;
using AutomationApp.Application.Validation;
using AutomationApp.Application.Factories;
using AutomationApp.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using AutomationApp.Application.UseCases;
using AutomationApp.Domain.Interfaces;
using AutomationApp.Infrastructure.Input;
using AutomationApp.Infrastructure.Vision;
using AutomationApp.Domain.Vision;

namespace AutomationApp.UI;

public partial class App : System.Windows.Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        var services = new ServiceCollection();

        // Application
        services.AddScoped<RunAutomationUseCase>();
        services.AddSingleton<ActionFactory>();
        services.AddSingleton<ScenarioLoader>();
        services.AddSingleton<ScenarioValidator>();

        // Infrastructure
        services.AddSingleton<IScreenCaptureService, ScreenCaptureService>();
        services.AddSingleton<SimpleOcrService>();
        services.AddSingleton<ITemplateMatcher, OpenCvTemplateMatcher>();
        services.AddSingleton<IMouseService, MouseService>();
        services.AddSingleton<IKeyboardService, KeyboardService>();

        Services = services.BuildServiceProvider();

        base.OnStartup(e);
    }
}
