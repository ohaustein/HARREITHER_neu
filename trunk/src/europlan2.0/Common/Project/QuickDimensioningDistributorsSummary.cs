using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class QuickDimensioningDistributorsSummary : UserControl {

		public event ProjectChangedHandler ProjectChanged;

		public QuickDimensioningDistributorsSummary() {
			InitializeComponent();
			UpdateControl();
		}

		public void UpdateControl() {
			object oldSelectedItem = listDistributors.SelectedItem;
			listDistributors.Items.Clear();
			if (Project.Instance != null) {
				List<QuickDimensioningDistributor> distributors = Project.Instance.QuickDimensioning.Distributors;
				foreach (QuickDimensioningDistributor distributor in distributors) {
					listDistributors.Items.Add(distributor);
				}
			}
			if (oldSelectedItem != null) {
				listDistributors.SelectedItem = oldSelectedItem;
			}
			if (listDistributors.SelectedItem == null && listDistributors.Items.Count > 0) {
				listDistributors.SelectedIndex = 0;
			}
		}

		public bool AllowLeave() {
			return true;
		}

		private void OnProjectChanged() {
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
		}

		private void btnAdd_Click(object sender, EventArgs e) {
			if (txtDistributor.Text != "") {
				QuickDimensioningDistributor distributor = new QuickDimensioningDistributor();
				distributor.Name = txtDistributor.Text;
				txtDistributor.Text = "";
				Project.Instance.QuickDimensioning.Distributors.Add(distributor);
				this.OnProjectChanged();
				UpdateControl();
			}
		}

		private void btnRemove_Click(object sender, EventArgs e) {
			if (listDistributors.SelectedIndex >= 0) {
				Project project = Project.Instance;
				QuickDimensioningDistributor distributor = listDistributors.SelectedItem as QuickDimensioningDistributor;
				project.QuickDimensioning.Distributors.Remove(distributor);
				foreach (Floor floor in project.Floors) {
					foreach (Room room in floor.Rooms) {
						foreach (Product product in room.UsedProductsForQuickDimensioning) {
							product.QuickDimensioningConnectedDistributors.Remove(distributor.Id);
						}
					}
				}
			}
			this.OnProjectChanged();
			UpdateControl();
		}

		private void listDistributors_SelectedValueChanged(object sender, EventArgs e) {
			QuickDimensioningDistributor distributor = this.listDistributors.SelectedItem as QuickDimensioningDistributor;
			this.distributorGrid.Distributor = distributor;
		}

		public bool Euroval {
			get { return this.distributorGrid.Euroval; }
			set { this.distributorGrid.Euroval = value; }
		}

		public bool ConcreteActivation {
			get { return this.distributorGrid.ConcreteActivation; }
			set { this.distributorGrid.ConcreteActivation = value; }
		}

		public bool Hitherm {
			get { return this.distributorGrid.Hitherm; }
			set { this.distributorGrid.Hitherm = value; }
		}

		public bool HithermCompact {
			get { return this.distributorGrid.HithermCompact; }
			set { this.distributorGrid.HithermCompact = value; }
		}

		public bool ModulKlimaBoden {
			get { return this.distributorGrid.ModulKlimaBoden; }
			set { this.distributorGrid.ModulKlimaBoden = value; }
		}

		public bool ModulKlimaDecke {
			get { return this.distributorGrid.ModulKlimaDecke; }
			set { this.distributorGrid.ModulKlimaDecke = value; }
		}


	}
}
