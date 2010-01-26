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

		public ProjectReport(Project project, ProjectReportOptions reportOptions) {
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
			this.listLabelPreviewControl1.SaveAsFileName = this.project.ProjectFileName.Replace(".e2p", "");
			this.Controls.Add(this.listLabelPreviewControl1);

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct p in room.PlannedProducts) {
						p.ConfigureProduct(project.VariableSpreizung);
					}
				}
			}

			DataSet reportData = new DataSet();

			List<BilanzWrapper> projektBilanzWrapper = new List<BilanzWrapper>();
			List<ProjectWarningWrapper> projectWarningWrapper = new List<ProjectWarningWrapper>();
			List<ProjectWarningWrapper> projectNotificationWrapper = new List<ProjectWarningWrapper>();
			List<FloorOverviewWrapper> floorOverviewWrapper = new List<FloorOverviewWrapper>();
			List<EurovalAreaOverviewWrapper> eurovalOverviewWrapper = new List<EurovalAreaOverviewWrapper>();
			List<EcothermAreaOverviewWrapper> ecothermOverviewWrapper = new List<EcothermAreaOverviewWrapper>();
			List<HithermOverviewWrapper> hithermOverviewWrapper = new List<HithermOverviewWrapper>();
			List<HithermCompactOverviewWrapper> hithermCompactOverviewWrapper = new List<HithermCompactOverviewWrapper>();
			List<ModulBodenOverviewWrapper> modulBodenOverviewWrapper = new List<ModulBodenOverviewWrapper>();
			List<ModulDeckeOverviewWrapper> modulDeckeOverviewWrapper = new List<ModulDeckeOverviewWrapper>();
			List<OpenLoadForRoomWrapper> openHeatLoadWrapper = new List<OpenLoadForRoomWrapper>();
			List<OpenLoadForRoomWrapper> openCoolLoadWrapper = new List<OpenLoadForRoomWrapper>();
			List<RegulatorCircuitWrapper> regulatorCircuitWrapper = new List<RegulatorCircuitWrapper>();
			List<DistributorWrapper> distributorWrapper = new List<DistributorWrapper>();
			List<RoomOverviewWrapper> roomOverviewWrapper = new List<RoomOverviewWrapper>();
			List<EurovalWrapper> eurovalAuslegungWrapper = new List<EurovalWrapper>();
			List<EcothermWrapper> ecothermAuslegungWrapper = new List<EcothermWrapper>();
			List<HithermWrapper> hithermAuslegungWrapper = new List<HithermWrapper>();
			List<HithermCompactWrapper> hithermCompactAuslegungWrapper = new List<HithermCompactWrapper>();
			List<ModulBodenWrapper> modulBodenAuslegungWrapper = new List<ModulBodenWrapper>();
			List<BilanzWrapper> eurovalBilanzWrapper = new List<BilanzWrapper>();
			List<BilanzWrapper> ecothermBilanzWrapper = new List<BilanzWrapper>();
			List<BilanzWrapper> hithermBilanzWrapper = new List<BilanzWrapper>();
			List<BilanzWrapper> hithermCompactBilanzWrapper = new List<BilanzWrapper>();
			List<BilanzWrapper> modulBodenBilanzWrapper = new List<BilanzWrapper>();
			List<BilanzWrapper> modulDeckeBilanzWrapper = new List<BilanzWrapper>();
			List<VerlegedatenCircuitWrapper> verlegedatenCircuitWrapper = new List<VerlegedatenCircuitWrapper>();
			List<RequiredMaterialWrapper> requiredMaterialWrapper = new List<RequiredMaterialWrapper>();

			projectWarningWrapper = this.GetProjectWarnings();
			projectNotificationWrapper = this.GetProjectNotifications();

			if (reportOptions.ProjectOverview) {
				projektBilanzWrapper = this.GetProkjektBilanzReport();

				if (reportOptions.AreaOverview) {
					floorOverviewWrapper = this.GetFloorOverviewWrapper();
					eurovalOverviewWrapper = GetEurovalOverviewWrapper();
					ecothermOverviewWrapper = GetEcothermOverviewWrapper();
					hithermOverviewWrapper = GetHithermOverviewWrapper();
					hithermCompactOverviewWrapper = GetHithermCompactOverviewWrapper();
					modulBodenOverviewWrapper = GetModulBodenOverviewWrapper();
					modulDeckeOverviewWrapper = GetModulDeckeOverviewWrapper();
				}

				openHeatLoadWrapper = this.GetOpenHeatLoadForRoomWrapper();
				openCoolLoadWrapper = this.GetOpenCoolLoadForRoomWrapper();
				regulatorCircuitWrapper = this.GetRegulatorCircuitWrapper();
				distributorWrapper = this.GetDistributorWrapper();
				roomOverviewWrapper = this.GetRoomOverviewWrapper();
			}



			if (reportOptions.Auslegung || reportOptions.Verlegedaten) {
				eurovalAuslegungWrapper = GetEurovalWrapper();
				ecothermAuslegungWrapper = GetEcothermWrapper();
				hithermAuslegungWrapper = GetHithermWrapper();
				hithermCompactAuslegungWrapper = GetHithermCompactWrapper();
				modulBodenAuslegungWrapper = GetModulBodenWrapper();
			}

			if (reportOptions.Auslegung && reportOptions.AuslegungBilanz) {
				eurovalBilanzWrapper = GetEurovalBilanzWrapper();
				ecothermBilanzWrapper = GetEcothermBilanzWrapper();
				hithermBilanzWrapper = GetHithermBilanzWrapper();
				hithermCompactBilanzWrapper = GetHithermCompactBilanzWrapper();
				modulBodenBilanzWrapper = GetModulBodenBilanzWrapper();
				modulDeckeBilanzWrapper = GetModulDeckeBilanzWrapper();
			}

			if (reportOptions.Verlegedaten) {
				verlegedatenCircuitWrapper = GetVerlegedatenCircuitWrapper();
			}
			
			if (reportOptions.RequiredMaterial || reportOptions.RecommendedMaterial) {
				requiredMaterialWrapper = GetRequiredMaterialWrapper();
			}

			DataTable projektBilanz = ReportHelper.ListToDataTable<BilanzWrapper>(projektBilanzWrapper);
			DataTable projectWarnings = ReportHelper.ListToDataTable<ProjectWarningWrapper>(projectWarningWrapper);
			DataTable projectNotification = ReportHelper.ListToDataTable<ProjectWarningWrapper>(projectNotificationWrapper);
			DataTable floorOverwiew = ReportHelper.ListToDataTable<FloorOverviewWrapper>(floorOverviewWrapper);
			DataTable eurovalOverview = ReportHelper.ListToDataTable<EurovalAreaOverviewWrapper>(eurovalOverviewWrapper);
			DataTable ecothermOverview = ReportHelper.ListToDataTable<EcothermAreaOverviewWrapper>(ecothermOverviewWrapper);
			DataTable hithermOverview = ReportHelper.ListToDataTable<HithermOverviewWrapper>(hithermOverviewWrapper);
			DataTable hithermCompactOverview = ReportHelper.ListToDataTable<HithermCompactOverviewWrapper>(hithermCompactOverviewWrapper);
			DataTable modulBodenOverview = ReportHelper.ListToDataTable<ModulBodenOverviewWrapper>(modulBodenOverviewWrapper);
			DataTable modulDeckeOverview = ReportHelper.ListToDataTable<ModulDeckeOverviewWrapper>(modulDeckeOverviewWrapper);
			DataTable openHeatLoad = ReportHelper.ListToDataTable<OpenLoadForRoomWrapper>(openHeatLoadWrapper);
			DataTable openCoolLoad = ReportHelper.ListToDataTable<OpenLoadForRoomWrapper>(openCoolLoadWrapper);
			DataTable regulatorCircuits = ReportHelper.ListToDataTable<RegulatorCircuitWrapper>(regulatorCircuitWrapper);
			DataTable distributors = ReportHelper.ListToDataTable<DistributorWrapper>(distributorWrapper);
			DataTable roomOverview = ReportHelper.ListToDataTable<RoomOverviewWrapper>(roomOverviewWrapper);
			DataTable eurovalAuslegung = ReportHelper.ListToDataTable<EurovalWrapper>(eurovalAuslegungWrapper);
			DataTable ecothermAuslegung = ReportHelper.ListToDataTable<EcothermWrapper>(ecothermAuslegungWrapper);
			DataTable hithermAuslegung = ReportHelper.ListToDataTable<HithermWrapper>(hithermAuslegungWrapper);
			DataTable hithermCompactAuslegung = ReportHelper.ListToDataTable<HithermCompactWrapper>(hithermCompactAuslegungWrapper);
			DataTable modulBodenAuslegung = ReportHelper.ListToDataTable<ModulBodenWrapper>(modulBodenAuslegungWrapper);
			DataTable eurovalBilanz = ReportHelper.ListToDataTable<BilanzWrapper>(eurovalBilanzWrapper);
			DataTable ecothermBilanz = ReportHelper.ListToDataTable<BilanzWrapper>(ecothermBilanzWrapper);
			DataTable hithermBilanz = ReportHelper.ListToDataTable<BilanzWrapper>(hithermBilanzWrapper);
			DataTable hithermCompactBilanz = ReportHelper.ListToDataTable<BilanzWrapper>(hithermCompactBilanzWrapper);
			DataTable modulBodenBilanz = ReportHelper.ListToDataTable<BilanzWrapper>(modulBodenBilanzWrapper);
			DataTable modulDeckeBilanz = ReportHelper.ListToDataTable<BilanzWrapper>(modulDeckeBilanzWrapper);
			DataTable verlegedatenCircuit = ReportHelper.ListToDataTable<VerlegedatenCircuitWrapper>(verlegedatenCircuitWrapper);
			DataTable requiredMaterial = ReportHelper.ListToDataTable<RequiredMaterialWrapper>(requiredMaterialWrapper);

			projektBilanz.TableName = "ProjektBilanz";
			projectWarnings.TableName = "ProjectWarnings";
			projectNotification.TableName = "ProjectNotifications";
			floorOverwiew.TableName = "FloorOverview";
			eurovalOverview.TableName = "EurovalOverview";
			ecothermOverview.TableName = "EcothermOverview";
			hithermOverview.TableName = "HithermOverview";
			hithermCompactOverview.TableName = "HithermCompactOverview";
			modulBodenOverview.TableName = "ModulBodenOverview";
			modulDeckeOverview.TableName = "ModulDeckeOverview";
			openHeatLoad.TableName = "OpenHeatLoad";
			openCoolLoad.TableName = "OpenCoolLoad";
			regulatorCircuits.TableName = "RegulatorCircuits";
			distributors.TableName = "Distributors";
			roomOverview.TableName = "RoomOverview";
			eurovalAuslegung.TableName = "EurovalAuslegung";
			ecothermAuslegung.TableName = "EcothermAuslegung";
			hithermAuslegung.TableName = "HithermAuslegung";
			hithermCompactAuslegung.TableName = "HithermCompactAuslegung";
			modulBodenAuslegung.TableName = "ModulBodenAuslegung";
			eurovalBilanz.TableName = "EurovalBilanz";
			ecothermBilanz.TableName = "EcothermBilanz";
			hithermBilanz.TableName = "HithermBilanz";
			hithermCompactBilanz.TableName = "HithermCompactBilanz";
			modulBodenBilanz.TableName = "ModulBodenBilanz";
			modulDeckeBilanz.TableName = "ModulDeckeBilanz";
			verlegedatenCircuit.TableName = "VerlegedatenCircuit";
			requiredMaterial.TableName = "RequiredMaterial";

			reportData.Tables.Add(projektBilanz);
			reportData.Tables.Add(projectWarnings);
			reportData.Tables.Add(projectNotification);
			reportData.Tables.Add(floorOverwiew);
			reportData.Tables.Add(eurovalOverview);
			reportData.Tables.Add(ecothermOverview);
			reportData.Tables.Add(hithermOverview);
			reportData.Tables.Add(hithermCompactOverview);
			reportData.Tables.Add(modulBodenOverview);
			reportData.Tables.Add(modulDeckeOverview);
			reportData.Tables.Add(openHeatLoad);
			reportData.Tables.Add(openCoolLoad);
			reportData.Tables.Add(regulatorCircuits);
			reportData.Tables.Add(distributors);
			reportData.Tables.Add(roomOverview);
			reportData.Tables.Add(eurovalAuslegung);
			reportData.Tables.Add(ecothermAuslegung);
			reportData.Tables.Add(hithermAuslegung);
			reportData.Tables.Add(hithermCompactAuslegung);
			reportData.Tables.Add(modulBodenAuslegung);
			reportData.Tables.Add(eurovalBilanz);
			reportData.Tables.Add(ecothermBilanz);
			reportData.Tables.Add(hithermBilanz);
			reportData.Tables.Add(hithermCompactBilanz);
			reportData.Tables.Add(modulBodenBilanz);
			reportData.Tables.Add(modulDeckeBilanz);
			reportData.Tables.Add(verlegedatenCircuit);
			reportData.Tables.Add(requiredMaterial);

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

			listLabel1.Variables.Add("@ProjectOverview", reportOptions.ProjectOverview);
			listLabel1.Variables.Add("@AreaOverview", reportOptions.AreaOverview);
			listLabel1.Variables.Add("@Auslegung", reportOptions.Auslegung);
			listLabel1.Variables.Add("@AuslegungBilanz", reportOptions.AuslegungBilanz);
			listLabel1.Variables.Add("@Verlegedaten", reportOptions.Verlegedaten);
			listLabel1.Variables.Add("@RequiredMaterial", reportOptions.RequiredMaterial);
			listLabel1.Variables.Add("@RecommendedMaterial", reportOptions.RecommendedMaterial);

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
			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct p in room.PlannedProducts) {
						p.ConfigureProduct(false);
					}
				}
			}

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

		public List<BilanzWrapper> GetProkjektBilanzReport() {
			List<BilanzWrapper> wrapperList = new List<BilanzWrapper>();

			double normWaermeBedarf = 0;
			double normKuehlBedarf = 0;
			double normWaermeBedarfBereinigt = 0;
			double normKuehlBedarfBereinigt = 0;

			double roomArea = 0;
			double plannedFloorArea = 0;
			double plannedWallArea = 0;
			double plannedCeilingArea = 0;

			double transmissionFloorHeat = 0;
			double transmissionWallHeat = 0;
			double transmissionCeilingHeat = 0;
			double transmissionFloorCool = 0;
			double transmissionWallCool = 0;
			double transmissionCeilingCool = 0;
			double qHeat = 0;
			double qCool = 0;

			double durchflussHeat = 0;
			double durchflussCool = 0;

			double deltaRhoHeatMax = 0;
			double deltaRhoCoolMax = 0;
			double wasserInhalt = 0;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					normWaermeBedarf += room.NormalizedHeatLoad;
					normKuehlBedarf += room.NormalizedCoolLoad;
					roomArea += room.Area;
					foreach (PlannedProduct pp in room.PlannedProducts) {
						normWaermeBedarfBereinigt += pp.Product.PlannedHeizlastBereinigung;
						normKuehlBedarfBereinigt += pp.Product.PlannedKuehllastBereinigung;

						plannedFloorArea += pp.Product.PlannedFloorArea;
						plannedWallArea += pp.Product.PlannedWallArea;
						plannedCeilingArea += pp.Product.PlannedCeilingArea;

						transmissionFloorHeat += pp.Product.TransmissionFloorHeat;
						transmissionWallHeat += pp.Product.TransmissionWallHeat;
						transmissionCeilingHeat += pp.Product.TransmissionCeilingHeat;
						transmissionCeilingHeat += pp.Product.TransmissionRoofHeat;
						transmissionFloorCool += pp.Product.TransmissionFloorCool;
						transmissionWallCool += pp.Product.TransmissionWallCool;
						transmissionCeilingCool += pp.Product.TransmissionCeilingCool;
						transmissionCeilingCool += pp.Product.TransmissionRoofCool;
						qHeat += pp.Product.PlannedHeatLoad;
						qCool += pp.Product.PlannedCoolLoad;

						durchflussHeat += pp.Product.PlannedDurchflussHeat;
						durchflussCool += pp.Product.PlannedDurchflussCool;
												
						deltaRhoHeatMax = deltaRhoHeatMax < pp.Product.PlannedDeltaRhoHeat ? pp.Product.PlannedDeltaRhoHeat : deltaRhoHeatMax;
						deltaRhoCoolMax = deltaRhoCoolMax < pp.Product.PlannedDeltaRhoCool ? pp.Product.PlannedDeltaRhoCool : deltaRhoCoolMax;
						wasserInhalt += pp.Product.WasserInhalt;
					}
				}
			}

			BilanzWrapper wrapper = new BilanzWrapper();
			wrapper.Description = "Gesamt-Normwärmebedarf";
			wrapper.HeatValue = normWaermeBedarf.ToString("0.##");
			wrapper.HeatUnit = "W";
			wrapper.CoolValue = normKuehlBedarf.ToString("0.##");
			wrapper.CoolUnit = "W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gesamter bereinigter Wärmebedarf";
			wrapper.HeatValue = (normWaermeBedarf - normWaermeBedarfBereinigt).ToString("0.##");
			wrapper.HeatUnit = "W";
			wrapper.CoolValue = (normKuehlBedarf - normKuehlBedarfBereinigt).ToString("0.##");
			wrapper.CoolUnit = "W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gesamt-Heizleistung (nach innen)";
			wrapper.HeatValue = qHeat.ToString("0.##");
			wrapper.HeatUnit = "W";
			wrapper.CoolValue = qCool.ToString("0.##");
			wrapper.CoolUnit = "W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gesamte aufgenommene Leistung";
			wrapper.HeatValue = (transmissionFloorHeat + transmissionWallHeat + transmissionCeilingHeat + qHeat).ToString("0.##");
			wrapper.HeatUnit = "W";
			wrapper.CoolValue = (transmissionFloorCool + transmissionWallCool + transmissionCeilingCool + qCool).ToString("0.##");
			wrapper.CoolUnit = "W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gesamt-Wassermenge";
			wrapper.HeatValue = durchflussHeat.ToString("0.##");
			wrapper.HeatUnit = "l/h";
			wrapper.CoolValue = durchflussCool.ToString("0.##");
			wrapper.CoolUnit = "l/h";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Maximaler Druckverlust (inkl. Verteiler)";
			wrapper.HeatValue = deltaRhoHeatMax.ToString("0.##");
			wrapper.HeatUnit = "mbar";
			wrapper.CoolValue = deltaRhoCoolMax.ToString("0.##");
			wrapper.CoolUnit = "mbar";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gesamt-Wasserinhalt (ab Verteiler)";
			wrapper.HeatValue = wasserInhalt.ToString("0.##");
			wrapper.HeatUnit = "l";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gesamt-Raumfläche";
			wrapper.HeatValue = roomArea.ToString("0.##");
			wrapper.HeatUnit = "m²";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gesamt-Fußbodenheizungsfläche";
			wrapper.HeatValue = plannedFloorArea.ToString("0.##");
			wrapper.HeatUnit = "m²";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gesamt-Wandheizungsfläche";
			wrapper.HeatValue = plannedWallArea.ToString("0.##");
			wrapper.HeatUnit = "m²";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gesamt-Deckenkühlungsfläche";
			wrapper.HeatValue = plannedCeilingArea.ToString("0.##");
			wrapper.HeatUnit = "m²";
			wrapperList.Add(wrapper);

			return wrapperList;
		}

		public List<BilanzWrapper> GetEurovalBilanzWrapper() {

			List<BilanzWrapper> wrapperList = new List<BilanzWrapper>();

			double normWaermeBedarf = 0;
			double normKuehlBedarf = 0;
			double normWaermeBedarfBereinigt = 0;
			double normKuehlBedarfBereinigt = 0;

			double roomArea = 0;
			double estrichArea = 0;
			double plannedArea = 0;

			double transmissionFloorHeat = 0;
			double transmissionWallHeat = 0;
			double transmissionCeilingHeat = 0;
			double transmissionFloorCool = 0;
			double transmissionWallCool = 0;
			double transmissionCeilingCool = 0;
			double qHeat = 0;
			double qCool = 0;

			double durchflussHeat = 0;
			double durchflussCool = 0;

			double deltaRhoHeatMax = 0;
			double deltaRhoCoolMax = 0;
			double wasserInhalt = 0;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						if (pp.Product is EurovalProduct) {
							normWaermeBedarf += pp.RequestedHeatLoad;
							normKuehlBedarf += pp.RequestedCoolLoad;
							roomArea += room.Area;
							if (pp.Product.HasInsideConstruction) {
								if (pp.Product.PlannedInsideConstruction.Type == ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_STD_ESTRICH) ||
									pp.Product.PlannedInsideConstruction.Type == ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_USER_ESTRICH)) {
									estrichArea += pp.Product.PlannedFloorArea;
								}
							}
							plannedArea += pp.Product.PlannedFloorArea;

							normWaermeBedarfBereinigt += pp.Product.PlannedHeizlastBereinigung;
							normKuehlBedarfBereinigt += pp.Product.PlannedKuehllastBereinigung;

							transmissionFloorHeat += pp.Product.TransmissionFloorHeat;
							transmissionWallHeat += pp.Product.TransmissionWallHeat;
							transmissionCeilingHeat += pp.Product.TransmissionCeilingHeat;
							transmissionCeilingHeat += pp.Product.TransmissionRoofHeat;
							transmissionFloorCool += pp.Product.TransmissionFloorCool;
							transmissionWallCool += pp.Product.TransmissionWallCool;
							transmissionCeilingCool += pp.Product.TransmissionCeilingCool;
							transmissionCeilingHeat += pp.Product.TransmissionRoofCool;
							qHeat += pp.Product.PlannedHeatLoad;
							qCool += pp.Product.PlannedCoolLoad;

							durchflussHeat += pp.Product.PlannedDurchflussHeat;
							durchflussCool += pp.Product.PlannedDurchflussCool;

							deltaRhoHeatMax = deltaRhoHeatMax < pp.Product.PlannedDeltaRhoHeat ? pp.Product.PlannedDeltaRhoHeat : deltaRhoHeatMax;
							deltaRhoCoolMax = deltaRhoCoolMax < pp.Product.PlannedDeltaRhoCool ? pp.Product.PlannedDeltaRhoCool : deltaRhoCoolMax;

							wasserInhalt += pp.Product.WasserInhalt;
						}
					}
				}
			}

			BilanzWrapper wrapper = new BilanzWrapper();
			wrapper.Description = "Gewünschter Wärmebedarf";
			wrapper.HeatValue = normWaermeBedarf.ToString("0.##");
			wrapper.HeatUnit = "W";
			wrapper.CoolValue = normKuehlBedarf.ToString("0.##");
			wrapper.CoolUnit = "W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Bereinigter Wärmebedarf";
			wrapper.HeatValue = (normWaermeBedarf - normWaermeBedarfBereinigt).ToString("0.##");
			wrapper.HeatUnit = "W";
			wrapper.CoolValue = (normKuehlBedarf - normKuehlBedarfBereinigt).ToString("0.##");
			wrapper.CoolUnit = "W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Erreichte Heizleistung nach innen";
			wrapper.HeatValue = qHeat.ToString("0.##");
			wrapper.HeatUnit = "W";
			wrapper.CoolValue = qCool.ToString("0.##");
			wrapper.CoolUnit = "W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gesamte zugeführte Heizleistung";
			wrapper.HeatValue = (transmissionFloorHeat + transmissionWallHeat + transmissionCeilingHeat + qHeat).ToString("0.##");
			wrapper.HeatUnit = "W";
			wrapper.CoolValue = (transmissionFloorCool + transmissionWallCool + transmissionCeilingCool + qCool).ToString("0.##");
			wrapper.CoolUnit = "W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Wassermenge";
			wrapper.HeatValue = durchflussHeat.ToString("0.##");
			wrapper.HeatUnit = "l/h";
			wrapper.CoolValue = durchflussCool.ToString("0.##");
			wrapper.CoolUnit = "l/h";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Maximaler Druckverlust (inkl. Verteiler)";
			wrapper.HeatValue = deltaRhoHeatMax.ToString("0.##");
			wrapper.HeatUnit = "mbar";
			wrapper.CoolValue = deltaRhoCoolMax.ToString("0.##");
			wrapper.CoolUnit = "mbar";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Wasserinhalt (ab Verteiler)";
			wrapper.HeatValue = wasserInhalt.ToString("0.##");
			wrapper.HeatUnit = "l";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gesamte Raumfläche (Räume mit Euroval® Fußbodenheizung)";
			wrapper.HeatValue = roomArea.ToString("0.##");
			wrapper.HeatUnit = "m²";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gesamte Estrichfläche";
			wrapper.HeatValue = estrichArea.ToString("0.##");
			wrapper.HeatUnit = "m²";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gesamte Heizfläche";
			wrapper.HeatValue = plannedArea.ToString("0.##");
			wrapper.HeatUnit = "m²";
			wrapperList.Add(wrapper);

			return wrapperList;
		}

		public List<BilanzWrapper> GetEcothermBilanzWrapper() {

			List<BilanzWrapper> wrapperList = new List<BilanzWrapper>();

			double normWaermeBedarf = 0;
			double normKuehlBedarf = 0;
			double normWaermeBedarfBereinigt = 0;
			double normKuehlBedarfBereinigt = 0;

			double roomArea = 0;
			double estrichArea = 0;
			double plannedArea = 0;

			double transmissionFloorHeat = 0;
			double transmissionWallHeat = 0;
			double transmissionCeilingHeat = 0;
			double transmissionFloorCool = 0;
			double transmissionWallCool = 0;
			double transmissionCeilingCool = 0;
			double qHeat = 0;
			double qCool = 0;

			double durchflussHeat = 0;
			double durchflussCool = 0;

			double deltaRhoHeatMax = 0;
			double deltaRhoCoolMax = 0;
			double wasserInhalt = 0;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						if (pp.Product is EcothermProduct) {
							normWaermeBedarf += pp.RequestedHeatLoad;
							normKuehlBedarf += pp.RequestedCoolLoad;
							roomArea += room.Area;
							if (pp.Product.HasInsideConstruction) {
								if (pp.Product.PlannedInsideConstruction.Type == ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_STD_ESTRICH) ||
									pp.Product.PlannedInsideConstruction.Type == ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_USER_ESTRICH)) {
									estrichArea += pp.Product.PlannedFloorArea;
								}
							}
							plannedArea += pp.Product.PlannedFloorArea;

							normWaermeBedarfBereinigt += pp.Product.PlannedHeizlastBereinigung;
							normKuehlBedarfBereinigt += pp.Product.PlannedKuehllastBereinigung;

							transmissionFloorHeat += pp.Product.TransmissionFloorHeat;
							transmissionWallHeat += pp.Product.TransmissionWallHeat;
							transmissionCeilingHeat += pp.Product.TransmissionCeilingHeat;
							transmissionCeilingHeat += pp.Product.TransmissionRoofHeat;
							transmissionFloorCool += pp.Product.TransmissionFloorCool;
							transmissionWallCool += pp.Product.TransmissionWallCool;
							transmissionCeilingCool += pp.Product.TransmissionCeilingCool;
							transmissionCeilingHeat += pp.Product.TransmissionRoofCool;
							qHeat += pp.Product.PlannedHeatLoad;
							qCool += pp.Product.PlannedCoolLoad;

							durchflussHeat += pp.Product.PlannedDurchflussHeat;
							durchflussCool += pp.Product.PlannedDurchflussCool;

							deltaRhoHeatMax = deltaRhoHeatMax < pp.Product.PlannedDeltaRhoHeat ? pp.Product.PlannedDeltaRhoHeat : deltaRhoHeatMax;
							deltaRhoCoolMax = deltaRhoCoolMax < pp.Product.PlannedDeltaRhoCool ? pp.Product.PlannedDeltaRhoCool : deltaRhoCoolMax;

							wasserInhalt += pp.Product.WasserInhalt;
						}
					}
				}
			}

			BilanzWrapper wrapper = new BilanzWrapper();
			wrapper.Description = "Gewünschter Wärmebedarf";
			wrapper.HeatValue = normWaermeBedarf.ToString("0.##");
			wrapper.HeatUnit = "W";
			wrapper.CoolValue = normKuehlBedarf.ToString("0.##");
			wrapper.CoolUnit = "W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Bereinigter Wärmebedarf";
			wrapper.HeatValue = (normWaermeBedarf - normWaermeBedarfBereinigt).ToString("0.##");
			wrapper.HeatUnit = "W";
			wrapper.CoolValue = (normKuehlBedarf - normKuehlBedarfBereinigt).ToString("0.##");
			wrapper.CoolUnit = "W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Erreichte Heizleistung nach innen";
			wrapper.HeatValue = qHeat.ToString("0.##");
			wrapper.HeatUnit = "W";
			wrapper.CoolValue = qCool.ToString("0.##");
			wrapper.CoolUnit = "W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gesamte zugeführte Heizleistung";
			wrapper.HeatValue = (transmissionFloorHeat + transmissionWallHeat + transmissionCeilingHeat + qHeat).ToString("0.##");
			wrapper.HeatUnit = "W";
			wrapper.CoolValue = (transmissionFloorCool + transmissionWallCool + transmissionCeilingCool + qCool).ToString("0.##");
			wrapper.CoolUnit = "W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Wassermenge";
			wrapper.HeatValue = durchflussHeat.ToString("0.##");
			wrapper.HeatUnit = "l/h";
			wrapper.CoolValue = durchflussCool.ToString("0.##");
			wrapper.CoolUnit = "l/h";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Maximaler Druckverlust (inkl. Verteiler)";
			wrapper.HeatValue = deltaRhoHeatMax.ToString("0.##");
			wrapper.HeatUnit = "mbar";
			wrapper.CoolValue = deltaRhoCoolMax.ToString("0.##");
			wrapper.CoolUnit = "mbar";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Wasserinhalt (ab Verteiler)";
			wrapper.HeatValue = wasserInhalt.ToString("0.##");
			wrapper.HeatUnit = "l";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gesamte Raumfläche (Räume mit Ecotherm® Fußbodenheizung)";
			wrapper.HeatValue = roomArea.ToString("0.##");
			wrapper.HeatUnit = "m²";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gesamte Estrichfläche";
			wrapper.HeatValue = estrichArea.ToString("0.##");
			wrapper.HeatUnit = "m²";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gesamte Heizfläche";
			wrapper.HeatValue = plannedArea.ToString("0.##");
			wrapper.HeatUnit = "m²";
			wrapperList.Add(wrapper);

			return wrapperList;
		}

		public List<BilanzWrapper> GetHithermBilanzWrapper() {

			List<BilanzWrapper> wrapperList = new List<BilanzWrapper>();

			double normWaermeBedarf = 0;
			double normKuehlBedarf = 0;
			double normWaermeBedarfBereinigt = 0;
			double normKuehlBedarfBereinigt = 0;

			double totalArea = 0;

			double transmissionFloorHeat = 0;
			double transmissionWallHeat = 0;
			double transmissionCeilingHeat = 0;
			double transmissionFloorCool = 0;
			double transmissionWallCool = 0;
			double transmissionCeilingCool = 0;
			double qHeat = 0;
			double qCool = 0;

			double durchflussHeat = 0;
			double durchflussCool = 0;

			double deltaRhoHeatMax = 0;
			double deltaRhoCoolMax = 0;
			double wasserInhalt = 0;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						if (pp.Product is HithermProduct) {
							normWaermeBedarf += pp.RequestedHeatLoad;
							normKuehlBedarf += pp.RequestedCoolLoad;
							totalArea = pp.Product.PlannedNetArea;

							normWaermeBedarfBereinigt += pp.Product.PlannedHeizlastBereinigung;
							normKuehlBedarfBereinigt += pp.Product.PlannedKuehllastBereinigung;

							transmissionFloorHeat += pp.Product.TransmissionFloorHeat;
							transmissionWallHeat += pp.Product.TransmissionWallHeat;
							transmissionCeilingHeat += pp.Product.TransmissionCeilingHeat;
							transmissionCeilingHeat += pp.Product.TransmissionRoofHeat;
							transmissionFloorCool += pp.Product.TransmissionFloorCool;
							transmissionWallCool += pp.Product.TransmissionWallCool;
							transmissionCeilingCool += pp.Product.TransmissionCeilingCool;
							transmissionCeilingHeat += pp.Product.TransmissionRoofCool;
							qHeat += pp.Product.PlannedHeatLoad;
							qCool += pp.Product.PlannedCoolLoad;

							durchflussHeat += pp.Product.PlannedDurchflussHeat;
							durchflussCool += pp.Product.PlannedDurchflussCool;

							deltaRhoHeatMax = deltaRhoHeatMax < pp.Product.PlannedDeltaRhoHeat ? pp.Product.PlannedDeltaRhoHeat : deltaRhoHeatMax;
							deltaRhoCoolMax = deltaRhoCoolMax < pp.Product.PlannedDeltaRhoCool ? pp.Product.PlannedDeltaRhoCool : deltaRhoCoolMax;

							wasserInhalt += pp.Product.WasserInhalt;
						}
					}
				}
			}

			BilanzWrapper wrapper = new BilanzWrapper();
			wrapper.Description = "Gewünschter Wärmebedarf";
			wrapper.HeatValue = normWaermeBedarf.ToString("0.##");
			wrapper.HeatUnit = "W";
			wrapper.CoolValue = normKuehlBedarf.ToString("0.##");
			wrapper.CoolUnit = "W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Bereinigter Wärmebedarf";
			wrapper.HeatValue = (normWaermeBedarf - normWaermeBedarfBereinigt).ToString("0.##");
			wrapper.HeatUnit = "W";
			wrapper.CoolValue = (normKuehlBedarf - normKuehlBedarfBereinigt).ToString("0.##");
			wrapper.CoolUnit = "W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Erreichte Heizleistung nach innen";
			wrapper.HeatValue = qHeat.ToString("0.##");
			wrapper.HeatUnit = "W";
			wrapper.CoolValue = qCool.ToString("0.##");
			wrapper.CoolUnit = "W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gesamte zugeführte Heizleistung";
			wrapper.HeatValue = (transmissionFloorHeat + transmissionWallHeat + transmissionCeilingHeat + qHeat).ToString("0.##");
			wrapper.HeatUnit = "W";
			wrapper.CoolValue = (transmissionFloorCool + transmissionWallCool + transmissionCeilingCool + qCool).ToString("0.##");
			wrapper.CoolUnit = "W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Wassermenge";
			wrapper.HeatValue = durchflussHeat.ToString("0.##");
			wrapper.HeatUnit = "l/h";
			wrapper.CoolValue = durchflussCool.ToString("0.##");
			wrapper.CoolUnit = "l/h";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Maximaler Druckverlust (inkl. Verteiler)";
			wrapper.HeatValue = deltaRhoHeatMax.ToString("0.##");
			wrapper.HeatUnit = "mbar";
			wrapper.CoolValue = deltaRhoCoolMax.ToString("0.##");
			wrapper.CoolUnit = "mbar";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Wasserinhalt (ab Verteiler)";
			wrapper.HeatValue = wasserInhalt.ToString("0.##");
			wrapper.HeatUnit = "l";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gesamtheizfläche";
			wrapper.HeatValue = totalArea.ToString("0.##");
			wrapper.HeatUnit = "m²";
			wrapperList.Add(wrapper);

			return wrapperList;
		}

		public List<BilanzWrapper> GetHithermCompactBilanzWrapper() {

			List<BilanzWrapper> wrapperList = new List<BilanzWrapper>();

			double normWaermeBedarf = 0;
			double normKuehlBedarf = 0;
			double normWaermeBedarfBereinigt = 0;
			double normKuehlBedarfBereinigt = 0;

			double totalArea = 0;

			double transmissionFloorHeat = 0;
			double transmissionWallHeat = 0;
			double transmissionCeilingHeat = 0;
			double transmissionFloorCool = 0;
			double transmissionWallCool = 0;
			double transmissionCeilingCool = 0;
			double qHeat = 0;
			double qCool = 0;

			double durchflussHeat = 0;
			double durchflussCool = 0;

			double deltaRhoHeatMax = 0;
			double deltaRhoCoolMax = 0;
			double wasserInhalt = 0;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						if (pp.Product is HithermCompactProduct) {
							normWaermeBedarf += pp.RequestedHeatLoad;
							normKuehlBedarf += pp.RequestedCoolLoad;
							totalArea = pp.Product.PlannedNetArea;

							normWaermeBedarfBereinigt += pp.Product.PlannedHeizlastBereinigung;
							normKuehlBedarfBereinigt += pp.Product.PlannedKuehllastBereinigung;

							transmissionFloorHeat += pp.Product.TransmissionFloorHeat;
							transmissionWallHeat += pp.Product.TransmissionWallHeat;
							transmissionCeilingHeat += pp.Product.TransmissionCeilingHeat;
							transmissionCeilingHeat += pp.Product.TransmissionRoofHeat;
							transmissionFloorCool += pp.Product.TransmissionFloorCool;
							transmissionWallCool += pp.Product.TransmissionWallCool;
							transmissionCeilingCool += pp.Product.TransmissionCeilingCool;
							transmissionCeilingHeat += pp.Product.TransmissionRoofCool;
							qHeat += pp.Product.PlannedHeatLoad;
							qCool += pp.Product.PlannedCoolLoad;

							durchflussHeat += pp.Product.PlannedDurchflussHeat;
							durchflussCool += pp.Product.PlannedDurchflussCool;

							deltaRhoHeatMax = deltaRhoHeatMax < pp.Product.PlannedDeltaRhoHeat ? pp.Product.PlannedDeltaRhoHeat : deltaRhoHeatMax;
							deltaRhoCoolMax = deltaRhoCoolMax < pp.Product.PlannedDeltaRhoCool ? pp.Product.PlannedDeltaRhoCool : deltaRhoCoolMax;

							wasserInhalt += pp.Product.WasserInhalt;
						}
					}
				}
			}

			BilanzWrapper wrapper = new BilanzWrapper();
			wrapper.Description = "Gewünschter Wärmebedarf";
			wrapper.HeatValue = normWaermeBedarf.ToString("0.##");
			wrapper.HeatUnit = "W";
			wrapper.CoolValue = normKuehlBedarf.ToString("0.##");
			wrapper.CoolUnit = "W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Bereinigter Wärmebedarf";
			wrapper.HeatValue = (normWaermeBedarf - normWaermeBedarfBereinigt).ToString("0.##");
			wrapper.HeatUnit = "W";
			wrapper.CoolValue = (normKuehlBedarf - normKuehlBedarfBereinigt).ToString("0.##");
			wrapper.CoolUnit = "W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Erreichte Heizleistung nach innen";
			wrapper.HeatValue = qHeat.ToString("0.##");
			wrapper.HeatUnit = "W";
			wrapper.CoolValue = qCool.ToString("0.##");
			wrapper.CoolUnit = "W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gesamte zugeführte Heizleistung";
			wrapper.HeatValue = (transmissionFloorHeat + transmissionWallHeat + transmissionCeilingHeat + qHeat).ToString("0.##");
			wrapper.HeatUnit = "W";
			wrapper.CoolValue = (transmissionFloorCool + transmissionWallCool + transmissionCeilingCool + qCool).ToString("0.##");
			wrapper.CoolUnit = "W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Wassermenge";
			wrapper.HeatValue = durchflussHeat.ToString("0.##");
			wrapper.HeatUnit = "l/h";
			wrapper.CoolValue = durchflussCool.ToString("0.##");
			wrapper.CoolUnit = "l/h";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Maximaler Druckverlust (inkl. Verteiler)";
			wrapper.HeatValue = deltaRhoHeatMax.ToString("0.##");
			wrapper.HeatUnit = "mbar";
			wrapper.CoolValue = deltaRhoCoolMax.ToString("0.##");
			wrapper.CoolUnit = "mbar";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Wasserinhalt (ab Verteiler)";
			wrapper.HeatValue = wasserInhalt.ToString("0.##");
			wrapper.HeatUnit = "l";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gesamtheizfläche";
			wrapper.HeatValue = totalArea.ToString("0.##");
			wrapper.HeatUnit = "m²";
			wrapperList.Add(wrapper);

			return wrapperList;
		}

		public List<BilanzWrapper> GetModulBodenBilanzWrapper() {

			List<BilanzWrapper> wrapperList = new List<BilanzWrapper>();

			double normWaermeBedarf = 0;
			double normKuehlBedarf = 0;
			double normWaermeBedarfBereinigt = 0;
			double normKuehlBedarfBereinigt = 0;

			double roomArea = 0;
			double coveredArea = 0;
			double modulArea = 0;

			double transmissionFloorHeat = 0;
			double transmissionWallHeat = 0;
			double transmissionCeilingHeat = 0;
			double transmissionFloorCool = 0;
			double transmissionWallCool = 0;
			double transmissionCeilingCool = 0;
			double qHeat = 0;
			double qCool = 0;

			double durchflussHeat = 0;
			double durchflussCool = 0;

			double deltaRhoHeatMax = 0;
			double deltaRhoCoolMax = 0;
			double wasserInhalt = 0;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						if (pp.Product is ModulKlimaBodenProduct) {
							normWaermeBedarf += pp.RequestedHeatLoad;
							normKuehlBedarf += pp.RequestedCoolLoad;
							
							roomArea += room.Area;
							coveredArea += ((ModulKlimaBodenProduct)pp.Product).CoveredFloorArea;
							modulArea += ((ModulKlimaBodenProduct)pp.Product).PlannedModulArea;

							normWaermeBedarfBereinigt += pp.Product.PlannedHeizlastBereinigung;
							normKuehlBedarfBereinigt += pp.Product.PlannedKuehllastBereinigung;

							transmissionFloorHeat += pp.Product.TransmissionFloorHeat;
							transmissionWallHeat += pp.Product.TransmissionWallHeat;
							transmissionCeilingHeat += pp.Product.TransmissionCeilingHeat;
							transmissionCeilingHeat += pp.Product.TransmissionRoofHeat;
							transmissionFloorCool += pp.Product.TransmissionFloorCool;
							transmissionWallCool += pp.Product.TransmissionWallCool;
							transmissionCeilingCool += pp.Product.TransmissionCeilingCool;
							transmissionCeilingHeat += pp.Product.TransmissionRoofCool;
							qHeat += pp.Product.PlannedHeatLoad;
							qCool += pp.Product.PlannedCoolLoad;

							durchflussHeat += pp.Product.PlannedDurchflussHeat;
							durchflussCool += pp.Product.PlannedDurchflussCool;

							deltaRhoHeatMax = deltaRhoHeatMax < pp.Product.PlannedDeltaRhoHeat ? pp.Product.PlannedDeltaRhoHeat : deltaRhoHeatMax;
							deltaRhoCoolMax = deltaRhoCoolMax < pp.Product.PlannedDeltaRhoCool ? pp.Product.PlannedDeltaRhoCool : deltaRhoCoolMax;

							wasserInhalt += pp.Product.WasserInhalt;
						}
					}
				}
			}

			BilanzWrapper wrapper = new BilanzWrapper();
			wrapper.Description = "Gewünschter Wärmebedarf";
			wrapper.HeatValue = normWaermeBedarf.ToString("0.##");
			wrapper.HeatUnit = "W";
			wrapper.CoolValue = normKuehlBedarf.ToString("0.##");
			wrapper.CoolUnit = "W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Bereinigter Wärmebedarf";
			wrapper.HeatValue = (normWaermeBedarf - normWaermeBedarfBereinigt).ToString("0.##");
			wrapper.HeatUnit = "W";
			wrapper.CoolValue = (normKuehlBedarf - normKuehlBedarfBereinigt).ToString("0.##");
			wrapper.CoolUnit = "W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Erreichte Heizleistung nach innen";
			wrapper.HeatValue = qHeat.ToString("0.##");
			wrapper.HeatUnit = "W";
			wrapper.CoolValue = qCool.ToString("0.##");
			wrapper.CoolUnit = "W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gesamte zugeführte Heizleistung";
			wrapper.HeatValue = (transmissionFloorHeat + transmissionWallHeat + transmissionCeilingHeat + qHeat).ToString("0.##");
			wrapper.HeatUnit = "W";
			wrapper.CoolValue = (transmissionFloorCool + transmissionWallCool + transmissionCeilingCool + qCool).ToString("0.##");
			wrapper.CoolUnit = "W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Wassermenge";
			wrapper.HeatValue = durchflussHeat.ToString("0.##");
			wrapper.HeatUnit = "l/h";
			wrapper.CoolValue = durchflussCool.ToString("0.##");
			wrapper.CoolUnit = "l/h";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Maximaler Druckverlust (inkl. Verteiler)";
			wrapper.HeatValue = deltaRhoHeatMax.ToString("0.##");
			wrapper.HeatUnit = "mbar";
			wrapper.CoolValue = deltaRhoCoolMax.ToString("0.##");
			wrapper.CoolUnit = "mbar";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Wasserinhalt (ab Verteiler)";
			wrapper.HeatValue = wasserInhalt.ToString("0.##");
			wrapper.HeatUnit = "l";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gesamte Raumfläche (Räume mit Modul Klimaboden)";
			wrapper.HeatValue = roomArea.ToString("0.##");
			wrapper.HeatUnit = "m²";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gesamte belegte Fläche";
			wrapper.HeatValue = coveredArea.ToString("0.##");
			wrapper.HeatUnit = "m²";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gesamte beheizte Fläche";
			wrapper.HeatValue = modulArea.ToString("0.##");
			wrapper.HeatUnit = "m²";
			wrapperList.Add(wrapper);
			
			return wrapperList;
		}

		public List<BilanzWrapper> GetModulDeckeBilanzWrapper() {

			List<BilanzWrapper> wrapperList = new List<BilanzWrapper>();

			double normWaermeBedarf = 0;
			double normKuehlBedarf = 0;
			double normWaermeBedarfBereinigt = 0;
			double normKuehlBedarfBereinigt = 0;

			double roomArea = 0;
			double totalArea = 0;

			double transmissionFloorHeat = 0;
			double transmissionWallHeat = 0;
			double transmissionCeilingHeat = 0;
			double transmissionFloorCool = 0;
			double transmissionWallCool = 0;
			double transmissionCeilingCool = 0;
			double qHeat = 0;
			double qCool = 0;

			double durchflussHeat = 0;
			double durchflussCool = 0;

			double deltaRhoHeatMax = 0;
			double deltaRhoCoolMax = 0;
			double wasserInhalt = 0;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						if (pp.Product is ModulKlimaDeckeProduct) {
							normWaermeBedarf += pp.RequestedHeatLoad;
							normKuehlBedarf += pp.RequestedCoolLoad;

							roomArea += room.Area;
							totalArea += ((ModulKlimaDeckeProduct)pp.Product).TotalPlannedArea;

							normWaermeBedarfBereinigt += pp.Product.PlannedHeizlastBereinigung;
							normKuehlBedarfBereinigt += pp.Product.PlannedKuehllastBereinigung;

							transmissionFloorHeat += pp.Product.TransmissionFloorHeat;
							transmissionWallHeat += pp.Product.TransmissionWallHeat;
							transmissionCeilingHeat += pp.Product.TransmissionCeilingHeat;
							transmissionCeilingHeat += pp.Product.TransmissionRoofHeat;
							transmissionFloorCool += pp.Product.TransmissionFloorCool;
							transmissionWallCool += pp.Product.TransmissionWallCool;
							transmissionCeilingCool += pp.Product.TransmissionCeilingCool;
							transmissionCeilingHeat += pp.Product.TransmissionRoofCool;
							qHeat += pp.Product.PlannedHeatLoad;
							qCool += pp.Product.PlannedCoolLoad;

							durchflussHeat += pp.Product.PlannedDurchflussHeat;
							durchflussCool += pp.Product.PlannedDurchflussCool;

							deltaRhoHeatMax = deltaRhoHeatMax < pp.Product.PlannedDeltaRhoHeat ? pp.Product.PlannedDeltaRhoHeat : deltaRhoHeatMax;
							deltaRhoCoolMax = deltaRhoCoolMax < pp.Product.PlannedDeltaRhoCool ? pp.Product.PlannedDeltaRhoCool : deltaRhoCoolMax;

							wasserInhalt += pp.Product.WasserInhalt;
						}
					}
				}
			}

			BilanzWrapper wrapper = new BilanzWrapper();
			wrapper.Description = "Gewünschter Wärmebedarf";
			wrapper.HeatValue = normWaermeBedarf.ToString("0.##");
			wrapper.HeatUnit = "W";
			wrapper.CoolValue = normKuehlBedarf.ToString("0.##");
			wrapper.CoolUnit = "W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Bereinigter Wärmebedarf";
			wrapper.HeatValue = (normWaermeBedarf - normWaermeBedarfBereinigt).ToString("0.##");
			wrapper.HeatUnit = "W";
			wrapper.CoolValue = (normKuehlBedarf - normKuehlBedarfBereinigt).ToString("0.##");
			wrapper.CoolUnit = "W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Erreichte Heizleistung nach innen";
			wrapper.HeatValue = qHeat.ToString("0.##");
			wrapper.HeatUnit = "W";
			wrapper.CoolValue = qCool.ToString("0.##");
			wrapper.CoolUnit = "W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gesamte zugeführte Heizleistung";
			wrapper.HeatValue = (transmissionFloorHeat + transmissionWallHeat + transmissionCeilingHeat + qHeat).ToString("0.##");
			wrapper.HeatUnit = "W";
			wrapper.CoolValue = (transmissionFloorCool + transmissionWallCool + transmissionCeilingCool + qCool).ToString("0.##");
			wrapper.CoolUnit = "W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Wassermenge";
			wrapper.HeatValue = durchflussHeat.ToString("0.##");
			wrapper.HeatUnit = "l/h";
			wrapper.CoolValue = durchflussCool.ToString("0.##");
			wrapper.CoolUnit = "l/h";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Maximaler Druckverlust (inkl. Verteiler)";
			wrapper.HeatValue = deltaRhoHeatMax.ToString("0.##");
			wrapper.HeatUnit = "mbar";
			wrapper.CoolValue = deltaRhoCoolMax.ToString("0.##");
			wrapper.CoolUnit = "mbar";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Wasserinhalt (ab Verteiler)";
			wrapper.HeatValue = wasserInhalt.ToString("0.##");
			wrapper.HeatUnit = "l";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gesamte Raumfläche (Räume mit Modul Klimadecke)";
			wrapper.HeatValue = roomArea.ToString("0.##");
			wrapper.HeatUnit = "m²";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gesamte belegte Fläche";
			wrapper.HeatValue = totalArea.ToString("0.##");
			wrapper.HeatUnit = "m²";
			wrapperList.Add(wrapper);

			return wrapperList;
		}

		public List<ProjectWarningWrapper> GetProjectWarnings() {
			List<ProjectWarningWrapper> wrapperList = new List<ProjectWarningWrapper>();


			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct plannedProduct in room.PlannedProducts) {
						if (plannedProduct.Product.LastErrorMessage != null) {
							ProjectWarningWrapper wrapper = new ProjectWarningWrapper();
							wrapper.FloorId = floor.Id;
							wrapper.FloorName = floor.Name;
							wrapper.Warning = "WARNUNG " + plannedProduct.InternalName + " in " + room.Id + "(" + room.Name + "): " + plannedProduct.Product.LastErrorMessage;
							wrapperList.Add(wrapper);
						}
					}
				}
			}

			return wrapperList;
		}

		public List<ProjectWarningWrapper> GetProjectNotifications() {
			List<ProjectWarningWrapper> wrapperList = new List<ProjectWarningWrapper>();

			ProjectWarningWrapper wrapper;

			if (Project.Instance.NotificationMessage != null) {
				wrapper = new ProjectWarningWrapper();
				wrapper.Warning = Project.Instance.NotificationMessage;
				wrapperList.Add(wrapper);
			}

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct plannedProduct in room.PlannedProducts) {
						if (plannedProduct.Product.NotificationMessage != null) {
							wrapper = new ProjectWarningWrapper();
							wrapper.FloorId = floor.Id;
							wrapper.FloorName = floor.Name;
							wrapper.Warning = plannedProduct.InternalName + " in " + room.Id + "(" + room.Name + "): " + plannedProduct.Product.NotificationMessage;
							wrapperList.Add(wrapper);
						}
					}
				}
			}

			return wrapperList;
		}

		public List<FloorOverviewWrapper> GetFloorOverviewWrapper() {
			List<FloorOverviewWrapper> wrapperListHeat = new List<FloorOverviewWrapper>();
			List<FloorOverviewWrapper> wrapperListCool = new List<FloorOverviewWrapper>();
			foreach (Floor floor in project.Floors) {
				double area = 0;
				double transmissionFloorHeat = 0;
				double transmissionWallHeat = 0;
				double transmissionCeilingHeat = 0;
				double transmissionFloorCool = 0;
				double transmissionWallCool = 0;
				double transmissionCeilingCool = 0;
				double qHeat = 0;
				double qCool = 0;
				
				foreach (Room room in floor.Rooms) {
				    foreach (PlannedProduct plannedProduct in room.PlannedProducts) {
						area += plannedProduct.Product.PlannedFloorArea;
						transmissionFloorHeat += plannedProduct.Product.TransmissionFloorHeat;
						transmissionWallHeat += plannedProduct.Product.TransmissionWallHeat;
						transmissionCeilingHeat += plannedProduct.Product.TransmissionCeilingHeat;
						transmissionCeilingHeat += plannedProduct.Product.TransmissionRoofHeat;
						transmissionFloorCool += plannedProduct.Product.TransmissionFloorCool;
						transmissionWallCool += plannedProduct.Product.TransmissionWallCool;
						transmissionCeilingCool += plannedProduct.Product.TransmissionCeilingCool;
						transmissionCeilingHeat += plannedProduct.Product.TransmissionRoofCool;
						qHeat += plannedProduct.Product.PlannedHeatLoad;
						qCool += plannedProduct.Product.PlannedCoolLoad;
				    }
				}
				FloorOverviewWrapper wrapper = new FloorOverviewWrapper();
				wrapper.HeatOrCool = "Heizbetrieb";
				wrapper.FloorName = floor.Name;
				wrapper.FloorArea = area;
				wrapper.QH2o = transmissionFloorHeat + transmissionWallHeat + transmissionCeilingHeat + qHeat;
				wrapper.Q = qHeat;
				wrapper.TransmissionFloor = transmissionFloorHeat;
				wrapper.TransmissionWall = transmissionWallHeat;
				wrapper.TransmissionCeiling = transmissionCeilingHeat;
				wrapperListHeat.Add(wrapper);

				if (project.CalculateCoolLoad) {
					wrapper = new FloorOverviewWrapper();
					wrapper.HeatOrCool = "Kühlbetrieb";
					wrapper.FloorName = floor.Name;
					wrapper.FloorArea = area;
					wrapper.QH2o = transmissionFloorCool + transmissionWallCool + transmissionCeilingCool + qCool;
					wrapper.Q = qCool;
					wrapper.TransmissionFloor = transmissionFloorCool;
					wrapper.TransmissionWall = transmissionWallCool;
					wrapper.TransmissionCeiling = transmissionCeilingCool;
					wrapperListCool.Add(wrapper);
				}
			}

			wrapperListHeat.AddRange(wrapperListCool);
			return wrapperListHeat;
		}

		public List<OpenLoadForRoomWrapper> GetOpenHeatLoadForRoomWrapper() {
			List<OpenLoadForRoomWrapper> wrapperList = new List<OpenLoadForRoomWrapper>();

			double normWaermeBedarf = 0;
			double normWaermeBedarfBereinigt = 0;
			double qHeat = 0;


			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					normWaermeBedarf = room.HeatLoad;
					foreach (PlannedProduct plannedProduct in room.PlannedProducts) {
						normWaermeBedarfBereinigt += plannedProduct.Product.PlannedHeizlastBereinigung;
						qHeat += plannedProduct.PlannedHeatLoad;
					}

					if (qHeat < (normWaermeBedarf - normWaermeBedarfBereinigt)) {
						OpenLoadForRoomWrapper wrapper = new OpenLoadForRoomWrapper();
						wrapper.RoomId = room.Id;
						wrapper.RoomName = room.Name;
						wrapper.RequiredLoad = normWaermeBedarf;
						wrapper.NetLoad = normWaermeBedarf - normWaermeBedarfBereinigt;
						wrapper.Power = qHeat;
						wrapperList.Add(wrapper);
					}
				}
			}

			return wrapperList;
		}

		public List<OpenLoadForRoomWrapper> GetOpenCoolLoadForRoomWrapper() {
			List<OpenLoadForRoomWrapper> wrapperList = new List<OpenLoadForRoomWrapper>();

			double normKuehlBedarf = 0;
			double normKuehlBedarfBereinigt = 0;
			double qCool = 0;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					normKuehlBedarf = room.CoolLoad;
					foreach (PlannedProduct plannedProduct in room.PlannedProducts) {
						normKuehlBedarfBereinigt += plannedProduct.Product.PlannedKuehllastBereinigung;
						qCool += plannedProduct.PlannedCoolLoad;
					}

					if (qCool < (normKuehlBedarf - normKuehlBedarfBereinigt)) {
						OpenLoadForRoomWrapper wrapper = new OpenLoadForRoomWrapper();
						wrapper.RoomId = room.Id;
						wrapper.RoomName = room.Name;
						wrapper.RequiredLoad = normKuehlBedarf;
						wrapper.NetLoad = normKuehlBedarf - normKuehlBedarfBereinigt;
						wrapper.Power = qCool;
						wrapperList.Add(wrapper);
					}
				}
			}

			return wrapperList;
		}

		public List<RegulatorCircuitWrapper> GetRegulatorCircuitWrapper() {

			List<RegulatorCircuitWrapper> wrapperHeatList = new List<RegulatorCircuitWrapper>();
			List<RegulatorCircuitWrapper> wrapperCoolList = new List<RegulatorCircuitWrapper>();

			RegulatorCircuitWrapper wrapper;

			foreach (RegulatorCircuit rc in project.RegulatorCircuits) {

				double vorlauf = 0;
				double ruecklauf = 0;
				double ruecklaufHeat = rc.HeatFlowTemperature;
				double ruecklaufCool = rc.CoolFlowTemperature;
				double durchflussHeat = 0;
				double durchflussCool = 0;
				double deltaRhoHeat = 0;
				double deltaRhoCool = 0;
				double wasserInhalt = 0;

				foreach (Floor floor in project.Floors) {
					foreach (Distributor d in floor.Distributors) {
						if (d.RegulatorCircuit == rc) {
							foreach (PlannedProduct pp in d.PlannedConnectedProducts) {
								if (!pp.Product.PlannedProductIsConnection) {
									pp.Product.GetHeatFlow(out vorlauf, out ruecklauf);
									if (ruecklauf < ruecklaufHeat) {
										ruecklaufHeat = ruecklauf;
									}
									pp.Product.GetCoolFlow(out vorlauf, out ruecklauf);
									if (ruecklauf > ruecklaufCool) {
										ruecklaufCool = ruecklauf;
									}
								}
								durchflussHeat += pp.Product.PlannedDurchflussHeat;
								durchflussCool += pp.Product.PlannedDurchflussCool;
								deltaRhoHeat = deltaRhoHeat < pp.Product.PlannedDeltaRhoHeat ? pp.Product.PlannedDeltaRhoHeat : deltaRhoHeat;
								deltaRhoCool = deltaRhoCool < pp.Product.PlannedDeltaRhoCool ? pp.Product.PlannedDeltaRhoCool : deltaRhoCool;
								wasserInhalt += pp.Product.WasserInhalt;
							}
						}
					}
				}

				wrapper = new RegulatorCircuitWrapper();
				wrapper.HeatOrCool = "Heizbetrieb";
				wrapper.Id = rc.Id;
				wrapper.Name = rc.Name;
				wrapper.Medium = "Wasser";
				wrapper.VorlaufTemp = rc.HeatFlowTemperature;
				wrapper.RuecklaufTemp = ruecklaufHeat;
				wrapper.Durchfluss = durchflussHeat;
				wrapper.Druckverlust = deltaRhoHeat;
				wrapper.Inhalt = wasserInhalt;
				wrapperHeatList.Add(wrapper);
				if (project.CalculateCoolLoad) {
					wrapper = new RegulatorCircuitWrapper();
					wrapper.HeatOrCool = "Kühlbetrieb";
					wrapper.Id = rc.Id;
					wrapper.Name = rc.Name;
					wrapper.Medium = "Wasser";
					wrapper.VorlaufTemp = rc.CoolFlowTemperature;
					wrapper.RuecklaufTemp = ruecklaufCool;
					wrapper.Durchfluss = durchflussCool;
					wrapper.Druckverlust = deltaRhoCool;
					wrapper.Inhalt = wasserInhalt;
					wrapperCoolList.Add(wrapper);
				}
			}

			wrapperHeatList.AddRange(wrapperCoolList);

			return wrapperHeatList;
		}

		public List<DistributorWrapper> GetDistributorWrapper() {
			List<DistributorWrapper> wrapperHeatList = new List<DistributorWrapper>();
			List<DistributorWrapper> wrapperCoolList = new List<DistributorWrapper>();

			DistributorWrapper wrapper;
			foreach (Floor floor in project.Floors) {
				foreach (Distributor distributor in floor.Distributors) {
					double vorlauf = 0;
					double ruecklauf = 0;
					double ruecklaufHeat = distributor.RegulatorCircuit.HeatFlowTemperature;
					double ruecklaufCool = distributor.RegulatorCircuit.CoolFlowTemperature;
					double durchflussHeat = 0;
					double durchflussCool = 0;
					double deltaRhoHeat = 0;
					double deltaRhoCool = 0;
					double wasserInhalt = 0;
					foreach (PlannedProduct pp in distributor.PlannedConnectedProducts) {
						if (!pp.Product.PlannedProductIsConnection) {
							pp.Product.GetHeatFlow(out vorlauf, out ruecklauf);
							if (ruecklauf < ruecklaufHeat) {
								ruecklaufHeat = ruecklauf;
							}
							pp.Product.GetCoolFlow(out vorlauf, out ruecklauf);
							if (ruecklauf > ruecklaufCool) {
								ruecklaufCool = ruecklauf;
							}
						}
						durchflussHeat += pp.Product.PlannedDurchflussHeat;
						durchflussCool += pp.Product.PlannedDurchflussCool;
						deltaRhoHeat = deltaRhoHeat < pp.Product.PlannedDeltaRhoHeat ? pp.Product.PlannedDeltaRhoHeat : deltaRhoHeat;
						deltaRhoCool = deltaRhoCool < pp.Product.PlannedDeltaRhoCool ? pp.Product.PlannedDeltaRhoCool : deltaRhoCool;
						wasserInhalt += pp.Product.WasserInhalt;
					}

					wrapper = new DistributorWrapper();
					wrapper.HeatOrCool = "Heizbetrieb";
					wrapper.Id = distributor.Id;
					wrapper.Name = distributor.Name;
					wrapper.Groups = distributor.PlannedCircuits + distributor.AdditionalCircuits;
					wrapper.RegulatorCircuit = distributor.RegulatorCircuitId;
					wrapper.VorlaufTemp = distributor.RegulatorCircuit.HeatFlowTemperature;
					wrapper.RuecklaufTemp = ruecklaufHeat;
					wrapper.Durchfluss = durchflussHeat;
					wrapper.Druckverlust = deltaRhoHeat;
					wrapper.Inhalt = wasserInhalt;
					wrapperHeatList.Add(wrapper);
					if (project.CalculateCoolLoad) {
						wrapper = new DistributorWrapper();
						wrapper.HeatOrCool = "Kühlbetrieb";
						wrapper.Id = distributor.Id;
						wrapper.Name = distributor.Name;
						wrapper.Groups = distributor.PlannedCircuits + distributor.AdditionalCircuits;
						wrapper.RegulatorCircuit = distributor.RegulatorCircuitId;
						wrapper.VorlaufTemp = distributor.RegulatorCircuit.CoolFlowTemperature;
						wrapper.RuecklaufTemp = ruecklaufCool;
						wrapper.Durchfluss = durchflussCool;
						wrapper.Druckverlust = deltaRhoCool;
						wrapper.Inhalt = wasserInhalt;
						wrapperCoolList.Add(wrapper);
					}
				}
			}

			wrapperHeatList.AddRange(wrapperCoolList);

			return wrapperHeatList;
		}

		public List<RoomOverviewWrapper> GetRoomOverviewWrapper() {
			List<RoomOverviewWrapper> wrapperList = new List<RoomOverviewWrapper>();

			RoomOverviewWrapper wrapper;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					double heatPower = 0;
					double coolPower = 0;
					foreach (PlannedProduct pp in room.PlannedProducts) {
						heatPower += pp.PlannedHeatLoad;
						coolPower += pp.PlannedCoolLoad;
					}

					wrapper = new RoomOverviewWrapper();
					wrapper.Id = room.Id;
					wrapper.Name = room.Name;
					wrapper.HeatTemperature = room.RoomHeatTemperature;
					wrapper.HeatNetLoad = room.HeatLoad;
					wrapper.HeatPower = heatPower;
					wrapper.CoolTemperature = room.RoomCoolTemperature;
					wrapper.CoolNetLoad = room.CoolLoad;
					wrapper.CoolPower = coolPower;
					wrapper.Area = room.Area;
					wrapper.FloorId = floor.Id;
					wrapper.FloorName = floor.Name;
					wrapperList.Add(wrapper);
				}
			}

			return wrapperList;
		}

		public List<EurovalWrapper> GetEurovalWrapper() {
			List<EurovalWrapper> wrapperHeatList = new List<EurovalWrapper>();
			List<EurovalWrapper> wrapperCoolList = new List<EurovalWrapper>();

			EurovalWrapper wrapperHeat = null;
			EurovalWrapper wrapperCool = null;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						wrapperHeat = null;
						wrapperCool = null;
						if (pp.Product is EurovalProduct) {
							EurovalProduct ep = pp.Product as EurovalProduct;
							if (wrapperHeat == null) {
								wrapperHeat = new EurovalWrapper();
								wrapperHeat.HeatOrCool = "Heizen";
								wrapperHeat.FloorId = floor.Id;
								wrapperHeat.FloorName = floor.Name;

								wrapperHeat.RoomId = room.Id;
								wrapperHeat.RoomName = room.Name;
								wrapperHeat.TeilSystem = pp.InternalName;
								if (pp.Product.HasInsideConstruction) {
									wrapperHeat.InsideConstruction = pp.Product.PlannedInsideConstruction.Id;
									wrapperHeat.InsideRValue = pp.Product.PlannedInsideConstructionRValue;
								}
								if (pp.Product.HasOutsideConstruction) {
									wrapperHeat.OutsideConstruction = pp.Product.PlannedOutsideConstruction.Id;
									wrapperHeat.OutsideRValue = pp.Product.PlannedOutsideConstructionRValue;
								}
								wrapperHeat.Circuits = pp.Product.PlannedCircuitCount;
								wrapperHeat.RzLayDistance = ep.PlannedRimLayDistance.ToString();
								wrapperHeat.RzWidth = ep.PlannedRimWidth;
								wrapperHeat.RzArea = ep.PlannedAreaRim;
								wrapperHeat.AzLayDistance = ep.PlannedLayDistance.ToString();
								wrapperHeat.AzArea = ep.PlannedAreaResidence;
								wrapperHeat.ConnectionArea = ep.PlannedRemoveArea;
																
								double v, r;
								pp.Product.GetHeatFlow(out v, out r);
								wrapperHeat.RoomTemp = room.RoomHeatTemperature;
								wrapperHeat.VorlaufTemp = v;
								wrapperHeat.RuecklaufTemp = r;
								wrapperHeat.QSoll = pp.RequestedHeatLoad;
								wrapperHeat.QFBH = pp.PlannedHeatLoad;
								wrapperHeat.tFBAz = ep.PlannedFloorTemperatureHeatResidence;
								wrapperHeat.tFBRz = ep.PlannedFloorTemperatureHeatRim;

								wrapperHeat.Wassermenge = pp.Product.PlannedDurchflussHeat;
								wrapperHeat.DruckverlustRohr = pp.Product.PlannedDeltaRhoHeat;
								wrapperHeat.DruckverlustVerteiler = pp.Product.PlannedDeltaRhoDistributorHeat;

								wrapperHeat.UnusedArea = ep.PlannedAreaUnheated;

								if (ep.PlannedConnection != null) {
									if (ep.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
										wrapperHeat.SubSystem = true;
										wrapperHeat.VorlaufTemp = -1;
										wrapperHeat.RuecklaufTemp = -1;
									}
								}
								if (ep.IsOtherProductConnected) {
									wrapperHeat.OtherSystemsConnected = true;
								}								
							}
							if (project.CalculateCoolLoad && wrapperCool == null) {
								wrapperCool = new EurovalWrapper();
								wrapperCool.HeatOrCool = "Kühlen";
								wrapperCool.FloorId = floor.Id;
								wrapperCool.FloorName = floor.Name;

								wrapperCool.RoomId = room.Id;
								wrapperCool.RoomName = room.Name;
								wrapperCool.TeilSystem = pp.InternalName;
								if (pp.Product.HasInsideConstruction) {
									wrapperHeat.InsideConstruction = pp.Product.PlannedInsideConstruction.Id;
									wrapperHeat.InsideRValue = pp.Product.PlannedInsideConstructionRValue;
								}
								if (pp.Product.HasOutsideConstruction) {
									wrapperHeat.OutsideConstruction = pp.Product.PlannedOutsideConstruction.Id;
									wrapperHeat.OutsideRValue = pp.Product.PlannedOutsideConstructionRValue;
								}
								wrapperCool.Circuits = pp.Product.PlannedCircuitCount;
								wrapperCool.RzLayDistance = ep.PlannedRimLayDistance.ToString();
								wrapperCool.RzWidth = ep.PlannedRimWidth;
								wrapperCool.RzArea = ep.PlannedAreaRim;
								wrapperCool.AzLayDistance = ep.PlannedLayDistance.ToString();
								wrapperCool.AzArea = ep.PlannedAreaResidence;
								wrapperCool.ConnectionArea = ep.PlannedRemoveArea;

								double v, r;
								pp.Product.GetCoolFlow(out v, out r);
								wrapperCool.RoomTemp = room.RoomCoolTemperature;
								wrapperCool.VorlaufTemp = v;
								wrapperCool.RuecklaufTemp = r;
								wrapperCool.QSoll = pp.RequestedCoolLoad;
								wrapperCool.QFBH = pp.PlannedCoolLoad;
								wrapperCool.tFBAz = ep.PlannedFloorTemperatureCoolResidence;
								wrapperCool.tFBRz = ep.PlannedFloorTemperatureCoolRim;

								wrapperCool.Wassermenge = pp.Product.PlannedDurchflussCool;
								wrapperCool.DruckverlustRohr = pp.Product.PlannedDeltaRhoCool;
								wrapperCool.DruckverlustVerteiler = pp.Product.PlannedDeltaRhoDistributorCool;
								
								wrapperCool.UnusedArea = ep.PlannedAreaUnheated;

								if (ep.PlannedConnection != null) {
									if (ep.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
										wrapperCool.SubSystem = true;
										wrapperCool.VorlaufTemp = -1;
										wrapperCool.RuecklaufTemp = -1;
									}
								}
								if (ep.IsOtherProductConnected) {
									wrapperCool.OtherSystemsConnected = true;
								}
							}
							if (wrapperHeat != null) {
								if (ep.PlannedCircuitCount == 0) {
									wrapperHeatList.Add(wrapperHeat);
								} else {
									wrapperHeatList.Add(wrapperHeat);

									EurovalWrapper prevWrapper = null;
									foreach (EurovalCircuit ec in ep.PlannedCircuits) {
										EurovalWrapper wrapper = new EurovalWrapper(wrapperHeat);
										wrapper.UsedAsCircuitWrapper = true;

										wrapper.Circuits = ec.NrOfCircuit + 1;
										wrapper.CircuitsAsString = wrapper.Circuits.ToString();
										wrapper.LengthRzAz = ec.PipeLengthWithoutConnections;
										wrapper.LengthConnection = ec.PipeLengthVorlaufWithoutOtherProductTotal + ec.PipeLengthRuecklaufWithoutOtherProductTotal;
										wrapper.LengthCircuitFbh = ec.PipeLengthWithAllConnections;
										// TODO
										wrapper.LengthCircuitAll = ec.PipeLengthWithAllConnections;

										wrapper.Wassermenge = ec.C_DurchflussHeat;
										wrapper.DruckverlustRohr = ec.C_DruckverlustHeat;
										wrapper.DruckverlustVerteiler = ec.C_DruckverlustDistributorHeat;
										wrapper.V = ec.C_FlussGeschwindigkeitHeat;

										if (prevWrapper == null) {
											prevWrapper = wrapper;
											wrapperHeatList.Add(wrapper);
										} else {
											bool ok = true;
											ok = ok && prevWrapper.LengthRzAz == wrapper.LengthRzAz;
											ok = ok && prevWrapper.LengthConnection == wrapper.LengthConnection;
											ok = ok && prevWrapper.LengthCircuitFbh == wrapper.LengthCircuitFbh;
											ok = ok && prevWrapper.LengthCircuitAll == wrapper.LengthCircuitAll;
											ok = ok && prevWrapper.Wassermenge == wrapper.Wassermenge;
											ok = ok && prevWrapper.DruckverlustRohr == wrapper.DruckverlustRohr;
											ok = ok && prevWrapper.DruckverlustVerteiler == wrapper.DruckverlustVerteiler;
											ok = ok && prevWrapper.V == wrapper.V;

											if (ok) {
												prevWrapper.CircuitsAsString = prevWrapper.Circuits.ToString() + "-" + wrapper.Circuits.ToString();
											} else {
												prevWrapper = wrapper;
												wrapperHeatList.Add(wrapper);
											}
										}
									}
								}
							}
							if (wrapperCool != null) {
								if (ep.PlannedCircuitCount == 0) {
									wrapperCoolList.Add(wrapperCool);
								} else {
									wrapperCoolList.Add(wrapperCool);

									EurovalWrapper prevWrapper = null;
									foreach (EurovalCircuit ec in ep.PlannedCircuits) {
										EurovalWrapper wrapper = new EurovalWrapper(wrapperCool);
										wrapper.UsedAsCircuitWrapper = true;

										wrapper.Circuits = ec.NrOfCircuit + 1;
										wrapper.CircuitsAsString = wrapper.Circuits.ToString();
										wrapper.LengthRzAz = ec.PipeLengthWithoutConnections;
										wrapper.LengthConnection = ec.PipeLengthVorlaufWithoutOtherProductTotal + ec.PipeLengthRuecklaufWithoutOtherProductTotal;
										wrapper.LengthCircuitFbh = ec.PipeLengthWithAllConnections;
										// TODO
										wrapper.LengthCircuitAll = ec.PipeLengthWithAllConnections;

										wrapper.Wassermenge = ec.C_DurchflussCool;
										wrapper.DruckverlustRohr = ec.C_DruckverlustCool;
										wrapper.DruckverlustVerteiler = ec.C_DruckverlustDistributorCool;
										wrapper.V = ec.C_FlussGeschwindigkeitCool;

										if (prevWrapper == null) {
											prevWrapper = wrapper;
											wrapperCoolList.Add(wrapper);
										} else {
											bool ok = true;
											ok = ok && prevWrapper.LengthRzAz == wrapper.LengthRzAz;
											ok = ok && prevWrapper.LengthConnection == wrapper.LengthConnection;
											ok = ok && prevWrapper.LengthCircuitFbh == wrapper.LengthCircuitFbh;
											ok = ok && prevWrapper.LengthCircuitAll == wrapper.LengthCircuitAll;
											ok = ok && prevWrapper.Wassermenge == wrapper.Wassermenge;
											ok = ok && prevWrapper.DruckverlustRohr == wrapper.DruckverlustRohr;
											ok = ok && prevWrapper.DruckverlustVerteiler == wrapper.DruckverlustVerteiler;
											ok = ok && prevWrapper.V == wrapper.V;

											if (ok) {
												prevWrapper.CircuitsAsString = prevWrapper.Circuits.ToString() + "-" + wrapper.Circuits.ToString();
											} else {
												prevWrapper = wrapper;
												wrapperCoolList.Add(wrapper);
											}
										}

									}
								}
							}
						}
					}
				}
			}

			wrapperHeatList.AddRange(wrapperCoolList);

			return wrapperHeatList;
		}

		public List<EcothermWrapper> GetEcothermWrapper() {
			List<EcothermWrapper> wrapperHeatList = new List<EcothermWrapper>();
			List<EcothermWrapper> wrapperCoolList = new List<EcothermWrapper>();

			EcothermWrapper wrapperHeat = null;
			EcothermWrapper wrapperCool = null;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						wrapperHeat = null;
						wrapperCool = null;
						if (pp.Product is EcothermProduct) {
							EcothermProduct ep = pp.Product as EcothermProduct;
							if (wrapperHeat == null) {
								wrapperHeat = new EcothermWrapper();
								wrapperHeat.HeatOrCool = "Heizen";
								wrapperHeat.FloorId = floor.Id;
								wrapperHeat.FloorName = floor.Name;

								wrapperHeat.RoomId = room.Id;
								wrapperHeat.RoomName = room.Name;
								wrapperHeat.TeilSystem = pp.InternalName;
								if (pp.Product.HasInsideConstruction) {
									wrapperHeat.InsideConstruction = pp.Product.PlannedInsideConstruction.Id;
									wrapperHeat.InsideRValue = pp.Product.PlannedInsideConstructionRValue;
								}
								if (pp.Product.HasOutsideConstruction) {
									wrapperHeat.OutsideConstruction = pp.Product.PlannedOutsideConstruction.Id;
									wrapperHeat.OutsideRValue = pp.Product.PlannedOutsideConstructionRValue;
								}
								wrapperHeat.Circuits = pp.Product.PlannedCircuitCount;
								wrapperHeat.RzLayDistance = ep.PlannedRimLayDistance.ToString();
								wrapperHeat.RzWidth = ep.PlannedRimWidth;
								wrapperHeat.RzArea = ep.PlannedAreaRim;
								wrapperHeat.AzLayDistance = ep.PlannedLayDistance.ToString();
								wrapperHeat.AzArea = ep.PlannedAreaResidence;
								wrapperHeat.ConnectionArea = ep.PlannedRemoveArea;

								double v, r;
								pp.Product.GetHeatFlow(out v, out r);
								wrapperHeat.RoomTemp = room.RoomHeatTemperature;
								wrapperHeat.VorlaufTemp = v;
								wrapperHeat.RuecklaufTemp = r;
								wrapperHeat.QSoll = pp.RequestedHeatLoad;
								wrapperHeat.QFBH = pp.PlannedHeatLoad;
								wrapperHeat.tFBAz = ep.PlannedFloorTemperatureHeatResidence;
								wrapperHeat.tFBRz = ep.PlannedFloorTemperatureHeatRim;

								wrapperHeat.Wassermenge = pp.Product.PlannedDurchflussHeat;
								wrapperHeat.DruckverlustRohr = pp.Product.PlannedDeltaRhoHeat;
								wrapperHeat.DruckverlustVerteiler = pp.Product.PlannedDeltaRhoDistributorHeat;

								wrapperHeat.UnusedArea = ep.PlannedAreaUnheated;

								if (ep.PlannedConnection != null) {
									if (ep.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
										wrapperHeat.SubSystem = true;
										wrapperHeat.VorlaufTemp = -1;
										wrapperHeat.RuecklaufTemp = -1;
									}
								}
								if (ep.IsOtherProductConnected) {
									wrapperHeat.OtherSystemsConnected = true;
								}
							}
							if (project.CalculateCoolLoad && wrapperCool == null) {
								wrapperCool = new EcothermWrapper();
								wrapperCool.HeatOrCool = "Kühlen";
								wrapperCool.FloorId = floor.Id;
								wrapperCool.FloorName = floor.Name;

								wrapperCool.RoomId = room.Id;
								wrapperCool.RoomName = room.Name;
								wrapperCool.TeilSystem = pp.InternalName;
								if (pp.Product.HasInsideConstruction) {
									wrapperHeat.InsideConstruction = pp.Product.PlannedInsideConstruction.Id;
									wrapperHeat.InsideRValue = pp.Product.PlannedInsideConstructionRValue;
								}
								if (pp.Product.HasOutsideConstruction) {
									wrapperHeat.OutsideConstruction = pp.Product.PlannedOutsideConstruction.Id;
									wrapperHeat.OutsideRValue = pp.Product.PlannedOutsideConstructionRValue;
								}
								wrapperCool.Circuits = pp.Product.PlannedCircuitCount;
								wrapperCool.RzLayDistance = ep.PlannedRimLayDistance.ToString();
								wrapperCool.RzWidth = ep.PlannedRimWidth;
								wrapperCool.RzArea = ep.PlannedAreaRim;
								wrapperCool.AzLayDistance = ep.PlannedLayDistance.ToString();
								wrapperCool.AzArea = ep.PlannedAreaResidence;
								wrapperCool.ConnectionArea = ep.PlannedRemoveArea;

								double v, r;
								pp.Product.GetCoolFlow(out v, out r);
								wrapperCool.RoomTemp = room.RoomCoolTemperature;
								wrapperCool.VorlaufTemp = v;
								wrapperCool.RuecklaufTemp = r;
								wrapperCool.QSoll = pp.RequestedCoolLoad;
								wrapperCool.QFBH = pp.PlannedCoolLoad;
								wrapperCool.tFBAz = ep.PlannedFloorTemperatureCoolResidence;
								wrapperCool.tFBRz = ep.PlannedFloorTemperatureCoolRim;

								wrapperCool.Wassermenge = pp.Product.PlannedDurchflussCool;
								wrapperCool.DruckverlustRohr = pp.Product.PlannedDeltaRhoCool;
								wrapperCool.DruckverlustVerteiler = pp.Product.PlannedDeltaRhoDistributorCool;

								wrapperCool.UnusedArea = ep.PlannedAreaUnheated;

								if (ep.PlannedConnection != null) {
									if (ep.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
										wrapperCool.SubSystem = true;
										wrapperCool.VorlaufTemp = -1;
										wrapperCool.RuecklaufTemp = -1;
									}
								}
								if (ep.IsOtherProductConnected) {
									wrapperCool.OtherSystemsConnected = true;
								}
							}
							if (wrapperHeat != null) {
								if (ep.PlannedCircuitCount == 0) {
									wrapperHeatList.Add(wrapperHeat);
								} else {
									wrapperHeatList.Add(wrapperHeat);

									EcothermWrapper prevWrapper = null;
									foreach (EcothermCircuit ec in ep.PlannedCircuits) {
										EcothermWrapper wrapper = new EcothermWrapper(wrapperHeat);
										wrapper.UsedAsCircuitWrapper = true;

										wrapper.Circuits = ec.NrOfCircuit + 1;
										wrapper.CircuitsAsString = wrapper.Circuits.ToString();
										wrapper.LengthRzAz = ec.PipeLengthWithoutConnections;
										wrapper.LengthConnection = ec.PipeLengthVorlaufWithoutOtherProductTotal + ec.PipeLengthRuecklaufWithoutOtherProductTotal;
										wrapper.LengthCircuitFbh = ec.PipeLengthWithAllConnections;
										// TODO
										wrapper.LengthCircuitAll = ec.PipeLengthWithAllConnections;

										wrapper.Wassermenge = ec.C_DurchflussHeat;
										wrapper.DruckverlustRohr = ec.C_DruckverlustHeat;
										wrapper.DruckverlustVerteiler = ec.C_DruckverlustDistributorHeat;
										wrapper.V = ec.C_FlussGeschwindigkeitHeat;

										if (prevWrapper == null) {
											prevWrapper = wrapper;
											wrapperHeatList.Add(wrapper);
										} else {
											bool ok = true;
											ok = ok && prevWrapper.LengthRzAz == wrapper.LengthRzAz;
											ok = ok && prevWrapper.LengthConnection == wrapper.LengthConnection;
											ok = ok && prevWrapper.LengthCircuitFbh == wrapper.LengthCircuitFbh;
											ok = ok && prevWrapper.LengthCircuitAll == wrapper.LengthCircuitAll;
											ok = ok && prevWrapper.Wassermenge == wrapper.Wassermenge;
											ok = ok && prevWrapper.DruckverlustRohr == wrapper.DruckverlustRohr;
											ok = ok && prevWrapper.DruckverlustVerteiler == wrapper.DruckverlustVerteiler;
											ok = ok && prevWrapper.V == wrapper.V;

											if (ok) {
												prevWrapper.CircuitsAsString = prevWrapper.Circuits.ToString() + "-" + wrapper.Circuits.ToString();
											} else {
												prevWrapper = wrapper;
												wrapperHeatList.Add(wrapper);
											}
										}
									}
								}
							}
							if (wrapperCool != null) {
								if (ep.PlannedCircuitCount == 0) {
									wrapperCoolList.Add(wrapperCool);
								} else {
									wrapperCoolList.Add(wrapperCool);

									EcothermWrapper prevWrapper = null;
									foreach (EcothermCircuit ec in ep.PlannedCircuits) {
										EcothermWrapper wrapper = new EcothermWrapper(wrapperCool);
										wrapper.UsedAsCircuitWrapper = true;

										wrapper.Circuits = ec.NrOfCircuit + 1;
										wrapper.CircuitsAsString = wrapper.Circuits.ToString();
										wrapper.LengthRzAz = ec.PipeLengthWithoutConnections;
										wrapper.LengthConnection = ec.PipeLengthVorlaufWithoutOtherProductTotal + ec.PipeLengthRuecklaufWithoutOtherProductTotal;
										wrapper.LengthCircuitFbh = ec.PipeLengthWithAllConnections;
										// TODO
										wrapper.LengthCircuitAll = ec.PipeLengthWithAllConnections;

										wrapper.Wassermenge = ec.C_DurchflussCool;
										wrapper.DruckverlustRohr = ec.C_DruckverlustCool;
										wrapper.DruckverlustVerteiler = ec.C_DruckverlustDistributorCool;
										wrapper.V = ec.C_FlussGeschwindigkeitCool;

										if (prevWrapper == null) {
											prevWrapper = wrapper;
											wrapperCoolList.Add(wrapper);
										} else {
											bool ok = true;
											ok = ok && prevWrapper.LengthRzAz == wrapper.LengthRzAz;
											ok = ok && prevWrapper.LengthConnection == wrapper.LengthConnection;
											ok = ok && prevWrapper.LengthCircuitFbh == wrapper.LengthCircuitFbh;
											ok = ok && prevWrapper.LengthCircuitAll == wrapper.LengthCircuitAll;
											ok = ok && prevWrapper.Wassermenge == wrapper.Wassermenge;
											ok = ok && prevWrapper.DruckverlustRohr == wrapper.DruckverlustRohr;
											ok = ok && prevWrapper.DruckverlustVerteiler == wrapper.DruckverlustVerteiler;
											ok = ok && prevWrapper.V == wrapper.V;

											if (ok) {
												prevWrapper.CircuitsAsString = prevWrapper.Circuits.ToString() + "-" + wrapper.Circuits.ToString();
											} else {
												prevWrapper = wrapper;
												wrapperCoolList.Add(wrapper);
											}
										}

									}
								}
							}
						}
					}
				}
			}

			wrapperHeatList.AddRange(wrapperCoolList);

			return wrapperHeatList;
		}

		public List<HithermWrapper> GetHithermWrapper() {
			List<HithermWrapper> wrapperHeatList = new List<HithermWrapper>();
			List<HithermWrapper> wrapperCoolList = new List<HithermWrapper>();

			HithermWrapper wrapperHeat = null;
			HithermWrapper wrapperCool = null;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						wrapperHeat = null;
						wrapperCool = null;
						if (pp.Product is HithermProduct) {
							HithermProduct hp = pp.Product as HithermProduct;
							foreach (HithermCircuit c in hp.PlannedCircuits) {

								wrapperHeat = new HithermWrapper();
								wrapperHeat.HeatOrCool = "Heizen";
								wrapperHeat.FloorId = floor.Id;
								wrapperHeat.FloorName = floor.Name;
								wrapperHeat.RoomId = room.Id;
								wrapperHeat.RoomName = room.Name;
								wrapperHeat.TeilSystem = pp.InternalName;

								wrapperHeat.Circuit = c.NrOfCircuit + 1;
								
								foreach (HithermRegister register in c.Registers) {
									wrapperHeat.RegisterList.Add(register);
									if (wrapperHeat.Registers.ContainsKey(register.RegisterType)) {
										wrapperHeat.Registers[register.RegisterType] += register.RegisterCount;
									} else {
										wrapperHeat.Registers.Add(register.RegisterType, register.RegisterCount);
									}
									wrapperHeat.PipeHorizontal += register.PipeHorizontal;
									wrapperHeat.PipeVertical += register.PipeVertical;
									if (register.IsHochleistungsRegister) {
										wrapperHeat.Ra5Area += register.Area;
									} else {
										wrapperHeat.Ra10Area += register.Area;
									}
									wrapperHeat.WallConstruction = register.Wall.Id;
								}

								double v, r;
								pp.Product.GetHeatFlow(out v, out r);
								wrapperHeat.RoomTemp = room.RoomHeatTemperature;
								wrapperHeat.VorlaufTemp = v;
								wrapperHeat.RuecklaufTemp = r;
								wrapperHeat.QDelta = hp.PlannedHeizlastBereinigung;
								wrapperHeat.QSoll = pp.RequestedHeatLoad - hp.PlannedHeizlastBereinigung;
								wrapperHeat.QWH = pp.PlannedHeatLoad;
								wrapperHeat.QWHSqm = pp.PlannedHeatLoad / pp.PlannedArea.Value;

								wrapperHeat.LengthConnectionVorlauf = c.PipeLengthVorlaufWithoutOtherProductTotal;
								wrapperHeat.LengthConnectionRuecklauf = c.PipeLengthRuecklaufWithoutOtherProductTotal;

								wrapperHeat.Wassermenge = c.C_DurchflussHeat;
								wrapperHeat.DruckverlustRohr = c.C_DruckverlustHeat;
								wrapperHeat.DruckverlustVerteiler = c.C_DruckverlustDistributorHeat;
								wrapperHeat.V = c.C_FlussGeschwindigkeitHeat;

								if (hp.PlannedConnection != null) {
									if (hp.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
										wrapperHeat.SubSystem = true;
										wrapperHeat.VorlaufTemp = -1;
										wrapperHeat.RuecklaufTemp = -1;
									}
								}
								if (hp.IsOtherProductConnected) {
									wrapperHeat.OtherSystemsConnected = true;
								}
								wrapperHeatList.Add(wrapperHeat);

								if (project.CalculateCoolLoad) {
									wrapperCool = new HithermWrapper();
									wrapperCool.HeatOrCool = "Kühlen";
									wrapperCool.FloorId = floor.Id;
									wrapperCool.FloorName = floor.Name;
									wrapperCool.RoomId = room.Id;
									wrapperCool.RoomName = room.Name;
									wrapperCool.TeilSystem = pp.InternalName;

									wrapperCool.Circuit = c.NrOfCircuit + 1;
									
									foreach (HithermRegister register in c.Registers) {
										wrapperCool.RegisterList.Add(register);
										if (wrapperCool.Registers.ContainsKey(register.RegisterType)) {
											wrapperCool.Registers[register.RegisterType] += register.RegisterCount;
										} else {
											wrapperCool.Registers.Add(register.RegisterType, register.RegisterCount);
										}
										wrapperCool.PipeHorizontal += register.PipeHorizontal;
										wrapperCool.PipeVertical += register.PipeVertical;
										if (register.IsHochleistungsRegister) {
											wrapperCool.Ra5Area += register.Area;
										} else {
											wrapperCool.Ra10Area += register.Area;
										}
										wrapperCool.WallConstruction = register.Wall.Id;
									}

									//double v, r;
									pp.Product.GetCoolFlow(out v, out r);
									wrapperCool.RoomTemp = room.RoomCoolTemperature;
									wrapperCool.VorlaufTemp = v;
									wrapperCool.RuecklaufTemp = r;
									wrapperCool.QDelta = hp.PlannedKuehllastBereinigung;
									wrapperCool.QSoll = pp.RequestedCoolLoad - hp.PlannedKuehllastBereinigung;
									wrapperCool.QWH = pp.PlannedCoolLoad;
									wrapperCool.QWHSqm = pp.PlannedCoolLoad / pp.PlannedArea.Value;

									wrapperCool.LengthConnectionVorlauf = c.PipeLengthVorlaufWithoutOtherProductTotal;
									wrapperCool.LengthConnectionRuecklauf = c.PipeLengthRuecklaufWithoutOtherProductTotal;

									wrapperCool.Wassermenge = c.C_DurchflussCool;
									wrapperCool.DruckverlustRohr = c.C_DruckverlustCool;
									wrapperCool.DruckverlustVerteiler = c.C_DruckverlustDistributorCool;
									wrapperCool.V = c.C_FlussGeschwindigkeitCool;

									if (hp.PlannedConnection != null) {
										if (hp.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
											wrapperCool.SubSystem = true;
											wrapperCool.VorlaufTemp = -1;
											wrapperCool.RuecklaufTemp = -1;
										}
									}
									if (hp.IsOtherProductConnected) {
										wrapperCool.OtherSystemsConnected = true;
									}
									wrapperCoolList.Add(wrapperCool);
								}


							}
						}
					}
				}
			}

			wrapperHeatList.AddRange(wrapperCoolList);

			return wrapperHeatList;
		}

		public List<HithermCompactWrapper> GetHithermCompactWrapper() {
			List<HithermCompactWrapper> wrapperHeatList = new List<HithermCompactWrapper>();
			List<HithermCompactWrapper> wrapperCoolList = new List<HithermCompactWrapper>();

			HithermCompactWrapper wrapperHeat = null;
			HithermCompactWrapper wrapperCool = null;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						wrapperHeat = null;
						wrapperCool = null;
						if (pp.Product is HithermCompactProduct) {
							HithermCompactProduct hp = pp.Product as HithermCompactProduct;
							foreach (HithermCompactCircuit c in hp.PlannedCircuits) {

								wrapperHeat = new HithermCompactWrapper();
								wrapperHeat.HeatOrCool = "Heizen";
								wrapperHeat.FloorId = floor.Id;
								wrapperHeat.FloorName = floor.Name;
								wrapperHeat.RoomId = room.Id;
								wrapperHeat.RoomName = room.Name;
								wrapperHeat.TeilSystem = pp.InternalName;

								wrapperHeat.Circuit = c.NrOfCircuit + 1;

								foreach (HithermCompactRegister register in c.Registers) {
									wrapperHeat.RegisterList.Add(register);
									if (wrapperHeat.Registers.ContainsKey(register.RegisterType)) {
										wrapperHeat.Registers[register.RegisterType] += register.RegisterCount;
									} else {
										wrapperHeat.Registers.Add(register.RegisterType, register.RegisterCount);
									}
									wrapperHeat.PipeHorizontal += register.PipeHorizontal;
									wrapperHeat.PipeVertical += register.PipeVertical;
									
									wrapperHeat.Ra5Area += register.RegisterArea;
									wrapperHeat.WallConstruction = register.Wall.Id;
								}

								double v, r;
								pp.Product.GetHeatFlow(out v, out r);
								wrapperHeat.RoomTemp = room.RoomHeatTemperature;
								wrapperHeat.VorlaufTemp = v;
								wrapperHeat.RuecklaufTemp = r;
								wrapperHeat.QDelta = hp.PlannedHeizlastBereinigung;
								wrapperHeat.QSoll = pp.RequestedHeatLoad - hp.PlannedHeizlastBereinigung;
								wrapperHeat.QWH = pp.PlannedHeatLoad;
								wrapperHeat.QWHSqm = pp.PlannedHeatLoad / pp.PlannedArea.Value;

								wrapperHeat.LengthConnectionVorlauf = c.PipeLengthVorlaufWithoutOtherProductTotal;
								wrapperHeat.LengthConnectionRuecklauf = c.PipeLengthRuecklaufWithoutOtherProductTotal;

								wrapperHeat.Wassermenge = c.C_DurchflussHeat;
								wrapperHeat.DruckverlustRohr = c.C_DruckverlustHeat;
								wrapperHeat.DruckverlustVerteiler = c.C_DruckverlustDistributorHeat;
								wrapperHeat.V = c.C_FlussGeschwindigkeitHeat;

								if (hp.PlannedConnection != null) {
									if (hp.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
										wrapperHeat.SubSystem = true;
										wrapperHeat.VorlaufTemp = -1;
										wrapperHeat.RuecklaufTemp = -1;
									}
								}
								if (hp.IsOtherProductConnected) {
									wrapperHeat.OtherSystemsConnected = true;
								}
								wrapperHeatList.Add(wrapperHeat);

								if (project.CalculateCoolLoad) {
									wrapperCool = new HithermCompactWrapper();
									wrapperCool.HeatOrCool = "Kühlen";
									wrapperCool.FloorId = floor.Id;
									wrapperCool.FloorName = floor.Name;
									wrapperCool.RoomId = room.Id;
									wrapperCool.RoomName = room.Name;
									wrapperCool.TeilSystem = pp.InternalName;

									wrapperCool.Circuit = c.NrOfCircuit + 1;

									foreach (HithermCompactRegister register in c.Registers) {
										wrapperCool.RegisterList.Add(register);
										if (wrapperCool.Registers.ContainsKey(register.RegisterType)) {
											wrapperCool.Registers[register.RegisterType] += register.RegisterCount;
										} else {
											wrapperCool.Registers.Add(register.RegisterType, register.RegisterCount);
										}
										wrapperCool.PipeHorizontal += register.PipeHorizontal;
										wrapperCool.PipeVertical += register.PipeVertical;

										wrapperCool.Ra5Area += register.RegisterArea;
										wrapperCool.WallConstruction = register.Wall.Id;
									}

									//double v, r;
									pp.Product.GetCoolFlow(out v, out r);
									wrapperCool.RoomTemp = room.RoomCoolTemperature;
									wrapperCool.VorlaufTemp = v;
									wrapperCool.RuecklaufTemp = r;
									wrapperCool.QDelta = hp.PlannedKuehllastBereinigung;
									wrapperCool.QSoll = pp.RequestedCoolLoad - hp.PlannedKuehllastBereinigung;
									wrapperCool.QWH = pp.PlannedCoolLoad;
									wrapperCool.QWHSqm = pp.PlannedCoolLoad / pp.PlannedArea.Value;

									wrapperCool.LengthConnectionVorlauf = c.PipeLengthVorlaufWithoutOtherProductTotal;
									wrapperCool.LengthConnectionRuecklauf = c.PipeLengthRuecklaufWithoutOtherProductTotal;

									wrapperCool.Wassermenge = c.C_DurchflussCool;
									wrapperCool.DruckverlustRohr = c.C_DruckverlustCool;
									wrapperCool.DruckverlustVerteiler = c.C_DruckverlustDistributorCool;
									wrapperCool.V = c.C_FlussGeschwindigkeitCool;

									if (hp.PlannedConnection != null) {
										if (hp.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
											wrapperCool.SubSystem = true;
											wrapperCool.VorlaufTemp = -1;
											wrapperCool.RuecklaufTemp = -1;
										}
									}
									if (hp.IsOtherProductConnected) {
										wrapperCool.OtherSystemsConnected = true;
									}
									wrapperCoolList.Add(wrapperCool);
								}
							}
						}
					}
				}
			}

			wrapperHeatList.AddRange(wrapperCoolList);

			return wrapperHeatList;
		}

		public List<ModulBodenWrapper> GetModulBodenWrapper() {
			List<ModulBodenWrapper> wrapperHeatList = new List<ModulBodenWrapper>();
			List<ModulBodenWrapper> wrapperCoolList = new List<ModulBodenWrapper>();

			ModulBodenWrapper wrapperHeat = null;
			ModulBodenWrapper wrapperCool = null;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						wrapperHeat = null;
						wrapperCool = null;
						if (pp.Product is ModulKlimaBodenProduct) {
							ModulKlimaBodenProduct mp = pp.Product as ModulKlimaBodenProduct;
							if (wrapperHeat == null) {
								wrapperHeat = new ModulBodenWrapper();
								wrapperHeat.HeatOrCool = "Heizen";
								wrapperHeat.FloorId = floor.Id;
								wrapperHeat.FloorName = floor.Name;

								wrapperHeat.RoomId = room.Id;
								wrapperHeat.RoomName = room.Name;
								wrapperHeat.TeilSystem = pp.InternalName;
								if (pp.Product.HasInsideConstruction) {
									wrapperHeat.InsideConstruction = pp.Product.PlannedInsideConstruction.Id;
									wrapperHeat.InsideRValue = pp.Product.PlannedInsideConstructionRValue;
								}
								if (pp.Product.HasOutsideConstruction) {
									wrapperHeat.OutsideConstruction = pp.Product.PlannedOutsideConstruction.Id;
									wrapperHeat.OutsideRValue = pp.Product.PlannedOutsideConstructionRValue;
								}
								wrapperHeat.Circuits = pp.Product.PlannedCircuitCount;
								wrapperHeat.DichtArea = mp.RequestedModulesDichtArea;
								wrapperHeat.ModulierendArea = mp.RequestedModulesModulierendArea;
								wrapperHeat.SonstigeArea = mp.RequestedModulesSonstigeArea;
								wrapperHeat.ConnectionArea = mp.PlannedRemoveArea;

								double v, r;
								pp.Product.GetHeatFlow(out v, out r);
								wrapperHeat.RoomTemp = room.RoomHeatTemperature;
								wrapperHeat.VorlaufTemp = v;
								wrapperHeat.RuecklaufTemp = r;
								wrapperHeat.QSoll = pp.RequestedHeatLoad;
								wrapperHeat.QFBH = pp.PlannedHeatLoad;
								wrapperHeat.tFB = mp.PlannedFloorTemperatureHeat;

								wrapperHeat.Wassermenge = pp.Product.PlannedDurchflussHeat;
								wrapperHeat.DruckverlustHeizkreis = pp.Product.PlannedDeltaRhoHeat;
								wrapperHeat.DruckverlustVerteiler = pp.Product.PlannedDeltaRhoDistributorHeat;

								wrapperHeat.UnusedArea = mp.PlannedAreaUnheated;

								if (mp.PlannedConnection != null) {
									if (mp.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
										wrapperHeat.SubSystem = true;
										wrapperHeat.VorlaufTemp = -1;
										wrapperHeat.RuecklaufTemp = -1;
									}
								}
								if (mp.IsOtherProductConnected) {
									wrapperHeat.OtherSystemsConnected = true;
								}
							}
							if (project.CalculateCoolLoad && wrapperCool == null) {
								wrapperCool = new ModulBodenWrapper();
								wrapperCool.HeatOrCool = "Kühlen";
								wrapperCool.FloorId = floor.Id;
								wrapperCool.FloorName = floor.Name;

								wrapperCool.RoomId = room.Id;
								wrapperCool.RoomName = room.Name;
								wrapperCool.TeilSystem = pp.InternalName;
								if (pp.Product.HasInsideConstruction) {
									wrapperHeat.InsideConstruction = pp.Product.PlannedInsideConstruction.Id;
									wrapperHeat.InsideRValue = pp.Product.PlannedInsideConstructionRValue;
								}
								if (pp.Product.HasOutsideConstruction) {
									wrapperHeat.OutsideConstruction = pp.Product.PlannedOutsideConstruction.Id;
									wrapperHeat.OutsideRValue = pp.Product.PlannedOutsideConstructionRValue;
								}
								wrapperCool.Circuits = pp.Product.PlannedCircuitCount;
								wrapperCool.DichtArea = mp.RequestedModulesDichtArea;
								wrapperCool.ModulierendArea = mp.RequestedModulesModulierendArea;
								wrapperCool.SonstigeArea = mp.RequestedModulesSonstigeArea;
								wrapperCool.ConnectionArea = mp.PlannedRemoveArea;

								double v, r;
								pp.Product.GetCoolFlow(out v, out r);
								wrapperCool.RoomTemp = room.RoomCoolTemperature;
								wrapperCool.VorlaufTemp = v;
								wrapperCool.RuecklaufTemp = r;
								wrapperCool.QSoll = pp.RequestedCoolLoad;
								wrapperCool.QFBH = pp.PlannedCoolLoad;
								wrapperCool.tFB = mp.PlannedFloorTemperatureCool;

								wrapperCool.Wassermenge = pp.Product.PlannedDurchflussCool;
								wrapperCool.DruckverlustHeizkreis = pp.Product.PlannedDeltaRhoCool;
								wrapperCool.DruckverlustVerteiler = pp.Product.PlannedDeltaRhoDistributorCool;

								wrapperCool.UnusedArea = mp.PlannedAreaUnheated;

								if (mp.PlannedConnection != null) {
									if (mp.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
										wrapperCool.SubSystem = true;
										wrapperCool.VorlaufTemp = -1;
										wrapperCool.RuecklaufTemp = -1;
									}
								}
								if (mp.IsOtherProductConnected) {
									wrapperCool.OtherSystemsConnected = true;
								}
							}
							if (wrapperHeat != null) {
								if (mp.PlannedCircuitCount == 0) {
									wrapperHeatList.Add(wrapperHeat);
								} else {
									wrapperHeatList.Add(wrapperHeat);

									ModulBodenWrapper prevWrapper = null;
									foreach (ModulBodenCircuit mc in mp.PlannedCircuits) {
										ModulBodenWrapper wrapper = new ModulBodenWrapper(wrapperHeat);
										wrapper.UsedAsCircuitWrapper = true;

										wrapper.Circuits = mc.NrOfCircuit + 1;
										wrapper.CircuitsAsString = wrapper.Circuits.ToString();
										wrapper.TotalModules = mp.RequestedModulesTotal;
										wrapper.LengthConnection = mc.PipeLengthVorlaufWithoutOtherProductTotal + mc.PipeLengthRuecklaufWithoutOtherProductTotal;

										wrapper.Wassermenge = mc.C_DurchflussHeat;
										wrapper.DruckverlustHeizkreis = mc.C_DruckverlustHeat;
										wrapper.DruckverlustVerteiler = mc.C_DruckverlustDistributorHeat;
										wrapper.V = mc.C_FlussGeschwindigkeitHeat;

										if (prevWrapper == null) {
											prevWrapper = wrapper;
											wrapperHeatList.Add(wrapper);
										} else {
											bool ok = true;
											ok = ok && prevWrapper.TotalModules == wrapper.TotalModules;
											ok = ok && prevWrapper.LengthConnection == wrapper.LengthConnection;
											ok = ok && prevWrapper.Wassermenge == wrapper.Wassermenge;
											ok = ok && prevWrapper.DruckverlustHeizkreis == wrapper.DruckverlustHeizkreis;
											ok = ok && prevWrapper.DruckverlustVerteiler == wrapper.DruckverlustVerteiler;
											ok = ok && prevWrapper.V == wrapper.V;

											if (ok) {
												prevWrapper.CircuitsAsString = prevWrapper.Circuits.ToString() + "-" + wrapper.Circuits.ToString();
											} else {
												prevWrapper = wrapper;
												wrapperHeatList.Add(wrapper);
											}
										}
									}
								}
							}
							if (wrapperCool != null) {
								if (mp.PlannedCircuitCount == 0) {
									wrapperCoolList.Add(wrapperCool);
								} else {
									wrapperCoolList.Add(wrapperCool);

									ModulBodenWrapper prevWrapper = null;
									foreach (ModulBodenCircuit mc in mp.PlannedCircuits) {
										ModulBodenWrapper wrapper = new ModulBodenWrapper(wrapperCool);
										wrapper.UsedAsCircuitWrapper = true;

										wrapper.Circuits = mc.NrOfCircuit + 1;
										wrapper.CircuitsAsString = wrapper.Circuits.ToString();
										wrapper.TotalModules = mp.RequestedModulesTotal;
										wrapper.LengthConnection = mc.PipeLengthVorlaufWithoutOtherProductTotal + mc.PipeLengthRuecklaufWithoutOtherProductTotal;

										wrapper.Wassermenge = mc.C_DurchflussCool;
										wrapper.DruckverlustHeizkreis = mc.C_DruckverlustCool;
										wrapper.DruckverlustVerteiler = mc.C_DruckverlustDistributorCool;
										wrapper.V = mc.C_FlussGeschwindigkeitCool;

										if (prevWrapper == null) {
											prevWrapper = wrapper;
											wrapperCoolList.Add(wrapper);
										} else {
											bool ok = true;
											ok = ok && prevWrapper.TotalModules == wrapper.TotalModules;
											ok = ok && prevWrapper.LengthConnection == wrapper.LengthConnection;
											ok = ok && prevWrapper.Wassermenge == wrapper.Wassermenge;
											ok = ok && prevWrapper.DruckverlustHeizkreis == wrapper.DruckverlustHeizkreis;
											ok = ok && prevWrapper.DruckverlustVerteiler == wrapper.DruckverlustVerteiler;
											ok = ok && prevWrapper.V == wrapper.V;

											if (ok) {
												prevWrapper.CircuitsAsString = prevWrapper.Circuits.ToString() + "-" + wrapper.Circuits.ToString();
											} else {
												prevWrapper = wrapper;
												wrapperCoolList.Add(wrapper);
											}
										}

									}
								}
							}
						}
					}
				}
			}

			wrapperHeatList.AddRange(wrapperCoolList);

			return wrapperHeatList;
		}

		public List<EurovalAreaOverviewWrapper> GetEurovalOverviewWrapper() {
			List<EurovalAreaOverviewWrapper> wrapperList = new List<EurovalAreaOverviewWrapper>();

			EurovalAreaOverviewWrapper wrapper;
			EurovalProduct p = null;
			Dictionary<EurovalProduct.EurovalLayDistance, double> aZAreaPerLayDistance = new Dictionary<EurovalProduct.EurovalLayDistance, double>();
			Dictionary<EurovalProduct.EurovalLayDistance, double> rZAreaPerLayDistance = new Dictionary<EurovalProduct.EurovalLayDistance, double>();
			Dictionary<EurovalProduct.EurovalLayDistance, double> connectingAreaPerLayDistance = new Dictionary<EurovalProduct.EurovalLayDistance, double>();
			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						if (pp.Product is EurovalProduct) {
							p = pp.Product as EurovalProduct;
							if (p.PlannedLayDistance.HasValue) {
								if (aZAreaPerLayDistance.ContainsKey(p.PlannedLayDistance.Value)) {
									aZAreaPerLayDistance[p.PlannedLayDistance.Value] += p.PlannedAreaResidence;
								} else {
									aZAreaPerLayDistance.Add(p.PlannedLayDistance.Value, p.PlannedAreaResidence);
								}
							}
							if (p.PlannedRimLayDistance.HasValue) {
								if (rZAreaPerLayDistance.ContainsKey(p.PlannedRimLayDistance.Value)) {
									rZAreaPerLayDistance[p.PlannedRimLayDistance.Value] += p.PlannedAreaRim;
								} else {
									rZAreaPerLayDistance.Add(p.PlannedRimLayDistance.Value, p.PlannedAreaRim);
								}
							}
							foreach (ConnectionPipe pipe in p.PlannedConnectionPipes) {
								if (connectingAreaPerLayDistance.ContainsKey(ConnectionPipe.GetEurovalLayDistance(pipe.Verlegeart))) {
									connectingAreaPerLayDistance[ConnectionPipe.GetEurovalLayDistance(pipe.Verlegeart)] += pipe.AreaTotal;
								} else {
									connectingAreaPerLayDistance.Add(ConnectionPipe.GetEurovalLayDistance(pipe.Verlegeart), pipe.AreaTotal);
								}
							}
						}
					}
				}
			}


			foreach (EurovalProduct.EurovalLayDistance distance in Enum.GetValues(typeof(EurovalProduct.EurovalLayDistance))) {
				if (distance != EurovalProduct.EurovalLayDistance.NONE) {
					wrapper = new EurovalAreaOverviewWrapper();
					wrapper.LayDistance = distance.ToString();
					wrapper.AzArea = aZAreaPerLayDistance.ContainsKey(distance) ? aZAreaPerLayDistance[distance] : 0;
					wrapper.RzArea = rZAreaPerLayDistance.ContainsKey(distance) ? rZAreaPerLayDistance[distance] : 0;
					wrapper.ConnectingArea = connectingAreaPerLayDistance.ContainsKey(distance) ? connectingAreaPerLayDistance[distance] : 0;
					wrapperList.Add(wrapper);
				}
			}

			return wrapperList;
		}

		public List<EcothermAreaOverviewWrapper> GetEcothermOverviewWrapper() {
			List<EcothermAreaOverviewWrapper> wrapperList = new List<EcothermAreaOverviewWrapper>();

			EcothermAreaOverviewWrapper wrapper;
			EcothermProduct p = null;
			Dictionary<EcothermProduct.EcothermLayDistance, double> aZAreaPerLayDistance = new Dictionary<EcothermProduct.EcothermLayDistance, double>();
			Dictionary<EcothermProduct.EcothermLayDistance, double> rZAreaPerLayDistance = new Dictionary<EcothermProduct.EcothermLayDistance, double>();
			Dictionary<EcothermProduct.EcothermLayDistance, double> connectingAreaPerLayDistance = new Dictionary<EcothermProduct.EcothermLayDistance, double>();
			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						if (pp.Product is EcothermProduct) {
							p = pp.Product as EcothermProduct;
							if (p.PlannedLayDistance.HasValue) {
								if (aZAreaPerLayDistance.ContainsKey(p.PlannedLayDistance.Value)) {
									aZAreaPerLayDistance[p.PlannedLayDistance.Value] += p.PlannedAreaResidence;
								} else {
									aZAreaPerLayDistance.Add(p.PlannedLayDistance.Value, p.PlannedAreaResidence);
								}
							}
							if (p.PlannedRimLayDistance.HasValue) {
								if (rZAreaPerLayDistance.ContainsKey(p.PlannedRimLayDistance.Value)) {
									rZAreaPerLayDistance[p.PlannedRimLayDistance.Value] += p.PlannedAreaRim;
								} else {
									rZAreaPerLayDistance.Add(p.PlannedRimLayDistance.Value, p.PlannedAreaRim);
								}
							}
							foreach (ConnectionPipe pipe in p.PlannedConnectionPipes) {
								if (connectingAreaPerLayDistance.ContainsKey(ConnectionPipe.GetEcothermLayDistance(pipe.Verlegeart))) {
									connectingAreaPerLayDistance[ConnectionPipe.GetEcothermLayDistance(pipe.Verlegeart)] += pipe.AreaTotal;
								} else {
									connectingAreaPerLayDistance.Add(ConnectionPipe.GetEcothermLayDistance(pipe.Verlegeart), pipe.AreaTotal);
								}
							}
						}
					}
				}
			}


			foreach (EcothermProduct.EcothermLayDistance distance in Enum.GetValues(typeof(EcothermProduct.EcothermLayDistance))) {
				if (distance != EcothermProduct.EcothermLayDistance.NONE) {
					wrapper = new EcothermAreaOverviewWrapper();
					wrapper.LayDistance = distance.ToString();
					wrapper.AzArea = aZAreaPerLayDistance.ContainsKey(distance) ? aZAreaPerLayDistance[distance] : 0;
					wrapper.RzArea = rZAreaPerLayDistance.ContainsKey(distance) ? rZAreaPerLayDistance[distance] : 0;
					wrapper.ConnectingArea = connectingAreaPerLayDistance.ContainsKey(distance) ? connectingAreaPerLayDistance[distance] : 0;
					wrapperList.Add(wrapper);
				}
			}

			return wrapperList;
		}

		public List<HithermOverviewWrapper> GetHithermOverviewWrapper() {
			List<HithermOverviewWrapper> wrapperList = new List<HithermOverviewWrapper>();

			HithermProduct p = null;
			double ra5Area = 0;
			double ra10Area = 0;
			double rohr21Length = 0;
			double rohr2417Length = 0;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						if (pp.Product is HithermProduct) {
							p = pp.Product as HithermProduct;
							foreach (HithermCircuit c in p.PlannedCircuits) {
								foreach (HithermRegister register in c.Registers) {
									if (register.IsHochleistungsRegister) {
										ra5Area += register.Area;
									} else {
										ra10Area += register.Area;
									}
									rohr2417Length = register.PipeHorizontal + register.PipeVertical;
								}
								rohr21Length += c.PipeLengthVorlaufWithoutOtherProductTotal + c.PipeLengthRuecklaufWithoutOtherProductTotal;
							}
						}
					}
				}
			}

			HithermOverviewWrapper wrapper = new HithermOverviewWrapper();
			wrapper.Text = "Fläche mit Rohrabstand RA5";
			wrapper.Amount = ra5Area;
			wrapper.Unit = "m²";
			wrapperList.Add(wrapper);

			wrapper = new HithermOverviewWrapper();
			wrapper.Text = "Fläche mit Rohrabstand RA10";
			wrapper.Amount = ra10Area;
			wrapper.Unit = "m²";
			wrapperList.Add(wrapper);

			wrapper = new HithermOverviewWrapper();
			wrapper.Text = "Rundrohr 21";
			wrapper.Amount = rohr21Length;
			wrapper.Unit = "m";
			wrapperList.Add(wrapper);

			wrapper = new HithermOverviewWrapper();
			wrapper.Text = "Hitherm Klimawand 24/17";
			wrapper.Amount = rohr2417Length;
			wrapper.Unit = "m";
			wrapperList.Add(wrapper);

			return wrapperList;
		}

		public List<HithermCompactOverviewWrapper> GetHithermCompactOverviewWrapper() {
			List<HithermCompactOverviewWrapper> wrapperList = new List<HithermCompactOverviewWrapper>();

			HithermCompactProduct p = null;
			double registerArea = 0;
			double rohr21Length = 0;
			double rohr2417Length = 0;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						if (pp.Product is HithermCompactProduct) {
							p = pp.Product as HithermCompactProduct;
							foreach (HithermCompactCircuit c in p.PlannedCircuits) {
								foreach (HithermCompactRegister register in c.Registers) {
									registerArea += register.RegisterArea;
									rohr2417Length = register.PipeHorizontal + register.PipeVertical;
								}
								rohr21Length += c.PipeLengthVorlaufWithoutOtherProductTotal + c.PipeLengthRuecklaufWithoutOtherProductTotal;
							}
						}
					}
				}
			}

			HithermCompactOverviewWrapper wrapper = new HithermCompactOverviewWrapper();
			wrapper.Text = "Heizfläche gesamt";
			wrapper.Amount = registerArea;
			wrapper.Unit = "m²";
			wrapperList.Add(wrapper);

			wrapper = new HithermCompactOverviewWrapper();
			wrapper.Text = "Rundrohr 21";
			wrapper.Amount = rohr21Length;
			wrapper.Unit = "m";
			wrapperList.Add(wrapper);

			wrapper = new HithermCompactOverviewWrapper();
			wrapper.Text = "Hitherm Klimawand 24/17";
			wrapper.Amount = rohr2417Length;
			wrapper.Unit = "m";
			wrapperList.Add(wrapper);

			return wrapperList;
		}

		public List<ModulBodenOverviewWrapper> GetModulBodenOverviewWrapper() {
			List<ModulBodenOverviewWrapper> wrapperList = new List<ModulBodenOverviewWrapper>();

			ModulKlimaBodenProduct p = null;
			double modulBodenArea = 0;
			double rohr21Length = 0;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						if (pp.Product is ModulKlimaBodenProduct) {
							p = pp.Product as ModulKlimaBodenProduct;
							foreach (ModulBodenCircuit c in p.PlannedCircuits) {
								foreach (KlimaFlaechenModul register in c.Row.List) {
									modulBodenArea += register.Area;
								}
								rohr21Length += c.Row.LengthVerbindeleitungen;
								rohr21Length += c.PipeLengthVorlaufWithoutOtherProductTotal + c.PipeLengthRuecklaufWithoutOtherProductTotal;
							}
						}
					}
				}
			}

			ModulBodenOverviewWrapper wrapper = new ModulBodenOverviewWrapper();
			wrapper.Text = "Fläche mit Modul Klimaboden";
			wrapper.Amount = modulBodenArea;
			wrapper.Unit = "m²";
			wrapperList.Add(wrapper);

			wrapper = new ModulBodenOverviewWrapper();
			wrapper.Text = "Rundrohr 21";
			wrapper.Amount = rohr21Length;
			wrapper.Unit = "m";
			wrapperList.Add(wrapper);

			return wrapperList;
		}

		public List<ModulDeckeOverviewWrapper> GetModulDeckeOverviewWrapper() {
			List<ModulDeckeOverviewWrapper> wrapperList = new List<ModulDeckeOverviewWrapper>();

			ModulKlimaDeckeProduct p = null;
			Dictionary<KlimaFlaechenModul.ModulTypeEnum, double> modulAreas = new Dictionary<KlimaFlaechenModul.ModulTypeEnum, double>();
			double rohr21Length = 0;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						if (pp.Product is ModulKlimaDeckeProduct) {
							p = pp.Product as ModulKlimaDeckeProduct;
							foreach (ModulDeckeCircuit c in p.PlannedCircuits) {
								foreach (ModulDeckeSubArea a in c.SubAreas) {
									foreach (KlimaFlaechenList l in a.Rows) {
										foreach (KlimaFlaechenModul register in l.List) {
											if (!modulAreas.ContainsKey(register.ModulType)) {
												modulAreas.Add(register.ModulType, register.Area);
											} else {
												modulAreas[register.ModulType] += register.Area;
											}
										}
										rohr21Length += l.LengthVerbindeleitungen;
									}
								}
								rohr21Length += c.PipeLengthVorlaufWithoutOtherProductTotal + c.PipeLengthRuecklaufWithoutOtherProductTotal;
							}
						}
					}
				}
			}

			ModulDeckeOverviewWrapper wrapper;

			foreach (KlimaFlaechenModul.ModulTypeEnum item in Enum.GetValues(typeof(KlimaFlaechenModul.ModulTypeEnum))) {
				wrapper = new ModulDeckeOverviewWrapper();
				wrapper.Text = "Fläche mit " + new KlimaFlaechenModul.ModulTypeEnumConverter().ConvertToString(item);
				if (modulAreas.ContainsKey(item)) {
					wrapper.Amount = modulAreas[item];
				}
				wrapper.Unit = "m²";
				wrapperList.Add(wrapper);
			}

			wrapper = new ModulDeckeOverviewWrapper();
			wrapper.Text = "Rundrohr 21";
			wrapper.Amount = rohr21Length;
			wrapper.Unit = "m";
			wrapperList.Add(wrapper);

			return wrapperList;
		}

		public List<VerlegedatenCircuitWrapper> GetVerlegedatenCircuitWrapper() {
			List<VerlegedatenCircuitWrapper> wrapperList = new List<VerlegedatenCircuitWrapper>();
			VerlegedatenCircuitWrapper wrapper = null;
			Dictionary<string, int> circuitCount = new Dictionary<string, int>();

			int count = 1;

			foreach (Floor floor in project.Floors) {
				foreach (Distributor d in floor.Distributors) {
					foreach (PlannedProduct pp in d.PlannedConnectedProducts) {
				//foreach (Room room in floor.Rooms) {
				//    foreach (PlannedProduct pp in room.PlannedProducts) {
						ProductConnection connection = pp.Product.PlannedConnection;
						foreach (Circuit c in pp.Product.PlannedCircuits) {
							if (connection != null && connection.ConnectionType == ProductConnection.ConnectionTypeEnum.DISTRIBUTOR) {
								wrapper = new VerlegedatenCircuitWrapper();
								wrapper.Name = "";
								wrapper.Area = "";

								wrapper.Distributor = connection.Distributor.Id + " " + connection.Distributor.Name + " " + connection.Distributor.AssociatedFloor.Name;
								wrapper.Durchfluss = c.C_DurchflussHeat / 60;
								
								foreach (ConnectionPipe pipe in pp.Product.PlannedConnectionPipes) {
									if (pipe.ConnectionThrough != null && pipe.ConnectionThrough.Product.PlannedProductIsConnection && (!pipe.OnlyFirst || c.NrOfCircuit == 0)) {
										wrapper.Name += "Anbindung durch Raum " + pipe.ConnectionThrough.Product.AssociatedRoom.Id + " (" + pipe.ConnectionThrough.Product.AssociatedRoom.Name + ")";
										wrapper.Name += ", " + new ConnectionPipe.PipeTypeEnumConverter().ConvertToString(pipe.PipeType);
										wrapper.Name += ", " + new ConnectionPipe.VerlegeartEnumConverter().ConvertToString(pipe.Verlegeart);
										if (pipe.Verlegeart != ConnectionPipe.VerlegeartEnum.VA_UNTER_ESTRICH && pipe.Insulation != ConnectionPipe.InsulationEnum.IN_NONE) {
											wrapper.Name += ", " + new ConnectionPipe.InsulationEnumConverter().ConvertToString(pipe.Insulation) + " gedämmt";
										}
										wrapper.Name += "\n";
										wrapper.Area += pipe.Vorlauf + "m\n";
									}
								}

								wrapper.Name += pp.Product.FullName + " in ";
								if (floor != connection.Distributor.AssociatedFloor) {
									wrapper.Name += floor.Name + ", ";
								}
								wrapper.Name += pp.Product.AssociatedRoom.Id + " (" + pp.Product.AssociatedRoom.Name + ")";
								if (pp.Product.PlannedCircuits.Count > 1) {
									wrapper.Name += ", Heizkreis " + (c.NrOfCircuit + 1);
								}

								wrapper.Area += pp.PlannedArea + "m²";
								if (!circuitCount.ContainsKey(connection.Distributor.Id)) {
									circuitCount.Add(connection.Distributor.Id, 1);							
								} 
								wrapper.CircuitNumber = circuitCount[connection.Distributor.Id]++;	
								if (pp.Product.ConnectedCircuits.ContainsKey(c.NrOfCircuit)) {
									Circuit.CircuitConnection con = pp.Product.ConnectedCircuits[c.NrOfCircuit];
									Product otherProduct = con.OtherProduct;
									wrapper.Name += "\n" + otherProduct.FullName + " in ";
									if (otherProduct.AssociatedRoom.AssociatedFloor != connection.Distributor.AssociatedFloor) {
										wrapper.Name += otherProduct.AssociatedRoom.AssociatedFloor.Name + ", ";
									}
									wrapper.Name += otherProduct.AssociatedRoom.Id + " (" + otherProduct.AssociatedRoom.Name + ")";
									if (pp.Product.PlannedCircuits.Count > 1) {
										wrapper.Name += ", Heizkreis " + (c.NrOfCircuit + 1);
									}
									wrapper.Area += "\n" + (otherProduct.PlannedFloorArea + otherProduct.PlannedWallArea + otherProduct.PlannedCeilingArea) + "m²";
								}
								foreach (ConnectionPipe pipe in pp.Product.PlannedConnectionPipes) {
									if (pipe.ConnectionThrough != null && pipe.ConnectionThrough.Product.PlannedProductIsConnection && (!pipe.OnlyFirst || c.NrOfCircuit == 0)) {
										wrapper.Name += "\n";
										wrapper.Name += "Anbindung durch Raum " + pipe.ConnectionThrough.Product.AssociatedRoom.Id + " (" + pipe.ConnectionThrough.Product.AssociatedRoom.Name + ")";
										wrapper.Name += ", " + new ConnectionPipe.PipeTypeEnumConverter().ConvertToString(pipe.PipeType);
										wrapper.Name += ", " + new ConnectionPipe.VerlegeartEnumConverter().ConvertToString(pipe.Verlegeart);
										if (pipe.Verlegeart != ConnectionPipe.VerlegeartEnum.VA_UNTER_ESTRICH && pipe.Insulation != ConnectionPipe.InsulationEnum.IN_NONE) {
											wrapper.Name += ", " + new ConnectionPipe.InsulationEnumConverter().ConvertToString(pipe.Insulation) + " gedämmt";
										}
										wrapper.Area += "\n" + pipe.Ruecklauf + "m";
									}
								}
								wrapperList.Add(wrapper);
							}
						}
					}
				}
			}

			return wrapperList;
		}

		public List<RequiredMaterialWrapper> GetRequiredMaterialWrapper() {
			Project.Instance.CalculateRequiredMaterial();

			List<RequiredMaterialWrapper> wrapperList = new List<RequiredMaterialWrapper>();
			RequiredMaterialWrapper wrapper = null;
			foreach (Category category in Project.Instance.Config.Categories) {
				foreach (Material material in category.Materials) {
					wrapper = new RequiredMaterialWrapper(material);
					if ((wrapper.RecommendedAmount.HasValue) && wrapper.RequiredAmount.HasValue) {
						wrapperList.Add(wrapper);
					}
				}
			}
			wrapperList.Sort(delegate(RequiredMaterialWrapper w1, RequiredMaterialWrapper w2) {
				if (w1.CategoryType.CompareTo(w2.CategoryType) == 0) {
					if (w1.Category.Order.CompareTo(w2.Category.Order) == 0) {
						return w1.PartNumber.CompareTo(w2.PartNumber);
					} else {
						return w1.Category.Order.CompareTo(w2.Category.Order);
					}
				} else {
					return w1.CategoryType.CompareTo(w2.CategoryType);
				}
			});
			return wrapperList;
		}

	}
}