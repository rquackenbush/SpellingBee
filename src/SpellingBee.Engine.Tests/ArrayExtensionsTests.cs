namespace SpellingBee.Engine.Tests;

public class ArrayExtensionsTests
{
    [Theory]
    [InlineData(new char[] { 'a', 'b', 'c' }, 'd', new char[] { 'a', 'b', 'c', 'd' })]
    public void AppendToNewArrayTests(char[] values, char value, char[] expected)
    {
        values.AppendToNewArray(value).ShouldBe(expected);
    }
}
