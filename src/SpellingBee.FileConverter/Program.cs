using CsvHelper;
using SpellingBee.Engine;
using System.Globalization;

var wordEntries = new List<WordEntry>(55000);
var foundWords = new HashSet<string>();
string? previousWord = null;

using (var reader = new StreamReader("dictionary.csv"))
using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
{
    while (csv.Read())
    {
        var word = csv.GetField<string>(0);

        if (!string.IsNullOrEmpty(word))
        {
            //If the previous word is the same, that means that the input file is likely in alphabetical order.
            if (word != previousWord)
            {
                previousWord = word;

                if (WordEntryFactory.TryCreateWordEntry(word, out var wordEntry))
                { 
                    if (!foundWords.Contains(wordEntry.Word))
                    {
                        foundWords.Add(wordEntry.Word);

                        wordEntries.Add(wordEntry);
                    }
                }
            }
        }
    }
}

using (var writer = new StreamWriter("../../../../SpellingBee.Host/dictionary.txt"))
{
    foreach(var wordEntry in wordEntries)
    {
        writer.WriteLine(wordEntry.Word);
    }

    Console.WriteLine($"Wrote {wordEntries.Count:###,###,###,###} words to the custom dictionary.");
}