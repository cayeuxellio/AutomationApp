using System;
using System.Drawing;
using System.Threading.Tasks;
using AutomationApp.Application.GameModules;
using AutomationApp.Application.Services;
using AutomationApp.Infrastructure.Services;

namespace AutomationApp.Infrastructure.GameModules;

public class MinersHavenModule : IGameModule
{
    private readonly IScreenCaptureService _screen;
    private readonly IOcrService _ocr;

    public string Name => "Miners Haven";

    public MinersHavenModule(
        IScreenCaptureService screen,
        IOcrService ocr)
    {
        _screen = screen;
        _ocr = ocr;
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task<GameState> ReadStateAsync()
    {
        //var region = new Rectangle(100, 100, 300, 100); // TEMP

        var image = _screen.CaptureRegion(100, 100, 300, 100);

        var processed = ImagePreprocessor.Preprocess(image);

        var text = _ocr.ReadText(processed);

        var money = ParseMoney(text);

        return Task.FromResult(new GameState
        {
            CurrentMoney = money,
            RebirthCost = 1000 // temp
        });
    }

    public Task ExecuteAsync(GameState state)
    {
        if (state.CurrentMoney >= state.RebirthCost)
        {
            Console.WriteLine("Rebirth triggered!");
        }

        return Task.CompletedTask;
    }

    private double ParseMoney(string text)
    {
        // VERY basic for now
        text = text.Replace("$", "").Trim();

        if (double.TryParse(text, out var value))
            return value;

        return 0;
    }
}