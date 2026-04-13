using System;

namespace AutomationApp.Domain.Common
{
    public class ScreenRegion
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        
        public ScreenRegion(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
        
        public ScreenRegion()
        {
            X = 0;
            Y = 0;
            Width = 0;
            Height = 0;
        }
        
        public override string ToString()
        {
            return $"X={X}, Y={Y}, Width={Width}, Height={Height}";
        }
    }
}