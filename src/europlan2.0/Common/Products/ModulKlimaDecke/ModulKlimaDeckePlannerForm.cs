using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;
using WW.Math;

namespace Europlan.Common.Products {
	public partial class ModulKlimaDeckePlannerForm : Form {

		private ModulKlimaDeckeConstructionGlatt glatt = null;
		private ModulKlimaDeckeConstructionAkustik akustik = null;
		private ModulKlimaDeckeConstructionKassette kassette = null;

		private int ignoreRotation = 0;
		private bool newVisible = false;
		private PlannedProduct plannedProduct;

		private bool changed = false;
		private bool updateOngoing = false;

		public ModulKlimaDeckePlannerForm(PlannedProduct plannedProduct) {
			updateOngoing = true;
			InitializeComponent();
			switch (Product.ConfigPlanMeasureEnum) {
				case Product.PlanMeasureEnum.PM_CENTIMETER:
					this.lblBeplankungBreiteUnit.Text = "cm";
					this.lblBeplankungLaengeUnit.Text = "cm";
					this.numBeplankungBreite.EditType = NumericBox.NumericEditType.BEPLANKUNG_CM;
					this.numBeplankungLaenge.EditType = NumericBox.NumericEditType.BEPLANKUNG_CM;
					break;

				case Product.PlanMeasureEnum.PM_MILLIMETER:
					this.lblBeplankungBreiteUnit.Text = "mm";
					this.lblBeplankungLaengeUnit.Text = "mm";
					this.numBeplankungBreite.EditType = NumericBox.NumericEditType.BEPLANKUNG_MM;
					this.numBeplankungLaenge.EditType = NumericBox.NumericEditType.BEPLANKUNG_MM;
					break;

				case Product.PlanMeasureEnum.PM_METER:
				default:
					this.lblBeplankungBreiteUnit.Text = "m";
					this.lblBeplankungLaengeUnit.Text = "m";
					this.numBeplankungBreite.EditType = NumericBox.NumericEditType.BEPLANKUNG_M;
					this.numBeplankungLaenge.EditType = NumericBox.NumericEditType.BEPLANKUNG_M;
					break;
			}
			this.plannedProduct = plannedProduct;
			this.cbAutomaticOrientation.Checked = this.modulKlimaDeckePlanner.AutomaticOrientation;
			this.cbAutomaticRows.Checked = this.modulKlimaDeckePlanner.AutomaticRows;
			ModulKlimaDeckeProduct product = plannedProduct.Product as ModulKlimaDeckeProduct;

			ModulKlimaDeckeProduct.ModulCeilingConstructionEnum defaultConstrType = (ModulKlimaDeckeProduct.ModulCeilingConstructionEnum)ModulKlimaDeckeProduct.ConfigModulCeilingConstruction;

			this.modulKlimaDeckePlanner.Product = product;
			if (product.GraphConstruction == null) {
				product.GraphConstruction = (defaultConstrType == ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.KASSETTENDECKE ? (ModulKlimaDeckeConstruction)new ModulKlimaDeckeConstructionKassette() : (ModulKlimaDeckeConstruction)new ModulKlimaDeckeConstructionGlatt());
				product.GraphConstruction.Planner = this.modulKlimaDeckePlanner;
				product.GraphConstruction.RotationRelativeToPlan = 0;
			}
			this.glatt = product.GraphConstruction as ModulKlimaDeckeConstructionGlatt;
			this.akustik = product.GraphConstruction as ModulKlimaDeckeConstructionAkustik;
			this.kassette = product.GraphConstruction as ModulKlimaDeckeConstructionKassette;

			if (this.glatt == null) {
				this.glatt = new ModulKlimaDeckeConstructionGlatt();
				this.glatt.Planner = this.modulKlimaDeckePlanner;
				this.glatt.RotationRelativeToPlan = 0;
			} else {
				this.glatt.Planner = this.modulKlimaDeckePlanner;
			}
			this.glatt.RecalculateSchienen();

			if (this.akustik == null) {
				this.akustik = new ModulKlimaDeckeConstructionAkustik();
				this.akustik.Planner = this.modulKlimaDeckePlanner;
				this.akustik.RotationRelativeToPlan = 0;
			} else {
				this.akustik.Planner = this.modulKlimaDeckePlanner;
			}
			this.numRandfries.Value = (decimal)Math.Round(this.akustik.Randfries * 100.0);
			this.akustik.RecalculateSchienen();

			if (this.kassette == null) {
				this.kassette = new ModulKlimaDeckeConstructionKassette();
				this.kassette.Planner = this.modulKlimaDeckePlanner;
				this.kassette.RotationRelativeToPlan = 0;
			} else {
				this.kassette.Planner = this.modulKlimaDeckePlanner;
			}
			this.kassette.RecalculateSchienen();


			if (product.GraphConstruction.CeilingConstruction == ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.C_PROFIL) {
				this.rbCProfil.Checked = true;
				this.rbHolzstaffel.Checked = false;
				this.rbKassettendecke.Checked = false;
				//this.rbGlatt.Visible = true;
				//this.rbAkustik.Visible = true;
				//this.rbKassetten.Visible = false;
				this.grpBeplankung.Enabled = true;
				this.grpCeilingContruction.Visible = true;
			} else if (product.GraphConstruction.CeilingConstruction == ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.HOLZSTAFFEL) {
				this.rbCProfil.Checked = false;
				this.rbHolzstaffel.Checked = true;
				this.rbKassettendecke.Checked = false;
				//this.rbGlatt.Visible = true;
				//this.rbAkustik.Visible = true;
				//this.rbKassetten.Visible = false;
				this.grpBeplankung.Enabled = true;
				this.grpCeilingContruction.Visible = true;
			} else if (product.GraphConstruction.CeilingConstruction == ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.KASSETTENDECKE) {
				this.rbCProfil.Checked = false;
				this.rbHolzstaffel.Checked = false;
				this.rbKassettendecke.Checked = true;
				//this.rbGlatt.Visible = false;
				//this.rbAkustik.Visible = false;
				//this.rbKassetten.Visible = true;
				this.grpBeplankung.Enabled = false;
				this.grpCeilingContruction.Visible = false;
			}

			if (product.GraphConstruction is ModulKlimaDeckeConstructionGlatt) {
				this.rbGlatt.Checked = true;
				ignoreBeplankung++;
				if (this.glatt.Beplankung.HasValue) {
					this.numBeplankungLaenge.Value = (decimal)(this.glatt.Beplankung.Value.X * Product.ConfigPlanMeasureMultiplier);
					this.numBeplankungBreite.Value = (decimal)(this.glatt.Beplankung.Value.Y * Product.ConfigPlanMeasureMultiplier);
					this.cbBeplankung.Checked = true;
				} else {
					this.cbBeplankung.Checked = false;
					this.numBeplankungLaenge.Text = "";
					this.numBeplankungBreite.Text = "";
				}
				ignoreBeplankung--;
			} else if (product.GraphConstruction is ModulKlimaDeckeConstructionAkustik) {
				this.rbAkustik.Checked = true;
				ignoreBeplankung++;
				if (this.akustik.Beplankung.HasValue) {
					this.numBeplankungLaenge.Value = (decimal)(this.akustik.Beplankung.Value.X * Product.ConfigPlanMeasureMultiplier);
					this.numBeplankungBreite.Value = (decimal)(this.akustik.Beplankung.Value.Y * Product.ConfigPlanMeasureMultiplier);
					this.cbBeplankung.Checked = true;
				} else {
					this.cbBeplankung.Checked = false;
					this.numBeplankungLaenge.Text = "";
					this.numBeplankungBreite.Text = "";
				}
				ignoreBeplankung--;
			} else if (product.GraphConstruction is ModulKlimaDeckeConstructionKassette) {
				//this.rbKassetten.Checked = true;
			}

			if (this.cbBeplankung.Checked) {
				this.numBeplankungBreite.Enabled = true;
				this.numBeplankungLaenge.Enabled = true;
				this.numBeplankungLaenge.ReadOnly = false;
				this.numBeplankungBreite.ReadOnly = false;
			} else {
				this.numBeplankungBreite.Enabled = false;
				this.numBeplankungLaenge.Enabled = false;
				this.numBeplankungLaenge.ReadOnly = true;
				this.numBeplankungBreite.ReadOnly = true;
			}

			this.cmbOrientation.Items.Add(KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT);
			this.cmbOrientation.Items.Add(KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT);
			this.cmbOrientation.SelectedIndex = 0;
			this.cmbSelectedModuleOrientation.Items.Add(KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT);
			this.cmbSelectedModuleOrientation.Items.Add(KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT);
			this.cmbSelectedModuleOrientation.SelectedIndex = -1;
			this.cmbSelectedModuleOrientation.Enabled = false;

			this.cmbModulType.Items.Clear();
			this.cmbSelectedModuleType.Items.Clear();

			if (product.GraphConstruction is ModulKlimaDeckeConstructionGlatt || product.GraphConstruction is ModulKlimaDeckeConstructionAkustik) {
				this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_80_30);
				this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_30);
				this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_120_30);
				this.cmbModulType.SelectedIndex = 2;
				this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_80_30);
				this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_30);
				this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_120_30);
				this.cmbSelectedModuleType.SelectedIndex = -1;
			} else if (product.GraphConstruction is ModulKlimaDeckeConstructionKassette) {
				if (this.kassette.Raster == ModulKlimaDeckeConstructionKassette.RasterMass.Raster_1050_450) {
					this.rb1050.Checked = true;
					this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40);
					this.cmbModulType.SelectedIndex = 0;
					this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40);
					this.cmbSelectedModuleType.SelectedIndex = -1;
				} else if (this.kassette.Raster == ModulKlimaDeckeConstructionKassette.RasterMass.Raster_625) {
					this.rb625.Checked = true;
					this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60);
					this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B);
					this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60C);
					this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60D);
					this.cmbModulType.SelectedIndex = 0;
					this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60);
					this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B);
					this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60C);
					this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60D);
					this.cmbSelectedModuleType.SelectedIndex = -1;
				} else if (this.kassette.Raster == ModulKlimaDeckeConstructionKassette.RasterMass.Raster_600) {
					this.rb600.Checked = true;
					this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60);
					this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B);
					this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60C);
					this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60D);
					this.cmbModulType.SelectedIndex = 0;
					this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60);
					this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B);
					this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60C);
					this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60D);
					this.cmbSelectedModuleType.SelectedIndex = -1;
				}
			}

			this.cmbSelectedModuleType.Enabled = false;
			this.lblTypError.Text = "Kein Modul ausgewählt";

			this.UpdateLists(true, true, true, false);

			if (product.ContainsModules) {
				this.tabs.SelectedTab = this.pageLayout;
			} else {
				this.tabs.SelectedTab = this.pageConstruction;
			}
			updateOngoing = false;
			
			this.UpdateToolbar(this.tabs.SelectedTab);
			this.UpdateButtons();
			this.UpdateControls();
			this.CalculateAndUpdate();

			this.connectionPlanner.Product = product;
		}

		private void UpdateControls() {
			updateOngoing = true;
			if (this.modulKlimaDeckePlanner.Product.GraphConstruction.CeilingConstruction == ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.KASSETTENDECKE) {
				rbKassettendecke.Checked = true;
				this.grpCeilingContruction.Visible = false;
				this.grpBeplankung.Enabled = false;
			} else if (this.modulKlimaDeckePlanner.Product.GraphConstruction.CeilingConstruction == ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.C_PROFIL) {
				rbCProfil.Checked = true;
				this.grpCeilingContruction.Visible = true;
				this.grpBeplankung.Enabled = true;
			} else if (this.modulKlimaDeckePlanner.Product.GraphConstruction.CeilingConstruction == ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.HOLZSTAFFEL) {
				rbHolzstaffel.Checked = true;
				this.grpCeilingContruction.Visible = true;
				this.grpBeplankung.Enabled = true;
			}
			if (this.modulKlimaDeckePlanner.Product.GraphConstruction is ModulKlimaDeckeConstructionAkustik) {
				rbAkustik.Checked = true;
			} else if (this.modulKlimaDeckePlanner.Product.GraphConstruction is ModulKlimaDeckeConstructionGlatt) {
				rbGlatt.Checked = true;
			} else if (this.modulKlimaDeckePlanner.Product.GraphConstruction is ModulKlimaDeckeConstructionKassette) {
				//rbKassetten.Checked = true;
			}
			if (!this.rbKassettendecke.Checked && this.rbGlatt.Checked) {
				ignoreRotation++;
				this.lblRandfries.Visible = false;
				this.numRandfries.Visible = false;
				this.lblRandfriesUnit.Visible = false;
				this.grpModulSerie.Visible = true;
				this.grpRasterMass.Visible = false;
				this.numRotation.Value = (decimal)this.glatt.RotationRelativeToPlan;
				if (Math.Round(this.glatt.SchienenAbstand, 2) == 0.3) {
					if (!this.rbSerie30.Checked) {
						this.rbSerie30.Checked = true;
					}
				} else {
					if (!this.rbSerie40.Checked) {
						this.rbSerie40.Checked = true;
					}
				}
				ignoreRotation--;
			} else if (!this.rbKassettendecke.Checked && this.rbAkustik.Checked) {
				ignoreRotation++;
				this.lblRandfries.Visible = true;
				this.numRandfries.Visible = true;
				this.lblRandfriesUnit.Visible = true;
				this.grpModulSerie.Visible = true;
				this.grpRasterMass.Visible = false;
				this.numRotation.Value = (decimal)this.akustik.RotationRelativeToPlan;
				if (Math.Round(this.akustik.SchienenAbstand, 2) == 0.3) {
					if (!this.rbSerie30.Checked) {
						this.rbSerie30.Checked = true;
					}
				} else {
					if (!this.rbSerie40.Checked) {
						this.rbSerie40.Checked = true;
					}
				}
				ignoreRotation--;
			} else if (this.rbKassettendecke.Checked) {
				ignoreRotation++;
				this.lblRandfries.Visible = false;
				this.numRandfries.Visible = false;
				this.lblRandfriesUnit.Visible = false;
				this.grpModulSerie.Visible = false;
				this.grpRasterMass.Visible = true;
				this.numRotation.Value = (decimal)this.kassette.RotationRelativeToPlan;
				if (this.kassette.Raster == ModulKlimaDeckeConstructionKassette.RasterMass.Raster_1050_450) {
					this.rb1050.Checked = true;
				} else if (this.kassette.Raster == ModulKlimaDeckeConstructionKassette.RasterMass.Raster_625) {
					this.rb625.Checked = true;
				} else if (this.kassette.Raster == ModulKlimaDeckeConstructionKassette.RasterMass.Raster_600) {
					this.rb600.Checked = true;
				}	
				ignoreRotation--;
			} else {
			}
			updateOngoing = false;
		}

		private void btnZoomOut_Click(object sender, EventArgs e) {
			this.planPanel.AddScale(0.9, null);
		}

		private void btnZoomIn_Click(object sender, EventArgs e) {
			this.planPanel.AddScale(1.1, null);
		}

		private void btnMove_Click(object sender, EventArgs e) {
			if (!btnMove.Checked) {
				this.modulKlimaDeckePlanner.Mode = ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_NONE;
				this.planPanel.Mode = PlanMode.PM_MOVE;
				this.UpdateButtons();
			}
		}

		private void btnConstruction_Click(object sender, EventArgs e) {
			if (!btnConstruction.Checked) {
				this.SetProductPlanner();
				this.modulKlimaDeckePlanner.Mode = ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_CONSTRUCTION;
				this.planPanel.Mode = PlanMode.PM_PLANNER_DRAG;
				this.UpdateButtons();
			}
		}

		private void btnAddModules_Click(object sender, EventArgs e) {
			if (!btnAddModules.Checked) {
				this.SetProductPlanner();
				this.modulKlimaDeckePlanner.Mode = ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_LAYOUT_ADD_AREA;
				this.planPanel.Mode = PlanMode.PM_PLANNER_DRAG;
				this.UpdateButtons();
			}
		}

		private void btnSelectModule_Click(object sender, EventArgs e) {
			if (!btnSelectModule.Checked) {
				this.SetProductPlanner();
				this.modulKlimaDeckePlanner.Mode = ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_PICK_MODULE;
				this.planPanel.Mode = PlanMode.PM_PLANNER_DRAG;
				this.UpdateButtons();
			}
		}

		private void btnAddConnections_Click(object sender, EventArgs e) {
			if (!this.btnAddConnections.Checked) {
				this.SetProductPlanner();
				this.modulKlimaDeckePlanner.Mode = ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_ADD_CONNECTION;
				this.planPanel.Mode = PlanMode.PM_PLANNER_CLICK;
				this.UpdateButtons();
			}
		}

		private void btnDeleteConnection_Click(object sender, EventArgs e) {
			if (!this.btnDeleteConnection.Checked) {
				this.SetProductPlanner();
				this.modulKlimaDeckePlanner.Mode = ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_DEL_CONNECTION;
				this.planPanel.Mode = PlanMode.PM_PLANNER_CLICK;
				this.UpdateButtons();
			}
		}

		private void btnAddAnbindeleitungen_Click(object sender, EventArgs e) {
			if (!this.btnAddAnbindeleitungen.Checked) {
				this.SetConnectionPlanner();
				this.planPanel.Mode = PlanMode.PM_PLANNER_CLICK;
				this.connectionPlanner.Mode = ConnectionPlanner.ConnectionMode.KDM_ADD_CONNECTION;
				this.UpdateButtons();
			}
		}

		private void btnSelectAnbindeleitungen_Click(object sender, EventArgs e) {
			if (!this.btnSelectAnbindeleitungen.Checked) {
				this.SetConnectionPlanner();
				this.planPanel.Mode = PlanMode.PM_PLANNER_DRAG;
				this.connectionPlanner.Mode = ConnectionPlanner.ConnectionMode.KDM_SELECT_CONNECTION;
				this.UpdateButtons();
			}
		}

		private void UpdateButtons() {
			if (this.planPanel.Mode == PlanMode.PM_MOVE) {
				this.btnMove.Checked = true;
				this.btnConstruction.Checked = false;
				this.btnAddModules.Checked = false;
				this.btnSelectModule.Checked = false;
				this.btnAddConnections.Checked = false;
				this.btnDeleteConnection.Checked = false;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = false;
			} else if (this.planPanel.ProductPlanner == this.modulKlimaDeckePlanner && (this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.modulKlimaDeckePlanner.Mode == ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_CONSTRUCTION) {
				this.btnMove.Checked = false;
				this.btnConstruction.Checked = true;
				this.btnAddModules.Checked = false;
				this.btnSelectModule.Checked = false;
				this.btnAddConnections.Checked = false;
				this.btnDeleteConnection.Checked = false;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = false;
			} else if (this.planPanel.ProductPlanner == this.modulKlimaDeckePlanner && (this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.modulKlimaDeckePlanner.Mode == ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_LAYOUT_ADD_AREA) {
				this.btnMove.Checked = false;
				this.btnConstruction.Checked = false;
				this.btnAddModules.Checked = true;
				this.btnSelectModule.Checked = false;
				this.btnAddConnections.Checked = false;
				this.btnDeleteConnection.Checked = false;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = false;
			} else if (this.planPanel.ProductPlanner == this.modulKlimaDeckePlanner && (this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.modulKlimaDeckePlanner.Mode == ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_PICK_MODULE) {
				this.btnMove.Checked = false;
				this.btnConstruction.Checked = false;
				this.btnAddModules.Checked = false;
				this.btnSelectModule.Checked = true;
				this.btnAddConnections.Checked = false;
				this.btnDeleteConnection.Checked = false;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = false;
			} else if (this.planPanel.ProductPlanner == this.modulKlimaDeckePlanner && (this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.modulKlimaDeckePlanner.Mode == ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_ADD_CONNECTION) {
				this.btnMove.Checked = false;
				this.btnConstruction.Checked = false;
				this.btnAddModules.Checked = false;
				this.btnSelectModule.Checked = false;
				this.btnAddConnections.Checked = true;
				this.btnDeleteConnection.Checked = false;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = false;
			} else if (this.planPanel.ProductPlanner == this.modulKlimaDeckePlanner && (this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.modulKlimaDeckePlanner.Mode == ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_ADD_CONNECTION) {
				this.btnMove.Checked = false;
				this.btnConstruction.Checked = false;
				this.btnAddModules.Checked = false;
				this.btnSelectModule.Checked = false;
				this.btnAddConnections.Checked = false;
				this.btnDeleteConnection.Checked = true;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = false;
			} else if (this.planPanel.ProductPlanner == this.connectionPlanner && (this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.connectionPlanner.Mode == ConnectionPlanner.ConnectionMode.KDM_ADD_CONNECTION) {
				this.btnMove.Checked = false;
				this.btnConstruction.Checked = false;
				this.btnAddModules.Checked = false;
				this.btnSelectModule.Checked = false;
				this.btnAddConnections.Checked = false;
				this.btnDeleteConnection.Checked = false;
				this.btnAddAnbindeleitungen.Checked = true;
				this.btnSelectAnbindeleitungen.Checked = false;
			} else if (this.planPanel.ProductPlanner == this.connectionPlanner && (this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.connectionPlanner.Mode == ConnectionPlanner.ConnectionMode.KDM_SELECT_CONNECTION) {
				this.btnMove.Checked = false;
				this.btnConstruction.Checked = false;
				this.btnAddModules.Checked = false;
				this.btnSelectModule.Checked = false;
				this.btnAddConnections.Checked = false;
				this.btnDeleteConnection.Checked = false;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = true;
			} else {
				this.btnMove.Checked = false;
				this.btnConstruction.Checked = false;
				this.btnAddModules.Checked = false;
				this.btnSelectModule.Checked = false;
				this.btnAddConnections.Checked = false;
				this.btnDeleteConnection.Checked = false;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = false;
			}
			this.grpSelectedModules.Visible = this.btnSelectModule.Checked;
			this.grpSelection.Visible = this.btnSelectModule.Checked;
			this.grpNewModules.Visible = this.btnAddModules.Checked;
			this.grpAutomatic.Visible = this.btnAddModules.Checked;
			if ((this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.modulKlimaDeckePlanner.Mode == ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_LAYOUT_ADD_AREA) {
				if (!this.newVisible) {
					this.newVisible = true;
					this.ignoreListChange++;
					lstCircuits.Items.Add("neuer HK");
					lstSubarea.Items.Add("neue Teilfläche");
					lstRows.Items.Add("neue Reihen");
					this.ignoreListChange--;
				}
			} else {
				if (this.newVisible) {
					this.newVisible = false;
					this.ignoreListChange++;
					this.lstCircuits.Items.RemoveAt(this.lstCircuits.Items.Count - 1);
					this.lstSubarea.Items.RemoveAt(this.lstSubarea.Items.Count - 1);
					this.lstRows.Items.RemoveAt(this.lstRows.Items.Count - 1);
					this.ignoreListChange--;
				}
			}
			this.UpdateLists(true, true, true, false);
		}

		private KlimaFlaechenList GetSelectedRow() {
			return this.modulKlimaDeckePlanner.HighlightRow;
		}

		private ModulDeckeCircuit GetSelectedCircuit() {
			return this.modulKlimaDeckePlanner.HighlightCircuit;
		}

		private void rbGlatt_CheckedChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (rbGlatt.Checked && this.modulKlimaDeckePlanner.Product.GraphConstruction != this.glatt) {
					this.modulKlimaDeckePlanner.Product.GraphConstruction = this.glatt;
					this.changed = true;
					this.UpdateControls();
					this.planPanel.InvalidateGraphics();
				}
			}
		}

		private void rbAkustik_CheckedChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (rbAkustik.Checked && this.modulKlimaDeckePlanner.Product.GraphConstruction != this.akustik) {
					this.modulKlimaDeckePlanner.Product.GraphConstruction = this.akustik;
					this.changed = true;
					this.UpdateControls();
					this.planPanel.InvalidateGraphics();
				}
			}
		}

		/*private void rbKassetten_CheckedChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (rbKassetten.Checked && this.modulKlimaDeckePlanner.Product.GraphConstruction != this.kassette) {
					this.modulKlimaDeckePlanner.Product.GraphConstruction = this.kassette;
					this.changed = true;
					this.UpdateControls();
					this.planPanel.InvalidateGraphics();
				}
			}
		}*/

		private void rbKassettendecke_CheckedChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (this.rbKassettendecke.Checked && this.modulKlimaDeckePlanner.Product.GraphConstruction.CeilingConstruction != ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.KASSETTENDECKE) {
					this.modulKlimaDeckePlanner.Product.GraphConstruction = this.kassette;
					this.changed = true;
					this.UpdateControls();
					this.planPanel.InvalidateGraphics();
				}
			}
		}

		private void rbCProfil_CheckedChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (this.rbCProfil.Checked && this.modulKlimaDeckePlanner.Product.GraphConstruction.CeilingConstruction != ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.C_PROFIL) {
					this.akustik.ContructionType = ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.C_PROFIL;
					this.glatt.ContructionType = ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.C_PROFIL;
					if (this.rbAkustik.Checked) {
						this.modulKlimaDeckePlanner.Product.GraphConstruction = this.akustik;
					} else {
						this.modulKlimaDeckePlanner.Product.GraphConstruction = this.glatt;
					}
					this.modulKlimaDeckePlanner.Product.GraphConstruction.RecalculateSchienen();
					this.changed = true;
					this.UpdateControls();
					this.planPanel.InvalidateGraphics();
				}
			}
		}

		private void rbHolzstaffel_CheckedChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (this.rbHolzstaffel.Checked && this.modulKlimaDeckePlanner.Product.GraphConstruction.CeilingConstruction != ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.HOLZSTAFFEL) {
					this.akustik.ContructionType = ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.HOLZSTAFFEL;
					this.glatt.ContructionType = ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.HOLZSTAFFEL;
					if (this.rbAkustik.Checked) {
						this.modulKlimaDeckePlanner.Product.GraphConstruction = this.akustik;
					} else {
						this.modulKlimaDeckePlanner.Product.GraphConstruction = this.glatt;
					}
					this.modulKlimaDeckePlanner.Product.GraphConstruction.RecalculateSchienen();
					this.changed = true;
					this.UpdateControls();
					this.planPanel.InvalidateGraphics();
				}
			}
		}

		private void numAusrichtung_ValueChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (ignoreRotation == 0) {
					this.changed = true;
					if (this.modulKlimaDeckePlanner.Product.GraphConstruction is ModulKlimaDeckeConstructionAkustik) {
						ModulKlimaDeckeConstructionAkustik akustik = this.modulKlimaDeckePlanner.Product.GraphConstruction as ModulKlimaDeckeConstructionAkustik;
						akustik.RotationRelativeToPlan = (double)this.numRotation.Value;
						this.planPanel.InvalidateGraphics();
					} else if (this.modulKlimaDeckePlanner.Product.GraphConstruction is ModulKlimaDeckeConstructionGlatt) {
						ModulKlimaDeckeConstructionGlatt glatt = this.modulKlimaDeckePlanner.Product.GraphConstruction as ModulKlimaDeckeConstructionGlatt;
						glatt.RotationRelativeToPlan = (double)this.numRotation.Value;
						this.planPanel.InvalidateGraphics();
					} else if (this.modulKlimaDeckePlanner.Product.GraphConstruction is ModulKlimaDeckeConstructionKassette) {
						ModulKlimaDeckeConstructionKassette kassette = this.modulKlimaDeckePlanner.Product.GraphConstruction as ModulKlimaDeckeConstructionKassette;
						kassette.RotationRelativeToPlan = (double)this.numRotation.Value;
						this.planPanel.InvalidateGraphics();
					}
				}
			}
		}

		private decimal smallRotate = (decimal)0.5;
		private decimal largeRotate = 5;

		private void btnRotate_Click(object sender, EventArgs e) {
			this.changed = true;
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

		private void btnHorizontal_Click(object sender, EventArgs e) {
			this.changed = true;
			this.numRotation.Value = 90;
		}

		private void btnVertical_Click(object sender, EventArgs e) {
			this.changed = true;
			this.numRotation.Value = 0;
		}

		private TabPage previousTab = null;

		private void tabs_Selecting(object sender, TabControlCancelEventArgs e) {
			if ((previousTab == this.pageLayout || previousTab == this.pageCalculations) && e.TabPage == this.pageConstruction) {
				if (this.modulKlimaDeckePlanner.Product.ContainsModules) {
					if (MessageBox.Show("Wenn Sie die Konstruktion ändern wollen, werden alle bereits verplanten Module gelöscht!", "Bestätigen", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK) {
						e.Cancel = true;
					} else {
						this.modulKlimaDeckePlanner.Product.PlannedCircuits.Clear();
						this.modulKlimaDeckePlanner.Product.PlannedCircuits.Add(new ModulDeckeCircuit());
					}
				}
			}
			if (!e.Cancel) {
				this.UpdateToolbar(e.TabPage);
				this.planPanel.InvalidateGraphics();
			}
		}

		private void UpdateToolbar(TabPage tabPage) {
			if (tabPage == this.pageLayout) {
				this.btnConstruction.Visible = false;
				this.btnAddModules.Visible = true;
				this.btnSelectModule.Visible = true;
				this.btnAddConnections.Visible = true;
				this.btnDeleteConnection.Visible = true;
				this.btnAddAnbindeleitungen.Visible = true;
				this.btnSelectAnbindeleitungen.Visible = true;
				if (!this.btnAddModules.Checked && !this.btnSelectModule.Checked && !this.btnMove.Checked) {
					this.planPanel.Mode = PlanMode.PM_MOVE;
					this.modulKlimaDeckePlanner.Mode = ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_NONE;
					this.UpdateButtons();
				}
			} else if (tabPage == this.pageConstruction) {
				this.btnAddModules.Visible = false;
				this.btnSelectModule.Visible = false;
				this.btnConstruction.Visible = true;
				this.btnAddConnections.Visible = false;
				this.btnDeleteConnection.Visible = false;
				this.btnAddAnbindeleitungen.Visible = false;
				this.btnSelectAnbindeleitungen.Visible = false;
				if (!this.btnConstruction.Checked && !this.btnMove.Checked) {
					this.planPanel.Mode = PlanMode.PM_MOVE;
					this.modulKlimaDeckePlanner.Mode = ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_NONE;
					this.UpdateButtons();
				}
			}
		}

		private void tabs_Deselected(object sender, TabControlEventArgs e) {
			this.previousTab = e.TabPage;
		}

		private void modulKlimaBodenPlanner_ListsNeedUpdate(object sender, ModulKlimaDeckePlanner.ListNeedsUpdateEventArgs e) {
			this.UpdateLists(true, true, true, e.selectLastCircuit);
		}

		private void UpdateLists(bool updateCircuits, bool updateSubareas, bool updateRows, bool selectLastCircuit) {
			List<Circuit> circuits = (this.modulKlimaDeckePlanner.Product.ContainsModules ? this.modulKlimaDeckePlanner.Product.PlannedCircuits : new List<Circuit>());

			int dec = this.newVisible ? 1 : 0;
			this.newVisible = this.modulKlimaDeckePlanner.Mode == ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_LAYOUT_ADD_AREA && this.modulKlimaDeckePlanner.Product.PlannedCircuits.Count < 12;
			ignoreListChange++;

			if (updateCircuits) {
				int circuitsCount = circuits.Count;
				int tmp = (lstCircuits.SelectedIndex == lstCircuits.Items.Count - dec ? circuitsCount : lstCircuits.SelectedIndex);
				lstCircuits.BeginUpdate();
				lstCircuits.Items.Clear();
				for (int i = 1; i <= circuitsCount; i++) {
					lstCircuits.Items.Add(EuroplanRes.PlannedModulKlimaDeckeProductPanel_HeizkreisAbkuerzung + i.ToString());
				}
				if (this.newVisible) {
					lstCircuits.Items.Add("neuer HK");
				}
				if (selectLastCircuit) {
					if (this.newVisible && lstCircuits.Items.Count > 1) {
						lstCircuits.SelectedIndex = lstCircuits.Items.Count - 2;
					} else {
						lstCircuits.SelectedIndex = lstCircuits.Items.Count - 1;
					}
				} else {
					if (tmp < lstCircuits.Items.Count && tmp >= 0) {
						lstCircuits.SelectedIndex = tmp;
					} else {
						if (this.newVisible) {
							if (tmp < 0 && lstCircuits.Items.Count > 1) {
								lstCircuits.SelectedIndex = lstCircuits.Items.Count - 2;
							} else {
								lstCircuits.SelectedIndex = lstCircuits.Items.Count - 1;
							}
						} else {
							lstCircuits.SelectedIndex = -1;
						}
					}
				}
				lstCircuits.EndUpdate();
			}

			if (updateSubareas) {
				int subAreasCount = (this.lstCircuits.SelectedIndex < this.lstCircuits.Items.Count - dec && this.lstCircuits.SelectedIndex >= 0 ? (circuits[this.lstCircuits.SelectedIndex] as ModulDeckeCircuit).SubAreas.Count : 0);
				int tmp = (lstSubarea.SelectedIndex == lstSubarea.Items.Count - dec ? subAreasCount : lstSubarea.SelectedIndex);
				lstSubarea.BeginUpdate();
				lstSubarea.Items.Clear();
				for (int i = 1; i <= subAreasCount; i++) {
					lstSubarea.Items.Add(EuroplanRes.PlannedModulKlimaDeckeProductPanel_Teilflaeche + i.ToString());
				}
				if (this.newVisible) {
					lstSubarea.Items.Add("neue Teilfläche");
				}
				if (tmp < lstSubarea.Items.Count) {
					if (this.newVisible && tmp < 0 && lstCircuits.SelectedIndex < lstCircuits.Items.Count - dec && lstCircuits.SelectedIndex >= 0) {
						tmp = lstSubarea.Items.Count - 1;
					}
					lstSubarea.SelectedIndex = tmp;
				} else {
					lstSubarea.SelectedIndex = -1;
				}
				lstSubarea.EndUpdate();
			}

			if (updateRows) {
				int rowsCount = (this.lstSubarea.SelectedIndex < this.lstSubarea.Items.Count - dec && this.lstSubarea.SelectedIndex >= 0 ? (circuits[this.lstCircuits.SelectedIndex] as ModulDeckeCircuit).SubAreas[this.lstSubarea.SelectedIndex].Rows.Count : 0);
				int tmp = (lstRows.SelectedIndex == lstRows.Items.Count - dec ? rowsCount : lstRows.SelectedIndex);
				lstRows.BeginUpdate();
				lstRows.Items.Clear();
				for (int i = 1; i <= rowsCount; i++) {
					lstRows.Items.Add(EuroplanRes.PlannedModulKlimaDeckeProductPanel_Reihe + i.ToString());
				}
				if (this.newVisible) {
					lstRows.Items.Add("neue Reihen");
				}
				if (tmp < lstRows.Items.Count) {
					if (this.newVisible && tmp < 0 && lstSubarea.SelectedIndex < lstSubarea.Items.Count - dec && lstSubarea.SelectedIndex >= 0) {
						tmp = lstRows.Items.Count - 1;
					}
					lstRows.SelectedIndex = tmp;
				} else {
					lstRows.SelectedIndex = -1;
				}
				lstRows.EndUpdate();
			}

			ignoreListChange--;
			this.lstCircuits_SelectedIndexChanged(this.lstRows, EventArgs.Empty);
		}

		private int ignoreListChange = 0;
		private void lstCircuits_SelectedIndexChanged(object sender, EventArgs e) {
			if (this.ignoreListChange == 0) {
				this.ignoreListChange++;
				int dec = this.newVisible ? 1 : 0;
				if (this.lstCircuits.SelectedIndex >= 0 && this.lstCircuits.SelectedIndex < this.lstCircuits.Items.Count - dec) {
					if (this.lstSubarea.SelectedIndex < 0 || sender == this.lstCircuits) {
						if (this.newVisible) {
							this.lstSubarea.SelectedIndex = this.lstSubarea.Items.Count - 1;
						} else {
							this.lstSubarea.SelectedIndex = -1;
						}
					}
				} else {
					this.lstSubarea.SelectedIndex = -1;
				}

				if (this.lstSubarea.SelectedIndex >= 0 && this.lstSubarea.SelectedIndex < this.lstSubarea.Items.Count - dec) {
					if (this.lstRows.SelectedIndex < 0 || sender == this.lstSubarea) {
						if (this.newVisible) {
							this.lstRows.SelectedIndex = this.lstRows.Items.Count - 1;
						} else {
							this.lstRows.SelectedIndex = -1;
						}
					}
				} else {
					this.lstRows.SelectedIndex = -1;
				}

				if (sender != lstRows) {
					this.UpdateLists(false, sender == lstCircuits, true, false);
				}

				if (this.lstRows.SelectedIndex >= 0 && this.lstRows.SelectedIndex < this.lstRows.Items.Count - dec) {
					this.modulKlimaDeckePlanner.HighlightRow = (this.lstRows.Items.Count - dec > this.lstRows.SelectedIndex ? (this.modulKlimaDeckePlanner.Product.PlannedCircuits[this.lstCircuits.SelectedIndex] as ModulDeckeCircuit).SubAreas[this.lstSubarea.SelectedIndex].Rows[this.lstRows.SelectedIndex] : null);
					/*List<KlimaFlaechenModul> modules = new List<KlimaFlaechenModul>();
					if (this.modulKlimaBodenPlanner.HighlightRow != null) {
						foreach (KlimaFlaechenModul modul in this.modulKlimaBodenPlanner.HighlightRow.List) {
							modules.Add(modul);
						}
					}*/
					this.UpdateSelectedModules();
				} else if (this.lstSubarea.SelectedIndex >= 0 && this.lstSubarea.SelectedIndex < this.lstSubarea.Items.Count - dec) {
					this.modulKlimaDeckePlanner.HighlightSubArea = (this.lstSubarea.Items.Count - dec > this.lstSubarea.SelectedIndex ? (this.modulKlimaDeckePlanner.Product.PlannedCircuits[this.lstCircuits.SelectedIndex] as ModulDeckeCircuit).SubAreas[this.lstSubarea.SelectedIndex] : null);
					/*List<KlimaFlaechenModul> modules = new List<KlimaFlaechenModul>();
					if (this.modulKlimaBodenPlanner.HighlightSubArea != null) {
						foreach (KlimaFlaechenList row in this.modulKlimaBodenPlanner.HighlightSubArea.Rows) {
							foreach (KlimaFlaechenModul modul in row.List) {
								modules.Add(modul);
							}
						}
					}*/
					this.UpdateSelectedModules();
				} else if (this.lstCircuits.SelectedIndex >= 0 && this.lstCircuits.SelectedIndex < this.lstCircuits.Items.Count - dec) {
					this.modulKlimaDeckePlanner.HighlightCircuit = (this.lstCircuits.Items.Count - dec > this.lstCircuits.SelectedIndex ? (this.modulKlimaDeckePlanner.Product.PlannedCircuits[this.lstCircuits.SelectedIndex] as ModulDeckeCircuit) : null);
					/*List<KlimaFlaechenModul> modules = new List<KlimaFlaechenModul>();
					if (this.modulKlimaBodenPlanner.HighlightCircuit != null) {
						foreach (ModulDeckeSubArea subArea in this.modulKlimaBodenPlanner.HighlightCircuit.SubAreas) {
							foreach (KlimaFlaechenList row in subArea.Rows) {
								foreach (KlimaFlaechenModul modul in row.List) {
									modules.Add(modul);
								}
							}
						}
					}*/
					this.UpdateSelectedModules();
				} else {
					this.modulKlimaDeckePlanner.HighlightCircuit = null;
					this.UpdateSelectedModules();
				}

				numLength.Enabled = GetSelectedRow() != null && GetSelectedRow().List.Count > 0;

				if (GetSelectedRow() != null) {
					this.numLength.Value = (decimal)GetSelectedRow().LengthVerbindeleitungen;
				}

				if (this.lstCircuits.SelectedIndex >= 0 && this.lstCircuits.SelectedIndex < this.lstCircuits.Items.Count - dec) {
					ModulDeckeCircuit circuit = (this.lstCircuits.Items.Count - dec > this.lstCircuits.SelectedIndex ? (this.modulKlimaDeckePlanner.Product.PlannedCircuits[this.lstCircuits.SelectedIndex] as ModulDeckeCircuit) : null);
					if (circuit != null) {
						btnColor.BackColor = circuit.CircuitColor;
						btnColor.Enabled = true;
					} else {
						btnColor.BackColor = Color.Transparent;
						btnColor.Enabled = false;
					}
				} else {
					btnColor.BackColor = Color.Transparent;
					btnColor.Enabled = false;
				}

				this.ignoreListChange--;
			}
		}

		private void cmbModulType_SelectedIndexChanged(object sender, EventArgs e) {
			if (this.cmbModulType.SelectedItem is KlimaFlaechenModul.ModulTypeEnum) {
				this.modulKlimaDeckePlanner.ModuleTypeToAdd = (KlimaFlaechenModul.ModulTypeEnum)this.cmbModulType.SelectedItem;
			}
			if (this.cmbOrientation.SelectedItem is KlimaFlaechenModul.ModulOrientationEnum) {
				this.modulKlimaDeckePlanner.StartingOrientation = (KlimaFlaechenModul.ModulOrientationEnum)this.cmbOrientation.SelectedItem;
			}
		}

		private void modulKlimaBodenPlanner_ModuleSelected(object sender, ModulKlimaDeckePlanner.ModuleSelectedEventArgs e) {
			this.UpdateSelectedModules();
		}

		private void UpdateSelectedModules() {
			this.ignoreModuleOrientationChange++;
			this.ignoreModuleTypeChange++;

			if ((this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.modulKlimaDeckePlanner.Mode == ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_PICK_MODULE) {
				if (this.modulKlimaDeckePlanner.HighlightCircuit == null && this.modulKlimaDeckePlanner.HighlightSubArea == null && this.modulKlimaDeckePlanner.HighlightRow == null) {
					this.lstCircuits.SelectedIndex = -1;
				}
			}
			List<KlimaFlaechenModul> module = this.modulKlimaDeckePlanner.GetAllSelectedModules();

			this.btnInvertDirection.Enabled = module != null && module.Count > 0;

			if (module != null && module.Count > 0) {
				Nullable<KlimaFlaechenModul.ModulTypeEnum> typ = null;
				bool typOk = true;
				Nullable<KlimaFlaechenModul.ModulOrientationEnum> orientation = null;
				bool orientationOk = true;
				foreach (KlimaFlaechenModul modul in module) {
					if (!typ.HasValue) {
						typ = modul.ModulType;
					}
					if (!orientation.HasValue) {
						orientation = modul.Orientation;
					}
					if (typ != modul.ModulType) {
						typOk = false;
					}
					if (orientation != modul.Orientation) {
						orientationOk = false;
					}
					if (!typOk && !orientationOk) {
						break;
					}
				}
				if (typOk && typ.HasValue) {
					this.cmbSelectedModuleType.SelectedItem = typ.Value;
				} else {
					this.cmbSelectedModuleType.SelectedIndex = -1;
				}
				if (orientationOk && orientation.HasValue) {
					this.cmbSelectedModuleOrientation.SelectedItem = orientation.Value;
				} else {
					this.cmbSelectedModuleOrientation.SelectedIndex = -1;
				}

				if (typ.HasValue) {
					if (typOk) {
						this.lblTypError.Text = "";
					} else {
						this.lblTypError.Text = "Verschiedene Modulgrößen ausgewählt";
					}
					if (orientationOk) {
						this.lblOrientationError.Text = "";
						if (this.cmbSelectedModuleOrientation.Items.Count == 3) {
							this.cmbSelectedModuleOrientation.Items.RemoveAt(2);
						}
					} else {
						this.lblOrientationError.Text = "Verschiedene Ausrichtungen ausgewählt";
						if (this.cmbSelectedModuleOrientation.Items.Count == 2) {
							this.cmbSelectedModuleOrientation.Items.Add("Alle umdrehen");
						}
					}
					this.cmbSelectedModuleType.Enabled = true;
					this.cmbSelectedModuleOrientation.Enabled = true;
				} else {
					this.lblTypError.Text = "Kein Modul ausgewählt";
					this.cmbSelectedModuleType.Enabled = false;
					this.cmbSelectedModuleOrientation.Enabled = false;
				}
			} else {
				this.lblTypError.Text = "Kein Modul ausgewählt";
				this.cmbSelectedModuleType.SelectedIndex = -1;
				this.cmbSelectedModuleOrientation.SelectedIndex = -1;
				this.cmbSelectedModuleType.Enabled = false;
				this.cmbSelectedModuleOrientation.Enabled = false;
			}
			this.ignoreModuleOrientationChange--;
			this.ignoreModuleTypeChange--;

			//List<KlimaFlaechenModul> module = this.modulKlimaBodenPlanner.GetAllSelectedModules();
			if (module.Count == 0) {
				this.llHk.Enabled = false;
				this.llHk.Tag = null;
				this.llHk.Text = "keiner";
				this.llSubarea.Enabled = false;
				this.llSubarea.Text = "keine";
				this.llSubarea.Tag = null;
				this.llReihe.Enabled = false;
				this.llReihe.Text = "keine";
				this.llReihe.Tag = null;
			} else {
				ModulDeckeCircuit circuit = null;
				bool circuitOk = true;
				ModulDeckeSubArea subArea = null;
				bool subAreaOk = true;
				KlimaFlaechenList row = null;
				bool rowOk = true;
				int circuitIndex = 0;
				int subAreaIndex = 0;
				int rowIndex = 0;
				foreach (KlimaFlaechenModul modul in module) {
					ModulDeckeCircuit curCircuit = this.modulKlimaDeckePlanner.Product.GetCircuitForModul(modul, out circuitIndex);
					if (circuit != null && circuit != curCircuit) {
						circuitOk = false;
						subAreaOk = false;
						rowOk = false;
					}
					if (curCircuit == null) {
						circuitOk = false;
						subAreaOk = false;
						rowOk = false;
					} else {
						ModulDeckeSubArea curSubArea = curCircuit.GetSubareaForModul(modul, out subAreaIndex);
						if (subArea != null && subArea != curSubArea) {
							subAreaOk = false;
							rowOk = false;
						}
						if (curSubArea == null) {
							subAreaOk = false;
							rowOk = false;
						} else {
							KlimaFlaechenList curRow = curSubArea.GetRowForModul(modul, out rowIndex);
							if (row != null && row != curRow) {
								rowOk = false;
							}
							if (curRow == null) {
								rowOk = false;
							} else {
								row = curRow;
							}
							subArea = curSubArea;
						}
						circuit = curCircuit;
					}
				}
				if (circuitOk) {
					this.llHk.Tag = circuitIndex;
					circuitIndex++;
					this.llHk.Text = "HK" + circuitIndex.ToString();
					this.llHk.Enabled = true;
				} else {
					this.llHk.Tag = null;
					this.llHk.Enabled = false;
					this.llHk.Text = "verschiende";
				}
				if (subAreaOk) {
					this.llSubarea.Tag = subAreaIndex;
					subAreaIndex++;
					this.llSubarea.Text = "Teilfäche " + subAreaIndex.ToString();
					this.llSubarea.Enabled = true;
				} else {
					this.llSubarea.Tag = null;
					this.llSubarea.Enabled = false;
					this.llSubarea.Text = "verschiende";
				}
				if (rowOk) {
					this.llReihe.Tag = rowIndex;
					rowIndex++;
					this.llReihe.Text = "Reihe " + rowIndex.ToString();
					this.llReihe.Enabled = true;
				} else {
					this.llReihe.Tag = null;
					this.llReihe.Enabled = false;
					this.llReihe.Text = "verschiende";
				}
			}
		}

		private int ignoreModuleTypeChange = 0;
		private int ignoreModuleOrientationChange = 0;

		private void cmbSelectedModuleType_SelectedIndexChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (ignoreModuleTypeChange == 0) {
					this.changed = true;
					if (this.modulKlimaDeckePlanner.Product == null || this.modulKlimaDeckePlanner.Product.AssociatedRoom == null ||
						this.modulKlimaDeckePlanner.Product.AssociatedRoom.AssociatedPlan == null || this.modulKlimaDeckePlanner.Product.AssociatedRoom.AssociatedPlan.Measure == null) {
						return;
					}
					List<KlimaFlaechenModul> modules = this.modulKlimaDeckePlanner.GetAllSelectedModules();
					bool allChanged = true;
					bool nonChanged = true;
					if (this.cmbSelectedModuleType.SelectedItem is KlimaFlaechenModul.ModulTypeEnum) {
						foreach (KlimaFlaechenModul modul in modules) {
							PossibleModulLane lane = this.modulKlimaDeckePlanner.Product.GraphConstruction.PossibleLanes[modul.GraphLane];
							if (lane.ModuleChangesPossible(modul, (KlimaFlaechenModul.ModulTypeEnum)this.cmbSelectedModuleType.SelectedItem, modul.GraphPositionInLan, this.modulKlimaDeckePlanner.Product.AssociatedRoom.AssociatedPlan.Measure.Value, this.modulKlimaDeckePlanner.Product)) {
								modul.ModulType = (KlimaFlaechenModul.ModulTypeEnum)this.cmbSelectedModuleType.SelectedItem;
								nonChanged = false;
							} else {
								allChanged = false;
							}
						}
					}
					this.UpdateSelectedModules();
					this.planPanel.InvalidateGraphics();
					if (nonChanged) {
						MessageBox.Show("Es konnte kein Modul geändert werden, da nicht genug Platz zur Verfügugn steht", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
					} else if (!allChanged) {
						MessageBox.Show("Es konnten nicht alle Module geändert werden, da nicht genug Platz zur Verfügugn steht", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					this.CalculateAndUpdate();
				}
			}
		}

		private void cmbSelectedModuleOrientation_SelectedIndexChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (ignoreModuleOrientationChange == 0) {
					this.changed = true;
					List<KlimaFlaechenModul> modules = this.modulKlimaDeckePlanner.GetAllSelectedModules();
					if (cmbSelectedModuleOrientation.SelectedIndex == 2) {
						foreach (KlimaFlaechenModul modul in modules) {
							modul.Orientation = (modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT ? KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT : KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT);
						}
					} else if (this.cmbSelectedModuleOrientation.SelectedItem is KlimaFlaechenModul.ModulOrientationEnum) {
						foreach (KlimaFlaechenModul modul in modules) {
							modul.Orientation = (KlimaFlaechenModul.ModulOrientationEnum)this.cmbSelectedModuleOrientation.SelectedItem;
						}
					}
					this.UpdateSelectedModules();
					this.planPanel.InvalidateGraphics();
				}
			}
		}

		private void ModulKlimaDeckePlannerForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ModulKlimaDeckePlannerForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
		}

		private void ModulKlimaDeckePlannerForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ModulKlimaDeckePlannerForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			SettingsFile.Update();
		}

		private void rbSerie_CheckedChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (sender is RadioButton && (sender as RadioButton).Checked) {

					this.changed = true;
					this.cmbModulType.Items.Clear();
					this.cmbSelectedModuleType.Items.Clear();

					if (sender == this.rbSerie30) {
						this.glatt.SchienenAbstand = 0.3;
						this.akustik.SchienenAbstand = 0.3;

						this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_80_30);
						this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_30);
						this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_120_30);
						this.cmbModulType.SelectedIndex = 2;
						this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_80_30);
						this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_30);
						this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_120_30);
						this.cmbSelectedModuleType.SelectedIndex = -1;

					} else if (sender == this.rbSerie40) {
						this.glatt.SchienenAbstand = 0.4;
						this.akustik.SchienenAbstand = 0.4;

						this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40);
						this.cmbModulType.SelectedIndex = 0;
						this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40);
						this.cmbSelectedModuleType.SelectedIndex = -1;
					}
				}
				this.planPanel.InvalidateGraphics();
			}
		}

		private void rbRasterMass_CheckedChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (sender is RadioButton && (sender as RadioButton).Checked) {
					this.changed = true;
					this.cmbModulType.Items.Clear();
					this.cmbSelectedModuleType.Items.Clear();

					if (sender == this.rb1050) {
						this.kassette.Raster = ModulKlimaDeckeConstructionKassette.RasterMass.Raster_1050_450;
						this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40);
						this.cmbModulType.SelectedIndex = 0;
						this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40);
						this.cmbSelectedModuleType.SelectedIndex = -1;
					} else if (sender == this.rb625) {
						this.kassette.Raster = ModulKlimaDeckeConstructionKassette.RasterMass.Raster_625;
						this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60);
						this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B);
						this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60C);
						this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60D);
						this.cmbModulType.SelectedIndex = 0;
						this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60);
						this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B);
						this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60C);
						this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60D);
						this.cmbSelectedModuleType.SelectedIndex = -1;
					} else if (sender == this.rb600) {
						this.kassette.Raster = ModulKlimaDeckeConstructionKassette.RasterMass.Raster_600;
						this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60);
						this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B);
						this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60C);
						this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60D);
						this.cmbModulType.SelectedIndex = 0;
						this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60);
						this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B);
						this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60C);
						this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60D);
						this.cmbSelectedModuleType.SelectedIndex = -1;
					}
				}
				this.planPanel.InvalidateGraphics();
			}
		}

		private void cbAutomaticOrientation_CheckedChanged(object sender, EventArgs e) {
			this.modulKlimaDeckePlanner.AutomaticOrientation = this.cbAutomaticOrientation.Checked;
		}

		private void cbAutomaticRows_CheckedChanged(object sender, EventArgs e) {
			this.modulKlimaDeckePlanner.AutomaticRows = this.cbAutomaticRows.Checked;
		}

		private void llHk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			object hkTag = llHk.Tag;

			if (hkTag != null && this.lstCircuits.Items.Count > (int)hkTag) {
				this.lstCircuits.SelectedIndex = -1;
				this.lstCircuits.SelectedIndex = (int)hkTag;
			}
		}

		private void llSubarea_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			object hkTag = llHk.Tag;
			object saTag = llSubarea.Tag;

			if (hkTag != null && this.lstCircuits.Items.Count > (int)hkTag) {
				this.lstCircuits.SelectedIndex = -1;
				this.lstCircuits.SelectedIndex = (int)hkTag;
				if (saTag != null && this.lstSubarea.Items.Count > (int)saTag) {
					this.lstSubarea.SelectedIndex = (int)saTag;
				}
			}
		}

		private void llReihe_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			object hkTag = llHk.Tag;
			object saTag = llSubarea.Tag;
			object rTag = llReihe.Tag;

			if (hkTag != null && this.lstCircuits.Items.Count > (int)hkTag) {
				this.lstCircuits.SelectedIndex = -1;
				this.lstCircuits.SelectedIndex = (int)hkTag;
				if (saTag != null && this.lstSubarea.Items.Count > (int)saTag) {
					this.lstSubarea.SelectedIndex = (int)saTag;
					if (rTag != null && this.lstRows.Items.Count > (int)rTag) {
						this.lstRows.SelectedIndex = (int)rTag;
					}
				}
			}
		}

		private void CalculateAndUpdate() {
			this.plannedProduct.Product.ConfigureProduct(this.plannedProduct.RequestedHeatLoad, this.plannedProduct.RequestedCoolLoad, this.plannedProduct.CalculateHeat, this.plannedProduct.CalculateCool, false);
			string errorMsg = this.plannedProduct.Product.LastErrorMessage;
			this.lstError.Items.Clear();
			string[] messages;
			if (errorMsg != null) {
				messages = errorMsg.Split('\n');
				foreach (string message in messages) {
					if (!string.IsNullOrEmpty(message)) {
						ListViewItem item = new ListViewItem(message);
						item.ForeColor = Color.Red;
						//item.Font = new Font(item.Font, FontStyle.Bold);
						this.lstError.Items.Add(item);
					}
				}
			}
			string notifications = this.plannedProduct.Product.NotificationMessage;
			if (notifications != null) {
				messages = notifications.Split('\n');
				foreach (string message in messages) {
					if (!string.IsNullOrEmpty(message)) {
						ListViewItem item = new ListViewItem(message);
						item.ForeColor = Color.Orange;
						this.lstError.Items.Add(item);
					}
				}
			}
			notifications = ModulKlimaDeckeProduct.GlobalNotificationMessage;
			if (notifications != null) {
				messages = notifications.Split('\n');
				foreach (string message in messages) {
					if (!string.IsNullOrEmpty(message)) {
						ListViewItem item = new ListViewItem(message);
						item.ForeColor = Color.Orange;
						this.lstError.Items.Add(item);
					}
				}
			}
			if (lstError.Items.Count > 0) {
				this.lstError.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
				int height = this.lstError.Items[this.lstError.Items.Count - 1].Position.Y + this.lstError.Items[this.lstError.Items.Count - 1].Bounds.Height + 5;
				this.lstError.Height = height;
				this.lstError.Visible = true;
			} else {
				this.lstError.Visible = false;
			}

			bool showHeat = this.plannedProduct.RequestedHeatLoad > 0;
			bool showCool = this.plannedProduct.RequestedCoolLoad > 0;

			lblQHeat.Visible = showHeat;
			lblQHeatUnit.Visible = showHeat;
			lblQAnbHeat.Visible = showHeat;
			lblQAnbHeatUnit.Visible = showHeat;
			lblQHeatDiff.Visible = showHeat;
			lblQHeatDiffUnit.Visible = showHeat;
			lblQHeatRest.Visible = showHeat;
			lblQHeatRestUnit.Visible = showHeat;

			lblQCool.Visible = showCool;
			lblQCoolUnit.Visible = showCool;
			lblQAnbCool.Visible = showCool;
			lblQAnbCoolUnit.Visible = showCool;
			lblQCoolDiff.Visible = showCool;
			lblQCoolDiffUnit.Visible = showCool;
			lblQCoolRest.Visible = showCool;
			lblQCoolRestUnit.Visible = showCool;

			// General
			double qDiffHeat = this.plannedProduct.PlannedHeatLoad - this.plannedProduct.RequestedHeatLoad;
			double qDiffCool = this.plannedProduct.PlannedCoolLoad - this.plannedProduct.RequestedCoolLoad;

			lblQHeat.Text = Math.Round(this.plannedProduct.Product.PlannedHeatLoad, 2).ToString();
			lblQAnbHeat.Text = Math.Round(this.plannedProduct.Product.PlannedHeatLoadAnbindung, 0).ToString();
			lblQHeatDiff.Text = Math.Round(qDiffHeat, 0).ToString("+0;-0");
			lblQHeatRest.Text = Math.Round(this.plannedProduct.Product.AssociatedRoom.OpenHeatLoad, 2).ToString("+0.00;-0.00");
			lblQCool.Text = Math.Round(this.plannedProduct.Product.PlannedCoolLoad, 2).ToString();
			lblQAnbCool.Text = Math.Round(this.plannedProduct.Product.PlannedCoolLoadAnbindung, 0).ToString();
			lblQCoolDiff.Text = Math.Round(qDiffCool, 0).ToString("+0;-0");
			lblQCoolRest.Text = Math.Round(this.plannedProduct.Product.AssociatedRoom.OpenCoolLoad, 2).ToString("+0.00;-0.00");

		}

		private void button1_Click(object sender, EventArgs e) {
			CalculateAndUpdate();
		}

		private void numLength_ValueChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (ignoreListChange == 0 && GetSelectedRow() != null) {
					this.changed = true;
					GetSelectedRow().LengthVerbindeleitungen = (double)this.numLength.Value;
					CalculateAndUpdate();
				}
			}
		}

		private void modulKlimaBodenPlanner_ProjectChanged(object sender) {
			this.CalculateAndUpdate();
			this.changed = true;
		}

		public bool Changed {
			get { return this.changed; }
		}

		private int ignoreRandfries = 0;
		private void numRandfries_ValueChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (ignoreRandfries == 0) {
					this.changed = true;
					this.akustik.Randfries = ((double)numRandfries.Value) / 100.0;
					this.planPanel.InvalidateGraphics();
				}
			}
		}

		private int ignoreBeplankung = 0;

		private void cbBeplankung_CheckedChanged(object sender, EventArgs e) {
			if (ignoreBeplankung == 0) {
				// Akustik is subclass of Glatt!!!
				ModulKlimaDeckeConstructionGlatt constr = this.modulKlimaDeckePlanner.Product.GraphConstruction as ModulKlimaDeckeConstructionGlatt;
				if (constr != null) {
					ignoreBeplankungValue++;
					if (this.cbBeplankung.Checked) {
						this.numBeplankungBreite.Value = (decimal)(0.625 * Product.ConfigPlanMeasureMultiplier);
						this.numBeplankungLaenge.Value = (decimal)(2.500 * Product.ConfigPlanMeasureMultiplier);
						this.numBeplankungBreite.Enabled = true;
						this.numBeplankungLaenge.Enabled = true;
						this.numBeplankungBreite.ReadOnly = false;
						this.numBeplankungLaenge.ReadOnly = false;
						constr.Beplankung = new WW.Math.Size2D((double)this.numBeplankungLaenge.Value / Product.ConfigPlanMeasureMultiplier, (double)this.numBeplankungBreite.Value / Product.ConfigPlanMeasureMultiplier);
					} else {
						this.numBeplankungBreite.Enabled = false;
						this.numBeplankungLaenge.Enabled = false;
						this.numBeplankungLaenge.ReadOnly = true;
						this.numBeplankungBreite.ReadOnly = true;
						this.numBeplankungLaenge.Text = "";
						this.numBeplankungBreite.Text = "";
						constr.Beplankung = null;
					}
					this.planPanel.InvalidateGraphics();
					ignoreBeplankungValue--;
				}
			}
		}

		private int ignoreBeplankungValue = 0;
		private void numBeplankung_ValueChanged(object sender, EventArgs e) {
			if (this.ignoreBeplankungValue == 0) {
				// Akustik is subclass of Glatt!!!
				ModulKlimaDeckeConstructionGlatt constr = this.modulKlimaDeckePlanner.Product.GraphConstruction as ModulKlimaDeckeConstructionGlatt;
				if (constr != null && this.cbBeplankung.Checked) {
					constr.Beplankung = new WW.Math.Size2D((double)this.numBeplankungLaenge.Value / Product.ConfigPlanMeasureMultiplier, (double)this.numBeplankungBreite.Value / Product.ConfigPlanMeasureMultiplier);
					this.planPanel.InvalidateGraphics();
				}				
			}
		}

		private void btnColor_Click(object sender, EventArgs e) {
			int dec = this.newVisible ? 1 : 0;
			if (this.lstCircuits.SelectedIndex >= 0 && this.lstCircuits.SelectedIndex < this.lstCircuits.Items.Count - dec) {
				ModulDeckeCircuit circuit = (this.lstCircuits.Items.Count - dec > this.lstCircuits.SelectedIndex ? (this.modulKlimaDeckePlanner.Product.PlannedCircuits[this.lstCircuits.SelectedIndex] as ModulDeckeCircuit) : null);
				if (circuit != null) {
					colorDialog.Color = circuit.CircuitColor;
					if (colorDialog.ShowDialog() == DialogResult.OK) {
						circuit.CircuitColor = colorDialog.Color;
						this.btnColor.BackColor = circuit.CircuitColor;
						this.planPanel.InvalidateGraphics();
					}
				}
			}
		}

		private void btnShowBeplankung_Click(object sender, EventArgs e) {
			this.modulKlimaDeckePlanner.DrawBeplankung = btnShowBeplankung.Checked;
			this.planPanel.InvalidateGraphics();
		}

		private void btnInvertDirection_Click(object sender, EventArgs e) {
			this.changed = true;
			List<KlimaFlaechenModul> modules = this.modulKlimaDeckePlanner.GetAllSelectedModules();
			foreach (KlimaFlaechenModul modul in modules) {
				modul.GraphBottomUp = !modul.GraphBottomUp;
			}
			this.UpdateSelectedModules();
			this.planPanel.InvalidateGraphics();
		}
		private void lst_KeyDown(object sender, KeyEventArgs e) {
			this.modulKlimaDeckePlanner.PlannerKeyPress(e.KeyCode);
		}

		private void modulKlimaDeckePlanner_UpdateNewCount(object sender, ModulKlimaDeckePlanner.UpdateNewCountArgs e) {
			if (e.count == 0) {
				this.lblNewModules.Visible = false;
			} else {
				this.lblNewModules.Text = e.count.ToString() + " neue Module";
				this.lblNewModules.Visible = true;
			}
		}

		private void SetProductPlanner() {
			if (this.planPanel.ProductPlanner != this.modulKlimaDeckePlanner) {
				double scale = this.planPanel.PlanScale;
				Vector2D translation = this.planPanel.PlanTranslation;
				this.planPanel.ProductPlanner = this.modulKlimaDeckePlanner;
				this.planPanel.PlanScale = scale;
				this.planPanel.PlanTranslation = translation;
			}
		}

		private void SetConnectionPlanner() {
			if (this.planPanel.ProductPlanner != this.connectionPlanner) {
				double scale = this.planPanel.PlanScale;
				Vector2D translation = this.planPanel.PlanTranslation;
				this.planPanel.ProductPlanner = this.connectionPlanner;
				this.planPanel.PlanScale = scale;
				this.planPanel.PlanTranslation = translation;
			}
		}
	}
}