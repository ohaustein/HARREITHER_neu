using System;
using System.Collections.Generic;
using System.IO;
using System.Security;

namespace Europlan.Application
{
    public class UpdateSourceFileSystem : UpdateSource
    {


        private const Int32 BYTE_PUFFER_SIZE = 16384;


        public static UpdateSourceFileSystem Load(BinaryReader binaryReader)
        {
            var caption = binaryReader.ReadDecodedString();
            var folder = binaryReader.ReadDecodedString();
            var username = binaryReader.ReadDecodedString();
            var password = binaryReader.ReadDecodedString().ToSecureString();
            return new UpdateSourceFileSystem(caption, folder, username, password);
        }


        private String Folder { get; set; }

        private SecureString Password { get; set; }

        private String Username { get; set; }


        public UpdateSourceFileSystem(String caption, String folder, String username, SecureString password)
            : base(caption)
        {
            Folder = folder;
            Password = password;
            Username = username;
        }


        protected override void DownloadUpdateFromSource(String filename, String destinationFilePath, Action<Int64, Int64> progressReporter)
        {
            var filePath = Path.Combine(Folder, filename);
            File.Copy(filePath, destinationFilePath, overwrite: true);
        }

        protected override IEnumerable<String> GetAvailableFilenames()
        {
            var filenames = Directory.GetFiles(Folder);
            return filenames;
        }

        protected internal override void Save(BinaryWriter binaryWriter)
        {
            binaryWriter.Write('Y');
            binaryWriter.WriteEncodedString(Caption);
            binaryWriter.WriteEncodedString(Folder);
            binaryWriter.WriteEncodedString(Username);
            binaryWriter.WriteEncodedString(Password.ToUnsecureString());
        }


    }
}
