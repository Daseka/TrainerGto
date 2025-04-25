using System.Drawing;

namespace Poker.GameReader.ScreenUtilities
{
    public interface IScreenGrabber
    {
        Bitmap GrabScreenBlock(int sourceX, int sourceY, int width, int height);
        void GrabScreenBlockAndSave(int sourceX, int sourceY, int width, int height, string filePath);
    }
}