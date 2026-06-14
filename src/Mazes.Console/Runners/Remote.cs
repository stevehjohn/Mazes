using Mazes.Console.Infrastructure;
using Mazes.Core.Infrastructure;
using Mazes.Core.Models;
using static System.Console;

namespace Mazes.Console.Runners;

public static class Remote
{
    public static void Run(RemoteOptions options)
    {
        var validationResult = options.Validate();

        if (! validationResult.IsValid)
        {
            WriteLine($"\n{validationResult.Message}\n");

            return;
        }

        var client = new PuzzleClient();

        Clear();

        var startTime = DateTime.Now;
        WriteLine();

        WriteLine($"Fetching {options.Difficulty.ToString().ToLowerInvariant()} maze...");

        WriteLine();

        (DateOnly Date, Maze Maze)? puzzle = null;

        for (var retry = 1; retry < 21; retry++)
        {
            try
            {
                puzzle = options.Day > 0
                    ? client.GetPuzzle(options.Difficulty, new DateOnly(options.Year!.Value, options.Month!.Value, options.Day.Value))
                    : client.GetNextPuzzle(options.Difficulty);
            }
            catch
            {
                //
            }

            if (puzzle != null)
            {
                break;
            }

            var sleep = (int) Math.Pow(retry, 2);

            for (var timer = 0; timer < sleep; timer++)
            {
                if (retry > 1)
                {
                    CursorTop -= 2;
                }

                WriteLine($"Waiting for {sleep - timer:N0}s before attempt {retry}.  ");

                WriteLine();

                Thread.Sleep(1_000);

                CursorTop -= 2;

                WriteLine("Retrying...                         ");

                WriteLine();
            }
        }

        Clear();

        if (puzzle == null)
        {
            WriteLine("No puzzles available.\n");

            return;
        }

        var maze = puzzle.Value.Maze;

        WriteLine();

        WriteLine($@"Started: {startTime:F}, runtime: {DateTime.Now - startTime:h\:mm\:ss\.fff}.");

        WriteLine();

        WriteLine($"Solving {options.Difficulty.ToString().ToLowerInvariant()} puzzle for {puzzle.Value.Date:R} ({maze.Width}x{maze.Height}).");

        WriteLine();

        WriteLine(maze.ToString());
    }
}