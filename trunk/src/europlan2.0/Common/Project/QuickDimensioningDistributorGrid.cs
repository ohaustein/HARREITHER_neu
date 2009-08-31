using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class QuickDimensioningDistributorGrid : UserControl {

		public QuickDimensioningDistributorGrid() {
			InitializeComponent();
		}

		public List<Distributor> Distributors {
			set { 
				quickDimensioningDistributorBindingSource.DataSource = value;
				quickDimensioningDistributorBindingSource.ResetBindings(false);
			}
		}

		private void gridDistributors_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e) {
			if (this.quickDimensioningDistributorBindingSource.Count <= 1) {
				MessageBox.Show("Der Verteiler kann nicht gelöscht werden, da für die Flächenaufstellung mindestens ein Verteiler vorhanden sein muss.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
				e.Cancel = true;
			} else {
				Distributor distributor = e.Row.DataBoundItem as Distributor;
				Project project = Project.Instance;
				if (project != null) {
					foreach (Floor floor in project.Floors) {
						foreach (Room room in floor.Rooms) {
							foreach (Product product in room.UsedProductsForQuickDimensioning) {
								product.QuickDimensioningConnectedDistributors.Remove(distributor.Id);
							}
						}
					}
				}
			}
		}

		public void Cleanup() {
			if (this.gridDistributors.SelectedCells.Count > 0) {
				if (this.gridDistributors.SelectedCells[0].OwningRow.DataBoundItem == null) {
					bool dirty = this.gridDistributors.IsCurrentRowDirty;
					bool clear = (this.quickDimensioningDistributorBindingSource.DataSource as List<Distributor>).Count == 1;
					this.gridDistributors.CancelEdit();
					if (clear) {
						(this.quickDimensioningDistributorBindingSource.DataSource as List<Distributor>).Clear();
					}
				} else {
					this.gridDistributors.EndEdit();
				}
			}
		}

	}
}
