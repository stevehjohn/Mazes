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
        var gridWidth = puzzle.GridWidth;

        var gridHeight = puzzle.GridHeight;

        Width = gridWidth * 2 + 1;

        Height = gridHeight * 2 + 1;

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

        for (y = 0; y < gridHeight - 1; y++)
        {
            for (x = 0; x < gridWidth; x++)
            {
                var index = y * gridWidth + x;

                if (maze.HorizontalWalls[index] == 1)
                {
                    var mx = x * 2 + 1;
                    var my = y * 2 + 2;

                    this[mx, my] = true;
                    this[mx + 1, my] = true;
                }
            }
        }

        for (y = 0; y < gridHeight; y++)
        {
            for (x = 0; x < gridWidth - 1; x++)
            {
                var index = y * (gridWidth - 1) + x;

                if (maze.VerticalWalls[index] == 1)
                {
                    var mx = x * 2 + 2;
                    var my = y * 2 + 1;

                    this[mx, my] = true;
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