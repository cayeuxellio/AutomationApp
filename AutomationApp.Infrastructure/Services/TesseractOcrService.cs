using System;
using Tesseract;
using System.Drawing;
using System.IO;
using AutomationApp.Application.Services;

namespace AutomationApp.Infrastructure.Services;

public class TesseractOcrService : IOcrService
{
    private readonly TesseractEngine _engine;

    public TesseractOcrService()
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var tessPath = Path.Combine(basePath, "tessdata");

        _engine = new TesseractEngine(tessPath, "eng", EngineMode.Default);
    }

    public string ReadText(byte[] imageData)
    {
        using var ms = new MemoryStream(imageData);
        using var bitmap = new Bitmap(ms);
        using var pix = PixConverter.ToPix(bitmap);
        using var page = _engine.Process(pix);

        return page.GetText();
    }
}