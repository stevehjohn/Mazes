using CommandLine;
using Mazes.Console.Infrastructure;
using Mazes.Console.Runners;

namespace Mazes.Console;

public static class EntryPoint
{
    public static void Main(string[] arguments)
    {
        var parser = new Parser(settings =>
        {
            settings.CaseInsensitiveEnumValues = true;
            settings.HelpWriter = System.Console.Out;
        });

        parser.ParseArguments<RemoteOptions>(arguments)
            .WithParsed(options => new Remote().Run(options));
    }
}