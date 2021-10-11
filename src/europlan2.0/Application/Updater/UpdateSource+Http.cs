using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Europlan.Application
{
    public class UpdateSourceHttp : UpdateSource
    {


        private const String FILENAME_VERSION_INFO = "versions";


        public static UpdateSourceHttp Load(BinaryReader binaryReader)
        {
            var caption = binaryReader.ReadDecodedString();
            var url = binaryReader.ReadDecodedString();
            return new UpdateSourceHttp(caption, url);
        }


        private String URL { get; set; }


        public UpdateSourceHttp(String caption, String url)
            : base(caption)
        {
            URL = url;
        }


        protected override void DownloadUpdateFromSource(String filename, String destinationFilePath, Action<Int64, Int64> progressReporter)
        {
            using (var webClient = new WebClient())
            {
                var uri = new Uri(URL);
                var address = new Uri(uri, filename);
                webClient.DownloadProgressChanged +=
                    delegate(Object sender, DownloadProgressChangedEventArgs e)
                    {
                        progressReporter.Invoke(e.BytesReceived, e.TotalBytesToReceive);
                    };
                var downloadTask = webClient.DownloadFileTaskAsync(address, destinationFilePath);
                downloadTask.Wait();
            }
        }

        protected override IEnumerable<String> GetAvailableFilenames()
        {
            try
            {
                using (var webClient = new WebClient())
                {
                    var uri = new Uri(URL);
                    var address = new Uri(uri, FILENAME_VERSION_INFO);
                    var versionsString = webClient.DownloadString(address);
                    var versions = versionsString.Split(new String[] { Environment.NewLine, "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
                    return versions.AsEnumerable();
                }
            }
            catch
            {
                return new String[] { };
            }
        }

        protected internal override void Save(BinaryWriter binaryWriter)
        {
            binaryWriter.Write('H');
            binaryWriter.WriteEncodedString(Caption);
            binaryWriter.WriteEncodedString(URL);
        }


    }
}
