
using System.Diagnostics;
using Mazes.Console.Infrastructure;
using Mazes.Core.Infrastructure;
using static System.Console;

namespace Mazes.Console.Runners;

public static class Local
{
    public static void Run(LocalOptions options)
    {
        Clear();

        PuzzleManager.Path = "Data/Puzzles.json";
        
        var puzzleNumber = options.PuzzleNumber;
        
        var maze = PuzzleManager.Instance.GetPuzzle(puzzleNumber);
        
        WriteLine($"Puzzle number: {puzzleNumber} ({maze.GridWidth}x{maze.GridHeight})");
        
        WriteLine();

        WriteLine(maze.ToString());
        
        var stopwatch = Stopwatch.StartNew();
        
        //var result = solver.Solve(puzzle);

        // CursorTop = puzzle.Height + 3;
        //
        // WriteLine(puzzle.ToString());
        //
        // WriteLine($"Solve state: {result}                 ");
        //
        // WriteLine($"Steps:       {_count:N0}              ");
        //         
        // WriteLine($@"Elapsed:     {_stopwatch.Elapsed:h\:mm\:ss\.fff}");

        WriteLine();
    }    
}