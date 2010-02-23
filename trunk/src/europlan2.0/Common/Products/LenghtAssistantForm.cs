using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;
using System.Threading;

namespace Europlan.Common {
	public partial class LengthAssistantForm : Form {

		private ConnectionPipe pipe;
		private bool updateOngoing = false;
		private double availableArea;
		private System.Resources.ResourceManager resources = EuroplanRes.ResourceManager;

		public LengthAssistantForm(ConnectionPipe pipe) {
			InitializeComponent();
			this.SetLanguage();
			this.pipe = pipe;

			this.cmbLayDistance.Items.Clear();
			foreach (ConnectionPipe.VerlegeartEnum item in Enum.GetValues(typeof(ConnectionPipe.VerlegeartEnum))) {
				this.cmbLayDistance.Items.Add(item);
			}

			string connOf = EuroplanRes.LengthAssistent_AnbindeleitungVon;
			connOf = connOf.Replace("%SYSTEM%", pipe.ConnectionOf.ToString());
			connOf = connOf.Replace("%RAUM%", pipe.ConnectionOf.Product.AssociatedRoom.ToString());
			connOf = connOf.Replace("%ANZHK%", pipe.PlannedCircuits.ToString());
			this.lblDescription.Text = "Anbindeleitung von %SYSTEM% in %RAUM%, %ANZHK% Heizkreis(e)";
			this.cmbLayDistance.SelectedItem = pipe.Verlegeart;
			this.numVorlauf.Value = (decimal)pipe.Vorlauf;
			this.numVorlaufArea.Value = (decimal)GetAreaForPipeLength((double)this.numVorlauf.Value);
			this.numRuecklauf.Value = (decimal)pipe.Ruecklauf;
			this.numRuecklaufArea.Value = (decimal)GetAreaForPipeLength((double)this.numRuecklauf.Value);
			List<ConnectionPipe> pipes = pipe.ConnectionThrough.Product.PlannedConnectionPipesThroughThisProduct;
			double usedArea = 0;
			foreach (ConnectionPipe p in pipes) {
				if (p != pipe)
					usedArea += p.AreaTotal;
			}
			this.lblHeatArea.Text = pipe.ConnectionThrough.Product.PlannedNetArea + EuroplanRes.General_Quadratmeter;
			availableArea = pipe.ConnectionThrough.Product.PlannedNetArea - usedArea;
			this.lblAvailableArea.Text = availableArea + EuroplanRes.General_Quadratmeter;
		}

		private void SetLanguage() {
			this.btnCancel.Text = EuroplanRes.General_Abbrechen; //"Cancel";
			this.btnOk.Text = EuroplanRes.General_Ok; //"OK";
			this.label12.Text = EuroplanRes.LengthAssistent_VerfuegbareFlaecheOhneAnb; //"Verfügbare Fläche ohne andere Anbindeleitungen:";
			this.label11.Text = EuroplanRes.LengthAssistent_FlaecheFbh; //"Fläche der Fußbodenheizung ohne unbeheizte Flächen:";
			this.btnRestAreaRuecklauf.Text = EuroplanRes.LengthAssistent_RestlicheFlaeche; //"Restliche Fläche";
			this.btnAvailableAreaRuecklauf.Text = EuroplanRes.LengthAssistent_VerfuegbareFlaeche; //"Verfügbare Fläche";
			this.label7.Text = EuroplanRes.General_Quadratmeter; //"m²";
			this.label8.Text = EuroplanRes.LengthAssistent_Flaeche; //"Fläche";
			this.label9.Text = EuroplanRes.General_Meter; //"m";
			this.label10.Text = EuroplanRes.LengthAssistent_LaengeVorlauf; //"Rohrlänge Vorlauf:";
			this.btnRestAreaVorlauf.Text = EuroplanRes.LengthAssistent_RestlicheFlaeche; //"Restliche Fläche";
			this.btnAvailableAreaVorlauf.Text = EuroplanRes.LengthAssistent_VerfuegbareFlaeche; //"Verfügbare Fläche";
			this.label6.Text = EuroplanRes.General_Quadratmeter; //"m²";
			this.label5.Text = EuroplanRes.LengthAssistent_Flaeche; //"Fläche";
			this.label4.Text = EuroplanRes.General_Meter; //"m";
			this.label3.Text = EuroplanRes.LengthAssistent_LaengeVorlauf; //"Rohrlänge Vorlauf:";
			this.label2.Text = EuroplanRes.LengthAssistent_Verlegeart; //"Verlegeart:";
			this.Text = EuroplanRes.LengthAssistent_Laengenassistent; //"Längenassistent";
		}

