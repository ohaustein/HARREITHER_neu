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
		}

		public void UpdateControl() {
			listDistributors.Clear();
			if (Project.Instance != null) {
				List<QuickDimensioningDistributor> distributors = Project.Instance.QuickDimensioning.Distributors;
				foreach (QuickDimensioningDistributor distributor in distributors) {
					ListViewItem item = new ListViewItem(distributor.Name);
					item.Tag = distributor;
					listDistributors.Items.Add(item);
				}
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

		}

	}
}
