using Haustein.Bibliotheken.Assemblies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Haustein.Extensions;
using System.Collections;

namespace Europlan.Application
{
    public partial class UpdateSourceCollection : IEnumerable<UpdateSource>
    {


        private const String UPDATER_SOURCES_DEFAULT_FILENAME = "UpdateSources.xml";
        private const String XMLNAME_UPDATESOURCE = "UpdateSource";
        private const String XMLNAME_UPDATESOURCE_CAPTION = "Caption";
        private const String XMLNAME_UPDATESOURCE_DOWNLOADURL= "DownloadURL";
        private const String XMLNAME_UPDATESOURCE_VERSIONSURL = "VersionsURL";


        public static UpdateSourceCollection Load(String updateSourceFilePath)
        {
            if (File.Exists(updateSourceFilePath))
            {
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

        public static UpdateSourceCollection LoadFromFileInDirectory(String directoryName)
        {
            var sourcesFilePath = Path.Combine(directoryName, UPDATER_SOURCES_DEFAULT_FILENAME);
            return Load(sourcesFilePath);
        }

        public static UpdateSourceCollection LoadFromFileInApplicationDirectory()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            var entryAssemblyLocation = entryAssembly.Location;
            var entryAssemblyDirectoryName = Path.GetDirectoryName(entryAssemblyLocation);
            return LoadFromFileInDirectory(entryAssemblyDirectoryName);
        }

        public static UpdateSourceCollection LoadFromEmbeddedResource()
        {
            var embeddedResources = EmbeddedResources.CreateFromEntryAssembly();
            using (var stream = embeddedResources.GetMemoryStream(UPDATER_SOURCES_DEFAULT_FILENAME))
            {
                return LoadFromStream(stream);
            }
        }

        private static UpdateSourceCollection LoadFromStream(Stream stream)
        {

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(stream);

            var updateSourceArray = xmlDocument.DocumentElement.GetChildNodesArray(XMLNAME_UPDATESOURCE, ReadUpdateSource);

            var result = new UpdateSourceCollection(updateSourceArray);
            return result;
        }


        private static UpdateSource ReadUpdateSource(XmlNode xmlNode)
        {
            var caption = xmlNode.GetAttributeValue<String>(XMLNAME_UPDATESOURCE_CAPTION, attributeMustExist: true, attributeMustHaveValue :true);
            var versionsUrl = xmlNode.GetChildNodeValue<String>(XMLNAME_UPDATESOURCE_VERSIONSURL, childNodeMustExist: true, childNodeMustHaveValue: true);
            var downloadUrlTemplate = xmlNode.GetChildNodeValue<String>(XMLNAME_UPDATESOURCE_DOWNLOADURL, childNodeMustExist: true, childNodeMustHaveValue: true);
            var updateSource = new UpdateSourceHttp(caption, versionsUrl, downloadUrlTemplate);
            return updateSource;
        }

        public IEnumerator<UpdateSource> GetEnumerator()
        {
            return ((IEnumerable<UpdateSource>)UpdateSourceArray).GetEnumerator();
        }

          IEnumerator IEnumerable.GetEnumerator()
        {
            return UpdateSourceArray.GetEnumerator();
        }

        private readonly UpdateSource[] UpdateSourceArray;


        private UpdateSourceCollection(IEnumerable<UpdateSource> updateSourceCollection)
        {
            UpdateSourceArray = updateSourceCollection.ToArray();
        }


    }
}
