// ReSharper disable UnusedMember.Global

using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Mazes.Core.Models;

[UsedImplicitly]
public enum Difficulty
{
    Small,
    Medium,
    Large,
    [JsonStringEnumMemberName("xlarge")]
    ExtraLarge,
    Mixed
}