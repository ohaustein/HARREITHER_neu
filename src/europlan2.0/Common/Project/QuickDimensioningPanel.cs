using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.IO;

namespace Europlan.Common {
	public partial class QuickDimensioningPanel : UserControl, IEditorUserControl {

		public event ProjectStructureChangedHandler ProjectStructureChanged;
		public event ProjectChangedHandler ProjectChanged;
		public event TreeSelectionRequestedHandler TreeSelectionRequested;

		private combit.ListLabel14.ListLabel listLabel1;
		DataSet reportingData;

		private Dictionary<Floor, QuickDimensioningFloorGrid> grids = new Dictionary<Floor, QuickDimensioningFloorGrid>();
		private System.ComponentModel.ComponentResourceManager resources = ResourcesManager.resources;

		public QuickDimensioningPanel() {
			InitializeComponent();
		}

		public void UpdateControl() {
			//Project.Instance.Config.

			this.cbEurovalHeat.Checked = ((Project.Instance.QuickDimensioning.EurovalCheckState & QuickDimensioning.ProductCheckState.Heat) == QuickDimensioning.ProductCheckState.Heat);
			this.cbEurovalCool.Checked = ((Project.Instance.QuickDimensioning.EurovalCheckState & QuickDimensioning.ProductCheckState.Cool) == QuickDimensioning.ProductCheckState.Cool);
			this.cbBkaHeat.Checked = ((Project.Instance.QuickDimensioning.ConcreteActivationCheckState & QuickDimensioning.ProductCheckState.Heat) == QuickDimensioning.ProductCheckState.Heat);
			this.cbBkaCool.Checked = ((Project.Instance.QuickDimensioning.ConcreteActivationCheckState & QuickDimensioning.ProductCheckState.Cool) == QuickDimensioning.ProductCheckState.Cool);
			this.cbHithermHeat.Checked = ((Project.Instance.QuickDimensioning.HithermCheckState & QuickDimensioning.ProductCheckState.Heat) == QuickDimensioning.ProductCheckState.Heat);
			this.cbHithermCool.Checked = ((Project.Instance.QuickDimensioning.HithermCheckState & QuickDimensioning.ProductCheckState.Cool) == QuickDimensioning.ProductCheckState.Cool);
			this.cbHithermCompactHeat.Checked = ((Project.Instance.QuickDimensioning.HithermCompactCheckState & QuickDimensioning.ProductCheckState.Heat) == QuickDimensioning.ProductCheckState.Heat);
			this.cbHithermCompactCool.Checked = ((Project.Instance.QuickDimensioning.HithermCompactCheckState & QuickDimensioning.ProductCheckState.Cool) == QuickDimensioning.ProductCheckState.Cool);
			this.cbModulKlimaBodenHeat.Checked = ((Project.Instance.QuickDimensioning.ModulBodenCheckState & QuickDimensioning.ProductCheckState.Heat) == QuickDimensioning.ProductCheckState.Heat);
			this.cbModulKlimaBodenCool.Checked = ((Project.Instance.QuickDimensioning.ModulBodenCheckState & QuickDimensioning.ProductCheckState.Cool) == QuickDimensioning.ProductCheckState.Cool);
			this.cbModulKlimaDeckeHeat.Checked = ((Project.Instance.QuickDimensioning.ModulDeckeCheckState & QuickDimensioning.ProductCheckState.Heat) == QuickDimensioning.ProductCheckState.Heat);
			this.cbModulKlimaDeckeCool.Checked = ((Project.Instance.QuickDimensioning.ModulDeckeCheckState & QuickDimensioning.ProductCheckState.Cool) == QuickDimensioning.ProductCheckState.Cool);

			this.lblTemp1.Visible = this.EurovalHeating;
			this.cmbHeatFlowTemperature.Visible = this.EurovalHeating;
			this.lblTemp2.Visible = this.EurovalHeating;
			this.cmbDistance.Visible = this.EurovalHeating;
			this.lblDistance.Visible = this.EurovalHeating;
			this.lblAssumptions.Visible = this.EurovalHeating || this.ConcreteActivationHeating || this.ConcreteActivationCooling || this.ModulKlimaDeckeHeating || this.ModulKlimaDeckeCooling;

			this.lblTemp3.Visible = this.ConcreteActivationCooling || this.ModulKlimaDeckeCooling;
			this.lblTemp4.Visible = this.ConcreteActivationCooling || this.ModulKlimaDeckeCooling;
			this.txtCoolTemperature.Visible = this.ConcreteActivationCooling || this.ModulKlimaDeckeCooling;

			this.lblAllocation.Visible = this.ModulKlimaDeckeHeating || this.ModulKlimaDeckeCooling;
			this.lblAllocation2.Visible = this.ModulKlimaDeckeHeating || this.ModulKlimaDeckeCooling;
			this.txtAllocation.Visible = this.ModulKlimaDeckeHeating || this.ModulKlimaDeckeCooling;

			int index = 0;
			if (Project.Instance.QuickDimensioning.HeatFlowTemperature == 30) {
				index = 0;
			} else if (Project.Instance.QuickDimensioning.HeatFlowTemperature == 35) {
				index = 1;
			} else if (Project.Instance.QuickDimensioning.HeatFlowTemperature == 40) {
				index = 2;
			} else if (Project.Instance.QuickDimensioning.HeatFlowTemperature == 45) {
				index = 3;
			}

			this.cmbHeatFlowTemperature.SelectedIndex = index;
			this.txtCoolTemperature.Text = Project.Instance.QuickDimensioning.CoolFlowTemperature.ToString();
			this.cmbDistance.SelectedIndex = (int)Project.Instance.QuickDimensioning.LayDistance;
			this.txtAllocation.Text = Project.Instance.QuickDimensioning.CeilingAllocation.ToString();

			//this.grids.Clear();
			List<Floor> floorsToRemove = new List<Floor>();
			foreach (Floor floor in grids.Keys) {
				floorsToRemove.Add(floor);
			}
			foreach (Floor floor in floorsToRemove) {
				QuickDimensioningFloorGrid grid = this.grids[floor];
				grid.ProjectChanged -= new ProjectChangedHandler(grid_ProjectChanged);
				this.grids.Remove(floor);
				grid.Dispose();
				GC.Collect();
			}
			floorsToRemove.Clear();

			List<TabPage> pagesToRemove = new List<TabPage>();
			foreach (TabPage page in this.tabQuickDimensioning.TabPages) {
				if (page != pageSettings) {
					pagesToRemove.Add(page);
				}
			}
			foreach (TabPage page in pagesToRemove) {
				this.tabQuickDimensioning.TabPages.Remove(page);
			}
			pagesToRemove.Clear();

			foreach (Floor floor in Project.Instance.Floors) {
				foreach (Room room in floor.Rooms) {
					if (!room.QuickDimensioningInitialized) {
						room.InitializeQuickDimensioning();
					}
				}

				TabPage page = new TabPage(floor.Name);
				page.UseVisualStyleBackColor = true;
				QuickDimensioningFloorGrid grid = new QuickDimensioningFloorGrid();
				grid.Euroval = this.EurovalHeating || this.EurovalCooling;
				grid.ConcreteActivation = this.ConcreteActivationHeating || this.ConcreteActivationCooling;
				grid.Hitherm = this.HithermHeating || this.HithermCooling;
				grid.HithermCompact = this.HithermCompactHeating || this.HithermCompactCooling;
				grid.ModulKlimaBoden = this.ModulKlimaBodenHeating || this.ModulKlimaBodenCooling;
				grid.ModulKlimaDecke = this.ModulKlimaDeckeHeating || this.ModulKlimaDeckeCooling;
				grid.Cooling = this.Cooling;
				grid.Floor = floor;
				page.Controls.Add(grid);
				grid.Dock = DockStyle.Fill;
				grid.ProjectChanged += new ProjectChangedHandler(grid_ProjectChanged);
				this.tabQuickDimensioning.TabPages.Add(page);
				this.grids.Add(floor, grid);
			}

			this.quickDimensioningDistributorsSummary.Euroval = this.EurovalHeating || this.EurovalCooling;
			this.quickDimensioningDistributorsSummary.ConcreteActivation = this.ConcreteActivationHeating || this.ConcreteActivationCooling;
			this.quickDimensioningDistributorsSummary.Hitherm = this.HithermHeating || this.HithermCooling;
			this.quickDimensioningDistributorsSummary.HithermCompact = this.HithermCompactHeating || this.HithermCompactCooling;
			this.quickDimensioningDistributorsSummary.ModulKlimaBoden = this.ModulKlimaBodenHeating || this.ModulKlimaBodenCooling;
			this.quickDimensioningDistributorsSummary.ModulKlimaDecke = this.ModulKlimaDeckeHeating || this.ModulKlimaDeckeCooling;

			this.tabQuickDimensioning.TabPages.Add(pageDistributors);
			this.tabQuickDimensioning.TabPages.Add(pageSummary);
		}

