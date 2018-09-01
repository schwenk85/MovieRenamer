using System;
using System.Windows;
using System.Windows.Controls;

namespace MovieRenamer
{
    /// <summary>
    ///     Source: http://stackoverflow.com/questions/263551/databind-the-source-property-of-the-webbrowser-in-wpf
    /// </summary>
    public static class WebBrowserUtility
    {
        public static readonly DependencyProperty BindableSourceProperty =
            DependencyProperty.RegisterAttached(
                "BindableSource",
                typeof(string),
                typeof(WebBrowserUtility),
                new UIPropertyMetadata(null, BindableSourcePropertyChanged));

        public static string GetBindableSource(DependencyObject obj)
        {
            return (string) obj.GetValue(BindableSourceProperty);
        }

        public static void SetBindableSource(DependencyObject obj, string value)
        {
            obj.SetValue(BindableSourceProperty, value);
        }

        public static void BindableSourcePropertyChanged(
            DependencyObject @object,
            DependencyPropertyChangedEventArgs eventArgs)
        {
            if (@object is WebBrowser browser)
            {
                var uri = eventArgs.NewValue as string;
                browser.Source = string.IsNullOrEmpty(uri) ? null : new Uri(uri);
            }
        }
    }
}