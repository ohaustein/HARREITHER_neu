using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Haustein.Assemblies
{
    /// <summary>
    /// Die Klasse ermittelt und repräsentiert die AssemblyInformationen einer Assembly
    /// </summary>
    public class ClassAssemblyInfo
    {


        /// <summary>
        /// Ein lokaler, statischer Cache, in dem alle bereits abgerufenen Assemblyinformationen gehalten werden werden
        /// </summary>
        private static Dictionary<Assembly, ClassAssemblyInfo> AssemblyInfoCache;
        private static AttributeSettingConfiguration[] AttributeSettingConfigurations;


        /// <summary>
        /// Initialisiert die statischen Elemente der Klasse
        /// </summary>
        static ClassAssemblyInfo()
        {
            AssemblyInfoCache = new Dictionary<Assembly, ClassAssemblyInfo>();
            AttributeSettingConfigurations = new AttributeSettingConfiguration[]
            {
                new AttributeSettingConfiguration("Company", typeof(AssemblyCompanyAttribute), (attribute, instance) => instance.Company = (attribute as AssemblyCompanyAttribute).Company),
                new AttributeSettingConfiguration("Copyright", typeof(AssemblyCopyrightAttribute), (attribute, instance) => instance.Copyright = (attribute as AssemblyCopyrightAttribute).Copyright),
                new AttributeSettingConfiguration("Description", typeof(AssemblyDescriptionAttribute), (attribute, instance) => instance.Description = (attribute as AssemblyDescriptionAttribute).Description),
                new AttributeSettingConfiguration("Product", typeof(AssemblyProductAttribute), (attribute, instance) => instance.Product = (attribute as AssemblyProductAttribute).Product),
                new AttributeSettingConfiguration("Title", typeof(AssemblyTitleAttribute), (attribute, instance) => instance.Title = (attribute as AssemblyTitleAttribute).Title)
            };
        }


        /// <summary>
        /// Erzeug eine neue Instanz aus der angegebenen Assembly
        /// </summary>
        /// <param name="assembly">Die Assembly, aus der die Informationen bezogen werden sollen</param>
        /// <returns>Gibt die Assemblyinformationen der Assembly zurück</returns>
        /// <remarks>Es wird eine eventuell gecachte Instanz zurück gegeben</remarks>
        public static ClassAssemblyInfo FromAssembly(Assembly assembly)
        {
            if (AssemblyInfoCache.ContainsKey(assembly))
            {
                return AssemblyInfoCache[assembly];
            }
            else
            {
                var assemblyInfo = new ClassAssemblyInfo(assembly);
                AssemblyInfoCache.Add(assembly, assemblyInfo);
                return assemblyInfo;
            }
        }

        /// <summary>
        /// Gibt die Assemblyinformationen der CallingAssembly zurück
        /// </summary>
        /// <returns>Die Assemblyinformationen der CallingAssembly</returns>
        public static ClassAssemblyInfo FromCallingAssmbly()
        {
            var assembly = Assembly.GetCallingAssembly();
            return FromAssembly(assembly);
        }

        /// <summary>
        /// Gibt die Assemblyinformationen der EntryAssembly zurück
        /// </summary>
        /// <returns>Die Assemblyinformationen der EntryAssembly</returns>
        public static ClassAssemblyInfo FromEntryAssembly()
        {
            var assembly = Assembly.GetEntryAssembly();
            return FromAssembly(assembly);
        }

        /// <summary>
        /// Gibt die Assemblyinformationen der ExecutingAssembly zurück
        /// </summary>
        /// <returns>Die Assemblyinformationen der ExecutingAssembly</returns>
        public static ClassAssemblyInfo FromExecutingAssembly()
        {
            var assembly = Assembly.GetExecutingAssembly();
            return FromAssembly(assembly);
        }


        /// <summary>
        /// Die Firma (Company) der Assembly
        /// </summary>
        public String Company { get; private set; }

        /// <summary>
        /// Das Copyright der Assembly
        /// </summary>
        public String Copyright { get; private set; }

        /// <summary>
        /// Die Beschreibung (Description) der Assembly
        /// </summary>
        public String Description { get; private set; }

        /// <summary>
        /// Das Produkt (Product) der Assembly
        /// </summary>
        public String Product { get; private set; }

        /// <summary>
        /// Der Titel (Title) der Assembly
        /// </summary>
        public String Title { get; private set; }

        /// <summary>
        /// Die Version der Assembly
        /// </summary>
        public Version Version { get; private set; }


        /// <summary>
        /// Erzeugt eine neue Instanz
        /// </summary>
        /// <param name="assembly">Die Assembly, von der die Informationen bezogen werden sollen</param>
        private ClassAssemblyInfo(Assembly assembly)
        {
            var customAttributes = assembly.GetCustomAttributes(false);
            DetermineInformationFromCustomAttributes(customAttributes);
            Version = assembly.GetName().Version;
        }


        private void DetermineInformationFromCustomAttributes(Object[] customAttributes)
        {
            foreach (var attribute in customAttributes)
            {
                var matchingAttributeSettingConfiguration = AttributeSettingConfigurations.SingleOrDefault(item => attribute.GetType().Equals(item.Type));
                if (matchingAttributeSettingConfiguration != null)
                {
                    matchingAttributeSettingConfiguration.SetValue(attribute, this);
                }
            }
            var attributeNamesNotDetermined = AttributeSettingConfigurations
                    .Where(item => item.ValueWasSet == false)
                    .OrderBy(item => item.Caption)
                    .Select(item => item.Caption);
            if (attributeNamesNotDetermined.Count() > 0)
            {
                throw new AttributesNotDeterminedException(attributeNamesNotDetermined);
            }
        }


        private class AttributeSettingConfiguration
        {

            public String Caption { get; private set; }

            private Action<Object, ClassAssemblyInfo> SettingAction { get; set; }

            public Boolean ValueWasSet { get; private set; }

            public Type Type { get; private set; }

            public AttributeSettingConfiguration(String caption, Type type, Action<Object, ClassAssemblyInfo> settingAction)
            {
                Caption = caption;
                Type = type;
                SettingAction = settingAction;
                ValueWasSet = false;
            }

            public void SetValue(Object attribute, ClassAssemblyInfo instance)
            {
                SettingAction.Invoke(attribute, instance);
                ValueWasSet = true;
            }

        }


        public class AttributesNotDeterminedException : Exception
        {


            private const String DEFAULT_MESSAGE = "Not all attributes could be determined from the assembly (please see Data property for details)";


            public AttributesNotDeterminedException(IEnumerable<String> attributeNames)
                : base(DEFAULT_MESSAGE)
            {
                foreach (var attributeName in attributeNames)
                {
                    Data.Add("Attribute", attributeName);
                }
            }


        }


    }
}
