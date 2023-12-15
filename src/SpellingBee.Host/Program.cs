using SpellingBee.Engine;

const string outerLetters = "ndriao";
const char innerLetter = 'l';

var wordEntries = WordEntryFactory.LoadWordEntries("dictionary.txt");

var finder = new WordFinder(wordEntries, innerLetter, outerLetters.ToCharArray());

var result = finder.FindWords();

foreach(var word in result.Words)
{
    var prefix = word.IsPanagram ? "*" : " ";

    Console.WriteLine($"{prefix} {word.Word}");
}

Console.WriteLine();

Console.WriteLine($"  {result.Duration}");