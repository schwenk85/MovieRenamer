using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MovieRenamer.MVVM;
using System.IO;
using System.Windows.Input;
using MovieRenamer.MVVM.Commands;
using System.Diagnostics;

namespace MovieRenamer
{
    public class MovieCollection : ObservableObject, IComparable
    {
        #region Fields

        protected MovieCollectionDirectory _MovieCollectionDirectory;
        protected MovieFiles _Movies;
        protected MovieFile _SelectedMovie;

        protected string _SearchString;
        protected string _ImdbId;
        protected string _ImdbMovieName;

        #endregion
        
        #region Constructors

        public MovieCollection(MovieCollections parent)
        {
            Parent = parent;

            _MovieCollectionDirectory = new MovieCollectionDirectory(this);
            _Movies = new MovieFiles(this);
            _SelectedMovie = null;

            _SearchString = string.Empty;
            _ImdbId = string.Empty;
            _ImdbMovieName = string.Empty;

            MovieCollectionOpen = new MovieCollection_OpenCommand(this);
            MovieCollectionReScan = new MovieCollection_ReScanCommand(this);
            MovieCollectionReset = new MovieCollection_ResetCommand(this);
            MovieCollectionSave = new MovieCollection_SaveCommand(this);
            SearchMovieFolder = new MovieCollection_SearchMovieFolderCommand(this);
            SearchMovie = new MovieCollection_SearchMovieCommand(this);
            RenameMovieFolder = new MovieCollection_RenameMovieFolderCommand(this);
            RenameMovie = new MovieCollection_RenameMovieCommand(this);
        }

        #endregion

        #region Data Properties

        public MovieCollections Parent { get; set; }

        public MovieCollectionDirectory MovieCollectionDirectory
        {
            get { return _MovieCollectionDirectory; }
            set
            {
                _MovieCollectionDirectory = value;
                base.RaisePropertyChangedEvent("MovieCollectionDirectory");
            }
        }

        public MovieFiles Movies
        {
            get { return _Movies; }
            set
            {
                _Movies = value;
                base.RaisePropertyChangedEvent("Movies");
            }
        }

        public MovieFile SelectedMovie
        {
            get { return _SelectedMovie; }
            set
            {
                _SelectedMovie = value;
                //SetImdbSearchString();
                base.RaisePropertyChangedEvent("SelectedMovie");
            }
        }

        public string SearchString
        {
            get { return _SearchString; }
            set
            {
                _SearchString = value;
                base.RaisePropertyChangedEvent("SearchString");
            }
        }

        public string ImdbId
        {
            get { return _ImdbId; }
            set
            {
                _ImdbId = value;
                base.RaisePropertyChangedEvent("ImdbId");
            }
        }

        public string ImdbMovieName
        {
            get { return _ImdbMovieName; }
            set
            {
                _ImdbMovieName = value;
                base.RaisePropertyChangedEvent("ImdbMovieName");
            }
        }

        #endregion

        #region Command Properties

        public ICommand MovieCollectionOpen { get; set; }
        public ICommand MovieCollectionReScan { get; set; }
        public ICommand MovieCollectionReset { get; set; }
        public ICommand MovieCollectionSave { get; set; }
        public ICommand SearchMovieFolder { get; set; }
        public ICommand SearchMovie { get; set; }
        public ICommand RenameMovieFolder { get; set; }
        public ICommand RenameMovie { get; set; }

        #endregion

        #region Methods

        public void Read(DirectoryInfo movieCollectionDirectory)
        {
            MovieCollectionDirectory.Read(movieCollectionDirectory);
            Movies.Read(movieCollectionDirectory);

            if (Movies.Count > 0)
                SelectedMovie = Movies[0];
        }

        public void Reset()
        {
            MovieCollectionDirectory.Reset();
            Movies.Reset();
        }

        public void Write()
        {
            Movies.Write();
            MovieCollectionDirectory.Write();

            this.Read(MovieCollectionDirectory.OriginalMovieCollectionDirectory);
        }

        public void Open()
        {
            Process.Start(MovieCollectionDirectory.OriginalMovieCollectionDirectory.FullName);
        }

        public void SetImdbSearchString(string movieName)
        {
            string cleanedMovieName = RemoveBracketParts(movieName);

            string[] movieNameParts = cleanedMovieName.Split(new char[] { ' ', '.', '_' });

            string chain = string.Empty;

            int intTemp = 0;

            foreach (string movieNamePart in movieNameParts)
            {
                if (!string.IsNullOrWhiteSpace(movieNamePart) && 
                    !(movieNamePart.Length == 4 && Int32.TryParse(movieNamePart, out intTemp)) &&
                    movieNamePart != "-")
                    chain += movieNamePart + "+";
            }

            SearchString = "http://www.imdb.de/find?q=" + chain.TrimEnd(new char[] { '+' }); //+ "&s=all";
        }

        private string RemoveBracketParts(string str)
        {
            while (str.Contains("(") && str.Contains(")"))
                str = str.Remove(str.IndexOf("("), str.IndexOf(")") + 1 - str.IndexOf("("));

            while (str.Contains("[") && str.Contains("]"))
                str = str.Remove(str.IndexOf("["), str.IndexOf("]") + 1 - str.IndexOf("["));

            return str;
        }

        public void SetSelectedMovieNewMovieName()
        {
            SelectedMovie.NewMovieName = ImdbMovieName + " [" + ImdbId + "]";
        }

        public void SetMovieCollectionDirectoryName()
        {
            string movieFolderName = RemoveBracketParts(ImdbMovieName).Trim();

            if (movieFolderName.Contains(" - "))
                movieFolderName = movieFolderName.Remove(movieFolderName.IndexOf(" - "));

            MovieCollectionDirectory.NewMovieCollectionName = movieFolderName;
        }

        #endregion

        #region Compare

        public int CompareTo(object obj)
        {
            MovieCollection movieCollection = obj as MovieCollection;
            if (movieCollection == null)
            {
                throw new ArgumentException("Object is not MovieCollection");
            }
            return this.MovieCollectionDirectory.OriginalMovieCollectionName.CompareTo(movieCollection.MovieCollectionDirectory.OriginalMovieCollectionName);
        }

        #endregion
    }
}