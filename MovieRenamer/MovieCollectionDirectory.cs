using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MovieRenamer.MVVM;
using System.IO;
using System.Text.RegularExpressions;

namespace MovieRenamer
{
    public class MovieCollectionDirectory : ObservableObject
    {
        #region Fields

        protected DirectoryInfo _OriginalMovieCollectionDirectory;
        protected string _OriginalMovieCollectionName;
        protected string _NewMovieCollectionName;

        #endregion
        
        #region Constructors

        public MovieCollectionDirectory(MovieCollection parent)
        {
            Parent = parent;

            _OriginalMovieCollectionDirectory = null;
            _OriginalMovieCollectionName = string.Empty;
            _NewMovieCollectionName = string.Empty;
        }

        #endregion

        #region Data Properties

        public MovieCollection Parent { get; set; }

        public DirectoryInfo OriginalMovieCollectionDirectory
        {
            get { return _OriginalMovieCollectionDirectory; }
            set
            {
                _OriginalMovieCollectionDirectory = value;
                base.RaisePropertyChangedEvent("OriginalMovieCollectionDirectory");
            }
        }

        public string OriginalMovieCollectionName
        {
            get { return _OriginalMovieCollectionName; }
            set
            {
                _OriginalMovieCollectionName = value;
                base.RaisePropertyChangedEvent("OriginalMovieCollectionName");
            }
        }

        public string NewMovieCollectionName
        {
            get { return _NewMovieCollectionName; }
            set
            {
                _NewMovieCollectionName = value.RemoveBadCharacters();
                base.RaisePropertyChangedEvent("NewMovieCollectionName");
            }
        }

        #endregion

        #region Methods

        public void Read(DirectoryInfo movieCollectionDirectory)
        {
            OriginalMovieCollectionDirectory = movieCollectionDirectory;

            Reset();
        }

        public void Reset()
        {
            OriginalMovieCollectionName = OriginalMovieCollectionDirectory.Name;
            NewMovieCollectionName = OriginalMovieCollectionDirectory.Name;
        }

        public void Write()
        {
            if (OriginalMovieCollectionDirectory != null && 
                OriginalMovieCollectionDirectory.Exists)
            {
                _NewMovieCollectionName = _NewMovieCollectionName.TrimEnd(new char[] { '.' });

                if (!string.IsNullOrWhiteSpace(NewMovieCollectionName) &&
                    NewMovieCollectionName != OriginalMovieCollectionName)
                {
                    try
                    {
                        OriginalMovieCollectionDirectory.MoveTo(_OriginalMovieCollectionDirectory.Parent.FullName + "\\" + _NewMovieCollectionName);
                        Reset();
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show("Foldername " + _NewMovieCollectionName + " cannot be written.\n\n" + ex.Message);
                    }
                }
            }
        }

        #endregion
    }
}
