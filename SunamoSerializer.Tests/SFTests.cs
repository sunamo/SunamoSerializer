namespace SunamoSerializer.Tests;

/// <summary>
/// Tests for the SF serialization class.
/// </summary>
public class SFTests
{
    /// <summary>
    /// Tests that PrepareToSerialization correctly joins elements with default delimiter.
    /// </summary>
    [Fact]
    public void PrepareToSerializationTest()
    {
        var result = SF.PrepareToSerialization("ab", "cd");
        Assert.Equal("ab|cd", result);
    }
}
