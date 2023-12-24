namespace SpellingBee.Engine.Tests
{
    public class WordEntryFactoryTests
    {
        [Theory]
        [InlineData('a', 0b00000001)]
        [InlineData('b', 0b00000010)]
        [InlineData('c', 0b00000100)]
        public void ValidLetterMaskTests(char letter, uint expectedLetterMask)
        {
            WordEntryFactory.TryGetLetterMask(letter, out var letterMask).ShouldBeTrue();

            letterMask.ShouldBe(expectedLetterMask);
        }

        [Theory]
        [InlineData('+')]
        [InlineData('-')]
        [InlineData('=')]
        [InlineData('.')]
        [InlineData('Z')]
        public void InvalidletterMaskTests(char letter)
        {
            WordEntryFactory.TryGetLetterMask(letter, out _).ShouldBeFalse();
        }

        [Theory]
        [InlineData("land", 0b00000000000000000010100000001001)]
        public void ValidWordMaskTests(string word, uint expectedWordMask)
        {
            WordEntryFactory.TryGetWordMask(word, out var wordMask).ShouldBeTrue();

            wordMask.ShouldBe(expectedWordMask);
        }

        [Theory]
        [InlineData("09123")]
        public void InvalidWordMaskTests(string word)
        {
            WordEntryFactory.TryGetWordMask(word, out _).ShouldBeFalse();
        }
    }
}
