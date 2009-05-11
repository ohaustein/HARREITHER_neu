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
		private Dictionary<string, Material> materials;
		private Dictionary<string, Construction> constructions;

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
			this.materials = new Dictionary<string, Material>();
			this.constructions = new Dictionary<string, Construction>();
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

			foreach (Material material in config1.Materials.Values) {
				if (!config.Materials.ContainsKey(material.Id)) {
					config.Materials.Add(material.Id, material);
				}
			}
			foreach (Material material in config2.Materials.Values) {
				if (!config.Materials.ContainsKey(material.Id)) {
					config.Materials.Add(material.Id, material);
				}
			}

			foreach (Construction construction in config1.Constructions.Values) {
				if (!config.Constructions.ContainsKey(construction.Id)) {
					config.Constructions.Add(construction.Id, construction);
				}
			}

			foreach (Construction construction in config2.Constructions.Values) {
				if (!config.Constructions.ContainsKey(construction.Id)) {
					config.Constructions.Add(construction.Id, construction);
				}
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

		[XmlIgnore]
		public Dictionary<string, Material> Materials {
			get {
				return materials;
			}
			set {
				this.materials = value;
			}
		}

		[XmlIgnore]
		public Dictionary<string, Construction> Constructions {
			get {
				return constructions;
			}
			set {
				this.constructions = value;
			}
		}
		public List<Material> SerializableMaterials {
			get {
				List<Material> materialList = new List<Material>();
				if (type == ConfigurationType.ProjectConfiguration || type == ConfigurationType.UserConfiguration) {
					foreach (Material material in this.materials.Values) {
						if (material.UserDefined) {
							materialList.Add(material);
						}
					}
				} else if (type == ConfigurationType.AdminConfiguration) {
					foreach (Material material in this.materials.Values) {
						if (!material.UserDefined) {
							materialList.Add(material);
						}
					}
				}
				return materialList;
			}
			set {
				// do nothing
				List<Material> test = value;
			}
		}

		public List<Construction> SerializableConstructions {
			get {
				List<Construction> constructionList = new List<Construction>();
				if (type == ConfigurationType.ProjectConfiguration || type == ConfigurationType.UserConfiguration) {
					foreach (Construction construction in this.constructions.Values) {
						if (construction.Type.UserDefined) {
							constructionList.Add(construction);
						}
					}
				} else if (type == ConfigurationType.AdminConfiguration) {
					foreach (Construction construction in this.constructions.Values) {
						if (!construction.Type.UserDefined) {
							constructionList.Add(construction);
						}
					}
				}
				return constructionList;
			}
			set {
				// do nothing
				List<Construction> test = value;
			}
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
