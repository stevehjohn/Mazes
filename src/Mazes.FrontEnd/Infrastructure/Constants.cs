using Microsoft.Xna.Framework;

namespace Mazes.FrontEnd.Infrastructure;

public static class Constants
{
    public const int PathSize = 15;

    public const int WallSize = 1;

    public const int BorderSize = 10;

    public const int LineThickness = 1;

    public static Color BackgroundColour { get; } = Color.Black;

    public static Color WallColour { get; } = new(0xE0, 0xE0, 0xE0, 0xFF);

    public static Color PathColour { get; } = new(0xFF, 0xB0, 0x00, 0xFF);

    public static Color StartColour { get; } = Color.Lime;

    public static Color FinishColour { get; } = Color.Fuchsia;
}
