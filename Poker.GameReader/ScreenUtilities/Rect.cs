using System.Runtime.InteropServices;

namespace Poker.GameReader.ScreenUtilities;

[StructLayout(LayoutKind.Sequential)]
internal struct Rect
{
    public int Left;
    public int Top;
    public int Right;
    public int Bottom;

    public static Rect Empty => new();
}
