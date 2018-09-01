using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MovieRenamer
{
    public partial class MovieRenamerControl
    {
        public MovieRenamerControl()
        {
            InitializeComponent();
        }

        private void OnChildGotFocus(object sender, RoutedEventArgs eventArgs)
        {
            if (sender is TextBox textBox)
            {
                ListBoxOriginalFileNames.SelectedItem = textBox.DataContext;
            }
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs eventArgs)
        {
            if (string.IsNullOrEmpty(eventArgs.Text.RemoveBadCharacters()))
            {
                eventArgs.Handled = true;
            }
        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs eventArgs)
        {
            if (sender is Button button)
            {
                if (button.DataContext is MovieFile movieFile)
                {
                    Process.Start(movieFile.OriginalMovieFile.FullName);
                }
            }
        }
    }
}