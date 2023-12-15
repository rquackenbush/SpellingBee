using System.Diagnostics;

namespace SpellingBee.Engine;

public class WordFinder
{
    private readonly WordEntry[] wordEntries;
    private readonly char innerLetter;
    private readonly char[] outerLetters;
    private readonly char[] allLetters;
    private readonly int numberOfLetters;

    public WordFinder(WordEntry[] wordEntries, char innerLetter, char[] outerLetters)
    {
        this.wordEntries = wordEntries;
        this.innerLetter = innerLetter;
        this.outerLetters = outerLetters;
        numberOfLetters = outerLetters.Length + 1;
        allLetters = new char[numberOfLetters];

        allLetters[0] = innerLetter; ;

        for(var index = 0; index < outerLetters.Length; index++)
        {
            allLetters[index + 1] = outerLetters[index];
        }
    }

    public FindWordsResult FindWords()
    {
        var stopwatch = Stopwatch.StartNew();

        var foundWords = new List<FoundWord>();

        var requiredBit = WordEntryFactory.GetLetterMask(innerLetter);

        if (requiredBit == null)
            throw new InvalidOperationException("The center letter wasn't bitmapped.");

        var allLettersMask = WordEntryFactory.GetWordMask(new string(allLetters));

        if (allLettersMask == null)
            throw new InvalidOperationException("All letters couldn't create a mask");

        var invertedAllLettersMask = ~allLettersMask.Value;

        foreach(var wordEntry in wordEntries)
        {
            if ((wordEntry.Mask & invertedAllLettersMask) == 0)
            {
                if ((wordEntry.Mask & requiredBit) > 0)
                {
                    var isPanagram = (wordEntry.Mask & allLettersMask.Value) == allLettersMask;

                    foundWords.Add(new FoundWord(wordEntry.Word, isPanagram));
                }
            }
        }        

        stopwatch.Stop();

        return new FindWordsResult(stopwatch.Elapsed, foundWords.ToArray());
    }

    //private string GetWord(char[] letters, int[] indices)
    //{
    //    var wordChars = new char[indices.Length];

    //    for(var index = 0; index < indices.Length; index++)
    //    {
    //        wordChars[index] = letters[indices[index]];
    //    }

    //    return new string(wordChars);
    //}   
}

