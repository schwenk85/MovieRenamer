using System.Windows;
using System.Windows.Controls;
using System;
using System.Net;

namespace MovieRenamer
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MovieRenamerWindow : Window
    {
        #region Fields

        WebClient _WebClient = new WebClient();

        #endregion

        #region Constructors

        public MovieRenamerWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void Navigate()
        {
            // Get URI to navigate to
            Uri uri = new Uri(this.txtURL.Text, UriKind.RelativeOrAbsolute);

            // Only absolute URIs can be navigated to
            if (!uri.IsAbsoluteUri)
                System.Windows.MessageBox.Show("The Address URI must be absolute eg 'http://www.imdb.com/'");
            else
                browser.Navigate(uri);
        }

        private string ReplaceSpecialSigns(string str)
        {
            str = str.Replace("&#xC4;", "Ä");
            str = str.Replace("&#xD6;", "Ö");
            str = str.Replace("&#xDC;", "Ü");
            str = str.Replace("&#xE4;", "ä");
            str = str.Replace("&#xF6;", "ö");
            str = str.Replace("&#xFC;", "ü");
            str = str.Replace("&#xDF;", "ß");
            str = str.Replace("&#x26;", "&");
            str = str.Replace("&#x27;", "'");

            return str;
        }

        #endregion

        #region Events

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtMainFolder.Focus();
        }

        private void btnPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (browser.CanGoBack)
                browser.GoBack();
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (browser.CanGoForward)
                browser.GoForward();
        }

        private void btnOpenUri_Click(object sender, RoutedEventArgs e)
        {
            Navigate();
        }

        private void btnOpenImdbDe_Click(object sender, RoutedEventArgs e)
        {
            txtURL.Text = "http://www.imdb.de/";
            Navigate();
        }

        private void btnOpenImdbCom_Click(object sender, RoutedEventArgs e)
        {
            txtURL.Text = "http://www.imdb.com/";
            Navigate();
        }

        private void btnOpenGoogle_Click(object sender, RoutedEventArgs e)
        {
            txtURL.Text = "http://www.google.de/";
            Navigate();
        }

        private void txtURL_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return ||
                e.Key == System.Windows.Input.Key.Enter)
            {
                Navigate();
            }
        }

        private void browser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            btnPreviousPage.IsEnabled = ((WebBrowser)sender).CanGoBack;
            btnNextPage.IsEnabled = ((WebBrowser)sender).CanGoForward;

            if (e.Uri != null)
            {
                string uriString = e.Uri.OriginalString;
                string source = string.Empty;

                try
                {
                    source = _WebClient.DownloadString(uriString);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Source code from " + uriString + " cannot be read.\n\n" + ex.Message);
                }

                txtURL.Text = uriString;
                txtMovieID.Text = string.Empty;
                txtMovieName.Text = string.Empty;
                txtSourceCode.Text = source;

                string[] uriStringParts = uriString.Split('/');
                foreach (string uriStringPart in uriStringParts)
                {
                    if (uriStringPart.StartsWith("tt") && uriStringPart.Length == 9)
                    {
                        txtMovieID.Text = uriStringPart;

                        if (!string.IsNullOrWhiteSpace(source))
                        {
                            int indexTitleStart = source.LastIndexOf("<title>") + 7;
                            int indexTitleEnd = source.LastIndexOf("</title>") - indexTitleStart;

                            string movieName = source.Substring(indexTitleStart, indexTitleEnd);
                            txtMovieName.Text = ReplaceSpecialSigns(movieName);
                        }

                        break;
                    }
                }
            }
        }

        #endregion
    }
}
