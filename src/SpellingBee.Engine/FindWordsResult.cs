namespace SpellingBee.Engine;

public record class FindWordsResult(TimeSpan Duration, FoundWord[] Words)
{

}

public record class FoundWord(string Word, bool IsPanagram)
{
}
