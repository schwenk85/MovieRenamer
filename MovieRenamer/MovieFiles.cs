using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;

namespace MovieRenamer
{
    public class MovieFiles : ObservableCollection<MovieFile>
    {
        #region Constructors

        public MovieFiles(MovieCollection parent)
        {
            Parent = parent;
        }

        #endregion

        #region Properties

        public MovieCollection Parent { get; set; }

        #endregion

        #region Methods

        public void Read(DirectoryInfo moviesFolder)
        {
            if (moviesFolder.Exists)
            {
                this.Clear();

                foreach (FileInfo movie in moviesFolder.GetFiles())
                {
                    MovieFile movieFile = new MovieFile(this);
                    movieFile.Read(movie);
                    this.Add(movieFile);
                }

                this.BubbleSort();
            }
        }

        public void Reset()
        {
            foreach (MovieFile movieFile in this)
                movieFile.Reset();
        }

        public void Write()
        {
            foreach (MovieFile movieFile in this)
                movieFile.Write();
        }

        #endregion
    }
}
