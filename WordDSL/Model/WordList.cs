namespace WordPlay.WordDSL.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// List of Words
    /// </summary>
    public class WordList
    {   
        public WordList(string name, string url)
        {
            Name = name;
            URL = url;
            Words = new List<Word>();            
        }

        private static DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        public string Name { get; private set; }

        public string URL { get; private set; }

        public IList<Word> Words { get; private set; }

        public Word CurrentWord { get; private set; }

        public int CurrentWordNumber { get; private set; }
        
        /// <summary>
        /// Word Enumerator, infinite
        /// </summary>
        private IEnumerator<Word> _theEnumerator;

        public override string ToString()
        {
            return Name;
        }

        private IEnumerable<Word> GetRandomWord()
        {            
            while (true)
            {
                if (!Words.Any())
                    yield break;
                Random rnd = new Random((int)(Epoch - DateTime.UtcNow).TotalSeconds);
                CurrentWordNumber = 0;
                foreach (Word randomword in Words.OrderBy(a => rnd.Next()))
                {
                    CurrentWordNumber++;
                    yield return randomword;
                }
            }
        }

        /// <summary>
        /// Returns the next word in a pseudo-random order
        /// </summary>        
        public Word GetNextWord()
        {
            if (_theEnumerator == null)
                _theEnumerator = GetRandomWord().GetEnumerator();
            _theEnumerator.MoveNext();
            return (CurrentWord = _theEnumerator.Current);
        }
    }
}
