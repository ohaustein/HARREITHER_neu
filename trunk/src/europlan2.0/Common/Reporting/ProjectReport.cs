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

			List<ProjektBilanzWrapper> projektBilanzWrapper = project.GetProkjektBilanzReport();
			List<ProjectWarningWrapper> projectWarningWrapper = project.GetProjectWarningReport();
			List<FloorOverviewWrapper> floorOverviewWrapper = project.GetFloorOverviewWrapper();
			List<OpenLoadForRoomWrapper> openHeatLoadWrapper = project.GetOpenHeatLoadForRoomWrapper();
			List<OpenLoadForRoomWrapper> openCoolLoadWrapper = project.GetOpenCoolLoadForRoomWrapper();
			List<RegulatorCircuitWrapper> regulatorCircuitWrapper = project.GetRegulatorCircuitWrapper();
			List<DistributorWrapper> distributorWrapper = project.GetDistributorWrapper();
			List<RoomOverviewWrapper> roomOverviewWrapper = project.GetRoomOverviewWrapper();

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

	}
}