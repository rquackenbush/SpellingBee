using SpellingBee.Engine;
using System.Diagnostics;

const string outerLetters = "ndriao";
const char innerLetter = 'l';

var stopwatch = Stopwatch.StartNew();

var wordReader = new FileWordReader("dictionary.txt");

var finder = new WordFinder();

foreach(var word in finder.FindWords(wordReader, innerLetter, outerLetters.ToCharArray()))
{
    var prefix = word.IsPanagram ? "*" : " ";

    Console.WriteLine($"{prefix} {word.Word}");
}

Console.WriteLine();

stopwatch.Stop();

Console.WriteLine($" Elapsed time: {stopwatch.Elapsed}");