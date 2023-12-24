namespace SpellingBee.Engine;

public class WordFinder
{
    /// <summary>
    /// Find the words in the provided <paramref name="wordReader"/> that contain <paramref name="innerLetter"/> and some combination of <paramref name="outerLetters"/>.
    /// </summary>
    /// <param name="wordReader"></param>
    /// <param name="innerLetter"></param>
    /// <param name="outerLetters"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public IEnumerable<FoundWord> FindWords(FileWordReader wordReader, char innerLetter, char[] outerLetters)
    {
        var numberOfLetters = outerLetters.Length + 1;

        if (numberOfLetters != WordEntryFactory.LetterCount)
            throw new InvalidOperationException($"Expected a total letter count of {WordEntryFactory.LetterCount} but received {numberOfLetters} instead.");

        var allowedLetters = outerLetters.AppendToNewArray(innerLetter);

        if (!WordEntryFactory.TryGetLetterMask(innerLetter, out var requiredBit))
            throw new InvalidOperationException("The center letter wasn't bitmapped.");

        if (!WordEntryFactory.TryGetWordMask(new string(allowedLetters), out var allLettersMask))
            throw new InvalidOperationException("All letters couldn't create a mask");

        var invertedAllowedLettersMask = ~allLettersMask;

        foreach(var wordEntry in wordReader.ReadWords())
        {
            if (IsMatch(wordEntry.Mask, invertedAllowedLettersMask, requiredBit))
            {
                var isPanagram = IsPanagram(wordEntry.Mask, allLettersMask);

                yield return new FoundWord(wordEntry.Word, isPanagram);
            }
        }        
    }

    /// <summary>
    /// Determines if the provided word (mask) is a panagram.
    /// </summary>
    /// <param name="wordMask"></param>
    /// <param name="allLettersMask"></param>
    /// <returns></returns>
    public static bool IsPanagram(uint wordMask, uint allLettersMask)
    {
        return (wordMask & allLettersMask) == allLettersMask;
    }

    /// <summary>
    /// Determines if the provided word is a match (is only comprised of allowed letters and has the required letter)
    /// </summary>
    /// <param name="wordMask"></param>
    /// <param name="invertedAllowedLettersMask"></param>
    /// <param name="requiredBit"></param>
    /// <returns></returns>
    public static bool IsMatch(uint wordMask, uint invertedAllowedLettersMask, uint requiredBit)
    {
        if ((wordMask & invertedAllowedLettersMask) != 0)
            return false;
        
        if ((wordMask & requiredBit) > 0)
            return true;

        return false;
    }
}

