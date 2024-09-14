using System.Drawing;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace Poker.GameReader.ScreenUtilities;

internal class ScreenGrabber
{
    public Bitmap GrabScreenBlock(int sourceX, int sourceY, int width, int height)
    {
        // Create a new bitmap with the size of the screen
        var bitmap = new Bitmap(width, height);
        // Create a graphics object from the bitmap
        using (var g = Graphics.FromImage(bitmap))
        {
            // Capture the screen
            g.CopyFromScreen(sourceX, sourceY, 0, 0, bitmap.Size);
        }

        return bitmap;
    }

    public void GrabScreenBlockAndSave(int sourceX, int sourceY, int width, int height, string filePath)
    {
        // Create a new bitmap with the size of the screen
        using var bitmap = new Bitmap(width, height);
        // Create a graphics object from the bitmap
        using (var g = Graphics.FromImage(bitmap))
        {
            // Capture the screen
            g.CopyFromScreen(sourceX, sourceY, 0, 0, bitmap.Size);
        }

        // Save the bitmap to a file
        bitmap.Save(filePath, ImageFormat.Png);

        Console.WriteLine($"Screenshot saved to {filePath}");
    }
}
