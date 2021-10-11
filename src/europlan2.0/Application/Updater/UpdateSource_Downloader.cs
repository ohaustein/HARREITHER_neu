using System;
using System.IO;

namespace Europlan.Application
{
    public abstract partial class UpdateSource
    {
        public class Downloader
        {


            private String DestinationFilePath;
            private Action<String, String, Action<Int64, Int64>> DownloadMethod;
            private String Filename;
            private UpdateSource UpdateSource;


            internal Downloader(UpdateSource updateSource, String filename, String destinationFilePath, Action<String, String, Action<Int64, Int64>> downloadMethod)
            {
                DestinationFilePath = destinationFilePath;
                DownloadMethod = downloadMethod;
                Filename = filename;
                UpdateSource = updateSource;
            }


            internal Boolean Download()
            {
                RaiseDownloadStart();
                for (var repeatCounter = 1; repeatCounter <= MAX_REPEAT_COUNTER; repeatCounter++)
                {
                    try
                    {
                        try
                        {
                            DownloadMethod.Invoke(Filename, DestinationFilePath, (bytesCount, bytesTotal) => RaiseDownloadProgress(bytesCount, bytesTotal, repeatCounter, MAX_REPEAT_COUNTER));
                            RaiseDownloadFinished();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                File.Delete(DestinationFilePath);
                            }
                            catch { }
                            RaiseDownloadError(UpdateSource, Filename, DestinationFilePath, repeatCounter, MAX_REPEAT_COUNTER, ex);
                            throw;
                        }
                    }
                    catch (IOException) { continue; }
                    catch { break; }
                }
                return false;
            }


            public class __ev1 : EventArgs // TODo rename
            {

                public String DestinationFilePath { get; private set; }

                public String Filename { get; private set; }

                public UpdateSource UpdateSource { get; private set; }


                public __ev1(UpdateSource updateSource, String filename, String destinationFilePath)
                {
                    DestinationFilePath = destinationFilePath;
                    Filename = filename;
                    UpdateSource = updateSource;
                }

            }

            public class __ev2 : __ev1 // TODo rename
            {

                public Int32 RepeatCount { get; private set; }

                public Int32 RepeatTotal { get; private set; }


                public __ev2(UpdateSource updateSource, String filename, String destinationFilePath, Int32 repeatCount, Int32 repeatTotal)
                    : base(updateSource, filename, destinationFilePath)
                {
                    RepeatCount = repeatCount;
                    RepeatTotal = repeatTotal;
                }

            }

            public class __ev2b : __ev2 // TODo rename
            {

                public Exception Error { get; private set; }


                public __ev2b(UpdateSource updateSource, String filename, String destinationFilePath, Int32 repeatCount, Int32 repeatTotal, Exception error)
                    : base(updateSource, filename, destinationFilePath, repeatCount, repeatTotal)
                {
                    Error = error;
                }

            }

            public class __ev3 : __ev2 // TODo rename
            {

                public Int64 BytesCount { get; private set; }

                public Int64 BytesTotal { get; private set; }


                public __ev3(UpdateSource updateSource, String filename, String destinationFilePath, Int32 repeatCount, Int32 repeatTotal, Int64 bytesCount, Int64 bytesTotal)
                    : base(updateSource, filename, destinationFilePath, repeatCount, repeatTotal)
                {
                    BytesCount = bytesCount;
                    BytesTotal = bytesTotal;
                }

            }


            // TODO event

            protected internal void RaiseDownloadStart()
            {
                // TODO
            }

            protected internal void RaiseDownloadFinished()
            {
                // TODO
            }

            protected internal void RaiseDownloadProgress(Int64 bytesCount, Int64 bytesTotal, Int32 repeatCount, Int32 repeatTotal)
            {
                // TODO
            }


            internal event EventHandler<__ev2b> DownloadError;

            protected internal void RaiseDownloadError(UpdateSource updateSource, String filename, String destinationFilePath, Int32 repeatCounter, Int32 repeatTotal, Exception ex)
            {
                // TODO
                var handler = DownloadError;
                if (handler != null)
                {
                    var e = new __ev2b(updateSource, filename, destinationFilePath, repeatCounter, repeatTotal, ex);
                    handler.Invoke(this, e);
                }
            }


        }
    }
}