		private void grid_ProjectChanged(object sender) {
			this.OnProjectChanged();
		}

		public bool AllowLeave() {
			return true;
		}

		public bool Heating {
			get {
				return this.EurovalHeating ||
						this.ConcreteActivationHeating ||
						this.HithermHeating ||
						this.HithermCompactHeating ||
						this.ModulKlimaBodenHeating ||
						this.ModulKlimaDeckeHeating;
			}
		}

		public bool Cooling {
			get {
				return this.EurovalCooling ||
						this.ConcreteActivationCooling ||
						this.HithermCooling ||
						this.HithermCompactCooling ||
						this.ModulKlimaBodenCooling ||
						this.ModulKlimaDeckeCooling;
			}
		}

		public bool EurovalHeating {
			get { return this.cbEurovalHeat.Checked; }
			set { this.cbEurovalHeat.Checked = value; }
		}

		public bool EurovalCooling {
			get { return this.cbEurovalCool.Checked; }
			set { this.cbEurovalCool.Checked = value; }
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

		public bool HithermCooling {
			get { return this.cbHithermCool.Checked; }
			set { this.cbHithermCool.Checked = value; }
		}

		public bool HithermCompactHeating {
			get { return this.cbHithermCompactHeat.Checked; }
			set { this.cbHithermCompactHeat.Checked = value; }
		}

		public bool HithermCompactCooling {
			get { return this.cbHithermCompactCool.Checked; }
			set { this.cbHithermCompactCool.Checked = value; }
		}

		public bool ModulKlimaBodenHeating {
			get { return this.cbModulKlimaBodenHeat.Checked; }
			set { this.cbModulKlimaBodenHeat.Checked = value; }
		}

		public bool ModulKlimaBodenCooling {
			get { return this.cbModulKlimaBodenCool.Checked; }
			set { this.cbModulKlimaBodenCool.Checked = value; }
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
			if (!this.EurovalHeating && !this.EurovalCooling) {
				bool productFound = false;
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						if (room.GetProductForQuickDimensioning<EurovalProduct>() != null) {
							productFound = true;
						}
					}
				}
				if (productFound) {
					result = MessageBox.Show("Wollen sie das Produkt Euroval wirklich aus der Flächenaufstellung entfernen?", "Betonkernaktivierung entfernen", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				}
			}
			if (result == DialogResult.No) {
				this.EurovalHeating = !this.EurovalHeating;
			} else {
				foreach (QuickDimensioningFloorGrid grid in this.grids.Values) {
					grid.Euroval = this.EurovalHeating || this.EurovalCooling;
					grid.Cooling = this.Cooling;
				}
				this.quickDimensioningDistributorsSummary.Euroval = this.EurovalHeating || this.EurovalCooling;
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
				this.OnProjectChanged();
			}

			this.lblTemp1.Visible = this.Heating;
			this.cmbHeatFlowTemperature.Visible = this.Heating;
			this.lblTemp2.Visible = this.Heating;
			this.cmbDistance.Visible = this.EurovalHeating || this.ConcreteActivationHeating;
			this.lblDistance.Visible = this.EurovalHeating || this.ConcreteActivationHeating;
			this.lblAssumptions.Visible = this.Heating || this.Cooling;
			Project.Instance.QuickDimensioning.EurovalCheckState = (this.EurovalHeating ? QuickDimensioning.ProductCheckState.Heat : QuickDimensioning.ProductCheckState.None);
		}

