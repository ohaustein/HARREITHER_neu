using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Europlan.Common {
	public partial class QuickDimensioningDistributorsSummary : UserControl {

		public event ProjectChangedHandler ProjectChanged;

		private System.ComponentModel.ComponentResourceManager resources = ResourcesManager.resources;

		public QuickDimensioningDistributorsSummary() {
			InitializeComponent();
			UpdateControl();
			this.distributorGrid.ProjectChanged += new ProjectChangedHandler(distributorGrid_ProjectChanged);
		}

		void distributorGrid_ProjectChanged(object sender) {
			UpdateRemainingConnectors();
			OnProjectChanged();
		}

		private void UpdateRemainingConnectors() {
			QuickDimensioningDistributor distributor = cmbDistributors.SelectedItem as QuickDimensioningDistributor;
			int count = 0;
			if (distributor != null) {
				Project project = Project.Instance;
				if (project != null) {
					foreach (Floor floor in project.Floors) {
						foreach (Room room in floor.Rooms) {
							foreach (Product product in room.UsedProductsForQuickDimensioning) {
								if (product.QuickDimensioningConnectedDistributors.ContainsKey(distributor.Id)) {
									count += product.QuickDimensioningConnectedDistributors[distributor.Id];
								}
							}
						}
					}
				}
				lblRemainingLabel.Visible = true;
				if (count < 12) {
					lblPlanned.Text = "" + count;
				} else if (count == 12) {
					lblPlanned.Text = "Alle Anschlüße des Verteilers sind verplant.";
				} else  {
					lblPlanned.Text = "Dem Verteiler sind zu viele Heizkreise zugeordnet!!!";
				}
			} else {
				lblRemainingLabel.Visible = false;
				lblPlanned.Text = "";
			}
		}

		public void UpdateControl() {
			object oldSelectedItem = cmbDistributors.SelectedItem;
			cmbDistributors.Items.Clear();
			if (Project.Instance != null) {
				List<QuickDimensioningDistributor> distributors = Project.Instance.QuickDimensioning.Distributors;
				if (distributors.Count == 0) {
					QuickDimensioningDistributor distributor = new QuickDimensioningDistributor();
					string localized = resources.GetString("Distributor1", Thread.CurrentThread.CurrentUICulture);
					distributor.Name = localized;
					distributors.Add(distributor);
				}
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
			UpdateRemainingConnectors();
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
			UpdateRemainingConnectors();
		}

		private void btnNewDistributor_Click(object sender, EventArgs e) {
			NewQuickDimensioningDistributorForm form = new NewQuickDimensioningDistributorForm(Project.Instance);
			form.ShowDialog();
			this.OnProjectChanged();
			UpdateControl();
		}


	}
}
