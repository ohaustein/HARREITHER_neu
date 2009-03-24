using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;

namespace Europlan.Licensing {
	[XmlRoot("license")]
	public class License {

		private LicenseKey key = null;
		private string licensedTo = "";
		private string header = "";
		private DateTime validUntil = DateTime.Now;
		private List<LicensedModule> modules = new List<LicensedModule>();
		private List<LicensedSystem> systems = new List<LicensedSystem>();
		private string signature = null;

		public License() {
		}

		// creates a new empty License with the given LicenseKey
		public License(LicenseKey key) {
			this.key = key;
		}

		public static License LoadLicense(Stream stream) {
			XmlSerializer serializer = new XmlSerializer(typeof(License));
			return (License)serializer.Deserialize(stream);
		}

		public void SaveLicense(Stream stream) {
			XmlSerializer serializer = new XmlSerializer(typeof(License));
			serializer.Serialize(stream, this);
		}

		public LicenseKey Key {
			get { return this.key; }
		}

		[XmlAttribute("key")]
		public string HashedKey {
			get { return this.key.HashedKey; }
			set { this.key = new LicenseKey(value, true); }
		}

		[XmlAttribute("licensedTo")]
		public string LicensedTo {
			get { return this.licensedTo; }
			set { this.licensedTo = value; }
		}

		[XmlAttribute("header")]
		public string Header {
			get { return this.header; }
			set { this.header = value; }
		}

		[XmlAttribute("validUntil")]
		public DateTime ValidUntil {
			get { return this.validUntil; }
			set { this.validUntil = value; }
		}

		[XmlArray("modules")]
		[XmlArrayItem("module")]
		public List<LicensedModule> Modules {
			get { return this.modules; }
			set { this.modules = value; }
		}

		public List<LicensedModule> GetEnabledModules() {
			List<LicensedModule> enabledModules = new List<LicensedModule>();
			foreach (LicensedModule module in this.modules) {
				if (module.Enabled) {
					enabledModules.Add(module);
				}
			}
			return enabledModules;
		}

		public bool IsModuleEnabled(string moduleName) {
			foreach (LicensedModule module in this.modules) {
				if (module.Name.Equals(moduleName)) {
					return module.Enabled;
				}
			}
			return false;
		}

		[XmlArray("approvedSystems")]
		[XmlArrayItem("system")]
		public List<LicensedSystem> Systems {
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
			get { return EncryptionManager.Instance.VerifySignature(this.LicenseStringForSigning, this.Signature); }
		}

		public bool IsSystemValid {
			get {
				HardwareId curId = new HardwareId();
				foreach (LicensedSystem system in this.systems) {
					if (system.MatchesCurrentSystem) {
						return true;
					}
				}
				return false;
			}
		}

		public bool IsLicenseKeyValid(string licenseKey) {
			LicenseKey enteredKey = new LicenseKey(licenseKey, false);
			return this.key.Equals(enteredKey);
		}

		public bool IsValid(string licenseKey) {
			return this.IsSignatureValid && this.IsSystemValid && this.IsLicenseKeyValid(licenseKey);
		}

		public string LicenseStringForSigning {
			get {
				string tmp = "key: " + this.key.HashedKey;
				tmp += "\nlicensed to: " + this.licensedTo;
				tmp += "\nheader: " + this.header;
				tmp += "\nvalid until: " + this.validUntil.ToBinary().ToString();
				tmp += "\nmodules: ";
				foreach (LicensedModule module in this.modules) {
					tmp += module.ToString() + ";";
				}
				tmp += "\nsytems: ";
				foreach (LicensedSystem system in this.systems) {
					tmp += system.ToString() + ";";
				}
				return tmp;
			}
		}
	}
}
