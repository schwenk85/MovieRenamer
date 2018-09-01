using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using MovieRenamer.MVVM;
using MovieRenamer.MVVM.Commands;

namespace MovieRenamer
{
    public class MovieRenamerViewModel : ObservableObject
    {
        private ObservableCollection<MovieCollection> _movieCollections =
            new ObservableCollection<MovieCollection>();
        private RelayCommand _setNextMovieCollectionCommand;
        private RelayCommand _setPreviousMovieCollectionCommand;
        private string _moviesFolder = @"Z:\Movies\MKV";
        private RelayCommand _moviesFolderOpenCommand;
        private RelayCommand _moviesFolderScanCommand;
        private MovieCollection _selectedMovieCollection;

        public string MoviesFolder
        {
            get => _moviesFolder;
            set => SetProperty(ref _moviesFolder, value);
        }

        public ObservableCollection<MovieCollection> MovieCollections
        {
            get => _movieCollections;
            set => SetProperty(ref _movieCollections, value);
        }

        public MovieCollection SelectedMovieCollection
        {
            get => _selectedMovieCollection;
            set => SetProperty(ref _selectedMovieCollection, value);
        }

        public ICommand MoviesFolderOpenCommand
        {
            get
            {
                return _moviesFolderOpenCommand ?? (_moviesFolderOpenCommand =
                           new RelayCommand(param => GetMoviesFolderPath()));
            }
        }

        public ICommand MoviesFolderScanCommand
        {
            get
            {
                return _moviesFolderScanCommand ?? (_moviesFolderScanCommand =
                           new RelayCommand(
                               param => ReadMovieCollections(),
                               param => Directory.Exists(MoviesFolder)));
            }
        }

        public ICommand SetPreviousMovieCollectionCommand
        {
            get
            {
                return _setPreviousMovieCollectionCommand ?? (_setPreviousMovieCollectionCommand =
                           new RelayCommand(
                               param => SetPreviousMovieCollection(),
                               param => MovieCollections.Any()));
            }
        }

        public ICommand SetNextMovieCollectionCommand
        {
            get
            {
                return _setNextMovieCollectionCommand ?? (_setNextMovieCollectionCommand =
                           new RelayCommand(
                               param => SetNextMovieCollection(),
                               param => MovieCollections.Any()));
            }
        }
        
        private void GetMoviesFolderPath()
        {
            var dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                MoviesFolder = dialog.SelectedPath;
            }
        }

        private void ReadMovieCollections()
        {
            var moviesDirectory = new DirectoryInfo(MoviesFolder);

            if (moviesDirectory.Exists)
            {
                MovieCollections.Clear();

                var movieCollectionDirectories = moviesDirectory.GetDirectories();
                foreach (var movieCollectionDirectory in movieCollectionDirectories)
                {
                    var movieCollection = new MovieCollection();
                    movieCollection.Read(movieCollectionDirectory);
                    MovieCollections.Add(movieCollection);
                }

                MovieCollections.BubbleSort();
            }

            RaisePropertyChangedEvent("MovieCollections");

            if (MovieCollections.Any())
            {
                SelectedMovieCollection = MovieCollections[0];
            }
        }

        private void SetPreviousMovieCollection()
        {
            SelectedMovieCollection = MovieCollections.GetPrevious(SelectedMovieCollection);
        }

        private void SetNextMovieCollection()
        {
            SelectedMovieCollection = MovieCollections.GetNext(SelectedMovieCollection);
        }
    }
}