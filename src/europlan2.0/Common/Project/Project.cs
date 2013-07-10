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
using WW.Math;

namespace Europlan.Common {

	[XmlRootAttribute("Project")]
	public class Project : IGuiRepresentation {

		private static Project instance = null;
		private static readonly object padlock = new object();
		private static readonly ILog log = LogManager.GetLogger(typeof(Project));

        private static string autosaveExtension = ".as";

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
		private TreeNode importedPlansNode = null;

		public delegate void ProjectLoadedHandler(object sender);
		public delegate void ProjectSavedHandler(object sender);

		public static event ProjectLoadedHandler ProjectLoaded;
		public static event ProjectSavedHandler ProjectSaved;

		FloorList floors;
		List<RegulatorCircuit> regulatorCircuits;
		List<Plan> importedPlans;
		private Configuration configuration = null;
		private QuickDimensioning quickDimensioning = null;
		private SerializableDictionary<string, double> requiredMaterialOverrides;
		private SerializableDictionary<string, double> requiredMaterialCalculated;

		private List<HithermWall> hithermWalls = null;
		private List<HithermWall> serializableHithermWalls = new List<HithermWall>();
		private List<HithermWall> hithermCompactWalls = null;
		private List<HithermWall> serializableHithermCompactWalls = new List<HithermWall>();

		protected Project() {
			log.Debug("default constructor called");
			InitializeProject();
		}

		private string projectEuroplanVersion = null;

		public string EuroplanVersion {
			get { return this.EuroplanVersionObj.ToString(); }
			set {
				this.projectEuroplanVersion = value;
			}
		}

        [XmlIgnore]
        public Version EuroplanVersionObj {
            get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version; }
        }

