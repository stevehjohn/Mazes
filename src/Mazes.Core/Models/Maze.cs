using System.Text;

namespace Mazes.Core.Models;

public class Maze
{
    private readonly bool[] _maze;

    public int GridWidth { get; }

    public int GridHeight { get; }

    public int Width { get; }

    public int Height { get; }

    public int Right => Width - 1;

    public int Bottom => Height - 1;

    public Coordinate Start;

    public Coordinate End;

    public bool this[int x, int y]
    {
        get => _maze[y * Width + x];
        private init => _maze[y * Width + x] = value;
    }

    public Maze(Puzzle puzzle)
    {
        GridWidth = puzzle.GridWidth;

        GridHeight = puzzle.GridHeight;

        Width = GridWidth * 2 + 1;

        Height = GridHeight * 2 + 1;

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

        for (y = 0; y < GridHeight - 1; y++)
        {
            for (x = 0; x < GridWidth; x++)
            {
                var index = y * GridWidth + x;

                if (maze.HorizontalWalls[index] == 1)
                {
                    var mx = x * 2 + 1;

                    var my = y * 2 + 2;

                    this[mx - 1, my] = true;

                    this[mx, my] = true;

                    this[mx + 1, my] = true;
                }
            }
        }

        for (y = 0; y < GridHeight; y++)
        {
            for (x = 0; x < GridWidth - 1; x++)
            {
                var index = y * (GridWidth - 1) + x;

                if (maze.VerticalWalls[index] == 1)
                {
                    var mx = x * 2 + 2;

                    var my = y * 2 + 1;

                    this[mx, my - 1] = true;

                    this[mx, my] = true;

                    this[mx, my + 1] = true;
                }
            }
        }

        var startX = maze.StartingTile % GridWidth;

        var startY = maze.StartingTile / GridWidth;

        Start = new Coordinate(startX * 2 + 1, startY * 2 + 1);
        
        var endX = maze.FinishTile % GridWidth;
        
        var endY = maze.FinishTile / GridWidth;

        End = new Coordinate(endX * 2 + 1, endY * 2 + 1);
    }

    public override string ToString()
    {
        var builder = new StringBuilder();

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                if (x == Start.X && y == Start.Y)
                {
                    builder.Append('S');

                    continue;
                }

                if (x == End.X && y == End.Y)
                {
                    builder.Append('E');

                    continue;
                }

                builder.Append(this[x, y] ? '█' : ' ');
            }

            builder.AppendLine();
        }

        return builder.ToString();
    }
}