namespace WordPlay
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Media;
    using System.Linq;
    using System.Windows.Controls;
    using System.Windows.Input;
    using WordPlay.WordDSL;
    using WordPlay.WordDSL.Model;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IWordListFetcher _fetcher = new BykiWordListFetcher();
        private WordManager _wordManager;
        private Score _score = new Score();

        public MainWindow()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            if(ex != null)
                MessageBox.Show(string.Format("An unexpected error occured with message: {0}, stack: {1}", ex.Message, ex.StackTrace));
            else
                MessageBox.Show("An unexpected error occured, no further information is available.");            
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            RunAsync((a, b) =>
            {
                var available = _fetcher.GetAvailableLanguages().ToList();
                InvokeSync(() => { lbLanguageSelection.ItemsSource = available; });
            });
        }

        /// <summary>
        /// Invokes the Action on the UI thread
        /// </summary>
        /// <param name="ac"></param>
        private void InvokeSync(Action ac)
        {
            this.Dispatcher.Invoke(ac);
        }

        /// <summary>
        /// Runs the given delegate async by using a BackgroundWorker and default error handling
        /// </summary>
        /// <param name="doWork"></param>
        private void RunAsync(DoWorkEventHandler doWork)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += doWork;
            worker.RunWorkerCompleted += (result, args) => { if (args.Error != null) MessageBox.Show(args.Error.Message); };
            worker.RunWorkerAsync();
        }

        /// <summary>
        /// Language was selected so retrieve all the wordlists for the selected language
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbLanguageSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {   
            ComboBox control = sender as ComboBox;
            if(control == null)
                throw new InvalidOperationException("It's not a combobox");
            string selection = control.SelectedItem as string;
            if (string.IsNullOrEmpty(selection))
            {
                lbWordListSelection.ItemsSource = null;
                return;
            }
            RunAsync((a, b) => 
            { 
                var available = _fetcher.GetAvailableLists(selection).ToList();
                InvokeSync(() => { lbWordListSelection.ItemsSource = available; });
            });
        }

        /// <summary>
        /// Returns the currently selected direction as string
        /// </summary>
        /// <returns></returns>
        private string GetDirectionFromCombo()
        {
            string tag = string.Empty;
            InvokeSync(() =>
            {
                ComboBoxItem item = lbDirectionSelection.SelectedItem as ComboBoxItem;
                if (item == null)
                    throw new InvalidOperationException("SelectedItem is not a ComboBoxItem or nothing is selected");
                tag = item.Tag.ToString();
            });
            return tag;
        }

        /// <summary>
        /// WordList was selected so retrieve all the Words for the selected list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbWordListSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox control = sender as ComboBox;
            if (control == null)
                throw new InvalidOperationException("It's not a combobox");
            WordList list = control.SelectedItem as WordList;
            if (list == null)
                return;
            RunAsync((a, b) => {
                _fetcher.GetWordsForList(list);
                _score.Reset();
                UpdateScore();
                _wordManager = new WordManager(list);
                _wordManager.SetDirection(GetDirectionFromCombo());
                InvokeSync(NextQuestion);
            });
        }

        /// <summary>
        /// Retrieves the next word from the wordlist and fills the textfields
        /// </summary>
        private void NextQuestion()
        {
            if (_wordManager == null)
                return;
            tbYourAnswer.Text = string.Empty;
            tbYourAnswer.Foreground = new SolidColorBrush(Colors.Black);
            tbTheAnswer.Text = string.Empty;
            _wordManager.NextWord();
            tbQuestion.Text = _wordManager.Question;
        }

        /// <summary>
        /// Checks the answer, colors the field and shows the real answer
        /// </summary>
        private void CheckAnswer()
        {
            if (_wordManager == null)
                return;
            tbTheAnswer.Text = _wordManager.Answer;
            if (tbYourAnswer.Text.Equals(_wordManager.Answer, StringComparison.InvariantCultureIgnoreCase))
            {
                _score.AddCorrect();
                tbYourAnswer.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                _score.AddWrong();
                tbYourAnswer.Foreground = new SolidColorBrush(Colors.Red);
            }
            UpdateScore();
        }

        /// <summary>
        /// Updates the score display
        /// </summary>
        private void UpdateScore()
        {
            InvokeSync(() =>
            {
                lblScore.Content = string.Format("{0} out of {1}", _score.Correct, (_score.Correct + _score.Wrong));
            });
        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            CheckAnswer();
        }

        private void tbAnswer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // If enter is pressed when the answer has already been shown,
                // the user most likely just wants the next question
                if (string.IsNullOrEmpty(tbTheAnswer.Text))
                    CheckAnswer();
                else
                    NextQuestion();
            }   
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            NextQuestion();
        }

        private void lbDirectionSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_wordManager != null)
            {
                _wordManager.SetDirection(GetDirectionFromCombo());                
                NextQuestion();
            }
        }

        private class Score
        {
            public Score()
            {
                Reset();
            }

            public int Correct { get; private set; }

            public int Wrong { get; private set; }

            public void AddCorrect() { Correct++; }

            public void AddWrong() { Wrong++; }

            public void Reset() { Correct = 0; Wrong = 0; }
        }
    }
}
