using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace MovieRenamer
{
    /// <summary>
    /// Interaktionslogik für MovieRenamerControl.xaml
    /// </summary>
    public partial class MovieRenamerControl : UserControl
    {
        #region Constructors
        
        public MovieRenamerControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Events
        
        private void OnChildGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            lbxOriginalFileNames.SelectedItem = textBox.DataContext;
            //lbxNewFileNames.SelectedItem = textBox.DataContext;
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Text.RemoveBadCharacters()))
                e.Handled = true;
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            MovieFile movieFile = (MovieFile)button.DataContext;
            Process.Start(movieFile.OriginalMovieFile.FullName);
        }

        #endregion

    }
}
