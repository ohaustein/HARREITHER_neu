using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Haustein.Bibliotheken.Assemblies
{
    /// <summary>
    /// Eine Klasse, mit deren Hilfe es möglich ist, auf in der Assembly eingebettete Ressourcen zuzugreifen.
    /// Dabei kann die Ressource als ein Byte-Array, als ein String oder ein String-Array abgerufen werden,
    /// aber auch als ein beliebiger Stream.
    /// </summary>
    /// <remarks>
    /// Die Klasse eignet sich zum Beispiel, um im Projekt hinterlegte SQL-Abfrage, aber auch Bilder oder Texte
    /// im Code nutzen zu können. Dabei ist der Namensfilter sehr tolerant; es muss nicht der vollständig
    /// aufgelöste Pfad zur Ressource angegeben werden. Lediglich eine eindeutige Zuordnung ist erforderlich.
    /// </remarks>
    public class EmbeddedResources
    {


        private static EmbeddedResources InstanceFromCallingAssembly;
        private static EmbeddedResources InstanceFromEntryAssembly;
        private static EmbeddedResources InstanceFromExecutingAssembly;


        /// <summary>Erstellt eine neue Instanz der Klasse mit der angegebenen Assembly</summary>
        /// <param name="assembly">Die Assembly, deren Ressourcen verarbeitet werden sollen</param>
        /// <returns>Eine Instanz der Klasse <code>ClassEmbeddedResources</code></returns>
        public static EmbeddedResources CreateFromAssembly(Assembly assembly)
        {
            return new EmbeddedResources(assembly);
        }

        /// <summary>Erstellt eine neue Instanz der Klasse anhand der Executing-Assembly</summary>
        /// <returns>Eine Instanz der Klasse <code>ClassEmbeddedResources</code></returns>
        public static EmbeddedResources CreateFromCallingAssembly()
        {
            if (InstanceFromCallingAssembly == null)
            {
                var assembly = System.Reflection.Assembly.GetCallingAssembly();
                InstanceFromCallingAssembly = new EmbeddedResources(assembly);
            }
            return InstanceFromCallingAssembly;
        }

        /// <summary>Erstellt eine neue Instanz der Klasse anhand der Executing-Assembly</summary>
        /// <returns>Eine Instanz der Klasse <code>ClassEmbeddedResources</code></returns>
        public static EmbeddedResources CreateFromEntryAssembly()
        {
            if (InstanceFromEntryAssembly == null)
            {
                var assembly = System.Reflection.Assembly.GetEntryAssembly();
                InstanceFromEntryAssembly = new EmbeddedResources(assembly);
            }
            return InstanceFromEntryAssembly;
        }

        /// <summary>Erstellt eine neue Instanz der Klasse anhand der Executing-Assembly</summary>
        /// <returns>Eine Instanz der Klasse <code>ClassEmbeddedResources</code></returns>
        public static EmbeddedResources CreateFromExecutingAssembly()
        {
            if (InstanceFromExecutingAssembly == null)
            {
                var assembly = System.Reflection.Assembly.GetCallingAssembly();
                InstanceFromExecutingAssembly = new EmbeddedResources(assembly);
            }
            return InstanceFromExecutingAssembly;
        }

        /// <summary>Erstellt eine neue Instanz der Klasse mit der angegebenen Assembly</summary>
        /// <param name="instance">Eine Instanz, deren Assembly verarbeitet werden soll</param>
        /// <returns>Eine Instanz der Klasse <code>ClassEmbeddedResources</code></returns>
        public static EmbeddedResources CreateFromObject(Object instance)
        {
            var assembly = instance.GetType().Assembly;
            return new EmbeddedResources(assembly);
        }

        /// <summary>Erstellt eine neue Instanz der Klasse mit der einem bestimmten Typ zugeordneten Assembly</summary>
        /// <param name="type">Der Typ, dessen Assembly verarbeitet werden sollen</param>
        /// <returns>Eine Instanz der Klasse <code>ClassEmbeddedResources</code></returns>
        public static EmbeddedResources CreateFromType(Type type)
        {
            var assembly = type.Assembly;
            return new EmbeddedResources(assembly);
        }


        /// <summary>Die Assembly, deren Ressourcen genutzt werden sollen</summary>
        private Assembly Assembly;

        /// <summary>Ein Cache für die Inhalte der Ressourcen der Assembly</summary>
        private Dictionary<String, Byte[]> ResourceContentCache;

        /// <summary>Ein Cache für alle Ressourcennamen der Assembly</summary>
        private List<String> ListResourceNames;


        /// <summary>Alle Ressourcennamen der Assemby</summary>
        /// <remarks>Die Namen werden nur ein Mal aus der Assembly ausgelesen und dann gecacht.</remarks>
        private String[] ResourceNames { get { return ListResourceNames.ToArray(); } }


        /// <summary>Erstellt eine neue Instanz der Klase</summary>
        /// <param name="assembly">Die Assembly, deren Ressourcen verarbeitet werden sollen</param>
        /// <remarks>Von außerhalb werden Instanzen über die statischen Factory-Methoden gebildet</remarks>
        private EmbeddedResources(Assembly assembly)
        {
            Assembly = assembly;
            ResourceContentCache = new Dictionary<String, Byte[]>();
            ListResourceNames = new List<String>(DetermineResourceNames());
        }


        /// <summary>Ermittelt (für den Cache) alle Ressourcennamen der Assembly</summary>
        /// <returns>Gibt die Namen als ein String-Array zurück</returns>
        private String[] DetermineResourceNames()
        {
            var resourceNames = Assembly.GetManifestResourceNames();
            return resourceNames;
        }

        /// <summary>Sucht nach dem Namen einer Ressourcen und gibt deren Namen zurück, falls eine eindeutige Zuordnung möglich ist. Dabei werden auch Ressourcen gefunden, deren Name nur in Teilen übereinstimmt.</summary>
        /// <param name="name">Name der Ressource, nach der gesucht wird</param>
        /// <param name="onlyExactMatches">Es werden nur vollständige Übereinstimmungen berücksichtigt</param>
        /// <param name="actionWhenNotFound">Aktion, die ausgeführt wird, wenn keine Übereinstimmung gefunden wurde (unterdrückt eine Exception)</param>
        /// <param name="actionWhenAmbiguous">Aktion, die ausgeführt wird, wenn keine eindeutige Übereinstimmung gefunden wurde (unterdrückt eine Exception)</param>
        /// <param name="supressException">Falls keine oder keine eindeutige Übereinstimmung gefunden wird, wird keine Exception ausgelöst</param>
        /// <returns>Gibt den exakten Namen der Ressource zurück</returns>
        public String FindeResourceName(String name, Boolean onlyExactMatches = false, Action actionWhenNotFound = null, Action actionWhenAmbiguous = null, Boolean supressException = false)
        {
            if (ResourceNames.Contains(name)) return name;
            if (onlyExactMatches) throw EmbeddedResourcesException.NoExactMatch(name);
            var matches = new HashSet<String>();
            foreach (var resourceName in ResourceNames)
            {
                if (resourceName.ToUpper().Contains(name.ToUpper())) matches.Add(resourceName);
            }
            if (matches.Count == 1) return matches.First();
            if (matches.Count == 0) { if (actionWhenNotFound == null) { if (!supressException) throw EmbeddedResourcesException.NoMatchingResourceFound(name); } else actionWhenNotFound(); }
            if (matches.Count > 1) { if (actionWhenAmbiguous == null) { if (!supressException) throw EmbeddedResourcesException.ResourceNameIsAmbiguous(name); } else actionWhenAmbiguous(); }
            return null;
        }

        /// <summary>Lädt eine Resource aus der Assembly und speichert sie im Cache ab</summary>
        /// <param name="resourceName">Der Name der Resource, die geladen werden soll (exakte Übereinstimmung notwendig)</param>
        /// <remarks>Die Ressource wird im Cache als Byte-Array abgespeichert</remarks>
        private void StoreResourceInCache(String resourceName)
        {
            if (!ResourceContentCache.ContainsKey(resourceName))
            {
                using (var manifestResourceStream = Assembly.GetManifestResourceStream(resourceName))
                {
                    var count = Convert.ToInt32(manifestResourceStream.Length);
                    var buffer = new Byte[count];
                    manifestResourceStream.Read(buffer, 0, count);
                    ResourceContentCache.Add(resourceName, buffer);
                }
            }
        }


        /// <summary>Gibt die Ressource als ein Byte-Array zurück (für interne Verwendung)</summary>
        /// <param name="name">Name der Ressource, nach der gesucht wird</param>
        /// <param name="onlyExactMatches">Es werden nur vollständige Übereinstimmungen berücksichtigt</param>
        /// <returns>Ein Byte-Array, das die gesamte Ressource beinhaltet</returns>
        protected byte[] GetResource(String name, Boolean onlyExactMatches)
        {
            var resourceName = FindeResourceName(name, onlyExactMatches);
            if (!ResourceContentCache.ContainsKey(resourceName))
            {
                StoreResourceInCache(resourceName);
            }
            return ResourceContentCache[resourceName];
        }

        /// <summary>Gibt die Ressource als ein Byte-Array zurück</summary>
        /// <param name="resourceName">Name der Ressource, nach der gesucht wird</param>
        /// <param name="onlyExactMatches">Es werden nur vollständige Übereinstimmungen berücksichtigt</param>
        /// <returns>Ein Byte-Array, das die gesamte Ressource beinhaltet</returns>
        public byte[] GetByteArray(String resourceName, Boolean onlyExactMatches = false)
        {
            var buffer = GetResource(resourceName, onlyExactMatches);
            return buffer;
        }

        /// <summary>Gibt die Ressource als einen Stream zurück</summary>
        /// <typeparam name="TStream">Der Datentyp des Streams (dieser muss einen parameterlosen Konstruktor aufweisen)</typeparam>
        /// <param name="functionStreamCreation">Funktion, die eine Instanz des Streams erzeugt</param>
        /// <param name="resourceName">Name der Ressource, nach der gesucht wird</param>
        /// <param name="onlyExactMatches">Es werden nur vollständige Übereinstimmungen berücksichtigt</param>
        /// <returns></returns>
        public Stream GetStream<TStream>(Func<TStream> functionStreamCreation, String resourceName, Boolean onlyExactMatches = false) where TStream : Stream
        {
            var buffer = GetResource(resourceName, onlyExactMatches);
            var stream = functionStreamCreation();
            stream.Write(buffer, 0, buffer.Length);
            stream.Position = 0;
            return stream;
        }

        /// <summary>Gibt die Ressource als einen Stream zurück</summary>
        /// <typeparam name="TStream">Der Datentyp des Streams (dieser muss einen parameterlosen Konstruktor aufweisen)</typeparam>
        /// <param name="resourceName">Name der Ressource, nach der gesucht wird</param>
        /// <param name="onlyExactMatches">Es werden nur vollständige Übereinstimmungen berücksichtigt</param>
        /// <returns>Gibt einen Stream zurück, der die aktuelle Ressource beinhaltet</returns>
        public Stream GetStream<TStream>(String resourceName, Boolean onlyExactMatches = false) where TStream : Stream, new() { return GetStream<TStream>(() => new TStream(), resourceName, onlyExactMatches); }

        /// <summary>Gibt die Ressource als einen MemoryStream zurück</summary>
        /// <param name="resourceName">Name der Ressource, nach der gesucht wird</param>
        /// <param name="onlyExactMatches">Es werden nur vollständige Übereinstimmungen berücksichtigt</param>
        /// <returns>Gibt einen MemoryStream zurück, der die aktuelle Ressource beinhaltet</returns>
        public Stream GetMemoryStream(String resourceName, Boolean onlyExactMatches = false) { return GetStream<MemoryStream>(resourceName, onlyExactMatches); }

        /// <summary>Gibt die Ressource als einen String zurück</summary>
        /// <param name="encoding">Die Codierung, mit deren Hilfe das Byte-Array in einen String umgewandelt wird</param>
        /// <param name="resourceName">Name der Ressource, nach der gesucht wird</param>
        /// <param name="onlyExactMatches">Es werden nur vollständige Übereinstimmungen berücksichtigt</param>
        /// <returns>Einen String, der die gesamte Ressource beinhaltet</returns>
        public string GetString(String resourceName, Boolean onlyExactMatches = false, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.Default;
            var buffer = GetResource(resourceName, onlyExactMatches);
            return encoding.GetString(buffer);
        }

        /// <summary>Gibt die Ressource als ein String-Array zurück</summary>
        /// <param name="encoding">Die Codierung, mit deren Hilfe das Byte-Array in ein String-Array umgewandelt wird</param>
        /// <param name="resourceName">Name der Ressource, nach der gesucht wird</param>
        /// <param name="onlyExactMatches">Es werden nur vollständige Übereinstimmungen berücksichtigt</param>
        /// <returns>Ein String-Array, das die gesamte Ressource beinhaltet</returns>
        /// <remarks>Mit Hilfe eines Streams erfolgt hier die Umwandlung aus einem Byte-Array in einzelne Zeilen</remarks>
        public String[] GetStringArray(String resourceName, Boolean onlyExactMatches = false, Encoding encoding = null)
        {
            var listString = new List<String>();
            var buffer = GetResource(resourceName, onlyExactMatches);
            using (var memoryStream = new MemoryStream(buffer))
            {
                var streamReader = (encoding == null ? new StreamReader(memoryStream, detectEncodingFromByteOrderMarks: true) : new StreamReader(memoryStream, encoding));
                using (streamReader)
                {
                    while (!streamReader.EndOfStream)
                    {
                        var readLine = streamReader.ReadLine();
                        listString.Add(readLine);
                    }
                }
            }
            return listString.ToArray();
        }


        public class EmbeddedResourcesException : System.Exception
        {

            public static EmbeddedResourcesException NoExactMatch(String resourceName) { return new EmbeddedResourcesException(resourceName, "Kein exakte Übereinstimmung für Ressourcenname '{0}' gefunden", resourceName); }

            public static EmbeddedResourcesException NoMatchingResourceFound(String resourceName) { return new EmbeddedResourcesException(resourceName, "Ressourcenname '{0}' kann nicht gefunden werden", resourceName); }

            public static EmbeddedResourcesException ResourceNameIsAmbiguous(String resourceName) { return new EmbeddedResourcesException(resourceName, "Ressourcenname '{0}' kann nicht eindeutig zugeordnet werden", resourceName); }

            public String ResourceName { get; private set; }

            private EmbeddedResourcesException(String resourceName, String message, params Object[] parameters)
                : base(String.Format(message, parameters))
            {
                ResourceName = resourceName;
            }

        }

    }
}
