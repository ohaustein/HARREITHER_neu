using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Europlan.Application {
	
	public class BuildingDataImportManager {

		private static BuildingDataImportManager instance = null;
		private static readonly object padlock = new object();
		private static readonly ILog log = LogManager.GetLogger(typeof(BuildingDataImportManager));
		private System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));

		private IList<IBuildingDataImporter> importers;

		protected BuildingDataImportManager() {
			log.Debug("default constructor called");

			importers = new List<IBuildingDataImporter>();

			Type[] types = Assembly.GetExecutingAssembly().GetTypes();
			foreach (Type t in types) {
				log.Debug("Checking type: " + t.ToString());
				Type[] interfaces = t.GetInterfaces();
				foreach (Type i in interfaces) {
					if (i.Equals(typeof(IBuildingDataImporter))) {
						log.Debug("Found importer: " + t.ToString());
						IBuildingDataImporter importer = (IBuildingDataImporter)Activator.CreateInstance(t);
						importers.Add(importer);
						continue;
					}
				}
			}
		}

		/// <summary>
		/// Gets the instance of the class (Singleton)
		/// </summary>
		public static BuildingDataImportManager Instance {
			get {
				if (instance == null) {
					lock (padlock) {
						if (instance == null) {
							instance = new BuildingDataImportManager();
						}
					}
				}
				return instance;
			}
		}

		public void ImportBuildingData() {
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.CheckFileExists = true;
			dialog.CheckPathExists = true;
			string filter = "";
			foreach (IBuildingDataImporter importer in importers) {
				filter += (importer.FileExtensionFilter + "|");
			}
			if (importers.Count > 1) {
				// TODO: implement "All importers (*.???);(*.???) ...
				//dialog.Filter = "Alle (*.xxx)|*.xxx";
			} 
			filter = filter.TrimEnd('|');
			dialog.Filter = filter;
			dialog.Multiselect = false;
			DialogResult result = dialog.ShowDialog();
			if (result == DialogResult.OK) {
				foreach (IBuildingDataImporter importer in importers) {
					if (importer.FileExtension.Equals(Path.GetExtension(dialog.FileName), StringComparison.InvariantCultureIgnoreCase)) {
						FloorList floors = importer.ImportBuildingDataFromFile(dialog.FileName);
						if (Project.Instance.Floors.Count != 0) {
							string message = resources.GetString("SyncMessage", Thread.CurrentThread.CurrentUICulture);
							string caption = resources.GetString("SyncCaption", Thread.CurrentThread.CurrentUICulture);
							result = MessageBox.Show(message, caption, MessageBoxButtons.YesNoCancel);
							if (result == DialogResult.Yes) {
								foreach (Floor floor in floors) {
									Floor f = Project.Instance.Floors.Find(delegate(Floor f1) { return f1.Id == floor.Id; });
									if (f != null) {
										f.Synchronize(floor);
									} else {
										Project.Instance.Floors.Add(floor);
									}
								}
							} else if (result == DialogResult.No) {
								Project.Instance.Floors = floors;
							}
						} else {
							Project.Instance.Floors = floors;
						}
					}
				}
			}
		}

	}

}
