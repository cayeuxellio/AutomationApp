using System;
using System.Drawing;
using System.Windows;
using AutomationApp.Application.Strategies;
using AutomationApp.Application.Validation;
using AutomationApp.Application.Factories;
using AutomationApp.Infrastructure.Ocr;
using Microsoft.Extensions.DependencyInjection;
using AutomationApp.Application.UseCases;
using AutomationApp.Domain.Interfaces;
using AutomationApp.Infrastructure.Input;
using AutomationApp.Infrastructure.Services;



namespace AutomationApp.UI;

public partial class App : System.Windows.Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        var services = new ServiceCollection();
        var moneyRegion = new Rectangle(x: 100, y: 50, width: 200, height: 40);
        var rebirthButtonRegion = new Rectangle(x: 500, y: 300, width: 150, height: 40);
        var tesseractDataPath = @"C:\Program Files\Tesseract-OCR\tessdata";

        // Application
        services.AddScoped<RunAutomationUseCase>();
        services.AddSingleton<ActionFactory>();
        services.AddSingleton<ScenarioValidator>();
        services.AddSingleton<IOcrService>(sp => 
            new TesseractOcrService(tesseractDataPath));

        services.AddSingleton<MinersHavenStrategy>();
        

        services.AddSingleton<IGameStrategy>(sp => sp.GetRequiredService<MinersHavenStrategy>());

        // Infrastructure
        
        services.AddSingleton<IMouseService, MouseService>();
        services.AddSingleton<IKeyboardService, KeyboardService>();

        Services = services.BuildServiceProvider();

        base.OnStartup(e);
    }
}

