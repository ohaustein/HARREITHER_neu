using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;
using System.IO;

namespace Europlan.Common {
	public partial class ProjectReport : Form {
		
		private combit.ListLabel15.ListLabel listLabel1;
		private combit.ListLabel15.ListLabelPreviewControl listLabelPreviewControl1;
		private Project project;

		public ProjectReport(Project project) {
			this.project = project;
			InitializeComponent();

			Cursor current = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;

			this.listLabel1 = new combit.ListLabel15.ListLabel();
			listLabelPreviewControl1 = new combit.ListLabel15.ListLabelPreviewControl();

			this.listLabel1.AutoDestination = combit.ListLabel15.LlPrintMode.PreviewControl;
			this.listLabel1.LicensingInfo = "BUaWEQ";
			this.listLabel1.MaxRTFVersion = 65280;
			this.listLabel1.NoParameterCheck = true;
			this.listLabel1.PreviewControl = this.listLabelPreviewControl1;
			this.listLabel1.Unit = combit.ListLabel15.LlUnits.Millimeter_1_100;

			this.listLabelPreviewControl1.CloseMode = combit.ListLabel15.LlPreviewControlCloseMode.DeleteFile;
			this.listLabelPreviewControl1.Dock = DockStyle.Fill;
			this.listLabelPreviewControl1.BackColor = System.Drawing.SystemColors.Control;
			this.listLabelPreviewControl1.CurrentPage = 0;
			this.listLabelPreviewControl1.ForceReadOnly = true;
			this.listLabelPreviewControl1.Location = new System.Drawing.Point(0, 0);
			this.listLabelPreviewControl1.Name = "listLabelPreviewControl1";
			this.listLabelPreviewControl1.Size = new System.Drawing.Size(906, 506);
			this.listLabelPreviewControl1.SlideshowMode = false;
			this.listLabelPreviewControl1.TabIndex = 4;
			this.listLabelPreviewControl1.Text = "listLabelPreviewControl1";
			this.listLabelPreviewControl1.ToolbarButtons.Exit = combit.ListLabel15.LlButtonState.Invisible;
			this.listLabelPreviewControl1.ToolbarButtons.GotoFirst = combit.ListLabel15.LlButtonState.Default;
			this.listLabelPreviewControl1.ToolbarButtons.GotoLast = combit.ListLabel15.LlButtonState.Default;
			this.listLabelPreviewControl1.ToolbarButtons.GotoNext = combit.ListLabel15.LlButtonState.Default;
			this.listLabelPreviewControl1.ToolbarButtons.GotoPrev = combit.ListLabel15.LlButtonState.Default;
			this.listLabelPreviewControl1.ToolbarButtons.PageRange = combit.ListLabel15.LlButtonState.Default;
			this.listLabelPreviewControl1.ToolbarButtons.PrintAllPages = combit.ListLabel15.LlButtonState.Default;
			this.listLabelPreviewControl1.ToolbarButtons.PrintCurrentPage = combit.ListLabel15.LlButtonState.Default;
			this.listLabelPreviewControl1.ToolbarButtons.PrintToFax = combit.ListLabel15.LlButtonState.Invisible;
			this.listLabelPreviewControl1.ToolbarButtons.SaveAs = combit.ListLabel15.LlButtonState.Default;
			this.listLabelPreviewControl1.ToolbarButtons.SendTo = combit.ListLabel15.LlButtonState.Invisible;
			this.listLabelPreviewControl1.ToolbarButtons.SlideshowMode = combit.ListLabel15.LlButtonState.Default;
			this.listLabelPreviewControl1.ToolbarButtons.ZoomCombo = combit.ListLabel15.LlButtonState.Default;
			this.listLabelPreviewControl1.ToolbarButtons.ZoomReset = combit.ListLabel15.LlButtonState.Default;
			this.listLabelPreviewControl1.ToolbarButtons.ZoomRevert = combit.ListLabel15.LlButtonState.Default;
			this.listLabelPreviewControl1.ToolbarButtons.ZoomTimes2 = combit.ListLabel15.LlButtonState.Default;
			this.Controls.Add(this.listLabelPreviewControl1);

			DataSet reportData = new DataSet();

			List<ProjektBilanzWrapper> projektBilanzWrapper = this.GetProkjektBilanzReport();
			List<ProjectWarningWrapper> projectWarningWrapper = this.GetProjectWarningReport();
			List<FloorOverviewWrapper> floorOverviewWrapper = this.GetFloorOverviewWrapper();
			List<OpenLoadForRoomWrapper> openHeatLoadWrapper = this.GetOpenHeatLoadForRoomWrapper();
			List<OpenLoadForRoomWrapper> openCoolLoadWrapper = this.GetOpenCoolLoadForRoomWrapper();
			List<RegulatorCircuitWrapper> regulatorCircuitWrapper = this.GetRegulatorCircuitWrapper();
			List<DistributorWrapper> distributorWrapper = this.GetDistributorWrapper();
			List<RoomOverviewWrapper> roomOverviewWrapper = this.GetRoomOverviewWrapper();

			DataTable projektBilanz = ReportHelper.ListToDataTable<ProjektBilanzWrapper>(projektBilanzWrapper);
			DataTable projectWarnings = ReportHelper.ListToDataTable<ProjectWarningWrapper>(projectWarningWrapper);
			DataTable floorOverwiew = ReportHelper.ListToDataTable<FloorOverviewWrapper>(floorOverviewWrapper);
			DataTable openHeatLoad = ReportHelper.ListToDataTable<OpenLoadForRoomWrapper>(openHeatLoadWrapper);
			DataTable openCoolLoad = ReportHelper.ListToDataTable<OpenLoadForRoomWrapper>(openCoolLoadWrapper);
			DataTable regulatorCircuits = ReportHelper.ListToDataTable<RegulatorCircuitWrapper>(regulatorCircuitWrapper);
			DataTable distributors = ReportHelper.ListToDataTable<DistributorWrapper>(distributorWrapper);
			DataTable roomOverview = ReportHelper.ListToDataTable<RoomOverviewWrapper>(roomOverviewWrapper);

			projektBilanz.TableName = "ProjektBilanz";
			projectWarnings.TableName = "ProjectWarnings";
			floorOverwiew.TableName = "FloorOverview";
			openHeatLoad.TableName = "OpenHeatLoad";
			openCoolLoad.TableName = "OpenCoolLoad";
			regulatorCircuits.TableName = "RegulatorCircuits";
			distributors.TableName = "Distributors";
			roomOverview.TableName = "RoomOverview";

			reportData.Tables.Add(projektBilanz);
			reportData.Tables.Add(projectWarnings);
			reportData.Tables.Add(floorOverwiew);
			reportData.Tables.Add(openHeatLoad);
			reportData.Tables.Add(openCoolLoad);
			reportData.Tables.Add(regulatorCircuits);
			reportData.Tables.Add(distributors);
			reportData.Tables.Add(roomOverview);

			listLabel1.DataSource = reportData;

			string projectName = "";
			foreach (string line in project.ProjectName) {
				projectName += line + "\n";
			}
			string comments = "";
			foreach (string line in project.ProjectNotes) {
				comments += line + "\n";
			}
			string contact = "";
			foreach (string line in project.ProjectContact) {
				contact += line + "\n";
			}
			projectName = projectName.TrimEnd();
			listLabel1.Variables.Add("@ProjectNumber", project.ProjectNumber.TrimEnd());
			listLabel1.Variables.Add("@ProjectName", projectName);
			listLabel1.Variables.Add("@ProjectContact", contact);
			listLabel1.Variables.Add("@Comments", comments);
			listLabel1.Variables.Add("@ProjectCreated", project.ProjectCreated);
			listLabel1.Variables.Add("@ProjectLastChanged", project.ProjectLastChanged);
			listLabel1.Variables.Add("@ProjectEditor", project.ProjectEditor);
			listLabel1.Variables.Add("@CoolingEnabled", project.CalculateCoolLoad);
			

			listLabel1.Variables.Add("@PartnerContact", Licensing.LicenseManager.Instance.License.Header.Replace("\r", ""));
			listLabel1.Variables.Add("@ProgramVersion", project.EuroplanVersion);
			string filename = Configuration.UserTemplate.PartnerLogo;
			if (File.Exists(filename)) {
				listLabel1.Variables.Add("@PartnerLogo", Image.FromFile(filename));
			} else {
				listLabel1.Variables.Add("@PartnerLogo", "(NULL)");
			}

			listLabel1.Variables.Add("@FileName", Path.GetFileName(project.ProjectFileName));


#if DEBUG
			if (MessageBox.Show("Designer?", "", MessageBoxButtons.YesNo) == DialogResult.Yes) {
				listLabel1.Design();
			}
#endif

			filename = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Reporting"), "ProjectReport.lst");
			try {
				listLabel1.Print(combit.ListLabel15.LlProject.List, filename, false, combit.ListLabel15.LlPrintMode.PreviewControl, combit.ListLabel15.LlBoxType.None, "", false, Path.GetDirectoryName(System.Windows.Forms.Application.CommonAppDataPath));
				GC.Collect();
			} catch (Exception ex) {
				DialogResult result = MessageBox.Show("Die Anwendung konnte keinen installierten Drucker finden. Drücken Sie OK, um einen Standarddrucker einzurichten, mit dem die Vorschau und der Export in eine Datei ermöglicht wird oder Abbrechen, um manuell einen Drucker einzurichten.", "Kein Drucker vorhanden...", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
				if (result == DialogResult.OK) {
					try {
						System.Diagnostics.Process p = new System.Diagnostics.Process();
						p.StartInfo.FileName = "rundll32.exe";
						p.StartInfo.Arguments = "printui.dll,PrintUIEntry /if /b \"Europlan 2.0 Reporting\" /f " + Environment.GetEnvironmentVariable("windir") + "\\inf\\ntprint.inf /r \"lpt1:\" /m \"HP LaserJet 4\"";
						p.Start();
						p.WaitForExit();
						listLabel1.Print(combit.ListLabel15.LlProject.List, filename, false, combit.ListLabel15.LlPrintMode.PreviewControl, combit.ListLabel15.LlBoxType.None, "", false, null);
					} catch (Exception) {
						MessageBox.Show("Fehler bei der automatischen Einrichtung eines Druckers. Richten Sie bitte manuell einen beliebigen Drucker ein.");
						this.Close();
					}
				} else {
					this.Close();
					//this.tabQuickDimensioning.SelectedTab = this.pageSettings;
				}
			}

			Cursor.Current = current;
		}

		private void ProjectReport_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ProjectReport"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
			if (settings.GetSetting("Maximized", false)) {
				this.WindowState = FormWindowState.Maximized;
			} else {
				this.WindowState = FormWindowState.Normal;
			}
		}

