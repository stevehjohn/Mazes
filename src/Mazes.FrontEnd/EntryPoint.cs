using Mazes.FrontEnd.Display;

namespace Mazes.FrontEnd;

public static class EntryPoint
{
    public static void Main()
    {
        var renderer = new Renderer();
        
        renderer.Run();
    }
}