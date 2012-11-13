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

        private event ProjectChangedHandler projectChanged;
        public event ProjectChangedHandler ProjectChanged {
            add { this.projectChanged += value; }
            remove { this.projectChanged -= value; }
        }

		public QuickDimensioningDistributorsSummary() {
			InitializeComponent();

			this.SetLanguage();

			UpdateControl();
			this.distributorGrid.ProjectChanged += new ProjectChangedHandler(distributorGrid_ProjectChanged);
		}

		private void SetLanguage() {
			this.label1.Text = EuroplanRes.QuickDimensioningDistributorsSummary_Verteiler;
			this.lblRemainingLabel.Text = EuroplanRes.QuickDimensioningDistributorsSummary_VerplanteAnschluesse;
		}

		void distributorGrid_ProjectChanged(object sender) {
			UpdateRemainingConnectors();
			OnProjectChanged();
		}

		private void UpdateRemainingConnectors() {
			Distributor distributor = cmbDistributors.SelectedItem as Distributor;
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
					lblPlanned.Text = EuroplanRes.QuickDimensioningDistributorsSummary_AlleAnschluesseVerplant;
				} else  {
					lblPlanned.Text = EuroplanRes.QuickDimensioningDistributorsSummary_ZuVieleHeizkreise;
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
				List<Distributor> distributors = Project.Instance.QuickDimensioning.Distributors;
				foreach (Distributor distributor in distributors) {
					cmbDistributors.Items.Add(distributor);
				}
			}
			if (oldSelectedItem != null) {
				cmbDistributors.SelectedItem = oldSelectedItem;
			}
			if (cmbDistributors.SelectedItem == null && cmbDistributors.Items.Count > 0) {
				cmbDistributors.SelectedIndex = 0;
			}
			this.distributorGrid.Distributor = cmbDistributors.SelectedItem as Distributor;
			UpdateRemainingConnectors();
		}

		public bool AllowLeave() {
			return true;
		}

		private void OnProjectChanged() {
			if (this.projectChanged != null) {
				this.projectChanged(this);
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

		public bool HithermCompactRoof {
			get { return this.distributorGrid.HithermCompactRoof; }
			set { this.distributorGrid.HithermCompactRoof = value; }
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
			Distributor distributor = this.cmbDistributors.SelectedItem as Distributor;
			this.distributorGrid.Distributor = distributor;
			UpdateRemainingConnectors();
		}
	}
}
