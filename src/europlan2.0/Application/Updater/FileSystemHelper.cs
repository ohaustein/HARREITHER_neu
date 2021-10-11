using System;

namespace Europlan.Application
{
    public static class FileSystemHelper
    {


        private static Object GetTempFileNameLock = new Object();


        internal static String CreateTempFilePath()
        {
            lock (GetTempFileNameLock)
            {
                var filename = System.IO.Path.GetTempFileName();
                return filename;
            }
        }

        internal static String CreateTempFilePath(String extension)
        {
            lock (GetTempFileNameLock)
            {
                var filename = System.IO.Path.GetTempFileName();
                filename += extension;
                return filename;
            }
        }


    }
}
