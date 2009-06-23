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
			object oldSelectedItem = cmbDistributors.SelectedItem;
			cmbDistributors.Items.Clear();
			if (Project.Instance != null) {
				List<QuickDimensioningDistributor> distributors = Project.Instance.QuickDimensioning.Distributors;
				foreach (QuickDimensioningDistributor distributor in distributors) {
					cmbDistributors.Items.Add(distributor);
				}
			}
			if (oldSelectedItem != null) {
				cmbDistributors.SelectedItem = oldSelectedItem;
			}
			if (cmbDistributors.SelectedItem == null && cmbDistributors.Items.Count > 0) {
				cmbDistributors.SelectedIndex = 0;
			}
			this.distributorGrid.Distributor = cmbDistributors.SelectedItem as QuickDimensioningDistributor;
		}

		public bool AllowLeave() {
			return true;
		}

		private void OnProjectChanged() {
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
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

		private void cmbDistributors_SelectedIndexChanged(object sender, EventArgs e) {
			QuickDimensioningDistributor distributor = this.cmbDistributors.SelectedItem as QuickDimensioningDistributor;
			this.distributorGrid.Distributor = distributor;
		}

		private void btnNewDistributor_Click(object sender, EventArgs e) {
			NewQuickDimensioningDistributorForm form = new NewQuickDimensioningDistributorForm(Project.Instance);
			form.ShowDialog();
			this.OnProjectChanged();
			UpdateControl();
		}


	}
}
