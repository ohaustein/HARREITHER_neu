using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;

namespace Europlan.Common {
	public partial class ConnectionPlannerForm : Form {

		//private Floor floor;
		//private Dictionary<Distributor, Distributor.GraphicalRepresentation> distributors = new Dictionary<Distributor, Distributor.GraphicalRepresentation>();

		private bool changed = false;

		public ConnectionPlannerForm(Floor floor) {
			InitializeComponent();

			this.connectionPlanner1.Floor = floor;

			/*foreach (Distributor distributor in floor.GetAllAvailableDistributors()) {
				foreach (Distributor.GraphicalRepresentation rep in distributor.GraphicalRepresentations) {
					if (rep.floorId == floor.Id) {
						this.distributors.Add(distributor, rep);
					}
				}
			}*/

			this.UpdateToolbar(null);
		}

		private void UpdateControls() {
		}

		private void btnZoomOut_Click(object sender, EventArgs e) {
			this.planPanel.AddScale(0.9, null);
		}

		private void btnZoomIn_Click(object sender, EventArgs e) {
			this.planPanel.AddScale(1.1, null);
		}

		private void btnMove_Click(object sender, EventArgs e) {
			if (!btnMove.Checked) {
				this.connectionPlanner1.Mode = ConnectionPlanner.ConnectionMode.CM_NONE;
				this.planPanel.Mode = PlanMode.PM_MOVE;
				this.UpdateButtons();
			}
		}

		private void UpdateButtons() {
			if (this.planPanel.Mode == PlanMode.PM_MOVE) {
				this.btnMove.Checked = true;
				this.btnConnections.Checked = false;
				this.btnDeleteConnection.Checked = false;
			} else if ((this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.connectionPlanner1.Mode == ConnectionPlanner.ConnectionMode.KDM_ADD_CONNECTION) {
				this.btnMove.Checked = false;
				this.btnConnections.Checked = true;
				this.btnDeleteConnection.Checked = false;
			} else if ((this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.connectionPlanner1.Mode == ConnectionPlanner.ConnectionMode.KDM_DEL_CONNECTION) {
				this.btnMove.Checked = false;
				this.btnConnections.Checked = false;
				this.btnDeleteConnection.Checked = true;
			} else {
				this.btnMove.Checked = false;
			}
		}

		private TabPage previousTab = null;

		private void UpdateToolbar(TabPage tabPage) {
		}

		private void ModulKlimaBodenPlannerForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ConnectionPlannerForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
		}

		private void ModulKlimaBodenPlannerForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ConnectionPlannerForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			SettingsFile.Update();
		}
		public bool Changed {
			get {
				// TODO remove this when the cahnged flag is properly implemented
				return true;
				return this.changed; 
			}
		}

		private int ignoreRandfries = 0;
		private void numRandfries_ValueChanged(object sender, EventArgs e) {
			if (ignoreRandfries == 0) {
				this.changed = true;
				this.planPanel.InvalidateGraphics();
			}
		}

		private void btnConnections_Click(object sender, EventArgs e) {
			this.connectionPlanner1.Mode = ConnectionPlanner.ConnectionMode.KDM_ADD_CONNECTION;
			this.planPanel.Mode = PlanMode.PM_PLANNER_CLICK;
			this.UpdateButtons();
		}

		private void btnDeleteConnection_Click(object sender, EventArgs e) {
			this.connectionPlanner1.Mode = ConnectionPlanner.ConnectionMode.KDM_DEL_CONNECTION;
			this.planPanel.Mode = PlanMode.PM_PLANNER_CLICK;
			this.UpdateButtons();
		}
	}
}