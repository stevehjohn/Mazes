namespace Mazes.Core.Models;

public class Maze
{
    private readonly bool[] _maze;
    
    public int Width { get; }
    
    public int Height { get; }
    
    public bool this[int x, int y] => _maze[y * Width + x];
    
    public Maze(Puzzle puzzle)
    {
        Width = puzzle.GridWidth * 2 + 1;

        Height = puzzle.GridHeight * 2 + 1;
        
        _maze = new bool[Height * Width];
        
        Array.Fill(_maze, true);
    }

    public override string ToString()
    {
        return string.Empty;
    }
}