// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace Mazes.Core.Models;

public class Puzzle
{
    public int GridWidth { get; set; }
    
    public int GridHeight { get; set; }
    
    public Data Data { get; set; }
}