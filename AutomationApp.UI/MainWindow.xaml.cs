using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using AutomationApp.Application.Strategies;
using AutomationApp.Domain.Interfaces;
using AutomationApp.Infrastructure.Ocr;
using AutomationApp.Infrastructure.Services;

namespace AutomationApp.UI;

public partial class MainWindow : Window
{
    private readonly Dictionary<string, IGameStrategy> _gameStrategies;
    private IGameStrategy? _currentStrategy;  // ← Nullable
    private bool _isRunning;
    
    public MainWindow()
    {
        InitializeComponent();
        
        // Initialisation des services
        var tesseractPath = GetTesseractPath();
        IOcrService ocrService = new TesseractOcrService(tesseractPath);
        IScreenCaptureService screenCapture = new ScreenCaptureService();
        IImagePreprocessor imagePreprocessor = new ImagePreprocessor();
        
        // Enregistrement des stratégies
        _gameStrategies = new Dictionary<string, IGameStrategy>
        {
            ["Miners Haven"] = new MinersHavenStrategy(ocrService, screenCapture, imagePreprocessor)
        };
        
        GameComboBox.ItemsSource = _gameStrategies.Keys;
        if (_gameStrategies.Count > 0)
            GameComboBox.SelectedIndex = 0;
    }
    
    private string GetTesseractPath()
    {
        var localPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata");
        if (System.IO.Directory.Exists(localPath))
            return localPath;
            
        var systemPath = @"C:\Program Files\Tesseract-OCR\tessdata";
        if (System.IO.Directory.Exists(systemPath))
            return systemPath;
            
        throw new Exception("Tesseract data files not found");
    }
    
    private decimal GetMoneyThreshold()
    {
        if (decimal.TryParse(MoneyThresholdBox.Text, out var threshold))
            return threshold;
        return 1000000;
    }
    
    private async void StartButton_Click(object sender, RoutedEventArgs e)
    {
        // Vérifier qu'un jeu est sélectionné
        if (GameComboBox.SelectedItem == null)
        {
            StatusText.Text = "Please select a game first";
            return;
        }
        
        string? selectedGame = GameComboBox.SelectedItem.ToString();
        
        // Vérifier que la sélection est valide
        if (selectedGame != null && _gameStrategies.TryGetValue(selectedGame, out var strategy))
        {
            _currentStrategy = strategy;
            _isRunning = true;
            
            StartButton.IsEnabled = false;
            StopButton.IsEnabled = true;
            StatusText.Text = $"Running - Game: {selectedGame}";
            
            await RunMacroLoop();
        }
        else
        {
            StatusText.Text = "Invalid game selection";
        }
    }
    
    private void StopButton_Click(object sender, RoutedEventArgs e)
    {
        _isRunning = false;
        StartButton.IsEnabled = true;
        StopButton.IsEnabled = false;
        StatusText.Text = "Stopped";
    }
    
    private async Task RunMacroLoop()
    {
        while (_isRunning && _currentStrategy != null)
        {
            try
            {
                var threshold = GetMoneyThreshold();
                var currentMoney = await _currentStrategy.GetCurrentMoneyAsync();
                
                Dispatcher.Invoke(() =>
                {
                    MoneyText.Text = currentMoney >= 0 ? $"${currentMoney:N0}" : "Detecting...";
                });
                
                if (currentMoney >= threshold)
                {
                    StatusText.Text = $"Threshold reached! Performing rebirth...";
                    var success = await _currentStrategy.PerformRebirthAsync();
                    
                    if (success)
                        StatusText.Text = $"Rebirth completed at ${currentMoney:N0}!";
                    else
                        StatusText.Text = "Rebirth failed!";
                    
                    await Task.Delay(3000);
                }
                
                await Task.Delay(1000);
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    StatusText.Text = $"Error: {ex.Message}";
                });
                await Task.Delay(5000);
            }
        }
    }
}