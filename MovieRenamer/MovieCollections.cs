using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;

namespace MovieRenamer
{
    public class MovieCollections : ObservableCollection<MovieCollection>
    {
        #region Constructors

        public MovieCollections(MovieRenamerViewModel parent)
        {
            Parent = parent;
        }

        #endregion

        #region Properties

        public MovieRenamerViewModel Parent { get; set; }

        #endregion

        #region Methods

        public void Read(DirectoryInfo movieCollectionsFolder)
        {
            if (movieCollectionsFolder.Exists)
            {
                this.Clear();

                foreach (DirectoryInfo movieCollectionDir in movieCollectionsFolder.GetDirectories())
                {
                    MovieCollection movieCollection = new MovieCollection(this);
                    movieCollection.Read(movieCollectionDir);
                    this.Add(movieCollection);
                }

                this.BubbleSort();
            }
        }

        #endregion
    }
}