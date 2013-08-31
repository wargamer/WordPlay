namespace WordPlay.WordDSL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using HtmlAgilityPack;
    using Model;

    public class BykiWordListFetcher : IWordListFetcher
    {
        private const string BaseURL = "http://www.byki.com";
        private const string CategoryURL = "http://www.byki.com/category/";
        private const string LanguageListURL = "http://www.byki.com/listcentral.html";
        private HtmlWeb HTMLWeb = new HtmlWeb();

        /// <summary>
        /// Converts certain web chars to normal chars
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        private string FixChars(string subject)
        {
            return subject.Replace("`", "'");
        }

        public IEnumerable<string> GetAvailableLanguages()
        {
            HtmlDocument document = HTMLWeb.Load(LanguageListURL);

            var centerCard = document.DocumentNode.GetNodesByClass("centerCard").FirstOrDefault();
            if (centerCard == null)
                throw new InvalidOperationException("CenterCard could not be found!");

            foreach (var node in centerCard.GetNodesByClass("mainTag"))
                yield return node.InnerText;
        }

        public IEnumerable<WordList> GetAvailableLists(string language)
        {
            if (language == null)
                throw new NullReferenceException("language can not be null");

            string actualURL = string.Format("{0}{1}", CategoryURL, language.ToLowerInvariant());
            HtmlDocument document = HTMLWeb.Load(actualURL);

            foreach (var node in document.DocumentNode.GetNodesByClass("BList"))
            {
                HtmlNode theLink = node.Descendants("a").FirstOrDefault();
                if (theLink != null)
                {
                    yield return new WordList(theLink.InnerText, theLink.GetAttributeValue("href", ""));                    
                }
            }
        }

        public bool GetWordsForList(WordList list)
        {
            if (list == null)
                throw new NullReferenceException("list can not be null");
            
            HtmlDocument document = HTMLWeb.Load(string.Format("{0}{1}", BaseURL, list.URL));
            foreach (var node in document.DocumentNode.GetNodesByClass("tblListItems"))
            {
                foreach (var tr in node.Descendants("tr"))
                {
                    var queries = tr.GetNodesByClass("loadFontSide2").ToList();
                    var answers = tr.GetNodesByClass("loadFontSide1").ToList();
                    if (queries.Any() && answers.Any())
                    {
                        list.Words.Add(
                            new Word(
                                FixChars(queries.First().InnerText),
                                FixChars(answers.First().InnerText)
                            )
                        );
                    }
                }
            }

            return true;
        }
    }
}
