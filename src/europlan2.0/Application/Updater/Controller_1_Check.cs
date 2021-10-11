using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Europlan.Application
{
    internal partial class Controller
    {


        public static IEnumerable<CheckResult> RunCheck(IEnumerable<UpdateSource> updateSources, Version executableVersion, String language)
        {
            var task = Task.Run(
                    delegate ()
                    {
                        var availableUpdateList = new List<Tuple<Version, String, UpdateSource>>();
                        var versionPredicator = ReferenceEquals(executableVersion, null) ? new Func<Version, Boolean>(version => true) : new Func<Version, Boolean>(version => version > executableVersion);
                        try
                        {
                            // Alle Update-Quellen parallel abfragen
                            Parallel.ForEach(updateSources,
                                    delegate (UpdateSource updateSource)
                                    {
                                        // Alle verfügbaren Updates von der Update-Quelle abrufen
                                        var availableUpdates = updateSource.GetAvailableUpdates();

                                        // Die Updates filtern, die für die Version der Anwendung in Frage kommen
                                        availableUpdates = availableUpdates
                                                .Where(item => Path.GetFileNameWithoutExtension(item.Item2).EndsWith(language, StringComparison.OrdinalIgnoreCase))
                                                .Where(item => versionPredicator.Invoke(item.Item1));

                                        // Diese Updates in die Ergebnisliste eintragen
                                        foreach (var availableUpdate in availableUpdates)
                                        {
                                            lock (availableUpdateList)
                                            {
                                                availableUpdateList.Add(Tuple.Create(availableUpdate.Item1, availableUpdate.Item2, updateSource));
                                            }
                                        }
                                    }
                                );
                        }
                        catch { throw; }
                        // Die gefundenen Updates nach Version gruppieren und pro Version die verfügbaren Update-Quellen hinterlegen
                        return availableUpdateList
                    .GroupBy(item => item.Item1)
                    .Select(item => new CheckResult(item.Key, item.Select(item1 => Tuple.Create(item1.Item3, item1.Item2))))
                    .ToArray();
                    }
                );
            task.Wait();
            var checkResultArray = task.Result;
            return checkResultArray.AsEnumerable();
        }

        internal static Version GetExecutableVersion()
        {
            try
            {
                var entryAssembly = Assembly.GetEntryAssembly();
                var assemblyFileVersionAttribute = entryAssembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
                if (assemblyFileVersionAttribute != null)
                {
                    var version = Version.Parse(assemblyFileVersionAttribute.Version);
                    return version;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }


        public class CheckResult
        {

            private Tuple<UpdateSource, String>[] UpdateSourceFileNameArray;

            public IEnumerable<Tuple<UpdateSource, String>> UpdateSourceFileNameCollection
            {
                get { return UpdateSourceFileNameArray.AsEnumerable(); }
                private set { UpdateSourceFileNameArray = value.ToArray(); }
            }

            public Version Version { get; private set; }

            public CheckResult(Version version, IEnumerable<Tuple<UpdateSource, String>> updateSourceFileNameCollection)
            {
                UpdateSourceFileNameArray = updateSourceFileNameCollection.ToArray();
                Version = version;
            }

        }


    }
}
