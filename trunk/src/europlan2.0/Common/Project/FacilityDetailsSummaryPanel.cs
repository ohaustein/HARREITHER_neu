using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class FacilityDetailsSummaryPanel : UserControl, IEditorUserControl {

		public event ProjectStructureChangedHandler ProjectStructureChanged;
		public event ProjectChangedHandler ProjectChanged;
		public event TreeSelectionRequestedHandler TreeSelectionRequested;

		public FacilityDetailsSummaryPanel() {
			InitializeComponent();
			UpdateControl();
		}

		public void UpdateControl() {
			Project project = Project.Instance;
			numNormOutsideTemperature.Value = project.NormOutsideTemperature;
			chkSpreizung.Checked = project.VariableSpreizung;
			chkCool.Checked = project.CalculateCoolLoad;
			numOutsideTemperature.Value = project.OutsideTemperatureForCooling;
			numHumidity.Value = project.RelativeHumidity;
			numInsideTemperature.Value = project.InsideTemperatureForCooling;
			numOutsideTemperature.Enabled = chkCool.Checked;
			numHumidity.Enabled = chkCool.Checked;
			numInsideTemperature.Enabled = chkCool.Checked;
			if (chkCool.Checked) {
				numDewPoint.Text = Math.Round(EN1264.Instance.TaupunktTemperatur(((double)numHumidity.Value) / 100, (double)numInsideTemperature.Value), 2).ToString();
			} else {
				numDewPoint.Text = "";
			}

		}

		public bool AllowLeave() {
			return true;
		}

		private void numNormOutsideTemperature_ValueChanged(object sender, EventArgs e) {
			Project.Instance.NormOutsideTemperature = (int)numNormOutsideTemperature.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void chkSpreizung_CheckedChanged(object sender, EventArgs e) {
			Project.Instance.VariableSpreizung = chkSpreizung.Checked;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void chkCool_CheckedChanged(object sender, EventArgs e) {
			Project.Instance.CalculateCoolLoad = chkCool.Checked;
			numOutsideTemperature.Enabled = chkCool.Checked;
			numHumidity.Enabled = chkCool.Checked;
			numInsideTemperature.Enabled = chkCool.Checked;
			if (chkCool.Checked) {
				numDewPoint.Text = Math.Round(EN1264.Instance.TaupunktTemperatur(((double)numHumidity.Value) / 100, (double)numInsideTemperature.Value), 2).ToString();
			} else {
				numDewPoint.Text = "";
			}
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numOutsideTemperature_ValueChanged(object sender, EventArgs e) {
			Project.Instance.OutsideTemperatureForCooling = (int)numOutsideTemperature.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numHumidity_ValueChanged(object sender, EventArgs e) {
			Project.Instance.RelativeHumidity = (int)numHumidity.Value;
			if (chkCool.Checked) {
				numDewPoint.Text = Math.Round(EN1264.Instance.TaupunktTemperatur(((double)numHumidity.Value) / 100, (double)numInsideTemperature.Value), 2).ToString();
			} else {
				numDewPoint.Text = "";
			}
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numInsideTemperature_ValueChanged(object sender, EventArgs e) {
			Project.Instance.InsideTemperatureForCooling = (int)numInsideTemperature.Value;
			if (chkCool.Checked) {
				numDewPoint.Text = Math.Round(EN1264.Instance.TaupunktTemperatur(((double)numHumidity.Value) / 100, (double)numInsideTemperature.Value), 2).ToString();
			} else {
				numDewPoint.Text = "";
			}
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}


	}
}
