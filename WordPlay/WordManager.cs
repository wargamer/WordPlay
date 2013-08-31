namespace WordPlay
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using WordPlay.WordDSL.Model;

    class WordManager
    {
        private WordList _worldList;
        private Word _currentWord;

        public WordDirection Direction
        {
            get;
            set;
        }

        /// <summary>
        /// Returns the question based on the current direction
        /// </summary>
        public string Question
        {
            get
            {
                if (_currentWord == null)
                    return string.Empty;
                if(Direction == WordDirection.Normal)
                    return _currentWord.Local;
                if(Direction == WordDirection.Reverse)
                    return _currentWord.Native;
                throw new InvalidOperationException(string.Format("Direction not supported {0}", Direction));
            }
        }

        /// <summary>
        /// Returns the answer to the question based on current direction
        /// </summary>
        public string Answer
        {
            get
            {
                if (_currentWord == null)
                    return string.Empty;
                if (Direction == WordDirection.Normal)
                    return _currentWord.Native;
                if (Direction == WordDirection.Reverse)
                    return _currentWord.Local;
                throw new InvalidOperationException(string.Format("Direction not supported {0}", Direction));                
            }
        }

        public WordManager(WordList worldlist)
        {
            _worldList = worldlist;
        }

        /// <summary>
        /// Parses the passed direction to a WordDirection and sets it
        /// </summary>
        /// <param name="direction"></param>
        public void SetDirection(string direction)
        {
            if (string.IsNullOrEmpty(direction))
                throw new NullReferenceException("Direction can not be null");
            WordDirection dir;
            if (Enum.TryParse<WordDirection>(direction, true, out dir))
                Direction = dir;
            else
                throw new InvalidOperationException(string.Format("Invalid direction was passed {0}", direction)); 
        }

        /// <summary>
        /// Draws the next word
        /// </summary>
        public void NextWord()
        {
            _currentWord = _worldList.GetNextWord();
        }
    }

    public enum WordDirection
    {
        Normal, // Local -> Native
        Reverse, // Native -> Local
    };
}
