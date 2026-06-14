using System.Text;

namespace Mazes.Core.Models;

public class Maze
{
    private readonly bool[] _maze;
    
    public int Width { get; }
    
    public int Height { get; }
    
    public bool this[int x, int y]
    {
        get => _maze[y * Width + x];
        set => _maze[y * Width + x] = value;
    }

    public Maze(Puzzle puzzle)
    {
        Width = puzzle.GridWidth * 2 + 1;

        Height = puzzle.GridHeight * 2 + 1;
        
        _maze = new bool[Height * Width];
        
        Array.Fill(_maze, true);

        int x = 1, y = 2;

        for (var i = 0; i < puzzle.Data.HorizontalWalls.Length; i++)
        {
            this[x, y] =  puzzle.Data.HorizontalWalls[i] == 1;

            x += 2;

            if (x >= Width)
            {
                x = 1;
                
                y += 2;
            }
        }
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                builder.Append(_maze[y * Width + x] ? '█': ' ');
            }
            
            builder.AppendLine();
        }

        return builder.ToString();
    }
}