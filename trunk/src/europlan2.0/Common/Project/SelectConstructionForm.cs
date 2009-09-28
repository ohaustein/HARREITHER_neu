using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class SelectConstructionForm : Form {

		private bool loaded = false;
		private Construction selectConstructionAfterLoad = null;
		private ConstructionListWrapper constructions;

		public SelectConstructionForm(ConstructionScopeEnum scope) {
			InitializeComponent();
			this.constructions = new ConstructionListWrapper(Configuration.ConfigurationType.ProjectConfiguration);
			this.constructions.ConstructionScopeFilter = scope;
			this.constructionBindingSource.DataSource = this.constructions;
			this.constructionBindingSource.ResetBindings(false);
		}

		public Construction SelectedConstruction {
			get {
				if (this.dgvConstructions.SelectedRows.Count > 0) {
					return this.dgvConstructions.SelectedRows[0].DataBoundItem as Construction;
				}
				return null;
			}
			set {
				if (loaded) {
					foreach (DataGridViewRow row in this.dgvConstructions.Rows) {
						row.Selected = (row.DataBoundItem == value);
					}
				} else {
					this.selectConstructionAfterLoad = value;
				}
			}
		}

		private void SelectConstructionForm_Load(object sender, EventArgs e) {
			this.loaded = true;
			//if (this.selectConstructionAfterLoad != null) {
				this.SelectedConstruction = this.selectConstructionAfterLoad;
				this.selectConstructionAfterLoad = null;
			//}
		}

		private void SelectConstructionForm_FormClosing(object sender, FormClosingEventArgs e) {
			if (this.dgvConstructions.SelectedRows.Count == 0 && this.DialogResult == DialogResult.OK) {
				MessageBox.Show("Bitte wählen Sie eine Konstruktion aus", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				e.Cancel = true;
			}
		}
	}
}