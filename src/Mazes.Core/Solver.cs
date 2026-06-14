using Mazes.Core.Models;

namespace Mazes.Core;

public class Solver
{
    private readonly Maze _maze;

    public Solver(Maze maze)
    {
        _maze = maze;
    }

    public (List<(int X, int Y)> Path, List<(int X, int Y)> Visited) Solve()
    {
        var start = (_maze.Start.X, _maze.Start.Y);

        var end = (_maze.End.X, _maze.End.Y);

        var queue = new Queue<(int X, int Y, List<(int X, int Y)> History)>();

        var visited = new HashSet<(int X, int Y)>
        {
            start
        };

        queue.Enqueue((start.X, start.Y, []));

        while (queue.TryDequeue(out var node))
        {
            node.History.Add((node.X, node.Y));

            if ((node.X, node.Y) == end)
            {
                return (node.History, [..visited]);
            }

            foreach (var move in GetMoves(node.X, node.Y))
            {
                if (visited.Add(move))
                {
                    queue.Enqueue((move.X, move.Y, [.. node.History]));
                }
            }
        }

        return ([], [.. visited]);
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