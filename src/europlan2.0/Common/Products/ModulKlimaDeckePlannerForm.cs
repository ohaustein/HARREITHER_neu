using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common.Products {
	public partial class ModulKlimaDeckePlannerForm : Form {

		private ModulKlimaDeckeConstructionGlatt glatt = null;
		private ModulKlimaDeckeConstructionAkustik akustik = null;

		private int ignoreRotation = 0;

		public ModulKlimaDeckePlannerForm(ModulKlimaDeckeProduct product) {
			InitializeComponent();
			this.modulKlimaBodenPlanner.Product = product;
			if (product.GraphConstruction == null) {
				product.GraphConstruction = new ModulKlimaDeckeConstructionGlatt();
				product.GraphConstruction.Planner = this.modulKlimaBodenPlanner;
			}
			this.glatt = product.GraphConstruction as ModulKlimaDeckeConstructionGlatt;
			this.akustik = product.GraphConstruction as ModulKlimaDeckeConstructionAkustik;
			if (this.glatt == null) {
				this.glatt = new ModulKlimaDeckeConstructionGlatt();
			}
			this.glatt.Planner = this.modulKlimaBodenPlanner;
			if (this.akustik == null) {
				this.akustik = new ModulKlimaDeckeConstructionAkustik();
			}
			this.akustik.Planner = this.modulKlimaBodenPlanner;

			this.UpdateControls();
		}

		private void UpdateControls() {
			this.rbGlatt.Checked = this.modulKlimaBodenPlanner.Product.GraphConstruction is ModulKlimaDeckeConstructionGlatt;
			this.rbAkustik.Checked = this.modulKlimaBodenPlanner.Product.GraphConstruction is ModulKlimaDeckeConstructionAkustik;
			this.rbKassetten.Checked = false; //this.modulKlimaBodenPlanner.Product.GraphConstruction is ModulKlimaDeckeConstructionKassetten;
			if (this.rbGlatt.Checked) {
				ignoreRotation++;
				this.lblRandfries.Visible = false;
				this.numRandfries.Visible = false;
				this.lblRandfriesUnit.Visible = false;
				ModulKlimaDeckeConstructionGlatt glatt = this.modulKlimaBodenPlanner.Product.GraphConstruction as ModulKlimaDeckeConstructionGlatt;
				this.numRotation.Value = (decimal)glatt.RotationRelativeToPlan;
				ignoreRotation--;
			} else if (this.rbAkustik.Checked) {
			} else if (this.rbKassetten.Checked) {
			} else {
			}
		}

		private void btnZoomOut_Click(object sender, EventArgs e) {
			this.planPanel.AddScale(0.9, null);
		}

		private void btnZoomIn_Click(object sender, EventArgs e) {
			this.planPanel.AddScale(1.1, null);
		}

		private void btnMove_Click(object sender, EventArgs e) {
			if (!btnMove.Checked) {
				this.modulKlimaBodenPlanner.Mode = ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_NONE;
				this.planPanel.Mode = PlanMode.PM_MOVE;
				this.UpdateButtons();
			}
		}

		private void btnConstruction_Click(object sender, EventArgs e) {
			if (!btnConstruction.Checked) {
				this.modulKlimaBodenPlanner.Mode = ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_CONSTRUCTION;
				this.planPanel.Mode = PlanMode.PM_PLANNER_DRAG;
				this.UpdateButtons();
			}
		}

		private void UpdateButtons() {
			if (this.planPanel.Mode == PlanMode.PM_MOVE) {
				this.btnMove.Checked = true;
				this.btnConstruction.Checked = false;
			} else if ((this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.modulKlimaBodenPlanner.Mode == ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_CONSTRUCTION) {
				this.btnMove.Checked = false;
				this.btnConstruction.Checked = true;
			} else {
				this.btnMove.Checked = false;
				this.btnConstruction.Checked = false;
			}
		}

		private void rbGlatt_CheckedChanged(object sender, EventArgs e) {
			if (rbGlatt.Checked) {
				this.modulKlimaBodenPlanner.Product.GraphConstruction = this.glatt;
				this.planPanel.InvalidateGraphics();
			}
		}

		private void rbAkustik_CheckedChanged(object sender, EventArgs e) {
			if (rbAkustik.Checked) {
				this.modulKlimaBodenPlanner.Product.GraphConstruction = this.akustik;
				this.planPanel.InvalidateGraphics();
			}
		}

		private void numAusrichtung_ValueChanged(object sender, EventArgs e) {
			if (ignoreRotation == 0) {
				if (this.modulKlimaBodenPlanner.Product.GraphConstruction is ModulKlimaDeckeConstructionGlatt) {
					ModulKlimaDeckeConstructionGlatt glatt = this.modulKlimaBodenPlanner.Product.GraphConstruction as ModulKlimaDeckeConstructionGlatt;
					glatt.RotationRelativeToPlan = (double)this.numRotation.Value;
					this.planPanel.InvalidateGraphics();
				} else if (this.modulKlimaBodenPlanner.Product.GraphConstruction is ModulKlimaDeckeConstructionGlatt) {
				}
			}
		}

		private decimal smallRotate = (decimal)0.5;
		private decimal largeRotate = 5;

		private void btnRotate_Click(object sender, EventArgs e) {
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
				value += 180;
			}
			while (value >= 180) {
				value -= 180;
			}
			this.numRotation.Value = value;
		}

		private void btnHorizontal_Click(object sender, EventArgs e) {
			this.numRotation.Value = 90;
		}

		private void btnVertical_Click(object sender, EventArgs e) {
			this.numRotation.Value = 0;
		}
	}
}