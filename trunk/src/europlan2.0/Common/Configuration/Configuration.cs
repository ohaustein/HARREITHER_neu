using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Europlan.Common {

	[XmlRootAttribute("Configuration")]
	public class Configuration {

		private static Configuration adminTemplate = null;
		private static Configuration userTemplate = null;
		private static readonly object padlock = new object();
		private static string appDataPath = System.Windows.Forms.Application.CommonAppDataPath.Substring(0, System.Windows.Forms.Application.CommonAppDataPath.IndexOf(System.Windows.Forms.Application.ProductVersion));


		// IMPORTANT!!!
		//
		// do not forget to extend the initialization and the + operator when adding a new field
		//
		// IMPORTANT!!!
		private ConfigurationType type;

		public enum ConfigurationType {
			InitializedConfiguration,
			AdminConfiguration,
			UserConfiguration,
			ProjectConfiguration
		}

		public Configuration() {
			InitializeConfiguration();
		}

		private void InitializeConfiguration() {
			this.type = ConfigurationType.InitializedConfiguration;
		}

		public static Configuration operator+(Configuration config1, Configuration config2) {
			Configuration config = new Configuration();
			if (config1.type == config2.type) {
				throw new Exception("Cannot add configurations of the same type");
			}
			Configuration first, second;
			if (config1.type < config2.type) {
				first = config1;
				second = config2;
			} else {
				first = config2;
				second = config1;
			}

			config.type = second.type;

			return config;
		}

		/// <summary>
		/// Gets a admin template of the configuration object
		/// </summary>
		public static Configuration AdminTemplate {
			get {
				if (adminTemplate == null) {
					lock (padlock) {
						try {
							if (adminTemplate == null) {
								XmlSerializer s = new XmlSerializer(typeof(Configuration));
								Stream r = new FileStream(Path.Combine(appDataPath, "global.conf"), FileMode.Open);
								adminTemplate = (Configuration)s.Deserialize(r);
								r.Close();
							}
						} catch {
							// TODO
							adminTemplate = new Configuration();
						}
					}
				}
				adminTemplate.type = ConfigurationType.AdminConfiguration;
				return adminTemplate;
			}
		}

		/// <summary>
		/// Gets a user template of the configuration object
		/// </summary>
		public static Configuration UserTemplate {
			get {
				if (userTemplate == null) {
					lock (padlock) {
						try {
							if (userTemplate == null) {
								XmlSerializer s = new XmlSerializer(typeof(Configuration));
								Stream r = new FileStream(Path.Combine(appDataPath, "custom.conf"), FileMode.Open);
								userTemplate = adminTemplate + (Configuration)s.Deserialize(r);
								r.Close();
							}
						} catch {
							// TODO
							userTemplate = new Configuration();
						}
					}
				}
				userTemplate.type = ConfigurationType.UserConfiguration;
				return userTemplate;
			}
		}

		public ConfigurationType Type {
			get { return type; }
			set { type = value; }
		}

		public void Save() {
			try {
				XmlSerializer s = new XmlSerializer(typeof(Configuration));
				string filename = "";
				if (this.type == ConfigurationType.AdminConfiguration) {
					filename = Path.Combine(appDataPath, "global.conf");
				} else if (this.type == ConfigurationType.UserConfiguration) {
					filename = Path.Combine(appDataPath, "custom.conf");
				} else {
					// TODO
					return;
				}
				Stream w = new FileStream(filename, FileMode.Create);
				s.Serialize(w, this);
				w.Close();
			} catch {
				// TODO
			}
		}


	}

}
