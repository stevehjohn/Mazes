using Microsoft.Xna.Framework;

namespace Mazes.FrontEnd.Infrastructure;

public static class Constants
{
    public const int PathSize = 14;

    public const int WallSize = 1;

    public static Color BackgroundColour { get; } = Color.Black;

    public static Color WallColour { get; } = new(64, 64, 64);

    public static Color PathColour { get; } = new(0xFF, 0xB0, 0x00, 0xFF);

    public static Color StartColour { get; } = Color.Lime;

    public static Color FinishColour { get; } = Color.Fuchsia;
}
