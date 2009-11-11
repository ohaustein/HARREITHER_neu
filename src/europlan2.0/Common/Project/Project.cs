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

namespace Europlan.Common {

	[XmlRootAttribute("Project")]
	public class Project : IGuiRepresentation {

		private static Project instance = null;
		private static readonly object padlock = new object();
		private static readonly ILog log = LogManager.GetLogger(typeof(Project));
		private System.ComponentModel.ComponentResourceManager resources = ResourcesManager.resources;

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

		public delegate void ProjectLoadedHandler(object sender);
		public delegate void ProjectSavedHandler(object sender);

		public static event ProjectLoadedHandler ProjectLoaded;
		public static event ProjectSavedHandler ProjectSaved;

		FloorList floors;
		List<RegulatorCircuit> regulatorCircuits;
		private Configuration configuration = null;
		private QuickDimensioning quickDimensioning = null;

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

			configuration = Configuration.AdminTemplate + Configuration.UserTemplate;
			configuration.Type = Configuration.ConfigurationType.ProjectConfiguration;

			// root node
			string localized = resources.GetString("Project", Thread.CurrentThread.CurrentUICulture);
			rootNode = new TreeNode(localized == null ? "Projekt" : localized);
			rootNode.Tag = this;

			// building (floors and rooms)
			localized = resources.GetString("Floors", Thread.CurrentThread.CurrentUICulture);
			floorsNode = new TreeNode(localized == null ? "Geschoße" : localized);
			floorsNode.Tag = floors;

			localized = resources.GetString("FacilityDetails", Thread.CurrentThread.CurrentUICulture);
			facilityDetailsNode = new TreeNode(localized == null ? "Anlagedaten" : localized);
			facilityDetailsNode.Tag = typeof(FacilityDetailsSummaryPanel);

			localized = resources.GetString("RegulatorCircuits", Thread.CurrentThread.CurrentUICulture);
			regulatorCircuitsNode = new TreeNode(localized == null ? "Regelkreise" : localized);
			regulatorCircuitsNode.Tag = typeof(RegulatorCircuitsSummaryPanel);
			
			localized = resources.GetString("SystemParameters", Thread.CurrentThread.CurrentUICulture);
			systemParametersNode = new TreeNode(localized == null ? "Systemparameter" : localized);
			systemParametersNode.Tag = typeof(SystemParametersPanel);

			localized = resources.GetString("QuickDimensioning", Thread.CurrentThread.CurrentUICulture);
			quickDimensioningNode = new TreeNode(localized == null ? "Flächenaufstellung" : localized);
			quickDimensioningNode.Tag = quickDimensioning;

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

		/*public void SetFloors(List<Floor> floors) {
			this.floors.Clear();
			this.floors = new List<Floor>(floors);
		}*/

