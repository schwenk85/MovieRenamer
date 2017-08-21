using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MovieRenamer.MVVM;
using System.IO;
using System.Windows.Input;
using MovieRenamer.MVVM.Commands;

namespace MovieRenamer
{
    public class MovieRenamerViewModel : ViewModelBase
    {
        #region Fields

        protected string _MoviesFolder;
        protected MovieCollections _MovieCollections;
        protected MovieCollection _SelectedMovieCollection;

        #endregion

        #region Constructors

        public MovieRenamerViewModel()
        {
            //_MoviesFolder = string.Empty;
            _MoviesFolder = "Z:\\Filme.deutsch\\AVI";
            _MovieCollections = new MovieCollections(this);
            _SelectedMovieCollection = null;

            MoviesFolderOpen = new MoviesFolder_OpenCommand(this);
            MoviesFolderScan = new MoviesFolder_ScanCommand(this);

            MovieCollectionsPrevious = new MovieCollections_PreviousCommand(this);
            MovieCollectionsNext = new MovieCollections_NextCommand(this);
        }

        #endregion

        #region Data Properties

        public string MoviesFolder
        {
            get { return _MoviesFolder; }
            set
            {
                _MoviesFolder = value;
                base.RaisePropertyChangedEvent("MoviesFolder");
            }
        }

        public MovieCollections MovieCollections
        {
            get { return _MovieCollections; }
            set
            {
                _MovieCollections = value;
                base.RaisePropertyChangedEvent("MovieCollections");
            }
        }

        public MovieCollection SelectedMovieCollection
        {
            get { return _SelectedMovieCollection; }
            set
            {
                _SelectedMovieCollection = value;
                base.RaisePropertyChangedEvent("SelectedMovieCollection");
            }
        }

        #endregion

        #region Command Properties

        public ICommand MoviesFolderOpen { get; set; }
        public ICommand MoviesFolderScan { get; set; }

        public ICommand MovieCollectionsPrevious { get; set; }
        public ICommand MovieCollectionsNext { get; set; }

        #endregion

        #region Methods

        public void ReadMovieCollections()
        {
            MovieCollections.Read(new DirectoryInfo(MoviesFolder));
            RaisePropertyChangedEvent("MovieCollections");

            if (MovieCollections.Count > 0)
                SelectedMovieCollection = MovieCollections[0];
        }

        public void SetNextMovieCollection()
        {
            int index = MovieCollections.IndexOf(SelectedMovieCollection);

            if (index < 0)
                SelectedMovieCollection = null;
            else if (index == MovieCollections.Count - 1)
                SelectedMovieCollection = MovieCollections[0];
            else
                SelectedMovieCollection = MovieCollections[index + 1];
        }

        public void SetPreviousMovieCollection()
        {
            int index = MovieCollections.IndexOf(SelectedMovieCollection);

            if (index < 0)
                SelectedMovieCollection = null;
            else if (index == 0)
                SelectedMovieCollection = MovieCollections[MovieCollections.Count - 1];
            else
                SelectedMovieCollection = MovieCollections[index - 1];
        }

        #endregion
    }
}