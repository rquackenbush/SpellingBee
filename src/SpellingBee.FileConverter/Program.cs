using CsvHelper;
using SpellingBee.Engine;
using System.Globalization;

var wordEntries = new List<WordEntry>(55000);
var foundWords = new HashSet<string>();

using (var reader = new StreamReader("dictionary.csv"))
using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
{
    while (csv.Read())
    {
        var word = csv.GetField<string>(0);

        if (!string.IsNullOrEmpty(word))
        {
            var wordEntry = WordEntryFactory.CreateWordEntry(word);

            if (wordEntry != null)
            {
                if (!foundWords.Contains(wordEntry.Value.Word))
                {
                    foundWords.Add(wordEntry.Value.Word);

                    wordEntries.Add(wordEntry.Value);
                }
            }
        }
    }
}

using (var writer = new StreamWriter("../../../../SpellingBee.Host/dictionary.txt"))
{
    writer.WriteLine($"{wordEntries.Count}");

    foreach(var wordEntry in wordEntries)
    {
        writer.WriteLine(wordEntry.Word);
        writer.WriteLine(wordEntry.Mask);
    }
}