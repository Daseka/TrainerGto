using System.Runtime.InteropServices;
using System.Text;
using Poker.GameReader.ScreenUtilities;

namespace Poker.GameReader;

public partial class WindowHandles
{
    private delegate bool EnumWindowsProc(nint hWnd, nint lParam);

    [LibraryImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool EnumWindows(EnumWindowsProc lpEnumFunc, nint lParam);

    [LibraryImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool IsWindowVisible(nint hWnd);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetWindowRect(IntPtr hWnd, out Rect lpRect);

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString,int nMaxCount);

    public static List<nint> GetVisibleWindows()
    {
        List<nint> windows = [];
        _ = EnumWindows((hWnd, lParam) =>
        {
            if (IsWindowVisible(hWnd))
            {
                windows.Add(hWnd);
            }
            return true;
        }, nint.Zero);

        return windows;
    }

    internal static Rect GetWindowRect(nint window)
    {
        return GetWindowRect(window, out Rect rect) 
            ? rect 
            : Rect.Empty;
    }

    internal static string GetWindowTitle(nint window)
    {
        const int nChars = 256;
        var buff = new StringBuilder(nChars);

        return GetWindowText(window, buff, nChars) > 0
            ? buff.ToString()
            : string.Empty;
    }
}