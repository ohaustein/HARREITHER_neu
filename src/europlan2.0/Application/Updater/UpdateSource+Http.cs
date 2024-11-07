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

        private readonly String DownloadUrlTemplate;
        private readonly String VersionsUrl;


        internal UpdateSourceHttp(String caption, String versionsUrl, String downloadUrlTemplate) : base(caption)
        {
            DownloadUrlTemplate = downloadUrlTemplate;
            VersionsUrl = versionsUrl;
        }


        protected override void DownloadUpdateFromSource(String filename, String destinationFilePath, Action<Int64, Int64> progressReporter)
        {
            using (var webClient = new WebClient())
            {
                var uri = new Uri(String.Format(DownloadUrlTemplate, filename));
                webClient.DownloadProgressChanged +=
                    delegate (Object sender, DownloadProgressChangedEventArgs e)
                    {
                        progressReporter.Invoke(e.BytesReceived, e.TotalBytesToReceive);
                    };
                var downloadTask = webClient.DownloadFileTaskAsync(uri, destinationFilePath);
                downloadTask.Wait();
            }
        }

        protected override IEnumerable<String> GetAvailableFilenames()
        {
            try
            {
                using (var webClient = new WebClient())
                {
                    var uri = new Uri(VersionsUrl);
                    var versionsString = webClient.DownloadString(uri);
                    var versions = versionsString.Split(new String[] { Environment.NewLine, "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
                    return versions.AsEnumerable();
                }
            }
            catch
            {
                return new String[] { };
            }
        }


    }
}
