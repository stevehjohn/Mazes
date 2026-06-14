using System.Text;

namespace Mazes.Core.Models;

public class Maze
{
    private readonly bool[] _maze;

    public int Width { get; }

    public int Height { get; }
    
    public int Right => Width - 1;
    
    public int Bottom => Height - 1;

    public bool this[int x, int y]
    {
        get => _maze[y * Width + x];
        init => _maze[y * Width + x] = value;
    }

    public Maze(Puzzle puzzle)
    {
        Width = puzzle.GridWidth * 2 + 1;

        Height = puzzle.GridHeight * 2 + 1;

        _maze = new bool[Height * Width];

        var maze = puzzle.Data;

        int x = 1, y = 2;

        for (var i = 0; i < maze.HorizontalWalls.Length; i++)
        {
            this[x++, y] = maze.HorizontalWalls[i] == 1;

            this[x++, y] = maze.HorizontalWalls[i] == 1;
            
            if (x >= Width)
            {
                x = 1;

                y += 2;
            }
        }

        x = 2;

        y = 3;

        for (var i = 0; i < maze.VerticalWalls.Length; i++)
        {
            this[x, y] = maze.VerticalWalls[i] == 1;
        
            this[x, y + 1] = maze.VerticalWalls[i] == 1;
        
            x += 2;
        
            if (x >= Width)
            {
                x = 2;
        
                y += 2;
            }
        }

        for (x = 0; x < Width; x++)
        {
            this[x, 0] = true;
            
            this[x, Bottom] = true;
        }

        for (y = 0; y < Width; y++)
        {
            this[0, y] = true;
            
            this[Right, y] = true;
        }
    }

    public override string ToString()
    {
        var builder = new StringBuilder();

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                builder.Append(this[x, y] ? '█' : ' ');
            }

            builder.AppendLine();
        }

        return builder.ToString();
    }
}