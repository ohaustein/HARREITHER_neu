using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace Europlan.Common {
	public partial class QuickDimensioningPanel : UserControl, IEditorUserControl {

		public event ProjectStructureChangedHandler ProjectStructureChanged;
		public event ProjectChangedHandler ProjectChanged;
		public event TreeSelectionRequestedHandler TreeSelectionRequested;

		private Dictionary<Floor, QuickDimensioningFloorGrid> grids = new Dictionary<Floor, QuickDimensioningFloorGrid>();

		public QuickDimensioningPanel() {
			InitializeComponent();
		}

		public void UpdateControl() {
			//Project.Instance.Config.

			this.cbEurovalHeat.Checked = ((Project.Instance.QuickDimensioning.EurovalCheckState & QuickDimensioning.ProductCheckState.Heat) == QuickDimensioning.ProductCheckState.Heat);
			this.cbBkaHeat.Checked = ((Project.Instance.QuickDimensioning.ConcreteActivationCheckState & QuickDimensioning.ProductCheckState.Heat) == QuickDimensioning.ProductCheckState.Heat);
			this.cbBkaCool.Checked = ((Project.Instance.QuickDimensioning.ConcreteActivationCheckState & QuickDimensioning.ProductCheckState.Cool) == QuickDimensioning.ProductCheckState.Cool);
			this.cbHithermHeat.Checked = ((Project.Instance.QuickDimensioning.HithermCheckState & QuickDimensioning.ProductCheckState.Heat) == QuickDimensioning.ProductCheckState.Heat);
			this.cbHithermCompactHeat.Checked = ((Project.Instance.QuickDimensioning.HithermCompactCheckState & QuickDimensioning.ProductCheckState.Heat) == QuickDimensioning.ProductCheckState.Heat);
			this.cbModulKlimaBodenHeat.Checked = ((Project.Instance.QuickDimensioning.ModulBodenCheckState & QuickDimensioning.ProductCheckState.Heat) == QuickDimensioning.ProductCheckState.Heat);
			this.cbModulKlimaDeckeHeat.Checked = ((Project.Instance.QuickDimensioning.ModulDeckeCheckState & QuickDimensioning.ProductCheckState.Heat) == QuickDimensioning.ProductCheckState.Heat);
			this.cbModulKlimaDeckeCool.Checked = ((Project.Instance.QuickDimensioning.ModulDeckeCheckState & QuickDimensioning.ProductCheckState.Cool) == QuickDimensioning.ProductCheckState.Cool);

			this.lblTemp1.Visible = this.EurovalHeating;
			this.txtTemperature.Visible = this.EurovalHeating;
			this.lblTemp2.Visible = this.EurovalHeating;
			this.cmbDistance.Visible = this.EurovalHeating;
			this.lblDistance.Visible = this.EurovalHeating;

			this.lblAllocation.Visible = this.ModulKlimaDeckeHeating || this.ModulKlimaDeckeCooling;
			this.lblAllocation2.Visible = this.ModulKlimaDeckeHeating || this.ModulKlimaDeckeCooling;
			this.txtAllocation.Visible = this.ModulKlimaDeckeHeating || this.ModulKlimaDeckeCooling;

			this.txtTemperature.Text = Project.Instance.QuickDimensioning.FlowTemperature.ToString();
			this.cmbDistance.SelectedIndex = (int)Project.Instance.QuickDimensioning.LayDistance;
			this.txtAllocation.Text = Project.Instance.QuickDimensioning.CeilingAllocation.ToString();

			this.grids.Clear();

			List<TabPage> pagesToRemove = new List<TabPage>();
			foreach (TabPage page in this.tabQuickDimensioning.TabPages) {
				if (page != this.pageSettings) {
					pagesToRemove.Add(page);
				}
			}
			foreach (TabPage page in pagesToRemove) {
				this.tabQuickDimensioning.TabPages.Remove(page);
			}

			foreach (Floor floor in Project.Instance.Floors) {
				TabPage page = new TabPage(floor.Name);
				page.UseVisualStyleBackColor = true;
				QuickDimensioningFloorGrid grid = new QuickDimensioningFloorGrid();
				grid.Euroval = this.EurovalHeating;
				grid.ConcreteActivation = this.ConcreteActivationHeating || this.ConcreteActivationCooling;
				grid.Hitherm = this.HithermHeating;
				grid.HithermCompact = this.HithermCompactHeating;
				grid.ModulKlimaBoden = this.ModulKlimaBodenHeating;
				grid.ModulKlimaDecke = this.ModulKlimaDeckeHeating || this.ModulKlimaDeckeCooling;
				grid.Cooling = this.ModulKlimaDeckeCooling || this.ConcreteActivationCooling;
				grid.Floor = floor;
				page.Controls.Add(grid);
				grid.Dock = DockStyle.Fill;
				this.tabQuickDimensioning.TabPages.Add(page);
				this.grids.Add(floor, grid);
			}
		}

		public bool AllowLeave() {
			return true;
		}

		public bool EurovalHeating {
			get { return this.cbEurovalHeat.Checked; }
			set { this.cbEurovalHeat.Checked = value; }
		}

		public bool ConcreteActivationHeating {
			get { return this.cbBkaHeat.Checked; }
			set { this.cbBkaHeat.Checked = value; }
		}

		public bool ConcreteActivationCooling {
			get { return this.cbBkaCool.Checked; }
			set { this.cbBkaCool.Checked = value; }
		}

		public bool HithermHeating {
			get { return this.cbHithermHeat.Checked; }
			set { this.cbHithermHeat.Checked = value; }
		}

		public bool HithermCompactHeating {
			get { return this.cbHithermCompactHeat.Checked; }
			set { this.cbHithermCompactHeat.Checked = value; }
		}

		public bool ModulKlimaBodenHeating {
			get { return this.cbModulKlimaBodenHeat.Checked; }
			set { this.cbModulKlimaBodenHeat.Checked = value; }
		}

		public bool ModulKlimaDeckeHeating {
			get { return this.cbModulKlimaDeckeHeat.Checked; }
			set { this.cbModulKlimaDeckeHeat.Checked = value; }
		}

		public bool ModulKlimaDeckeCooling {
			get { return this.cbModulKlimaDeckeCool.Checked; }
			set { this.cbModulKlimaDeckeCool.Checked = value; }
		}

		private void cbEurovalHeat_CheckedChanged(object sender, EventArgs e) {
			DialogResult result = DialogResult.None;
			if (!this.EurovalHeating) {
				bool productFound = false;
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						if (room.GetProductForQuickDimensioning<EurovalProduct>() != null) {
							productFound = true;
						}
					}
				}
				if (productFound) {
					result = MessageBox.Show("Wollen sie das Produkt Euroval wirklich aus der Flächenaufstellung entfernen?", "Euroval entfernen", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				}
			}
			if (result == DialogResult.No) {
				this.EurovalHeating = !this.EurovalHeating;
			} else {
				foreach (QuickDimensioningFloorGrid grid in this.grids.Values) {
					grid.Euroval = this.EurovalHeating;
				}
				if (result == DialogResult.Yes) {
					foreach (Floor floor in Project.Instance.Floors) {
						foreach (Room room in floor.Rooms) {
							EurovalProduct product = room.GetProductForQuickDimensioning<EurovalProduct>();
							if (product != null) {
								room.UsedProductsForQuickDimensioning.Remove(product);
							}
						}
					}
				}
			}
			this.lblTemp1.Visible = this.EurovalHeating || this.ConcreteActivationHeating;
			this.txtTemperature.Visible = this.EurovalHeating || this.ConcreteActivationHeating;
			this.lblTemp2.Visible = this.EurovalHeating || this.ConcreteActivationHeating;
			this.cmbDistance.Visible = this.EurovalHeating || this.ConcreteActivationHeating;
			this.lblDistance.Visible = this.EurovalHeating || this.ConcreteActivationHeating;
			Project.Instance.QuickDimensioning.EurovalCheckState = (this.EurovalHeating ? QuickDimensioning.ProductCheckState.Heat : QuickDimensioning.ProductCheckState.None);
		}

		private void cbBkaHeat_CheckedChanged(object sender, EventArgs e) {
			DialogResult result = DialogResult.None;
			if (!this.ConcreteActivationHeating && !this.ConcreteActivationCooling) {
				bool productFound = false;
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						if (room.GetProductForQuickDimensioning<ConcreteActivationProduct>() != null) {
							productFound = true;
						}
					}
				}
				if (productFound) {
					result = MessageBox.Show("Wollen sie das Produkt Betonkernaktivierung wirklich aus der Flächenaufstellung entfernen?", "Betonkernaktivierung entfernen", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				}
			}
			if (result == DialogResult.No) {
				this.ConcreteActivationHeating = !this.ConcreteActivationHeating;
			} else {
				foreach (QuickDimensioningFloorGrid grid in this.grids.Values) {
					grid.ConcreteActivation = this.ConcreteActivationHeating || this.ConcreteActivationCooling;
				}
				if (result == DialogResult.Yes) {
					foreach (Floor floor in Project.Instance.Floors) {
						foreach (Room room in floor.Rooms) {
							ConcreteActivationProduct product = room.GetProductForQuickDimensioning<ConcreteActivationProduct>();
							if (product != null) {
								room.UsedProductsForQuickDimensioning.Remove(product);
							}
						}
					}
				}
			}

			this.lblTemp1.Visible = this.EurovalHeating || this.ConcreteActivationHeating;
			this.txtTemperature.Visible = this.EurovalHeating || this.ConcreteActivationHeating;
			this.lblTemp2.Visible = this.EurovalHeating || this.ConcreteActivationHeating;
			this.cmbDistance.Visible = this.EurovalHeating || this.ConcreteActivationHeating;
			this.lblDistance.Visible = this.EurovalHeating || this.ConcreteActivationHeating;
			Project.Instance.QuickDimensioning.ConcreteActivationCheckState = (this.ConcreteActivationHeating ? QuickDimensioning.ProductCheckState.Heat : QuickDimensioning.ProductCheckState.None) | (this.ConcreteActivationCooling ? QuickDimensioning.ProductCheckState.Cool : QuickDimensioning.ProductCheckState.None);
		}

		private void cbBkaCool_CheckedChanged(object sender, EventArgs e) {
			DialogResult result = DialogResult.None;
			if (!this.ConcreteActivationHeating && !this.ConcreteActivationCooling) {
				bool productFound = false;
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						if (room.GetProductForQuickDimensioning<ConcreteActivationProduct>() != null) {
							productFound = true;
						}
					}
				}
				if (productFound) {
					result = MessageBox.Show("Wollen sie das Produkt Betonkernaktivierung wirklich aus der Flächenaufstellung entfernen?", "Betonkernaktivierung entfernen", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				}
			}
			if (result == DialogResult.No) {
				this.ConcreteActivationCooling = !this.ConcreteActivationCooling;
			} else {
				foreach (QuickDimensioningFloorGrid grid in this.grids.Values) {
					grid.ConcreteActivation = this.ConcreteActivationHeating || this.ConcreteActivationCooling;
					grid.Cooling = this.ModulKlimaDeckeCooling | this.ConcreteActivationCooling;
				}
				if (result == DialogResult.Yes) {
					foreach (Floor floor in Project.Instance.Floors) {
						foreach (Room room in floor.Rooms) {
							ConcreteActivationProduct product = room.GetProductForQuickDimensioning<ConcreteActivationProduct>();
							if (product != null) {
								room.UsedProductsForQuickDimensioning.Remove(product);
							}
						}
					}
				}
			}

			Project.Instance.QuickDimensioning.ConcreteActivationCheckState = (this.ConcreteActivationHeating ? QuickDimensioning.ProductCheckState.Heat : QuickDimensioning.ProductCheckState.None) | (this.ConcreteActivationCooling ? QuickDimensioning.ProductCheckState.Cool : QuickDimensioning.ProductCheckState.None);
		}

		private void cbHithermHeat_CheckedChanged(object sender, EventArgs e) {
			DialogResult result = DialogResult.None;
			if (!this.HithermHeating) {
				bool productFound = false;
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						if (room.GetProductForQuickDimensioning<HithermProduct>() != null) {
							productFound = true;
						}
					}
				}
				if (productFound) {
					result = MessageBox.Show("Wollen sie das Produkt Hitherm wirklich aus der Flächenaufstellung entfernen?", "Hitherm entfernen", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				}
			}
			if (result == DialogResult.No) {
				this.HithermHeating = !this.HithermHeating;
			} else {
				foreach (QuickDimensioningFloorGrid grid in this.grids.Values) {
					grid.Hitherm = this.HithermHeating;
				}
				if (result == DialogResult.Yes) {
					foreach (Floor floor in Project.Instance.Floors) {
						foreach (Room room in floor.Rooms) {
							HithermProduct product = room.GetProductForQuickDimensioning<HithermProduct>();
							if (product != null) {
								room.UsedProductsForQuickDimensioning.Remove(product);
							}
						}
					}
				}
			}
			Project.Instance.QuickDimensioning.HithermCheckState = (this.HithermHeating ? QuickDimensioning.ProductCheckState.Heat : QuickDimensioning.ProductCheckState.None);
		}

		private void cbHithermCompactHeat_CheckedChanged(object sender, EventArgs e) {
			DialogResult result = DialogResult.None;
			if (!this.HithermCompactHeating) {
				bool productFound = false;
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						if (room.GetProductForQuickDimensioning<HithermCompactProduct>() != null) {
							productFound = true;
						}
					}
				}
				if (productFound) {
					result = MessageBox.Show("Wollen sie das Produkt Hitherm Compact wirklich aus der Flächenaufstellung entfernen?", "Hitherm Compact entfernen", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				}
			}
			if (result == DialogResult.No) {
				this.HithermCompactHeating = !this.HithermCompactHeating;
			} else {
				foreach (QuickDimensioningFloorGrid grid in this.grids.Values) {
					grid.HithermCompact = this.HithermCompactHeating;
				}
				if (result == DialogResult.Yes) {
					foreach (Floor floor in Project.Instance.Floors) {
						foreach (Room room in floor.Rooms) {
							HithermCompactProduct product = room.GetProductForQuickDimensioning<HithermCompactProduct>();
							if (product != null) {
								room.UsedProductsForQuickDimensioning.Remove(product);
							}
						}
					}
				}
			}
			Project.Instance.QuickDimensioning.HithermCompactCheckState = (this.HithermCompactHeating ? QuickDimensioning.ProductCheckState.Heat : QuickDimensioning.ProductCheckState.None);
		}

		private void cbModulKlimaBodenHeat_CheckedChanged(object sender, EventArgs e) {
			DialogResult result = DialogResult.None;
			if (!this.ModulKlimaBodenHeating) {
				bool productFound = false;
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						if (room.GetProductForQuickDimensioning<ModulKlimaBodenProduct>() != null) {
							productFound = true;
						}
					}
				}
				if (productFound) {
					result = MessageBox.Show("Wollen sie das Produkt Modul Klimaboden wirklich aus der Flächenaufstellung entfernen?", "Modul Klimaboden entfernen", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				}
			}
			if (result == DialogResult.No) {
				this.ModulKlimaBodenHeating = !this.ModulKlimaBodenHeating;
			} else {
				foreach (QuickDimensioningFloorGrid grid in this.grids.Values) {
					grid.ModulKlimaBoden = this.ModulKlimaBodenHeating;
				}
				if (result == DialogResult.Yes) {
					foreach (Floor floor in Project.Instance.Floors) {
						foreach (Room room in floor.Rooms) {
							ModulKlimaBodenProduct product = room.GetProductForQuickDimensioning<ModulKlimaBodenProduct>();
							if (product != null) {
								room.UsedProductsForQuickDimensioning.Remove(product);
							}
						}
					}
				}
			}
			Project.Instance.QuickDimensioning.ModulBodenCheckState = (this.ModulKlimaBodenHeating ? QuickDimensioning.ProductCheckState.Heat : QuickDimensioning.ProductCheckState.None);
		}

		private void cbModulKlimaDeckeHeat_CheckedChanged(object sender, EventArgs e) {
			DialogResult result = DialogResult.None;
			if (!this.ModulKlimaDeckeHeating && !this.ModulKlimaDeckeCooling) {
				bool productFound = false;
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						if (room.GetProductForQuickDimensioning<ModulKlimaDeckeProduct>() != null) {
							productFound = true;
						}
					}
				}
				if (productFound) {
					result = MessageBox.Show("Wollen sie das Produkt Modul Klimadecke wirklich aus der Flächenaufstellung entfernen?", "Modul Klimadecke entfernen", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				}
			}
			if (result == DialogResult.No) {
				this.ModulKlimaDeckeHeating = !this.ModulKlimaDeckeHeating;
			} else {
				foreach (QuickDimensioningFloorGrid grid in this.grids.Values) {
					grid.ModulKlimaDecke = this.ModulKlimaDeckeHeating || this.ModulKlimaDeckeCooling;
				}
				if (result == DialogResult.Yes) {
					foreach (Floor floor in Project.Instance.Floors) {
						foreach (Room room in floor.Rooms) {
							ModulKlimaDeckeProduct product = room.GetProductForQuickDimensioning<ModulKlimaDeckeProduct>();
							if (product != null) {
								room.UsedProductsForQuickDimensioning.Remove(product);
							}
						}
					}
				}
			}
			this.lblAllocation.Visible = this.ModulKlimaDeckeHeating || this.ModulKlimaDeckeCooling;
			this.lblAllocation2.Visible = this.ModulKlimaDeckeHeating || this.ModulKlimaDeckeCooling;
			this.txtAllocation.Visible = this.ModulKlimaDeckeHeating || this.ModulKlimaDeckeCooling;
			Project.Instance.QuickDimensioning.ModulDeckeCheckState = (this.ModulKlimaDeckeHeating ? QuickDimensioning.ProductCheckState.Heat : QuickDimensioning.ProductCheckState.None) | (this.ModulKlimaDeckeCooling ? QuickDimensioning.ProductCheckState.Cool : QuickDimensioning.ProductCheckState.None);
		}

		private void cbModulKlimaDeckeCool_CheckedChanged(object sender, EventArgs e) {
			DialogResult result = DialogResult.None;
			if (!this.ModulKlimaDeckeHeating && !this.ModulKlimaDeckeCooling) {
				bool productFound = false;
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						if (room.GetProductForQuickDimensioning<ModulKlimaDeckeProduct>() != null) {
							productFound = true;
						}
					}
				}
				if (productFound) {
					result = MessageBox.Show("Wollen sie das Produkt Modul Klimadecke wirklich aus der Flächenaufstellung entfernen?", "Modul Klimadecke entfernen", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				}
			}
			if (result == DialogResult.No) {
				this.ModulKlimaDeckeCooling = !this.ModulKlimaDeckeCooling;
			} else {
				foreach (QuickDimensioningFloorGrid grid in this.grids.Values) {
					grid.ModulKlimaDecke = this.ModulKlimaDeckeHeating || this.ModulKlimaDeckeCooling;
					grid.Cooling = this.ModulKlimaDeckeCooling | this.ConcreteActivationCooling;
				}
				if (result == DialogResult.Yes) {
					foreach (Floor floor in Project.Instance.Floors) {
						foreach (Room room in floor.Rooms) {
							ModulKlimaDeckeProduct product = room.GetProductForQuickDimensioning<ModulKlimaDeckeProduct>();
							if (product != null) {
								room.UsedProductsForQuickDimensioning.Remove(product);
							}
						}
					}
				}
			}
			this.lblAllocation.Visible = this.ModulKlimaDeckeHeating || this.ModulKlimaDeckeCooling;
			this.lblAllocation2.Visible = this.ModulKlimaDeckeHeating || this.ModulKlimaDeckeCooling;
			this.txtAllocation.Visible = this.ModulKlimaDeckeHeating || this.ModulKlimaDeckeCooling;
			Project.Instance.QuickDimensioning.ModulDeckeCheckState = (this.ModulKlimaDeckeHeating ? QuickDimensioning.ProductCheckState.Heat : QuickDimensioning.ProductCheckState.None) | (this.ModulKlimaDeckeCooling ? QuickDimensioning.ProductCheckState.Cool : QuickDimensioning.ProductCheckState.None);
		}

		//private void txtAllocation_Validating(object sender, CancelEventArgs e) {
		//    float percent = 0;
		//    if (float.TryParse(txtAllocation.Text, out percent)) {
		//        if (!(percent >= 0) || !(percent <= 100)) {
		//            DialogResult result = MessageBox.Show("Der Belegefaktor muss zwischen 1% und 100% liegen.\nDrücken Sie Ja, um den Belegefaktor zu korrigieren oder\nNein, um den Standardwert einzutragen", "Ungültiger Belegefaktor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
		//            if (result.Equals(DialogResult.Yes)) {
		//                e.Cancel = true;
		//            } else {
		//                txtAllocation.Text = "100";
		//            }
		//        }
		//    } else {
		//        e.Cancel = true;
		//    }
		//}


		private void txtTemperature_ValueChanged(object sender, EventArgs e) {
			float temperature = (float)this.txtTemperature.Value;
			Project.Instance.QuickDimensioning.FlowTemperature = temperature;
			if (temperature <= 31.25) {
				this.cmbDistance.SelectedIndex = (int)EurovalProduct.LayDistance.EV5;
			} else if (temperature > 31.25 && temperature <= 33.75) {
				this.cmbDistance.SelectedIndex = (int)EurovalProduct.LayDistance.EV15;
			} else if (temperature > 33.75 && temperature <= 36.25) {
				this.cmbDistance.SelectedIndex = (int)EurovalProduct.LayDistance.EV20;
			} else if (temperature > 36.25) {
				this.cmbDistance.SelectedIndex = (int)EurovalProduct.LayDistance.EV25;
			}
		}

		private void txtAllocation_ValueChanged(object sender, EventArgs e) {
			float allocation = (float)this.txtAllocation.Value;
			Project.Instance.QuickDimensioning.CeilingAllocation = allocation;
		}

		private void cmbDistance_SelectedIndexChanged(object sender, EventArgs e) {
			Project.Instance.QuickDimensioning.LayDistance = (EurovalProduct.LayDistance)this.cmbDistance.SelectedIndex;
		}

		//private void txtAllocation_ValueChanged(object sender, EventArgs e) {
		//    int percent = (int)this.txtAllocation.Value;
		//    if (!(percent > 0) || !(percent <= 100)) {
		//        DialogResult result = MessageBox.Show("Der Belegefaktor muss zwischen 1% und 100% liegen.\nDrücken Sie Ja, um den Belegefaktor zu korrigieren oder\nNein, um den Standardwert einzutragen", "Ungültiger Belegefaktor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
		//        if (result.Equals(DialogResult.No)) {
		//            txtAllocation.Value = 80;
		//        } 
		//    }
		//}
	}
}
