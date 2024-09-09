using System;
using System.IO;

namespace Europlan.Common
{
    class PlanFileHandling
    {


        private static PlanFileHandling Instance;

        internal static PlanFileHandling Default
        {
            get
            {
                if (Instance == null) Instance = new PlanFileHandling();
                return Instance;
            }
        }


        private PlanFileHandling()
        {
        }


        internal String GetLocalFilePath(String sourceFilePath, String localFilesDirectoryPath, Boolean isPdf)
        {
            if (isPdf)
            {
                for (var number = null as Int32?; ; number = number.GetValueOrDefault(0) + 1)
                {
                    var localFileName =
                            Path.GetFileNameWithoutExtension(sourceFilePath) +
                            (number.HasValue ? "-" + number.Value.ToString() : String.Empty) +
                            ".png";
                    var localFilePath = Path.Combine(localFilesDirectoryPath, localFileName);
                    if (File.Exists(localFilePath)) continue;
                    return localFilePath;
                }
            }
            else
            {
                var sourceFileName = Path.GetFileName(sourceFilePath);
                var localFilePath = Path.Combine(localFilesDirectoryPath, sourceFileName);
                return localFilePath;
            }
        }


    }
}
