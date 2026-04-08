using System;
using System.Linq;
using System.Threading.Tasks;
using AutomationApp.Domain.Interfaces;
using AutomationApp.Domain.Common;

namespace AutomationApp.Application.Strategies
{
    public class MinersHavenStrategy : IGameStrategy
    {
        private readonly IOcrService _ocrService;
        private readonly IScreenCaptureService _screenCapture;
        private readonly IImagePreprocessor _imagePreprocessor;
        
        public string GameName => "Miners Haven";
        
        public ScreenRegion MoneyRegion => new(100, 50, 200, 40);
        public ScreenRegion RebirthButtonRegion => new(500, 300, 150, 40);
        
        public MinersHavenStrategy(
            IOcrService ocrService,
            IScreenCaptureService screenCapture,
            IImagePreprocessor imagePreprocessor)
        {
            _ocrService = ocrService;
            _screenCapture = screenCapture;
            _imagePreprocessor = imagePreprocessor;
        }
        
        public async Task<decimal> GetCurrentMoneyAsync()
        {
            try
            {
                var screenshot = _screenCapture.CaptureRegionAsBytes(MoneyRegion);
                var processed = _imagePreprocessor.Preprocess(screenshot);
                var text = await _ocrService.ReadTextFromRegionAsync(MoneyRegion);
                return ExtractMoneyFromText(text);
            }
            catch
            {
                return -1;
            }
        }
        
        public async Task<bool> IsRebirthAvailableAsync()
        {
            try
            {
                var text = await _ocrService.ReadTextFromRegionAsync(RebirthButtonRegion);
                return text.Contains("Rebirth", StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }
        
        public async Task<bool> PerformRebirthAsync()
        {
            if (!await IsRebirthAvailableAsync())
                return false;
            
            ClickOnRegion(RebirthButtonRegion);
            return true;
        }
        
        private decimal ExtractMoneyFromText(string text)
        {
            if (string.IsNullOrEmpty(text)) return -1;
            
            var cleaned = new string(text.Where(c => char.IsDigit(c) || c == ',' || c == '.').ToArray());
            cleaned = cleaned.Replace(",", ".");
            
            if (decimal.TryParse(cleaned, System.Globalization.NumberStyles.Any, 
                System.Globalization.CultureInfo.InvariantCulture, out decimal result))
                return result;
            
            return -1;
        }
        
        private void ClickOnRegion(ScreenRegion region)
        {
            var centerX = region.X + region.Width / 2;
            var centerY = region.Y + region.Height / 2;
            SetCursorPosition(centerX, centerY);
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }
        
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetCursorPosition(int x, int y);
        
        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;
    }
}