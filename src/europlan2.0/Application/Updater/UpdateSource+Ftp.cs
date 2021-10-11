using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;

namespace Europlan.Application
{
    public class UpdateSourceFtp : UpdateSource
    {


        private const Int32 BYTE_PUFFER_SIZE = 16384;


        public static UpdateSourceFtp Load(BinaryReader binaryReader)
        {
            var caption = binaryReader.ReadDecodedString();
            var host = binaryReader.ReadDecodedString();
            var userID = binaryReader.ReadDecodedString();
            var password = binaryReader.ReadDecodedString().ToSecureString();
            var folder = binaryReader.ReadDecodedString();
            return new UpdateSourceFtp(caption, host, userID, password, folder);  
        }


        private String Folder { get; set; }

        private String Host { get; set; }

        private SecureString Password { get; set; }

        private String UserID { get; set; }


        public UpdateSourceFtp(String caption, String host, String userID, SecureString password, String folder)
            : base(caption)
        {
            Host = host;
            Password = password;
            Folder = folder;
            UserID = userID;
        }


        protected override void DownloadUpdateFromSource(String filename, String destinationFilePath, Action<Int64, Int64> progressReporter)
        {
            var request = CreateFtpRequest(filename);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            var response = GetResponse(request);
            var readingStream = response.GetResponseStream();
            var bytesTotal = response.ContentLength;
            var bytesCount = 0L;
            using (var outputStream = new FileStream(destinationFilePath, FileMode.Create))
            {
                var buffer = new Byte[BYTE_PUFFER_SIZE];
                while (true)
                {
                    var bytesRead = readingStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;
                    outputStream.Write(buffer, 0, bytesRead);
                    bytesCount += bytesRead;
                    progressReporter.Invoke(bytesCount, bytesTotal);
                }
            }
        }



        private FtpWebRequest CreateFtpRequest(String filename = null)
        {
            try
            {
                var requestUriString = String.Concat(Host, Folder);
                if (filename != null) requestUriString += "/" + filename;
                var webRequest = WebRequest.Create(requestUriString);
                if (webRequest is FtpWebRequest)
                {
                    var ftpWebRequest = webRequest as FtpWebRequest;
                    ftpWebRequest.Credentials = new NetworkCredential(UserID, Password);
                    return ftpWebRequest;
                }
                else
                {
                    throw new Exception("The created request is not of FTP type");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Creating an FTP web request failed", ex);
            }
        }

        protected override IEnumerable<String> GetAvailableFilenames()
        {
            var request = CreateFtpRequest();
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            var response = GetResponse(request);
            var filenameList = new List<String>();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    var directoryParts = line.Split('/');
                    var filename = directoryParts.Last();
                    filenameList.Add(filename);
                }
            }
            return filenameList.AsEnumerable();
        }

        private FtpWebResponse GetResponse(FtpWebRequest ftpRequest)
        {
            try
            {
                if (ftpRequest == null) throw new ArgumentNullException("ftpRequest");
                var webResponse = ftpRequest.GetResponse();
                if (webResponse is FtpWebResponse)
                {
                    var ftpWebResponse = webResponse as FtpWebResponse;
                    return ftpWebResponse;
                }
                else
                {
                    throw new Exception("The response is not of FTP type");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Getting an FTP response failed", ex);
            }
        }


        protected internal override void Save(BinaryWriter binaryWriter)
        {
            binaryWriter.Write('F');
            binaryWriter.WriteEncodedString(Caption);
            binaryWriter.WriteEncodedString(Host);
            binaryWriter.WriteEncodedString(UserID);
            binaryWriter.WriteEncodedString(Password.ToUnsecureString());
            binaryWriter.WriteEncodedString(Folder);
        }


    }
}
