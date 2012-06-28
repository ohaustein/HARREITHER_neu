using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;
using System.IO;
using System.Resources;
using System.Reflection;
using System.Threading;
using System.Collections;

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
			List<ModulDeckeWrapper> modulDeckeAuslegungWrapper = new List<ModulDeckeWrapper>();
			List<ModulDeckeVerlegeDatenWrapper> modulDeckeVerlegeDatenWrapper = new List<ModulDeckeVerlegeDatenWrapper>();
			List<BilanzWrapper> eurovalBilanzWrapper = new List<BilanzWrapper>();
			List<BilanzWrapper> ecothermBilanzWrapper = new List<BilanzWrapper>();
			List<BilanzWrapper> hithermBilanzWrapper = new List<BilanzWrapper>();
			List<BilanzWrapper> hithermCompactBilanzWrapper = new List<BilanzWrapper>();
			List<BilanzWrapper> modulBodenBilanzWrapper = new List<BilanzWrapper>();
			List<BilanzWrapper> modulDeckeBilanzWrapper = new List<BilanzWrapper>();
			List<VerlegedatenCircuitWrapper> verlegedatenCircuitWrapper = new List<VerlegedatenCircuitWrapper>();
			List<KonstruktionenWrapper> konstruktionenWrapper = new List<KonstruktionenWrapper>();
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
				modulDeckeAuslegungWrapper = GetModulDeckeWrapper();
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
				modulDeckeVerlegeDatenWrapper = GetModulDeckeVerlegeDatenWrapper();
			}

			if (reportOptions.Konstruktionen) {
				konstruktionenWrapper = GetKonstruktionenWrapper();
			}
			
			if (reportOptions.RequiredMaterial || reportOptions.RecommendedMaterial || reportOptions.Prices) {
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
			DataTable modulDeckeAuslegung = ReportHelper.ListToDataTable<ModulDeckeWrapper>(modulDeckeAuslegungWrapper);
			DataTable modulDeckeVerlegeDaten = ReportHelper.ListToDataTable<ModulDeckeVerlegeDatenWrapper>(modulDeckeVerlegeDatenWrapper);
			DataTable eurovalBilanz = ReportHelper.ListToDataTable<BilanzWrapper>(eurovalBilanzWrapper);
			DataTable ecothermBilanz = ReportHelper.ListToDataTable<BilanzWrapper>(ecothermBilanzWrapper);
			DataTable hithermBilanz = ReportHelper.ListToDataTable<BilanzWrapper>(hithermBilanzWrapper);
			DataTable hithermCompactBilanz = ReportHelper.ListToDataTable<BilanzWrapper>(hithermCompactBilanzWrapper);
			DataTable modulBodenBilanz = ReportHelper.ListToDataTable<BilanzWrapper>(modulBodenBilanzWrapper);
			DataTable modulDeckeBilanz = ReportHelper.ListToDataTable<BilanzWrapper>(modulDeckeBilanzWrapper);
			DataTable verlegedatenCircuit = ReportHelper.ListToDataTable<VerlegedatenCircuitWrapper>(verlegedatenCircuitWrapper);
			DataTable konstruktionen = ReportHelper.ListToDataTable<KonstruktionenWrapper>(konstruktionenWrapper);
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
			modulDeckeAuslegung.TableName = "ModulDeckeAuslegung";
			modulDeckeVerlegeDaten.TableName = "ModulDeckeVerlegeDaten";
			eurovalBilanz.TableName = "EurovalBilanz";
			ecothermBilanz.TableName = "EcothermBilanz";
			hithermBilanz.TableName = "HithermBilanz";
			hithermCompactBilanz.TableName = "HithermCompactBilanz";
			modulBodenBilanz.TableName = "ModulBodenBilanz";
			modulDeckeBilanz.TableName = "ModulDeckeBilanz";
			verlegedatenCircuit.TableName = "VerlegedatenCircuit";
			konstruktionen.TableName = "Konstruktionen";
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
			reportData.Tables.Add(modulDeckeAuslegung);
			reportData.Tables.Add(modulDeckeVerlegeDaten);
			reportData.Tables.Add(eurovalBilanz);
			reportData.Tables.Add(ecothermBilanz);
			reportData.Tables.Add(hithermBilanz);
			reportData.Tables.Add(hithermCompactBilanz);
			reportData.Tables.Add(modulBodenBilanz);
			reportData.Tables.Add(modulDeckeBilanz);
			reportData.Tables.Add(verlegedatenCircuit);
			reportData.Tables.Add(konstruktionen);
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

			Licensing.License license = Licensing.LicenseManager.Instance.License;

			listLabel1.Variables.Add("@PartnerContact", license.Header.Replace("\r", ""));
			listLabel1.Variables.Add("@ProgramVersion", project.EuroplanVersion);
			string filename = Configuration.UserTemplate.PartnerLogo;
			if (!license.IsModuleEnabled(Licensing.AbstractLicensedModule.FeatInternal) && File.Exists(filename)) {
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
			listLabel1.Variables.Add("@Konstruktionen", reportOptions.Konstruktionen);
			listLabel1.Variables.Add("@RequiredMaterial", reportOptions.RequiredMaterial);
			listLabel1.Variables.Add("@RecommendedMaterial", reportOptions.RecommendedMaterial);
			listLabel1.Variables.Add("@PriceMaterial", reportOptions.Prices);
			if (!license.IsModuleEnabled(Licensing.AbstractLicensedModule.FeatInternal)) {
				listLabel1.Variables.Add("@InternalLicense", false);
			} else {
				listLabel1.Variables.Add("@InternalLicense", true);
			}



			listLabel1.Dictionary.Clear();
			ResourceSet resourceSet = EuroplanRes.ResourceManager.GetResourceSet(Thread.CurrentThread.CurrentUICulture, false, true);
			if (resourceSet != null) {
			IDictionaryEnumerator enumerator = resourceSet.GetEnumerator();
				while (enumerator.MoveNext()) {
					if (enumerator.Key is string) {
						string key = enumerator.Key as string;
						if (key.StartsWith("LL_") || key.StartsWith("Unit_")) {
							listLabel1.Variables.Add("@" + key, ((string)enumerator.Value).Replace("\r", ""));
						}
					}
				}
			}

#if DEBUG
			if (MessageBox.Show("Designer?", "", MessageBoxButtons.YesNo) == DialogResult.Yes) {
				listLabel1.Design();
			}
#endif

			filename = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Reporting"), "ProjectReport.lst");
			try {
				listLabel1.Print(combit.ListLabel15.LlProject.List, filename, false, combit.ListLabel15.LlPrintMode.PreviewControl, combit.ListLabel15.LlBoxType.None, "", false, PathUtil.DataPath);
				GC.Collect();
            } catch (Exception) {
				DialogResult result = MessageBox.Show(EuroplanRes.QuickDimensioningPanel_DruckerFehlerText, EuroplanRes.QuickDimensioningPanel_DruckerFehlerTitel, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
				if (result == DialogResult.OK) {
					try {
						System.Diagnostics.Process p = new System.Diagnostics.Process();
						p.StartInfo.FileName = "rundll32.exe";
						p.StartInfo.Arguments = "printui.dll,PrintUIEntry /if /b \"Europlan 2.0 Reporting\" /f " + Environment.GetEnvironmentVariable("windir") + "\\inf\\ntprint.inf /r \"lpt1:\" /m \"HP LaserJet 4\"";
						p.Start();
						p.WaitForExit();
						listLabel1.Print(combit.ListLabel15.LlProject.List, filename, false, combit.ListLabel15.LlPrintMode.PreviewControl, combit.ListLabel15.LlBoxType.None, "", false, null);
					} catch (Exception) {
						MessageBox.Show(EuroplanRes.QuickDimensioningPanel_DruckerEinrichtungFehlerText, EuroplanRes.QuickDimensioningPanel_DruckerEinrichtungFehlerTitel, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
					normWaermeBedarf += room.HeatLoad;
					normKuehlBedarf += room.CoolLoad;
					normWaermeBedarfBereinigt += room.NormalizedHeatLoad;
					normKuehlBedarfBereinigt += room.NormalizedCoolLoad;
					roomArea += room.Area;
					foreach (PlannedProduct pp in room.PlannedProducts) {
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

						if (pp.Product.PlannedConnection != null && pp.Product.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.DISTRIBUTOR) {
							durchflussHeat += pp.Product.PlannedDurchflussHeat;
							durchflussCool += pp.Product.PlannedDurchflussCool;
						}
												
						deltaRhoHeatMax = deltaRhoHeatMax < pp.Product.PlannedDeltaRhoHeat ? pp.Product.PlannedDeltaRhoHeat : deltaRhoHeatMax;
						deltaRhoCoolMax = deltaRhoCoolMax < pp.Product.PlannedDeltaRhoCool ? pp.Product.PlannedDeltaRhoCool : deltaRhoCoolMax;
						wasserInhalt += pp.Product.WasserInhalt;
					}
				}
			}

			BilanzWrapper wrapper = new BilanzWrapper();
			wrapper.Description = EuroplanRes.ProjectReport_GesamtNormwaermebedarf; //"Gesamt-Normwärmebedarf";
			wrapper.HeatValue = normWaermeBedarf.ToString("0.##");
			wrapper.HeatUnit = EuroplanRes.Unit_Watt; //"W";
			wrapper.CoolValue = normKuehlBedarf.ToString("0.##");
			wrapper.CoolUnit = EuroplanRes.Unit_Watt; //"W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = EuroplanRes.ProjectReport_GesamterBereinigterWaermebedarf; //"Gesamter bereinigter Wärmebedarf";
			wrapper.HeatValue = (normWaermeBedarfBereinigt).ToString("0.##");
			wrapper.HeatUnit = EuroplanRes.Unit_Watt; //"W";
			wrapper.CoolValue = (normKuehlBedarfBereinigt).ToString("0.##");
			wrapper.CoolUnit = EuroplanRes.Unit_Watt; //"W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = EuroplanRes.ProjectReport_GesamteHeizleistung; //"Gesamt-Heizleistung (nach innen)";
			wrapper.HeatValue = qHeat.ToString("0.##");
			wrapper.HeatUnit = EuroplanRes.Unit_Watt; //"W";
			wrapper.CoolValue = qCool.ToString("0.##");
			wrapper.CoolUnit = EuroplanRes.Unit_Watt; //"W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = EuroplanRes.ProjectReport_GesamteAufgenommeneLeistung; //"Gesamte aufgenommene Leistung";
			wrapper.HeatValue = (transmissionFloorHeat + transmissionWallHeat + transmissionCeilingHeat + qHeat).ToString("0.##");
			wrapper.HeatUnit = EuroplanRes.Unit_Watt; //"W";
			wrapper.CoolValue = (transmissionFloorCool + transmissionWallCool + transmissionCeilingCool + qCool).ToString("0.##");
			wrapper.CoolUnit = EuroplanRes.Unit_Watt; //"W";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = EuroplanRes.ProjectReport_GesamtWassermenge; //"Gesamt-Wassermenge";
			wrapper.HeatValue = durchflussHeat.ToString("0.##");
			wrapper.HeatUnit = EuroplanRes.Unit_LiterProStunde; //"l/h";
			wrapper.CoolValue = durchflussCool.ToString("0.##");
			wrapper.CoolUnit = EuroplanRes.Unit_LiterProStunde; //"l/h";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = EuroplanRes.ProjectReport_GesamterMaximalerDruckverlust; //"Maximaler Druckverlust (inkl. Verteiler)";
			wrapper.HeatValue = deltaRhoHeatMax.ToString("0.##");
			wrapper.HeatUnit = EuroplanRes.Unit_Mbar; //"mbar";
			wrapper.CoolValue = deltaRhoCoolMax.ToString("0.##");
			wrapper.CoolUnit = EuroplanRes.Unit_Mbar; //"mbar";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = EuroplanRes.ProjectReport_GesamtWasserinhalt; //"Gesamt-Wasserinhalt (ab Verteiler)";
			wrapper.HeatValue = wasserInhalt.ToString("0.##");
			wrapper.HeatUnit = EuroplanRes.Unit_Liter; //"l";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = EuroplanRes.ProjectReport_GesamtRaumflaeche; //"Gesamt-Raumfläche";
			wrapper.HeatValue = roomArea.ToString("0.##");
			wrapper.HeatUnit = EuroplanRes.Unit_Quadratmeter; //"m²";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = EuroplanRes.ProjectReport_GesamtFussbodenheizungsflaeche; //"Gesamt-Fußbodenheizungsfläche";
			wrapper.HeatValue = plannedFloorArea.ToString("0.##");
			wrapper.HeatUnit = EuroplanRes.Unit_Quadratmeter; //"m²";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = EuroplanRes.ProjectReport_GesamtWandheizungsflaeche; //"Gesamt-Wandheizungsfläche";
			wrapper.HeatValue = plannedWallArea.ToString("0.##");
			wrapper.HeatUnit = EuroplanRes.Unit_Quadratmeter; //"m²";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = EuroplanRes.ProjectReport_GesamtDeckenkuehlungsflaeche; //"Gesamt-Deckenkühlungsfläche";
			wrapper.HeatValue = plannedCeilingArea.ToString("0.##");
			wrapper.HeatUnit = EuroplanRes.Unit_Quadratmeter; //"m²";
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

			bool isProductPlanned = false;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						if (pp.Product is EurovalProduct) {
							isProductPlanned = true;
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

			if (isProductPlanned) {

				BilanzWrapper wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_GewuenschterWaermebedarf; //"Gewünschter Wärmebedarf";
				wrapper.HeatValue = normWaermeBedarf.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Watt; //"W";
				wrapper.CoolValue = normKuehlBedarf.ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_Watt; //"W";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_BereinigterWaermebedarf; //"Bereinigter Wärmebedarf";
				wrapper.HeatValue = (normWaermeBedarf - normWaermeBedarfBereinigt).ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Watt; //"W";
				wrapper.CoolValue = (normKuehlBedarf - normKuehlBedarfBereinigt).ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_Watt; //"W";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_ErreichteHeizleistung; //"Erreichte Heizleistung nach innen";
				wrapper.HeatValue = qHeat.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Watt; //"W";
				wrapper.CoolValue = qCool.ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_Watt; //"W";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_ZugefuehrteHeizleistung; //"Gesamte zugeführte Heizleistung";
				wrapper.HeatValue = (transmissionFloorHeat + transmissionWallHeat + transmissionCeilingHeat + qHeat).ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Watt; //"W";
				wrapper.CoolValue = (transmissionFloorCool + transmissionWallCool + transmissionCeilingCool + qCool).ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_Watt; //"W";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_Wassermenge; //"Wassermenge";
				wrapper.HeatValue = durchflussHeat.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_LiterProStunde; //"l/h";
				wrapper.CoolValue = durchflussCool.ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_LiterProStunde; //"l/h";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_MaximalerDruckverlust; //"Maximaler Druckverlust (inkl. Verteiler)";
				wrapper.HeatValue = deltaRhoHeatMax.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Mbar; //"mbar";
				wrapper.CoolValue = deltaRhoCoolMax.ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_Mbar; //"mbar";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_Wasserinhalt; //"Wasserinhalt (ab Verteiler)";
				wrapper.HeatValue = wasserInhalt.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Liter; //"l";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_RaumflaecheEuroval; //"Gesamte Raumfläche (Räume mit Euroval® Fußbodenheizung)";
				wrapper.HeatValue = roomArea.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Quadratmeter; //"m²";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_Estrichflaeche; //"Gesamte Estrichfläche";
				wrapper.HeatValue = estrichArea.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Quadratmeter; //"m²";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_Heizflaeche; //"Gesamte Heizfläche";
				wrapper.HeatValue = plannedArea.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Quadratmeter; //"m²";
				wrapperList.Add(wrapper);

			}

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

			bool isProductPlanned = false;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						if (pp.Product is EcothermProduct) {
							isProductPlanned = true;
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

			if (isProductPlanned) {

				BilanzWrapper wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_GewuenschterWaermebedarf; //"Gewünschter Wärmebedarf";
				wrapper.HeatValue = normWaermeBedarf.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Watt; //"W";
				wrapper.CoolValue = normKuehlBedarf.ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_Watt; //"W";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_BereinigterWaermebedarf; //"Bereinigter Wärmebedarf";
				wrapper.HeatValue = (normWaermeBedarf - normWaermeBedarfBereinigt).ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Watt; //"W";
				wrapper.CoolValue = (normKuehlBedarf - normKuehlBedarfBereinigt).ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_Watt; //"W";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_ErreichteHeizleistung; //"Erreichte Heizleistung nach innen";
				wrapper.HeatValue = qHeat.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Watt; //"W";
				wrapper.CoolValue = qCool.ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_Watt; //"W";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_ZugefuehrteHeizleistung; //"Gesamte zugeführte Heizleistung";
				wrapper.HeatValue = (transmissionFloorHeat + transmissionWallHeat + transmissionCeilingHeat + qHeat).ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Watt; //"W";
				wrapper.CoolValue = (transmissionFloorCool + transmissionWallCool + transmissionCeilingCool + qCool).ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_Watt; //"W";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_Wassermenge; //"Wassermenge";
				wrapper.HeatValue = durchflussHeat.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_LiterProStunde; //"l/h";
				wrapper.CoolValue = durchflussCool.ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_LiterProStunde; //"l/h";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_MaximalerDruckverlust; //"Maximaler Druckverlust (inkl. Verteiler)";
				wrapper.HeatValue = deltaRhoHeatMax.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Mbar; //"mbar";
				wrapper.CoolValue = deltaRhoCoolMax.ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_Mbar; //"mbar";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_Wasserinhalt; //"Wasserinhalt (ab Verteiler)";
				wrapper.HeatValue = wasserInhalt.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Liter; //"l";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_RaumflaecheEcotherm; //"Gesamte Raumfläche (Räume mit Ecotherm® Fußbodenheizung)";
				wrapper.HeatValue = roomArea.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Quadratmeter; //"m²";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_Estrichflaeche; //"Gesamte Estrichfläche";
				wrapper.HeatValue = estrichArea.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Quadratmeter; //"m²";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_Heizflaeche; //"Gesamte Heizfläche";
				wrapper.HeatValue = plannedArea.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Quadratmeter; //"m²";
				wrapperList.Add(wrapper);

			}

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

			bool isProductPlanned = false;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						if (pp.Product is HithermProduct) {
							isProductPlanned = true;
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

			if (isProductPlanned) {

				BilanzWrapper wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_GewuenschterWaermebedarf; //"Gewünschter Wärmebedarf";
				wrapper.HeatValue = normWaermeBedarf.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Watt; //"W";
				wrapper.CoolValue = normKuehlBedarf.ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_Watt; //"W";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_BereinigterWaermebedarf; //"Bereinigter Wärmebedarf";
				wrapper.HeatValue = (normWaermeBedarf - normWaermeBedarfBereinigt).ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Watt; //"W";
				wrapper.CoolValue = (normKuehlBedarf - normKuehlBedarfBereinigt).ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_Watt; //"W";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_ErreichteHeizleistung; //"Erreichte Heizleistung nach innen";
				wrapper.HeatValue = qHeat.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Watt; //"W";
				wrapper.CoolValue = qCool.ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_Watt; //"W";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_ZugefuehrteHeizleistung; //"Gesamte zugeführte Heizleistung";
				wrapper.HeatValue = (transmissionFloorHeat + transmissionWallHeat + transmissionCeilingHeat + qHeat).ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Watt; //"W";
				wrapper.CoolValue = (transmissionFloorCool + transmissionWallCool + transmissionCeilingCool + qCool).ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_Watt; //"W";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_Wassermenge; //"Wassermenge";
				wrapper.HeatValue = durchflussHeat.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_LiterProStunde; //"l/h";
				wrapper.CoolValue = durchflussCool.ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_LiterProStunde; //"l/h";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_MaximalerDruckverlust; //"Maximaler Druckverlust (inkl. Verteiler)";
				wrapper.HeatValue = deltaRhoHeatMax.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Mbar; //"mbar";
				wrapper.CoolValue = deltaRhoCoolMax.ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_Mbar; //"mbar";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_Wasserinhalt; //"Wasserinhalt (ab Verteiler)";
				wrapper.HeatValue = wasserInhalt.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Liter; //"l";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_Heizflaeche; //"Gesamtheizfläche"; // TODO
				wrapper.HeatValue = totalArea.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Quadratmeter; //"m²";
				wrapperList.Add(wrapper);

			}

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

			bool isProductPlanned = false;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						if (pp.Product is HithermCompactProduct) {
							isProductPlanned = true;
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

			if (isProductPlanned) {

				BilanzWrapper wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_GewuenschterWaermebedarf; //"Gewünschter Wärmebedarf";
				wrapper.HeatValue = normWaermeBedarf.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Watt; //"W";
				wrapper.CoolValue = normKuehlBedarf.ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_Watt; //"W";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_BereinigterWaermebedarf; //"Bereinigter Wärmebedarf";
				wrapper.HeatValue = (normWaermeBedarf - normWaermeBedarfBereinigt).ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Watt; //"W";
				wrapper.CoolValue = (normKuehlBedarf - normKuehlBedarfBereinigt).ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_Watt; //"W";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_ErreichteHeizleistung; //"Erreichte Heizleistung nach innen";
				wrapper.HeatValue = qHeat.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Watt; //"W";
				wrapper.CoolValue = qCool.ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_Watt; //"W";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_ZugefuehrteHeizleistung; //"Gesamte zugeführte Heizleistung";
				wrapper.HeatValue = (transmissionFloorHeat + transmissionWallHeat + transmissionCeilingHeat + qHeat).ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Watt; //"W";
				wrapper.CoolValue = (transmissionFloorCool + transmissionWallCool + transmissionCeilingCool + qCool).ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_Watt; //"W";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_Wassermenge; //"Wassermenge";
				wrapper.HeatValue = durchflussHeat.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_LiterProStunde; //"l/h";
				wrapper.CoolValue = durchflussCool.ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_LiterProStunde; //"l/h";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_MaximalerDruckverlust; //"Maximaler Druckverlust (inkl. Verteiler)";
				wrapper.HeatValue = deltaRhoHeatMax.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Mbar; //"mbar";
				wrapper.CoolValue = deltaRhoCoolMax.ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_Mbar; //"mbar";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_Wasserinhalt; //"Wasserinhalt (ab Verteiler)";
				wrapper.HeatValue = wasserInhalt.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Liter; //"l";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_Heizflaeche; //"Gesamtheizfläche"; // TODO
				wrapper.HeatValue = totalArea.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Quadratmeter; //"m²";
				wrapperList.Add(wrapper);

			}

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

			bool isProductPlanned = false;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						if (pp.Product is ModulKlimaBodenProduct) {
							isProductPlanned = true;
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

			if (isProductPlanned) {

				BilanzWrapper wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_GewuenschterWaermebedarf; //"Gewünschter Wärmebedarf";
				wrapper.HeatValue = normWaermeBedarf.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Watt; //"W";
				wrapper.CoolValue = normKuehlBedarf.ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_Watt; //"W";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_BereinigterWaermebedarf; //"Bereinigter Wärmebedarf";
				wrapper.HeatValue = (normWaermeBedarf - normWaermeBedarfBereinigt).ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Watt; //"W";
				wrapper.CoolValue = (normKuehlBedarf - normKuehlBedarfBereinigt).ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_Watt; //"W";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_ErreichteHeizleistung; //"Erreichte Heizleistung nach innen";
				wrapper.HeatValue = qHeat.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Watt; //"W";
				wrapper.CoolValue = qCool.ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_Watt; //"W";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_ZugefuehrteHeizleistung; //"Gesamte zugeführte Heizleistung";
				wrapper.HeatValue = (transmissionFloorHeat + transmissionWallHeat + transmissionCeilingHeat + qHeat).ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Watt; //"W";
				wrapper.CoolValue = (transmissionFloorCool + transmissionWallCool + transmissionCeilingCool + qCool).ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_Watt; //"W";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_Wassermenge; //"Wassermenge";
				wrapper.HeatValue = durchflussHeat.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_LiterProStunde; //"l/h";
				wrapper.CoolValue = durchflussCool.ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_LiterProStunde; //"l/h";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_MaximalerDruckverlust; //"Maximaler Druckverlust (inkl. Verteiler)";
				wrapper.HeatValue = deltaRhoHeatMax.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Mbar; //"mbar";
				wrapper.CoolValue = deltaRhoCoolMax.ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_Mbar; //"mbar";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_Wasserinhalt; //"Wasserinhalt (ab Verteiler)";
				wrapper.HeatValue = wasserInhalt.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Liter; //"l";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_RaumflaecheKlimaboden; //"Gesamte Raumfläche (Räume mit Modul Klimaboden)";
				wrapper.HeatValue = roomArea.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Quadratmeter; //"m²";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_BelegteFlaeche; //"Gesamte belegte Fläche";
				wrapper.HeatValue = coveredArea.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Quadratmeter; //"m²";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_BeheizteFlaeche; //"Gesamte beheizte Fläche";
				wrapper.HeatValue = modulArea.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Quadratmeter; //"m²";
				wrapperList.Add(wrapper);

			}
			
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

			bool isProductPlanned = false;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						if (pp.Product is ModulKlimaDeckeProduct) {
							isProductPlanned = true;
							normWaermeBedarf += pp.RequestedHeatLoad;
							normKuehlBedarf += pp.RequestedCoolLoad;

							roomArea += room.Area;
							totalArea += ((ModulKlimaDeckeProduct)pp.Product).CoveredArea;

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

			if (isProductPlanned) {

				BilanzWrapper wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_GewuenschterWaermebedarf; //"Gewünschter Wärmebedarf";
				wrapper.HeatValue = normWaermeBedarf.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Watt; //"W";
				wrapper.CoolValue = normKuehlBedarf.ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_Watt; //"W";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_BereinigterWaermebedarf; //"Bereinigter Wärmebedarf";
				wrapper.HeatValue = (normWaermeBedarf - normWaermeBedarfBereinigt).ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Watt; //"W";
				wrapper.CoolValue = (normKuehlBedarf - normKuehlBedarfBereinigt).ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_Watt; //"W";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_ErreichteHeizleistung; //"Erreichte Heizleistung nach innen";
				wrapper.HeatValue = qHeat.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Watt; //"W";
				wrapper.CoolValue = qCool.ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_Watt; //"W";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_ZugefuehrteHeizleistung; //"Gesamte zugeführte Heizleistung";
				wrapper.HeatValue = (transmissionFloorHeat + transmissionWallHeat + transmissionCeilingHeat + qHeat).ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Watt; //"W";
				wrapper.CoolValue = (transmissionFloorCool + transmissionWallCool + transmissionCeilingCool + qCool).ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_Watt; //"W";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_Wassermenge; //"Wassermenge";
				wrapper.HeatValue = durchflussHeat.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_LiterProStunde; //"l/h";
				wrapper.CoolValue = durchflussCool.ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_LiterProStunde; //"l/h";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_MaximalerDruckverlust; //"Maximaler Druckverlust (inkl. Verteiler)";
				wrapper.HeatValue = deltaRhoHeatMax.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Mbar; //"mbar";
				wrapper.CoolValue = deltaRhoCoolMax.ToString("0.##");
				wrapper.CoolUnit = EuroplanRes.Unit_Mbar; //"mbar";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_Wasserinhalt; //"Wasserinhalt (ab Verteiler)";
				wrapper.HeatValue = wasserInhalt.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Liter; //"l";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_RaumflaecheKlimadecke; //"Gesamte Raumfläche (Räume mit Modul Klimadecke)";
				wrapper.HeatValue = roomArea.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Quadratmeter; //"m²";
				wrapperList.Add(wrapper);

				wrapper = new BilanzWrapper();
				wrapper.Description = EuroplanRes.ProjectReport_BelegteFlaeche; //"Gesamte belegte Fläche";
				wrapper.HeatValue = totalArea.ToString("0.##");
				wrapper.HeatUnit = EuroplanRes.Unit_Quadratmeter; //"m²";
				wrapperList.Add(wrapper);

			}

			return wrapperList;
		}

		public List<ProjectWarningWrapper> GetProjectWarnings() {
			List<ProjectWarningWrapper> wrapperList = new List<ProjectWarningWrapper>();

			foreach (Floor floor in Project.Instance.Floors) {
				foreach (Distributor dist in floor.Distributors) {
					foreach (string error in dist.ErrorMessageArray) {
						ProjectWarningWrapper wrapper = new ProjectWarningWrapper();
						wrapper.FloorId = floor.Id;
						wrapper.FloorName = floor.Name;
						string message = EuroplanRes.ProjectReport_VerteilerWarnung;
						message = message.Replace("%VERTEILERID%", dist.Id);
						message = message.Replace("%VERTEILERNAME%", dist.Name);
						message = message.Replace("%WARNUNG%", error);
						wrapper.Warning = message;

						wrapperList.Add(wrapper);
					}
				}
			}

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct plannedProduct in room.PlannedProducts) {
						foreach (string warning in plannedProduct.Product.ErrorMessageArray) {
							ProjectWarningWrapper wrapper = new ProjectWarningWrapper();
							wrapper.FloorId = floor.Id;
							wrapper.FloorName = floor.Name;
							string message = EuroplanRes.ProjectReport_Warnung;
							message = message.Replace("%SYSTEM%", plannedProduct.InternalName);
							message = message.Replace("%RAUMID%", room.Id);
							message = message.Replace("%RAUMNAME%", room.Name);
							message = message.Replace("%WARNUNG%", warning);
							wrapper.Warning = message;
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

			foreach (string notification in Project.Instance.NotificationMessageArray) {
				wrapper = new ProjectWarningWrapper();
				wrapper.Warning = notification;
				wrapperList.Add(wrapper);
			}

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct plannedProduct in room.PlannedProducts) {
						foreach (string notification in plannedProduct.Product.NotificationMessageArray) {
							wrapper = new ProjectWarningWrapper();
							wrapper.FloorId = floor.Id;
							wrapper.FloorName = floor.Name;
							string message = EuroplanRes.ProjectReport_Hinweis;
							message = message.Replace("%SYSTEM%", plannedProduct.InternalName);
							message = message.Replace("%RAUMID%", room.Id);
							message = message.Replace("%RAUMNAME%", room.Name);
							message = message.Replace("%HINWEIS%", notification);
							wrapper.Warning = message;
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
				wrapper.HeatOrCool = EuroplanRes.LL_Report_Heizbetrieb; //"Heizbetrieb";
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
					wrapper.HeatOrCool = EuroplanRes.LL_Report_Kuehlbetrieb; //"Kühlbetrieb";
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
					normWaermeBedarf = room.NormalizedHeatLoad;
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
					normKuehlBedarf = room.NormalizedCoolLoad;
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
							}
							foreach (PlannedProduct pp in d.PlannedDirectAndIndirectConnectedProducts) {
								wasserInhalt += pp.Product.WasserInhalt;
							}
						}
					}
				}
				string anschlussDimensionierung = "";
				double maxDurchfluss = Math.Max(durchflussHeat, durchflussCool);
				if (maxDurchfluss <= 1000) {
					anschlussDimensionierung = EuroplanRes.ProjectReport_HISAN32;
				} else if (maxDurchfluss > 1000 && maxDurchfluss <= 2000) {
					anschlussDimensionierung = EuroplanRes.ProjectReport_HISAN40;
				} else if (maxDurchfluss > 2000 && maxDurchfluss <= 3000) {
					anschlussDimensionierung = EuroplanRes.ProjectReport_HISAN50;
				} else if (maxDurchfluss > 3000 && maxDurchfluss <= 5000) {
					anschlussDimensionierung = EuroplanRes.ProjectReport_HISAN63;
				} else if (maxDurchfluss > 5000 && maxDurchfluss <= 7000) {
					anschlussDimensionierung = EuroplanRes.ProjectReport_HISAN75;
				}

				wrapper = new RegulatorCircuitWrapper();
				wrapper.HeatOrCool = EuroplanRes.LL_Report_Heizbetrieb; //"Heizbetrieb";
				wrapper.Id = rc.Id;
				wrapper.Name = rc.Name;
				wrapper.Medium = EuroplanRes.ProjectReport_Wasser; //"Wasser";
				wrapper.VorlaufTemp = rc.HeatFlowTemperature;
				wrapper.RuecklaufTemp = ruecklaufHeat;
				wrapper.Durchfluss = durchflussHeat;
				wrapper.AnschlussDimensionierung = anschlussDimensionierung;
				wrapper.Druckverlust = deltaRhoHeat;
				wrapper.Inhalt = wasserInhalt;
				wrapperHeatList.Add(wrapper);
				if (project.CalculateCoolLoad) {
					wrapper = new RegulatorCircuitWrapper();
					wrapper.HeatOrCool = EuroplanRes.LL_Report_Kuehlbetrieb; //"Kühlbetrieb";
					wrapper.Id = rc.Id;
					wrapper.Name = rc.Name;
					wrapper.Medium = EuroplanRes.ProjectReport_Wasser; //"Wasser";
					wrapper.VorlaufTemp = rc.CoolFlowTemperature;
					wrapper.RuecklaufTemp = ruecklaufCool;
					wrapper.Durchfluss = durchflussCool;
					wrapper.AnschlussDimensionierung = anschlussDimensionierung;
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
					}
					foreach (PlannedProduct pp in distributor.PlannedDirectAndIndirectConnectedProducts) {
						wasserInhalt += pp.Product.WasserInhalt;
					}

					string anschlussDimensionierung = "";
					double maxDurchfluss = Math.Max(durchflussHeat, durchflussCool);
					if (maxDurchfluss <= 1000) {
						anschlussDimensionierung = EuroplanRes.ProjectReport_HISAN32;
					} else if (maxDurchfluss > 1000 && maxDurchfluss <= 2000) {
						anschlussDimensionierung = EuroplanRes.ProjectReport_HISAN40;
					} else if (maxDurchfluss > 2000 && maxDurchfluss <= 3000) {
						anschlussDimensionierung = EuroplanRes.ProjectReport_HISAN50;
					} else if (maxDurchfluss > 3000 && maxDurchfluss <= 5000) {
						anschlussDimensionierung = EuroplanRes.ProjectReport_HISAN63;
					} else if (maxDurchfluss > 5000 && maxDurchfluss <= 7000) {
						anschlussDimensionierung = EuroplanRes.ProjectReport_HISAN75;
					}

					wrapper = new DistributorWrapper();
					wrapper.HeatOrCool = EuroplanRes.LL_Report_Heizbetrieb; //"Heizbetrieb";
					wrapper.Id = distributor.Id;
					wrapper.Name = distributor.Name;
					wrapper.Groups = distributor.PlannedCircuits + distributor.AdditionalCircuits;
					wrapper.RegulatorCircuit = distributor.RegulatorCircuitId;
					wrapper.VorlaufTemp = distributor.RegulatorCircuit.HeatFlowTemperature;
					wrapper.RuecklaufTemp = ruecklaufHeat;
					wrapper.Durchfluss = durchflussHeat;
					wrapper.AnschlussDimensionierung = anschlussDimensionierung;
					wrapper.Druckverlust = deltaRhoHeat;
					wrapper.Inhalt = wasserInhalt;
					wrapperHeatList.Add(wrapper);
					if (project.CalculateCoolLoad) {
						wrapper = new DistributorWrapper();
						wrapper.HeatOrCool = EuroplanRes.LL_Report_Kuehlbetrieb; //"Kühlbetrieb";
						wrapper.Id = distributor.Id;
						wrapper.Name = distributor.Name;
						wrapper.Groups = distributor.PlannedCircuits + distributor.AdditionalCircuits;
						wrapper.RegulatorCircuit = distributor.RegulatorCircuitId;
						wrapper.VorlaufTemp = distributor.RegulatorCircuit.CoolFlowTemperature;
						wrapper.RuecklaufTemp = ruecklaufCool;
						wrapper.Durchfluss = durchflussCool;
						wrapper.AnschlussDimensionierung = anschlussDimensionierung;
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
					wrapper.HeatNetLoad = room.NormalizedHeatLoad;
					wrapper.HeatPower = heatPower;
					wrapper.CoolTemperature = room.RoomCoolTemperature;
					wrapper.CoolNetLoad = room.NormalizedCoolLoad;
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

			EurovalWrapper wrapperOverview = null;
			EurovalWrapper wrapperHeat = null;
			EurovalWrapper wrapperCool = null;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						wrapperOverview = null;
						wrapperHeat = null;
						wrapperCool = null;
						if (pp.Product is EurovalProduct) {
							EurovalProduct ep = pp.Product as EurovalProduct;

							wrapperOverview = new EurovalWrapper();
							wrapperOverview.HeatOrCool = "overview";
							wrapperOverview.FloorId = floor.Id;
							wrapperOverview.FloorName = floor.Name;

							wrapperOverview.RoomId = room.Id;
							wrapperOverview.RoomName = room.Name;
							wrapperOverview.TeilSystem = pp.InternalName;
							if (pp.Product.HasInsideConstruction) {
								wrapperOverview.InsideConstruction = pp.Product.PlannedInsideConstruction.Id;
								wrapperOverview.InsideRValue = pp.Product.PlannedInsideConstructionRValue;
							}
							if (pp.Product.HasOutsideConstruction) {
								wrapperOverview.OutsideConstruction = pp.Product.PlannedOutsideConstruction.Id;
								wrapperOverview.OutsideRValue = pp.Product.PlannedOutsideConstructionRValue;
							}
							wrapperOverview.Circuits = pp.Product.PlannedCircuitCount;
							wrapperOverview.RzLayDistance = ep.PlannedRimLayDistance.ToString();
							wrapperOverview.RzWidth = ep.PlannedRimWidth;
							wrapperOverview.RzArea = ep.PlannedAreaRim;
							wrapperOverview.AzLayDistance = ep.PlannedLayDistance.ToString();
							wrapperOverview.AzArea = ep.PlannedAreaResidence;
							wrapperOverview.ConnectionArea = ep.PlannedRemoveArea;

							double v, r;
							pp.Product.GetHeatFlow(out v, out r);
							wrapperOverview.RoomTemp = room.RoomHeatTemperature;
							wrapperOverview.VorlaufTemp = v;
							wrapperOverview.RuecklaufTemp = r;
							wrapperOverview.QSoll = pp.RequestedHeatLoad;
							wrapperOverview.QFBH = pp.PlannedHeatLoad;
							wrapperOverview.tFBAz = ep.PlannedFloorTemperatureHeatResidence;
							wrapperOverview.tFBRz = ep.PlannedFloorTemperatureHeatRim;

							wrapperOverview.Wassermenge = pp.Product.PlannedDurchflussHeat;
							wrapperOverview.DruckverlustRohr = pp.Product.PlannedDeltaRhoHeat;
							wrapperOverview.DruckverlustVerteiler = pp.Product.PlannedDeltaRhoDistributorHeat;

							wrapperOverview.UnusedArea = ep.PlannedAreaUnheated;

							if (ep.PlannedConnection != null) {
								if (ep.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
									wrapperOverview.SubSystem = true;
									wrapperOverview.VorlaufTemp = -1;
									wrapperOverview.RuecklaufTemp = -1;
								}
							}
							if (ep.IsOtherProductConnected) {
								wrapperOverview.OtherSystemsConnected = true;
							}

							wrapperHeatList.Add(wrapperOverview);

							EurovalWrapper prevWrapper = null;
							foreach (EurovalCircuit ec in ep.PlannedCircuits) {
								EurovalWrapper wrapper = new EurovalWrapper(wrapperOverview);
								wrapper.UsedAsCircuitWrapper = true;

								wrapper.AzArea = ec.AreaAz;
								wrapper.RzArea = ec.RimLength;

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
									ok = ok && Math.Round(prevWrapper.AzArea, 3) == Math.Round(wrapper.AzArea, 3);
									ok = ok && Math.Round(prevWrapper.RzArea, 3) == Math.Round(wrapper.RzArea, 3);
									ok = ok && Math.Round(prevWrapper.LengthRzAz, 3) == Math.Round(wrapper.LengthRzAz, 3);
									ok = ok && Math.Round(prevWrapper.LengthConnection, 3) == Math.Round(wrapper.LengthConnection, 3);
									ok = ok && Math.Round(prevWrapper.LengthCircuitFbh, 3) == Math.Round(wrapper.LengthCircuitFbh, 3);
									ok = ok && Math.Round(prevWrapper.LengthCircuitAll, 3) == Math.Round(wrapper.LengthCircuitAll, 3);
									ok = ok && Math.Round(prevWrapper.Wassermenge, 3) == Math.Round(wrapper.Wassermenge, 3);
									ok = ok && Math.Round(prevWrapper.DruckverlustRohr, 3) == Math.Round(wrapper.DruckverlustRohr, 3);
									ok = ok && Math.Round(prevWrapper.DruckverlustVerteiler, 3) == Math.Round(wrapper.DruckverlustVerteiler, 3);
									ok = ok && Math.Round(prevWrapper.V, 3) == Math.Round(wrapper.V, 3);

									if (ok) {
										prevWrapper.CircuitsAsString = prevWrapper.Circuits.ToString() + "-" + wrapper.Circuits.ToString();
									} else {
										prevWrapper = wrapper;
										wrapperHeatList.Add(wrapper);
									}
								}
							}

							if (wrapperHeat == null && pp.RequestedHeatLoad != 0) {
								wrapperHeat = new EurovalWrapper();
								wrapperHeat.HeatOrCool = EuroplanRes.LL_Report_Heizbetrieb; //"Heizen";
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
							if (project.CalculateCoolLoad && wrapperCool == null && pp.RequestedCoolLoad != 0) {
								wrapperCool = new EurovalWrapper();
								wrapperCool.HeatOrCool = EuroplanRes.LL_Report_Kuehlbetrieb; //"Kühlen";
								wrapperCool.FloorId = floor.Id;
								wrapperCool.FloorName = floor.Name;

								wrapperCool.RoomId = room.Id;
								wrapperCool.RoomName = room.Name;
								wrapperCool.TeilSystem = pp.InternalName;
								if (pp.Product.HasInsideConstruction) {
									wrapperCool.InsideConstruction = pp.Product.PlannedInsideConstruction.Id;
									wrapperCool.InsideRValue = pp.Product.PlannedInsideConstructionRValue;
								}
								if (pp.Product.HasOutsideConstruction) {
									wrapperCool.OutsideConstruction = pp.Product.PlannedOutsideConstruction.Id;
									wrapperCool.OutsideRValue = pp.Product.PlannedOutsideConstructionRValue;
								}
								wrapperCool.Circuits = pp.Product.PlannedCircuitCount;
								wrapperCool.RzLayDistance = ep.PlannedRimLayDistance.ToString();
								wrapperCool.RzWidth = ep.PlannedRimWidth;
								wrapperCool.RzArea = ep.PlannedAreaRim;
								wrapperCool.AzLayDistance = ep.PlannedLayDistance.ToString();
								wrapperCool.AzArea = ep.PlannedAreaResidence;
								wrapperCool.ConnectionArea = ep.PlannedRemoveArea;

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

									prevWrapper = null;
									foreach (EurovalCircuit ec in ep.PlannedCircuits) {
										EurovalWrapper wrapper = new EurovalWrapper(wrapperHeat);
										wrapper.UsedAsCircuitWrapper = true;

										wrapper.AzArea = ec.AreaAz;
										wrapper.RzArea = ec.RimLength;

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
											ok = ok && Math.Round(prevWrapper.AzArea, 3) == Math.Round(wrapper.AzArea, 3);
											ok = ok && Math.Round(prevWrapper.RzArea, 3) == Math.Round(wrapper.RzArea, 3);
											ok = ok && Math.Round(prevWrapper.LengthRzAz, 3) == Math.Round(wrapper.LengthRzAz, 3);
											ok = ok && Math.Round(prevWrapper.LengthConnection, 3) == Math.Round(wrapper.LengthConnection, 3);
											ok = ok && Math.Round(prevWrapper.LengthCircuitFbh, 3) == Math.Round(wrapper.LengthCircuitFbh, 3);
											ok = ok && Math.Round(prevWrapper.LengthCircuitAll, 3) == Math.Round(wrapper.LengthCircuitAll, 3);
											ok = ok && Math.Round(prevWrapper.Wassermenge, 3) == Math.Round(wrapper.Wassermenge, 3);
											ok = ok && Math.Round(prevWrapper.DruckverlustRohr, 3) == Math.Round(wrapper.DruckverlustRohr, 3);
											ok = ok && Math.Round(prevWrapper.DruckverlustVerteiler, 3) == Math.Round(wrapper.DruckverlustVerteiler, 3);
											ok = ok && Math.Round(prevWrapper.V, 3) == Math.Round(wrapper.V, 3);

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

									prevWrapper = null;
									foreach (EurovalCircuit ec in ep.PlannedCircuits) {
										EurovalWrapper wrapper = new EurovalWrapper(wrapperCool);
										wrapper.UsedAsCircuitWrapper = true;

										wrapper.AzArea = ec.AreaAz;
										wrapper.RzArea = ec.RimLength;

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
											ok = ok && Math.Round(prevWrapper.AzArea, 3) == Math.Round(wrapper.AzArea, 3);
											ok = ok && Math.Round(prevWrapper.RzArea, 3) == Math.Round(wrapper.RzArea, 3);
											ok = ok && Math.Round(prevWrapper.LengthRzAz, 3) == Math.Round(wrapper.LengthRzAz, 3);
											ok = ok && Math.Round(prevWrapper.LengthConnection, 3) == Math.Round(wrapper.LengthConnection, 3);
											ok = ok && Math.Round(prevWrapper.LengthCircuitFbh, 3) == Math.Round(wrapper.LengthCircuitFbh, 3);
											ok = ok && Math.Round(prevWrapper.LengthCircuitAll, 3) == Math.Round(wrapper.LengthCircuitAll, 3);
											ok = ok && Math.Round(prevWrapper.Wassermenge, 3) == Math.Round(wrapper.Wassermenge, 3);
											ok = ok && Math.Round(prevWrapper.DruckverlustRohr, 3) == Math.Round(wrapper.DruckverlustRohr, 3);
											ok = ok && Math.Round(prevWrapper.DruckverlustVerteiler, 3) == Math.Round(wrapper.DruckverlustVerteiler, 3);
											ok = ok && Math.Round(prevWrapper.V, 3) == Math.Round(wrapper.V, 3);

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

			EcothermWrapper wrapperOverview = null;
			EcothermWrapper wrapperHeat = null;
			EcothermWrapper wrapperCool = null;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						wrapperOverview = null;
						wrapperHeat = null;
						wrapperCool = null;
						if (pp.Product is EcothermProduct) {
							EcothermProduct ep = pp.Product as EcothermProduct;

							wrapperOverview = new EcothermWrapper();
							wrapperOverview.HeatOrCool = "overview";
							wrapperOverview.FloorId = floor.Id;
							wrapperOverview.FloorName = floor.Name;

							wrapperOverview.RoomId = room.Id;
							wrapperOverview.RoomName = room.Name;
							wrapperOverview.TeilSystem = pp.InternalName;
							if (pp.Product.HasInsideConstruction) {
								wrapperOverview.InsideConstruction = pp.Product.PlannedInsideConstruction.Id;
								wrapperOverview.InsideRValue = pp.Product.PlannedInsideConstructionRValue;
							}
							if (pp.Product.HasOutsideConstruction) {
								wrapperOverview.OutsideConstruction = pp.Product.PlannedOutsideConstruction.Id;
								wrapperOverview.OutsideRValue = pp.Product.PlannedOutsideConstructionRValue;
							}
							wrapperOverview.Circuits = pp.Product.PlannedCircuitCount;
							wrapperOverview.RzLayDistance = ep.PlannedRimLayDistance.ToString();
							wrapperOverview.RzWidth = ep.PlannedRimWidth;
							wrapperOverview.RzArea = ep.PlannedAreaRim;
							wrapperOverview.AzLayDistance = ep.PlannedLayDistance.ToString();
							wrapperOverview.AzArea = ep.PlannedAreaResidence;
							wrapperOverview.ConnectionArea = ep.PlannedRemoveArea;

							double v, r;
							pp.Product.GetHeatFlow(out v, out r);
							wrapperOverview.RoomTemp = room.RoomHeatTemperature;
							wrapperOverview.VorlaufTemp = v;
							wrapperOverview.RuecklaufTemp = r;
							wrapperOverview.QSoll = pp.RequestedHeatLoad;
							wrapperOverview.QFBH = pp.PlannedHeatLoad;
							wrapperOverview.tFBAz = ep.PlannedFloorTemperatureHeatResidence;
							wrapperOverview.tFBRz = ep.PlannedFloorTemperatureHeatRim;

							wrapperOverview.Wassermenge = pp.Product.PlannedDurchflussHeat;
							wrapperOverview.DruckverlustRohr = pp.Product.PlannedDeltaRhoHeat;
							wrapperOverview.DruckverlustVerteiler = pp.Product.PlannedDeltaRhoDistributorHeat;

							wrapperOverview.UnusedArea = ep.PlannedAreaUnheated;

							if (ep.PlannedConnection != null) {
								if (ep.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
									wrapperOverview.SubSystem = true;
									wrapperOverview.VorlaufTemp = -1;
									wrapperOverview.RuecklaufTemp = -1;
								}
							}
							if (ep.IsOtherProductConnected) {
								wrapperOverview.OtherSystemsConnected = true;
							}

							wrapperHeatList.Add(wrapperOverview);

							EcothermWrapper prevWrapper = null;
							foreach (EcothermCircuit ec in ep.PlannedCircuits) {
								EcothermWrapper wrapper = new EcothermWrapper(wrapperOverview);
								wrapper.UsedAsCircuitWrapper = true;

								wrapper.AzArea = ec.AreaAz;
								wrapper.RzArea = ec.RimLength;

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
									ok = ok && Math.Round(prevWrapper.AzArea, 3) == Math.Round(wrapper.AzArea, 3);
									ok = ok && Math.Round(prevWrapper.RzArea, 3) == Math.Round(wrapper.RzArea, 3);
									ok = ok && Math.Round(prevWrapper.LengthRzAz, 3) == Math.Round(wrapper.LengthRzAz, 3);
									ok = ok && Math.Round(prevWrapper.LengthConnection, 3) == Math.Round(wrapper.LengthConnection, 3);
									ok = ok && Math.Round(prevWrapper.LengthCircuitFbh, 3) == Math.Round(wrapper.LengthCircuitFbh, 3);
									ok = ok && Math.Round(prevWrapper.LengthCircuitAll, 3) == Math.Round(wrapper.LengthCircuitAll, 3);
									ok = ok && Math.Round(prevWrapper.Wassermenge, 3) == Math.Round(wrapper.Wassermenge, 3);
									ok = ok && Math.Round(prevWrapper.DruckverlustRohr, 3) == Math.Round(wrapper.DruckverlustRohr, 3);
									ok = ok && Math.Round(prevWrapper.DruckverlustVerteiler, 3) == Math.Round(wrapper.DruckverlustVerteiler, 3);
									ok = ok && Math.Round(prevWrapper.V, 3) == Math.Round(wrapper.V, 3);

									if (ok) {
										prevWrapper.CircuitsAsString = prevWrapper.Circuits.ToString() + "-" + wrapper.Circuits.ToString();
									} else {
										prevWrapper = wrapper;
										wrapperHeatList.Add(wrapper);
									}
								}
							}

							if (wrapperHeat == null && pp.RequestedHeatLoad != 0) {
								wrapperHeat = new EcothermWrapper();
								wrapperHeat.HeatOrCool = EuroplanRes.LL_Report_Heizbetrieb; //"Heizen";
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
							if (project.CalculateCoolLoad && wrapperCool == null && pp.RequestedCoolLoad != 0) {
								wrapperCool = new EcothermWrapper();
								wrapperCool.HeatOrCool = EuroplanRes.LL_Report_Kuehlbetrieb; //"Kühlen";
								wrapperCool.FloorId = floor.Id;
								wrapperCool.FloorName = floor.Name;

								wrapperCool.RoomId = room.Id;
								wrapperCool.RoomName = room.Name;
								wrapperCool.TeilSystem = pp.InternalName;
								if (pp.Product.HasInsideConstruction) {
									wrapperCool.InsideConstruction = pp.Product.PlannedInsideConstruction.Id;
									wrapperCool.InsideRValue = pp.Product.PlannedInsideConstructionRValue;
								}
								if (pp.Product.HasOutsideConstruction) {
									wrapperCool.OutsideConstruction = pp.Product.PlannedOutsideConstruction.Id;
									wrapperCool.OutsideRValue = pp.Product.PlannedOutsideConstructionRValue;
								}
								wrapperCool.Circuits = pp.Product.PlannedCircuitCount;
								wrapperCool.RzLayDistance = ep.PlannedRimLayDistance.ToString();
								wrapperCool.RzWidth = ep.PlannedRimWidth;
								wrapperCool.RzArea = ep.PlannedAreaRim;
								wrapperCool.AzLayDistance = ep.PlannedLayDistance.ToString();
								wrapperCool.AzArea = ep.PlannedAreaResidence;
								wrapperCool.ConnectionArea = ep.PlannedRemoveArea;

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

									prevWrapper = null;
									foreach (EcothermCircuit ec in ep.PlannedCircuits) {
										EcothermWrapper wrapper = new EcothermWrapper(wrapperHeat);
										wrapper.UsedAsCircuitWrapper = true;

										wrapper.AzArea = ec.AreaAz;
										wrapper.RzArea = ec.RimLength;

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
											ok = ok && Math.Round(prevWrapper.AzArea, 3) == Math.Round(wrapper.AzArea, 3);
											ok = ok && Math.Round(prevWrapper.RzArea, 3) == Math.Round(wrapper.RzArea, 3);
											ok = ok && Math.Round(prevWrapper.LengthRzAz, 3) == Math.Round(wrapper.LengthRzAz, 3);
											ok = ok && Math.Round(prevWrapper.LengthConnection, 3) == Math.Round(wrapper.LengthConnection, 3);
											ok = ok && Math.Round(prevWrapper.LengthCircuitFbh, 3) == Math.Round(wrapper.LengthCircuitFbh, 3);
											ok = ok && Math.Round(prevWrapper.LengthCircuitAll, 3) == Math.Round(wrapper.LengthCircuitAll, 3);
											ok = ok && Math.Round(prevWrapper.Wassermenge, 3) == Math.Round(wrapper.Wassermenge, 3);
											ok = ok && Math.Round(prevWrapper.DruckverlustRohr, 3) == Math.Round(wrapper.DruckverlustRohr, 3);
											ok = ok && Math.Round(prevWrapper.DruckverlustVerteiler, 3) == Math.Round(wrapper.DruckverlustVerteiler, 3);
											ok = ok && Math.Round(prevWrapper.V, 3) == Math.Round(wrapper.V, 3);

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

									prevWrapper = null;
									foreach (EcothermCircuit ec in ep.PlannedCircuits) {
										EcothermWrapper wrapper = new EcothermWrapper(wrapperCool);
										wrapper.UsedAsCircuitWrapper = true;

										wrapper.AzArea = ec.AreaAz;
										wrapper.RzArea = ec.RimLength;

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
											ok = ok && Math.Round(prevWrapper.AzArea, 3) == Math.Round(wrapper.AzArea, 3);
											ok = ok && Math.Round(prevWrapper.RzArea, 3) == Math.Round(wrapper.RzArea, 3);
											ok = ok && Math.Round(prevWrapper.LengthRzAz, 3) == Math.Round(wrapper.LengthRzAz, 3);
											ok = ok && Math.Round(prevWrapper.LengthConnection, 3) == Math.Round(wrapper.LengthConnection, 3);
											ok = ok && Math.Round(prevWrapper.LengthCircuitFbh, 3) == Math.Round(wrapper.LengthCircuitFbh, 3);
											ok = ok && Math.Round(prevWrapper.LengthCircuitAll, 3) == Math.Round(wrapper.LengthCircuitAll, 3);
											ok = ok && Math.Round(prevWrapper.Wassermenge, 3) == Math.Round(wrapper.Wassermenge, 3);
											ok = ok && Math.Round(prevWrapper.DruckverlustRohr, 3) == Math.Round(wrapper.DruckverlustRohr, 3);
											ok = ok && Math.Round(prevWrapper.DruckverlustVerteiler, 3) == Math.Round(wrapper.DruckverlustVerteiler, 3);
											ok = ok && Math.Round(prevWrapper.V, 3) == Math.Round(wrapper.V, 3);

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

			HithermWrapper wrapperOverview = null;
			HithermWrapper wrapperHeat = null;
			HithermWrapper wrapperCool = null;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						wrapperOverview = null;
						wrapperHeat = null;
						wrapperCool = null;
						if (pp.Product is HithermProduct) {
							HithermProduct hp = pp.Product as HithermProduct;
							foreach (HithermCircuit c in hp.PlannedCircuits) {
								double v, r;

								wrapperOverview = new HithermWrapper();
								wrapperOverview.HeatOrCool = "overview";
								wrapperOverview.FloorId = floor.Id;
								wrapperOverview.FloorName = floor.Name;
								wrapperOverview.RoomId = room.Id;
								wrapperOverview.RoomName = room.Name;
								wrapperOverview.TeilSystem = pp.InternalName;

								wrapperOverview.Circuit = c.NrOfCircuit + 1;

								foreach (HithermRegister register in c.Registers) {
									wrapperOverview.RegisterList.Add(register);
									if (wrapperOverview.Registers.ContainsKey(register.RegisterType)) {
										wrapperOverview.Registers[register.RegisterType] += register.RegisterCount;
									} else {
										wrapperOverview.Registers.Add(register.RegisterType, register.RegisterCount);
									}
									wrapperOverview.PipeHorizontal += register.PipeHorizontal;
									wrapperOverview.PipeVertical += register.PipeVertical;
									if (register.IsHochleistungsRegister) {
										wrapperOverview.Ra5Area += register.CoveredArea;
									} else {
										wrapperOverview.Ra10Area += register.CoveredArea;
									}
								}

								pp.Product.GetHeatFlow(out v, out r);
								wrapperOverview.RoomTemp = room.RoomHeatTemperature;
								wrapperOverview.VorlaufTemp = v;
								wrapperOverview.RuecklaufTemp = r;
								wrapperOverview.QDelta = hp.PlannedHeizlastBereinigung;
								wrapperOverview.QSoll = pp.RequestedHeatLoad - hp.PlannedHeizlastBereinigung;
								wrapperOverview.QWH = pp.PlannedHeatLoad;
								wrapperOverview.QWHSqm = pp.PlannedHeatLoad / pp.PlannedArea.Value;

								wrapperOverview.LengthConnectionVorlauf = c.PipeLengthVorlaufWithoutOtherProductTotal;
								wrapperOverview.LengthConnectionRuecklauf = c.PipeLengthRuecklaufWithoutOtherProductTotal;

								wrapperOverview.Wassermenge = c.C_DurchflussHeat;
								wrapperOverview.DruckverlustRohr = c.C_DruckverlustHeat;
								wrapperOverview.DruckverlustVerteiler = c.C_DruckverlustDistributorHeat;
								wrapperOverview.V = c.C_FlussGeschwindigkeitHeat;

								if (hp.PlannedConnection != null) {
									if (hp.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
										wrapperOverview.SubSystem = true;
										wrapperOverview.VorlaufTemp = -1;
										wrapperOverview.RuecklaufTemp = -1;
									}
								}
								if (hp.IsOtherProductConnected) {
									wrapperOverview.OtherSystemsConnected = true;
								}
								wrapperHeatList.Add(wrapperOverview);

								if (pp.RequestedHeatLoad != 0) {
									wrapperHeat = new HithermWrapper();
									wrapperHeat.HeatOrCool = EuroplanRes.LL_Report_Heizbetrieb; //"Heizen";
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
											wrapperHeat.Ra5Area += register.CoveredArea;
										} else {
											wrapperHeat.Ra10Area += register.CoveredArea;
										}
									}

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
								}
								if (project.CalculateCoolLoad && pp.RequestedCoolLoad != 0) {
									wrapperCool = new HithermWrapper();
									wrapperCool.HeatOrCool = EuroplanRes.LL_Report_Kuehlbetrieb; //"Kühlen";
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
											wrapperCool.Ra5Area += register.CoveredArea;
										} else {
											wrapperCool.Ra10Area += register.CoveredArea;
										}
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

			HithermCompactWrapper wrapperOverview = null;
			HithermCompactWrapper wrapperHeat = null;
			HithermCompactWrapper wrapperCool = null;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						wrapperOverview = null;
						wrapperHeat = null;
						wrapperCool = null;
						if (pp.Product is HithermCompactProduct) {
							HithermCompactProduct hp = pp.Product as HithermCompactProduct;
							foreach (HithermCompactCircuit c in hp.PlannedCircuits) {
								double v, r;

								wrapperOverview = new HithermCompactWrapper();
								wrapperOverview.HeatOrCool = "overview";
								wrapperOverview.FloorId = floor.Id;
								wrapperOverview.FloorName = floor.Name;
								wrapperOverview.RoomId = room.Id;
								wrapperOverview.RoomName = room.Name;
								wrapperOverview.TeilSystem = pp.InternalName;

								wrapperOverview.Circuit = c.NrOfCircuit + 1;

								foreach (HithermCompactRegister register in c.Registers) {
									wrapperOverview.RegisterList.Add(register);
									if (wrapperOverview.Registers.ContainsKey(register.RegisterType)) {
										wrapperOverview.Registers[register.RegisterType] += register.RegisterCount;
									} else {
										wrapperOverview.Registers.Add(register.RegisterType, register.RegisterCount);
									}
									wrapperOverview.PipeHorizontal += register.PipeHorizontal;
									wrapperOverview.PipeVertical += register.PipeVertical;

									wrapperOverview.Ra5Area += register.CoveredArea;
								}

								pp.Product.GetHeatFlow(out v, out r);
								wrapperOverview.RoomTemp = room.RoomHeatTemperature;
								wrapperOverview.VorlaufTemp = v;
								wrapperOverview.RuecklaufTemp = r;
								wrapperOverview.QDelta = hp.PlannedHeizlastBereinigung;
								wrapperOverview.QSoll = pp.RequestedHeatLoad - hp.PlannedHeizlastBereinigung;
								wrapperOverview.QWH = pp.PlannedHeatLoad;
								wrapperOverview.QWHSqm = pp.PlannedHeatLoad / pp.PlannedArea.Value;

								wrapperOverview.LengthConnectionVorlauf = c.PipeLengthVorlaufWithoutOtherProductTotal;
								wrapperOverview.LengthConnectionRuecklauf = c.PipeLengthRuecklaufWithoutOtherProductTotal;

								wrapperOverview.Wassermenge = c.C_DurchflussHeat;
								wrapperOverview.DruckverlustRohr = c.C_DruckverlustHeat;
								wrapperOverview.DruckverlustVerteiler = c.C_DruckverlustDistributorHeat;
								wrapperOverview.V = c.C_FlussGeschwindigkeitHeat;

								if (hp.PlannedConnection != null) {
									if (hp.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
										wrapperOverview.SubSystem = true;
										wrapperOverview.VorlaufTemp = -1;
										wrapperOverview.RuecklaufTemp = -1;
									}
								}
								if (hp.IsOtherProductConnected) {
									wrapperOverview.OtherSystemsConnected = true;
								}
								wrapperHeatList.Add(wrapperOverview);

								if (pp.RequestedHeatLoad != 0) {
									wrapperHeat = new HithermCompactWrapper();
									wrapperHeat.HeatOrCool = EuroplanRes.LL_Report_Heizbetrieb; //"Heizen";
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

										wrapperHeat.Ra5Area += register.CoveredArea;
									}

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
								}
								if (project.CalculateCoolLoad && pp.RequestedCoolLoad != 0) {
									wrapperCool = new HithermCompactWrapper();
									wrapperCool.HeatOrCool = EuroplanRes.LL_Report_Kuehlbetrieb; //"Kühlen";
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

										wrapperCool.Ra5Area += register.CoveredArea;
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

			ModulBodenWrapper wrapperOverview = null;
			ModulBodenWrapper wrapperHeat = null;
			ModulBodenWrapper wrapperCool = null;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						wrapperOverview = null;
						wrapperHeat = null;
						wrapperCool = null;
						if (pp.Product is ModulKlimaBodenProduct) {
							ModulKlimaBodenProduct mp = pp.Product as ModulKlimaBodenProduct;

							wrapperOverview = new ModulBodenWrapper();
							wrapperOverview.HeatOrCool = "overview";
							wrapperOverview.FloorId = floor.Id;
							wrapperOverview.FloorName = floor.Name;

							wrapperOverview.RoomId = room.Id;
							wrapperOverview.RoomName = room.Name;
							wrapperOverview.TeilSystem = pp.InternalName;
							if (pp.Product.HasInsideConstruction) {
								wrapperOverview.InsideConstruction = pp.Product.PlannedInsideConstruction.Id;
								wrapperOverview.InsideRValue = pp.Product.PlannedInsideConstructionRValue;
							}
							if (pp.Product.HasOutsideConstruction) {
								wrapperOverview.OutsideConstruction = pp.Product.PlannedOutsideConstruction.Id;
								wrapperOverview.OutsideRValue = pp.Product.PlannedOutsideConstructionRValue;
							}
							wrapperOverview.Circuits = pp.Product.PlannedCircuitCount;
							wrapperOverview.DichtArea = mp.RequestedModulesDichtArea;
							wrapperOverview.ModulierendArea = mp.RequestedModulesModulierendArea;
							wrapperOverview.SonstigeArea = mp.RequestedModulesSonstigeArea;
							wrapperOverview.ConnectionArea = mp.PlannedRemoveArea;

							double v, r;
							pp.Product.GetHeatFlow(out v, out r);
							wrapperOverview.RoomTemp = room.RoomHeatTemperature;
							wrapperOverview.VorlaufTemp = v;
							wrapperOverview.RuecklaufTemp = r;
							wrapperOverview.QSoll = pp.RequestedHeatLoad;
							wrapperOverview.QFBH = pp.PlannedHeatLoad;
							wrapperOverview.tFB = mp.PlannedFloorTemperatureHeat;

							wrapperOverview.Wassermenge = pp.Product.PlannedDurchflussHeat;
							wrapperOverview.DruckverlustHeizkreis = pp.Product.PlannedDeltaRhoHeat;
							wrapperOverview.DruckverlustVerteiler = pp.Product.PlannedDeltaRhoDistributorHeat;

							wrapperOverview.UnusedArea = mp.PlannedAreaUnheated;

							if (mp.PlannedConnection != null) {
								if (mp.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
									wrapperOverview.SubSystem = true;
									wrapperOverview.VorlaufTemp = -1;
									wrapperOverview.RuecklaufTemp = -1;
								}
							}
							if (mp.IsOtherProductConnected) {
								wrapperOverview.OtherSystemsConnected = true;
							}

							wrapperHeatList.Add(wrapperOverview);

							ModulBodenWrapper prevWrapper = null;
							foreach (ModulBodenCircuit mc in mp.PlannedCircuits) {
								ModulBodenWrapper wrapper = new ModulBodenWrapper(wrapperOverview);
								wrapper.UsedAsCircuitWrapper = true;

								wrapper.Circuits = mc.NrOfCircuit + 1;
								wrapper.CircuitsAsString = wrapper.Circuits.ToString();
								wrapper.TotalModules = mc.Row.List.Count;
								wrapper.LengthConnection = mc.PipeLengthVorlaufWithoutOtherProductTotal + mc.PipeLengthRuecklaufWithoutOtherProductTotal;

								wrapper.DichteModule = mc.DichteModule;
								wrapper.ModulierendeModule = mc.ModulierendeModule;
								wrapper.SonstigeModule = mc.SonstigeModule;
								wrapper.SonstigeVerbindeleitung = mc.SonstigeVerbindeleitung;

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
									ok = ok && prevWrapper.DichteModule == wrapper.DichteModule;
									ok = ok && prevWrapper.ModulierendeModule == wrapper.ModulierendeModule;
									ok = ok && prevWrapper.SonstigeModule == wrapper.SonstigeModule;
									ok = ok && prevWrapper.SonstigeVerbindeleitung == wrapper.SonstigeVerbindeleitung;
									ok = ok && Math.Round(prevWrapper.LengthConnection, 1) == Math.Round(wrapper.LengthConnection, 1);
									ok = ok && Math.Round(prevWrapper.Wassermenge, 1) == Math.Round(wrapper.Wassermenge, 1);
									ok = ok && Math.Round(prevWrapper.DruckverlustHeizkreis, 1) == Math.Round(wrapper.DruckverlustHeizkreis, 1);
									ok = ok && Math.Round(prevWrapper.DruckverlustVerteiler, 1) == Math.Round(wrapper.DruckverlustVerteiler, 1);
									ok = ok && Math.Round(prevWrapper.V, 1) == Math.Round(wrapper.V, 1);

									if (ok) {
										prevWrapper.CircuitsAsString = prevWrapper.Circuits.ToString() + "-" + wrapper.Circuits.ToString();
									} else {
										prevWrapper = wrapper;
										wrapperHeatList.Add(wrapper);
									}
								}
							}

							if (wrapperHeat == null && pp.RequestedHeatLoad != 0) {
								wrapperHeat = new ModulBodenWrapper();
								wrapperHeat.HeatOrCool = EuroplanRes.LL_Report_Heizbetrieb; //"Heizen";
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
							if (project.CalculateCoolLoad && wrapperCool == null && pp.RequestedCoolLoad != 0) {
								wrapperCool = new ModulBodenWrapper();
								wrapperCool.HeatOrCool = EuroplanRes.LL_Report_Kuehlbetrieb; //"Kühlen";
								wrapperCool.FloorId = floor.Id;
								wrapperCool.FloorName = floor.Name;

								wrapperCool.RoomId = room.Id;
								wrapperCool.RoomName = room.Name;
								wrapperCool.TeilSystem = pp.InternalName;
								if (pp.Product.HasInsideConstruction) {
									wrapperCool.InsideConstruction = pp.Product.PlannedInsideConstruction.Id;
									wrapperCool.InsideRValue = pp.Product.PlannedInsideConstructionRValue;
								}
								if (pp.Product.HasOutsideConstruction) {
									wrapperCool.OutsideConstruction = pp.Product.PlannedOutsideConstruction.Id;
									wrapperCool.OutsideRValue = pp.Product.PlannedOutsideConstructionRValue;
								}
								wrapperCool.Circuits = pp.Product.PlannedCircuitCount;
								wrapperCool.DichtArea = mp.RequestedModulesDichtArea;
								wrapperCool.ModulierendArea = mp.RequestedModulesModulierendArea;
								wrapperCool.SonstigeArea = mp.RequestedModulesSonstigeArea;
								wrapperCool.ConnectionArea = mp.PlannedRemoveArea;

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

									prevWrapper = null;
									foreach (ModulBodenCircuit mc in mp.PlannedCircuits) {
										ModulBodenWrapper wrapper = new ModulBodenWrapper(wrapperHeat);
										wrapper.UsedAsCircuitWrapper = true;

										wrapper.Circuits = mc.NrOfCircuit + 1;
										wrapper.CircuitsAsString = wrapper.Circuits.ToString();
										wrapper.TotalModules = mc.Row.List.Count;
										wrapper.LengthConnection = mc.PipeLengthVorlaufWithoutOtherProductTotal + mc.PipeLengthRuecklaufWithoutOtherProductTotal;

										wrapper.DichteModule = mc.DichteModule;
										wrapper.ModulierendeModule = mc.ModulierendeModule;
										wrapper.SonstigeModule = mc.SonstigeModule;
										wrapper.SonstigeVerbindeleitung = mc.SonstigeVerbindeleitung;

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
											ok = ok && prevWrapper.DichteModule == wrapper.DichteModule;
											ok = ok && prevWrapper.ModulierendeModule == wrapper.ModulierendeModule;
											ok = ok && prevWrapper.SonstigeModule == wrapper.SonstigeModule;
											ok = ok && prevWrapper.SonstigeVerbindeleitung == wrapper.SonstigeVerbindeleitung;
											ok = ok && Math.Round(prevWrapper.LengthConnection, 1) == Math.Round(wrapper.LengthConnection, 1);
											ok = ok && Math.Round(prevWrapper.Wassermenge, 1) == Math.Round(wrapper.Wassermenge, 1);
											ok = ok && Math.Round(prevWrapper.DruckverlustHeizkreis, 1) == Math.Round(wrapper.DruckverlustHeizkreis, 1);
											ok = ok && Math.Round(prevWrapper.DruckverlustVerteiler, 1) == Math.Round(wrapper.DruckverlustVerteiler, 1);
											ok = ok && Math.Round(prevWrapper.V, 1) == Math.Round(wrapper.V, 1);

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

									prevWrapper = null;
									foreach (ModulBodenCircuit mc in mp.PlannedCircuits) {
										ModulBodenWrapper wrapper = new ModulBodenWrapper(wrapperCool);
										wrapper.UsedAsCircuitWrapper = true;

										wrapper.Circuits = mc.NrOfCircuit + 1;
										wrapper.CircuitsAsString = wrapper.Circuits.ToString();
										wrapper.TotalModules = mc.Row.List.Count;
										wrapper.LengthConnection = mc.PipeLengthVorlaufWithoutOtherProductTotal + mc.PipeLengthRuecklaufWithoutOtherProductTotal;
										
										wrapper.DichteModule = mc.DichteModule;
										wrapper.ModulierendeModule = mc.ModulierendeModule;
										wrapper.SonstigeModule = mc.SonstigeModule;
										wrapper.SonstigeVerbindeleitung = mc.SonstigeVerbindeleitung;

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
											ok = ok && prevWrapper.DichteModule == wrapper.DichteModule;
											ok = ok && prevWrapper.ModulierendeModule == wrapper.ModulierendeModule;
											ok = ok && prevWrapper.SonstigeModule == wrapper.SonstigeModule;
											ok = ok && prevWrapper.SonstigeVerbindeleitung == wrapper.SonstigeVerbindeleitung;
											ok = ok && Math.Round(prevWrapper.LengthConnection, 1) == Math.Round(wrapper.LengthConnection, 1);
											ok = ok && Math.Round(prevWrapper.Wassermenge, 1) == Math.Round(wrapper.Wassermenge, 1);
											ok = ok && Math.Round(prevWrapper.DruckverlustHeizkreis, 1) == Math.Round(wrapper.DruckverlustHeizkreis, 1);
											ok = ok && Math.Round(prevWrapper.DruckverlustVerteiler, 1) == Math.Round(wrapper.DruckverlustVerteiler, 1);
											ok = ok && Math.Round(prevWrapper.V, 1) == Math.Round(wrapper.V, 1);

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

		public List<ModulDeckeWrapper> GetModulDeckeWrapper() {
			List<ModulDeckeWrapper> wrapperHeatList = new List<ModulDeckeWrapper>();
			List<ModulDeckeWrapper> wrapperCoolList = new List<ModulDeckeWrapper>();

			ModulDeckeWrapper wrapperOverview = null;
			ModulDeckeWrapper wrapperHeat = null;
			ModulDeckeWrapper wrapperCool = null;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						wrapperOverview = null;
						wrapperHeat = null;
						wrapperCool = null;
						if (pp.Product is ModulKlimaDeckeProduct) {
							ModulKlimaDeckeProduct mp = pp.Product as ModulKlimaDeckeProduct;

							wrapperOverview = new ModulDeckeWrapper();
							wrapperOverview.HeatOrCool = "overview";
							wrapperOverview.FloorId = floor.Id;
							wrapperOverview.FloorName = floor.Name;

							wrapperOverview.RoomId = room.Id;
							wrapperOverview.RoomName = room.Name;
							wrapperOverview.TeilSystem = pp.InternalName;
							if (pp.Product.HasInsideConstruction) {
								wrapperOverview.InsideConstruction = pp.Product.PlannedInsideConstruction.Id;
								wrapperOverview.InsideRValue = pp.Product.PlannedInsideConstructionRValue;
							}
							if (pp.Product.HasOutsideConstruction) {
								wrapperOverview.OutsideConstruction = pp.Product.PlannedOutsideConstruction.Id;
								wrapperOverview.OutsideRValue = pp.Product.PlannedOutsideConstructionRValue;
							}
							wrapperOverview.Circuits = pp.Product.PlannedCircuitCount;

							foreach (ModulDeckeCircuit mc in mp.PlannedCircuits) {
								foreach (ModulDeckeSubArea sa in mc.SubAreas) {
									foreach (KlimaFlaechenList ml in sa.Rows) {
										foreach (KlimaFlaechenModul modul in ml.List) {
											if (!wrapperOverview.Modules.ContainsKey(modul.ModulType)) {
												wrapperOverview.Modules.Add(modul.ModulType, 1);
											} else {
												wrapperOverview.Modules[modul.ModulType]++;
											}
										}
										wrapperOverview.SonstigeVerbindeleitung += ml.LengthVerbindeleitungen;
									}
								}
							}

							wrapperOverview.TotalArea = mp.CoveredArea;
							wrapperOverview.ConnectionArea = mp.PlannedRemoveArea;

							double v, r;
							pp.Product.GetHeatFlow(out v, out r);
							wrapperOverview.RoomTemp = room.RoomHeatTemperature;
							wrapperOverview.VorlaufTemp = v;
							wrapperOverview.RuecklaufTemp = r;
							wrapperOverview.QSoll = pp.RequestedHeatLoad;
							wrapperOverview.QFBH = pp.PlannedHeatLoad;
							//wrapperHeat.tFB = mp.PlannedFloorTemperatureHeat;

							wrapperOverview.Wassermenge = pp.Product.PlannedDurchflussHeat;
							wrapperOverview.DruckverlustHeizkreis = pp.Product.PlannedDeltaRhoHeat;
							wrapperOverview.DruckverlustVerteiler = pp.Product.PlannedDeltaRhoDistributorHeat;

							wrapperOverview.UnusedArea = mp.PlannedAreaUnheated;

							if (mp.PlannedConnection != null) {
								if (mp.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
									wrapperOverview.SubSystem = true;
									wrapperOverview.VorlaufTemp = -1;
									wrapperOverview.RuecklaufTemp = -1;
								}
							}
							if (mp.IsOtherProductConnected) {
								wrapperOverview.OtherSystemsConnected = true;
							}

							wrapperHeatList.Add(wrapperOverview);

							ModulDeckeWrapper prevWrapper = null;
							foreach (ModulDeckeCircuit mc in mp.PlannedCircuits) {
								ModulDeckeWrapper wrapper = new ModulDeckeWrapper(wrapperOverview);
								wrapper.UsedAsCircuitWrapper = true;

								wrapper.Circuits = mc.NrOfCircuit + 1;
								wrapper.CircuitsAsString = wrapper.Circuits.ToString();

								foreach (ModulDeckeSubArea sa in mc.SubAreas) {
									foreach (KlimaFlaechenList ml in sa.Rows) {
										foreach (KlimaFlaechenModul modul in ml.List) {
											if (!wrapper.Modules.ContainsKey(modul.ModulType)) {
												wrapper.Modules.Add(modul.ModulType, 1);
											} else {
												wrapper.Modules[modul.ModulType]++;
											}
										}
										wrapper.SonstigeVerbindeleitung += ml.LengthVerbindeleitungen;
									}
								}

								wrapper.LengthConnection = mc.PipeLengthVorlaufWithoutOtherProductTotal + mc.PipeLengthRuecklaufWithoutOtherProductTotal;

								wrapper.TotalArea = mc.CoveredArea;
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
									foreach (KlimaFlaechenModul.ModulTypeEnum item in wrapper.Modules.Keys) {
										if (!(prevWrapper.Modules.ContainsKey(item) && prevWrapper.Modules[item] == wrapper.Modules[item])) {
											ok = false;
										}
									}
									foreach (KlimaFlaechenModul.ModulTypeEnum item in prevWrapper.Modules.Keys) {
										if (!(wrapper.Modules.ContainsKey(item) && prevWrapper.Modules[item] == wrapper.Modules[item])) {
											ok = false;
										}
									}
									ok = ok && prevWrapper.SonstigeVerbindeleitung == wrapper.SonstigeVerbindeleitung;
									ok = ok && Math.Round(prevWrapper.LengthConnection, 1) == Math.Round(wrapper.LengthConnection, 1);
									ok = ok && Math.Round(prevWrapper.Wassermenge, 1) == Math.Round(wrapper.Wassermenge, 1);
									ok = ok && Math.Round(prevWrapper.DruckverlustHeizkreis, 1) == Math.Round(wrapper.DruckverlustHeizkreis, 1);
									ok = ok && Math.Round(prevWrapper.DruckverlustVerteiler, 1) == Math.Round(wrapper.DruckverlustVerteiler, 1);
									ok = ok && Math.Round(prevWrapper.V, 1) == Math.Round(wrapper.V, 1);

									if (ok) {
										prevWrapper.CircuitsAsString = prevWrapper.Circuits.ToString() + "-" + wrapper.Circuits.ToString();
									} else {
										prevWrapper = wrapper;
										wrapperHeatList.Add(wrapper);
									}
								}
							}

							if (wrapperHeat == null && pp.RequestedHeatLoad != 0) {
								wrapperHeat = new ModulDeckeWrapper();
								wrapperHeat.HeatOrCool = EuroplanRes.LL_Report_Heizbetrieb; //"Heizen";
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

								foreach (ModulDeckeCircuit mc in mp.PlannedCircuits) {
									foreach (ModulDeckeSubArea sa in mc.SubAreas) {
										foreach (KlimaFlaechenList ml in sa.Rows) {
											foreach (KlimaFlaechenModul modul in ml.List) {
												if (!wrapperHeat.Modules.ContainsKey(modul.ModulType)) {
													wrapperHeat.Modules.Add(modul.ModulType, 1);
												} else {
													wrapperHeat.Modules[modul.ModulType]++;
												}
											}
											wrapperHeat.SonstigeVerbindeleitung += ml.LengthVerbindeleitungen;
										}
									}
								}

								wrapperHeat.TotalArea = mp.CoveredArea;
								wrapperHeat.ConnectionArea = mp.PlannedRemoveArea;

								pp.Product.GetHeatFlow(out v, out r);
								wrapperHeat.RoomTemp = room.RoomHeatTemperature;
								wrapperHeat.VorlaufTemp = v;
								wrapperHeat.RuecklaufTemp = r;
								wrapperHeat.QSoll = pp.RequestedHeatLoad;
								wrapperHeat.QFBH = pp.PlannedHeatLoad;
								//wrapperHeat.tFB = mp.PlannedFloorTemperatureHeat;

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
							if (project.CalculateCoolLoad && wrapperCool == null && pp.RequestedCoolLoad != 0) {
								wrapperCool = new ModulDeckeWrapper();
								wrapperCool.HeatOrCool = EuroplanRes.LL_Report_Kuehlbetrieb; //"Kühlen";
								wrapperCool.FloorId = floor.Id;
								wrapperCool.FloorName = floor.Name;

								wrapperCool.RoomId = room.Id;
								wrapperCool.RoomName = room.Name;
								wrapperCool.TeilSystem = pp.InternalName;
								if (pp.Product.HasInsideConstruction) {
									wrapperCool.InsideConstruction = pp.Product.PlannedInsideConstruction.Id;
									wrapperCool.InsideRValue = pp.Product.PlannedInsideConstructionRValue;
								}
								if (pp.Product.HasOutsideConstruction) {
									wrapperCool.OutsideConstruction = pp.Product.PlannedOutsideConstruction.Id;
									wrapperCool.OutsideRValue = pp.Product.PlannedOutsideConstructionRValue;
								}
								wrapperCool.Circuits = pp.Product.PlannedCircuitCount;

								foreach (ModulDeckeCircuit mc in mp.PlannedCircuits) {
									foreach (ModulDeckeSubArea sa in mc.SubAreas) {
										foreach (KlimaFlaechenList ml in sa.Rows) {
											foreach (KlimaFlaechenModul modul in ml.List) {
												if (!wrapperCool.Modules.ContainsKey(modul.ModulType)) {
													wrapperCool.Modules.Add(modul.ModulType, 1);
												} else {
													wrapperCool.Modules[modul.ModulType]++;
												}
											}
											wrapperCool.SonstigeVerbindeleitung += ml.LengthVerbindeleitungen;
										}
									}
								}

								wrapperCool.TotalArea = mp.CoveredArea;
								wrapperCool.ConnectionArea = mp.PlannedRemoveArea;

								pp.Product.GetCoolFlow(out v, out r);
								wrapperCool.RoomTemp = room.RoomCoolTemperature;
								wrapperCool.VorlaufTemp = v;
								wrapperCool.RuecklaufTemp = r;
								wrapperCool.QSoll = pp.RequestedCoolLoad;
								wrapperCool.QFBH = pp.PlannedCoolLoad;
								//wrapperCool.tFB = mp.PlannedFloorTemperatureCool;

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

									prevWrapper = null;
									foreach (ModulDeckeCircuit mc in mp.PlannedCircuits) {
										ModulDeckeWrapper wrapper = new ModulDeckeWrapper(wrapperHeat);
										wrapper.UsedAsCircuitWrapper = true;

										wrapper.Circuits = mc.NrOfCircuit + 1;
										wrapper.CircuitsAsString = wrapper.Circuits.ToString();

										foreach (ModulDeckeSubArea sa in mc.SubAreas) {
											foreach (KlimaFlaechenList ml in sa.Rows) {
												foreach (KlimaFlaechenModul modul in ml.List) {
													if (!wrapper.Modules.ContainsKey(modul.ModulType)) {
														wrapper.Modules.Add(modul.ModulType, 1);
													} else {
														wrapper.Modules[modul.ModulType]++;
													}
												}
												wrapper.SonstigeVerbindeleitung += ml.LengthVerbindeleitungen;
											}
										}

										wrapper.LengthConnection = mc.PipeLengthVorlaufWithoutOtherProductTotal + mc.PipeLengthRuecklaufWithoutOtherProductTotal;

										wrapper.TotalArea = mc.CoveredArea;
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
											foreach (KlimaFlaechenModul.ModulTypeEnum item in wrapper.Modules.Keys) {
												if (!(prevWrapper.Modules.ContainsKey(item) && prevWrapper.Modules[item] == wrapper.Modules[item])) {
													ok = false;
												}
											}
											foreach (KlimaFlaechenModul.ModulTypeEnum item in prevWrapper.Modules.Keys) {
												if (!(wrapper.Modules.ContainsKey(item) && prevWrapper.Modules[item] == wrapper.Modules[item])) {
													ok = false;
												}
											}
											ok = ok && prevWrapper.SonstigeVerbindeleitung == wrapper.SonstigeVerbindeleitung;
											ok = ok && Math.Round(prevWrapper.LengthConnection, 1) == Math.Round(wrapper.LengthConnection, 1);
											ok = ok && Math.Round(prevWrapper.Wassermenge, 1) == Math.Round(wrapper.Wassermenge, 1);
											ok = ok && Math.Round(prevWrapper.DruckverlustHeizkreis, 1) == Math.Round(wrapper.DruckverlustHeizkreis, 1);
											ok = ok && Math.Round(prevWrapper.DruckverlustVerteiler, 1) == Math.Round(wrapper.DruckverlustVerteiler, 1);
											ok = ok && Math.Round(prevWrapper.V, 1) == Math.Round(wrapper.V, 1);

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

									prevWrapper = null;
									foreach (ModulDeckeCircuit mc in mp.PlannedCircuits) {
										ModulDeckeWrapper wrapper = new ModulDeckeWrapper(wrapperCool);
										wrapper.UsedAsCircuitWrapper = true;

										wrapper.Circuits = mc.NrOfCircuit + 1;
										wrapper.CircuitsAsString = wrapper.Circuits.ToString();

										foreach (ModulDeckeSubArea sa in mc.SubAreas) {
											foreach (KlimaFlaechenList ml in sa.Rows) {
												foreach (KlimaFlaechenModul modul in ml.List) {
													if (!wrapper.Modules.ContainsKey(modul.ModulType)) {
														wrapper.Modules.Add(modul.ModulType, 1);
													} else {
														wrapper.Modules[modul.ModulType]++;
													}
												}
												wrapper.SonstigeVerbindeleitung += ml.LengthVerbindeleitungen;
											}
										}

										wrapper.LengthConnection = mc.PipeLengthVorlaufWithoutOtherProductTotal + mc.PipeLengthRuecklaufWithoutOtherProductTotal;

										wrapper.TotalArea = mc.CoveredArea;
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
											foreach (KlimaFlaechenModul.ModulTypeEnum item in wrapper.Modules.Keys) {
												if (!(prevWrapper.Modules.ContainsKey(item) && prevWrapper.Modules[item] == wrapper.Modules[item])) {
													ok = false;
												}
											}
											foreach (KlimaFlaechenModul.ModulTypeEnum item in prevWrapper.Modules.Keys) {
												if (!(wrapper.Modules.ContainsKey(item) && prevWrapper.Modules[item] == wrapper.Modules[item])) {
													ok = false;
												}
											}
											ok = ok && prevWrapper.SonstigeVerbindeleitung == wrapper.SonstigeVerbindeleitung;
											ok = ok && Math.Round(prevWrapper.LengthConnection, 1) == Math.Round(wrapper.LengthConnection, 1);
											ok = ok && Math.Round(prevWrapper.Wassermenge, 1) == Math.Round(wrapper.Wassermenge, 1);
											ok = ok && Math.Round(prevWrapper.DruckverlustHeizkreis, 1) == Math.Round(wrapper.DruckverlustHeizkreis, 1);
											ok = ok && Math.Round(prevWrapper.DruckverlustVerteiler, 1) == Math.Round(wrapper.DruckverlustVerteiler, 1);
											ok = ok && Math.Round(prevWrapper.V, 1) == Math.Round(wrapper.V, 1);

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

			bool isProductPlanned = false;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						if (pp.Product is EurovalProduct) {
							isProductPlanned = true;
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

			if (isProductPlanned) {
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

			bool isProductPlanned = false;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						if (pp.Product is EcothermProduct) {
							isProductPlanned = true;
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

			if (isProductPlanned) {
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

			bool isProductPlanned = false;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						if (pp.Product is HithermProduct) {
							isProductPlanned = true;
							p = pp.Product as HithermProduct;
							foreach (HithermCircuit c in p.PlannedCircuits) {
								foreach (HithermRegister register in c.Registers) {
									if (register.IsHochleistungsRegister) {
										ra5Area += register.CoveredArea;
									} else {
										ra10Area += register.CoveredArea;
									}
									rohr2417Length = register.PipeHorizontal + register.PipeVertical;
								}
								rohr21Length += c.PipeLengthVorlaufWithoutOtherProductTotal + c.PipeLengthRuecklaufWithoutOtherProductTotal;
							}
						}
					}
				}
			}

			if (isProductPlanned) {

				HithermOverviewWrapper wrapper = new HithermOverviewWrapper();
				wrapper.Text = EuroplanRes.ProjectReport_FlaecheRa5; //"Fläche mit Rohrabstand RA5";
				wrapper.Amount = ra5Area;
				wrapper.Unit = EuroplanRes.Unit_Quadratmeter; //"m²";
				wrapperList.Add(wrapper);

				wrapper = new HithermOverviewWrapper();
				wrapper.Text = EuroplanRes.ProjectReport_FlaecheRa10; //"Fläche mit Rohrabstand RA10";
				wrapper.Amount = ra10Area;
				wrapper.Unit = EuroplanRes.Unit_Quadratmeter; //"m²";
				wrapperList.Add(wrapper);

				wrapper = new HithermOverviewWrapper();
				wrapper.Text = EuroplanRes.ProjectReport_Rundrohr; //"Rundrohr 21";
				wrapper.Amount = rohr21Length;
				wrapper.Unit = EuroplanRes.Unit_Meter; //"m";
				wrapperList.Add(wrapper);

				wrapper = new HithermOverviewWrapper();
				wrapper.Text = EuroplanRes.ProjectReport_HithermKlimawand; //"Hitherm Klimawand 24/17";
				wrapper.Amount = rohr2417Length;
				wrapper.Unit = EuroplanRes.Unit_Meter; //"m";
				wrapperList.Add(wrapper);

			}

			return wrapperList;
		}

		public List<HithermCompactOverviewWrapper> GetHithermCompactOverviewWrapper() {
			List<HithermCompactOverviewWrapper> wrapperList = new List<HithermCompactOverviewWrapper>();

			HithermCompactProduct p = null;
			double registerArea = 0;
			double rohr21Length = 0;
			double rohr2417Length = 0;

			bool isProductPlanned = false;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						if (pp.Product is HithermCompactProduct) {
							isProductPlanned = true;
							p = pp.Product as HithermCompactProduct;
							foreach (HithermCompactCircuit c in p.PlannedCircuits) {
								foreach (HithermCompactRegister register in c.Registers) {
									registerArea += register.CoveredArea;
									rohr2417Length = register.PipeHorizontal + register.PipeVertical;
								}
								rohr21Length += c.PipeLengthVorlaufWithoutOtherProductTotal + c.PipeLengthRuecklaufWithoutOtherProductTotal;
							}
						}
					}
				}
			}

			if (isProductPlanned) {

				HithermCompactOverviewWrapper wrapper = new HithermCompactOverviewWrapper();
				wrapper.Text = EuroplanRes.ProjectReport_HeizflaecheGesamt; //"Heizfläche gesamt";
				wrapper.Amount = registerArea;
				wrapper.Unit = EuroplanRes.Unit_Quadratmeter; //"m²";
				wrapperList.Add(wrapper);

				wrapper = new HithermCompactOverviewWrapper();
				wrapper.Text = EuroplanRes.ProjectReport_Rundrohr; //"Rundrohr 21";
				wrapper.Amount = rohr21Length;
				wrapper.Unit = EuroplanRes.Unit_Meter; //"m";
				wrapperList.Add(wrapper);

				wrapper = new HithermCompactOverviewWrapper();
				wrapper.Text = EuroplanRes.ProjectReport_HithermKlimawand; //"Hitherm Klimawand 24/17";
				wrapper.Amount = rohr2417Length;
				wrapper.Unit = EuroplanRes.Unit_Meter; //"m";
				wrapperList.Add(wrapper);

			}

			return wrapperList;
		}

		public List<ModulBodenOverviewWrapper> GetModulBodenOverviewWrapper() {
			List<ModulBodenOverviewWrapper> wrapperList = new List<ModulBodenOverviewWrapper>();

			ModulKlimaBodenProduct p = null;
			double modulBodenArea = 0;
			double rohr21Length = 0;

			bool isProductPlanned = false;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						if (pp.Product is ModulKlimaBodenProduct) {
							isProductPlanned = true;
							p = pp.Product as ModulKlimaBodenProduct;
							foreach (ModulBodenCircuit c in p.PlannedCircuits) {
								foreach (KlimaFlaechenModul register in c.Row.List) {
									modulBodenArea += register.GetCoveredArea(true);
								}
								rohr21Length += c.Row.LengthVerbindeleitungen;
								rohr21Length += c.PipeLengthVorlaufWithoutOtherProductTotal + c.PipeLengthRuecklaufWithoutOtherProductTotal;
							}
						}
					}
				}
			}

			if (isProductPlanned) {

				ModulBodenOverviewWrapper wrapper = new ModulBodenOverviewWrapper();
				wrapper.Text = EuroplanRes.ProjectReport_FlaecheKlimaboden; //"Fläche mit Modul Klimaboden";
				wrapper.Amount = modulBodenArea;
				wrapper.Unit = EuroplanRes.Unit_Quadratmeter; //"m²";
				wrapperList.Add(wrapper);

				wrapper = new ModulBodenOverviewWrapper();
				wrapper.Text = EuroplanRes.ProjectReport_Rundrohr; //"Rundrohr 21";
				wrapper.Amount = rohr21Length;
				wrapper.Unit = EuroplanRes.Unit_Meter; //"m";
				wrapperList.Add(wrapper);

			}

			return wrapperList;
		}

		public List<ModulDeckeOverviewWrapper> GetModulDeckeOverviewWrapper() {
			List<ModulDeckeOverviewWrapper> wrapperList = new List<ModulDeckeOverviewWrapper>();

			ModulKlimaDeckeProduct p = null;
			Dictionary<KlimaFlaechenModul.ModulTypeEnum, double> modulAreas = new Dictionary<KlimaFlaechenModul.ModulTypeEnum, double>();
			double rohr21Length = 0;

			bool isProductPlanned = false;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						if (pp.Product is ModulKlimaDeckeProduct) {
							isProductPlanned = true;
							p = pp.Product as ModulKlimaDeckeProduct;
							foreach (ModulDeckeCircuit c in p.PlannedCircuits) {
								foreach (ModulDeckeSubArea a in c.SubAreas) {
									foreach (KlimaFlaechenList l in a.Rows) {
										foreach (KlimaFlaechenModul register in l.List) {
											if (!modulAreas.ContainsKey(register.ModulType)) {
												modulAreas.Add(register.ModulType, register.GetCoveredArea(false));
											} else {
												modulAreas[register.ModulType] += register.GetCoveredArea(false);
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

			if (isProductPlanned) {

				foreach (KlimaFlaechenModul.ModulTypeEnum item in Enum.GetValues(typeof(KlimaFlaechenModul.ModulTypeEnum))) {
					wrapper = new ModulDeckeOverviewWrapper();
					string modulArea = EuroplanRes.ProjectReport_FlaecheMitModul;
					modulArea = modulArea.Replace("%MODUL%", new KlimaFlaechenModul.ModulTypeEnumConverter().ConvertToString(item));
					wrapper.Text = modulArea;
					if (modulAreas.ContainsKey(item)) {
						wrapper.Amount = modulAreas[item];
					}
					wrapper.Unit = EuroplanRes.Unit_Quadratmeter; //"m²";
					wrapperList.Add(wrapper);
				}

				wrapper = new ModulDeckeOverviewWrapper();
				wrapper.Text = EuroplanRes.ProjectReport_Rundrohr; //"Rundrohr 21";
				wrapper.Amount = rohr21Length;
				wrapper.Unit = EuroplanRes.Unit_Meter; //"m";
				wrapperList.Add(wrapper);

			}

			return wrapperList;
		}

		public List<VerlegedatenCircuitWrapper> GetVerlegedatenCircuitWrapper() {
			List<VerlegedatenCircuitWrapper> wrapperList = new List<VerlegedatenCircuitWrapper>();
			VerlegedatenCircuitWrapper wrapper = null;
			Dictionary<string, int> circuitCount = new Dictionary<string, int>();

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
								wrapper.Durchfluss = Math.Max(c.C_DurchflussHeat, c.C_DurchflussCool) / 60;
								
								foreach (ConnectionPipe pipe in pp.Product.PlannedConnectionPipes) {
									if (pipe.ConnectionThrough != null && pipe.ConnectionThrough.Product.PlannedProductIsConnection && (!pipe.OnlyFirst || c.NrOfCircuit == 0)) {
										if (pipe.Verlegeart != ConnectionPipe.VerlegeartEnum.VA_UNTER_ESTRICH && pipe.Insulation != ConnectionPipe.InsulationEnum.IN_NONE) {
											string connStr = EuroplanRes.ProjectReport_AnbindungDurchRaumGedaemmt;
											connStr = connStr.Replace("%RAUMID%", pipe.ConnectionThrough.Product.AssociatedRoom.Id);
											connStr = connStr.Replace("%RAUMNAME%", pipe.ConnectionThrough.Product.AssociatedRoom.Name);
											connStr = connStr.Replace("%ROHRTYP%", new ConnectionPipe.PipeTypeEnumConverter().ConvertToString(pipe.PipeType));
											connStr = connStr.Replace("%VERLEGEART%", new ConnectionPipe.VerlegeartEnumConverter().ConvertToString(pipe.Verlegeart));
											connStr = connStr.Replace("%DAEMMUNG%", new ConnectionPipe.InsulationEnumConverter().ConvertToString(pipe.Insulation));
											wrapper.Name += connStr;
										} else {
											string connStr = EuroplanRes.ProjectReport_AnbindungDurchRaum;
											connStr = connStr.Replace("%RAUMID%", pipe.ConnectionThrough.Product.AssociatedRoom.Id);
											connStr = connStr.Replace("%RAUMNAME%", pipe.ConnectionThrough.Product.AssociatedRoom.Name);
											connStr = connStr.Replace("%ROHRTYP%", new ConnectionPipe.PipeTypeEnumConverter().ConvertToString(pipe.PipeType));
											connStr = connStr.Replace("%VERLEGEART%", new ConnectionPipe.VerlegeartEnumConverter().ConvertToString(pipe.Verlegeart));
											wrapper.Name += connStr;
										}
										wrapper.Name += "\n";
										wrapper.Area += Math.Round(pipe.Vorlauf, 1) + EuroplanRes.Unit_Meter + "\n"; //"m\n";
									}
								}

								if (floor != connection.Distributor.AssociatedFloor) {
									if (pp.Product.PlannedCircuits.Count > 1) {
										string circuit = EuroplanRes.ProjectReport_SystemHeizkreisGeschoss;
										circuit = circuit.Replace("%SYSTEM%", pp.Product.FullName);
										circuit = circuit.Replace("%GESCHOSS%", floor.Name);
										circuit = circuit.Replace("%RAUMID%", pp.Product.AssociatedRoom.Id);
										circuit = circuit.Replace("%RAUMNAME%", pp.Product.AssociatedRoom.Name);
										circuit = circuit.Replace("%HK%", (c.NrOfCircuit + 1).ToString());
										wrapper.Name += circuit;
									} else {
										string circuit = EuroplanRes.ProjectReport_SystemGeschoss;
										circuit = circuit.Replace("%SYSTEM%", pp.Product.FullName);
										circuit = circuit.Replace("%GESCHOSS%", floor.Name);
										circuit = circuit.Replace("%RAUMID%", pp.Product.AssociatedRoom.Id);
										circuit = circuit.Replace("%RAUMNAME%", pp.Product.AssociatedRoom.Name);
										wrapper.Name += circuit;
									}
								} else {
									if (pp.Product.PlannedCircuits.Count > 1) {
										string circuit = EuroplanRes.ProjectReport_SystemHeizkreis;
										circuit = circuit.Replace("%SYSTEM%", pp.Product.FullName);
										circuit = circuit.Replace("%RAUMID%", pp.Product.AssociatedRoom.Id);
										circuit = circuit.Replace("%RAUMNAME%", pp.Product.AssociatedRoom.Name);
										circuit = circuit.Replace("%HK%", (c.NrOfCircuit + 1).ToString());
										wrapper.Name += circuit;
									} else {
										string circuit = EuroplanRes.ProjectReport_System;
										circuit = circuit.Replace("%SYSTEM%", pp.Product.FullName);
										circuit = circuit.Replace("%RAUMID%", pp.Product.AssociatedRoom.Id);
										circuit = circuit.Replace("%RAUMNAME%", pp.Product.AssociatedRoom.Name);
										wrapper.Name += circuit;
									}
								}

								wrapper.Area += Math.Round(c.CircuitArea, 1) + EuroplanRes.Unit_Quadratmeter; //"m²";
								if (!circuitCount.ContainsKey(connection.Distributor.Id)) {
									circuitCount.Add(connection.Distributor.Id, 1);							
								} 
								wrapper.CircuitNumber = circuitCount[connection.Distributor.Id]++;	
								if (pp.Product.ConnectedCircuits.ContainsKey(c.NrOfCircuit)) {
									Circuit.CircuitConnection con = pp.Product.ConnectedCircuits[c.NrOfCircuit];
									Product otherProduct = con.OtherProduct;
									if (otherProduct.AssociatedRoom.AssociatedFloor != connection.Distributor.AssociatedFloor) {
										if (pp.Product.PlannedCircuits.Count > 1) {
											string circuit = EuroplanRes.ProjectReport_SystemHeizkreisGeschoss;
											circuit = circuit.Replace("%SYSTEM%", otherProduct.FullName);
											circuit = circuit.Replace("%GESCHOSS%", otherProduct.AssociatedRoom.AssociatedFloor.Name);
											circuit = circuit.Replace("%RAUMID%", otherProduct.AssociatedRoom.Id);
											circuit = circuit.Replace("%RAUMNAME%", otherProduct.AssociatedRoom.Name);
											circuit = circuit.Replace("%HK%", (c.NrOfCircuit + 1).ToString());
											wrapper.Name += "\n" + circuit;
										} else {
											string circuit = EuroplanRes.ProjectReport_SystemGeschoss;
											circuit = circuit.Replace("%SYSTEM%", otherProduct.FullName);
											circuit = circuit.Replace("%GESCHOSS%", otherProduct.AssociatedRoom.AssociatedFloor.Name);
											circuit = circuit.Replace("%RAUMID%", otherProduct.AssociatedRoom.Id);
											circuit = circuit.Replace("%RAUMNAME%", otherProduct.AssociatedRoom.Name);
											wrapper.Name += "\n" + circuit;
										}
									} else {
										if (pp.Product.PlannedCircuits.Count > 1) {
											string circuit = EuroplanRes.ProjectReport_SystemHeizkreis;
											circuit = circuit.Replace("%SYSTEM%", otherProduct.FullName);
											circuit = circuit.Replace("%RAUMID%", otherProduct.AssociatedRoom.Id);
											circuit = circuit.Replace("%RAUMNAME%", otherProduct.AssociatedRoom.Name);
											circuit = circuit.Replace("%HK%", (c.NrOfCircuit + 1).ToString());
											wrapper.Name += "\n" + circuit;
										} else {
											string circuit = EuroplanRes.ProjectReport_System;
											circuit = circuit.Replace("%SYSTEM%", otherProduct.FullName);
											circuit = circuit.Replace("%RAUMID%", otherProduct.AssociatedRoom.Id);
											circuit = circuit.Replace("%RAUMNAME%", otherProduct.AssociatedRoom.Name);
											wrapper.Name += "\n" + circuit;
										}
									}
									wrapper.Area += "\n" + (otherProduct.PlannedFloorArea + otherProduct.PlannedWallArea + otherProduct.PlannedCeilingArea) + "m²";
								}
								foreach (ConnectionPipe pipe in pp.Product.PlannedConnectionPipes) {
									if (pipe.ConnectionThrough != null && pipe.ConnectionThrough.Product.PlannedProductIsConnection && (!pipe.OnlyFirst || c.NrOfCircuit == 0)) {
										wrapper.Name += "\n";
										if (pipe.Verlegeart != ConnectionPipe.VerlegeartEnum.VA_UNTER_ESTRICH && pipe.Insulation != ConnectionPipe.InsulationEnum.IN_NONE) {
											string conStr = EuroplanRes.ProjectReport_AnbindungDurchRaumGedaemmt;
											conStr = conStr.Replace("%RAUMID%", pipe.ConnectionThrough.Product.AssociatedRoom.Id);
											conStr = conStr.Replace("%RAUMNAME%", pipe.ConnectionThrough.Product.AssociatedRoom.Name);
											conStr = conStr.Replace("%ROHRTYP%", new ConnectionPipe.PipeTypeEnumConverter().ConvertToString(pipe.PipeType));
											conStr = conStr.Replace("%VERLEGEART%", new ConnectionPipe.VerlegeartEnumConverter().ConvertToString(pipe.Verlegeart));
											conStr = conStr.Replace("%DAEMMUNG%", new ConnectionPipe.InsulationEnumConverter().ConvertToString(pipe.Insulation));
											wrapper.Name += conStr;
										} else {
											string conStr = EuroplanRes.ProjectReport_AnbindungDurchRaum;
											conStr = conStr.Replace("%RAUMID%", pipe.ConnectionThrough.Product.AssociatedRoom.Id);
											conStr = conStr.Replace("%RAUMNAME%", pipe.ConnectionThrough.Product.AssociatedRoom.Name);
											conStr = conStr.Replace("%ROHRTYP%", new ConnectionPipe.PipeTypeEnumConverter().ConvertToString(pipe.PipeType));
											conStr = conStr.Replace("%VERLEGEART%", new ConnectionPipe.VerlegeartEnumConverter().ConvertToString(pipe.Verlegeart));
											wrapper.Name += conStr;
										}
										wrapper.Area += "\n" + Math.Round(pipe.Ruecklauf, 1) + EuroplanRes.Unit_Meter; // "m";
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

		public List<ModulDeckeVerlegeDatenWrapper> GetModulDeckeVerlegeDatenWrapper() {
			List<ModulDeckeVerlegeDatenWrapper> wrapperList = new List<ModulDeckeVerlegeDatenWrapper>();

			ModulKlimaDeckeProduct p = null;
			int prevCircuit;
			int prevTeilFlaeche;
			ModulDeckeVerlegeDatenWrapper wrapper;
			int subAreaCount;
			int rowCount;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						if (pp.Product is ModulKlimaDeckeProduct) {
							p = pp.Product as ModulKlimaDeckeProduct;
							prevCircuit = 0;
							foreach (ModulDeckeCircuit c in p.PlannedCircuits) {
								subAreaCount = 0;
								prevTeilFlaeche = 0;
								foreach (ModulDeckeSubArea a in c.SubAreas) {
									subAreaCount++;
									rowCount = 0;
									foreach (KlimaFlaechenList l in a.Rows) {
										rowCount++;
										wrapper = new ModulDeckeVerlegeDatenWrapper();
										wrapper.FloorId = floor.Id;
										wrapper.FloorName = floor.Name;
										if (prevCircuit == 0 && prevTeilFlaeche == 0) {
											wrapper.RoomId = room.Id;
											wrapper.RoomName = room.Name;
											wrapper.TeilSystem = pp.InternalName;
											wrapper.Circuit = c.NrOfCircuit + 1;
											prevCircuit = wrapper.Circuit;
											wrapper.TeilFlaeche = subAreaCount;
											prevTeilFlaeche = wrapper.TeilFlaeche;
										} else {
											wrapper.RoomId = "";
											wrapper.RoomName = "";
											wrapper.TeilSystem = "";
											if (prevCircuit != c.NrOfCircuit + 1) {
												wrapper.Circuit = c.NrOfCircuit + 1;
												prevCircuit = wrapper.Circuit;
											} else {
												wrapper.Circuit = 0;
											}
											if (prevTeilFlaeche != subAreaCount) {
												wrapper.TeilFlaeche = subAreaCount;
												prevTeilFlaeche = wrapper.TeilFlaeche;
											} else {
												wrapper.TeilFlaeche = 0;
											}
										}
										wrapper.Reihe = rowCount;

										foreach (KlimaFlaechenModul register in l.List) {
											wrapper.Modules.Add(register);
										}

										wrapperList.Add(wrapper);
									}
								}
							}
						}
					}
				}
			}

			return wrapperList;
		}

		public List<KonstruktionenWrapper> GetKonstruktionenWrapper() {
			List<KonstruktionenWrapper> wrapperList = new List<KonstruktionenWrapper>();

			string konstruktion = "";
			Dictionary<string, List<PlannedProduct>> konstruktionen = new Dictionary<string, List<PlannedProduct>>();

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						if (pp.Product is HithermProduct) {
							HithermProduct hp = pp.Product as HithermProduct;
							foreach (HithermCircuit c in hp.PlannedCircuits) {
								foreach (HithermRegister register in c.Registers) {
									if (register.Wall != null) {
										konstruktion = register.Wall.Id + " " + register.Wall.Name;
										AddKeyValueToDictionary(konstruktion, pp, konstruktionen);
									}
								}
							}
						} else if (pp.Product is HithermCompactProduct) {
							HithermCompactProduct hp = pp.Product as HithermCompactProduct;
							foreach (HithermCompactCircuit c in hp.PlannedCircuits) {
								foreach (HithermCompactRegister register in c.Registers) {
									if (register.Wall != null) {
										konstruktion = register.Wall.Id + " " + register.Wall.Name;
										AddKeyValueToDictionary(konstruktion, pp, konstruktionen);
									}
								}
							}
						} else {
							if (pp.Product.PlannedInsideConstruction != null) {
								konstruktion = pp.Product.PlannedInsideConstruction.Id + " " +
									pp.Product.PlannedInsideConstruction.LocalizedName;
								if (pp.Product.PlannedOutsideConstruction != null) {
									konstruktion += " + " +
										pp.Product.PlannedOutsideConstruction.Id + " " +
										pp.Product.PlannedOutsideConstruction.LocalizedName;
								}
								AddKeyValueToDictionary(konstruktion, pp, konstruktionen);
							} else {
								if (pp.Product.PlannedOutsideConstruction != null) {
									konstruktion += pp.Product.PlannedOutsideConstruction.Id + " " +
										pp.Product.PlannedOutsideConstruction.LocalizedName;
									AddKeyValueToDictionary(konstruktion, pp, konstruktionen);
								}
							}
						}
					}
				}
			}

			KonstruktionenWrapper wrapper;
			foreach (string k in konstruktionen.Keys) {
				wrapper = new KonstruktionenWrapper(k, konstruktionen[k]);
				wrapperList.Add(wrapper);
			}

			return wrapperList;
		}

		private void AddKeyValueToDictionary(string konstruktion, PlannedProduct pp, Dictionary<string, List<PlannedProduct>> konstruktionen) {
			if (konstruktionen.ContainsKey(konstruktion)) {
				if (!konstruktionen[konstruktion].Contains(pp)) {
					konstruktionen[konstruktion].Add(pp);
				}
			} else {
				List<PlannedProduct> products = new List<PlannedProduct>();
				products.Add(pp);
				konstruktionen.Add(konstruktion, products);
			}
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