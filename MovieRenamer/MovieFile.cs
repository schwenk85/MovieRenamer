using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MovieRenamer.MVVM;
using System.IO;
using System.Text.RegularExpressions;

namespace MovieRenamer
{
    public class MovieFile : ObservableObject, IComparable
    {
        #region Fields

        protected FileInfo _OriginalMovieFile;
        protected string _OriginalMovieName;
        protected string _NewMovieName;

        #endregion
        
        #region Constructors

        public MovieFile(MovieFiles parent)
        {
            Parent = parent;

            _OriginalMovieFile = null;
            _OriginalMovieName = string.Empty;
            _NewMovieName = string.Empty;
        }

        #endregion

        #region Data Properties

        public MovieFiles Parent { get; set; }

        public FileInfo OriginalMovieFile
        {
            get { return _OriginalMovieFile; }
            set
            {
                _OriginalMovieFile = value;
                base.RaisePropertyChangedEvent("OriginalMovieFile");
            }
        }

        public string OriginalMovieName
        {
            get { return _OriginalMovieName; }
            set
            {
                _OriginalMovieName = value;
                base.RaisePropertyChangedEvent("OriginalMovieName");
            }
        }

        public string NewMovieName
        {
            get { return _NewMovieName; }
            set
            {
                _NewMovieName = value.RemoveBadCharacters();
                base.RaisePropertyChangedEvent("NewMovieName");
            }
        }

        #endregion

        #region Methods

        public void Read(FileInfo movie)
        {
            OriginalMovieFile = movie;

            Reset();
        }

        public void Reset()
        {
            int movieNameLength = OriginalMovieFile.Name.Length - OriginalMovieFile.Extension.Length;
            string movieName = OriginalMovieFile.Name.Remove(movieNameLength);

            OriginalMovieName = movieName;
            NewMovieName = movieName;
        }

        public void Write()
        {
            if (OriginalMovieFile != null && 
                OriginalMovieFile.Exists)
            {
                if (!string.IsNullOrWhiteSpace(NewMovieName) && 
                    NewMovieName != OriginalMovieName)
                {
                    try
                    { 
                        OriginalMovieFile.MoveTo(_OriginalMovieFile.DirectoryName + "\\" + _NewMovieName + _OriginalMovieFile.Extension);
                        Reset();
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show("Filename " + _NewMovieName + " cannot be written.\n\n" + ex.Message);
                    }
                }
            }
        }

        #endregion

        #region Compare

        public int CompareTo(object obj)
        {
            MovieFile movieFile = obj as MovieFile;
            if (movieFile == null)
            {
                throw new ArgumentException("Object is not MovieFile");
            }
            return this.OriginalMovieName.CompareTo(movieFile.OriginalMovieName);
        }

        #endregion
    }
}
