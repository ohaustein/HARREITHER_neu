using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Europlan.Application
{
    internal partial class Controller
    {


        internal static IEnumerable<DownloadResult> RunDownload(IEnumerable<CheckResult> checkResultCollection)
        {
            var task = Task.Run(
                    delegate ()
                    {
                        var resultList = new List<DownloadResult>();
                        try
                        {
                            Parallel.ForEach(checkResultCollection,
                                delegate (CheckResult checkResult)
                                {
                                    var version = checkResult.Version;
                                    var resultItem = new DownloadResult(version);
                                    foreach (var updateSourceFilenameTuple in checkResult.UpdateSourceFileNameCollection)
                                    {
                                        var updateSource = updateSourceFilenameTuple.Item1;
                                        var filename = updateSourceFilenameTuple.Item2;
                                        try
                                        {
                                            var localFilePath = FileSystemHelper.CreateTempFilePath();
                                            var downloader = updateSource.CreateDownloader(filename, localFilePath);
                                            var result = downloader.Download();
                                            if (result) resultItem.SetSuccess(localFilePath);
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            resultItem.AppendError(updateSource, filename, ex);
                                        }
                                    }
                                    resultList.Add(resultItem);
                                }
                            );
                        }
                        catch { throw; }
                        return resultList.ToArray();
                    }
                     );
            task.Wait();
            var downloadResultArray = task.Result;
            return downloadResultArray;
        }


        public class DownloadResult
        {

            private List<Tuple<UpdateSource, String, Exception>> ExceptionList;

            public String LocalFilePath { get; private set; }

            public Version Version { get; private set; }

            public Boolean? WasSuccessful { get; private set; }

            public DownloadResult(Version version)
            {
                ExceptionList = new List<Tuple<UpdateSource, String, Exception>>();
                Version = version;
            }

            public void AppendError(UpdateSource updateSource, String filename, Exception exception)
            {
                ExceptionList.Add(Tuple.Create(updateSource, filename, exception));
            }

            public void SetSuccess(String localFilePath)
            {
                LocalFilePath = localFilePath;
                WasSuccessful = true;
            }

        }


    }
}
