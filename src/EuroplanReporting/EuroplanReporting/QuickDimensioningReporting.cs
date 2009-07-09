using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Europlan.Common;
using System.Threading;

namespace Europlan.EuroplanReporting {
	public partial class QuickDimensioningReporting : UserControl {

		private System.ComponentModel.ComponentResourceManager resources = ResourcesManager.resources;
		
		public QuickDimensioningReporting() {
			InitializeComponent();
		}

		public void DesignReport() {
			listLabel1.Design();
		}

		public void UpdateControl(DataSet data, bool cooling, bool heating, bool ceilingPlanned, Project project) {
			listLabel1.DataSource = data;
			List<QuickDimensioningReportWrapper> reportWrapper = Project.Instance.QuickDimensioning.GetQuickDimensioningRoomReports();

			string projectName = "";
			foreach (string line in Project.Instance.ProjectName) {
				projectName += line + "\n";
			}
			projectName = projectName.TrimEnd();
			listLabel1.Variables.Add("@ProjectName", projectName);
			listLabel1.Variables.Add("@ProjectEditor", Project.Instance.ProjectEditor);
			listLabel1.Variables.Add("@NrOfProducts", Project.Instance.QuickDimensioning.GetPlannedProducts().Count);
			listLabel1.Variables.Add("@PartnerContact", Licensing.LicenseManager.Instance.License.Header.Replace("\r", ""));
			if (heating) {
				listLabel1.Variables.Add("@tvHeat", Project.Instance.QuickDimensioning.HeatFlowTemperature);
			} else {
				listLabel1.Variables.Add("@tvHeat", -1);
			}
			if (cooling) {
				listLabel1.Variables.Add("@tvCool", Project.Instance.QuickDimensioning.CoolFlowTemperature);
			} else {
				listLabel1.Variables.Add("@tvCool", -1);
			}
			if (ceilingPlanned) {
				listLabel1.Variables.Add("@Allocation", Project.Instance.QuickDimensioning.CeilingAllocation + "%");
			} else {
				listLabel1.Variables.Add("@Allocation", "");
			}
			listLabel1.Variables.Add("@PartnerLogo", Image.FromFile("Reporting/partner.jpg"));
			string usedRoomTypes = "";
			foreach (RoomType roomType in Project.Instance.Config.RoomTypes) {
				foreach (QuickDimensioningReportWrapper wrapper in reportWrapper) {
					if (wrapper.RoomType == roomType.Name) {
						usedRoomTypes += roomType.Name + ": " + roomType.HeatLoadPerSquareMeter + "W/m² - " + roomType.CoolLoadPerSquareMeter + "W/m²\n";
						break;
					}
				}
			}
			usedRoomTypes = usedRoomTypes.TrimEnd();
			listLabel1.Variables.Add("@RoomTypes", usedRoomTypes);
			int i = 1;
			foreach (string productName in Project.Instance.QuickDimensioning.GetPlannedProducts()) {
				listLabel1.Variables.Add("@Product" + i, productName);
				i++;
			}
			Dictionary<Room.RoomController, int> roomControllers = new Dictionary<Room.RoomController, int>();
			foreach (Floor floor in Project.Instance.Floors) {
				foreach (Room room in floor.Rooms) {
					if (room.QuickDimensioningRoomController != Room.RoomController.None) {
						if (!roomControllers.ContainsKey(room.QuickDimensioningRoomController)) {
							roomControllers.Add(room.QuickDimensioningRoomController, 1);
						} else {
							roomControllers[room.QuickDimensioningRoomController] = roomControllers[room.QuickDimensioningRoomController] + 1;
						}
					}
				}
			}
			string controllersSummary = null;
			string localized = "";
			foreach (KeyValuePair<Room.RoomController, int> kvp in roomControllers) {
				if (controllersSummary != null) {
					controllersSummary += ", ";
				} else {
					controllersSummary = "";
				}
				localized = resources.GetString(kvp.Key.ToString(), Thread.CurrentThread.CurrentUICulture);
				controllersSummary += kvp.Value.ToString() + " * " + localized;
			}
			listLabel1.Variables.Add("@RoomControllers", controllersSummary);
			listLabel1.Print(combit.ListLabel14.LlProject.List, @"Reporting\QuickDimensioning.lst", false, combit.ListLabel14.LlPrintMode.PreviewControl, combit.ListLabel14.LlBoxType.None, "", false, null);
		}
	}
}