		private void LengthAssistantForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["LengthAssistantForm"];
			this.Location = settings.GetPoint("Location", this.Location);
		}

		private void LengthAssistantForm_FormClosing(object sender, FormClosingEventArgs e) {
			if (this.DialogResult == DialogResult.OK && numVorlaufArea.Value + numRuecklaufArea.Value > (decimal)availableArea) {
				DialogResult result = MessageBox.Show(EuroplanRes.LengthAssistentForm_FlaecheZuGrossText, EuroplanRes.LengthAssistentForm_FlaecheZuGrossTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
				if (result == DialogResult.No) {
					e.Cancel = true;
					return;
				}
			}
			SettingsKey settings = SettingsFile.Settings["LengthAssistantForm"];
			settings.StorePoint("Location", this.Location);
			SettingsFile.Update();
		}

		private void btnAvailableAreaVorlauf_Click(object sender, EventArgs e) {
			numVorlaufArea.Value = (decimal)availableArea;
			numRuecklaufArea.Value = 0;
		}

		private void btnRestAreaVorlauf_Click(object sender, EventArgs e) {
			numVorlaufArea.Value = (decimal)availableArea - numRuecklaufArea.Value;
		}

		private void btnAvailableAreaRuecklauf_Click(object sender, EventArgs e) {
			numVorlaufArea.Value = 0;
			numRuecklaufArea.Value = (decimal)availableArea;
		}

		private void btnRestAreaRuecklauf_Click(object sender, EventArgs e) {
			numRuecklaufArea.Value = (decimal)availableArea - numVorlaufArea.Value;
		}

		private void cmbLayDistance_SelectedValueChanged(object sender, EventArgs e) {
			if ((ConnectionPipe.VerlegeartEnum)cmbLayDistance.SelectedItem == ConnectionPipe.VerlegeartEnum.VA_UNTER_ESTRICH) {
				numVorlaufArea.Enabled = false;
				numRuecklaufArea.Enabled = false;
				btnRestAreaVorlauf.Enabled = false;
				btnAvailableAreaVorlauf.Enabled = false;
				btnAvailableAreaRuecklauf.Enabled = false;
				btnRestAreaRuecklauf.Enabled = false;
				updateOngoing = true;
				numVorlaufArea.Value = 0;
				numRuecklaufArea.Value = 0;
				updateOngoing = false;
			} else {
				numVorlaufArea.Enabled = true;
				numRuecklaufArea.Enabled = true;
				btnRestAreaVorlauf.Enabled = true;
				btnAvailableAreaVorlauf.Enabled = true;
				btnAvailableAreaRuecklauf.Enabled = true;
				btnRestAreaRuecklauf.Enabled = true;
				numVorlaufArea.Value = (decimal)GetAreaForPipeLength((double)numVorlauf.Value);
				numRuecklaufArea.Value = (decimal)GetAreaForPipeLength((double)numRuecklauf.Value);
			}
		}

		private void numVorlauf_ValueChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				updateOngoing = true;
				numVorlaufArea.Value = (decimal)GetAreaForPipeLength((double)numVorlauf.Value);
				updateOngoing = false;
			}
		}

		private void numVorlaufArea_ValueChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				updateOngoing = true;
				numVorlauf.Value = (decimal)GetPipeLengthForArea((double)numVorlaufArea.Value);
				updateOngoing = false;
			}
		}

		private void numRuecklauf_ValueChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				updateOngoing = true;
				numRuecklaufArea.Value = (decimal)GetAreaForPipeLength((double)numRuecklauf.Value);
				updateOngoing = false;
			}
		}

		private void numRuecklaufArea_ValueChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				updateOngoing = true;
				numRuecklauf.Value = (decimal)GetPipeLengthForArea((double)numRuecklaufArea.Value);
				updateOngoing = false;
			}
		}

		private double GetPipeLengthForArea(double area) {
			int circuits = pipe.OnlyFirst ? 1 : pipe.PlannedCircuits;
			double length = 0;

			switch ((ConnectionPipe.VerlegeartEnum)cmbLayDistance.SelectedItem) {
				case Europlan.Common.ConnectionPipe.VerlegeartEnum.VA_UNTER_ESTRICH:
					length = 0;
					break;
				case ConnectionPipe.VerlegeartEnum.VA_EV35:
					length = area * EurovalProduct.GetPipeLengthPerSqm(EurovalProduct.EurovalLayDistance.EV35);
					break;

				case ConnectionPipe.VerlegeartEnum.VA_EV30:
					length = area * EurovalProduct.GetPipeLengthPerSqm(EurovalProduct.EurovalLayDistance.EV30);
					break;

				case ConnectionPipe.VerlegeartEnum.VA_EV25:
					length = area * EurovalProduct.GetPipeLengthPerSqm(EurovalProduct.EurovalLayDistance.EV25);
					break;

				case ConnectionPipe.VerlegeartEnum.VA_EV20:
					length = area * EurovalProduct.GetPipeLengthPerSqm(EurovalProduct.EurovalLayDistance.EV20);
					break;

				case ConnectionPipe.VerlegeartEnum.VA_EV15:
					length = area * EurovalProduct.GetPipeLengthPerSqm(EurovalProduct.EurovalLayDistance.EV15);
					break;

				case ConnectionPipe.VerlegeartEnum.VA_EV10:
					length = area * EurovalProduct.GetPipeLengthPerSqm(EurovalProduct.EurovalLayDistance.EV10);
					break;

				case ConnectionPipe.VerlegeartEnum.VA_EV5:
					length = area * EurovalProduct.GetPipeLengthPerSqm(EurovalProduct.EurovalLayDistance.EV5);
					break;

				case ConnectionPipe.VerlegeartEnum.VA_A5:
					length = area * EurovalProduct.GetPipeLengthPerSqm(EurovalProduct.EurovalLayDistance.A5);
					break;
				default:
					length = 0;
					break;
			}
			return length > 0 ? length / circuits : 0;
		}

		private double GetAreaForPipeLength(double pipeLength) {
			int circuits = pipe.OnlyFirst ? 1 : pipe.PlannedCircuits;
			double area = 0;

			switch ((ConnectionPipe.VerlegeartEnum)cmbLayDistance.SelectedItem) {
				case Europlan.Common.ConnectionPipe.VerlegeartEnum.VA_UNTER_ESTRICH:
					area = 0;
					break;
				case ConnectionPipe.VerlegeartEnum.VA_EV35:
					area = pipeLength / EurovalProduct.GetPipeLengthPerSqm(EurovalProduct.EurovalLayDistance.EV35);
					break;

				case ConnectionPipe.VerlegeartEnum.VA_EV30:
					area = pipeLength / EurovalProduct.GetPipeLengthPerSqm(EurovalProduct.EurovalLayDistance.EV30);
					break;

				case ConnectionPipe.VerlegeartEnum.VA_EV25:
					area = pipeLength / EurovalProduct.GetPipeLengthPerSqm(EurovalProduct.EurovalLayDistance.EV25);
					break;

				case ConnectionPipe.VerlegeartEnum.VA_EV20:
					area = pipeLength / EurovalProduct.GetPipeLengthPerSqm(EurovalProduct.EurovalLayDistance.EV20);
					break;

				case ConnectionPipe.VerlegeartEnum.VA_EV15:
					area = pipeLength / EurovalProduct.GetPipeLengthPerSqm(EurovalProduct.EurovalLayDistance.EV15);
					break;

				case ConnectionPipe.VerlegeartEnum.VA_EV10:
					area = pipeLength / EurovalProduct.GetPipeLengthPerSqm(EurovalProduct.EurovalLayDistance.EV10);
					break;

				case ConnectionPipe.VerlegeartEnum.VA_EV5:
					area = pipeLength / EurovalProduct.GetPipeLengthPerSqm(EurovalProduct.EurovalLayDistance.EV5);
					break;

				case ConnectionPipe.VerlegeartEnum.VA_A5:
					area = pipeLength / EurovalProduct.GetPipeLengthPerSqm(EurovalProduct.EurovalLayDistance.A5);
					break;
				default:
					area = 0;
					break;
			}
			return area * circuits;
		}

		public double Vorlauf {
			get { return (double)numVorlauf.Value; }
		}

		public double Ruecklauf {
			get { return (double)numRuecklauf.Value; }
		}

		public ConnectionPipe.VerlegeartEnum Verlegeart {
			get { return (ConnectionPipe.VerlegeartEnum)cmbLayDistance.SelectedItem; }
		}

	}
}