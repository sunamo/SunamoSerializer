using SunamoSerializer.Tests;

namespace RunnerSerializer;

/// <summary>
/// Entry point for the SunamoSerializer test runner.
/// </summary>
internal class Program
{
    static void Main()
    {
        Console.WriteLine("Hello, World!");

        SFTests t = new();
        t.PrepareToSerializationTest();
    }
}
