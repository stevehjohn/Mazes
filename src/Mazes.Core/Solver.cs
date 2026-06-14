using Mazes.Core.Extensions;
using Mazes.Core.Models;
using Microsoft.VisualBasic;

namespace Mazes.Core;

public class Solver
{
    private readonly Maze _maze;
    
    public Solver(Maze maze)
    {
        _maze = maze;
    }

    public (List<(int X, int Y)> Path, List<(int X, int Y)> Visited) SolveMaze()
    {
        var queue = new Queue<(int X, int Y, List<(int X, int Y)> History)>();

        queue.Enqueue((_maze.Start.X, _maze.Start.Y, []));

        var visited = new List<(int X, int Y)>();

        while (queue.TryDequeue(out var node))
        {
            node.History.Add((node.X, node.Y));

            if (! visited.Contains((node.X, node.Y)))
            {
                visited.Add((node.X, node.Y));
            }

            if (node.X == _maze.End.X && node.Y == _maze.End.Y)
            {
                return (node.History, visited);
            }

            var moves = GetMoves(node.X, node.Y);

            moves.ForAll((_, m) =>
            {
                if (! node.History.Contains(m))
                {
                    queue.Enqueue((m.X, m.Y, [..node.History]));
                }
            });
        }

        return ([], visited);
    }

    private List<(int X, int Y)> GetMoves(int x, int y)
    {
        var moves = new List<(int, int)>();

        if (x > 0 && _maze[x - 1, y])
        {
            moves.Add((x - 1, y));
        }

        if (x < _maze.Width - 1 && _maze[x + 1, y])
        {
            moves.Add((x + 1, y));
        }

        if (y > 0 && _maze[x, y - 1])
        {
            moves.Add((x, y - 1));
        }

        if (y < _maze.Height - 1 && _maze[x, y + 1])
        {
            moves.Add((x, y + 1));
        }

        return moves;
    }
}