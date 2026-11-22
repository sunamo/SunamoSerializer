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

    /// <summary>
    /// Tests that empty elements keep their position when a line is split.
    /// </summary>
    [Fact]
    public void GetAllElementsLineKeepEmptyTest()
    {
        var result = SF.GetAllElementsLineKeepEmpty("a| |c||e");
        Assert.Equal(new List<string> { "a", "", "c", "", "e" }, result);
    }

    /// <summary>
    /// Tests that a missing file gives an empty list and blank lines are skipped.
    /// </summary>
    [Fact]
    public void GetAllElementsFileKeepEmptyTest()
    {
        Assert.Empty(SF.GetAllElementsFileKeepEmpty(Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt")));
        var file = Path.GetTempFileName();
        File.WriteAllText(file, "a||c" + Environment.NewLine + Environment.NewLine + "d|e|");
        var rows = SF.GetAllElementsFileKeepEmpty(file);
        File.Delete(file);
        Assert.Equal(2, rows.Count);
        Assert.Equal(new List<string> { "a", "", "c" }, rows[0]);
        Assert.Equal(new List<string> { "d", "e", "" }, rows[1]);
    }
}
