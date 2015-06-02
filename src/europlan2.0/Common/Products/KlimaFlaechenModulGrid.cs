using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Europlan.Common {

	public delegate void KlimaFlaechenModulGridContentChangedHandler(object sender);

	public partial class KlimaFlaechenModulGrid : UserControl {

		public event KlimaFlaechenModulGridContentChangedHandler GridContentChanged;

		private List<KlimaFlaechenModul> modules;
		private bool boden = true;

		public KlimaFlaechenModulGrid() {
			InitializeComponent();
			this.SetLanguage();
			UpdateComboboxValues();
		}

		private void SetLanguage() {
			this.btnAdd.Text = EuroplanRes.General_Plus; //"+"
			this.btnRemove.Text = EuroplanRes.General_Minus; //"-"
			this.btnAlign.Text = EuroplanRes.KlimaFlaechenModulGrid_AutomatischeAusrichtung; //"Automatische Ausrichtung"
			this.label1.Text = EuroplanRes.KlimaFlaechenModulGrid_AnzahlModule; //"Anzahl der Module:"
			this.modulTypeDataGridViewTextBoxColumn.HeaderText = EuroplanRes.KlimaFlaechenModulGrid_Modultype; //"Modultyp"
			this.orientationDataGridViewTextBoxColumn.HeaderText = EuroplanRes.KlimaFlaechenModulGrid_Ausrichtung; //"Ausrichtung"
            this.modulationDataGridViewTextBoxColumn.HeaderText = EuroplanRes.KlimaFlaechenModulGrid_Modulation; // "Verlegeart"
		}

		public void ResetGrid() {
			klimaFlaechenModulBindingSource.DataSource = this.modules;
			klimaFlaechenModulBindingSource.ResetBindings(false);
			lblCount.Text = modules.Count.ToString();
			if (modules != null) {
				btnRemove.Enabled = modules.Count > 0;
				btnAlign.Enabled = modules.Count > 1;
			}
		}

		public List<KlimaFlaechenModul> Row {
			set {
				modules = value;
				ResetGrid();
			}
		}

        private bool showModulation = false;

        public bool ShowModulation
        {
            get { return showModulation; }
            set
            {
                showModulation = value;
                UpdateComboboxValues();
            }
        }	

		public bool Boden {
			get { return boden; }
			set { 
				boden = value;
				UpdateComboboxValues();
			}
		}

		private void UpdateComboboxValues() {
            modulationDataGridViewTextBoxColumn.Items.Clear();
            modulationDataGridViewTextBoxColumn.Items.Add(KlimaFlaechenModul.ModulModulationEnum.MODULATION_NONE);
            modulationDataGridViewTextBoxColumn.Items.Add(KlimaFlaechenModul.ModulModulationEnum.MODULATION_SINGLE_MODULATED);
			orientationDataGridViewTextBoxColumn.Items.Clear();
			orientationDataGridViewTextBoxColumn.Items.Add(KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT);
			orientationDataGridViewTextBoxColumn.Items.Add(KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT);
			modulTypeDataGridViewTextBoxColumn.Items.Clear();
			if (boden) {
				modulTypeDataGridViewTextBoxColumn.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40);
			} else {
				modulTypeDataGridViewTextBoxColumn.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_120_30);
				modulTypeDataGridViewTextBoxColumn.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_30);
				modulTypeDataGridViewTextBoxColumn.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_80_30);
				modulTypeDataGridViewTextBoxColumn.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60);
				modulTypeDataGridViewTextBoxColumn.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B);
				modulTypeDataGridViewTextBoxColumn.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60C);
				modulTypeDataGridViewTextBoxColumn.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60D);
				modulTypeDataGridViewTextBoxColumn.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40);
			}

            modulationDataGridViewTextBoxColumn.Visible = ShowModulation;
		}

		private void btnAdd_Click(object sender, EventArgs e) {
			KlimaFlaechenModul modul = new KlimaFlaechenModul();
			if (modules.Count > 0) {
				modul.ModulType = modules[modules.Count - 1].ModulType;
			} else {
				modul.ModulType = (KlimaFlaechenModul.ModulTypeEnum)modulTypeDataGridViewTextBoxColumn.Items[0];
			}
			if (modules.Count > 0) {
				KlimaFlaechenModul m = modules[modules.Count - 1];
				if (m.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT) {
					modul.Orientation = KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT;
				} else {
					modul.Orientation = KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT;
				}
			}
			modules.Add(modul);
			ResetGrid();
			if (GridContentChanged != null) {
				this.GridContentChanged(this);
			}
		}

		private void btnRemove_Click(object sender, EventArgs e) {
			if (dgvModules.CurrentCell.RowIndex >= 0) {
				KlimaFlaechenModul modul = dgvModules.Rows[dgvModules.CurrentCell.RowIndex].DataBoundItem as KlimaFlaechenModul;
				if (modul != null) {
					modules.Remove(modul);
				}
				ResetGrid();
				if (GridContentChanged != null) {
					this.GridContentChanged(this);
				}
			}
		}

		private void btnAlign_Click(object sender, EventArgs e) {
			if (modules != null && modules.Count >= 1) {
				bool left = modules[0].Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT;
				foreach (KlimaFlaechenModul modul in modules) {
					if (left) {
						modul.Orientation = KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT;
					} else {
						modul.Orientation = KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT;
					}
					left = !left;
				}
				ResetGrid();
				if (GridContentChanged != null) {
					this.GridContentChanged(this);
				}
			}
		}

		private void dgvModules_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			if (GridContentChanged != null) {
				this.GridContentChanged(this);
			}
		}

		private void dgvModules_DataError(object sender, DataGridViewDataErrorEventArgs e) {
		}
	}
}
