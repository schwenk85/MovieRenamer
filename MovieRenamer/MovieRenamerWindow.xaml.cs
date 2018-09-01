using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace MovieRenamer
{
    public partial class MovieRenamerWindow
    {
        private readonly WebClient _webClient = new WebClient();

        public MovieRenamerWindow()
        {
            InitializeComponent();
        }

        private void Navigate()
        {
            // Get URI to navigate to
            var uri = new Uri(TextBoxUrl.Text, UriKind.RelativeOrAbsolute);

            // Only absolute URIs can be navigated to
            if (!uri.IsAbsoluteUri)
            {
                MessageBox.Show("The Address URI must be absolute eg 'http://www.imdb.com/'");
            }
            else
            {
                WebBrowser.Navigate(uri);
            }
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TextBoxMainFolder.Focus();
        }

        private void ButtonPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (WebBrowser.CanGoBack)
            {
                WebBrowser.GoBack();
            }
        }

        private void ButtonNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (WebBrowser.CanGoForward)
            {
                WebBrowser.GoForward();
            }
        }

        private void ButtonOpenUri_Click(object sender, RoutedEventArgs e)
        {
            Navigate();
        }

        private void ButtonOpenImdbDe_Click(object sender, RoutedEventArgs e)
        {
            TextBoxUrl.Text = "http://www.imdb.de/";
            Navigate();
        }

        private void ButtonOpenImdbCom_Click(object sender, RoutedEventArgs e)
        {
            TextBoxUrl.Text = "http://www.imdb.com/";
            Navigate();
        }

        private void ButtonOpenGoogle_Click(object sender, RoutedEventArgs e)
        {
            TextBoxUrl.Text = "http://www.google.de/";
            Navigate();
        }

        private void TextBoxUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return ||
                e.Key == Key.Enter)
            {
                Navigate();
            }
        }

        private void WebBrowser_Navigated(
            object sender,
            NavigationEventArgs eventArgs)
        {
            ButtonPreviousPage.IsEnabled = ((WebBrowser) sender).CanGoBack;
            ButtonNextPage.IsEnabled = ((WebBrowser) sender).CanGoForward;

            if (eventArgs.Uri != null)
            {
                var uriString = eventArgs.Uri.OriginalString;
                var source = string.Empty;

                try
                {
                    source = _webClient.DownloadString(uriString);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Source code from " + uriString + " cannot be read.\n\n" + ex.Message);
                }

                TextBoxUrl.Text = uriString;
                TextBoxMovieId.Text = string.Empty;
                TextBoxMovieName.Text = string.Empty;
                TextBoxSourceCode.Text = source;

                var uriStringParts = uriString.Split('/');
                foreach (var uriStringPart in uriStringParts)
                {
                    if (uriStringPart.StartsWith("tt") && uriStringPart.Length == 9)
                    {
                        TextBoxMovieId.Text = uriStringPart;

                        if (!string.IsNullOrWhiteSpace(source))
                        {
                            var indexTitleStart = source.LastIndexOf("<title>", StringComparison.Ordinal) + 7;
                            var indexTitleEnd = source.LastIndexOf("</title>", StringComparison.Ordinal) -
                                                indexTitleStart;

                            var movieName = source.Substring(indexTitleStart, indexTitleEnd);
                            TextBoxMovieName.Text = ReplaceSpecialSigns(movieName);
                        }

                        break;
                    }
                }
            }
        }
    }
}