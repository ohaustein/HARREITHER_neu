using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.ComponentModel;
using Europlan.Licensing;

namespace Europlan.Common {

	[XmlRootAttribute("Project")]
	public class Project : IGuiRepresentation {

		private static Project instance = null;
		private static readonly object padlock = new object();
		private static readonly ILog log = LogManager.GetLogger(typeof(Project));

		private string projectNumber;
		private string[] projectName;
		private string[] projectContact;
		private string[] projectNotes;
		private DateTime projectCreated;
		private DateTime projectLastChanged;
		private string projectEditor;
		private string projectFileName;

		// facility details
		private int normOutsideTemperature;
		private bool variableSpreizung;
		private bool calculateCoolLoad;
		private int outsideTemperatureForCooling;
		private int relativeHumidity;
		private int insideTemperatureForCooling;

		private TreeNode rootNode = null;
		private TreeNode floorsNode = null;
		private TreeNode facilityDetailsNode = null;
		private TreeNode regulatorCircuitsNode = null;
		private TreeNode systemParametersNode = null;
		private TreeNode quickDimensioningNode = null;
		private TreeNode requiredMaterialNode = null;

		public delegate void ProjectLoadedHandler(object sender);
		public delegate void ProjectSavedHandler(object sender);

		public static event ProjectLoadedHandler ProjectLoaded;
		public static event ProjectSavedHandler ProjectSaved;

		FloorList floors;
		List<RegulatorCircuit> regulatorCircuits;
		private Configuration configuration = null;
		private QuickDimensioning quickDimensioning = null;
		private SerializableDictionary<string, double> requiredMaterialOverrides;
		private SerializableDictionary<string, double> requiredMaterialCalculated;

		private List<HithermWall> hithermWalls = null;
		private List<HithermWall> serializableHithermWalls = new List<HithermWall>();
		private List<HithermWall> hithermCompactWalls = null;
		private List<HithermWall> serializableHithermCompactWalls = new List<HithermWall>();
		//private List<HithermWall> defaultHithermWalls = new List<HithermWall>();

		protected Project() {
			log.Debug("default constructor called");
			InitializeProject();
		}

		public string EuroplanVersion {
			get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
			set { /*nothing to do here; this shall only be serialized but not loaded;*/ }
		}

		/// <summary>
		/// Gets the instance of the class (Singleton)
		/// </summary>
		public static Project Instance {
			get {
				if (instance == null) {
					lock (padlock) {
						if (instance == null) {
							try {
								instance = new Project();
							} catch {
								instance = null;
							} 
						}
					}
				}
				return instance;
			}
		}

		private void InitializeProject() {
			DateTime now = DateTime.Now;
			projectNumber = "";
			projectCreated = now;
			projectLastChanged = now;
			projectName = new string[] { "" };
			projectContact = new string[] { "" };
			projectNotes = new string[] { "" };
			projectEditor = "";
			projectFileName = "";
			normOutsideTemperature = -16;
			variableSpreizung = false;
			calculateCoolLoad = false;
			outsideTemperatureForCooling = 30;
			relativeHumidity = 50;
			insideTemperatureForCooling = 25;

			floors = new FloorList();
			regulatorCircuits = new List<RegulatorCircuit>();
			quickDimensioning = new QuickDimensioning();

			requiredMaterialOverrides = new SerializableDictionary<string, double>();
			requiredMaterialCalculated = new SerializableDictionary<string, double>();

			//configuration = Configuration.AdminTemplate + Configuration.UserTemplate;
			configuration = new Configuration(Configuration.UserTemplate);
			configuration.Type = Configuration.ConfigurationType.ProjectConfiguration;

			// root node
			string localized = EuroplanRes.General_Projekt;
			rootNode = new TreeNode(localized == null ? "Projekt" : localized);
			rootNode.Tag = this;
			rootNode.ImageIndex = 1;
			rootNode.SelectedImageIndex = 1;

			// building (floors and rooms)
			localized = EuroplanRes.General_Geschosse;
			floorsNode = new TreeNode(localized == null ? "Geschoﬂe" : localized);
			floorsNode.Tag = floors;

			localized = EuroplanRes.General_Anlagedaten;
			facilityDetailsNode = new TreeNode(localized == null ? "Anlagedaten" : localized);
			facilityDetailsNode.Tag = typeof(FacilityDetailsSummaryPanel);
			facilityDetailsNode.ImageIndex = 2;
			facilityDetailsNode.SelectedImageIndex = 2;

			localized = EuroplanRes.General_Regelkreise;
			regulatorCircuitsNode = new TreeNode(localized == null ? "Regelkreise" : localized);
			regulatorCircuitsNode.Tag = typeof(RegulatorCircuitsSummaryPanel);

			localized = EuroplanRes.General_Systemparameter;
			systemParametersNode = new TreeNode(localized == null ? "Systemparameter" : localized);
			systemParametersNode.Tag = typeof(SystemParametersPanel);

			localized = EuroplanRes.General_Flaechenausfstellung;
			quickDimensioningNode = new TreeNode(localized == null ? "Fl‰chenaufstellung" : localized);
			quickDimensioningNode.Tag = quickDimensioning;

			localized = EuroplanRes.General_Materialbedarf;
			requiredMaterialNode = new TreeNode(localized == null ? "Materialbedarf" : localized);
			requiredMaterialNode.Tag = typeof(RequiredMaterialPanel);

			/*ConstructionListWrapper wrapper = new ConstructionListWrapper(Configuration.ConfigurationType.UserConfiguration);
			wrapper.ConstructionScopeFilter = ConstructionScopeEnum.WallConstruction;
			foreach (WallConstruction wc in wrapper) {
				HithermWall w = new HithermWall(wc.Id, wc.Name, wc, null, null, false, null, -16, 30, true);
				defaultHithermWalls.Add(w);
			}*/
		}

