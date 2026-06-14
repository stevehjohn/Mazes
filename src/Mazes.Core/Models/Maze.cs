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

        var i = 0;

        for (var x = 0; x <= puzzle.GridHeight; x++)
        {
            for (var y = 0; y < puzzle.GridWidth; y++)
            {
                this[y * 2 + 1, x * 2] = puzzle.Data.HorizontalWalls[i++] == 1;
            }
        }

        i = 0;

        for (var x = 0; x < puzzle.GridHeight; x++)
        {
            for (var y = 0; y <= puzzle.GridWidth; y++)
            {
                this[y * 2, x * 2 + 1] = puzzle.Data.VerticalWalls[i++] == 1;
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