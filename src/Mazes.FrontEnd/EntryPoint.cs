using System;
using Mazes.Core.Models;
using Mazes.FrontEnd.Display;

namespace Mazes.FrontEnd;

public static class EntryPoint
{
    public static void Main(string[] arguments)
    {
        Console.WriteLine($"Args: {string.Join(", ", arguments)}");
        var renderer = arguments.Length > 0
            ? new Renderer(Enum.Parse<Difficulty>(arguments[0], true))
            : new Renderer(Difficulty.Small);

        renderer.Run();
    }
}