		private void ProjectReport_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ProjectReport"];
			if (this.WindowState == FormWindowState.Normal) {
				settings.StorePoint("Location", this.Location);
				settings.StoreSize("Size", this.Size);
				settings.StoreSetting("Maximized", false);
			} else if (this.WindowState == FormWindowState.Maximized) {
				settings.StoreSetting("Maximized", true);
			}
			SettingsFile.Update();
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
			foreach (Floor floor in project.Floors) {
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

				if (project.CalculateCoolLoad) {
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
				}
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

			foreach (RegulatorCircuit rc in project.RegulatorCircuits) {
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
				if (project.CalculateCoolLoad) {
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
			}

			wrapperHeatList.AddRange(wrapperCoolList);

			return wrapperHeatList;
		}

		public List<DistributorWrapper> GetDistributorWrapper() {
			// TODO

			List<DistributorWrapper> wrapperHeatList = new List<DistributorWrapper>();
			List<DistributorWrapper> wrapperCoolList = new List<DistributorWrapper>();

			DistributorWrapper wrapper;

			foreach (Floor floor in project.Floors) {
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
					if (project.CalculateCoolLoad) {
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
			}

			wrapperHeatList.AddRange(wrapperCoolList);

			return wrapperHeatList;
		}

		public List<RoomOverviewWrapper> GetRoomOverviewWrapper() {
			// TODO 

			List<RoomOverviewWrapper> wrapperList = new List<RoomOverviewWrapper>();

			RoomOverviewWrapper wrapper;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					wrapper = new RoomOverviewWrapper();
					wrapper.Id = room.Id;
					wrapper.Name = room.Name;
					wrapper.HeatTemperature = room.RoomHeatTemperature;
					wrapper.HeatNetLoad = room.HeatLoad;
					wrapper.HeatPower = room.HeatLoad;
					wrapper.CoolTemperature = room.RoomCoolTemperature;
					wrapper.CoolNetLoad = room.CoolLoad;
					wrapper.CoolPower = room.CoolLoad;
					wrapper.Area = room.Area;
					wrapper.FloorId = floor.Id;
					wrapper.FloorName = floor.Name;
					wrapperList.Add(wrapper);
				}
			}

			return wrapperList;
		}

	}
}