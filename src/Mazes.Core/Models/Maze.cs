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

        int x, y;

        for (x = 0; x < Width; x++)
        {
            this[x, 0] = true;

            this[x, Bottom] = true;
        }

        for (y = 0; y < Height; y++)
        {
            this[0, y] = true;

            this[Right, y] = true;
        }

        for (y = 0; y < puzzle.GridHeight - 1; y++)
        {
            for (x = 0; x < puzzle.GridWidth; x++)
            {
                if (maze.HorizontalWalls[y * puzzle.GridWidth + x] == 1)
                {
                    this[x * 2 + 1, y * 2 + 2] = true;

                    this[x * 2 + 2, y * 2 + 2] = true;
                }
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
                builder.Append(this[x, y] ? '█' : ' ');
            }

            builder.AppendLine();
        }

        return builder.ToString();
    }
}