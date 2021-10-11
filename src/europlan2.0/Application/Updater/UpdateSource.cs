using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Europlan.Application
{
    public abstract partial class UpdateSource
    {


        private const Int32 MAX_REPEAT_COUNTER = 3;


        public String Caption { get; private set; }


        protected UpdateSource(String caption)
        {
            Caption = caption;
        }


        internal Downloader CreateDownloader(String filename, String destinationFilePath)
        {
            if (ReferenceEquals(filename, null)) throw new ArgumentNullException("filename"); // TODO nameof
            if (ReferenceEquals(destinationFilePath, null)) throw new ArgumentNullException("destinationFilePath"); // TODO nameof

            var downloader = new Downloader(this, filename, destinationFilePath, DownloadUpdateFromSource);
            return downloader;
        }

        protected Version CreateVersionFromFilename(String filename)
        {
            var extension = Path.GetExtension(filename);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filename);
            try
            {
                Version version;
                if (Version.TryParse(fileNameWithoutExtension, out version))
                {
                    return version;
                }
                else
                {
                    return null;
                }
            }
            catch (Version.VersionParsingException) { return null; }
        }

        protected abstract void DownloadUpdateFromSource(String filename, String destinationFilePath, Action<Int64, Int64> progressReporter);

        protected abstract IEnumerable<String> GetAvailableFilenames();

        internal IEnumerable<Tuple<Version, String>> GetAvailableUpdates()
        {
            var filenameArray = GetAvailableFilenames().ToArray();
            var updateList = new List<Tuple<Version, String>>();
            foreach (var filename in filenameArray)
            {
                var version = CreateVersionFromFilename(filename);
                if (ReferenceEquals(version, null)) continue;
                updateList.Add(Tuple.Create(version, filename));
            }
            return updateList.AsEnumerable();
        }

        internal protected abstract void Save(BinaryWriter binaryWriter);


    }
}
