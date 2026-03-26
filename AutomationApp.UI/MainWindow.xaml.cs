using System.Windows;
using Microsoft.Win32;
using AutomationApp.Application.Services;
using AutomationApp.Application.UseCases;
using AutomationApp.Application.Automation;
using AutomationApp.Infrastructure.GameModules;
using AutomationApp.Infrastructure.Logging;
using AutomationApp.Domain.Actions;
using Microsoft.Extensions.DependencyInjection;
using AutomationApp.Domain.Vision;
using AutomationApp.Infrastructure.Services;
using AutomationApp.Infrastructure.Vision;

namespace AutomationApp.UI;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var gameModule = new MinersHavenModule(
            new ScreenCaptureService(),
            new TesseractOcrService());
        var logger = new ConsoleLogger();

        _engine = new AutomationEngine(gameModule, logger);
    }

    private async void OnRunClicked(object sender, RoutedEventArgs e)
    {
        var loader = App.Services.GetRequiredService<ScenarioLoader>();
        var useCase = App.Services.GetRequiredService<RunAutomationUseCase>();

        var json = """
                   [
                       { "type": "click", "x": 500, "y": 500 },
                       { "type": "wait", "ms": 1000 },
                       { "type": "type", "text": "Hello from JSON!" }
                   ]
                   """;

        var actions = loader.LoadFromJson(json);
        await useCase.ExecuteAsync(actions);
    }

    private async void OnLoadFromFileClicked(object sender, RoutedEventArgs e)
    {
        try
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json"
            };

            if (dialog.ShowDialog() == true)
            {
                var json = System.IO.File.ReadAllText(dialog.FileName);

                var loader = App.Services.GetRequiredService<ScenarioLoader>();
                var useCase = App.Services.GetRequiredService<RunAutomationUseCase>();

                var actions = loader.LoadFromJson(json);

                await useCase.ExecuteAsync(actions);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString()); // IMPORTANT: ToString(), not Message
        }
    }
    
    private IAutomationEngine _engine;
    
    private async void Start_Click(object sender, RoutedEventArgs e)
    {
        await _engine.StartAsync();
    }

    private async void Stop_Click(object sender, RoutedEventArgs e)
    {
        await _engine.StopAsync();
    }
}