namespace WordPlay.WordDSL
{
    using System.Collections.Generic;
    using System.IO;
    using Model;
    using System.Linq;

    /// <summary>
    /// An implementation of IWordListFetcher which pulls wordlists from local text files
    /// </summary>
    public class TextFileWordListFetcher : IWordListFetcher
    {
        private const string LanguageDir = "wordlists";

        /// <inheritdoc />
        public IEnumerable<string> GetAvailableLanguages()
        {
            return Directory.EnumerateFiles(LanguageDir)
                .Select(f => StripDirectory(f).Split('_').First().ToLowerInvariant().CapFirstLetter()).Distinct();
        }

        /// <inheritdoc />
        public IEnumerable<WordList> GetAvailableLists(string language)
        {   
            return Directory.EnumerateFiles(LanguageDir)
                .Where(f => StripDirectory(f).StartsWith(language, true, null))
                    .Select(f => new WordList(GetWordlistNameFromPath(f), f));
        }

        /// <inheritdoc />
        public bool GetWordsForList(WordList list)
        {
            StreamReader reader = new StreamReader(list.URL, true);
            string line;

            while((line = reader.ReadLine()) != null) {
                string[] split = line.Split('~');
                if(split.Length == 2)
                    list.Words.Add(new Word(split[1], split[0])); // Local ~ Native
            }

            return true;
        }

        private string StripDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;
            return path.Replace(LanguageDir, "").Replace("\\", "");
        }

        private string GetWordlistNameFromPath(string path)
        {
            return StripDirectory(path).Split('_').Last().RemoveExtension().CapFirstLetter();
        }
    }

    /// <summary>
    /// A class which holds extension methods on string
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Capitalizes the first letter of the given string
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string CapFirstLetter(this string @this)
        {
            if (string.IsNullOrEmpty(@this))
                return @this;
            return @this.First().ToString().ToUpperInvariant() + @this.Substring(1);
        }

        /// <summary>
        /// Removes the extension from the given filename
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string RemoveExtension(this string @this)
        {
            if (string.IsNullOrEmpty(@this) || !@this.Contains("."))
                return @this;
            return @this.Substring(0, @this.LastIndexOf('.'));
        }
    }
}
