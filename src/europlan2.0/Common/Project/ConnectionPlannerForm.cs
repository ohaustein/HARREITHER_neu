using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;
using WW.Math.Geometry;

namespace Europlan.Common {
	public partial class ConnectionPlannerForm : Form {

		private bool changed = false;

		public ConnectionPlannerForm(Floor floor) {
			InitializeComponent();

			this.SetLanguage();

			this.connectionPlanner.Floor = floor;
            this.btnShowPlanBg.Checked = Product.ShowPlanInBackground;

			this.UpdateToolbar(null);
		}

		private void SetLanguage() {
			this.btnBoden.Text = EuroplanRes.ConnectionPlannerForm_Boden;
			this.btnDecke.Text = EuroplanRes.ConnectionPlannerForm_Decke;
			this.btnZoomOut.Text = EuroplanRes.Plan_Herauszoomen;
			this.btnZoomIn.Text = EuroplanRes.Plan_Heranzoomen;
			this.btnFirstCircuit.Text = EuroplanRes.ConnectionPlannerForm_ErsterHeizkreis;
			this.btnOtherCircuits.Text = EuroplanRes.ConnectionPlannerForm_RestlicheHeizkreise;
			this.btnMove.Text = EuroplanRes.Plan_Verschieben;
			this.btnMove.ToolTipText = EuroplanRes.Plan_Verschieben;
			this.btnConnections.Text = EuroplanRes.ConnectionPlannerForm_AnbindeleitungHinzufuegen;
			this.btnDeleteConnection.Text = EuroplanRes.ConnectionPlannerForm_AnbindeleitungLoeschen;
			this.btnPickConnection.Text = EuroplanRes.ConnectionPlannerForm_AnbindeleitungenAendern;
            this.btnShowPlanBg.Text = EuroplanRes.ProductPlannerForm_PlanImHintergrundAnzeigen;
			this.Text = EuroplanRes.ConnectionPlannerForm_Titel;
		}

		public ConnectionPlannerForm(Product product, bool ceiling) {
			InitializeComponent();

            this.SetLanguage();

			this.connectionPlanner.Product = product;
			this.connectionPlanner.PlanCeiling = ceiling;

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
				this.connectionPlanner.Mode = ConnectionPlanner.ConnectionMode.CM_NONE;
				this.planPanel.Mode = PlanMode.PM_MOVE;
				this.UpdateButtons();
			}
		}

		private void UpdateButtons() {
			if (this.planPanel.Mode == PlanMode.PM_MOVE) {
				this.btnMove.Checked = true;
				this.btnConnections.Checked = false;
				this.btnDeleteConnection.Checked = false;
				this.btnPickConnection.Checked = false;
			} else if ((this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.connectionPlanner.Mode == ConnectionPlanner.ConnectionMode.KDM_ADD_CONNECTION) {
				this.btnMove.Checked = false;
				this.btnConnections.Checked = true;
				this.btnDeleteConnection.Checked = false;
				this.btnPickConnection.Checked = false;
			} else if ((this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.connectionPlanner.Mode == ConnectionPlanner.ConnectionMode.KDM_DEL_CONNECTION) {
				this.btnMove.Checked = false;
				this.btnConnections.Checked = false;
				this.btnDeleteConnection.Checked = true;
				this.btnPickConnection.Checked = false;
			} else if ((this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.connectionPlanner.Mode == ConnectionPlanner.ConnectionMode.KDM_SELECT_CONNECTION) {
				this.btnMove.Checked = false;
				this.btnConnections.Checked = false;
				this.btnDeleteConnection.Checked = false;
				this.btnPickConnection.Checked = true;
			} else {
				this.btnMove.Checked = false;
			}
		}

		private void UpdateToolbar(TabPage tabPage) {
		}

		private void ModulKlimaBodenPlannerForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ConnectionPlannerForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
		}

		private void ModulKlimaBodenPlannerForm_FormClosing(object sender, FormClosingEventArgs e) {
			this.connectionPlanner.ReGenerateConnectionPipes();

			SettingsKey settings = SettingsFile.Settings["ConnectionPlannerForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			SettingsFile.Update();
		}

		public bool Changed {
			get {
				return this.changed || true;
#warning TODO properly implement changed flag
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
			this.connectionPlanner.Mode = ConnectionPlanner.ConnectionMode.KDM_ADD_CONNECTION;
			this.planPanel.Mode = PlanMode.PM_PLANNER_CLICK;
			this.UpdateButtons();
		}

		private void btnDeleteConnection_Click(object sender, EventArgs e) {
			this.connectionPlanner.Mode = ConnectionPlanner.ConnectionMode.KDM_DEL_CONNECTION;
			this.planPanel.Mode = PlanMode.PM_PLANNER_CLICK;
			this.UpdateButtons();
		}

		private void btnPickConnection_Click(object sender, EventArgs e) {
			this.connectionPlanner.Mode = ConnectionPlanner.ConnectionMode.KDM_SELECT_CONNECTION;
			this.planPanel.Mode = PlanMode.PM_PLANNER_DRAG;
			this.UpdateButtons();
		}

		private void btnBoden_Click(object sender, EventArgs e) {
			this.btnBoden.Checked = true;
			this.btnDecke.Checked = false;
			this.connectionPlanner.PlanFloor = true;
		}

		private void btnDecke_Click(object sender, EventArgs e) {
			this.btnBoden.Checked = false;
			this.btnDecke.Checked = true;
			this.connectionPlanner.PlanCeiling = true;
		}

		private void btnCircuits_Click(object sender, EventArgs e) {
			if (!this.btnFirstCircuit.Checked && !this.btnOtherCircuits.Checked) {
				if (sender == this.btnFirstCircuit) {
					this.btnOtherCircuits.Checked = true;
				} else {
					this.btnFirstCircuit.Checked = true;
				}
			}
			this.connectionPlanner.AddFirstCircuit = this.btnFirstCircuit.Checked;
			this.connectionPlanner.AddOtherCircuits = this.btnOtherCircuits.Checked;
		}

        private void btnShowPlanBg_Click(object sender, EventArgs e) {
            Product.ShowPlanInBackground = !Product.ShowPlanInBackground;
            this.btnShowPlanBg.Checked = Product.ShowPlanInBackground;
            this.planPanel.InvalidateGraphics();
        }
	}
}