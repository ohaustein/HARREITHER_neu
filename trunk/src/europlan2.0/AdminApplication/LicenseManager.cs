using System;
using System.Collections.Generic;
using System.Text;
using Europlan.Licensing;
using System.Xml.Serialization;
using System.IO;

namespace Europlan.AdminApplication {
	[XmlRoot("licenseTemplates")]
	public class LicenseManager {

		private static LicenseManager instance = null;

		public static LicenseManager Instance {
			get {
				if (instance == null) {
					instance = new LicenseManager();
				}
				return instance;
			}
		}

		private List<LicenseTemplate> licenses;

		private LicenseManager() {
			this.licenses = new List<LicenseTemplate>();
		}

		[XmlElement("licenseTemplate")]
		public List<LicenseTemplate> Licenses {
			get { return this.licenses; }
			set { this.licenses = value; }
		}

		public static LicenseManager LoadLicenseManager(Stream stream) {
			XmlSerializer serializer = new XmlSerializer(typeof(LicenseManager));
			instance = (LicenseManager)serializer.Deserialize(stream);
			return instance;
		}

		public void SaveLicenseManager(Stream stream) {
			XmlSerializer serializer = new XmlSerializer(typeof(LicenseManager));
			serializer.Serialize(stream, this);
		}
	}
}
