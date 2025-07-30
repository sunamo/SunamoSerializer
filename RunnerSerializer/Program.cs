using SunamoSerializer.Tests;

namespace RunnerSerializer;

internal class Program
{
    static void Main()
    {
        Console.WriteLine("Hello, World!");

        SFTests t = new();
        t.PrepareToSerializationTest();
    }
}
