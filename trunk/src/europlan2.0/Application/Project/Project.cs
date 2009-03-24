using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using System.IO;
using System.Xml.Serialization;

namespace Europlan.Application {

	[XmlRoot("project")]
	public class Project {

		private static Project instance = null;
		private static readonly object padlock = new object();
		private static readonly ILog log = LogManager.GetLogger(typeof(Project));
		
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
			projectCreated = DateTime.MinValue;
			projectLastChanged = DateTime.MinValue;
			projectName = "";
			projectContact = "";
			projectNotes = "";
			projectEditor = "";
		}

		[XmlAttribute("projectName")]
		public string ProjectName {
			get { return projectName; }
			set { projectName = value; }
		}

		[XmlAttribute("projectContact")]
		public string ProjectContact {
			get { return projectContact; }
			set { projectContact = value; }
		}

		[XmlAttribute("projectNotes")]
		public string ProjectNotes {
			get { return projectNotes; }
			set { projectNotes = value; }
		}

		[XmlAttribute("projectCreated")]
		public DateTime ProjectCreated {
			get { return projectCreated; }
			set { projectCreated = value; }
		}

		[XmlAttribute("projectLastChanged")]
		public DateTime ProjectLastChanged {
			get { return projectLastChanged; }
			set { projectLastChanged = value; }
		}

		[XmlAttribute("projectEditor")]
		public string ProjectEditor {
			get { return projectEditor; }
			set { projectEditor = value; }
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
				DateTime now = DateTime.Now;
				if (Instance.ProjectCreated == DateTime.MinValue) {
					Instance.ProjectCreated = now;
				}
				Instance.ProjectLastChanged = now;

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

	}
}
