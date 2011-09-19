 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;
using System.Drawing.Drawing2D;
using WW.Cad.IO;
using WW.Cad.Model.Tables;
using WW.Math;
using WW.Cad.Model;

namespace Europlan.Common {

	public partial class DistributorPositionerForm : Form {

		private bool unsavedChanges = false;

		private class LayerListViewItem : ListViewItem {

			private DxfLayer layer = null;

			public LayerListViewItem(DxfLayer layer) : base(layer.Name) {
				this.layer = layer;
				this.Checked = layer.Enabled;
			}

			public DxfLayer Layer {
				get { return this.layer; }
				set { this.layer = value; }
			}
		}

		public DistributorPositionerForm(Distributor distributor, Floor floor) {
			InitializeComponent();
			this.SetLanguage();
			this.distributorPositioner.Distributor = distributor;
			this.distributorPositioner.Floor = floor;
			foreach (Plan p in Project.Instance.ImportedPlans) {
				if (p.Id == floor.AssociatedPlanId) {
					this.panel.Plan = p;
					break;
				}
			}			
			this.panel.Mode = PlanMode.PM_MOVE;

			this.UpdateButtons();
		}

		public PlanPanel Panel {
			get {
				return this.panel;
			}
		}

		private void SetLanguage() {
			this.Text = EuroplanRes.DistributorPositionerForm_Titel;
			this.btnZoomIn.Text = EuroplanRes.Plan_Heranzoomen;
			this.btnZoomOut.Text = EuroplanRes.Plan_Herauszoomen;
			this.btnMove.Text = EuroplanRes.Plan_Verschieben;
			this.btnPosition.Text = EuroplanRes.DistributorPositionerForm_Positionieren;
			this.lblRotation.Text = EuroplanRes.DistributorPositionerForm_Ausrichtung;
			this.btnOk.Text = EuroplanRes.General_Uebernehmen;
		}

		private void DistributionPositionerForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["DistributionPositionerForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			SettingsFile.Update();
		}

		private void DistributionPositionerForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["DistributionPositionerForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
		}

		public bool UnsavedChanges {
			get { return this.unsavedChanges || this.panel.UnsavedChanges || this.distributorPositioner.UnsavedChanges; }
		}

		private void btnZoomIn_Click(object sender, EventArgs e) {
			this.panel.AddScale(1.1, null);
		}

		private void btnZoomOut_Click(object sender, EventArgs e) {
			this.panel.AddScale(0.9, null);
		}

		private void btnMove_Click(object sender, EventArgs e) {
			if (!btnMove.Checked) {
				this.distributorPositioner.Mode = DistributorPositioner.DistributorPositionerMode.DPM_NONE;
				this.panel.Mode = PlanMode.PM_MOVE;
				this.UpdateButtons();
			}
		}

		private void btnPosition_Click(object sender, EventArgs e) {
			if (!btnPosition.Checked) {
				if (this.distributorPositioner.Distributor.AreProductsConnected) {
					if (MessageBox.Show("An diesen Verteiler sind bereits grafische Anbindeleitunge angeschlossen. Wenn sie die Position des Verteilers ändern wollen, werden diese Anbindeleitungen gelöscht!", "Anbindeleitungen löschen", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK) {
						return;
					}
					foreach (Floor f in Project.Instance.Floors) {
						foreach (Room r in f.Rooms) {
							foreach (PlannedProduct pp in r.PlannedProducts) {
								List<GraphicalProductConnection> connectionsToDelete = new List<GraphicalProductConnection>();
								foreach (GraphicalProductConnection c in pp.Product.Connections) {
									connectionsToDelete.Add(c);
								}
								foreach (GraphicalProductConnection c in connectionsToDelete) {
									pp.Product.Connections.Remove(c);
								}
							}
						}
					}
				}
				this.distributorPositioner.Mode = DistributorPositioner.DistributorPositionerMode.DPM_POSITION;
				this.panel.Mode = PlanMode.PM_SET_DISTRIBUTOR;
				this.UpdateButtons();
			}
		}

		private void UpdateButtons() {
			if (this.panel.Mode == PlanMode.PM_MOVE) {
				this.btnMove.Checked = true;
				this.btnPosition.Checked = false;
				this.btnCcwLarge.Enabled = false;
				this.btnCcwSmall.Enabled = false;
				this.btnCwLarge.Enabled = false;
				this.btnCwSmall.Enabled = false;
				this.numRotation.Enabled = false;
			} else {
				this.btnMove.Checked = false;
				this.btnPosition.Checked = true;
				this.btnCcwLarge.Enabled = true;
				this.btnCcwSmall.Enabled = true;
				this.btnCwLarge.Enabled = true;
				this.btnCwSmall.Enabled = true;
				this.numRotation.Enabled = true;
			}
		}

		private void btnOk_Click(object sender, EventArgs e) {
			this.Close();
		}

		private decimal smallRotate = (decimal)0.5;
		private decimal largeRotate = 5;

		private void btnRotate_Click(object sender, EventArgs e) {
			this.unsavedChanges = true;
			decimal rotation = 0;
			if (sender == this.btnCcwLarge) {
				rotation = -largeRotate;
			} else if (sender == this.btnCcwSmall) {
				rotation = -smallRotate;
			} else if (sender == btnCwLarge) {
				rotation = largeRotate;
			} else if (sender == btnCwSmall) {
				rotation = smallRotate;
			}
			decimal value = this.numRotation.Value + rotation;
			while (value < 0) {
				value += 360;
			}
			while (value >= 360) {
				value -= 360;
			}
			this.numRotation.Value = value;
		}

		private void numRotation_ValueChanged(object sender, EventArgs e) {
			this.distributorPositioner.Rotation = (double)this.numRotation.Value;
			this.panel.InvalidateGraphics();
		}

		private void distributorPositioner_ModeChanged(object sender, EventArgs e) {
			this.UpdateButtons();
		}

		public bool DrawOtherDistributorsInPlan {
			get { return this.distributorPositioner.DrawOtherDistributorsInPlan; }
			set { this.distributorPositioner.DrawOtherDistributorsInPlan = value; }
		}

	}
}