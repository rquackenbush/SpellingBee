namespace SpellingBee.Engine;

public static class WordEntryFactory
{
    public const int MinimumWordLength = 4;
    public const int MaximumWordLength = 19;

    public static WordEntry[] LoadWordEntries(string filename)
    {
        using (var reader = new StreamReader(filename))
        {
            var count = int.Parse(reader.ReadLine());

            var wordEntries = new WordEntry[count];

            for (var index = 0; index < count; index++)
            {
                var word = reader.ReadLine();
                uint mask = uint.Parse(reader.ReadLine());

                wordEntries[index] = new WordEntry(word, mask);
            }

            return wordEntries;
        }
    }

    public static WordEntry? CreateWordEntry(string word)
    {
        if (string.IsNullOrEmpty(word))
            throw new ArgumentException(nameof(word) + " was null or empty.");

        //Coerce the word
        word = word.Trim().ToLower();

        if (word.Length < MinimumWordLength)
            return null;

        if (word.Length > MaximumWordLength)
            return null;

        uint? wordMask = GetWordMask(word);

        if (wordMask != null)
            return new WordEntry(word, wordMask.Value);

        return null;
    }

    public static uint? GetWordMask(string word)
    {
        uint wordMask = 0;

        foreach (char letter in word)
        {
            var letterMask = GetLetterMask(letter);

            if (letterMask == null)
                return null;

            wordMask |= letterMask.Value;
        }

        return wordMask;
    }

    public static uint? GetLetterMask(char letter)
    {
        var index = letter - 'a';

        if (index < 0 || index > 26)
            return null;

        return (uint)1 << index;
    }
}
