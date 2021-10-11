using Haustein.Bibliotheken.Assemblies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Europlan.Application
{
    public partial class UpdateSourceCollection
    {


        private const String UPDATER_SOURCES_DEFAULT_FILENAME = "Updater.sources";


        private static IEnumerable<UpdateSource> LoadFromStream(Stream stream)
        {
            var updateSourceList = new List<UpdateSource>();
            var sourceCounter = 0;
            using (var binaryReader = new BinaryReader(stream))
            {
                while (stream.Position < stream.Length)
                {
                    var typeCode = binaryReader.ReadChar();
                    sourceCounter++;
                    switch (typeCode)
                    {
                        case 'F':
                            var updateSourceFtp = UpdateSourceFtp.Load(binaryReader);
                            updateSourceList.Add(updateSourceFtp);
                            break;
                        case 'H':
                            var updateSourceHttp = UpdateSourceHttp.Load(binaryReader);
                            updateSourceList.Add(updateSourceHttp);
                            break;
                        case 'Y':
                            var updateSourceFileSystem = UpdateSourceFileSystem.Load(binaryReader);
                            updateSourceList.Add(updateSourceFileSystem);
                            break;
                        default:
                            throw new InvalidTypeCodeException(sourceCounter, typeCode);
                    }
                }
            }
            return updateSourceList.AsEnumerable();
        }

        public static IEnumerable<UpdateSource> Load(String updateSourceFilePath)
        {
            if (File.Exists(updateSourceFilePath))
            {
                var updateSourceList = new List<UpdateSource>();
                using (var inputStream = new FileStream(updateSourceFilePath, FileMode.Open, FileAccess.Read))
                {
                    return LoadFromStream(inputStream);
                }
            }
            else
            {
                throw new UpdateSourceFileNotFoundException(updateSourceFilePath);
            }
        }

        public static IEnumerable<UpdateSource> LoadFromFileInDirectory(String directoryName)
        {
            var sourcesFilePath = Path.Combine(directoryName, UPDATER_SOURCES_DEFAULT_FILENAME);
            return Load(sourcesFilePath);
        }

        public static IEnumerable<UpdateSource> LoadFromFileInApplicationDirectory()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            var entryAssemblyLocation = entryAssembly.Location;
            var entryAssemblyDirectoryName = Path.GetDirectoryName(entryAssemblyLocation);
            return LoadFromFileInDirectory(entryAssemblyDirectoryName);
        }

        public static IEnumerable<UpdateSource> LoadFromEmbeddedResource()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            var embeddedResources = EmbeddedResources.CreateFromEntryAssembly();
            using (var stream = embeddedResources.GetMemoryStream(UPDATER_SOURCES_DEFAULT_FILENAME))
            {
                return LoadFromStream(stream);
            }
        }

        public static void Save(IEnumerable<UpdateSource> updateSourceCollection, String filePath)
        {
            // TODO 
            using (var outputStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (var binaryWriter = new BinaryWriter(outputStream))
                {
                    foreach (var updateSource in updateSourceCollection)
                    {
                        updateSource.Save(binaryWriter);
                    }
                }
            }
        }


    }
}
