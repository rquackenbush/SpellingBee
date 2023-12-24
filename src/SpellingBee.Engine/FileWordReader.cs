namespace SpellingBee.Engine
{
    /// <summary>
    /// Reads words from file.
    /// </summary>
    public class FileWordReader
    {
        private readonly string path;

        public FileWordReader(string path) 
        {
            this.path = path;
        }

        /// <summary>
        /// Loads our specialized dictionary format.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public IEnumerable<WordEntry> ReadWords()
        {
            using var reader = new StreamReader(path);

            string? word;
            var lineNumber = 1;

            while ((word = reader.ReadLine()) != null)
            {
                if (!WordEntryFactory.TryCreateWordEntry(word, out var wordEntry))
                    throw new Exception($"Line {lineNumber}: Unable to create WordEntry from '{word}'.");

                lineNumber++;

                yield return wordEntry;
            }
        }

    }
}
