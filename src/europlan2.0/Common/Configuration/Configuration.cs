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
		private List<Material> materials;
		private List<Category> categories;
		private List<Construction> constructions;
		private SerializableDictionary<string, string> materialToCategoryMapping;

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
			this.materials = new List<Material>();
			this.constructions = new List<Construction>();
			this.categories = new List<Category>();
			this.materialToCategoryMapping = new SerializableDictionary<string, string>();
		}

		private void InitializeMaterialToCategoryMapping() {

			StreamReader sr = new StreamReader(Path.Combine(appDataPath, "DATANORM.001"), System.Text.Encoding.GetEncoding(850));
			string line;
			while ((line = sr.ReadLine()) != null) {
				if (line.StartsWith("A")) {
					string[] positions = line.Split(';');
					string id = positions[2];
					string name = positions[4].Trim() + " " + positions[5].Trim();
					float price = Int32.Parse(positions[9]) / 100;
					this.materials.Add(new Material(id, name, "", null, "", price, null, false));
				}
			}

			foreach (Material material in materials) {
				if (materialToCategoryMapping.ContainsKey(material.Id)) {
					string categoryId = materialToCategoryMapping[material.Id];
					foreach (Category category in categories) {
						if (category.Id == categoryId) {
							material.Category = category;
							category.Materials.Add(material);
							continue;
						}
					}
				}
			}
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

			foreach (Material material in config1.Materials) {
				if (!config.Materials.Contains(material)) {
					config.Materials.Add(material);
				}
			}
			foreach (Material material in config2.Materials) {
				if (!config.Materials.Contains(material)) {
					config.Materials.Add(material);
				}
			}

			foreach (Construction construction in config1.Constructions) {
				if (!config.Constructions.Contains(construction)) {
					config.Constructions.Add(construction);
				}
			}

			foreach (Construction construction in config2.Constructions) {
				if (!config.Constructions.Contains(construction)) {
					config.Constructions.Add(construction);
				}
			}

			foreach (Category category in config1.Categories) {
				if (!config.Categories.Contains(category)) {
					config.Categories.Add(category);
				}
			}

			foreach (Category category in config2.Categories) {
				if (!config.Categories.Contains(category)) {
					config.Categories.Add(category);
				}
			}

			foreach (string materialId in config1.MaterialToCategoryMapping.Keys) {
				if (!config.MaterialToCategoryMapping.ContainsKey(materialId)) {
					config.MaterialToCategoryMapping.Add(materialId, config1.MaterialToCategoryMapping[materialId]);
				}
			}

			foreach (string materialId in config2.MaterialToCategoryMapping.Keys) {
				if (!config.MaterialToCategoryMapping.ContainsKey(materialId)) {
					config.MaterialToCategoryMapping.Add(materialId, config2.MaterialToCategoryMapping[materialId]);
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
				adminTemplate.InitializeMaterialToCategoryMapping();
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
				userTemplate.InitializeMaterialToCategoryMapping();
				userTemplate.type = ConfigurationType.UserConfiguration;
				return userTemplate;
			}
		}

		[XmlIgnore]
		public ConfigurationType Type {
			get { return type; }
			set { type = value; }
		}

		[XmlIgnore]
		public List<Material> Materials {
			get {
				return this.materials;
			}
			set {
				this.materials = value;
			}
		}

		[XmlIgnore]
		public List<Category> Categories {
			get {
				return this.categories;
			}
			set {
				this.categories = value;
			}
		}

		[XmlIgnore]
		public List<Construction> Constructions {
			get {
				return this.constructions;
			}
			set {
				this.constructions = value;
			}
		}

		public SerializableDictionary<string, string> MaterialToCategoryMapping {
			get {
				return this.materialToCategoryMapping;
			}
			set {
				this.materialToCategoryMapping = value;
			}
		}

		public List<Material> SerializableMaterials {
			get {
				List<Material> materialList = new List<Material>();
				if (type == ConfigurationType.ProjectConfiguration || type == ConfigurationType.UserConfiguration) {
					foreach (Material material in this.materials) {
						if (material.UserDefined) {
							materialList.Add(material);
						}
					}
				} 
				return materialList;
			}
		}

		public List<Category> SerializableCategories {
			get {
				List<Category> categoriesList = new List<Category>();
				if (type == ConfigurationType.AdminConfiguration) {
					categoriesList = categories;
				}
				return categoriesList;
			}
		}

		public List<Construction> SerializableConstructions {
			get {
				List<Construction> constructionList = new List<Construction>();
				if (type == ConfigurationType.ProjectConfiguration || type == ConfigurationType.UserConfiguration) {
					foreach (Construction construction in this.constructions) {
						if (construction.Type.UserDefined) {
							constructionList.Add(construction);
						}
					}
				} else if (type == ConfigurationType.AdminConfiguration) {
					foreach (Construction construction in this.constructions) {
						if (!construction.Type.UserDefined) {
							constructionList.Add(construction);
						}
					}
				} else {
					constructionList = constructions;
				}
				return constructionList;
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
			} catch (Exception e) {
				// TODO
			}
		}


	}

}
