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

		private TreeNode rootNode = null;
		private TreeNode floorsNode = null;
		//private TreeNode facilityDetailsNode = null;
		private TreeNode regulatorCircuitsNode = null;
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
			floorsNode = new TreeNode(localized == null ? "Geschoﬂe" : localized);
			floorsNode.Tag = floors;

			//localized = resources.GetString("FacilityDetails", Thread.CurrentThread.CurrentUICulture);
			//facilityDetailsNode = new TreeNode(localized == null ? "Anlagedaten" : localized);
			//facilityDetailsNode.Tag = typeof(FacilityDetailsSummaryPanel);

			localized = resources.GetString("RegulatorCircuits", Thread.CurrentThread.CurrentUICulture);
			regulatorCircuitsNode = new TreeNode(localized == null ? "Regelkreise" : localized);
			regulatorCircuitsNode.Tag = typeof(RegulatorCircuitsSummaryPanel);

			localized = resources.GetString("QuickDimensioning", Thread.CurrentThread.CurrentUICulture);
			quickDimensioningNode = new TreeNode(localized == null ? "Fl‰chenaufstellung" : localized);
			quickDimensioningNode.Tag = quickDimensioning;

		}

		public string[] ProjectName {
			get { return projectName; }
			set { projectName = value; }
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
				instance.configuration = Configuration.UserTemplate + instance.configuration;
				instance.configuration.Type = Configuration.ConfigurationType.ProjectConfiguration;
				instance.configuration.RecalculateMaterialToCategoryMapping();
				instance.RecalculateQuickDimensioningRoomToProjectMapping();
			}
			if (ProjectLoaded != null) {
				Project.ProjectLoaded(Instance);
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
			//if (this.rootNode.Nodes.Count == 0 || this.rootNode.Nodes[0] != this.facilityDetailsNode) {
			//	this.rootNode.Nodes.Insert(0, this.facilityDetailsNode);
			//}
			// insert regulatory circuit node if missing
			if (this.rootNode.Nodes.Count == 0 || this.rootNode.Nodes[0] != this.regulatorCircuitsNode) {
				this.rootNode.Nodes.Insert(1, this.regulatorCircuitsNode);
			}
			// insert floors node if missing
			if (this.rootNode.Nodes.Count == 1 || this.rootNode.Nodes[1] != this.floorsNode) {
				this.rootNode.Nodes.Insert(2, this.floorsNode);
			}
			// insert quick dimensioning node if missing
			if (this.rootNode.Nodes.Count == 2 || this.rootNode.Nodes[2] != this.quickDimensioningNode) {
				this.rootNode.Nodes.Insert(3, this.quickDimensioningNode);
			}

			// remove other nodes
			while (this.rootNode.Nodes.Count > 3) {
				this.rootNode.Nodes.RemoveAt(3);
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

	}
}
