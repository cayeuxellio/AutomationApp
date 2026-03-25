using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AutomationApp.Domain.Interfaces;

namespace AutomationApp.Infrastructure.Input;

public class MouseService : IMouseService
{
    [DllImport("user32.dll")]
    private static extern void mouse_event(int flags, int dx, int dy, int data, int extraInfo);

    private const int LEFTDOWN = 0x02;
    private const int LEFTUP = 0x04;

    public void Click(int x, int y)
    {
        Cursor.Position = new Point(x, y);
        mouse_event(LEFTDOWN, x, y, 0, 0);
        mouse_event(LEFTUP, x, y, 0, 0);
    }
}