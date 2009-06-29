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

		public List<QuickDimensioningDistributor> Distributors {
			set { 
				quickDimensioningDistributorBindingSource.DataSource = value;
				quickDimensioningDistributorBindingSource.ResetBindings(false);
			}
		}

		private void gridDistributors_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e) {
			QuickDimensioningDistributor distributor = e.Row.DataBoundItem as QuickDimensioningDistributor;
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

		public void Cleanup() {
			if (this.gridDistributors.SelectedCells.Count > 0) {
				if (this.gridDistributors.SelectedCells[0].OwningRow.DataBoundItem == null) {
					bool dirty = this.gridDistributors.IsCurrentRowDirty;
					bool clear = (this.quickDimensioningDistributorBindingSource.DataSource as List<QuickDimensioningDistributor>).Count == 1;
					this.gridDistributors.CancelEdit();
					if (clear) {
						(this.quickDimensioningDistributorBindingSource.DataSource as List<QuickDimensioningDistributor>).Clear();
					}
				} else {
					this.gridDistributors.EndEdit();
				}
			}
		}

	}
}
