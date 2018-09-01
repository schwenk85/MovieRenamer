using System.Windows;

namespace MovieRenamer
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            var movieRenamer = new MovieRenamerWindow
            {
                DataContext = new MovieRenamerViewModel()
            };
            movieRenamer.ShowDialog();
            Shutdown();
        }
    }
}