		private void cbEurovalCool_CheckedChanged(object sender, EventArgs e) {
			DialogResult result = DialogResult.None;
			if (!this.EurovalHeating && !this.EurovalCooling) {
				bool productFound = false;
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						if (room.GetProductForQuickDimensioning<EurovalProduct>() != null) {
							productFound = true;
						}
					}
				}
				if (productFound) {
					result = MessageBox.Show("Wollen sie das Produkt Euroval wirklich aus der Flächenaufstellung entfernen?", "Betonkernaktivierung entfernen", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				}
			}
			if (result == DialogResult.No) {
				this.EurovalCooling = !this.EurovalCooling;
			} else {
				foreach (QuickDimensioningFloorGrid grid in this.grids.Values) {
					grid.Euroval = this.EurovalHeating || this.EurovalCooling;
					grid.Cooling = this.Cooling;
				}
				this.quickDimensioningDistributorsSummary.Euroval = this.EurovalHeating || this.EurovalCooling;
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
				this.OnProjectChanged();
			}

			this.lblTemp3.Visible = this.Cooling;
			this.lblTemp4.Visible = this.Cooling;
			this.txtCoolTemperature.Visible = this.Cooling;
			this.lblAssumptions.Visible = this.Heating || this.Cooling;
			Project.Instance.QuickDimensioning.ConcreteActivationCheckState = (this.EurovalHeating ? QuickDimensioning.ProductCheckState.Heat : QuickDimensioning.ProductCheckState.None) | (this.EurovalCooling ? QuickDimensioning.ProductCheckState.Cool : QuickDimensioning.ProductCheckState.None);
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
					grid.Cooling = this.Cooling;
				}
				this.quickDimensioningDistributorsSummary.ConcreteActivation = this.ConcreteActivationHeating || this.ConcreteActivationCooling;
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
				this.OnProjectChanged();
			}

			this.lblTemp1.Visible = this.Heating;
			this.cmbHeatFlowTemperature.Visible = this.Heating;
			this.lblTemp2.Visible = this.Heating;
			this.cmbDistance.Visible = this.EurovalHeating || this.ConcreteActivationHeating;
			this.lblDistance.Visible = this.EurovalHeating || this.ConcreteActivationHeating;
			this.lblAssumptions.Visible = this.Heating || this.Cooling;
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
					grid.Cooling = this.Cooling;
				}
				this.quickDimensioningDistributorsSummary.ConcreteActivation = this.ConcreteActivationHeating || this.ConcreteActivationCooling;
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
				this.OnProjectChanged();
			}

			this.lblTemp3.Visible = this.Cooling;
			this.lblTemp4.Visible = this.Cooling;
			this.txtCoolTemperature.Visible = this.Cooling;
			this.lblAssumptions.Visible = this.Heating || this.Cooling;
			Project.Instance.QuickDimensioning.ConcreteActivationCheckState = (this.ConcreteActivationHeating ? QuickDimensioning.ProductCheckState.Heat : QuickDimensioning.ProductCheckState.None) | (this.ConcreteActivationCooling ? QuickDimensioning.ProductCheckState.Cool : QuickDimensioning.ProductCheckState.None);
		}

		private void cbHithermHeat_CheckedChanged(object sender, EventArgs e) {
			DialogResult result = DialogResult.None;
			if (!this.HithermHeating && !this.HithermCooling) {
				bool productFound = false;
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						if (room.GetProductForQuickDimensioning<HithermProduct>() != null) {
							productFound = true;
						}
					}
				}
				if (productFound) {
					result = MessageBox.Show("Wollen sie das Produkt Hitherm wirklich aus der Flächenaufstellung entfernen?", "Betonkernaktivierung entfernen", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				}
			}
			if (result == DialogResult.No) {
				this.HithermHeating = !this.HithermHeating;
			} else {
				foreach (QuickDimensioningFloorGrid grid in this.grids.Values) {
					grid.Hitherm = this.HithermHeating || this.HithermCooling;
					grid.Cooling = this.Cooling;
				}
				this.quickDimensioningDistributorsSummary.Hitherm = this.HithermHeating || this.HithermCooling;
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
				this.OnProjectChanged();
			}

			this.lblTemp1.Visible = this.Heating;
			this.cmbHeatFlowTemperature.Visible = this.Heating;
			this.lblTemp2.Visible = this.Heating;
			this.lblAssumptions.Visible = this.Heating || this.Cooling;
			Project.Instance.QuickDimensioning.HithermCheckState = (this.HithermHeating ? QuickDimensioning.ProductCheckState.Heat : QuickDimensioning.ProductCheckState.None) | (this.HithermCooling ? QuickDimensioning.ProductCheckState.Cool : QuickDimensioning.ProductCheckState.None);
		}

		private void cbHithermCool_CheckedChanged(object sender, EventArgs e) {
			DialogResult result = DialogResult.None;
			if (!this.HithermHeating && !this.HithermCooling) {
				bool productFound = false;
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						if (room.GetProductForQuickDimensioning<HithermProduct>() != null) {
							productFound = true;
						}
					}
				}
				if (productFound) {
					result = MessageBox.Show("Wollen sie das Produkt Hitherm wirklich aus der Flächenaufstellung entfernen?", "Betonkernaktivierung entfernen", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				}
			}
			if (result == DialogResult.No) {
				this.HithermCooling = !this.HithermCooling;
			} else {
				foreach (QuickDimensioningFloorGrid grid in this.grids.Values) {
					grid.Hitherm = this.HithermHeating || this.HithermCooling;
					grid.Cooling = this.Cooling;
				}
				this.quickDimensioningDistributorsSummary.Hitherm = this.HithermHeating || this.HithermCooling;
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
				this.OnProjectChanged();
			}

			this.lblTemp3.Visible = this.Cooling;
			this.lblTemp4.Visible = this.Cooling;
			this.txtCoolTemperature.Visible = this.Cooling;
			this.lblAssumptions.Visible = this.Heating || this.Cooling;
			Project.Instance.QuickDimensioning.HithermCheckState = (this.HithermHeating ? QuickDimensioning.ProductCheckState.Heat : QuickDimensioning.ProductCheckState.None) | (this.HithermCooling ? QuickDimensioning.ProductCheckState.Cool : QuickDimensioning.ProductCheckState.None);
		}

		private void cbHithermCompactHeat_CheckedChanged(object sender, EventArgs e) {
			DialogResult result = DialogResult.None;
			if (!this.HithermCompactHeating && !this.HithermCompactCooling) {
				bool productFound = false;
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						if (room.GetProductForQuickDimensioning<HithermCompactProduct>() != null) {
							productFound = true;
						}
					}
				}
				if (productFound) {
					result = MessageBox.Show("Wollen sie das Produkt Hitherm Compact wirklich aus der Flächenaufstellung entfernen?", "Betonkernaktivierung entfernen", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				}
			}
			if (result == DialogResult.No) {
				this.HithermCompactHeating = !this.HithermCompactHeating;
			} else {
				foreach (QuickDimensioningFloorGrid grid in this.grids.Values) {
					grid.HithermCompact = this.HithermCompactHeating || this.HithermCompactCooling;
					grid.Cooling = this.Cooling;
				}
				this.quickDimensioningDistributorsSummary.HithermCompact = this.HithermCompactHeating || this.HithermCompactCooling;
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
				this.OnProjectChanged();
			}

			this.lblTemp1.Visible = this.Heating;
			this.cmbHeatFlowTemperature.Visible = this.Heating;
			this.lblTemp2.Visible = this.Heating;
			this.lblAssumptions.Visible = this.Heating || this.Cooling;
			Project.Instance.QuickDimensioning.HithermCompactCheckState = (this.HithermCompactHeating ? QuickDimensioning.ProductCheckState.Heat : QuickDimensioning.ProductCheckState.None) | (this.HithermCompactCooling ? QuickDimensioning.ProductCheckState.Cool : QuickDimensioning.ProductCheckState.None);
		}

		private void cbHithermCompactCool_CheckedChanged(object sender, EventArgs e) {
			DialogResult result = DialogResult.None;
			if (!this.HithermCompactHeating && !this.HithermCompactCooling) {
				bool productFound = false;
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						if (room.GetProductForQuickDimensioning<HithermCompactProduct>() != null) {
							productFound = true;
						}
					}
				}
				if (productFound) {
					result = MessageBox.Show("Wollen sie das Produkt Hitherm Compact wirklich aus der Flächenaufstellung entfernen?", "Betonkernaktivierung entfernen", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				}
			}
			if (result == DialogResult.No) {
				this.HithermCompactCooling = !this.HithermCompactCooling;
			} else {
				foreach (QuickDimensioningFloorGrid grid in this.grids.Values) {
					grid.HithermCompact = this.HithermCompactHeating || this.HithermCompactCooling;
					grid.Cooling = this.Cooling;
				}
				this.quickDimensioningDistributorsSummary.HithermCompact = this.HithermCompactHeating || this.HithermCompactCooling;
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
				this.OnProjectChanged();
			}

			this.lblTemp3.Visible = this.Cooling;
			this.lblTemp4.Visible = this.Cooling;
			this.txtCoolTemperature.Visible = this.Cooling;
			this.lblAssumptions.Visible = this.Heating || this.Cooling;
			Project.Instance.QuickDimensioning.HithermCompactCheckState = (this.HithermCompactHeating ? QuickDimensioning.ProductCheckState.Heat : QuickDimensioning.ProductCheckState.None) | (this.HithermCompactCooling ? QuickDimensioning.ProductCheckState.Cool : QuickDimensioning.ProductCheckState.None);
		}

		private void cbModulKlimaBodenHeat_CheckedChanged(object sender, EventArgs e) {
			DialogResult result = DialogResult.None;
			if (!this.ModulKlimaBodenHeating && !this.ModulKlimaBodenCooling) {
				bool productFound = false;
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						if (room.GetProductForQuickDimensioning<ModulKlimaBodenProduct>() != null) {
							productFound = true;
						}
					}
				}
				if (productFound) {
					result = MessageBox.Show("Wollen sie das Produkt Modul Klimaboden wirklich aus der Flächenaufstellung entfernen?", "Betonkernaktivierung entfernen", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				}
			}
			if (result == DialogResult.No) {
				this.ModulKlimaBodenHeating = !this.ModulKlimaBodenHeating;
			} else {
				foreach (QuickDimensioningFloorGrid grid in this.grids.Values) {
					grid.ModulKlimaBoden = this.ModulKlimaBodenHeating || this.ModulKlimaBodenCooling;
					grid.Cooling = this.Cooling;
				}
				this.quickDimensioningDistributorsSummary.ModulKlimaBoden = this.ModulKlimaBodenHeating || this.ModulKlimaBodenCooling;
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
				this.OnProjectChanged();
			}

			this.lblTemp1.Visible = this.Heating;
			this.cmbHeatFlowTemperature.Visible = this.Heating;
			this.lblTemp2.Visible = this.Heating;
			this.lblAssumptions.Visible = this.Heating || this.Cooling;
			Project.Instance.QuickDimensioning.ModulBodenCheckState = (this.ModulKlimaBodenHeating ? QuickDimensioning.ProductCheckState.Heat : QuickDimensioning.ProductCheckState.None) | (this.ModulKlimaBodenCooling ? QuickDimensioning.ProductCheckState.Cool : QuickDimensioning.ProductCheckState.None);
		}

		private void cbModulKlimaBodenCool_CheckedChanged(object sender, EventArgs e) {
			DialogResult result = DialogResult.None;
			if (!this.ModulKlimaBodenHeating && !this.ModulKlimaBodenCooling) {
				bool productFound = false;
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						if (room.GetProductForQuickDimensioning<ModulKlimaBodenProduct>() != null) {
							productFound = true;
						}
					}
				}
				if (productFound) {
					result = MessageBox.Show("Wollen sie das Produkt Modul Klimaboden wirklich aus der Flächenaufstellung entfernen?", "Betonkernaktivierung entfernen", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				}
			}
			if (result == DialogResult.No) {
				this.ModulKlimaBodenCooling = !this.ModulKlimaBodenCooling;
			} else {
				foreach (QuickDimensioningFloorGrid grid in this.grids.Values) {
					grid.ModulKlimaBoden = this.ModulKlimaBodenHeating || this.ModulKlimaBodenCooling;
					grid.Cooling = this.Cooling;
				}
				this.quickDimensioningDistributorsSummary.ModulKlimaBoden = this.ModulKlimaBodenHeating || this.ModulKlimaBodenCooling;
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
				this.OnProjectChanged();
			}

			this.lblTemp3.Visible = this.Cooling;
			this.lblTemp4.Visible = this.Cooling;
			this.txtCoolTemperature.Visible = this.Cooling;
			this.lblAssumptions.Visible = this.Heating || this.Cooling;
			Project.Instance.QuickDimensioning.ModulBodenCheckState = (this.ModulKlimaBodenHeating ? QuickDimensioning.ProductCheckState.Heat : QuickDimensioning.ProductCheckState.None) | (this.ModulKlimaBodenCooling ? QuickDimensioning.ProductCheckState.Cool : QuickDimensioning.ProductCheckState.None);
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
					grid.Cooling = this.Cooling;
				}
				this.quickDimensioningDistributorsSummary.ModulKlimaDecke = this.ModulKlimaDeckeHeating || this.ModulKlimaDeckeCooling;
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
				this.OnProjectChanged();
			}

			this.lblTemp1.Visible = this.Heating;
			this.cmbHeatFlowTemperature.Visible = this.Heating;
			this.lblTemp2.Visible = this.Heating;
			this.lblAssumptions.Visible = this.Heating || this.Cooling;
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
					grid.Cooling = this.Cooling;
				}
				this.quickDimensioningDistributorsSummary.ModulKlimaDecke = this.ModulKlimaDeckeHeating || this.ModulKlimaDeckeCooling;
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
				this.OnProjectChanged();
			}
			this.lblAllocation.Visible = this.ModulKlimaDeckeHeating || this.ModulKlimaDeckeCooling;
			this.lblAllocation2.Visible = this.ModulKlimaDeckeHeating || this.ModulKlimaDeckeCooling;
			this.txtAllocation.Visible = this.ModulKlimaDeckeHeating || this.ModulKlimaDeckeCooling;
			this.lblTemp3.Visible = this.Cooling;
			this.lblTemp4.Visible = this.Cooling;
			this.txtCoolTemperature.Visible = this.Cooling;
			this.lblAssumptions.Visible = this.Heating || this.Cooling;
			Project.Instance.QuickDimensioning.ModulDeckeCheckState = (this.ModulKlimaDeckeHeating ? QuickDimensioning.ProductCheckState.Heat : QuickDimensioning.ProductCheckState.None) | (this.ModulKlimaDeckeCooling ? QuickDimensioning.ProductCheckState.Cool : QuickDimensioning.ProductCheckState.None);
		}

		private void cmbHeatFlowTemperature_SelectedIndexChanged(object sender, EventArgs e) {
			Project.Instance.QuickDimensioning.HeatFlowTemperature = (float)Int32.Parse((string)cmbHeatFlowTemperature.SelectedItem);
			switch (cmbHeatFlowTemperature.SelectedIndex) {
				case 0:
					this.cmbDistance.SelectedIndex = (int)EurovalProduct.LayDistance.EV5;
					break;
				case 1:
					this.cmbDistance.SelectedIndex = (int)EurovalProduct.LayDistance.EV15;
					break;
				case 2:
					this.cmbDistance.SelectedIndex = (int)EurovalProduct.LayDistance.EV20;
					break;
				case 3:
					this.cmbDistance.SelectedIndex = (int)EurovalProduct.LayDistance.EV25;
					break;
			}
			this.OnProjectChanged();
		}

		private void txtAllocation_ValueChanged(object sender, EventArgs e) {
			float allocation = (float)this.txtAllocation.Value;
			Project.Instance.QuickDimensioning.CeilingAllocation = allocation;
			this.OnProjectChanged();
		}

		private void cmbDistance_SelectedIndexChanged(object sender, EventArgs e) {
			Project.Instance.QuickDimensioning.LayDistance = (EurovalProduct.LayDistance)this.cmbDistance.SelectedIndex;
			this.OnProjectChanged();
		}


		private void tabQuickDimensioning_Selected(object sender, TabControlEventArgs e) {
			if (e.TabPage == this.pageSummary) {
				string filename = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Reporting"), "QuickDimensioning.lst");
				try {
					listLabel1.Print(combit.ListLabel14.LlProject.List, filename, false, combit.ListLabel14.LlPrintMode.PreviewControl, combit.ListLabel14.LlBoxType.None, "", false, null);
					GC.Collect();
				} catch (Exception ex) {
					DialogResult result = MessageBox.Show("Die Anwendung konnte keinen installierten Drucker finden. Drücken Sie OK, um einen Standarddrucker einzurichten, mit dem die Vorschau und der Export in eine Datei ermöglicht wird oder Abbrechen, um manuell einen Drucker einzurichten.", "Kein Drucker vorhanden...", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
					if (result == DialogResult.OK) {
						try {
							System.Diagnostics.Process p = new System.Diagnostics.Process();
							p.StartInfo.FileName = "rundll32.exe";
							p.StartInfo.Arguments = "printui.dll,PrintUIEntry /if /b \"Europlan 2.0 Reporting\" /f " + Environment.GetEnvironmentVariable("windir") + "\\inf\\ntprint.inf /r \"lpt1:\" /m \"HP LaserJet 4\"";
							p.Start();
							p.WaitForExit();
							listLabel1.Print(combit.ListLabel14.LlProject.List, filename, false, combit.ListLabel14.LlPrintMode.PreviewControl, combit.ListLabel14.LlBoxType.None, "", false, null);
						} catch (Exception) {
							MessageBox.Show("Fehler bei der automatischen Einrichtung eines Druckers. Richten Sie bitte manuell einen beliebigen Drucker ein.");
							this.tabQuickDimensioning.SelectedTab = this.pageSettings;
						}
					} else {
						//this.tabQuickDimensioning.SelectedTab = this.pageSettings;
					}
				}
			}
		}



		private void tabQuickDimensioning_Selecting(object sender, TabControlCancelEventArgs e) {
			if (e.TabPage == this.pageSummary) {
				Cursor current = Cursor.Current;
				Cursor.Current = Cursors.WaitCursor;

				if (listLabel1 != null) {
					listLabel1.Dispose();
					listLabel1 = null;
				}

				if (reportingData == null) {
					reportingData = new DataSet();
				} else {
					reportingData.Clear();
				}

				this.listLabel1 = new combit.ListLabel14.ListLabel();

				this.listLabel1.AutoDestination = combit.ListLabel14.LlPrintMode.PreviewControl;
				this.listLabel1.LicensingInfo = "5hKHEQ";
				this.listLabel1.MaxRTFVersion = 65280;
				this.listLabel1.NoParameterCheck = true;
				this.listLabel1.PreviewControl = this.listLabelPreviewControl1;
				this.listLabel1.Unit = combit.ListLabel14.LlUnits.Millimeter_1_100;

				List<QuickDimensioningReportWrapper> reportWrapper = Project.Instance.QuickDimensioning.GetQuickDimensioningRoomReports();
				DataTable rooms = ListToDataTable<QuickDimensioningReportWrapper>(reportWrapper);
				DataTable distributors = ListToDataTable<QuickDimensioningDistributorsReportWrapper>(Project.Instance.QuickDimensioning.GetQuickDimensioningDistributorsReports());
				rooms.TableName = "QuickDimensioningReportWrapper";
				distributors.TableName = "QuickDimensioningDistributorsReportWrapper";

				reportingData.Tables.Add(rooms);
				reportingData.Tables.Add(distributors);

				listLabel1.DataSource = reportingData;

				string projectName = "";
				foreach (string line in Project.Instance.ProjectName) {
					projectName += line + "\n";
				}
				projectName = projectName.TrimEnd();
				listLabel1.Variables.Add("@ProjectName", projectName);
				listLabel1.Variables.Add("@ProjectEditor", Project.Instance.ProjectEditor);
				listLabel1.Variables.Add("@NrOfProducts", Project.Instance.QuickDimensioning.GetPlannedProducts().Count);
				listLabel1.Variables.Add("@PartnerContact", Licensing.LicenseManager.Instance.License.Header.Replace("\r", ""));
				if (this.Heating) {
					listLabel1.Variables.Add("@tvHeat", Project.Instance.QuickDimensioning.HeatFlowTemperature);
				} else {
					listLabel1.Variables.Add("@tvHeat", -1);
				}
				if (this.Cooling) {
					listLabel1.Variables.Add("@tvCool", Project.Instance.QuickDimensioning.CoolFlowTemperature);
				} else {
					listLabel1.Variables.Add("@tvCool", -1);
				}
				if (this.ModulKlimaDeckeHeating || this.ModulKlimaDeckeCooling) {
					listLabel1.Variables.Add("@Allocation", Project.Instance.QuickDimensioning.CeilingAllocation + "%");
				} else {
					listLabel1.Variables.Add("@Allocation", "");
				}
				//TODO
				//string filename = Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.CommonAppDataPath), "partner.jpg");
				string filename = Configuration.UserTemplate.PartnerLogo;
				if (File.Exists(filename)) {
					listLabel1.Variables.Add("@PartnerLogo", Image.FromFile(filename));
				} else {
					listLabel1.Variables.Add("@PartnerLogo", "(NULL)");
				}
				string usedRoomTypes = "";
				foreach (RoomType roomType in Project.Instance.Config.RoomTypes) {
					foreach (QuickDimensioningReportWrapper wrapper in reportWrapper) {
						if (wrapper.RoomType == roomType.Name) {
							usedRoomTypes += roomType.Name + ": " + roomType.HeatLoadPerSquareMeter + "W/m² - " + roomType.CoolLoadPerSquareMeter + "W/m²\n";
							break;
						}
					}
				}
				usedRoomTypes = usedRoomTypes.TrimEnd();
				listLabel1.Variables.Add("@RoomTypes", usedRoomTypes);
				int i = 1;
				foreach (string productName in Project.Instance.QuickDimensioning.GetPlannedProducts()) {
					listLabel1.Variables.Add("@Product" + i, productName);
					i++;
				}
				Dictionary<Room.RoomController, int> roomControllers = new Dictionary<Room.RoomController, int>();
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						if (room.QuickDimensioningRoomController != Room.RoomController.None) {
							if (!roomControllers.ContainsKey(room.QuickDimensioningRoomController)) {
								roomControllers.Add(room.QuickDimensioningRoomController, 1);
							} else {
								roomControllers[room.QuickDimensioningRoomController] = roomControllers[room.QuickDimensioningRoomController] + 1;
							}
						}
					}
				}
				string controllersSummary = null;
				string localized = "";
				foreach (KeyValuePair<Room.RoomController, int> kvp in roomControllers) {
					if (controllersSummary != null) {
						controllersSummary += ", ";
					} else {
						controllersSummary = "";
					}
					localized = resources.GetString(kvp.Key.ToString(), Thread.CurrentThread.CurrentUICulture);
					controllersSummary += kvp.Value.ToString() + " * " + localized;
				}
				listLabel1.Variables.Add("@RoomControllers", controllersSummary);
			
				Cursor.Current = current;
			} else if (e.TabPage == this.pageDistributors) {
				this.quickDimensioningDistributorsSummary.UpdateControl();
			}
			
		}

		public static DataTable ListToDataTable<T>(List<T> list) {
			DataTable dt = new DataTable();

			foreach (PropertyInfo info in typeof(T).GetProperties()) {
				dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
			}
			foreach (T t in list) {
				DataRow row = dt.NewRow();
				foreach (PropertyInfo info in typeof(T).GetProperties()) {
					row[info.Name] = info.GetValue(t, null);
				}
				dt.Rows.Add(row);
			}
			return dt;
		}

		private void OnProjectChanged() {
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
		}

		private void btnRevert_Click(object sender, EventArgs e) {
			if (MessageBox.Show("Wollen Sie die Flächenaufstellung wirklich zurücksetzen? Alle Daten, die Sie in der Flächenaufstellung bereits eingegeben haben, gehen dadurch verloren.", "Wirklich Zurücksetzen?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						room.RevertQuickDimensioning();
					}
				}
				QuickDimensioning qd = Project.Instance.QuickDimensioning;
				qd.EurovalCheckState = QuickDimensioning.ProductCheckState.None;
				qd.ConcreteActivationCheckState = QuickDimensioning.ProductCheckState.None;
				qd.HithermCheckState = QuickDimensioning.ProductCheckState.None;
				qd.HithermCompactCheckState = QuickDimensioning.ProductCheckState.None;
				qd.ModulBodenCheckState = QuickDimensioning.ProductCheckState.None;
				qd.ModulDeckeCheckState = QuickDimensioning.ProductCheckState.None;
				// TODO revert parameters

				this.UpdateControl();
			}
		}

		private void quickDimensioningDistributorsSummary_ProjectChanged(object sender) {
			this.OnProjectChanged();
		}

		private void button1_Click_1(object sender, EventArgs e) {
			listLabel1.Design();
		}

	}
}
