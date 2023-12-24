namespace SpellingBee.Engine;

public static class WordEntryFactory
{
    public const int MinimumWordLength = 4;
    public const int MaximumWordLength = 19;

    /// <summary>
    /// The number of unique letters allowed for this game. If a word has more than this many unique letters, it cannot
    /// be a valid solution and can be discarded.
    /// </summary>
    public const int LetterCount = 7;

    public static bool TryCreateWordEntry(string word, out WordEntry wordEntry)
    {
        if (string.IsNullOrEmpty(word))
            throw new ArgumentException(nameof(word) + " was null or empty.");

        wordEntry = default;

        //Coerce the word
        word = word.Trim().ToLower();

        if (word.Length < MinimumWordLength)
            return false;

        if (word.Length > MaximumWordLength)
            return false;

        if (!TryGetWordMask(word, out var wordMask))
            return false;

        //We're good 
        wordEntry = new WordEntry(word, wordMask);
        return true;
    }

    public static bool TryGetWordMask(string word, out uint value)
    {
        uint wordMask = 0;
        var uniqueLetterCount = 0;

        value = default;

        foreach (char letter in word)
        {

            if (!TryGetLetterMask(letter, out var letterMask))
                return false;

            //Check to see if this letter has already appeared
            if ((wordMask & letterMask) == 0)
            {
                uniqueLetterCount++;
                wordMask |= letterMask;
            }

            if (uniqueLetterCount > LetterCount)
                return false;
        }

        value = wordMask;
        return true;
    }

    /// <summary>
    /// Attempts to get the 32bit bit-mask for a letter.
    /// </summary>
    /// <param name="letter"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <remarks>
    /// a = 00000000 00000000 00000000 000000001
    /// b = 00000000 00000000 00000000 000000010
    /// c = 00000000 00000000 00000000 000000100
    /// </remarks>
    public static bool TryGetLetterMask(char letter, out uint value)
    {
        var index = letter - 'a';

        //We only support the letters a-z
        if (index < 0 || index > 26)
        {
            value = default;
            return false;
        }

        //Shift!
        value = (uint)1 << index;

        return true;
    }
}
