using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {

	public delegate void KlimaFlaechenModulGridContentChangedHandler(object sender);

	public partial class KlimaFlaechenModulGrid : UserControl {

		public event KlimaFlaechenModulGridContentChangedHandler GridContentChanged;

		private List<KlimaFlaechenModul> modules;
		private bool boden = true;

		public KlimaFlaechenModulGrid() {
			InitializeComponent();
			UpdateComboboxValues();
		}

		public void ResetGrid() {
			klimaFlaechenModulBindingSource.DataSource = this.modules;
			klimaFlaechenModulBindingSource.ResetBindings(false);
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

		public bool Boden {
			get { return boden; }
			set { 
				boden = value;
				UpdateComboboxValues();
			}
		}

		private void UpdateComboboxValues() {
			orientationDataGridViewTextBoxColumn.Items.Clear();
			orientationDataGridViewTextBoxColumn.Items.Add(KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT);
			orientationDataGridViewTextBoxColumn.Items.Add(KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT);
			modulTypeDataGridViewTextBoxColumn.Items.Clear();
			if (boden) {
				modulTypeDataGridViewTextBoxColumn.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40);
			} else {
				//modulTypeDataGridViewTextBoxColumn.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40);
				modulTypeDataGridViewTextBoxColumn.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_30);
				modulTypeDataGridViewTextBoxColumn.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_120_30);
				modulTypeDataGridViewTextBoxColumn.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_80_30);
			}
		}

		private void btnAdd_Click(object sender, EventArgs e) {
			KlimaFlaechenModul modul = new KlimaFlaechenModul();
			modul.ModulType = (KlimaFlaechenModul.ModulTypeEnum)modulTypeDataGridViewTextBoxColumn.Items[0];
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
				}
				ResetGrid();
				if (GridContentChanged != null) {
					this.GridContentChanged(this);
				}
			}
		}
	}
}
