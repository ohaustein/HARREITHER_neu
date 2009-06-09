using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using log4net;

namespace Europlan.Common {

	[XmlRootAttribute("Configuration")]
	public class Configuration {

		private static Configuration adminTemplate = null;
		private static Configuration userTemplate = null;
		private static readonly object padlock = new object();
		private static string appDataPath = System.Windows.Forms.Application.CommonAppDataPath.Substring(0, System.Windows.Forms.Application.CommonAppDataPath.IndexOf(System.Windows.Forms.Application.ProductVersion));

		private static readonly ILog log = LogManager.GetLogger(typeof(Configuration));

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
		private Dictionary<string, float> discounts;
		private List<RoomType> roomTypes;

		private EurovalProduct eurovalProduct = new EurovalProduct();
		private ConcreteActivationProduct concreteActivationProduct = new ConcreteActivationProduct();
		private HithermProduct hithermProduct = new HithermProduct();
		private HithermCompactProduct hithermCompactProduct = new HithermCompactProduct();
		private ModulKlimaBodenProduct modulKlimaBodenProduct = new ModulKlimaBodenProduct();
		private ModulKlimaDeckeProduct modulKlimaDeckeProduct = new ModulKlimaDeckeProduct();

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
			this.discounts = new Dictionary<string, float>();
			this.roomTypes = new List<RoomType>();
			try {
				StreamReader sr = new StreamReader(Path.Combine(appDataPath, "DATANORM.001"), System.Text.Encoding.GetEncoding(850));
				string line;
				while ((line = sr.ReadLine()) != null) {
					if (line.StartsWith("A")) {
						string[] positions = line.Split(';');
						string id = positions[2].Trim();
						string name = positions[4].Trim() + " " + positions[5].Trim();
						float price = Int32.Parse(positions[9]) / 100;
						string discountGroup = positions[10].Trim();
						int denomination = Int32.Parse(positions[6]);
						string unit = positions[8].Trim(); ;
						this.materials.Add(new Material(id, name, id, denomination, unit, price, discountGroup, null, false));
					}
				}
			} catch (Exception ex) {
				log.Error("Error while parsing DATANORM.001 file", ex);
			}
			try {
				StreamReader sr = new StreamReader(Path.Combine(appDataPath, "DATANORM.RAB"), System.Text.Encoding.GetEncoding(850));
				string line;
				while ((line = sr.ReadLine()) != null) {
					if (line.StartsWith("R")) {
						string[] positions = line.Split(';');
						string discountGroup = positions[2].Trim();
						float discount = Int32.Parse(positions[4]) / 100;
						this.discounts.Add(discountGroup, discount);
					}
				}
			} catch (Exception ex) {
				log.Error("Error while parsing DATANORM.RAB file", ex);
			}

		}