		public static void Load(string filename) {
			lock (padlock) {
				XmlSerializer s = new XmlSerializer(typeof(Project));
				Stream r = new FileStream(filename, FileMode.Open);
				instance = (Project)s.Deserialize(r);
				r.Close();
				instance.configuration = (Configuration.AdminTemplate + Configuration.UserTemplate) + instance.configuration;
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
				string localized = ResourcesManager.resources.GetString("DefaultRegulatorCircuits", Thread.CurrentThread.CurrentUICulture);
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

			// remove other nodes
			while (this.rootNode.Nodes.Count > 5) {
				this.rootNode.Nodes.RemoveAt(5);
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

		public List<ProjektBilanzWrapper> GetProkjektBilanzReport() {
			//TODO
			List<ProjektBilanzWrapper> wrapperList = new List<ProjektBilanzWrapper>();

			ProjektBilanzWrapper wrapper = new ProjektBilanzWrapper();
			wrapper.Description = "Gestamt-Normwärmebedarf";
			wrapperList.Add(wrapper);

			wrapper = new ProjektBilanzWrapper();
			wrapper.Description = "Gesamter bereinigter Wärmebedarf";
			wrapperList.Add(wrapper);

			wrapper = new ProjektBilanzWrapper();
			wrapper.Description = "Gestamt-Heizleistung (nach innen)";
			wrapperList.Add(wrapper);

			wrapper = new ProjektBilanzWrapper();
			wrapper.Description = "Gesamte aufgenommene Leistung";
			wrapperList.Add(wrapper);

			wrapper = new ProjektBilanzWrapper();
			wrapper.Description = "Gestamt-Wassermenge";
			wrapperList.Add(wrapper);

			wrapper = new ProjektBilanzWrapper();
			wrapper.Description = "Maximaler Druckverlust (inkl. Verteiler)";
			wrapperList.Add(wrapper);

			wrapper = new ProjektBilanzWrapper();
			wrapper.Description = "Gesamt-Wasserinhalt (ab Verteiler)";
			wrapperList.Add(wrapper);

			wrapper = new ProjektBilanzWrapper();
			wrapper.Description = "Gestamt-Raumfläche";
			wrapperList.Add(wrapper);

			wrapper = new ProjektBilanzWrapper();
			wrapper.Description = "Gestamt-Fußbodenheizungsfläche";
			wrapperList.Add(wrapper);

			wrapper = new ProjektBilanzWrapper();
			wrapper.Description = "Gestamt-Wandheizungsfläche";
			wrapperList.Add(wrapper);

			wrapper = new ProjektBilanzWrapper();
			wrapper.Description = "Gestamt-Deckenkühlungsfläche";
			wrapperList.Add(wrapper);

			return wrapperList;
		}

		public List<ProjectWarningWrapper> GetProjectWarningReport() {
			//TODO
			//foreach (Floor floor in this.floors) {
			//    foreach (Room room in floor.Rooms) {
			//        foreach (PlannedProduct plannedProduct in room.PlannedProducts) {
						
			//        }
			//    }
			//}
			List<ProjectWarningWrapper> wrapperList = new List<ProjectWarningWrapper>();

			ProjectWarningWrapper wrapper = new ProjectWarningWrapper();
			wrapper.FloorId = "KG1";
			wrapper.FloorName = "Keller";
			wrapper.Warning = "WARNUNG FBH in K06....";
			wrapperList.Add(wrapper);

			wrapper = new ProjectWarningWrapper();
			wrapper.FloorId = "KG1";
			wrapper.FloorName = "Keller";
			wrapper.Warning = "WARNUNG FBH in K08....";
			wrapperList.Add(wrapper);

			wrapper = new ProjectWarningWrapper();
			wrapper.FloorId = "EG1";
			wrapper.FloorName = "Erdgeschoß";
			wrapper.Warning = "FEHLER WH in E02....";
			wrapperList.Add(wrapper);

			return wrapperList;
		}

		public List<FloorOverviewWrapper> GetFloorOverviewWrapper() {
			List<FloorOverviewWrapper> wrapperListHeat = new List<FloorOverviewWrapper>();
			List<FloorOverviewWrapper> wrapperListCool = new List<FloorOverviewWrapper>();
			//TODO
			foreach (Floor floor in this.floors) {
			//    foreach (Room room in floor.Rooms) {
			//        foreach (PlannedProduct plannedProduct in room.PlannedProducts) {
				FloorOverviewWrapper wrapper = new FloorOverviewWrapper();
				wrapper.HeatOrCool = "Heizbetrieb";
				wrapper.FloorName = floor.Name;
				wrapper.FloorArea = 242.3;
				wrapper.QH2o = 3568;
				wrapper.Q = 3401;
				wrapper.TransmissionFloor = 10;
				wrapper.TransmissionWall = 0;
				wrapper.TransmissionCeiling = 68.7;
				wrapperListHeat.Add(wrapper);

				wrapper = new FloorOverviewWrapper();
				wrapper.HeatOrCool = "Kühlbetrieb";
				wrapper.FloorName = floor.Name;
				wrapper.FloorArea = 42.3;
				wrapper.QH2o = 3568;
				wrapper.Q = 3401;
				wrapper.TransmissionFloor = 10;
				wrapper.TransmissionWall = 0;
				wrapper.TransmissionCeiling = 68.7;
				wrapperListCool.Add(wrapper);
			//        }
			//    }
			}

			wrapperListHeat.AddRange(wrapperListCool);
			return wrapperListHeat;
		}

		public List<OpenLoadForRoomWrapper> GetOpenHeatLoadForRoomWrapper() {
			List<OpenLoadForRoomWrapper> wrapperList = new List<OpenLoadForRoomWrapper>();

			//TODO

			OpenLoadForRoomWrapper wrapper = new OpenLoadForRoomWrapper();
			wrapper.RoomId = "E01";
			wrapper.RoomName = "Wohnzimmer";
			wrapper.RequiredLoad = 1650.0;
			wrapper.NetLoad = 1650.0;
			wrapper.Power = 1400.0;
			wrapperList.Add(wrapper);

			wrapper = new OpenLoadForRoomWrapper();
			wrapper.RoomId = "E03";
			wrapper.RoomName = "Küche";
			wrapper.RequiredLoad = 1230.9;
			wrapper.NetLoad = 1150.0;
			wrapper.Power = 1002.0;
			wrapperList.Add(wrapper);

			return wrapperList;
		}

		public List<OpenLoadForRoomWrapper> GetOpenCoolLoadForRoomWrapper() {
			List<OpenLoadForRoomWrapper> wrapperList = new List<OpenLoadForRoomWrapper>();

			// TODO

			OpenLoadForRoomWrapper wrapper = new OpenLoadForRoomWrapper();
			wrapper.RoomId = "E01";
			wrapper.RoomName = "Wohnzimmer";
			wrapper.RequiredLoad = 1650.0;
			wrapper.NetLoad = 1650.0;
			wrapper.Power = 1400.0;
			wrapperList.Add(wrapper);

			wrapper = new OpenLoadForRoomWrapper();
			wrapper.RoomId = "E03";
			wrapper.RoomName = "Küche";
			wrapper.RequiredLoad = 1230.9;
			wrapper.NetLoad = 1150.0;
			wrapper.Power = 1002.0;
			wrapperList.Add(wrapper);

			return wrapperList;
		}

		public List<RegulatorCircuitWrapper> GetRegulatorCircuitWrapper() {
			// TODO
			
			List<RegulatorCircuitWrapper> wrapperHeatList = new List<RegulatorCircuitWrapper>();
			List<RegulatorCircuitWrapper> wrapperCoolList = new List<RegulatorCircuitWrapper>();

			RegulatorCircuitWrapper wrapper;

			foreach (RegulatorCircuit rc in this.RegulatorCircuits) {
				wrapper = new RegulatorCircuitWrapper();
				wrapper.HeatOrCool = "Heizbetrieb";
				wrapper.Id = rc.Id;
				wrapper.Name = rc.Name;
				wrapper.Medium = "Wasser";
				wrapper.VorlaufTemp = rc.HeatFlowTemperature;
				wrapper.RuecklaufTemp = rc.HeatFlowTemperature - 7;
				wrapper.Durchfluss = 1068;
				wrapper.Druckverlust = 105;
				wrapper.Inhalt = 87;
				wrapperHeatList.Add(wrapper);
								wrapper = new RegulatorCircuitWrapper();
				wrapper.HeatOrCool = "Kühlbetrieb";
				wrapper.Id = rc.Id;
				wrapper.Name = rc.Name;
				wrapper.Medium = "Wasser";
				wrapper.VorlaufTemp = rc.CoolFlowTemperature;
				wrapper.RuecklaufTemp = rc.CoolFlowTemperature + 3;
				wrapper.Durchfluss = 1068;
				wrapper.Druckverlust = 105;
				wrapper.Inhalt = 87;
				wrapperCoolList.Add(wrapper);
			}
			
			wrapperHeatList.AddRange(wrapperCoolList);

			return wrapperHeatList;
		}

		public List<DistributorWrapper> GetDistributorWrapper() {
			// TODO

			List<DistributorWrapper> wrapperHeatList = new List<DistributorWrapper>();
			List<DistributorWrapper> wrapperCoolList = new List<DistributorWrapper>();

			DistributorWrapper wrapper;

			foreach (Floor floor in this.Floors) {
				foreach (Distributor distributor in floor.Distributors) {
					wrapper = new DistributorWrapper();
					wrapper.HeatOrCool = "Heizbetrieb";
					wrapper.Id = distributor.Id;
					wrapper.Name = distributor.Name;
					wrapper.Groups = 11;
					wrapper.RegulatorCircuit = distributor.RegulatorCircuitId;
					wrapper.VorlaufTemp = distributor.RegulatorCircuit.HeatFlowTemperature;
					wrapper.RuecklaufTemp = distributor.RegulatorCircuit.HeatFlowTemperature - 7;
					wrapper.Durchfluss = 1068;
					wrapper.Druckverlust = 105;
					wrapper.Inhalt = 87;
					wrapperHeatList.Add(wrapper);
					wrapper = new DistributorWrapper();
					wrapper.HeatOrCool = "Kühlbetrieb";
					wrapper.Id = distributor.Id;
					wrapper.Name = distributor.Name;
					wrapper.Groups = 11;
					wrapper.RegulatorCircuit = distributor.RegulatorCircuitId;
					wrapper.VorlaufTemp = distributor.RegulatorCircuit.CoolFlowTemperature;
					wrapper.RuecklaufTemp = distributor.RegulatorCircuit.CoolFlowTemperature + 3;
					wrapper.Durchfluss = 1068;
					wrapper.Druckverlust = 105;
					wrapper.Inhalt = 87;
					wrapperCoolList.Add(wrapper);
				}
			}

			wrapperHeatList.AddRange(wrapperCoolList);

			return wrapperHeatList;
		}
	}
}
