using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace MovieRenamer
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MovieRenamerViewModel viewModel = new MovieRenamerViewModel();
            MovieRenamerWindow movieRenamer = new MovieRenamerWindow();
            movieRenamer.DataContext = viewModel;
            movieRenamer.ShowDialog();
            this.Shutdown();


            //TODO: Wishlist
            // - Fensterposition, Gridsplitterpositionen, Textboxinhalte usw. speichern/wiederherstellen

        }
    }
}
