using JetBrains.Annotations;

namespace Mazes.Core.Models;

[UsedImplicitly]
public class Data
{
    public int[] HorizontalWalls { get; set; }
    
    public int[] VerticalWalls { get; set; }
    
    public int StartingTile { get; set; }
    
    public int FinishTile { get; set; }
}