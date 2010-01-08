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
			List<FloorOverviewWrapper> floorOverviewWrapper = new List<FloorOverviewWrapper>();
			List<EurovalAreaOverviewWrapper> eurovalOverviewWrapper = new List<EurovalAreaOverviewWrapper>();
			List<OpenLoadForRoomWrapper> openHeatLoadWrapper = new List<OpenLoadForRoomWrapper>();
			List<OpenLoadForRoomWrapper> openCoolLoadWrapper = new List<OpenLoadForRoomWrapper>();
			List<RegulatorCircuitWrapper> regulatorCircuitWrapper = new List<RegulatorCircuitWrapper>();
			List<DistributorWrapper> distributorWrapper = new List<DistributorWrapper>();
			List<RoomOverviewWrapper> roomOverviewWrapper = new List<RoomOverviewWrapper>();
			List<EurovalWrapper> eurovalAuslegungWrapper = new List<EurovalWrapper>();
			List<BilanzWrapper> eurovalBilanzWrapper = new List<BilanzWrapper>();
			List<VerlegedatenCircuitWrapper> verlegedatenCircuitWrapper = new List<VerlegedatenCircuitWrapper>();
			List<RequiredMaterialWrapper> requiredMaterialWrapper = new List<RequiredMaterialWrapper>();

			projectWarningWrapper = this.GetProjectWarningReport();

			if (reportOptions.ProjectOverview) {
				projektBilanzWrapper = this.GetProkjektBilanzReport();

				if (reportOptions.AreaOverview) {
					floorOverviewWrapper = this.GetFloorOverviewWrapper();
					eurovalOverviewWrapper = GetEurovalOverviewWrapper();
				}

				openHeatLoadWrapper = this.GetOpenHeatLoadForRoomWrapper();
				openCoolLoadWrapper = this.GetOpenCoolLoadForRoomWrapper();
				regulatorCircuitWrapper = this.GetRegulatorCircuitWrapper();
				distributorWrapper = this.GetDistributorWrapper();
				roomOverviewWrapper = this.GetRoomOverviewWrapper();
			}



			if (reportOptions.Auslegung || reportOptions.Verlegedaten) {
				eurovalAuslegungWrapper = GetEurovalWrapper();
			}

			if (reportOptions.Auslegung && reportOptions.AuslegungBilanz) {
				eurovalBilanzWrapper = GetEurovalBilanzWrapper();
			}

			if (reportOptions.Verlegedaten) {
				verlegedatenCircuitWrapper = GetVerlegedatenCircuitWrapper();
			}
			
			if (reportOptions.RequiredMaterial || reportOptions.RecommendedMaterial) {
				requiredMaterialWrapper = GetRequiredMaterialWrapper();
			}

			DataTable projektBilanz = ReportHelper.ListToDataTable<BilanzWrapper>(projektBilanzWrapper);
			DataTable projectWarnings = ReportHelper.ListToDataTable<ProjectWarningWrapper>(projectWarningWrapper);
			DataTable floorOverwiew = ReportHelper.ListToDataTable<FloorOverviewWrapper>(floorOverviewWrapper);
			DataTable eurovalOverview = ReportHelper.ListToDataTable<EurovalAreaOverviewWrapper>(eurovalOverviewWrapper);
			DataTable openHeatLoad = ReportHelper.ListToDataTable<OpenLoadForRoomWrapper>(openHeatLoadWrapper);
			DataTable openCoolLoad = ReportHelper.ListToDataTable<OpenLoadForRoomWrapper>(openCoolLoadWrapper);
			DataTable regulatorCircuits = ReportHelper.ListToDataTable<RegulatorCircuitWrapper>(regulatorCircuitWrapper);
			DataTable distributors = ReportHelper.ListToDataTable<DistributorWrapper>(distributorWrapper);
			DataTable roomOverview = ReportHelper.ListToDataTable<RoomOverviewWrapper>(roomOverviewWrapper);
			DataTable eurovalAuslegung = ReportHelper.ListToDataTable<EurovalWrapper>(eurovalAuslegungWrapper);
			DataTable eurovalBilanz = ReportHelper.ListToDataTable<BilanzWrapper>(eurovalBilanzWrapper);
			DataTable verlegedatenCircuit = ReportHelper.ListToDataTable<VerlegedatenCircuitWrapper>(verlegedatenCircuitWrapper);
			DataTable requiredMaterial = ReportHelper.ListToDataTable<RequiredMaterialWrapper>(requiredMaterialWrapper);

			projektBilanz.TableName = "ProjektBilanz";
			projectWarnings.TableName = "ProjectWarnings";
			floorOverwiew.TableName = "FloorOverview";
			eurovalOverview.TableName = "EurovalOverview";
			openHeatLoad.TableName = "OpenHeatLoad";
			openCoolLoad.TableName = "OpenCoolLoad";
			regulatorCircuits.TableName = "RegulatorCircuits";
			distributors.TableName = "Distributors";
			roomOverview.TableName = "RoomOverview";
			eurovalAuslegung.TableName = "EurovalAuslegung";
			eurovalBilanz.TableName = "EurovalBilanz";
			verlegedatenCircuit.TableName = "VerlegedatenCircuit";
			requiredMaterial.TableName = "RequiredMaterial";

			reportData.Tables.Add(projektBilanz);
			reportData.Tables.Add(projectWarnings);
			reportData.Tables.Add(floorOverwiew);
			reportData.Tables.Add(eurovalOverview);
			reportData.Tables.Add(openHeatLoad);
			reportData.Tables.Add(openCoolLoad);
			reportData.Tables.Add(regulatorCircuits);
			reportData.Tables.Add(distributors);
			reportData.Tables.Add(roomOverview);
			reportData.Tables.Add(eurovalAuslegung);
			reportData.Tables.Add(eurovalBilanz);
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

			double mhHeat = 0;
			double mhCool = 0;

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

						mhHeat += pp.Product.PlannedMhHeat;
						mhCool += pp.Product.PlannedMhCool;
												
						deltaRhoHeatMax = deltaRhoHeatMax < pp.Product.PlannedDeltaRhoHeat ? pp.Product.PlannedDeltaRhoHeat : deltaRhoHeatMax;
						deltaRhoCoolMax = deltaRhoCoolMax < pp.Product.PlannedDeltaRhoCool ? pp.Product.PlannedDeltaRhoCool : deltaRhoCoolMax;
						wasserInhalt += pp.Product.WasserInhalt;
					}
				}
			}

			BilanzWrapper wrapper = new BilanzWrapper();
			wrapper.Description = "Gestamt-Normwärmebedarf";
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
			wrapper.Description = "Gestamt-Heizleistung (nach innen)";
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
			wrapper.Description = "Gestamt-Wassermenge";
			wrapper.HeatValue = mhHeat.ToString("0.##");
			wrapper.HeatUnit = "l/h";
			wrapper.CoolValue = mhCool.ToString("0.##");
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
			wrapper.Description = "Gestamt-Raumfläche";
			wrapper.HeatValue = roomArea.ToString("0.##");
			wrapper.HeatUnit = "m²";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gestamt-Fußbodenheizungsfläche";
			wrapper.HeatValue = plannedFloorArea.ToString("0.##");
			wrapper.HeatUnit = "m²";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gestamt-Wandheizungsfläche";
			wrapper.HeatValue = plannedWallArea.ToString("0.##");
			wrapper.HeatUnit = "m²";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gestamt-Deckenkühlungsfläche";
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

			double mhHeat = 0;
			double mhCool = 0;

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

							mhHeat += pp.Product.PlannedMhHeat;
							mhCool += pp.Product.PlannedMhCool;

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
			wrapper.HeatValue = mhHeat.ToString("0.##");
			wrapper.HeatUnit = "l/h";
			wrapper.CoolValue = mhCool.ToString("0.##");
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
			wrapper.Description = "Gestamte Estrichfläche";
			wrapper.HeatValue = estrichArea.ToString("0.##");
			wrapper.HeatUnit = "m²";
			wrapperList.Add(wrapper);

			wrapper = new BilanzWrapper();
			wrapper.Description = "Gestamte Heizfläche";
			wrapper.HeatValue = plannedArea.ToString("0.##");
			wrapper.HeatUnit = "m²";
			wrapperList.Add(wrapper);

			return wrapperList;
		}

		public List<ProjectWarningWrapper> GetProjectWarningReport() {
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
								pp.Product.GetHeatFlow(out vorlauf, out ruecklauf);
								if (ruecklauf < ruecklaufHeat) {
									ruecklaufHeat = ruecklauf;
								}
								pp.Product.GetCoolFlow(out vorlauf, out ruecklauf);
								if (ruecklauf > ruecklaufCool) {
									ruecklaufCool = ruecklauf;
								}
								durchflussHeat += pp.Product.PlannedMhHeat;
								durchflussCool += pp.Product.PlannedMhCool;
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
						pp.Product.GetHeatFlow(out vorlauf, out ruecklauf);
						if (ruecklauf < ruecklaufHeat) {
							ruecklaufHeat = ruecklauf;
						}
						pp.Product.GetCoolFlow(out vorlauf, out ruecklauf);
						if (ruecklauf > ruecklaufCool) {
							ruecklaufCool = ruecklauf;
						}
						durchflussHeat += pp.Product.PlannedMhHeat;
						durchflussCool += pp.Product.PlannedMhCool;
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

								wrapperHeat.Wassermenge = pp.Product.PlannedMhHeat;
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

								wrapperCool.Wassermenge = pp.Product.PlannedMhCool;
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

		public List<VerlegedatenCircuitWrapper> GetVerlegedatenCircuitWrapper() {
			List<VerlegedatenCircuitWrapper> wrapperList = new List<VerlegedatenCircuitWrapper>();
			VerlegedatenCircuitWrapper wrapper = null;

			int count = 1;

			foreach (Floor floor in project.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
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
								wrapper.Name += room.Id + " (" + room.Name + ")";
								if (pp.Product.PlannedCircuits.Count > 1) {
									wrapper.Name += ", Heizkreis " + (c.NrOfCircuit + 1);
								}

								wrapper.Area += pp.PlannedArea + "m²";
								wrapper.CircuitNumber = count++;
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