using Mazes.Core.Models;

namespace Mazes.Core.Extensions;

public static class DifficultyExtensions
{
    extension(Difficulty difficulty)
    {
        public string ToUrlString()
        {
            return difficulty switch
            {
                Difficulty.ExtraLarge => "xlarge",
                _ => difficulty.ToString().ToLowerInvariant()
            };
        }
    }
}