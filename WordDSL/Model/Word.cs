namespace WordPlay.WordDSL.Model
{
    /// <summary>
    /// A single Word with a Native and Local form
    /// </summary>
    public class Word
    {
        /// <summary>
        /// Constructs a Word couple
        /// </summary>
        /// <param name="native"></param>
        /// <param name="local"></param>
        public Word(string native, string local)
        {
            Native = native;
            Local = local;
        }

        /// <summary>
        /// A word in the currently active language
        /// </summary>
        public string Native { get; private set; }

        /// <summary>
        /// A word in the language local to the user or application
        /// </summary>
        public string Local { get; private set; }
    }
}
