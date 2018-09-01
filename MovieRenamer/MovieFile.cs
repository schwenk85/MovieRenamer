using System;
using System.IO;
using System.Windows;
using MovieRenamer.MVVM;

namespace MovieRenamer
{
    public class MovieFile : ObservableObject, IComparable
    {
        private string _newMovieName = string.Empty;
        private FileInfo _originalMovieFile;
        private string _originalMovieName = string.Empty;

        public FileInfo OriginalMovieFile
        {
            get => _originalMovieFile;
            set => SetProperty(ref _originalMovieFile, value);
        }

        public string OriginalMovieName
        {
            get => _originalMovieName;
            set => SetProperty(ref _originalMovieName, value);
        }

        public string NewMovieName
        {
            get => _newMovieName;
            set => SetProperty(ref _newMovieName, value.RemoveBadCharacters());
        }

        public int CompareTo(object obj)
        {
            if (!(obj is MovieFile movieFile))
            {
                throw new ArgumentException("Object is not a MovieFile");
            }

            return string.Compare(OriginalMovieName, movieFile.OriginalMovieName, StringComparison.Ordinal);
        }

        public void Read(FileInfo movie)
        {
            OriginalMovieFile = movie;

            Reset();
        }

        public void Reset()
        {
            var movieNameLength = OriginalMovieFile.Name.Length - OriginalMovieFile.Extension.Length;
            var movieName = OriginalMovieFile.Name.Remove(movieNameLength);

            OriginalMovieName = movieName;
            NewMovieName = movieName;
        }

        public void Write()
        {
            if (OriginalMovieFile == null || !OriginalMovieFile.Exists)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(NewMovieName) || NewMovieName == OriginalMovieName)
            {
                return;
            }

            try
            {
                OriginalMovieFile.MoveTo(
                    _originalMovieFile.DirectoryName + @"\" + _newMovieName + _originalMovieFile.Extension);
                Reset();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Filename '{_newMovieName}' cannot be written.\n\n" + ex.Message);
            }
        }
    }
}