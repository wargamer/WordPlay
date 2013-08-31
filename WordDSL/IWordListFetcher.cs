namespace WordPlay.WordDSL
{
    using System.Collections.Generic;
    using Model;

    public interface IWordListFetcher
    {
        /// <summary>
        /// Returns a list of available languages
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetAvailableLanguages();

        /// <summary>
        /// Returns a list of available word lists
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        IEnumerable<WordList> GetAvailableLists(string language);

        /// <summary>
        /// Retrieves all the words for the given wordlist
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool GetWordsForList(WordList list);
    }
}