		public void RecalculateMaterialToCategoryMapping() {
			List<Material> materialsToRemove = new List<Material>();
			foreach (Material material in materials) {
				if (materialToCategoryMapping.ContainsKey(material.Id)) {
					string categoryId = materialToCategoryMapping[material.Id];
					foreach (Category category in categories) {
						if (category.Id == categoryId) {
							material.Category = category;
							if (!category.Materials.Contains(material)) {
								category.Materials.Add(material);
							}
							continue;
						}
					}
				} else {
					materialsToRemove.Add(material);
				}
			}
			if (this.type == ConfigurationType.UserConfiguration || this.type == ConfigurationType.ProjectConfiguration) {
				foreach (Material material in materialsToRemove) {
					materials.Remove(material);
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

			foreach (string materialId in config1.materialToCategoryMapping.Keys) {
				if (!config.materialToCategoryMapping.ContainsKey(materialId)) {
					config.materialToCategoryMapping.Add(materialId, config1.materialToCategoryMapping[materialId]);
				}
			}

			foreach (string materialId in config2.materialToCategoryMapping.Keys) {
				if (!config.materialToCategoryMapping.ContainsKey(materialId)) {
					config.materialToCategoryMapping.Add(materialId, config2.materialToCategoryMapping[materialId]);
				}
			}

			foreach (RoomType roomType in config1.RoomTypes) {
				if (!config.RoomTypes.Contains(roomType)) {
					config.RoomTypes.Add(roomType);
				}
			}

			foreach (RoomType roomType in config2.RoomTypes) {
				if (!config.RoomTypes.Contains(roomType)) {
					config.RoomTypes.Add(roomType);
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
						} catch (Exception ex) {
							log.Error("Error while loading global.conf", ex);
							adminTemplate = new Configuration();
						}
					}
					adminTemplate.type = ConfigurationType.AdminConfiguration;
					adminTemplate.RecalculateMaterialToCategoryMapping();
				}
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
								userTemplate = AdminTemplate + (Configuration)s.Deserialize(r);
								r.Close();
							}
						} catch (Exception ex) {
							log.Error("Error while loading custom.conf", ex);
							userTemplate = AdminTemplate + new Configuration();
						}
					}
					for (int i = 0; i < Enum.GetNames(typeof(CategoryType)).Length; i++) {
						int order = 1;
						foreach (Category c in userTemplate.categories) {
							if (c.Type == (CategoryType)i) {
								if (c.Order >= order) {
									order = c.Order + 1;
								}
							}
						}
						Category category = new Category(i.ToString(), "UserDefined", (CategoryType)i, order);
						userTemplate.categories.Add(category);
					}
					userTemplate.type = ConfigurationType.UserConfiguration;
					userTemplate.RecalculateMaterialToCategoryMapping();
				}
				return userTemplate;
			}
		}

		public List<Category> GetCategoriesForCategoryType(CategoryType type) {
			List<Category> result = new List<Category>();
			foreach (Category c in this.categories) {
				if (c.Type == type) {
					result.Add(c);
				}
			}
			result.Sort();
			return result;
		}

		public Category GetUserDefinedCategoryForCategoryType(CategoryType type) {
			List<Category> categories = GetCategoriesForCategoryType(type);
			if (categories.Count != 0) {
				return categories[categories.Count - 1];
			}
			return null;
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

		[XmlIgnore]
		public Dictionary<string, float> Discounts {
			get {
				return this.discounts;
			}
			set {
				this.discounts = value;
			}
		}

		[XmlIgnore]
		public List<RoomType> RoomTypes {
			get {
				return this.roomTypes;
			}
			set {
				this.roomTypes = value;
			}
		}

		public SerializableDictionary<string, string> MaterialToCategoryMapping {
			get {
				SerializableDictionary<string, string> mapping = new SerializableDictionary<string, string>();
				if (type == ConfigurationType.ProjectConfiguration || type == ConfigurationType.UserConfiguration) {
					foreach (Material material in this.materials) {
						if ((material.UserDefined) && (material.Category != null)) {
							if (!mapping.ContainsKey(material.Id)) {
								mapping.Add(material.Id, material.Category.Id);
							}
						}
					}
				} else if (type == ConfigurationType.AdminConfiguration) {
					foreach (Material material in this.materials) {
						if ((!material.UserDefined) && (material.Category != null)) {
							if (!mapping.ContainsKey(material.Id)) {
								mapping.Add(material.Id, material.Category.Id);
							}
						}
					}
				} else {
					mapping = this.materialToCategoryMapping;
				}
				return mapping;
			}
			set {
				if (type == ConfigurationType.InitializedConfiguration) {
					this.materialToCategoryMapping = value;
					RecalculateMaterialToCategoryMapping();
				}
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
				} else if (type == ConfigurationType.InitializedConfiguration) {
					materialList = this.materials;
				}
				return materialList;
			}
		}

		public List<Category> SerializableCategories {
			get {
				List<Category> categoriesList = new List<Category>();
				if (type == ConfigurationType.AdminConfiguration || type == ConfigurationType.InitializedConfiguration) {
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

		public List<RoomType> SerializableRoomTypes {
			get {
				List<RoomType> result = new List<RoomType>();
				if (type == ConfigurationType.ProjectConfiguration || type == ConfigurationType.UserConfiguration) {
					foreach (RoomType roomType in this.roomTypes) {
						if (roomType.UserDefined) {
							result.Add(roomType);
						}
					}
				} else if (type == ConfigurationType.AdminConfiguration) {
					foreach (RoomType roomType in this.roomTypes) {
						if (!roomType.UserDefined) {
							result.Add(roomType);
						}
					}
				} else {
					result = roomTypes;
				}
				return result;
			}
		}

		public EurovalProduct EurovalProduct {
			get { return eurovalProduct; }
			set { eurovalProduct = value; }
		}

		public ConcreteActivationProduct ConcreteActivationProduct {
			get { return concreteActivationProduct; }
			set { concreteActivationProduct = value; }
		}

		public HithermProduct HithermProduct {
			get { return hithermProduct; }
			set { hithermProduct = value; }
		}

		public HithermCompactProduct HithermCompactProduct {
			get { return hithermCompactProduct; }
			set { hithermCompactProduct = value; }
		}

		public ModulKlimaBodenProduct ModulKlimaBodenProduct {
			get { return modulKlimaBodenProduct; }
			set { modulKlimaBodenProduct = value; }
		}

		public ModulKlimaDeckeProduct ModulKlimaDeckeProduct {
			get { return modulKlimaDeckeProduct; }
			set { modulKlimaDeckeProduct = value; }
		}

		public P GetProduct<P>() where P: Product {
			if (typeof(P) == typeof(EurovalProduct)) {
				return this.EurovalProduct as P;
			} else if (typeof(P) == typeof(ConcreteActivationProduct)) {
				return this.ConcreteActivationProduct as P;
			} else if (typeof(P) == typeof(HithermProduct)) {
				return this.HithermProduct as P;
			} else if (typeof(P) == typeof(HithermCompactProduct)) {
				return this.HithermCompactProduct as P;
			} else if (typeof(P) == typeof(ModulKlimaBodenProduct)) {
				return this.ModulKlimaBodenProduct as P;
			} else if (typeof(P) == typeof(ModulKlimaDeckeProduct)) {
				return this.ModulKlimaDeckeProduct as P;
			} else {
				log.Warn("unknown product");
			}
			return null;
		}

		public void Reset() {
			if (this.type == ConfigurationType.AdminConfiguration) {
				adminTemplate = null;
				adminTemplate = Configuration.AdminTemplate;
			} else if (this.type == ConfigurationType.UserConfiguration) {
				userTemplate = null;
				userTemplate = Configuration.UserTemplate;
			} else {
				log.Error("Reset() is not supported for this configuration type");
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
					log.Error("Save() has been called for configuration type which is not supported: " + this.type);
					return;
				}
				Stream w = new FileStream(filename, FileMode.Create);
				s.Serialize(w, this);
				w.Close();
			} catch (Exception ex) {
				log.Error("Error while saving configuration", ex);
			}
		}

		public void Export(string filePath) {
			foreach (Construction construction in this.constructions) {
				construction.UpdateVersions();
			}

			if (Directory.Exists(filePath)) {
				try {
					XmlSerializer s = new XmlSerializer(typeof(Configuration));
					string filename = "";
					if (this.type == ConfigurationType.AdminConfiguration) {
						filename = Path.Combine(filePath, "global.conf");
					} else {
						log.Error("Export() has been called for configuration type which is not supported: " + this.type);
						return;
					}
					Stream w = new FileStream(filename, FileMode.Create);
					s.Serialize(w, this);
					w.Close();
				} catch (Exception ex) {
					log.Error("Error while exporting configuration", ex);
				}
			}
		}


	}

}