		public string[] ProjectName {
			get { return projectName; }
			set { projectName = value; }
		}

		[XmlIgnore]
		public string ProjectFileName {
			get { return projectFileName; }
			set { projectFileName = value; }
		}

		public string ProjectNumber {
			get { return projectNumber; }
			set { projectNumber = value; }
		}

		public string[] ProjectContact {
			get { return projectContact; }
			set { projectContact = value; }
		}

		public string[] ProjectNotes {
			get { return projectNotes; }
			set { projectNotes = value; }
		}

		public DateTime ProjectCreated {
			get { return projectCreated; }
			set { projectCreated = value; }
		}

		public DateTime ProjectLastChanged {
			get { return projectLastChanged; }
			set { projectLastChanged = value; }
		}

		public string ProjectEditor {
			get { return projectEditor; }
			set { projectEditor = value; }
		}

		public int NormOutsideTemperature {
			get { return normOutsideTemperature; }
			set { normOutsideTemperature = value; }
		}

		public bool VariableSpreizung {
			get { return variableSpreizung; }
			set { variableSpreizung = value; }
		}

		public bool CalculateCoolLoad {
			get { return calculateCoolLoad; }
			set { calculateCoolLoad = value; }
		}

		public int OutsideTemperatureForCooling {
			get { return outsideTemperatureForCooling; }
			set { outsideTemperatureForCooling = value; }
		}
		
		public int RelativeHumidity {
			get { return relativeHumidity; }
			set { relativeHumidity = value; }
		}

		public int InsideTemperatureForCooling {
			get { return insideTemperatureForCooling; }
			set { insideTemperatureForCooling = value; }
		}

		public FloorList Floors {
			get { return floors; }
			set { floors = value; }
		}

		public Configuration Config {
			get { return configuration; }
			set { configuration = value; }
		}

		public List<RegulatorCircuit> RegulatorCircuits {
			get { return regulatorCircuits; }
			set { regulatorCircuits = value; }
		}

		public QuickDimensioning QuickDimensioning {
			get { return quickDimensioning; }
			set { quickDimensioning = value; }
		}

		public SerializableDictionary<string, double> RequiredMaterialOverrides {
			get { return requiredMaterialOverrides; }
			set { requiredMaterialOverrides = value; }
		}

		[XmlIgnore]
		public SerializableDictionary<string, double> RequiredMaterialCalculated {
			get { return requiredMaterialCalculated; }
			set { requiredMaterialCalculated = value; }
		}		

		/*public void SetFloors(List<Floor> floors) {
			this.floors.Clear();
			this.floors = new List<Floor>(floors);
		}*/

