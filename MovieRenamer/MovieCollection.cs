using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using MovieRenamer.MVVM;
using MovieRenamer.MVVM.Commands;

namespace MovieRenamer
{
    public class MovieCollection : ObservableObject, IComparable
    {
        private string _imdbId = string.Empty;
        private string _imdbMovieName = string.Empty;
        private MovieCollectionDirectory _movieCollectionDirectory = new MovieCollectionDirectory();
        private ObservableCollection<MovieFile> _movies = new ObservableCollection<MovieFile>();

        private RelayCommand _openMovieFolderCommand;
        private RelayCommand _renameMovieCommand;
        private RelayCommand _renameMovieFolderCommand;
        private RelayCommand _reScanMovieCollectionCommand;
        private RelayCommand _resetMovieCollectionCommand;
        private RelayCommand _saveMovieCollectionCommand;
        private RelayCommand _searchMovieCommand;
        private RelayCommand _searchMovieFolderCommand;

        private string _searchString = string.Empty;
        private MovieFile _selectedMovie;

        public MovieCollectionDirectory MovieCollectionDirectory
        {
            get => _movieCollectionDirectory;
            set => SetProperty(ref _movieCollectionDirectory, value);
        }

        public ObservableCollection<MovieFile> Movies
        {
            get => _movies;
            set => SetProperty(ref _movies, value);
        }

        public MovieFile SelectedMovie
        {
            get => _selectedMovie;
            set => SetProperty(ref _selectedMovie, value);
        }

        public string SearchString
        {
            get => _searchString;
            set => SetProperty(ref _searchString, value);
        }

        public string ImdbId
        {
            get => _imdbId;
            set => SetProperty(ref _imdbId, value);
        }

        public string ImdbMovieName
        {
            get => _imdbMovieName;
            set => SetProperty(ref _imdbMovieName, value);
        }

        public ICommand OpenMovieFolderCommand
        {
            get
            {
                return _openMovieFolderCommand ?? (_openMovieFolderCommand =
                           new RelayCommand(param => OpenMovieFolder()));
            }
        }

        public ICommand ReScanMovieCollectionCommand
        {
            get
            {
                return _reScanMovieCollectionCommand ?? (_reScanMovieCollectionCommand =
                           new RelayCommand(param => Read(MovieCollectionDirectory.OriginalMovieCollectionDirectory)));
            }
        }

        public ICommand ResetMovieCollectionCommand
        {
            get
            {
                return _resetMovieCollectionCommand ?? (_resetMovieCollectionCommand =
                           new RelayCommand(param => Reset()));
            }
        }

        public ICommand SaveMovieCollectionCommand
        {
            get
            {
                return _saveMovieCollectionCommand ?? (_saveMovieCollectionCommand =
                           new RelayCommand(param => Write()));
            }
        }

        public ICommand SearchMovieFolderCommand
        {
            get
            {
                return _searchMovieFolderCommand ?? (_searchMovieFolderCommand =
                           new RelayCommand(param => SearchMovieFolder(), param => MovieCollectionDirectory != null));
            }
        }

        public ICommand SearchMovieCommand
        {
            get
            {
                return _searchMovieCommand ?? (_searchMovieCommand =
                           new RelayCommand(param => SearchMovie(), param => SelectedMovie != null));
            }
        }

        public ICommand RenameMovieFolderCommand
        {
            get
            {
                return _renameMovieFolderCommand ?? (_renameMovieFolderCommand =
                           new RelayCommand(param => SetMovieCollectionDirectoryName()));
            }
        }

        public ICommand RenameMovieCommand
        {
            get
            {
                return _renameMovieCommand ?? (_renameMovieCommand =
                           new RelayCommand(
                               param => SetSelectedMovieNewMovieName(),
                               param => SelectedMovie != null));
            }
        }

        public int CompareTo(object obj)
        {
            if (obj is MovieCollection movieCollection)
            {
                return string.Compare(
                    MovieCollectionDirectory.OriginalMovieCollectionName,
                    movieCollection.MovieCollectionDirectory.OriginalMovieCollectionName,
                    StringComparison.Ordinal);
            }

            throw new ArgumentException("Object is not a MovieCollection");

        }

        private void OpenMovieFolder()
        {
            Process.Start(MovieCollectionDirectory.OriginalMovieCollectionDirectory.FullName);
        }

        public void Read(DirectoryInfo movieCollectionDirectory)
        {
            MovieCollectionDirectory.Read(movieCollectionDirectory);

            if (movieCollectionDirectory.Exists)
            {
                Movies.Clear();

                foreach (var movieFile in movieCollectionDirectory.GetFiles())
                {
                    var movie = new MovieFile();
                    movie.Read(movieFile);
                    Movies.Add(movie);
                }

                Movies.BubbleSort();
            }

            if (Movies.Any())
            {
                SelectedMovie = Movies[0];
            }
        }

        private void Reset()
        {
            MovieCollectionDirectory.Reset();

            foreach (var movie in Movies)
            {
                movie.Reset();
            }
        }

        private void Write()
        {
            foreach (var movie in Movies)
            {
                movie.Write();
            }

            MovieCollectionDirectory.Write();

            Read(MovieCollectionDirectory.OriginalMovieCollectionDirectory);
        }

        private void SetImdbSearchString(string movieName)
        {
            var cleanedMovieName = RemoveBracketParts(movieName);

            var movieNameParts = cleanedMovieName.Split(' ', '.', '_');

            var chain = string.Empty;

            foreach (var movieNamePart in movieNameParts)
            {
                if (!string.IsNullOrWhiteSpace(movieNamePart) &&
                    !(movieNamePart.Length == 4 && int.TryParse(movieNamePart, out _)) &&
                    movieNamePart != "-")
                {
                    chain += movieNamePart + "+";
                }
            }

            SearchString = "http://www.imdb.de/find?q=" + chain.TrimEnd('+'); //+ "&s=all";
        }

        private static string RemoveBracketParts(string str)
        {
            while (str.Contains("(") && str.Contains(")"))
            {
                str = str.Remove(
                    str.IndexOf("(", StringComparison.Ordinal),
                    str.IndexOf(")", StringComparison.Ordinal) + 1 - str.IndexOf("(", StringComparison.Ordinal));
            }

            while (str.Contains("[") && str.Contains("]"))
            {
                str = str.Remove(
                    str.IndexOf("[", StringComparison.Ordinal),
                    str.IndexOf("]", StringComparison.Ordinal) + 1 - str.IndexOf("[", StringComparison.Ordinal));
            }

            return str;
        }

        private void SetSelectedMovieNewMovieName()
        {
            SelectedMovie.NewMovieName = ImdbMovieName + " [" + ImdbId + "]";
        }

        private void SetMovieCollectionDirectoryName()
        {
            var movieFolderName = RemoveBracketParts(ImdbMovieName).Trim();

            if (movieFolderName.Contains(" - "))
            {
                movieFolderName = movieFolderName.Remove(movieFolderName.IndexOf(" - ", StringComparison.Ordinal));
            }

            MovieCollectionDirectory.NewMovieCollectionName = movieFolderName;
        }

        private void SearchMovie()
        {
            var movieName = SelectedMovie.OriginalMovieName;
            SetImdbSearchString(movieName);
        }

        private void SearchMovieFolder()
        {
            var movieName = MovieCollectionDirectory.OriginalMovieCollectionName;
            SetImdbSearchString(movieName);
        }
    }
}