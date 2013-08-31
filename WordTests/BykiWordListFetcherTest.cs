namespace WordTests
{
    using WordPlay.WordDSL;
    using WordPlay.WordDSL.Model;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///This is a test class for BykiWordListFetcherTest and is intended
    ///to contain all BykiWordListFetcherTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BykiWordListFetcherTest
    {
        /// <summary>
        ///A test for BykiWordListFetcher Constructor
        ///</summary>
        [TestMethod()]
        public void BykiWordListFetcherConstructorTest()
        {
            IWordListFetcher target = new BykiWordListFetcher();
        }

        /// <summary>
        ///A test for GetAvailableLanguages
        ///</summary>
        [TestMethod()]
        public void GetAvailableLanguagesTest()
        {
            IWordListFetcher target = new BykiWordListFetcher();
            var languages = target.GetAvailableLanguages();
            Assert.IsTrue(languages.Any());
        }

        /// <summary>
        ///A test for GetAvailableLists
        ///</summary>
        [TestMethod()]
        public void GetAvailableListsTest()
        {
            IWordListFetcher target = new BykiWordListFetcher();
            var lists = target.GetAvailableLists("hungarian");            
            Assert.IsTrue(lists.Any());
        }

        /// <summary>
        ///A test for GetWordsForList
        ///</summary>
        [TestMethod()]
        public void GetWordsForListTest()
        {
            IWordListFetcher target = new BykiWordListFetcher();
            var lists = target.GetAvailableLists("hungarian").ToList();
            Assert.IsTrue(lists.Any());
            WordList list = lists.First();
            Assert.IsTrue(target.GetWordsForList(list));
            Assert.IsTrue(list.Words.Any());
        }
    }
}