		public static void Load(string filename) {
			lock (padlock) {
				XmlSerializer s = new XmlSerializer(typeof(Project));
				Stream r = new FileStream(filename, FileMode.Open);
				try {
					instance = (Project)s.Deserialize(r);
				} finally {
					r.Close();
				}
				//instance.configuration = (Configuration.AdminTemplate + Configuration.UserTemplate) + instance.configuration;
				instance.configuration = Configuration.UserTemplate + instance.configuration;
				instance.configuration.Type = Configuration.ConfigurationType.ProjectConfiguration;
				instance.configuration.RecalculateMaterialToCategoryMapping();
				instance.RecalculateQuickDimensioningRoomToProjectMapping();
				instance.FinalizeLoading();
				instance.ProjectFileName = filename;
			}
			if (ProjectLoaded != null) {
				Project.ProjectLoaded(Instance);
			}
		}

		internal void FinalizeLoading() {
			foreach (Floor floor in this.floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						pp.Product.AssociatedRoom = room;
					}
				} 
			}
			foreach (Floor floor in this.floors) {
				floor.FinalizeLoading();
			}
		}

		private void RecalculateQuickDimensioningRoomToProjectMapping() {
			foreach (Floor floor in floors) {
				foreach (Room room in floor.Rooms) {
					foreach (Product product in room.UsedProductsForQuickDimensioning) {
						product.AssociatedRoom = room;
					}
				}
			}
		}

		public static void Save(string filename) {
			lock (padlock) {
				Instance.ProjectLastChanged = DateTime.Now;

				XmlSerializer s = new XmlSerializer(typeof(Project));
				Stream w = new FileStream(filename, FileMode.Create);
				Instance.ProjectFileName = filename;
				s.Serialize(w, Instance);
				w.Close();
			}
			if (ProjectSaved != null) {
				Project.ProjectSaved(Instance);
			}
		}

		public static Project New() {
			lock (padlock) {
				Instance.InitializeProject();
				string localized = EuroplanRes.General_Standardregelkreis;
				Instance.RegulatorCircuits.Add(new RegulatorCircuit(localized));
				return Instance;
			}			
		}


		public void InitializeTreeView(System.Windows.Forms.TreeView tree) {
			/*TreeNode selectedNode = null;
			if (tree.SelectedNode != null) {
				selectedNode = tree.SelectedNode;
			}
			tree.Nodes.Clear();
			rootNode.Nodes.Clear();
			floorsNode.Nodes.Clear();
			regulatorCircuitsNode.Nodes.Clear();

			tree.Nodes.Add(rootNode);
			rootNode.Tag = this;

			regulatorCircuitsNode.Tag = typeof(RegulatorCircuitsSummaryPanel);
			rootNode.Nodes.Add(regulatorCircuitsNode);

			floorsNode.Tag = floors;
			rootNode.Nodes.Add(floorsNode);
		
			foreach (Floor floor in floors) {
				floor.InitializeTree(floorsNode);
			}
			
			tree.ExpandAll();
			if (selectedNode != null) {
				tree.SelectedNode = selectedNode;
			} else {
				tree.SelectedNode = rootNode;
			}*/
			this.UpdateTreeView(tree);
		}

		internal void UpdateTreeView(System.Windows.Forms.TreeView tree) {
			TreeNode selectedNode = null;
			if (tree.SelectedNode != null) {
				selectedNode = tree.SelectedNode;
			}

			bool expand = (tree.Nodes.Count == 0 || this.rootNode.Nodes.Count == 0);
			// insert root node if missing
			if (tree.Nodes.Count != 1 || tree.Nodes[0] != this.rootNode) {
				tree.Nodes.Clear();
				tree.Nodes.Add(this.rootNode);
			}
			// insert facility details node if missing
			if (this.rootNode.Nodes.Count == 0 || this.rootNode.Nodes[0] != this.facilityDetailsNode) {
				this.rootNode.Nodes.Insert(0, this.facilityDetailsNode);
			}
			// insert regulatory circuit node if missing
			if (this.rootNode.Nodes.Count == 1 || this.rootNode.Nodes[1] != this.regulatorCircuitsNode) {
				this.rootNode.Nodes.Insert(1, this.regulatorCircuitsNode);
			}
			// insert system parameters node if missing
			if (this.rootNode.Nodes.Count == 2 || this.rootNode.Nodes[2] != this.systemParametersNode) {
				this.rootNode.Nodes.Insert(2, this.systemParametersNode);
			}
			// insert floors node if missing
			if (this.rootNode.Nodes.Count == 3 || this.rootNode.Nodes[3] != this.floorsNode) {
				this.rootNode.Nodes.Insert(3, this.floorsNode);
			}
			// insert quick dimensioning node if missing
			if (this.rootNode.Nodes.Count == 4 || this.rootNode.Nodes[4] != this.quickDimensioningNode) {
				this.rootNode.Nodes.Insert(4, this.quickDimensioningNode);
			}
			// insert required material node if missing
			if (this.rootNode.Nodes.Count == 5 || this.rootNode.Nodes[5] != this.requiredMaterialNode) {
				this.rootNode.Nodes.Insert(5, this.requiredMaterialNode);
			}

			// remove other nodes
			while (this.rootNode.Nodes.Count > 6) {
				this.rootNode.Nodes.RemoveAt(6);
			}

			// update floors
			this.floors.UpdateTree(floorsNode);

			if (expand) {
				this.rootNode.Expand();
			}

			if (selectedNode != null) {
				tree.SelectedNode = selectedNode;
			} else {
				tree.SelectedNode = rootNode;
			}
		}


		public Type AssociatedPanelType {
			get { 
				return typeof(ProjectSummaryPanel);
			}
		}

		public Icon AssociatedIcon {
			get { 
				return null; 
			}
		}

		public TreeNode FindNode(object element) {
			if (element == this) {
				return rootNode;
			} else if (element == typeof(FloorListSummaryPanel)) {
				return floorsNode;
			} else if (element == typeof(FacilityDetailsSummaryPanel)) {
				return facilityDetailsNode;
			} else if (element == typeof(RegulatorCircuitsSummaryPanel)) {
				return regulatorCircuitsNode;
			} else {
				foreach (Floor f in floors) {
					TreeNode node = f.FindNode(element);
					if (node != null) {
						return node;
					}
				}
			}
			return null;
		}

		public Distributor GetDistributor(string id) {
			foreach (Floor f in this.floors) {
				foreach (Distributor d in f.Distributors) {
					if (d.Id == id) {
						return d;
					}
				}
			}
			return null;
		}


		public void CalculateRequiredMaterial() {
			requiredMaterialCalculated = new SerializableDictionary<string, double>();
			foreach (Floor floor in this.floors) {
				// distributors
				foreach (Distributor distributor in floor.Distributors) {
					distributor.CalculateRequiredMaterial(requiredMaterialCalculated);
				}
				foreach (Room room in floor.Rooms) {
					// products
					foreach (PlannedProduct product in room.PlannedProducts) {
						product.Product.CalculateRequiredMaterial(requiredMaterialCalculated);
					}
				}
			}

			ModulKlimaBodenProduct.ReviseRequiredMaterial(requiredMaterialCalculated);
			HithermCompactProduct.ReviseRequiredMaterial(requiredMaterialCalculated);
		}

		public void AddRequiredMaterial(SerializableDictionary<string, double> requiredMaterial, string materialId, double amount) {
			Material material = this.Config.Materials.Find(delegate(Material m) { return m.Id == materialId; });
			if (material != null) {
				if (requiredMaterial.ContainsKey(material.Id)) {
					requiredMaterial[material.Id] += amount;
				} else {
					requiredMaterial.Add(material.Id, amount);
				}
			}
		}

		public PlannedProduct GetPlannedProduct(Product product) {
			foreach (Floor f in this.Floors) {
				foreach (Room r in f.Rooms) {
					foreach (PlannedProduct pp in r.PlannedProducts) {
						if (pp.Product == product) {
							return pp;
						}
					}
				}
			}
			return null;
		}

		[XmlIgnore]
		public List<HithermWall> HithermWalls {
			get {
				if (this.hithermWalls == null) {
					this.hithermWalls = new List<HithermWall>();

					ConstructionListWrapper wrapper = new ConstructionListWrapper(Configuration.ConfigurationType.UserConfiguration);
					wrapper.ConstructionScopeFilter = ConstructionScopeEnum.WallConstruction;
					foreach (WallConstruction wc in wrapper) {
						if (wc.IsHithermWall) {
							hithermWalls.Add(wc.DefaultWall);
						}
					}

					hithermWalls.AddRange(this.serializableHithermWalls);
				}

				return hithermWalls;
			}
		}

		public List<HithermWall> SerializeableHithermWalls {
			get {
				if (this.hithermWalls == null) {
					return this.serializableHithermWalls;
				} else {
					List<HithermWall> walls = new List<HithermWall>();
					foreach (HithermWall w in this.hithermWalls) {
						if (!w.DefaultWall) {
							walls.Add(w);
						}
					}
					return walls;
				}
			}
			set { this.serializableHithermWalls = value; }
		}

		[XmlIgnore]
		public List<HithermWall> HithermCompactWalls {
			get {
				if (this.hithermCompactWalls == null) {
					this.hithermCompactWalls = new List<HithermWall>();

					ConstructionListWrapper wrapper = new ConstructionListWrapper(Configuration.ConfigurationType.UserConfiguration);
					wrapper.ConstructionScopeFilter = ConstructionScopeEnum.WallConstruction;
					foreach (WallConstruction wc in wrapper) {
						if (wc.IsHithermCompactWall) {
							this.hithermCompactWalls.Add(wc.DefaultWall);
						}
					}

					this.hithermCompactWalls.AddRange(this.serializableHithermCompactWalls);
				}

				return this.hithermCompactWalls;
			}
		}

		public List<HithermWall> SerializeableHithermCompactWalls {
			get {
				if (this.hithermCompactWalls == null) {
					return this.serializableHithermCompactWalls;
				} else {
					List<HithermWall> walls = new List<HithermWall>();
					foreach (HithermWall w in this.hithermCompactWalls) {
						if (!w.DefaultWall) {
							walls.Add(w);
						}
					}
					return walls;
				}
			}
			set { this.serializableHithermCompactWalls = value; }
		}

		public string NotificationMessage {
			get {
				bool ecotherm = false;
				bool euroval = false;
				bool hitherm = false;
				bool hithermCompact = false;
				bool modulBoden = false;
				bool modulDecke = false;
				foreach (Floor f in this.floors) {
					foreach (Room r in f.Rooms) {
						foreach (PlannedProduct pp in r.PlannedProducts) {
							if (pp.Product is EcothermProduct) {
								ecotherm = true;
							}
							if (pp.Product is EurovalProduct) {
								euroval = true;
							}
							if (pp.Product is HithermProduct) {
								hitherm = true;
							}
							if (pp.Product is HithermCompactProduct) {
								hithermCompact = true;
							}
							if (pp.Product is ModulKlimaBodenProduct) {
								modulBoden = true;
							}
							if (pp.Product is ModulKlimaDeckeProduct) {
								modulDecke = true;
							}
						}
					}
				}
				string message = "";
				string add;
				if (ecotherm) {
					add = EcothermProduct.GlobalNotificationMessage;
					if (add != null) {
						message += "\n" + add;
					}
				}
				if (euroval) {
					add = EurovalProduct.GlobalNotificationMessage;
					if (add != null) {
						message += "\n" + add;
					}
				}
				if (hitherm) {
					add = HithermProduct.GlobalNotificationMessage;
					if (add != null) {
						message += "\n" + add;
					}
				}
				if (hithermCompact) {
					add = HithermCompactProduct.GlobalNotificationMessage;
					if (add != null) {
						message += "\n" + add;
					}
				}
				if (modulBoden) {
					add = ModulKlimaBodenProduct.GlobalNotificationMessage;
					if (add != null) {
						message += "\n" + add;
					}
				}
				if (modulDecke) {
					add = ModulKlimaDeckeProduct.GlobalNotificationMessage;
					if (add != null) {
						message += "\n" + add;
					}
				}
				if (message.Length > 0) {
					message = message.Substring(1);
				}
				if (message.Length == 0) {
					message = null;
				}
				return message;
			}
		}

		public string[] NotificationMessageArray {
			get {
				string notificationMsg = this.NotificationMessage;
				List<String> notifications = new List<string>();
				if (notificationMsg != null) {
					string[] messages = notificationMsg.Split('\n');
					foreach (string message in messages) {
						if (!string.IsNullOrEmpty(message)) {
							notifications.Add(message);
						}
					}
				}
				String[] rtn = new String[notifications.Count];
				notifications.CopyTo(rtn);
				return rtn;
			}
		}
	}
}
