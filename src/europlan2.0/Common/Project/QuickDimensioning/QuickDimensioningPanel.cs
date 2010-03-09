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
using System.Resources;
using System.Collections;

namespace Europlan.Common {
	public partial class QuickDimensioningPanel : UserControl, IEditorUserControl {

		public event ProjectStructureChangedHandler ProjectStructureChanged;
		public event ProjectChangedHandler ProjectChanged;
		public event TreeSelectionRequestedHandler TreeSelectionRequested;

		private combit.ListLabel15.ListLabel listLabel1;
		private combit.ListLabel15.ListLabelPreviewControl listLabelPreviewControl1;
		DataSet reportingData;

		private Dictionary<Floor, QuickDimensioningFloorGrid> grids = new Dictionary<Floor, QuickDimensioningFloorGrid>();

		private bool updateControlOngoing = false;

		public QuickDimensioningPanel() {
			InitializeComponent();

			this.SetLanguage();

			Licensing.License license = Licensing.LicenseManager.Instance.License;
			/*this.tableLayoutPanel1.Controls.Remove(this.lblEuroval);
			this.tableLayoutPanel1.Controls.Remove(this.cbEurovalCool);
			this.tableLayoutPanel1.Controls.Remove(this.cbEurovalHeat);
			this.tableLayoutPanel1.Controls.Remove(this.lblHitherm);
			this.tableLayoutPanel1.Controls.Remove(this.cbHithermCool);
			this.tableLayoutPanel1.Controls.Remove(this.cbHithermHeat);
			this.tableLayoutPanel1.Controls.Remove(this.lblHithermCompact);
			this.tableLayoutPanel1.Controls.Remove(this.cbHithermCompactCool);
			this.tableLayoutPanel1.Controls.Remove(this.cbHithermCompactHeat);
			this.tableLayoutPanel1.Controls.Remove(this.lblHithermCompactRoof);
			this.tableLayoutPanel1.Controls.Remove(this.cbHithermCompactRoofCool);
			this.tableLayoutPanel1.Controls.Remove(this.cbHithermCompactRoofHeat);
			this.tableLayoutPanel1.Controls.Remove(this.lblModulKlimaBoden);
			this.tableLayoutPanel1.Controls.Remove(this.cbModulKlimaBodenCool);
			this.tableLayoutPanel1.Controls.Remove(this.cbModulKlimaBodenHeat);
			this.tableLayoutPanel1.Controls.Remove(this.lblModulKlimaDecke);
			this.tableLayoutPanel1.Controls.Remove(this.cbModulKlimaDeckeCool);
			this.tableLayoutPanel1.Controls.Remove(this.cbModulKlimaDeckeHeat);
			this.tableLayoutPanel1.Controls.Remove(this.lblBka);
			this.tableLayoutPanel1.Controls.Remove(this.cbBkaCool);
			this.tableLayoutPanel1.Controls.Remove(this.cbBkaHeat);*/
			this.tableLayoutPanel1.Controls.Clear();

			this.tableLayoutPanel1.Controls.Add(this.lblHeat, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblCool, 2, 0);

			int i = 1;
			if (license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdEuroval)) {
				this.tableLayoutPanel1.Controls.Add(this.lblEuroval, 0, i);
				this.tableLayoutPanel1.Controls.Add(this.cbEurovalHeat, 1, i);
				this.tableLayoutPanel1.Controls.Add(this.cbEurovalCool, 2, i);
				this.tableLayoutPanel1.Controls.Add(this.lblDistance, 3, i);
				this.tableLayoutPanel1.Controls.Add(this.cmbDistance, 4, i);
				i++;
			}
			if (license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdConcreteActivation)) {
				this.tableLayoutPanel1.Controls.Add(this.lblBka, 0, i);
				this.tableLayoutPanel1.Controls.Add(this.cbBkaHeat, 1, i);
				this.tableLayoutPanel1.Controls.Add(this.cbBkaCool, 2, i);
				if (!license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdEuroval)) {
					this.tableLayoutPanel1.Controls.Add(this.lblDistance, 3, i);
					this.tableLayoutPanel1.Controls.Add(this.cmbDistance, 4, i);
				}
				i++;
			}
			if (license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdEcotherm)) {
				// ecotherm will be added to quickdimensioning when requested
			}
			if (license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdHitherm)) {
				this.tableLayoutPanel1.Controls.Add(this.lblHitherm, 0, i);
				this.tableLayoutPanel1.Controls.Add(this.cbHithermHeat, 1, i);
				this.tableLayoutPanel1.Controls.Add(this.cbHithermCool, 2, i);
				i++;
			}
			if (license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdHithermCompact)) {
				this.tableLayoutPanel1.Controls.Add(this.lblHithermCompact, 0, i);
				this.tableLayoutPanel1.Controls.Add(this.cbHithermCompactHeat, 1, i);
				this.tableLayoutPanel1.Controls.Add(this.cbHithermCompactCool, 2, i);
				i++;
				this.tableLayoutPanel1.Controls.Add(this.lblHithermCompactRoof, 0, i);
				this.tableLayoutPanel1.Controls.Add(this.cbHithermCompactRoofHeat, 1, i);
				this.tableLayoutPanel1.Controls.Add(this.cbHithermCompactRoofCool, 2, i);
				i++;
			}
			if (license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdModulKlimaBoden)) {
				this.tableLayoutPanel1.Controls.Add(this.lblModulKlimaBoden, 0, i);
				this.tableLayoutPanel1.Controls.Add(this.cbModulKlimaBodenHeat, 1, i);
				this.tableLayoutPanel1.Controls.Add(this.cbModulKlimaBodenCool, 2, i);
				i++;
			}
			if (license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdModulKlimaDecke)) {
				this.tableLayoutPanel1.Controls.Add(this.lblModulKlimaDecke, 0, i);
				this.tableLayoutPanel1.Controls.Add(this.cbModulKlimaDeckeHeat, 1, i);
				this.tableLayoutPanel1.Controls.Add(this.cbModulKlimaDeckeCool, 2, i);
				this.tableLayoutPanel1.Controls.Add(this.lblAllocation, 3, i);
				this.tableLayoutPanel1.Controls.Add(this.txtAllocation, 4, i);
				this.tableLayoutPanel1.Controls.Add(this.lblAllocation2, 5, i);
				i++;
			}
			i++;
			this.tableLayoutPanel1.Controls.Add(this.btnRevert, 0, i);
		}

		private void SetLanguage() {
			this.pageSummary.Text = EuroplanRes.QuickDimensioningPanel_Flaechenaufstellung;//"Ergebnis Flächenaufstellung";
			this.pageSettings.Text = EuroplanRes.QuickDimensioningPanel_Einstellungen;//"Einstellungen";
			this.lblTemp4.Text = EuroplanRes.QuickDimensioningPanel_Tv;//"°C (Tv)";
			this.lblTemp3.Text = EuroplanRes.QuickDimensioningPanel_VorlauftemperaturKuehlen;//"Vorlauftemperatur\r\n(Kühlen)";
			this.lblTemp2.Text = EuroplanRes.QuickDimensioningPanel_Tv;//"°C (Tv)";
			this.label2.Text = EuroplanRes.QuickDimensioningPanel_ProdukteWaehlen;//"Bitte wählen Sie jene Harreither-Produkte aus, welche in der Flächenaufstellung zur Verfügung stehen sollen:";
			this.lblTemp1.Text = EuroplanRes.QuickDimensioningPanel_VorlauftemperaturHeizen;//"Vorlauftemperatur\r\n(Heizen)";
			this.lblEuroval.Text = EuroplanRes.QuickDimensioningPanel_Euroval;//"Euroval® Fußbodenheizung";
			this.btnRevert.Text = EuroplanRes.QuickDimensioningPanel_Zuruecksetzen;//"Flächenaufstellung zurücksetzen";
			this.lblAllocation2.Text = EuroplanRes.Unit_Prozent;//"%";
			this.lblAllocation.Text = EuroplanRes.QuickDimensioningPanel_Belegefaktor;//"Belegefaktor";
			this.lblHeat.Text = EuroplanRes.QuickDimensioningPanel_Heizen;//"Heizen";
			this.lblCool.Text = EuroplanRes.QuickDimensioningPanel_Kuehlen;//"Kühlen";
			this.lblHithermCompact.Text = EuroplanRes.QuickDimensioningPanel_HithermCompact;//"Hitherm® Compact";
			this.lblHitherm.Text = EuroplanRes.QuickDimensioningPanel_Hitherm;//"Hitherm® Klimawand";
			this.lblBka.Text = EuroplanRes.QuickDimensioningPanel_Bka;//"Betonkernaktivierung";
			this.lblModulKlimaDecke.Text = EuroplanRes.QuickDimensioningPanel_KlimaDecke;//"Modul Klima-Decke";
			this.lblModulKlimaBoden.Text = EuroplanRes.QuickDimensioningPanel_KlimaBoden;//"Modul Klima-Boden";
			this.lblHithermCompactRoof.Text = EuroplanRes.QuickDimensioningPanel_HithermCompactDach;//"Hitherm® Compact Dachschräge";
			this.lblDistance.Text = EuroplanRes.QuickDimensioningPanel_Verlegeabstand;//"Verlegeabstand";
			this.lblAssumptions.Text = EuroplanRes.QuickDimensioningPanel_Annahmen;//"Annahmen";
			this.pageDistributors.Text = EuroplanRes.QuickDimensioningPanel_Verteiler;//"Verteiler";
			this.label3.Text = EuroplanRes.QuickDimensioningPanel_Flaechenaufstellung;//"Flächenaufstellung";

			this.cmbDistance.Items.Clear();
			this.cmbDistance.Items.AddRange(new object[] {
            EuroplanRes.EurovalProduct_EV5,//"EV 5",
            EuroplanRes.EurovalProduct_EV10,//"EV10",
            EuroplanRes.EurovalProduct_EV15,//"EV15",
            EuroplanRes.EurovalProduct_EV20,//"EV20",
            EuroplanRes.EurovalProduct_EV25,//"EV25",
            EuroplanRes.EurovalProduct_EV30,//"EV30",
            EuroplanRes.EurovalProduct_EV35,//"EV35"
			});

		}

		public void UpdateControl(bool resetUserInterface) {
			//Project.Instance.Config.

			updateControlOngoing = true;

			this.cbEurovalHeat.Checked = ((Project.Instance.QuickDimensioning.EurovalCheckState & QuickDimensioning.ProductCheckState.Heat) == QuickDimensioning.ProductCheckState.Heat);
			this.cbEurovalCool.Checked = ((Project.Instance.QuickDimensioning.EurovalCheckState & QuickDimensioning.ProductCheckState.Cool) == QuickDimensioning.ProductCheckState.Cool);
			this.cbBkaHeat.Checked = ((Project.Instance.QuickDimensioning.ConcreteActivationCheckState & QuickDimensioning.ProductCheckState.Heat) == QuickDimensioning.ProductCheckState.Heat);
			this.cbBkaCool.Checked = ((Project.Instance.QuickDimensioning.ConcreteActivationCheckState & QuickDimensioning.ProductCheckState.Cool) == QuickDimensioning.ProductCheckState.Cool);
			this.cbHithermHeat.Checked = ((Project.Instance.QuickDimensioning.HithermCheckState & QuickDimensioning.ProductCheckState.Heat) == QuickDimensioning.ProductCheckState.Heat);
			this.cbHithermCool.Checked = ((Project.Instance.QuickDimensioning.HithermCheckState & QuickDimensioning.ProductCheckState.Cool) == QuickDimensioning.ProductCheckState.Cool);
			this.cbHithermCompactHeat.Checked = ((Project.Instance.QuickDimensioning.HithermCompactCheckState & QuickDimensioning.ProductCheckState.Heat) == QuickDimensioning.ProductCheckState.Heat);
			this.cbHithermCompactCool.Checked = ((Project.Instance.QuickDimensioning.HithermCompactCheckState & QuickDimensioning.ProductCheckState.Cool) == QuickDimensioning.ProductCheckState.Cool);
			this.cbHithermCompactRoofHeat.Checked = ((Project.Instance.QuickDimensioning.HithermCompactRoofCheckState & QuickDimensioning.ProductCheckState.Heat) == QuickDimensioning.ProductCheckState.Heat);
			this.cbHithermCompactRoofCool.Checked = ((Project.Instance.QuickDimensioning.HithermCompactRoofCheckState & QuickDimensioning.ProductCheckState.Cool) == QuickDimensioning.ProductCheckState.Cool);
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

			this.lblTemp3.Visible = this.Cooling;
			this.lblTemp4.Visible = this.Cooling;
			this.txtCoolTemperature.Visible = this.Cooling;

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
				grid.HithermCompactRoof = this.HithermCompactRoofHeating || this.HithermCompactRoofCooling;
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
			this.quickDimensioningDistributorsSummary.HithermCompactRoof = this.HithermCompactRoofHeating || this.HithermCompactRoofCooling;
			this.quickDimensioningDistributorsSummary.ModulKlimaBoden = this.ModulKlimaBodenHeating || this.ModulKlimaBodenCooling;
			this.quickDimensioningDistributorsSummary.ModulKlimaDecke = this.ModulKlimaDeckeHeating || this.ModulKlimaDeckeCooling;

			this.tabQuickDimensioning.TabPages.Add(pageDistributors);
			this.tabQuickDimensioning.TabPages.Add(pageSummary);

			updateControlOngoing = false;
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
						this.HithermCompactRoofHeating ||
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
						this.HithermCompactRoofCooling ||
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

		public bool HithermCompactRoofHeating {
			get { return this.cbHithermCompactRoofHeat.Checked; }
			set { this.cbHithermCompactRoofHeat.Checked = value; }
		}

		public bool HithermCompactRoofCooling {
			get { return this.cbHithermCompactRoofCool.Checked; }
			set { this.cbHithermCompactRoofCool.Checked = value; }
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
					result = MessageBox.Show(EuroplanRes.QuickDimensioningPanel_EurovalEntfernenText, EuroplanRes.QuickDimensioningPanel_EurovalEntfernenTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
			this.cmbDistance.Visible = this.EurovalHeating || this.EurovalCooling || this.ConcreteActivationHeating  || this.ConcreteActivationCooling;
			this.lblDistance.Visible = this.EurovalHeating || this.EurovalCooling || this.ConcreteActivationHeating || this.ConcreteActivationCooling;
			this.lblAssumptions.Visible = this.Heating || this.Cooling;
			if (!updateControlOngoing)
				Project.Instance.QuickDimensioning.EurovalCheckState = (this.EurovalHeating ? QuickDimensioning.ProductCheckState.Heat : QuickDimensioning.ProductCheckState.None) | (this.EurovalCooling ? QuickDimensioning.ProductCheckState.Cool : QuickDimensioning.ProductCheckState.None);
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
					result = MessageBox.Show(EuroplanRes.QuickDimensioningPanel_EurovalEntfernenText, EuroplanRes.QuickDimensioningPanel_EurovalEntfernenTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
			this.cmbDistance.Visible = this.EurovalHeating || this.EurovalCooling || this.ConcreteActivationHeating || this.ConcreteActivationCooling;
			this.lblDistance.Visible = this.EurovalHeating || this.EurovalCooling || this.ConcreteActivationHeating || this.ConcreteActivationCooling;
			this.lblAssumptions.Visible = this.Heating || this.Cooling;
			if (!updateControlOngoing)
				Project.Instance.QuickDimensioning.EurovalCheckState = (this.EurovalHeating ? QuickDimensioning.ProductCheckState.Heat : QuickDimensioning.ProductCheckState.None) | (this.EurovalCooling ? QuickDimensioning.ProductCheckState.Cool : QuickDimensioning.ProductCheckState.None);
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
					result = MessageBox.Show(EuroplanRes.QuickDimensioningPanel_BkaEntfernenText, EuroplanRes.QuickDimensioningPanel_BkaEntfernenTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
			this.cmbDistance.Visible = this.EurovalHeating || this.EurovalCooling || this.ConcreteActivationHeating || this.ConcreteActivationCooling;
			this.lblDistance.Visible = this.EurovalHeating || this.EurovalCooling || this.ConcreteActivationHeating || this.ConcreteActivationCooling;
			this.lblAssumptions.Visible = this.Heating || this.Cooling;
			if (!updateControlOngoing)
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
					result = MessageBox.Show(EuroplanRes.QuickDimensioningPanel_BkaEntfernenText, EuroplanRes.QuickDimensioningPanel_BkaEntfernenTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
			this.cmbDistance.Visible = this.EurovalHeating || this.EurovalCooling || this.ConcreteActivationHeating || this.ConcreteActivationCooling;
			this.lblDistance.Visible = this.EurovalHeating || this.EurovalCooling || this.ConcreteActivationHeating || this.ConcreteActivationCooling;
			this.lblAssumptions.Visible = this.Heating || this.Cooling;
			if (!updateControlOngoing)
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
					result = MessageBox.Show(EuroplanRes.QuickDimensioningPanel_HithermEntfernenText, EuroplanRes.QuickDimensioningPanel_HithermEntfernenTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
			if (!updateControlOngoing)
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
					result = MessageBox.Show(EuroplanRes.QuickDimensioningPanel_HithermEntfernenText, EuroplanRes.QuickDimensioningPanel_HithermEntfernenTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
			if (!updateControlOngoing)
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
					result = MessageBox.Show(EuroplanRes.QuickDimensioningPanel_HithermCompactEntfernenText, EuroplanRes.QuickDimensioningPanel_HithermCompactEntfernenTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
			if (!updateControlOngoing)
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
					result = MessageBox.Show(EuroplanRes.QuickDimensioningPanel_HithermCompactEntfernenText, EuroplanRes.QuickDimensioningPanel_HithermCompactEntfernenTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
			if (!updateControlOngoing)
				Project.Instance.QuickDimensioning.HithermCompactCheckState = (this.HithermCompactHeating ? QuickDimensioning.ProductCheckState.Heat : QuickDimensioning.ProductCheckState.None) | (this.HithermCompactCooling ? QuickDimensioning.ProductCheckState.Cool : QuickDimensioning.ProductCheckState.None);
		}


		private void cbHithermCompactRoofHeat_CheckedChanged(object sender, EventArgs e) {
			DialogResult result = DialogResult.None;
			if (!this.HithermCompactRoofHeating && !this.HithermCompactRoofCooling) {
				bool productFound = false;
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						if (room.GetProductForQuickDimensioning<HithermCompactRoofProduct>() != null) {
							productFound = true;
						}
					}
				}
				if (productFound) {
					result = MessageBox.Show(EuroplanRes.QuickDimensioningPanel_HithermCompactDachEntfernenText, EuroplanRes.QuickDimensioningPanel_HithermCompactDachEntfernenTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				}
			}
			if (result == DialogResult.No) {
				this.HithermCompactRoofHeating = !this.HithermCompactRoofHeating;
			} else {
				foreach (QuickDimensioningFloorGrid grid in this.grids.Values) {
					grid.HithermCompactRoof = this.HithermCompactRoofHeating || this.HithermCompactRoofCooling;
					grid.Cooling = this.Cooling;
				}
				this.quickDimensioningDistributorsSummary.HithermCompactRoof = this.HithermCompactRoofHeating || this.HithermCompactRoofCooling;
				if (result == DialogResult.Yes) {
					foreach (Floor floor in Project.Instance.Floors) {
						foreach (Room room in floor.Rooms) {
							HithermCompactRoofProduct product = room.GetProductForQuickDimensioning<HithermCompactRoofProduct>();
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
			if (!updateControlOngoing)
				Project.Instance.QuickDimensioning.HithermCompactRoofCheckState = (this.HithermCompactRoofHeating ? QuickDimensioning.ProductCheckState.Heat : QuickDimensioning.ProductCheckState.None) | (this.HithermCompactRoofCooling ? QuickDimensioning.ProductCheckState.Cool : QuickDimensioning.ProductCheckState.None);
		}

		private void cbHithermCompactRoofCool_CheckedChanged(object sender, EventArgs e) {
			DialogResult result = DialogResult.None;
			if (!this.HithermCompactRoofHeating && !this.HithermCompactRoofCooling) {
				bool productFound = false;
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						if (room.GetProductForQuickDimensioning<HithermCompactRoofProduct>() != null) {
							productFound = true;
						}
					}
				}
				if (productFound) {
					result = MessageBox.Show(EuroplanRes.QuickDimensioningPanel_HithermCompactDachEntfernenText, EuroplanRes.QuickDimensioningPanel_HithermCompactDachEntfernenTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				}
			}
			if (result == DialogResult.No) {
				this.HithermCompactRoofCooling = !this.HithermCompactRoofCooling;
			} else {
				foreach (QuickDimensioningFloorGrid grid in this.grids.Values) {
					grid.HithermCompactRoof = this.HithermCompactRoofHeating || this.HithermCompactRoofCooling;
					grid.Cooling = this.Cooling;
				}
				this.quickDimensioningDistributorsSummary.HithermCompactRoof = this.HithermCompactRoofHeating || this.HithermCompactRoofCooling;
				if (result == DialogResult.Yes) {
					foreach (Floor floor in Project.Instance.Floors) {
						foreach (Room room in floor.Rooms) {
							HithermCompactRoofProduct product = room.GetProductForQuickDimensioning<HithermCompactRoofProduct>();
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
			if (!updateControlOngoing)
				Project.Instance.QuickDimensioning.HithermCompactRoofCheckState = (this.HithermCompactRoofHeating ? QuickDimensioning.ProductCheckState.Heat : QuickDimensioning.ProductCheckState.None) | (this.HithermCompactRoofCooling ? QuickDimensioning.ProductCheckState.Cool : QuickDimensioning.ProductCheckState.None);
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
					result = MessageBox.Show(EuroplanRes.QuickDimensioningPanel_KlimaBodenEntfernenText, EuroplanRes.QuickDimensioningPanel_KlimaBodenEntfernenTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
			if (!updateControlOngoing)
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
					result = MessageBox.Show(EuroplanRes.QuickDimensioningPanel_KlimaBodenEntfernenText, EuroplanRes.QuickDimensioningPanel_KlimaBodenEntfernenTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
			if (!updateControlOngoing)
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
					result = MessageBox.Show(EuroplanRes.QuickDimensioningPanel_KlimaDeckeEntfernenText, EuroplanRes.QuickDimensioningPanel_KlimaDeckeEntfernenTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
			if (!updateControlOngoing)
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
					result = MessageBox.Show(EuroplanRes.QuickDimensioningPanel_KlimaDeckeEntfernenText, EuroplanRes.QuickDimensioningPanel_KlimaDeckeEntfernenTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
			if (!updateControlOngoing)
				Project.Instance.QuickDimensioning.ModulDeckeCheckState = (this.ModulKlimaDeckeHeating ? QuickDimensioning.ProductCheckState.Heat : QuickDimensioning.ProductCheckState.None) | (this.ModulKlimaDeckeCooling ? QuickDimensioning.ProductCheckState.Cool : QuickDimensioning.ProductCheckState.None);
		}

		private void cmbHeatFlowTemperature_SelectedIndexChanged(object sender, EventArgs e) {
			Project.Instance.QuickDimensioning.HeatFlowTemperature = (float)Int32.Parse((string)cmbHeatFlowTemperature.SelectedItem);
			switch (cmbHeatFlowTemperature.SelectedIndex) {
				case 0:
					this.cmbDistance.SelectedIndex = (int)EurovalProduct.EurovalLayDistance.EV5;
					break;
				case 1:
					this.cmbDistance.SelectedIndex = (int)EurovalProduct.EurovalLayDistance.EV15;
					break;
				case 2:
					this.cmbDistance.SelectedIndex = (int)EurovalProduct.EurovalLayDistance.EV20;
					break;
				case 3:
					this.cmbDistance.SelectedIndex = (int)EurovalProduct.EurovalLayDistance.EV25;
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
			Project.Instance.QuickDimensioning.LayDistance = (EurovalProduct.EurovalLayDistance)this.cmbDistance.SelectedIndex;
			this.OnProjectChanged();
		}


		private void tabQuickDimensioning_Selected(object sender, TabControlEventArgs e) {
			if (e.TabPage == this.pageSummary) {
				string filename = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Reporting"), "QuickDimensioning.lst");
				try {
					listLabel1.Print(combit.ListLabel15.LlProject.List, filename, false, combit.ListLabel15.LlPrintMode.PreviewControl, combit.ListLabel15.LlBoxType.None, "", false, Path.GetDirectoryName(System.Windows.Forms.Application.CommonAppDataPath));
					GC.Collect();
				} catch (Exception ex) {
					DialogResult result = MessageBox.Show(EuroplanRes.QuickDimensioningPanel_DruckerFehlerText, EuroplanRes.QuickDimensioningPanel_DruckerFehlerTitel, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
					if (result == DialogResult.OK) {
						try {
							System.Diagnostics.Process p = new System.Diagnostics.Process();
							p.StartInfo.FileName = "rundll32.exe";
							p.StartInfo.Arguments = "printui.dll,PrintUIEntry /if /b \"Europlan 2.0 Reporting\" /f " + Environment.GetEnvironmentVariable("windir") + "\\inf\\ntprint.inf /r \"lpt1:\" /m \"HP LaserJet 4\"";
							p.Start();
							p.WaitForExit();
							listLabel1.Print(combit.ListLabel15.LlProject.List, filename, false, combit.ListLabel15.LlPrintMode.PreviewControl, combit.ListLabel15.LlBoxType.None, "", false, null);
						} catch (Exception) {
							MessageBox.Show(EuroplanRes.QuickDimensioningPanel_DruckerEinrichtungFehlerText, EuroplanRes.QuickDimensioningPanel_DruckerEinrichtungFehlerTitel, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
					listLabel1.PreviewControl = null;
					//listLabel1.Dispose();
					listLabel1 = null;
				}

				if (listLabelPreviewControl1 != null) {
					this.pageSummary.Controls.Remove(this.listLabelPreviewControl1);
					listLabelPreviewControl1.FileName = null;
					//listLabelPreviewControl1.Dispose();
					listLabelPreviewControl1 = null;
				}

				if (reportingData != null) {
					reportingData.Clear();
					reportingData.Dispose();
					reportingData = null;
				}



				this.listLabel1 = new combit.ListLabel15.ListLabel();
				reportingData = new DataSet();
				listLabelPreviewControl1 = new combit.ListLabel15.ListLabelPreviewControl();

				this.listLabel1.AutoDestination = combit.ListLabel15.LlPrintMode.PreviewControl;
				this.listLabel1.LicensingInfo = "BUaWEQ";
				this.listLabel1.MaxRTFVersion = 65280;
				this.listLabel1.NoParameterCheck = true;
				this.listLabel1.PreviewControl = this.listLabelPreviewControl1;
				this.listLabel1.Unit = combit.ListLabel15.LlUnits.Millimeter_1_100;

				//this.listLabelPreviewControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
				//			| System.Windows.Forms.AnchorStyles.Left)
				//			| System.Windows.Forms.AnchorStyles.Right)));
				this.listLabelPreviewControl1.CloseMode = combit.ListLabel15.LlPreviewControlCloseMode.DeleteFile;
				this.listLabelPreviewControl1.Dock = DockStyle.Fill;
				this.listLabelPreviewControl1.BackColor = System.Drawing.SystemColors.Control;
				this.listLabelPreviewControl1.CurrentPage = 0;
				this.listLabelPreviewControl1.ForceReadOnly = true;
				this.listLabelPreviewControl1.Location = new System.Drawing.Point(0, 0);
				this.listLabelPreviewControl1.Name = "listLabelPreviewControl1";
				this.listLabelPreviewControl1.Size = new System.Drawing.Size(906, 506);
				this.listLabelPreviewControl1.SlideshowMode = false;
				this.listLabelPreviewControl1.TabIndex = 4;
				this.listLabelPreviewControl1.Text = "listLabelPreviewControl1";
				this.listLabelPreviewControl1.ToolbarButtons.Exit = combit.ListLabel15.LlButtonState.Invisible;
				this.listLabelPreviewControl1.ToolbarButtons.GotoFirst = combit.ListLabel15.LlButtonState.Default;
				this.listLabelPreviewControl1.ToolbarButtons.GotoLast = combit.ListLabel15.LlButtonState.Default;
				this.listLabelPreviewControl1.ToolbarButtons.GotoNext = combit.ListLabel15.LlButtonState.Default;
				this.listLabelPreviewControl1.ToolbarButtons.GotoPrev = combit.ListLabel15.LlButtonState.Default;
				this.listLabelPreviewControl1.ToolbarButtons.PageRange = combit.ListLabel15.LlButtonState.Default;
				this.listLabelPreviewControl1.ToolbarButtons.PrintAllPages = combit.ListLabel15.LlButtonState.Default;
				this.listLabelPreviewControl1.ToolbarButtons.PrintCurrentPage = combit.ListLabel15.LlButtonState.Default;
				this.listLabelPreviewControl1.ToolbarButtons.PrintToFax = combit.ListLabel15.LlButtonState.Invisible;
				this.listLabelPreviewControl1.ToolbarButtons.SaveAs = combit.ListLabel15.LlButtonState.Default;
				this.listLabelPreviewControl1.ToolbarButtons.SendTo = combit.ListLabel15.LlButtonState.Invisible;
				this.listLabelPreviewControl1.ToolbarButtons.SlideshowMode = combit.ListLabel15.LlButtonState.Default;
				this.listLabelPreviewControl1.ToolbarButtons.ZoomCombo = combit.ListLabel15.LlButtonState.Default;
				this.listLabelPreviewControl1.ToolbarButtons.ZoomReset = combit.ListLabel15.LlButtonState.Default;
				this.listLabelPreviewControl1.ToolbarButtons.ZoomRevert = combit.ListLabel15.LlButtonState.Default;
				this.listLabelPreviewControl1.ToolbarButtons.ZoomTimes2 = combit.ListLabel15.LlButtonState.Default;
				this.listLabelPreviewControl1.SaveAsFileName = Project.Instance.ProjectFileName.Replace(".e2p", "");
				this.pageSummary.Controls.Add(this.listLabelPreviewControl1);

				List<QuickDimensioningReportWrapper> reportWrapper = Project.Instance.QuickDimensioning.GetQuickDimensioningRoomReports();
				DataTable rooms = ReportHelper.ListToDataTable<QuickDimensioningReportWrapper>(reportWrapper);
				DataTable distributors = ReportHelper.ListToDataTable<QuickDimensioningDistributorsReportWrapper>(Project.Instance.QuickDimensioning.GetQuickDimensioningDistributorsReports());
				rooms.TableName = "QuickDimensioningReportWrapper";
				distributors.TableName = "QuickDimensioningDistributorsReportWrapper";

				reportingData.Tables.Add(rooms);
				reportingData.Tables.Add(distributors);

				listLabel1.DataSource = reportingData;

				string projectName = "";
				foreach (string line in Project.Instance.ProjectName) {
					projectName += line + "\n";
				}
				string comments = "";
				foreach (string line in Project.Instance.ProjectNotes) {
					comments += line + "\n";
				}
				projectName = projectName.TrimEnd();
				listLabel1.Variables.Add("@ProjectNumber", Project.Instance.ProjectNumber.TrimEnd());
				listLabel1.Variables.Add("@ProjectName", projectName);
				listLabel1.Variables.Add("@ProjectEditor", Project.Instance.ProjectEditor);
				listLabel1.Variables.Add("@Comments", comments);
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
							usedRoomTypes += roomType.Name + ": " + roomType.HeatLoadPerSquareMeter + EuroplanRes.Unit_WattProQm + " - " + roomType.CoolLoadPerSquareMeter + EuroplanRes.Unit_WattProQm + "\n";
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

				listLabel1.Variables.Add("@FileName", Path.GetFileName(Project.Instance.ProjectFileName));

				listLabel1.Dictionary.Clear();
				ResourceSet resourceSet = EuroplanRes.ResourceManager.GetResourceSet(Thread.CurrentThread.CurrentCulture, false, true);
				if (resourceSet != null) {
					IDictionaryEnumerator enumerator = resourceSet.GetEnumerator();
					while (enumerator.MoveNext()) {
						if (enumerator.Key is string) {
							string key = enumerator.Key as string;
							if (key.StartsWith("LL_") || key.StartsWith("Unit_")) {
								listLabel1.Variables.Add("@" + key, ((string)enumerator.Value).Replace("\r", ""));
							}
						}
					}
				}

				// ---------------------------------------------------------------------------------------------------
				// RoomControllers are now aggregated in the report itself
				// ---------------------------------------------------------------------------------------------------
				//Dictionary<Room.RoomController, int> roomControllers = new Dictionary<Room.RoomController, int>();
				//foreach (Floor floor in Project.Instance.Floors) {
				//    foreach (Room room in floor.Rooms) {
				//        if (room.QuickDimensioningRoomController != Room.RoomController.None) {
				//            if (!roomControllers.ContainsKey(room.QuickDimensioningRoomController)) {
				//                roomControllers.Add(room.QuickDimensioningRoomController, 1);
				//            } else {
				//                roomControllers[room.QuickDimensioningRoomController] = roomControllers[room.QuickDimensioningRoomController] + 1;
				//            }
				//        }
				//    }
				//}
				//string controllersSummary = null;
				//string localized = "";
				//foreach (KeyValuePair<Room.RoomController, int> kvp in roomControllers) {
				//    if (controllersSummary != null) {
				//        controllersSummary += ", ";
				//    } else {
				//        controllersSummary = "";
				//    }
				//    localized = resources.GetString(kvp.Key.ToString(), Thread.CurrentThread.CurrentUICulture);
				//    controllersSummary += kvp.Value.ToString() + " * " + localized;
				//}
				//listLabel1.Variables.Add("@RoomControllers", controllersSummary);
				// ---------------------------------------------------------------------------------------------------
#if DEBUG
				if (MessageBox.Show("Designer?", "", MessageBoxButtons.YesNo) == DialogResult.Yes) {
					listLabel1.Design();
				}
#endif
				Cursor.Current = current;
			} else if (e.TabPage == this.pageDistributors) {
				this.quickDimensioningDistributorsSummary.UpdateControl();
			}
			
		}

		private void OnProjectChanged() {
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
		}

		private void btnRevert_Click(object sender, EventArgs e) {
			if (MessageBox.Show(EuroplanRes.QuickDimensioningPanel_ZuruecksetzenBestaetigungText, EuroplanRes.QuickDimensioningPanel_ZuruecksetzenBestaetigungTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
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
				qd.HithermCompactRoofCheckState = QuickDimensioning.ProductCheckState.None;
				qd.ModulBodenCheckState = QuickDimensioning.ProductCheckState.None;
				qd.ModulDeckeCheckState = QuickDimensioning.ProductCheckState.None;
				// TODO revert parameters

				this.UpdateControl(true);
			}
		}

		private void quickDimensioningDistributorsSummary_ProjectChanged(object sender) {
			this.OnProjectChanged();
		}

		private void lblDistance_Click(object sender, EventArgs e) {

		}

	}
}
