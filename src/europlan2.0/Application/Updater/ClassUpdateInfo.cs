using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Europlan.Application
{
    internal class UpdateInfo
    {


        internal static UpdateInfo Default { get; private set; } = null;

        internal static void SetDefault(String localFilePath, String languageString, Version version)
        {
            Default = new UpdateInfo(localFilePath, languageString, version);
        }



        internal String DefaultFilename
        {
            get { return "Europlan " + LanguageString + "." + Version.ToShortString(); }
        }

        internal String LocalFilePath { get; private set; }

        internal String LanguageString { get; private set; } = null;

        internal Version Version { get; private set; }


        private UpdateInfo(String localFilePath, String languageString, Version version)
        {
            LocalFilePath = localFilePath;
            LanguageString = languageString;
            Version = version;
        }

        internal void Execute()
        {
            var executableSetupPath = FileSystemHelper.CreateTempFilePath(".msi");
            File.Copy(LocalFilePath, executableSetupPath);
            Process.Start(executableSetupPath);
        }

        internal void SaveTo(String path)
        {
            File.Copy(LocalFilePath, path);
        }


    }
}
