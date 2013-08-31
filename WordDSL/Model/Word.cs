namespace WordPlay.WordDSL.Model
{
    /// <summary>
    /// A single Word with a Native and Local form
    /// </summary>
    public class Word
    {
        public Word(string native, string local)
        {
            Native = native;
            Local = local;
        }

        public string Native { get; private set; }

        public string Local { get; private set; }
    }
}
