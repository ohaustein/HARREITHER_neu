using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;

namespace Europlan.Licensing {
	[XmlRoot("licenseTemplate")]
	public class LicenseTemplate : AbstractLicense<LicensedModuleTemplate, LicensedSystemTemplate> {

		protected string email = "";

		protected override LicensedModuleTemplate NewModule(string moduleName) {
			return new LicensedModuleTemplate(moduleName);
		}

		public string Signature {
			get {
				Debug.Assert(!EncryptionManager.Instance.PublicOnly);
				return EncryptionManager.Instance.CalculateSignature(this.LicenseStringForSigning);
			}
		}

		[XmlAttribute("email")]
		public string Email {
			get { return this.email; }
			set { this.email = value; }
		}

		public static License LoadLicense(Stream stream) {
			XmlSerializer serializer = new XmlSerializer(typeof(LicenseTemplate));
			return (License)serializer.Deserialize(stream);
		}

		public void SaveLicense(Stream stream) {
			XmlSerializer serializer = new XmlSerializer(typeof(LicenseTemplate));
			serializer.Serialize(stream, this);
		}

		public License CreateLicense() {
			License lic = new License();
			lic.LicensedTo = this.licensedTo;
			lic.Header = this.header;
			lic.ValidUntil = this.validUntil;
			foreach (LicensedModuleTemplate module in this.modules) {
				lic.Modules.Add(module.CreateModule());
			}
			foreach (LicensedSystemTemplate system in this.systems) {
				lic.Systems.Add(system.CreateSystem());
			}
			return lic;
		}
	}
}
