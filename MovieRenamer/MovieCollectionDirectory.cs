using System;
using System.IO;
using System.Windows;
using MovieRenamer.MVVM;

namespace MovieRenamer
{
    public class MovieCollectionDirectory : ObservableObject
    {
        private string _newMovieCollectionName = string.Empty;
        private DirectoryInfo _originalMovieCollectionDirectory;
        private string _originalMovieCollectionName = string.Empty;

        public DirectoryInfo OriginalMovieCollectionDirectory
        {
            get => _originalMovieCollectionDirectory;
            set => SetProperty(ref _originalMovieCollectionDirectory, value);
        }

        public string OriginalMovieCollectionName
        {
            get => _originalMovieCollectionName;
            set => SetProperty(ref _originalMovieCollectionName, value);
        }

        public string NewMovieCollectionName
        {
            get => _newMovieCollectionName;
            set => SetProperty(ref _newMovieCollectionName, value.RemoveBadCharacters());
        }

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
            if (OriginalMovieCollectionDirectory == null || !OriginalMovieCollectionDirectory.Exists)
            {
                return;
            }

            _newMovieCollectionName = _newMovieCollectionName.TrimEnd('.');

            if (string.IsNullOrWhiteSpace(NewMovieCollectionName) ||
                NewMovieCollectionName == OriginalMovieCollectionName)
            {
                return;
            }

            try
            {
                if (_originalMovieCollectionDirectory.Parent != null)
                {
                    OriginalMovieCollectionDirectory.MoveTo(
                        _originalMovieCollectionDirectory.Parent.FullName + @"\" + _newMovieCollectionName);
                }

                Reset();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Folder Name '{_newMovieCollectionName}' cannot be written.\n\n" + ex.Message);
            }
        }
    }
}