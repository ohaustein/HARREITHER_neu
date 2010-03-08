using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Europlan.Common;
using System.Threading;

namespace Europlan.Licensing {
	public abstract class AbstractLicensedModule {
		protected string name = null;
		protected string displayName = null;
		protected bool enabled = false;

		public static readonly string FeatInternal = "FeatInternal";
		public static readonly string FeatAdmin = "FeatAdmin";
		public static readonly string ProdEuroval = "ProdEuroval";
		public static readonly string ProdEcotherm = "ProdEcotherm";
		public static readonly string ProdHitherm = "ProdHitherm";
		public static readonly string ProdHithermCompact = "ProdHithermCompact";
		public static readonly string ProdModulKlimaBoden = "ProdModulKlimaBoden";
		public static readonly string ProdModulKlimaDecke = "ProdModulKlimaDecke";
		public static readonly string ProdConcreteActivation = "ProdConcreteActivation";
		private static Dictionary<string, string> defaultModules = null;
		private static List<string> defaultEnabledModules = null;
		public static Dictionary<string, string> DefaultModules {
			get {
				if (defaultModules == null) {
					defaultModules = new Dictionary<string,string>();
					defaultModules.Add(FeatInternal, EuroplanRes.AbstractLicensedModule_InterneLizenz /*"Interne Lizenz"*/);
					defaultModules.Add(FeatAdmin, EuroplanRes.AbstractLicensedModule_Adminmodus /*"Adminmodus"*/);
					defaultModules.Add(ProdEuroval, EuroplanRes.AbstractLicensedModule_Euroval /*"Euroval® Fußbodenheizung"*/);
					defaultModules.Add(ProdEcotherm, EuroplanRes.AbstractLicensedModule_Ecotherm /*"Ecotherm® Fußbodenheizung"*/);
					defaultModules.Add(ProdHitherm, EuroplanRes.AbstractLicensedModule_Hitherm /*"Hitherm® Klimawand"*/);
					defaultModules.Add(ProdHithermCompact, EuroplanRes.AbstractLicensedModule_HithermCompact /*"Hitherm® Compact Klimawand"*/);
					defaultModules.Add(ProdModulKlimaBoden, EuroplanRes.AbstractLicensedModule_ModulBoden /*"Modul Klima-Boden"*/);
					defaultModules.Add(ProdModulKlimaDecke, EuroplanRes.AbstractLicensedModule_ModulDecke /*"Modul Klima-Decke"*/);
					defaultModules.Add(ProdConcreteActivation, EuroplanRes.AbstractLicensedModule_BKA /*"Betonkernaktivierung"*/);
				}
				return defaultModules;
			}
		}

		public static List<string> DefaultEnabledModules {
			get {
				if (defaultEnabledModules == null) {
					defaultEnabledModules = new List<string>();
					defaultEnabledModules.Add(ProdEuroval);
					defaultEnabledModules.Add(ProdEcotherm);
					defaultEnabledModules.Add(ProdHitherm);
					defaultEnabledModules.Add(ProdHithermCompact);
					defaultEnabledModules.Add(ProdModulKlimaBoden);
					defaultEnabledModules.Add(ProdModulKlimaDecke);
					defaultEnabledModules.Add(ProdConcreteActivation);
				}
				return defaultEnabledModules;
			}
		}

		public AbstractLicensedModule() {
		}

		public AbstractLicensedModule(string name) {
			this.name = name;
			if (AbstractLicensedModule.DefaultModules.ContainsKey(name)) {
				this.displayName = AbstractLicensedModule.DefaultModules[name];
			} else {
				this.displayName = name;
			}
		}

		public AbstractLicensedModule(string name, bool enabled) {
			this.name = name;
			if (AbstractLicensedModule.DefaultModules.ContainsKey(name)) {
				this.displayName = AbstractLicensedModule.DefaultModules[name];
			} else {
				this.displayName = name;
			}
			this.enabled = enabled;
		}

		public AbstractLicensedModule(string name, string displayName, bool enabled) {
			this.name = name;
			this.displayName = displayName;
			this.enabled = enabled;
		}

		[XmlAttribute("name")]
		public string Name {
			get { return this.name; }
			set { this.name = value; }
		}

		[XmlAttribute("displayName")]
		public string DisplayName {
			get {
				if (this.displayName == null) {
					this.displayName = this.name;
				}
				return this.displayName;
			}
			set { this.displayName = value; }
		}

		[XmlAttribute("enabled")]
		public bool Enabled {
			get { return this.enabled; }
			set { this.enabled = value; }
		}

		public override string ToString() {
			return this.name + ":" + (this.enabled ? "1" : "0");
		}
	}
}
