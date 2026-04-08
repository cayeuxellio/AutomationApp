using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using AutomationApp.Domain.Common;
using AutomationApp.Domain.Interfaces;
using Tesseract;

namespace AutomationApp.Infrastructure.Ocr
{
    public class TesseractOcrService : IOcrService
    {
        private readonly string _tesseractDataPath;
        
        public TesseractOcrService(string tesseractDataPath)
        {
            _tesseractDataPath = tesseractDataPath;
        }
        
        public async Task<string> ReadTextFromRegionAsync(ScreenRegion region)
        {
            return await Task.Run(() =>
            {
                using var engine = new TesseractEngine(_tesseractDataPath, "eng", EngineMode.Default);
                using var bitmap = CaptureRegion(region);
                using var pix = PixConverter.ToPix(bitmap);
                using var page = engine.Process(pix);
                return page.GetText().Trim();
            });
        }
        
        public async Task<decimal> ReadNumberFromRegionAsync(ScreenRegion region)
        {
            var text = await ReadTextFromRegionAsync(region);
            
            // Extraire le premier nombre trouvé
            if (decimal.TryParse(text, System.Globalization.NumberStyles.Any, 
                    System.Globalization.CultureInfo.InvariantCulture, out decimal result))
            {
                return result;
            }
            
            return -1;
        }
        
        private Bitmap CaptureRegion(ScreenRegion region)
        {
            var bitmap = new Bitmap(region.Width, region.Height);
            using var graphics = Graphics.FromImage(bitmap);
            graphics.CopyFromScreen(region.X, region.Y, 0, 0, new Size(region.Width, region.Height));
            return bitmap;
        }
    }
}