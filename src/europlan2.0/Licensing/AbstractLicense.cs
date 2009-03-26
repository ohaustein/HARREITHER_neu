using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;

namespace Europlan.Licensing {
	public abstract class AbstractLicense<ModuleType, SystemType> where ModuleType : AbstractLicensedModule where SystemType : AbstractLicensedSystem {

		protected string licensedTo = "";
		protected string header = "";
		protected DateTime validUntil = DateTime.Now;
		protected List<ModuleType> modules;
		protected List<SystemType> systems;
		protected string signature = null;

		public AbstractLicense() {
			this.modules = new List<ModuleType>();
			this.systems = new List<SystemType>();
		}

		protected event EventHandler nameChanged;

		public event EventHandler NameChanged {
			add { this.nameChanged += value; }
			remove { this.nameChanged -= value; }
		}

		protected void OnNameChanged() {
			if (this.nameChanged != null) {
				this.nameChanged(this, EventArgs.Empty);
			}
		}

		public string DisplayName {
			get {
				string result = (string.IsNullOrEmpty(this.licensedTo)) ? "neue Lizenz" : this.licensedTo;
				result += " (" + this.validUntil.ToShortDateString() + ")";
				return result; // TODO
			}
		}

		/*public static AbstractLicense<ModuleType, SystemType> LoadLicense(Stream stream) {
			XmlSerializer serializer = new XmlSerializer(typeof(License));
			return (AbstractLicense<ModuleType, SystemType>)serializer.Deserialize(stream);
		}

		public void SaveLicense(Stream stream) {
			XmlSerializer serializer = new XmlSerializer(this.GetType());
			serializer.Serialize(stream, this);
		}*/

		[XmlAttribute("licensedTo")]
		public string LicensedTo {
			get { return this.licensedTo; }
			set {
				this.licensedTo = value;
				this.OnNameChanged();
			}
		}

		[XmlAttribute("header")]
		public string Header {
			get { return this.header; }
			set { this.header = value; }
		}

		[XmlAttribute("validUntil")]
		public DateTime ValidUntil {
			get { return this.validUntil; }
			set {
				this.validUntil = value;
				this.OnNameChanged();
			}
		}

		[XmlArray("modules")]
		[XmlArrayItem("module")]
		public List<ModuleType> Modules {
			get { return this.modules; }
			set { this.modules = value; }
		}

		public List<ModuleType> GetEnabledModules() {
			List<ModuleType> enabledModules = new List<ModuleType>();
			foreach (ModuleType module in this.modules) {
				if (module.Enabled) {
					enabledModules.Add(module);
				}
			}
			return enabledModules;
		}

		public bool IsModuleEnabled(string moduleName) {
			foreach (ModuleType module in this.modules) {
				if (module.Name.Equals(moduleName)) {
					return module.Enabled;
				}
			}
			return false;
		}

		public ModuleType GetModule(string moduleName) {
			foreach (ModuleType module in this.modules) {
				if (module.Name.Equals(moduleName)) {
					return module;
				}
			}
			return null;
		}

		public void SetModuleEnabled(string moduleName, bool enabled) {
			ModuleType module = this.GetModule(moduleName);
			if (module == null) {
				module = this.NewModule(moduleName);
				this.modules.Add(module);
			}
			module.Enabled = enabled;
		}

		protected abstract ModuleType NewModule(string moduleName);

		public void RemoveModule(string moduleName) {
			ModuleType module = this.GetModule(moduleName);
			if (module != null) {
				this.modules.Remove(module);
			}
		}

		[XmlArray("approvedSystems")]
		[XmlArrayItem("system")]
		public List<SystemType> Systems {
			get { return this.systems; }
			set { this.systems = value; }
		}

		[XmlAttribute("signature")]
		public string Signature {
			get {
				Debug.Assert(!EncryptionManager.Instance.PublicOnly || this.signature != null);
				if (!EncryptionManager.Instance.PublicOnly) {
					return EncryptionManager.Instance.CalculateSignature(this.LicenseStringForSigning);
				} else {
					return this.signature;
				}
			}
			set {
				if (EncryptionManager.Instance.PublicOnly) {
					this.signature = value;
				}
			}
		}

		public bool IsSignatureValid {
			get {
#if DEBUG
				return true;
#else
				return EncryptionManager.Instance.VerifySignature(this.LicenseStringForSigning, this.Signature);
#endif
			}
		}

		public bool IsSystemValid {
			get {
				HardwareId curId = new HardwareId();
				foreach (SystemType system in this.systems) {
					if (system.MatchesCurrentSystem) {
						return true;
					}
				}
				return false;
			}
		}

		public bool IsDateValid {
			get {
				return this.validUntil.CompareTo(DateTime.Today) >= 0;
			}
		}

		public bool IsValid {
			get { return this.IsSignatureValid && this.IsSystemValid && this.IsDateValid; }
		}

		public string LicenseStringForSigning {
			get {
				string tmp = "";
				/*tmp += "key: " + this.key.HashedKey;*/
				tmp += "licensed to: " + this.licensedTo;
				tmp += "\nheader: " + this.header;
				tmp += "\nvalid until: " + this.validUntil.ToBinary().ToString();
				tmp += "\nmodules: ";
				foreach (ModuleType module in this.modules) {
					tmp += module.ToString() + ";";
				}
				tmp += "\nsytems: ";
				foreach (SystemType system in this.systems) {
					tmp += system.ToString() + ";";
				}
				return tmp;
			}
		}
	}
}
