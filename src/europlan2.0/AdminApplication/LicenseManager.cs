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
			foreach (LicenseTemplate lt in instance.Licenses) {
				List<LicensedModuleTemplate> removeModules = new List<LicensedModuleTemplate>();
				foreach (LicensedModuleTemplate lmt in lt.Modules) {
					if (!AbstractLicensedModule.DefaultModules.ContainsKey(lmt.Name)) {
						removeModules.Add(lmt);
					}
				}
				foreach (LicensedModuleTemplate remove in removeModules) {
					lt.Modules.Remove(remove);
				}
				foreach (string m in AbstractLicensedModule.DefaultModules.Keys) {
					lt.SetModuleEnabled(m, lt.IsModuleEnabled(m));
				}
			}
			return instance;
		}

		public List<LicenseTemplate> LoadAdditionalLicenses(Stream stream, bool endUserLicense) {
			List<LicenseTemplate> newLicenses = new List<LicenseTemplate>();
			if (endUserLicense) {
				XmlSerializer serializer = new XmlSerializer(typeof(License));
				License additionalLicense = (License)serializer.Deserialize(stream);
				LicenseTemplate lt = new LicenseTemplate();
				//lt.Email = additionalLicense.Email; // Email is missing in License
				lt.Header = additionalLicense.Header;
				lt.LicensedTo = additionalLicense.LicensedTo;
				lt.ValidUntil = additionalLicense.ValidUntil;
				foreach (LicensedModule lm in additionalLicense.Modules) {
					if (AbstractLicensedModule.DefaultModules.ContainsKey(lm.Name)) {
						LicensedModuleTemplate lmt = new LicensedModuleTemplate();
						lmt.DisplayName = lm.DisplayName;
						lmt.Enabled = lm.Enabled;
						lmt.Name = lm.Name;
						lt.Modules.Add(lmt);
					}
				}
				foreach (LicensedSystem ls in additionalLicense.Systems) {
					LicensedSystemTemplate lst = new LicensedSystemTemplate();
					//lst.AddedDate = ls.AddedDate; // AddedDate is missing in LicensedSystem
					//lst.Annotation = ls.Annotation; // Annotation is missing in LicensedSystem
					lst.Id = ls.Id;
					lt.Systems.Add(lst);
				}
				foreach (string m in AbstractLicensedModule.DefaultModules.Keys) {
					lt.SetModuleEnabled(m, lt.IsModuleEnabled(m));
				}
				newLicenses.Add(lt);
				this.licenses.Add(lt);
			} else {
				XmlSerializer serializer = new XmlSerializer(typeof(LicenseManager));
				LicenseManager additionalLicenses = (LicenseManager)serializer.Deserialize(stream);
				foreach (LicenseTemplate lt in additionalLicenses.Licenses) {
					List<LicensedModuleTemplate> removeModules = new List<LicensedModuleTemplate>();
					foreach (LicensedModuleTemplate lmt in lt.Modules) {
						if (!AbstractLicensedModule.DefaultModules.ContainsKey(lmt.Name)) {
							removeModules.Add(lmt);
						}
					}
					foreach (LicensedModuleTemplate remove in removeModules) {
						lt.Modules.Remove(remove);
					}
					foreach (string m in AbstractLicensedModule.DefaultModules.Keys) {
						lt.SetModuleEnabled(m, lt.IsModuleEnabled(m));
					}
					newLicenses.Add(lt);
					this.licenses.Add(lt);
				}
			}
			return newLicenses;
		}

		public void SaveLicenseManager(Stream stream) {
			XmlSerializer serializer = new XmlSerializer(typeof(LicenseManager));
			serializer.Serialize(stream, this);
		}
	}
}
