using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class SelectMoveTargetForm : Form {

		private ModulKlimaDeckeProduct product;

		public SelectMoveTargetForm(ModulKlimaDeckeProduct product, bool moveRow) {
			InitializeComponent();
			this.product = product;
			if (moveRow) {
				this.label1.Text = EuroplanRes.SelectMoveTargetForm_TeilflaecheWaehlen;
			} else {
				this.label1.Text = EuroplanRes.SelectMoveTargetForm_HeizkreisWaehlen;
				this.lstSubarea.Enabled = false;
			}
			this.UpdateLists(true, true);
		}

		private void UpdateLists(bool updateCircuits, bool updateSubareas) {
			if (updateCircuits) {
				this.lstCircuits.BeginUpdate();
				this.lstCircuits.Items.Clear();
				for (int i = 1; i <= this.product.PlannedCircuitCount; i++) {
					lstCircuits.Items.Add(EuroplanRes.PlannedModulKlimaDeckeProductPanel_HeizkreisAbkuerzung + i.ToString());
				}
				this.lstCircuits.Items.Add(EuroplanRes.ModulKlimaBodenPlannerForm_NeuerHK);
				this.lstCircuits.SelectedIndex = lstCircuits.Items.Count - 1;
				this.lstCircuits.EndUpdate();
			}
			if (updateSubareas) {
				this.lstSubarea.BeginUpdate();
				this.lstSubarea.Items.Clear();
				if (this.SelectedCircuit != null) {
					for (int i = 1; i <= this.SelectedCircuit.SubAreas.Count; i++) {
						lstSubarea.Items.Add(EuroplanRes.PlannedModulKlimaDeckeProductPanel_Teilflaeche + " " + i.ToString());
					}
				}
				lstSubarea.Items.Add(EuroplanRes.ModulKlimaDeckePlannerForm_NeueTeilflaeche);
				lstSubarea.SelectedIndex = this.lstSubarea.Enabled ? lstSubarea.Items.Count - 1 : -1;
				this.lstSubarea.EndUpdate();
			}
		}

		public ModulDeckeCircuit SelectedCircuit {
			get { return this.lstCircuits.SelectedIndex < this.product.PlannedCircuits.Count ? (ModulDeckeCircuit)this.product.PlannedCircuits[this.lstCircuits.SelectedIndex] : null; }
		}

		public ModulDeckeSubArea SelectedSubarea {
			get { return this.lstSubarea.SelectedIndex < this.SelectedCircuit.SubAreas.Count ? this.SelectedCircuit.SubAreas[this.lstSubarea.SelectedIndex] : null; }
		}

		private void lstCircuits_SelectedIndexChanged(object sender, EventArgs e) {
			this.UpdateLists(false, true);
		}

		private void btnOk_Click(object sender, EventArgs e) {
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}