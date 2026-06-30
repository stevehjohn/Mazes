using Microsoft.Xna.Framework;

namespace Mazes.FrontEnd.Infrastructure;

public static class Constants
{
    public const int TileSize = 14;

    public static Color PathColor { get; } = new(0xFF, 0xB0, 0x00, 0xFF);

    public static Color StartColor { get; } = Color.Lime;

    public static Color FinishColor { get; } = Color.Fuchsia;
}