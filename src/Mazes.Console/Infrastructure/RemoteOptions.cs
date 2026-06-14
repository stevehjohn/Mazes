// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

using CommandLine;
using JetBrains.Annotations;
using Mazes.Core.Models;

namespace Mazes.Console.Infrastructure;

[UsedImplicitly]
[Verb("remote", HelpText = "Run puzzles from Puzzle Madness.")]
public class RemoteOptions
{
    [Option('d', "difficulty", Required = true, HelpText = "The class of puzzles to solve (small, medium, large, extra large, mixed).")]
    public Difficulty Difficulty { get; set; }

    [Option('y', "year", Required = false, HelpText = "The year of the puzzle.")]
    public int? Year { get; [UsedImplicitly] set; }

    [Option('m', "month", Required = false, HelpText = "The month of the puzzle.")]
    public int? Month { get; [UsedImplicitly] set; }

    [Option('w', "day", Required = false, HelpText = "The day of the puzzle.")]
    public int? Day { get; [UsedImplicitly] set; }

    public (bool IsValid, string Message) Validate()
    {
        if (Day.HasValue)
        {
            Month ??= DateTime.Now.Month;

            Year ??= DateTime.Now.Year;
        }

        return (true, null);
    }
}