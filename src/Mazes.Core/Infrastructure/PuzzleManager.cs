using System.Text.Json;
using Mazes.Core.Models;

namespace Mazes.Core.Infrastructure;

public class PuzzleManager
{
    private List<Maze> _puzzles;

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public Maze GetPuzzle(int puzzleNumber) => _puzzles[puzzleNumber]; //.Clone();
        
    public static string Path { get; set; }

    private static readonly Lazy<PuzzleManager> Lazy = new(GetPuzzleManager);

    public static PuzzleManager Instance => Lazy.Value;
    
    public int PuzzleCount => _puzzles.Count;

    private PuzzleManager()
    {
    }

    private static PuzzleManager GetPuzzleManager()
    {
        if (Path == null)
        {
            throw new InvalidOperationException("Please set the Path property before using the PuzzleManager.");
        }

        var puzzleJson = File.ReadAllText(Path);

        var puzzles = JsonSerializer.Deserialize<Puzzle[]>(puzzleJson, JsonSerializerOptions);

        var mazes = new List<Maze>();

        foreach (var puzzle in puzzles)
        {
            var grid = new Maze(puzzle);

            mazes.Add(grid);
        }
        
        var instance = new PuzzleManager
        {
            _puzzles = mazes
        };
        
        return instance;
    }
}