		[XmlIgnore]
		public Version ProjectEuroplanVersion {
			get {
                if (this.projectEuroplanVersion == null) {
                    return this.EuroplanVersionObj;
                }
                return new Version(this.projectEuroplanVersion);
            }
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
			insideTemperatureForCooling = 26;

			floors = new FloorList();
			regulatorCircuits = new List<RegulatorCircuit>();
			quickDimensioning = new QuickDimensioning();
			importedPlans = new List<Plan>();

			requiredMaterialOverrides = new SerializableDictionary<string, double>();
			requiredMaterialCalculated = new SerializableDictionary<string, double>();

			configuration = new Configuration(Configuration.UserTemplate);
			configuration.Type = Configuration.ConfigurationType.ProjectConfiguration;

			// root node
			string localized = EuroplanRes.General_Projekt;
			rootNode = new TreeNode(localized == null ? "Projekt" : localized);
			rootNode.Tag = this;
			rootNode.ImageKey = "Projekt.png";
			rootNode.SelectedImageKey = "Projekt.png";

			// building (floors and rooms)
			localized = EuroplanRes.General_Geschosse;
			floorsNode = new TreeNode(localized == null ? "Geschoﬂe" : localized);
			floorsNode.Tag = floors;
			floorsNode.ImageKey = "Geschoﬂ.png";
			floorsNode.SelectedImageKey = "Geschoﬂ.png";

			localized = EuroplanRes.General_Anlagedaten;
			facilityDetailsNode = new TreeNode(localized == null ? "Anlagedaten" : localized);
			facilityDetailsNode.Tag = typeof(FacilityDetailsSummaryPanel);
			facilityDetailsNode.ImageKey = "Anlagedaten.png";
			facilityDetailsNode.SelectedImageKey = "Anlagedaten.png";

			localized = EuroplanRes.General_Regelkreise;
			regulatorCircuitsNode = new TreeNode(localized == null ? "Regelkreise" : localized);
			regulatorCircuitsNode.Tag = typeof(RegulatorCircuitsSummaryPanel);
			regulatorCircuitsNode.ImageKey = "Regelkreise.png";
			regulatorCircuitsNode.SelectedImageKey = "Regelkreise.png";

			localized = EuroplanRes.General_Systemparameter;
			systemParametersNode = new TreeNode(localized == null ? "Systemparameter" : localized);
			systemParametersNode.Tag = typeof(SystemParametersPanel);
			systemParametersNode.ImageKey = "Systemparameter.png";
			systemParametersNode.SelectedImageKey = "Systemparameter.png";

			localized = EuroplanRes.General_Flaechenausfstellung;
			quickDimensioningNode = new TreeNode(localized == null ? "Fl‰chenaufstellung" : localized);
			quickDimensioningNode.Tag = quickDimensioning;
			quickDimensioningNode.ImageKey = "Fl‰chenaufstellung.png";
			quickDimensioningNode.SelectedImageKey = "Fl‰chenaufstellung.png";

			localized = EuroplanRes.General_Materialbedarf;
			requiredMaterialNode = new TreeNode(localized == null ? "Materialbedarf" : localized);
			requiredMaterialNode.Tag = typeof(RequiredMaterialPanel);
			requiredMaterialNode.ImageKey = "Materialbedarf.png";
			requiredMaterialNode.SelectedImageKey = "Materialbedarf.png";

			localized = EuroplanRes.General_ImportiertePlaene;
			importedPlansNode = new TreeNode(localized == null ? "Importierte Pl‰ne" : localized);
			importedPlansNode.Tag = typeof(ImportedPlansPanel);
			importedPlansNode.ImageKey = "Plaene.png";
			importedPlansNode.SelectedImageKey = "Plaene.png";
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

		public List<Plan> ImportedPlans {
			get { return importedPlans; }
			set { importedPlans = value; }
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

		public bool ProjectVersionCompatible() {
            Version projVers = instance.ProjectEuroplanVersion;
            Version curVers = instance.EuroplanVersionObj;

            return projVers.CompareTo(curVers) <= 0;
		}

        public static string AutoSaveFilename(string filename) {
            return filename + Project.autosaveExtension;
        }

		public static void Load(string filename) {
			lock (padlock) {
				Project old = instance;
				XmlSerializer s = new XmlSerializer(typeof(Project));
				Stream r = new FileStream(filename, FileMode.Open);
				try {
					instance = (Project)s.Deserialize(r);
				} finally {
					r.Close();
				}
				if (!instance.ProjectVersionCompatible()) {
					if (MessageBox.Show(EuroplanRes.ProjectLoad_VersionNichtKompatibelText, EuroplanRes.ProjectLoad_VersionNichtKompatibelTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) {
						throw new ProjectVersionNotCompatibleException();
					}
				}
                if (File.Exists(AutoSaveFilename(filename))) {
                    s = new XmlSerializer(typeof(Project));
                    r = new FileStream(AutoSaveFilename(filename), FileMode.Open);
                    Project asProject = null;
                    try {
                        asProject = (Project)s.Deserialize(r);
                    } catch (Exception) {
                        // nothing to do
                    } finally {
                        r.Close();
                    }
                    if (asProject != null) {
                        if (!asProject.ProjectVersionCompatible()) {
                            File.Delete(AutoSaveFilename(filename));
                        } else {
                            string message = "Das Projekt wurde zuletzt am %CHANGEDATE% gespeichert, es wurde aber eine automatisch gespeicherte Sicherung vom %AUTOSAVEDATE% gefunden. Wollen Sie diese Sicherung wiederherstellen?";
                            message = message.Replace("%CHANGEDATE%", instance.ProjectLastChanged.ToShortDateString() + " " + instance.ProjectLastChanged.ToShortTimeString());
                            message = message.Replace("%AUTOSAVEDATE%", asProject.ProjectLastChanged.ToShortDateString() + " " + asProject.ProjectLastChanged.ToShortTimeString());
                            if (MessageBox.Show(message, "Automatische Sicherung gefunden", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                                File.Delete(filename);
                                File.Move(AutoSaveFilename(filename), filename);
                                instance = asProject;
                            } else {
                                File.Delete(AutoSaveFilename(filename));
                            }
                        }
                    } else {
                        File.Delete(AutoSaveFilename(filename));
                    }
                }
				instance.configuration.Type = Configuration.ConfigurationType.ProjectConfiguration;
				instance.configuration = Configuration.UserTemplate + instance.configuration;
				instance.configuration.RecalculateMaterialToCategoryMapping();
				instance.FixOldProjects();
				instance.RecalculateQuickDimensioningRoomToProjectMapping();
				instance.InitializeProductParameters();
				instance.FinalizeLoading();
				instance.ProjectFileName = filename;

				if (instance.ImportedPlans.Count > 0) {
					List<Plan> toDelete = new List<Plan>();
					foreach (Plan plan in instance.ImportedPlans) {
						if (!File.Exists(plan.AbsoluteFileName)) {
							string text = EuroplanRes.Project_PlanFehltText;
							text = text.Replace("%PLANNAME%", plan.Name);
							text = text.Replace("%FILENAME%", plan.AbsoluteFileName);
							DialogResult result = MessageBox.Show(text, EuroplanRes.Project_PlanFehltTitel, MessageBoxButtons.YesNoCancel);
							if (result == DialogResult.Yes) {
								OpenFileDialog dialog = new OpenFileDialog();
								dialog.CheckFileExists = true;
								dialog.CheckPathExists = true;
								if (plan is CadPlan) {
									dialog.DefaultExt = "dxf";
									dialog.Filter = "CAD|*.dxf;*.dwg";
								} else if (plan is ImagePlan) {
									dialog.DefaultExt = Path.GetExtension(plan.AbsoluteFileName);
									dialog.Filter = dialog.DefaultExt + "|*." + dialog.DefaultExt;
								}
								dialog.Multiselect = false;
								result = dialog.ShowDialog();
								if (result == DialogResult.OK) {
									string dir = Path.GetDirectoryName(Project.Instance.ProjectFileName);
									string subDir = Path.GetFileNameWithoutExtension(Project.Instance.ProjectFileName) + "_plans";
									dir = Path.Combine(dir, subDir);
									if (!Directory.Exists(dir)) {
										Directory.CreateDirectory(dir);
									}
									string newFileName = Path.Combine(dir, Path.GetFileName(dialog.FileName));
									if (!dialog.FileName.Equals(newFileName)) {
										File.Copy(dialog.FileName, newFileName, true);
									}

									plan.RelativeFileName = Path.Combine(subDir, Path.GetFileName(dialog.FileName));	
								}
								dialog.Dispose();
							} else if (result == DialogResult.No) {
								toDelete.Add(plan);
							} else {
								instance = old;
								break;
							}
						}
					}
					if (toDelete.Count > 0) {
						foreach (Plan plan in toDelete) {
							instance.ImportedPlans.Remove(plan);
							foreach (Floor floor in instance.Floors) {
								if (floor.AssociatedPlanId == plan.Id) {
									floor.AssociatedPlanId = null;
									foreach (Room room in floor.Rooms) {
										room.RoomCoordinates = new List<Point2D>();
										room.CeilingCoordinates = new List<Point2D>();
										room.RoomUnusedAreaCoordinates = new List<List<Point2D>>();
										room.CeilingUnusedAreaCoordinates = new List<List<Point2D>>();
										foreach (PlannedProduct pp in room.PlannedProducts) {
											if (pp.Product.GraphicalMode.HasValue && pp.Product.GraphicalMode.Value) {
												pp.Product.GraphicalMode = false;
											}
										}
									}
								}
							}
						}
					}
				}
			}
			if (ProjectLoaded != null) {
				Project.ProjectLoaded(Instance);
				// calculate all products
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						foreach (PlannedProduct p in room.PlannedProducts) {
							p.ConfigureProduct(false);
						}
					}
				}
			}
		}

		private void FixOldProjects() {
			foreach (Floor f in Project.Instance.Floors) {
				foreach (Room r in f.Rooms) {
					foreach (PlannedProduct pp in r.PlannedProducts) {
						// for old projects where CalculateMode has not existed yet
						if (pp.Product.CalculateMode == Product.CalculateModeEnum.NONE) {
							bool heat = false;
							bool cool = false;
							if (pp.CoverHeatLoad) {
								heat = true;
							} else {
								heat = pp.RequestedHeatLoad > 0;
							}
							if (pp.CoverCoolLoad) {
								cool = true;
							} else {
								cool = pp.RequestedCoolLoad > 0;
							}
							if (heat && cool) {
								pp.Product.CalculateMode = Product.CalculateModeEnum.HEAT_AND_COOL;
							} else if (heat && !cool) {
								pp.Product.CalculateMode = Product.CalculateModeEnum.HEAT;
							} else if (!heat && cool) {
								pp.Product.CalculateMode = Product.CalculateModeEnum.COOL;
							} else {
								pp.Product.CalculateMode = pp.Product.DefaultCalculateMode;
							}
						}

						if (pp.Product is HithermCompactProduct && pp.Product.Type == Product.ProductType.DSH) {
							HithermCompactProduct hcp = pp.Product as HithermCompactProduct;
							hcp.HithermCompactType = Product.ProductType.WH;
							foreach (HithermCompactCircuit hcc in hcp.PlannedCircuits) {
								foreach (HithermCompactRegister hcr in hcc.Registers) {
									switch (hcr.RegisterType) {
										case HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std:
											hcr.RegisterType = HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Ds;
											break;

										case HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std:
											hcr.RegisterType = HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Ds;
											break;

										case HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std:
											hcr.RegisterType = HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Ds;
											break;

										case HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std:
											hcr.RegisterType = HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Ds;
											break;

										case HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std:
											hcr.RegisterType = HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Ds;
											break;
									}
								}
							}
						}
					}
				}
			}
            if (Project.Instance.ProjectEuroplanVersion.CompareTo(new Version(3, 0, 18, 0)) < 0) {
                // change sides of modules in dxf plans
                foreach (Floor f in Project.Instance.Floors) {
                    if (f.AssociatedPlan != null && f.AssociatedPlan is CadPlan) {
                        foreach (Room r in f.Rooms) {
                            foreach (PlannedProduct pp in r.PlannedProducts) {
                                if (pp.Product.GraphicalMode.HasValue && pp.Product.GraphicalMode.Value) {
                                    if (pp.Product is ModulKlimaDeckeProduct) {
                                        ModulKlimaDeckeProduct mkdp = pp.Product as ModulKlimaDeckeProduct;
                                        foreach (ModulDeckeCircuit circuit in mkdp.PlannedCircuits) {
                                            foreach (ModulDeckeSubArea subarea in circuit.SubAreas) {
                                                foreach (KlimaFlaechenList row in subarea.Rows) {
                                                    foreach (KlimaFlaechenModul m in row.List) {
                                                        if (m.ModulType != KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60 && m.ModulType != KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B) {
                                                            if (m.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT) {
                                                                m.Orientation = KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT;
                                                            } else {
                                                                m.Orientation = KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
#warning TODO: ¸berpr¸fen ob Klimaboden-Module auch gedreht werden m¸ssen
                                }
                            }
                        }
                    }
                }
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

			this.FixProductToProductConnection();
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
				try {
					Instance.FixProductToProductConnection();
					Instance.ProjectLastChanged = DateTime.Now;

					XmlSerializer s = new XmlSerializer(typeof(Project));
					MemoryStream w = new MemoryStream();
					Instance.ProjectFileName = filename;
					s.Serialize(w, Instance);
					Stream fs = new FileStream(filename, FileMode.Create);
					w.WriteTo(fs);
					w.Close();
					fs.Close();
					FileUtils.SetAccessForEveryone(filename);
                    Project.Instance.CleanupAutoSave();
					if (ProjectSaved != null) {
						Project.ProjectSaved(Instance);
					}
				} catch (Exception e) {
					log.Error(e);
					MessageBox.Show(EuroplanRes.Project_FehlerBeimSpeichernText, EuroplanRes.Project_FehlerBeimSpeichernTitel, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

        public static void AutoSave(string filename) {
            lock (padlock) {
                try {
                    Instance.ProjectLastChanged = DateTime.Now;

                    XmlSerializer s = new XmlSerializer(typeof(Project));
                    MemoryStream w = new MemoryStream();
                    Instance.ProjectFileName = filename;
                    s.Serialize(w, Instance);
                    Stream fs = new FileStream(AutoSaveFilename(filename), FileMode.Create);
                    w.WriteTo(fs);
                    w.Close();
                    fs.Close();
                    FileUtils.SetAccessForEveryone(filename);
                } catch (Exception e) {
                    log.Error(e);
                }
            }
        }

        public void CleanupAutoSave() {
            lock (padlock) {
                try {
                    if (!string.IsNullOrEmpty(this.ProjectFileName)) {
                        if (File.Exists(AutoSaveFilename(this.ProjectFileName))) {
                            File.Delete(AutoSaveFilename(this.ProjectFileName));
                        }
                    }
                } catch (Exception ex) {
                    log.Error("Problem deleting autosave file", ex);
                }
            }
        }

		public static Project New() {
			lock (padlock) {
				Instance.InitializeProject();
				string localized = EuroplanRes.General_Standardregelkreis;
				Instance.RegulatorCircuits.Add(new RegulatorCircuit(localized));
				Instance.InitializeProductParameters();
				return Instance;
			}
		}

		private void InitializeProductParameters() {
			Product.StaticInitialize(this.Config);
			ConcreteActivationProduct.StaticInitialize(this.Config);
			JumbovalProduct.StaticInitialize(this.Config);
			EcothermProduct.StaticInitialize(this.Config);
			EurovalProduct.StaticInitialize(this.Config);
			HithermCompactProduct.StaticInitialize(this.Config);
			HithermCompactRoofProduct.StaticInitialize(this.Config);
			HithermProduct.StaticInitialize(this.Config);
			ModulKlimaBodenProduct.StaticInitialize(this.Config);
			ModulKlimaDeckeProduct.StaticInitialize(this.Config);
		}


		public void InitializeTreeView(System.Windows.Forms.TreeView tree) {
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
			// insert imported plans node if missing
			if (this.rootNode.Nodes.Count == 3 || this.rootNode.Nodes[3] != this.importedPlansNode) {
				this.rootNode.Nodes.Insert(3, this.importedPlansNode);
			}
			// insert floors node if missing
			if (this.rootNode.Nodes.Count == 4 || this.rootNode.Nodes[4] != this.floorsNode) {
				this.rootNode.Nodes.Insert(4, this.floorsNode);
			}
			// insert quick dimensioning node if missing
			if (this.rootNode.Nodes.Count == 5 || this.rootNode.Nodes[5] != this.quickDimensioningNode) {
				this.rootNode.Nodes.Insert(5, this.quickDimensioningNode);
			}
			// insert required material node if missing
			if (this.rootNode.Nodes.Count == 6 || this.rootNode.Nodes[6] != this.requiredMaterialNode) {
				this.rootNode.Nodes.Insert(6, this.requiredMaterialNode);
			}

			// remove other nodes
			while (this.rootNode.Nodes.Count > 7) {
				this.rootNode.Nodes.RemoveAt(7);
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
			bool klimaBodenPlanned = false;
			bool hithermCompactPlanned = false;
			foreach (Floor floor in this.floors) {
				// distributors
				foreach (Distributor distributor in floor.Distributors) {
					distributor.CalculateRequiredMaterial(requiredMaterialCalculated);
				}
				foreach (Room room in floor.Rooms) {
					// products
					foreach (PlannedProduct product in room.PlannedProducts) {
						product.Product.CalculateRequiredMaterial(requiredMaterialCalculated);
						if (product.Product is ModulKlimaBodenProduct) {
							klimaBodenPlanned = true;
						}
						if (product.Product is HithermCompactProduct) {
							hithermCompactPlanned = true;
						}
					}
				}
			}

			if (klimaBodenPlanned) {
				ModulKlimaBodenProduct.ReviseRequiredMaterial(requiredMaterialCalculated);
			}
			if (hithermCompactPlanned) {
				HithermCompactProduct.ReviseRequiredMaterial(requiredMaterialCalculated);
			}
		}

		public void AddRequiredMaterial(SerializableDictionary<string, double> requiredMaterial, string materialId, double amount) {
			try {
				if (amount != 0) {
					Material material = this.Config.Materials.Find(delegate(Material m) { return m.Id == materialId; });
					if (material != null || materialId.StartsWith("PLACEHOLDER_")) {
						if (requiredMaterial.ContainsKey(materialId)) {
							double oldAmount = requiredMaterial[materialId];
							double newAmount;
							if (double.IsNegativeInfinity(oldAmount)) {
								newAmount = Math.Abs(amount);
							} else if (double.IsNegativeInfinity(amount)) {
								newAmount = Math.Abs(oldAmount);
							} else {
								newAmount = Math.Abs(oldAmount) + Math.Abs(amount);
							}
							if (double.IsNegativeInfinity(oldAmount) || double.IsNegativeInfinity(amount) || oldAmount < 0 || amount < 0) {
								newAmount = -newAmount;
							}
							requiredMaterial[materialId] = newAmount;
						} else {
							requiredMaterial.Add(materialId, amount);
						}
					}
				}
			} catch (Exception e) {
				Console.WriteLine(e);
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

		public static void CopyPlans(string source, string destination) {
			if (source != null && destination != null) {
				string sourceDir = Path.GetDirectoryName(source);
				string sourceSubDir = Path.GetFileNameWithoutExtension(source) + "_plans";
				string from = Path.Combine(sourceDir, sourceSubDir);
				string destinationDir = Path.GetDirectoryName(destination);
				string destinationSubDir = Path.GetFileNameWithoutExtension(destination) + "_plans";
				string to = Path.Combine(destinationDir, destinationSubDir);
				if (Directory.Exists(from)) {
					CopyDirectory(new DirectoryInfo(from), new DirectoryInfo(to));
					foreach (Plan plan in Project.Instance.ImportedPlans) {
						plan.RelativeFileName = plan.RelativeFileName.Replace(sourceSubDir, destinationSubDir);
					}
				}
			}
		}

		private static void CopyDirectory(DirectoryInfo diSourceDir, DirectoryInfo diDestDir) {
			if (!diDestDir.Exists) {
				diDestDir.Create();
			}
			FileInfo[] fiSrcFiles = diSourceDir.GetFiles();
			foreach (FileInfo fiSrcFile in fiSrcFiles) {
				fiSrcFile.CopyTo(Path.Combine(diDestDir.FullName, fiSrcFile.Name), true);
			}
			DirectoryInfo[] diSrcDirectories = diSourceDir.GetDirectories();
			foreach (DirectoryInfo diSrcDirectory in diSrcDirectories) {
				CopyDirectory(diSrcDirectory, new DirectoryInfo(Path.Combine(diDestDir.FullName, diSrcDirectory.Name)));
			}
		}

		private void FixProductToProductConnection() {
			foreach (Floor floor in this.floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						pp.Product.FixConnectedCircuits();
						pp.Product.FixInverseConnectedCircuits();
					}
				}
			}
		}
	}
}
