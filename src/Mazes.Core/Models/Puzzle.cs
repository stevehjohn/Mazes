namespace Mazes.Core.Models;

public class Puzzle
{
    public int GridWidth { get; set; }
    
    public int GridHeight { get; set; }
    
    public Data Data { get; set; }
    
    public Source Source { get; set; }
}