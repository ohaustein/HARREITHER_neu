using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using log4net;
using System.Reflection;
using System.Globalization;
using System.Collections;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Europlan.Common {

	[XmlRootAttribute("Configuration")]
	[XmlInclude(typeof(string))]
	public class Configuration {

		private static Configuration adminTemplate = null;
		private static Configuration userTemplate = null;
		private static readonly object padlock = new object();
		//private static string appDataPath = Path.GetDirectoryName(System.Windows.Forms.Application.CommonAppDataPath);

		private static readonly ILog log = LogManager.GetLogger(typeof(Configuration));

		// IMPORTANT!!!
		//
		// do not forget to extend the initialization, the + operator when adding a new field and the copy constructor
		//
		// IMPORTANT!!!
		private ConfigurationType type;
		private List<Material> materials;
		private static List<Material> allMaterials;
		private List<Category> categories;
		private List<Construction> constructions;
		private SerializableDictionary<string, string> materialToCategoryMapping;
		private Dictionary<string, float> discounts;
		private List<RoomType> roomTypes;
		private List<string> materialIdsWithPricePerPackage;
		private SerializableDictionary<string, SerializableDictionary<string, string>> productConfiguration = new SerializableDictionary<string, SerializableDictionary<string,string>>();


		// if you add a product don't forget to call the Initialize() function in setter of ProductConfiguration
		/*private EurovalProduct eurovalProduct = new EurovalProduct();
		private EcothermProduct ecothermProduct = new EcothermProduct();
		private ConcreteActivationProduct concreteActivationProduct = new ConcreteActivationProduct();
		private HithermProduct hithermProduct = new HithermProduct();
		private HithermCompactProduct hithermCompactProduct = new HithermCompactProduct();
		private HithermCompactRoofProduct hithermCompactRoofProduct = new HithermCompactRoofProduct();
		private ModulKlimaBodenProduct modulKlimaBodenProduct = new ModulKlimaBodenProduct();
		private ModulKlimaDeckeProduct modulKlimaDeckeProduct = new ModulKlimaDeckeProduct();*/

		private string partnerLogo;

		public enum ConfigurationType {
			InitializedConfiguration,
			AdminConfiguration,
			UserConfiguration,
			ProjectConfiguration
		}

		public Configuration() {
			InitializeConfiguration();
		}

		public Configuration(Configuration config) {
			InitializeConfiguration();
			if (config.partnerLogo != "") {
				this.partnerLogo = config.partnerLogo;
			}
			foreach (Material material in config.Materials) {
				if (!this.Materials.Contains(material)) {
					this.Materials.Add(material);
				}
			}

			foreach (Construction construction in config.Constructions) {
				if (!this.Constructions.Contains(construction)) {
					this.Constructions.Add(construction);
				}
			}

			foreach (Category category in config.Categories) {
				if (!this.Categories.Contains(category)) {
					this.Categories.Add(category);
				}
			}

			foreach (string materialId in config.materialToCategoryMapping.Keys) {
				if (!this.materialToCategoryMapping.ContainsKey(materialId)) {
					this.materialToCategoryMapping.Add(materialId, config.materialToCategoryMapping[materialId]);
				}
			}

			foreach (RoomType roomType in config.RoomTypes) {
				if (!this.RoomTypes.Contains(roomType)) {
					this.RoomTypes.Add(roomType);
				}
			}

			foreach (String typeName in config.productConfiguration.Keys) {
				if (!this.productConfiguration.ContainsKey(typeName)) {
					this.productConfiguration[typeName] = new SerializableDictionary<string, string>();
				}
				foreach (string parameterName in config.productConfiguration[typeName].Keys) {
					if (!this.productConfiguration[typeName].ContainsKey(parameterName)) {
						this.productConfiguration[typeName][parameterName] = config.productConfiguration[typeName][parameterName];
					}
				}
			}

			foreach (String materialId in config.MaterialIdsWithPricePerPackage) {
				if (!this.materialIdsWithPricePerPackage.Contains(materialId)) {
					this.materialIdsWithPricePerPackage.Add(materialId);
				}
			}

			this.type = config.type;			 
		}

		private void InitializeConfiguration() {
			this.type = ConfigurationType.InitializedConfiguration;
			this.materials = new List<Material>();
			this.constructions = new List<Construction>();
			this.categories = new List<Category>();
			this.materialToCategoryMapping = new SerializableDictionary<string, string>();
			this.discounts = new Dictionary<string, float>();
			this.roomTypes = new List<RoomType>();
			this.materialIdsWithPricePerPackage = new List<string>();

			this.partnerLogo = "";

			if (allMaterials == null) {
				allMaterials = new List<Material>();
				StreamReader sr = null;
				try {
					sr = new StreamReader(Path.Combine(PathUtil.DataPath, "BruttoPreise.csv"), System.Text.Encoding.Default);
					string line;
					// ignore the first line (only header)
					line = sr.ReadLine();
					while ((line = sr.ReadLine()) != null) {
						string[] positions = line.Split(';');
						if (positions[0].CompareTo("AG01") >= 0 && positions[0].CompareTo("YY76") <= 0) {
							string id = positions[0].Trim();
							string name = positions[1].Trim();
							//string name2 = positions[5].Trim();
							//if (name2 != "") {
							//    name += " " + name2;
							//}
							float price = 0;
							float.TryParse(positions[2], NumberStyles.Any, CultureInfo.GetCultureInfo("de-AT").NumberFormat, out price);
							string discountGroup = "" /*positions[10].Trim()*/;
							string unit = positions[9].Trim();
							float denomination = 0;
							if (float.TryParse(positions[4], NumberStyles.Any, CultureInfo.GetCultureInfo("de-AT").NumberFormat, out denomination)) {
								allMaterials.Add(new Material(id, name, id, (int)denomination, unit, price, discountGroup, null, false, false));
							} else {
								allMaterials.Add(new Material(id, name, id, null, unit, price, discountGroup, null, false, false));
							}

						}
					}
				} catch (Exception ex) {
					// TODO
					// show messagebox to user
					log.Error("Error while parsing BruttoPreise.csv file", ex);
				} finally {
					if (sr != null) {
						sr.Close();
					}
				}
			}

			this.materials.AddRange(allMaterials);
			//try {
			//    sr = new StreamReader(Path.Combine(appDataPath, "DATANORM.RAB"), System.Text.Encoding.GetEncoding(850));
			//    string line;
			//    while ((line = sr.ReadLine()) != null) {
			//        if (line.StartsWith("R")) {
			//            string[] positions = line.Split(';');
			//            string discountGroup = positions[2].Trim();
			//            float discount = Int32.Parse(positions[4]) / 100;
			//            this.discounts.Add(discountGroup, discount);
			//        }
			//    }
			//} catch (Exception ex) {
			//    log.Error("Error while parsing DATANORM.RAB file", ex);
			//} finally {
			//    if (sr != null) {
			//        sr.Close();
			//    }
			//}
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

			if (first.partnerLogo != "") {
				config.partnerLogo = first.partnerLogo;
			}
			if (second.partnerLogo != "") {
				config.partnerLogo = second.partnerLogo;
			}
			foreach (Material material in first.Materials) {
				if (!config.Materials.Contains(material)) {
					config.Materials.Add(material);
				}
			}
			foreach (Material material in second.Materials) {
				if (!config.Materials.Contains(material)) {
					config.Materials.Add(material);
				}
			}

			foreach (Construction construction in first.Constructions) {
				if (!config.Constructions.Contains(construction)) {
					config.Constructions.Add(construction);
				}
			}

			foreach (Construction construction in second.Constructions) {
				if (!config.Constructions.Contains(construction)) {
					config.Constructions.Add(construction);
				}
			}

			foreach (Category category in first.Categories) {
				if (!config.Categories.Contains(category)) {
					config.Categories.Add(category);
				}
			}

			foreach (Category category in second.Categories) {
				if (!config.Categories.Contains(category)) {
					config.Categories.Add(category);
				}
			}

			foreach (string materialId in first.materialToCategoryMapping.Keys) {
				if (!config.materialToCategoryMapping.ContainsKey(materialId)) {
					config.materialToCategoryMapping.Add(materialId, first.materialToCategoryMapping[materialId]);
				}
			}

			foreach (string materialId in second.materialToCategoryMapping.Keys) {
				if (!config.materialToCategoryMapping.ContainsKey(materialId)) {
					config.materialToCategoryMapping.Add(materialId, second.materialToCategoryMapping[materialId]);
				}
			}

			foreach (RoomType roomType in first.RoomTypes) {
				if (!config.RoomTypes.Contains(roomType)) {
					config.RoomTypes.Add(roomType);
				}
			}

			foreach (RoomType roomType in second.RoomTypes) {
				if (!config.RoomTypes.Contains(roomType)) {
					config.RoomTypes.Add(roomType);
				}
			}

			foreach (String typeName in first.productConfiguration.Keys) {
				if (!config.productConfiguration.ContainsKey(typeName)) {
					config.productConfiguration[typeName] = new SerializableDictionary<string, string>();
				}
				foreach (string parameterName in first.productConfiguration[typeName].Keys) {
					if (!config.productConfiguration[typeName].ContainsKey(parameterName)) {
						config.productConfiguration[typeName][parameterName] = first.productConfiguration[typeName][parameterName];
					}
				}
			}

			foreach (String typeName in second.productConfiguration.Keys) {
				if (!config.productConfiguration.ContainsKey(typeName)) {
					config.productConfiguration[typeName] = new SerializableDictionary<string, string>();
				}
				foreach (string parameterName in second.productConfiguration[typeName].Keys) {
					if (!config.productConfiguration[typeName].ContainsKey(parameterName)) {
						config.productConfiguration[typeName][parameterName] = second.productConfiguration[typeName][parameterName];
					}
				}
			}

			foreach (String materialId in first.materialIdsWithPricePerPackage) {
				if (!config.materialIdsWithPricePerPackage.Contains(materialId)) {
					config.materialIdsWithPricePerPackage.Add(materialId);
				}
			}

			foreach (String materialId in second.materialIdsWithPricePerPackage) {
				if (!config.materialIdsWithPricePerPackage.Contains(materialId)) {
					config.materialIdsWithPricePerPackage.Add(materialId);
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
								Stream r = new FileStream(Path.Combine(PathUtil.DataPath, "global.conf"), FileMode.Open);
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

					EurovalProduct.StaticInitialize(adminTemplate);
					EcothermProduct.StaticInitialize(adminTemplate);
					ConcreteActivationProduct.StaticInitialize(adminTemplate);
					HithermProduct.StaticInitialize(adminTemplate);
					HithermCompactProduct.StaticInitialize(adminTemplate);
					HithermCompactRoofProduct.StaticInitialize(adminTemplate);
					ModulKlimaBodenProduct.StaticInitialize(adminTemplate);
					ModulKlimaDeckeProduct.StaticInitialize(adminTemplate);

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
								Stream r = new FileStream(Path.Combine(PathUtil.DataPath, "custom.conf"), FileMode.Open);
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

					EurovalProduct.StaticInitialize(userTemplate);
					EcothermProduct.StaticInitialize(userTemplate);
					ConcreteActivationProduct.StaticInitialize(userTemplate);
					HithermProduct.StaticInitialize(userTemplate);
					HithermCompactProduct.StaticInitialize(userTemplate);
					HithermCompactRoofProduct.StaticInitialize(userTemplate);
					ModulKlimaBodenProduct.StaticInitialize(userTemplate);
					ModulKlimaDeckeProduct.StaticInitialize(userTemplate);
				}
				return userTemplate;
			}
		}

		public static void ResetConfigurations() {
			adminTemplate = null;
			userTemplate = null;
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

		[XmlArray("PricePerPackage")]
		[XmlArrayItem("MaterialId")]
		public List<string> MaterialIdsWithPricePerPackage {
			get {
				return this.materialIdsWithPricePerPackage;
			}
			set {
				this.materialIdsWithPricePerPackage = value;
			}
		}

		/*[XmlArray("PricePerPackage")]
		[XmlArrayItem("blub")]
		public List<string> SerializableMaterialIdsWithPricePerPackage {
			get {
				if (this.type == ConfigurationType.AdminConfiguration) {
					List<string> materialIds = new List<string>();
					foreach (String materialId in this.materialIdsWithPricePerPackage.Keys) {
						materialIds.Add(materialId);
					}
					return materialIds;
				} else {
					return null;
				}
			}
			set {
				this.materialIdsWithPricePerPackage = new Hashtable();
				if (value != null) {
					foreach (string materialId in value) {
						this.materialIdsWithPricePerPackage.Add(materialId, null);
					}
				}
			}
		}*/

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

		public string PartnerLogo {
			get {
				if (type == ConfigurationType.UserConfiguration) {
					return this.partnerLogo;
				} else {
					return "";
				}
			}
			set {
				this.partnerLogo = value;
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
					//RecalculateMaterialToCategoryMapping();
				}
			}
		}

		public List<Material> SerializableMaterials {
			get {
				List<Material> materialList;
				if (type == ConfigurationType.ProjectConfiguration || type == ConfigurationType.UserConfiguration) {
					materialList = new List<Material>();
					foreach (Material material in this.materials) {
						if (material.UserDefined) {
							materialList.Add(material);
						}
					}
				} else if (type == ConfigurationType.AdminConfiguration || type == ConfigurationType.UserConfiguration) {
					materialList = new List<Material>();
					foreach (Material material in this.materials) {
						if (material.Additional) {
							materialList.Add(material);
						}
					}
				} else if (type == ConfigurationType.InitializedConfiguration) {
					materialList = this.materials;
				} else {
					materialList = new List<Material>();
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

		/*[XmlIgnore]
		public EurovalProduct EurovalProduct {
			get { return eurovalProduct; }
			set { eurovalProduct = value; }
		}

		[XmlIgnore]
		public EcothermProduct EcothermProduct {
			get { return ecothermProduct; }
			set { ecothermProduct = value; }
		}

		[XmlIgnore]
		public ConcreteActivationProduct ConcreteActivationProduct {
			get { return concreteActivationProduct; }
			set { concreteActivationProduct = value; }
		}

		[XmlIgnore]
		public HithermProduct HithermProduct {
			get { return hithermProduct; }
			set { hithermProduct = value; }
		}

		[XmlIgnore]
		public HithermCompactProduct HithermCompactProduct {
			get { return hithermCompactProduct; }
			set { hithermCompactProduct = value; }
		}

		[XmlIgnore]
		public HithermCompactRoofProduct HithermCompactRoofProduct {
			get { return hithermCompactRoofProduct; }
			set { hithermCompactRoofProduct = value; }
		}

		[XmlIgnore]
		public ModulKlimaBodenProduct ModulKlimaBodenProduct {
			get { return modulKlimaBodenProduct; }
			set { modulKlimaBodenProduct = value; }
		}

		[XmlIgnore]
		public ModulKlimaDeckeProduct ModulKlimaDeckeProduct {
			get { return modulKlimaDeckeProduct; }
			set { modulKlimaDeckeProduct = value; }
		}*/

		public P GetProduct<P>() where P : Product {
			if (typeof(P) == typeof(EurovalProduct)) {
				return new EurovalProduct() as P;
				//return this.EurovalProduct as P;
			} else if (typeof(P) == typeof(EcothermProduct)) {
				return new EcothermProduct() as P;
				//return this.EcothermProduct as P;
			} else if (typeof(P) == typeof(ConcreteActivationProduct)) {
				return new ConcreteActivationProduct() as P;
				//return this.ConcreteActivationProduct as P;
			} else if (typeof(P) == typeof(HithermProduct)) {
				return new HithermProduct() as P;
				//return this.HithermProduct as P;
			} else if (typeof(P) == typeof(HithermCompactProduct)) {
				return new HithermCompactProduct() as P;
				//return this.HithermCompactProduct as P;
			} else if (typeof(P) == typeof(HithermCompactRoofProduct)) {
				return new HithermCompactRoofProduct() as P;
				//return this.HithermCompactRoofProduct as P;
			} else if (typeof(P) == typeof(ModulKlimaBodenProduct)) {
				return new ModulKlimaBodenProduct() as P;
				//return this.ModulKlimaBodenProduct as P;
			} else if (typeof(P) == typeof(ModulKlimaDeckeProduct)) {
				return new ModulKlimaDeckeProduct() as P;
				//return this.ModulKlimaDeckeProduct as P;
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
				List<string> originalMaterialIdsWithPricePerPackage = this.MaterialIdsWithPricePerPackage;
				bool resetMaterialIdsWithPricePerPackage = false;
				if (this.type == ConfigurationType.AdminConfiguration) {
					filename = Path.Combine(PathUtil.DataPath, "global.conf");
				} else if (this.type == ConfigurationType.UserConfiguration) {
					resetMaterialIdsWithPricePerPackage = true;
					this.MaterialIdsWithPricePerPackage = new List<string>();
					filename = Path.Combine(PathUtil.DataPath, "custom.conf");
				} else {
					log.Error("Save() has been called for configuration type which is not supported: " + this.type);
					return;
				}
				Stream w = new FileStream(filename, FileMode.Create);
				s.Serialize(w, this);
				w.Close();

				FileUtils.SetAccessForEveryone(filename);

				if (resetMaterialIdsWithPricePerPackage) {
					this.MaterialIdsWithPricePerPackage = originalMaterialIdsWithPricePerPackage;
				}
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

					FileUtils.SetAccessForEveryone(filename);
				} catch (Exception ex) {
					log.Error("Error while exporting configuration", ex);
				}
			}
		}

		public Construction GetConstruction(string id) {
			foreach (Construction constr in this.Constructions) {
				if (constr.Id == id) {
					return constr;
				}
			}
			return null;
		}

		[XmlIgnore]
		public SerializableDictionary<string, SerializableDictionary<string, string>> InternalProductConfiguration {
			get { return this.productConfiguration; }
		}

		public SerializableDictionary<string, SerializableDictionary<string, string>> ProductConfiguration {
			get {
				Type[] types;
				/*if (this.type == ConfigurationType.UserConfiguration) {
					SerializableDictionary<string, SerializableDictionary<string, string>> result;
					result = new SerializableDictionary<string, SerializableDictionary<string, string>>();

					types = Assembly.GetExecutingAssembly().GetTypes();
					foreach (Type t in types) {
						if (typeof(Product).IsAssignableFrom(t)) {
							foreach (System.Reflection.PropertyInfo info in t.GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.GetProperty)) {
								object[] attributes = info.GetCustomAttributes(typeof(ProductParameterAttribute), false);
								if (attributes.Length > 0) {
									ProductParameterAttribute ppa = attributes[0] as ProductParameterAttribute;
									if (ppa.saveForUser) {
										string valueStr = this.GetProductParameter(t, info.Name);

										if (!result.ContainsKey(t.FullName)) {
											result[t.FullName] = new SerializableDictionary<string, string>();
										}
										result[t.FullName][info.Name] = valueStr;
									}
								}
							}
						}
					}

					return result;
				}*/
				SerializableDictionary<string, SerializableDictionary<string, string>> result;
				if (this.Type == ConfigurationType.UserConfiguration) {
					result = new SerializableDictionary<string,SerializableDictionary<string,string>>();
				} else {
					result = this.productConfiguration;
					result.Clear();
				}
				types = Assembly.GetExecutingAssembly().GetTypes();
				foreach (Type t in types) {
					if (typeof(Product).IsAssignableFrom(t)) {
						foreach (System.Reflection.PropertyInfo info in t.GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.GetProperty)) {
							object[] attributes = info.GetCustomAttributes(typeof(ProductParameterAttribute), false);
							if (attributes.Length > 0) {
								ProductParameterAttribute ppa = attributes[0] as ProductParameterAttribute;
								if (this.Type == ConfigurationType.UserConfiguration && ppa.saveForUser ||
									this.Type == ConfigurationType.ProjectConfiguration && ppa.saveInProject ||
									this.Type == ConfigurationType.AdminConfiguration ||
									this.Type == ConfigurationType.InitializedConfiguration) {
									object value = info.GetValue(null, null);
									string valueStr = null;
									if (value.GetType() == typeof(int)) {
										int i = (int)value;
										valueStr = i.ToString(CultureInfo.InvariantCulture.NumberFormat);
									} else if (value.GetType() == typeof(double)) {
										double d = (double)value;
										valueStr = d.ToString(CultureInfo.InvariantCulture.NumberFormat);
									} else if (value.GetType() == typeof(float)) {
										float f = (float)value;
										valueStr = f.ToString(CultureInfo.InvariantCulture.NumberFormat);
									} else if (value.GetType() == typeof(string)) {
										valueStr = (string)value;
									} else if (value.GetType() == typeof(bool)) {
										bool b = (bool)value;
										valueStr = b.ToString(CultureInfo.InvariantCulture.NumberFormat);
										/*} else if (value.GetType().IsSubclassOf(typeof(Enum))) {
											valueStr = Enum.GetName(value.GetType(), value);*/
									} else {
										log.Warn("Error when trying to get Product Configuration: Unknown type");
										continue;
									}
									this.AddProductParameter(t, info.Name, valueStr, result);
								}
							}
						}
					}
				}
				return result;
			}
			set { 
				this.productConfiguration = value;
				/*
				Type[] types = Assembly.GetExecutingAssembly().GetTypes();
				SerializableDictionary<string, string> current;
				foreach (Type t in types) {
					if (typeof(Product).IsAssignableFrom(t)) {
						if (this.productConfiguration.ContainsKey(t.FullName)) {
							current = this.productConfiguration[t.FullName];
							foreach (System.Reflection.PropertyInfo info in t.GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.GetProperty)) {
								try {
									if (current.ContainsKey(info.Name) && info.CanWrite) {
										if (info.PropertyType == typeof(int)) {
											int val = 0;
											if (int.TryParse(current[info.Name], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out val)) {
												info.SetValue(null, val, null);
											} else {
												log.Warn("Error when trying to set Product Configuration");
											}
										} else if (info.PropertyType == typeof(double)) {
											double val = 0;
											if (double.TryParse(current[info.Name], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out val)) {
												info.SetValue(null, val, BindingFlags.Static | BindingFlags.Public, null, null, null);
											} else {
												log.Warn("Error when trying to set Product Configuration");
											}
										} else if (info.PropertyType == typeof(float)) {
											float val = 0;
											if (float.TryParse(current[info.Name], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out val)) {
												info.SetValue(null, val, null);
											} else {
												log.Warn("Error when trying to set Product Configuration");
											}
										} else if (info.PropertyType == typeof(string)) {
											info.SetValue(null, current[info.Name], null);
										} else if (info.PropertyType == typeof(bool)) {
											bool val = false;
											if (bool.TryParse(current[info.Name], out val)) {
												info.SetValue(null, val, null);
											} else {
												log.Warn("Error when trying to set Product Configuration");
											}
											//} else if (info.PropertyType.IsSubclassOf(typeof(Enum))) {
											//	if (Enum.IsDefined(info.PropertyType, current[info.Name])) {
											//		info.SetValue(null, Enum.Parse(info.PropertyType, current[info.Name]), null);
											//	} else {
											//		log.Warn("Error when trying to set Product Configuration");
											//	}
										} else {
											log.Warn("Error when trying to set Product Configuration: Unknown type");
										}
									} else {
										if (!info.CanWrite) {
											log.Debug("Error product config property " + info.Name + " in product " + t.Name + " cannot be set");
										}
									}
								} catch (Exception ex) {
									throw ex;
								}
							}
						}
					}
				}
				*/
			}
		}

		public string GetProductParameter(Type productType, string parameterName) {
			if (!typeof(Product).IsAssignableFrom(productType)) {
				return null;
			}

			string defaultValue = null;
			if (this.type == ConfigurationType.AdminConfiguration) {
				object[] attributes = productType.GetProperty(parameterName).GetCustomAttributes(typeof(ProductParameterAttribute), false);
				if (attributes.Length > 0) {
					defaultValue = (attributes[0] as ProductParameterAttribute).DefaultValueAsString;
				}
			} else if (this.type == ConfigurationType.ProjectConfiguration) {
				defaultValue = Configuration.UserTemplate.GetProductParameter(productType, parameterName);
			} else if (this.type == ConfigurationType.UserConfiguration) {
				defaultValue = Configuration.AdminTemplate.GetProductParameter(productType, parameterName);
			}

			if (!this.productConfiguration.ContainsKey(productType.FullName)) {
				return defaultValue;
			}
			if (!this.productConfiguration[productType.FullName].ContainsKey(parameterName)) {
				return defaultValue;
			}

			return this.productConfiguration[productType.FullName][parameterName];
		}

		public string GetProductParameter<ProductType>(string parameterName) where ProductType : Product {
			/*if (!this.productConfiguration.ContainsKey(typeof(ProductType).FullName)) {
				return null;
			}
			if (!this.productConfiguration[typeof(ProductType).FullName].ContainsKey(parameterName)) {
				return null;
			}
			return this.productConfiguration[typeof(ProductType).FullName][parameterName];*/
			return this.GetProductParameter(typeof(ProductType), parameterName);
		}

		public object GetProductParameterAsNativeType<ProductType>(string parameterName) where ProductType : Product {
			object[] attributes = typeof(ProductType).GetProperty(parameterName).GetCustomAttributes(typeof(ProductParameterAttribute), false);
			if (attributes.Length == 0) {
				return null;
			}

			if (attributes[0] is BoolProductParameterAttribute) {
				string valueStr = GetProductParameter(typeof(ProductType), parameterName);
				bool value;
				if (bool.TryParse(valueStr, out value)) {
					return value;
				}
				return null;
			} else if (attributes[0] is IntProductParameterAttribute) {
				string valueStr = GetProductParameter(typeof(ProductType), parameterName);
				int value;
				if (int.TryParse(valueStr, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out value)) {
					return value;
				}
				return null;
			} else if (attributes[0] is FloatProductParameterAttribute) {
				string valueStr = GetProductParameter(typeof(ProductType), parameterName);
				float value;
				if (float.TryParse(valueStr, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out value)) {
					return value;
				}
				return null;
			} else if (attributes[0] is DoubleProductParameterAttribute) {
				string valueStr = GetProductParameter(typeof(ProductType), parameterName);
				double value;
				if (double.TryParse(valueStr, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out value)) {
					return value;
				}
				return null;
			} else if (attributes[0] is StringProductParameterAttribute) {
				return GetProductParameter(typeof(ProductType), parameterName);
			} else {
				return null;
			}

			/*if (!this.productConfiguration.ContainsKey(typeof(ProductType).FullName)) {
				return null;
			}
			if (!this.productConfiguration[typeof(ProductType).FullName].ContainsKey(parameterName)) {
				return null;
			}
			return this.productConfiguration[typeof(ProductType).FullName][parameterName];*/
			return this.GetProductParameter(typeof(ProductType), parameterName);
		}

		public int GetProductParameterAsInt<ProductType>(string parameterName) where ProductType : Product {
			Nullable<int> defaultValue = null;
			object[] attributes = typeof(ProductType).GetProperty(parameterName).GetCustomAttributes(typeof(IntProductParameterAttribute), false);
			if (attributes.Length > 0) {
				defaultValue = (attributes[0] as IntProductParameterAttribute).defaultValue;
			}

			if (!this.productConfiguration.ContainsKey(typeof(ProductType).FullName)) {
				if (!defaultValue.HasValue) {
					log.Warn("default value for parameter " + parameterName + " of " + typeof(ProductType).Name + " not found");
					return 0;
				}
				return defaultValue.Value;
			}

			if (!this.productConfiguration[typeof(ProductType).FullName].ContainsKey(parameterName)) {
				if (!defaultValue.HasValue) {
					log.Warn("default value for parameter " + parameterName + " of " + typeof(ProductType).Name + " not found");
					return 0;
				}
				return defaultValue.Value;
			}

			int value;
			if (!int.TryParse(this.productConfiguration[typeof(ProductType).FullName][parameterName], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out value)) {
				if (!defaultValue.HasValue) {
					log.Warn("default value for parameter " + parameterName + " of " + typeof(ProductType).Name + " not found");
					value = 0;
				} else {
					value = defaultValue.Value;
				}
			}

			return value;
		}

		public double GetProductParameterAsDouble<ProductType>(string parameterName) where ProductType : Product {
			Nullable<double> defaultValue = null;
			object[] attributes = typeof(ProductType).GetProperty(parameterName).GetCustomAttributes(typeof(DoubleProductParameterAttribute), false);
			if (attributes.Length > 0) {
				defaultValue = (attributes[0] as DoubleProductParameterAttribute).defaultValue;
			}

			if (!this.productConfiguration.ContainsKey(typeof(ProductType).FullName)) {
				if (!defaultValue.HasValue) {
					log.Warn("default value for parameter " + parameterName + " of " + typeof(ProductType).Name + " not found");
					return 0;
				}
				return defaultValue.Value;
			}

			if (!this.productConfiguration[typeof(ProductType).FullName].ContainsKey(parameterName)) {
				if (!defaultValue.HasValue) {
					log.Warn("default value for parameter " + parameterName + " of " + typeof(ProductType).Name + " not found");
					return 0;
				}
				return defaultValue.Value;
			}

			double value;
			if (!double.TryParse(this.productConfiguration[typeof(ProductType).FullName][parameterName], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out value)) {
				if (!defaultValue.HasValue) {
					log.Warn("default value for parameter " + parameterName + " of " + typeof(ProductType).Name + " not found");
					value = 0;
				} else {
					value = defaultValue.Value;
				}
			}

			return value;
		}

		public float GetProductParameterAsFloat<ProductType>(string parameterName) where ProductType : Product {
			Nullable<float> defaultValue = null;
			object[] attributes = typeof(ProductType).GetProperty(parameterName).GetCustomAttributes(typeof(FloatProductParameterAttribute), false);
			if (attributes.Length > 0) {
				defaultValue = (attributes[0] as FloatProductParameterAttribute).defaultValue;
			}

			if (!this.productConfiguration.ContainsKey(typeof(ProductType).FullName)) {
				if (!defaultValue.HasValue) {
					log.Warn("default value for parameter " + parameterName + " of " + typeof(ProductType).Name + " not found");
					return 0;
				}
				return defaultValue.Value;
			}

			if (!this.productConfiguration[typeof(ProductType).FullName].ContainsKey(parameterName)) {
				if (!defaultValue.HasValue) {
					log.Warn("default value for parameter " + parameterName + " of " + typeof(ProductType).Name + " not found");
					return 0;
				}
				return defaultValue.Value;
			}

			float value;
			if (!float.TryParse(this.productConfiguration[typeof(ProductType).FullName][parameterName], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out value)) {
				if (!defaultValue.HasValue) {
					log.Warn("default value for parameter " + parameterName + " of " + typeof(ProductType).Name + " not found");
					value = 0;
				} else {
					value = defaultValue.Value;
				}
			}

			return value;
		}

		public string GetProductParameterAsString<ProductType>(string parameterName) where ProductType : Product {
			string defaultValue = null;
			bool defaultValueFound = false;
			object[] attributes = typeof(ProductType).GetProperty(parameterName).GetCustomAttributes(typeof(StringProductParameterAttribute), false);
			if (attributes.Length > 0) {
				defaultValue = (attributes[0] as StringProductParameterAttribute).defaultValue;
				defaultValueFound = true;
			}

			if (!this.productConfiguration.ContainsKey(typeof(ProductType).FullName)) {
				if (!defaultValueFound) {
					log.Warn("default value for parameter " + parameterName + " of " + typeof(ProductType).Name + " not found");
					return "";
				}
				return defaultValue;
			}

			if (!this.productConfiguration[typeof(ProductType).FullName].ContainsKey(parameterName)) {
				if (!defaultValueFound) {
					log.Warn("default value for parameter " + parameterName + " of " + typeof(ProductType).Name + " not found");
					return "";
				}
				return defaultValue;
			}

			return this.productConfiguration[typeof(ProductType).FullName][parameterName];
		}

		public bool GetProductParameterAsBool<ProductType>(string parameterName) where ProductType : Product {
			Nullable<bool> defaultValue = null;
			object[] attributes = typeof(ProductType).GetProperty(parameterName).GetCustomAttributes(typeof(BoolProductParameterAttribute), false);
			if (attributes.Length > 0) {
				defaultValue = (attributes[0] as BoolProductParameterAttribute).defaultValue;
			}

			if (!this.productConfiguration.ContainsKey(typeof(ProductType).FullName)) {
				if (!defaultValue.HasValue) {
					log.Warn("default value for parameter " + parameterName + " of " + typeof(ProductType).Name + " not found");
					return false;
				}
				return defaultValue.Value;
			}

			if (!this.productConfiguration[typeof(ProductType).FullName].ContainsKey(parameterName)) {
				if (!defaultValue.HasValue) {
					log.Warn("default value for parameter " + parameterName + " of " + typeof(ProductType).Name + " not found");
					return false;
				}
				return defaultValue.Value;
			}

			bool value;
			if (!bool.TryParse(this.productConfiguration[typeof(ProductType).FullName][parameterName], out value)) {
				if (!defaultValue.HasValue) {
					log.Warn("default value for parameter " + parameterName + " of " + typeof(ProductType).Name + " not found");
					value = false;
				} else {
					value = defaultValue.Value;
				}
			}

			return value;
		}

		/*public EnumType GetProductParameterAsEnum<ProductType, EnumType>(string parameterName)
															where ProductType : Product {
			Nullable<EnumType> defaultValue = null;
			object[] attributes = typeof(ProductType).GetProperty(parameterName).GetCustomAttributes(typeof(EnumProductParameterAttribute), false);
			if (attributes.Length > 0) {
				defaultValue = (attributes[0] as EnumProductParameterAttribute).defaultValue;
			}

			if (!this.productConfiguration.ContainsKey(typeof(ProductType).FullName)) {
				if (!defaultValue.HasValue) {
					log.Warn("default value for parameter " + parameterName + " of " + typeof(ProductType).Name + " not found");
					return Enum.GetValues(typeof(EnumType))[0];
				}
				return defaultValue.Value;
			}

			if (!this.productConfiguration[typeof(ProductType).FullName].ContainsKey(parameterName)) {
				if (!defaultValue.HasValue) {
					log.Warn("default value for parameter " + parameterName + " of " + typeof(ProductType).Name + " not found");
					return Enum.GetValues(typeof(EnumType))[0];
				}
				return defaultValue.Value;
			}

			EnumType value;
			if (Enum.IsDefined(typeof(EnumType), this.productConfiguration[typeof(ProductType).FullName][parameterName])) {
				value = (EnumType)Enum.Parse(typeof(EnumType), this.productConfiguration[typeof(ProductType).FullName][parameterName]);
			} else {
				if (!defaultValue.HasValue) {
					log.Warn("default value for parameter " + parameterName + " of " + typeof(ProductType).Name + " not found");
					value = Enum.GetValues(typeof(EnumType))[0]; ;
				} else {
					value = defaultValue.Value;
				}
			}

			return value;
		}*/

		private void AddProductParameter(Type t, string parameterName, string value) {
			this.AddProductParameter(t, parameterName, value, this.productConfiguration);
		}

		private void AddProductParameter(Type t, string parameterName, string value, SerializableDictionary<string, SerializableDictionary<string, string>> dictionary) {
			if (!(typeof(Product).IsAssignableFrom(t))) {
				log.Warn("Cannot add parameters for non-Product types");
				return;
			}
			if (!dictionary.ContainsKey(t.FullName)) {
				dictionary[t.FullName] = new SerializableDictionary<string, string>();
			}
			dictionary[t.FullName][parameterName] = value;
		}

		public void AddProductParameter<T>(string parameterName, string value) where T : Product {
			this.AddProductParameter(typeof(T), parameterName, value);
			/*if (!this.productConfiguration.ContainsKey(typeof(T).FullName)) {
				this.productConfiguration[typeof(T).FullName] = new SerializableDictionary<string, string>();
			}
			this.productConfiguration[typeof(T).FullName][parameterName] = value;*/
		}

		public void AddProductParameter<T>(string parameterName, double value) where T : Product {
			this.AddProductParameter<T>(parameterName, value.ToString(CultureInfo.InvariantCulture.NumberFormat));
		}

		public void AddProductParameter<T>(string parameterName, float value) where T : Product {
			this.AddProductParameter<T>(parameterName, value.ToString(CultureInfo.InvariantCulture.NumberFormat));
		}

		public void AddProductParameter<T>(string parameterName, int value) where T : Product {
			this.AddProductParameter<T>(parameterName, value.ToString(CultureInfo.InvariantCulture.NumberFormat));
		}

		public void AddProductParameter<T>(string parameterName, bool value) where T : Product {
			this.AddProductParameter<T>(parameterName, value.ToString(CultureInfo.InvariantCulture.NumberFormat));
		}

		public void RemoveProductParameter<T>(string parameterName) where T : Product {
			if (!this.productConfiguration.ContainsKey(typeof(T).FullName)) {
				return;
			}
			if (this.productConfiguration[typeof(T).FullName].ContainsKey(parameterName)) {
				this.productConfiguration[typeof(T).FullName].Remove(parameterName);
			}
			if (this.productConfiguration[typeof(T).FullName].Count == 0) {
				this.productConfiguration.Remove(typeof(T).FullName);
			}
		}
	}

}
