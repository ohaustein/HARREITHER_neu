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

			this.SetLanguage();

			UpdateControl(true);
		}

		private void SetLanguage() {
			this.label4.Text = EuroplanRes.General_GradCelsius;
			this.label5.Text = EuroplanRes.General_Prozent;
			this.label13.Text = EuroplanRes.General_GradCelsius;
			this.label11.Text = EuroplanRes.General_GradCelsius;
			this.label7.Text = EuroplanRes.General_GradCelsius;
			this.btnNext.Text = EuroplanRes.General_Weiter;

			this.label1.Text = EuroplanRes.FacilityDetailsSummaryPanel_Anlagedaten; //"Anlagedaten";
			this.label3.Text = EuroplanRes.FacilityDetailsSummaryPanel_Normaussentemperatur; //"Normaußentemperatur:";
			this.label2.Text = EuroplanRes.FacilityDetailsSummaryPanel_Spreizung; //"Spreizung:";
			this.chkSpreizung.Text = EuroplanRes.FacilityDetailsSummaryPanel_VariableSpreizungVerwenden; //"variable Spreizung für endgültige Berechnung verwenden";
			this.groupBox1.Text = EuroplanRes.FacilityDetailsSummaryPanel_Allgemein; //"Generell";
			this.groupBox2.Text = EuroplanRes.FacilityDetailsSummaryPanel_Kuehlung; //"Kühlung";
			this.label12.Text = EuroplanRes.FacilityDetailsSummaryPanel_Taupunkttemperatur; //"Taupunkttemperatur:";
			this.label10.Text = EuroplanRes.FacilityDetailsSummaryPanel_InnentemperaturKuehlung; //"Innentemperatur für Kühlung:";
			this.label8.Text = EuroplanRes.FacilityDetailsSummaryPanel_Luftfeuchtigkeit; //"Relative Luftfeuchtigkeit:";
			this.label6.Text = EuroplanRes.FacilityDetailsSummaryPanel_AussentemperaturKuehlung; //"Außentemperatur für Kühlung:";
			this.chkCool.Text = EuroplanRes.FacilityDetailsSummaryPanel_KuehlleistungBerechnen; //"Kühlleistung berechnen";

		}

		public void UpdateControl(bool resetUserInterface) {
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
				numDewPoint.Text = Math.Round(EN1264.Instance.TaupunktTemperatur(((double)numHumidity.Value) / 100, (double)numInsideTemperature.Value), 1).ToString();
			} else {
				numDewPoint.Text = "";
			}

		}

		public bool AllowLeave() {
			if (Project.Instance.CalculateCoolLoad) {
				double dewPoint = Math.Round(EN1264.Instance.TaupunktTemperatur(((double)Project.Instance.RelativeHumidity) / 100, (double)Project.Instance.InsideTemperatureForCooling), 1);
				foreach (RegulatorCircuit circuit in Project.Instance.RegulatorCircuits) {
					if (dewPoint > circuit.CoolFlowTemperature) {
						DialogResult result = MessageBox.Show(EuroplanRes.FacilityDetailsSummaryPanel_KuehltemperaturZuNiedrigText, EuroplanRes.FacilityDetailsSummaryPanel_KuehltemperaturZuNiedrigTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
						return result.Equals(DialogResult.Yes) ? false : true;
					}
				}
			}
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
				numDewPoint.Text = Math.Round(EN1264.Instance.TaupunktTemperatur(((double)numHumidity.Value) / 100, (double)numInsideTemperature.Value), 1).ToString();
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
				numDewPoint.Text = Math.Round(EN1264.Instance.TaupunktTemperatur(((double)numHumidity.Value) / 100, (double)numInsideTemperature.Value), 1).ToString();
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
				numDewPoint.Text = Math.Round(EN1264.Instance.TaupunktTemperatur(((double)numHumidity.Value) / 100, (double)numInsideTemperature.Value), 1).ToString();
			} else {
				numDewPoint.Text = "";
			}
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void btnNext_Click(object sender, EventArgs e) {
			if (TreeSelectionRequested != null) {
				TreeSelectionRequested(this, typeof(RegulatorCircuitsSummaryPanel));
			}
		}


	}
}
