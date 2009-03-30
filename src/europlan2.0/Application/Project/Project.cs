using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Threading;

namespace Europlan.Application {

	[XmlRootAttribute("Project")]
	public class Project {

		private static Project instance = null;
		private static readonly object padlock = new object();
		private static readonly ILog log = LogManager.GetLogger(typeof(Project));
		private System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
		
		private string projectName;
		private string projectContact;
		private string projectNotes;
		private DateTime projectCreated;
		private DateTime projectLastChanged;
		private string projectEditor;

		public delegate void ProjectLoadedHandler(object sender);
		public delegate void ProjectSavedHandler(object sender);

		public static event ProjectLoadedHandler ProjectLoaded;
		public static event ProjectSavedHandler ProjectSaved;

		List<Floor> floors;

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
							instance = new Project();
						}
					}
				}
				return instance;
			}
		}

		private void InitializeProject() {
			DateTime now = DateTime.Now;
			projectCreated = now;
			projectLastChanged = now;
			projectName = "";
			projectContact = "";
			projectNotes = "";
			projectEditor = "";

			floors = new List<Floor>();
		}

		public string ProjectName {
			get { return projectName; }
			set { projectName = value; }
		}

		public string ProjectContact {
			get { return projectContact; }
			set { projectContact = value; }
		}

		public string ProjectNotes {
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

		public List<Floor> Floors {
			get { return floors; }
			set { floors = value; }
		}

		public static void Load(string filename) {
			lock (padlock) {
				XmlSerializer s = new XmlSerializer(typeof(Project));
				TextReader r = new StreamReader(filename);
				instance = (Project)s.Deserialize(r);
				r.Close();
			}
			if (ProjectLoaded != null) {
				Project.ProjectLoaded(Instance);
			}
		}

		public static void Save(string filename) {
			lock (padlock) {
				Instance.ProjectLastChanged = DateTime.Now;

				XmlSerializer s = new XmlSerializer(typeof(Project));
				TextWriter w = new StreamWriter(filename);
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
				return Instance;
			}			
		}


		internal void InitializeTreeView(System.Windows.Forms.TreeView tree) {
			tree.Nodes.Clear();

			// root note
			string localized = resources.GetString("Project", Thread.CurrentThread.CurrentUICulture);
			TreeNode root = new TreeNode(localized == null ? "Projekt" : localized);
			tree.Nodes.Add(root);
			root.Tag = typeof(ProjectSummaryPanel);

			// building (floors and rooms)
			localized = resources.GetString("Floors", Thread.CurrentThread.CurrentUICulture);
			TreeNode floorsNode = new TreeNode(localized == null ? "Geschoﬂe" : localized);
			tree.Nodes.Add(floorsNode);
			foreach (Floor floor in floors) {
				floor.InitializeTree(floorsNode);
			}


			tree.ExpandAll();
		}
	}
}
