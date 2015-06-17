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
	public partial class ModulKlimaBoden20PlannerForm : Form {

		private ModulKlimaBoden20ConstructionFrei frei = null;
		private ModulKlimaBoden20ConstructionStaffeln staffeln = null;

		private int ignoreRotation = 0;
		private int ignoreStaffelBreite = 0;
		private int ignoreAchsAbstand = 0;
		private int ignoreConstruction = 0;
		private bool newVisible = false;
		private PlannedProduct plannedProduct;

		private bool changed = false;

		public ModulKlimaBoden20PlannerForm(PlannedProduct plannedProduct) {
			InitializeComponent();
			this.SetLanguage();
			this.plannedProduct = plannedProduct;
            this.btnShowPlanBg.Checked = Product.ShowPlanInBackground;

			this.cmbOrientation.Items.Add(KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT);
			this.cmbOrientation.Items.Add(KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT);
			this.cmbOrientation.SelectedIndex = 0;

			this.cmbSelectedModuleOrientation.Items.Add(KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT);
			this.cmbSelectedModuleOrientation.Items.Add(KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT);
			this.cmbSelectedModuleOrientation.SelectedIndex = -1;
			this.cmbSelectedModuleOrientation.Enabled = false;

			this.cmbHorizontal.Items.Add(Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_Dicht);
			this.cmbHorizontal.Items.Add(Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_Modulierend);
			this.cmbHorizontal.Items.Add(Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_ModulierendDoppelt);
			this.cmbHorizontal.SelectedIndex = 0;

			this.cmbVertical.Items.Add(Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_Dicht);
			this.cmbVertical.Items.Add(Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_Modulierend);
			this.cmbVertical.Items.Add(Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_ModulierendDoppelt);
			this.cmbVertical.SelectedIndex = 1;

			ModulKlimaBoden20Product product = plannedProduct.Product as ModulKlimaBoden20Product;

			this.modulKlimaBodenPlanner.Product = product;
			if (product.GraphConstruction == null) {
				product.GraphConstruction = new ModulKlimaBoden20ConstructionFrei();
			}
			this.frei = product.GraphConstruction as ModulKlimaBoden20ConstructionFrei;
			this.staffeln = product.GraphConstruction as ModulKlimaBoden20ConstructionStaffeln;

			if (this.frei == null) {
				this.frei = new ModulKlimaBoden20ConstructionFrei();
				this.frei.Planner = this.modulKlimaBodenPlanner;
				this.frei.RotationRelativeToPlan = 0;
			} else {
				this.frei.Planner = this.modulKlimaBodenPlanner;
			}
			this.frei.RecalculateStaffeln();

			if (this.staffeln == null) {
				this.staffeln = new ModulKlimaBoden20ConstructionStaffeln();
				this.staffeln.Planner = this.modulKlimaBodenPlanner;
				this.staffeln.RotationRelativeToPlan = 0;
			} else {
				this.staffeln.Planner = this.modulKlimaBodenPlanner;
			}
			this.staffeln.RecalculateStaffeln();

			this.UpdateLists(true, true, true, false);

			if (product.ContainsModules) {
				this.tabs.SelectedTab = this.pageLayout;
			}
			this.UpdateToolbar(this.tabs.SelectedTab);
			this.CalculateAndUpdate();
			this.connectionPlanner.Product = product;
		}

		private void SetLanguage() {
			this.btnZoomIn.Text = Europlan.Common.EuroplanRes.Plan_Heranzoomen;
			this.btnZoomIn.ToolTipText = Europlan.Common.EuroplanRes.Plan_Heranzoomen;
			this.btnZoomOut.Text = Europlan.Common.EuroplanRes.Plan_Herauszoomen;
			this.btnZoomOut.ToolTipText = Europlan.Common.EuroplanRes.Plan_Herauszoomen;
			this.btnMove.Text = Europlan.Common.EuroplanRes.Plan_Verschieben;
			this.btnMove.ToolTipText = Europlan.Common.EuroplanRes.Plan_Verschieben;
			this.btnConstruction.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_UnterkonstruktionAusrichten;
			this.btnConstruction.ToolTipText = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_UnterkonstruktionAusrichten;
			this.btnAddModules.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_ModuleHinzufuegen;
			this.btnAddModules.ToolTipText = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_ModuleHinzufuegen;
			this.btnSelectModule.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_ModuleAuswaehlen;
			this.btnSelectModule.ToolTipText = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_ModuleAuswaehlen;
			this.btnConnections.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_VerbindeleitungHinzufuegen;
			this.btnDeleteConnection.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_VerbindeleitungLoeschen;
			this.btnAddAnbindeleitungen.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_AnbindeleitungenHinzufuegen;
			this.btnSelectAnbindeleitungen.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_AnbindeleitungenAendern;
			this.pageConstruction.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_KonstruktionEinrichten;
			this.grpConstructionType.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_Konstruktionsart;
			this.rbHolzstaffeln.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_Holzstaffeln;
			this.rbFrei.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_Frei;
			this.grpConstructionParameter.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_Konstruktionsparameter;
			this.label5.Text = Europlan.Common.EuroplanRes.Unit_Millimeter;
			this.lblAchsabstand.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_Achsabstand;
			this.label1.Text = Europlan.Common.EuroplanRes.Unit_Millimeter;
			this.lblStaffelBreite.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_Staffelbreite;
			this.lblRotation.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_Ausrichtung;
			this.pageLayout.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_ModuleAuslegen;
			this.lblColor.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_FarbeHK;
			this.label14.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_ProWinkel1mVerbindungsleitung;
			this.label8.Text = Europlan.Common.EuroplanRes.Unit_Meter;
			this.label7.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_VerbindungsleitungenInHK;
			this.label12.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_Heizkreise;
			this.grpNewModules.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_NeueModule;
			this.lblNewModules.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_NeueModule2;
			this.chkSelectReferenceModule.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_ReferenzmodulFuerAusrichtung;
			this.btnAdd.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_Hinzufuegen;
			this.lblVertical.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_Breitseite2;
			this.lblHorizontal.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_Laengsseite2;
			this.lblNewRotation.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_Drehung2;
			this.label3.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_Ausrichtung;
			this.grpSelection.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_AusgewaehltesModul;
			this.lblHk.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_Heizkreis2;
			this.grpSelectedModules.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_AusgewaehlteModule;
			this.btnInvertDirection.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_DurchstroemungsrichtungUmdrehen;
			this.label4.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_Ausrichtung;
			this.pageCalculations.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_Berechnungsergebnisse;
			this.lblQAnbCoolUnit.Text = Europlan.Common.EuroplanRes.Unit_Watt;
			this.label45.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_LeistungAnbindeleitungen;
			this.lblQCoolRestUnit.Text = Europlan.Common.EuroplanRes.Unit_Watt;
			this.lblQCoolDiffUnit.Text = Europlan.Common.EuroplanRes.Unit_Watt;
			this.lblQCoolUnit.Text = Europlan.Common.EuroplanRes.Unit_Watt;
			this.lblQHeatRestUnit.Text = Europlan.Common.EuroplanRes.Unit_Watt;
			this.lblQHeatDiffUnit.Text = Europlan.Common.EuroplanRes.Unit_Watt;
			this.lblQHeatUnit.Text = Europlan.Common.EuroplanRes.Unit_Watt;
			this.label16.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_DifferenzZurErwartetenLeistung;
			this.label17.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_ErreichteLeistung;
			this.label11.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_Kuehlbetrieb;
			this.label10.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_Heizbetrieb;
			this.label6.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_Berechnungsergebnisse2;
			this.label9.Text = Europlan.Common.EuroplanRes.Unit_Watt;
			this.label18.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_DifferenzZurErwartetenLeistung;
            this.label13.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_Teilflaechen;
            this.label15.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_ParalleleReihen;
			this.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_Titel;
            this.btnShowPlanBg.Text = Europlan.Common.EuroplanRes.ProductPlannerForm_PlanImHintergrundAnzeigen;
		}

		private void UpdateControls() {
			this.grpConstructionParameter.Visible = this.modulKlimaBodenPlanner.Product.GraphConstruction is ModulKlimaBoden20ConstructionStaffeln;
		}

		private void btnZoomOut_Click(object sender, EventArgs e) {
			this.planPanel.AddScale(0.9, null);
		}

		private void btnZoomIn_Click(object sender, EventArgs e) {
			this.planPanel.AddScale(1.1, null);
		}

		private void btnMove_Click(object sender, EventArgs e) {
			if (!btnMove.Checked) {
				if (this.modulKlimaBodenPlanner.ContainsNotConfirmedModules) {
					DialogResult result = MessageBox.Show(Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_ModuleUebernehmenText, Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_ModuleUebernehmenTitel, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
					if (result == DialogResult.Cancel) {
						return;
					} else if (result == DialogResult.Yes) {
						ModulKlimaBoden20Circuit newCircuit = this.modulKlimaBodenPlanner.ConfirmNewModules();
						if (newCircuit != null) {
							this.UpdateLists(true, true, true, true);
						}
					}
				}
				this.modulKlimaBodenPlanner.Mode = ModulKlimaBoden20Planner.KlimaBodenMode.KDM_NONE;
				this.planPanel.Mode = PlanMode.PM_MOVE;
				this.UpdateButtons();
			}
		}

		private void btnAddModules_Click(object sender, EventArgs e) {
			if (!btnAddModules.Checked) {
				this.SetProductPlanner();
				this.modulKlimaBodenPlanner.Mode = ModulKlimaBoden20Planner.KlimaBodenMode.KDM_LAYOUT_ADD_AREA;
				this.planPanel.Mode = PlanMode.PM_PLANNER_DRAG;
				this.UpdateButtons();
			}
		}

		private void SetProductPlanner() {
			if (this.planPanel.ProductPlanner != this.modulKlimaBodenPlanner) {
				double scale = this.planPanel.PlanScale;
				Vector2D translation = this.planPanel.PlanTranslation;
				this.planPanel.ProductPlanner = this.modulKlimaBodenPlanner;
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

		private void btnSelectModule_Click(object sender, EventArgs e) {
			if (!btnSelectModule.Checked) {
				this.SetProductPlanner();
				if (this.modulKlimaBodenPlanner.ContainsNotConfirmedModules) {
					DialogResult result = MessageBox.Show(Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_ModuleUebernehmenText, Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_ModuleUebernehmenTitel, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
					if (result == DialogResult.Cancel) {
						return;
					} else if (result == DialogResult.Yes) {
						ModulKlimaBoden20Circuit newCircuit = this.modulKlimaBodenPlanner.ConfirmNewModules();
						if (newCircuit != null) {
							this.UpdateLists(true, true, true, true);
						}
					}
				}
				this.modulKlimaBodenPlanner.Mode = ModulKlimaBoden20Planner.KlimaBodenMode.KDM_PICK_MODULE;
				this.planPanel.Mode = PlanMode.PM_PLANNER_DRAG;
				this.UpdateButtons();
			}
		}

		private void UpdateButtons() {
			if (this.planPanel.Mode == PlanMode.PM_MOVE) {
				this.btnMove.Checked = true;
				this.btnAddModules.Checked = false;
				this.btnSelectModule.Checked = false;
				this.btnConstruction.Checked = false;
				this.btnConnections.Checked = false;
				this.btnDeleteConnection.Checked = false;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = false;
			} else if (this.planPanel.ProductPlanner == this.modulKlimaBodenPlanner && (this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.modulKlimaBodenPlanner.Mode == ModulKlimaBoden20Planner.KlimaBodenMode.KDM_LAYOUT_ADD_AREA) {
				this.btnMove.Checked = false;
				this.btnAddModules.Checked = true;
				this.btnSelectModule.Checked = false;
				this.btnConstruction.Checked = false;
				this.btnConnections.Checked = false;
				this.btnDeleteConnection.Checked = false;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = false;
			} else if (this.planPanel.ProductPlanner == this.modulKlimaBodenPlanner && (this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.modulKlimaBodenPlanner.Mode == ModulKlimaBoden20Planner.KlimaBodenMode.KDM_PICK_MODULE) {
				this.btnMove.Checked = false;
				this.btnAddModules.Checked = false;
				this.btnSelectModule.Checked = true;
				this.btnConstruction.Checked = false;
				this.btnConnections.Checked = false;
				this.btnDeleteConnection.Checked = false;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = false;
			} else if (this.planPanel.ProductPlanner == this.modulKlimaBodenPlanner && (this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.modulKlimaBodenPlanner.Mode == ModulKlimaBoden20Planner.KlimaBodenMode.KDM_CONSTRUCTION) {
				this.btnMove.Checked = false;
				this.btnAddModules.Checked = false;
				this.btnSelectModule.Checked = false;
				this.btnConstruction.Checked = true;
				this.btnConnections.Checked = false;
				this.btnDeleteConnection.Checked = false;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = false;
			} else if (this.planPanel.ProductPlanner == this.modulKlimaBodenPlanner && (this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.modulKlimaBodenPlanner.Mode == ModulKlimaBoden20Planner.KlimaBodenMode.KDM_ADD_CONNECTIONS) {
				this.btnMove.Checked = false;
				this.btnAddModules.Checked = false;
				this.btnSelectModule.Checked = false;
				this.btnConstruction.Checked = false;
				this.btnConnections.Checked = true;
				this.btnDeleteConnection.Checked = false;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = false;
			} else if (this.planPanel.ProductPlanner == this.modulKlimaBodenPlanner && (this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.modulKlimaBodenPlanner.Mode == ModulKlimaBoden20Planner.KlimaBodenMode.KDM_DEL_CONNECTION) {
				this.btnMove.Checked = false;
				this.btnAddModules.Checked = false;
				this.btnSelectModule.Checked = false;
				this.btnConstruction.Checked = false;
				this.btnConnections.Checked = false;
				this.btnDeleteConnection.Checked = true;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = false;
			} else if (this.planPanel.ProductPlanner == this.connectionPlanner && (this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.connectionPlanner.Mode == ConnectionPlanner.ConnectionMode.KDM_ADD_CONNECTION) {
				this.btnMove.Checked = false;
				this.btnAddModules.Checked = false;
				this.btnSelectModule.Checked = false;
				this.btnConstruction.Checked = false;
				this.btnConnections.Checked = false;
				this.btnDeleteConnection.Checked = false;
				this.btnAddAnbindeleitungen.Checked = true;
				this.btnSelectAnbindeleitungen.Checked = false;
			} else if (this.planPanel.ProductPlanner == this.connectionPlanner && (this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.connectionPlanner.Mode == ConnectionPlanner.ConnectionMode.KDM_SELECT_CONNECTION) {
				this.btnMove.Checked = false;
				this.btnAddModules.Checked = false;
				this.btnSelectModule.Checked = false;
				this.btnConstruction.Checked = false;
				this.btnConnections.Checked = false;
				this.btnDeleteConnection.Checked = false;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = true;
			} else {
				this.btnMove.Checked = false;
				this.btnAddModules.Checked = false;
				this.btnSelectModule.Checked = false;
				this.btnConstruction.Checked = false;
				this.btnConnections.Checked = false;
				this.btnDeleteConnection.Checked = false;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = false;
			}
			this.grpSelectedModules.Visible = this.btnSelectModule.Checked;
			this.grpSelection.Visible = this.btnSelectModule.Checked;
			this.grpNewModules.Visible = this.btnAddModules.Checked;
			if ((this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.modulKlimaBodenPlanner.Mode == ModulKlimaBoden20Planner.KlimaBodenMode.KDM_LAYOUT_ADD_AREA) {
				if (!this.newVisible) {
					this.newVisible = true;
					this.ignoreListChange++;
					lstCircuits.Items.Add(Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_NeuerHK);
                    lstSubarea.Items.Add(Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_NeueTeilflaeche);
                    lstRows.Items.Add(Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_NeueReihen);
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

			bool enableLayoutPage = !this.btnAddAnbindeleitungen.Checked;
			bool staffeln = (this.modulKlimaBodenPlanner.Product.GraphConstruction is ModulKlimaBoden20ConstructionStaffeln);
			if (staffeln) {
				this.numNewRotation.Value = (decimal)((this.modulKlimaBodenPlanner.Product.GraphConstruction as ModulKlimaBoden20ConstructionStaffeln).RotationRelativeToPlan);
			}

			this.lstCircuits.Enabled = enableLayoutPage;
			this.cmbOrientation.Enabled = enableLayoutPage;
			this.numRotation.Enabled = enableLayoutPage && !staffeln;
			this.numNewRotation.Enabled = enableLayoutPage && !staffeln;
			this.btnNewCcwLarge.Enabled = enableLayoutPage && !staffeln;
			this.btnNewCcwSmall.Enabled = enableLayoutPage && !staffeln;
			this.btnNewCwSmall.Enabled = enableLayoutPage && !staffeln;
			this.btnNewCwLarge.Enabled = enableLayoutPage && !staffeln;
			this.btnVertical.Enabled = enableLayoutPage && !staffeln;
			this.btnHorizontal.Enabled = enableLayoutPage && !staffeln;
			this.btnColor.Enabled = enableLayoutPage;
			this.numLength.Enabled = enableLayoutPage;
			this.cmbHorizontal.Enabled = enableLayoutPage;
			this.cmbVertical.Enabled = enableLayoutPage && !staffeln;
			this.chkSelectReferenceModule.Enabled = enableLayoutPage;
			this.btnAdd.Enabled = enableLayoutPage;

			this.UpdateLists(true, true, true, false);
		}

		private void numAusrichtung_ValueChanged(object sender, EventArgs e) {
			if (ignoreRotation == 0) {
				this.changed = true;
			}
		}

		private decimal smallRotate = (decimal)0.5;
		private decimal largeRotate = 5;

		private TabPage previousTab = null;

		private void tabs_Selecting(object sender, TabControlCancelEventArgs e) {
			if ((previousTab == this.pageLayout || previousTab == this.pageCalculations) && e.TabPage == this.pageConstruction) {
				if (this.modulKlimaBodenPlanner.Product.ContainsModules || this.modulKlimaBodenPlanner.ContainsNotConfirmedModules) {
					if (MessageBox.Show(Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_KonstruktionAendernText, Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_KonstruktionAendernTitel, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK) {
						e.Cancel = true;
					} else {
						this.modulKlimaBodenPlanner.HighlightCircuit = null;
						this.modulKlimaBodenPlanner.Product.PlannedCircuits.Clear();
						this.modulKlimaBodenPlanner.Product.PlannedCircuits.Add(new ModulKlimaBoden20Circuit(this.modulKlimaBodenPlanner.Product));
						this.modulKlimaBodenPlanner.Product.Connections.Clear();
					}
				}
				if (!e.Cancel) {
					this.SetProductPlanner();
				}
			}
			if (!e.Cancel) {
				this.UpdateToolbar(e.TabPage);
				this.planPanel.InvalidateGraphics();
			}
		}

		private void UpdateToolbar(TabPage tabPage) {
			if (tabPage == this.pageLayout) {
				this.btnAddModules.Visible = true;
				this.btnSelectModule.Visible = true;
				this.btnConstruction.Visible = false;
				this.btnConnections.Visible = true;
				this.btnDeleteConnection.Visible = true;
				this.btnAddAnbindeleitungen.Visible = true;
				this.btnSelectAnbindeleitungen.Visible = true;
				this.sepAnbindeleitungen.Visible = true;
				if (!this.btnAddModules.Checked && !this.btnSelectModule.Checked && !this.btnMove.Checked) {
					this.planPanel.Mode = PlanMode.PM_MOVE;
					this.modulKlimaBodenPlanner.Mode = ModulKlimaBoden20Planner.KlimaBodenMode.KDM_NONE;
					this.UpdateButtons();
				}
			} else if (tabPage == this.pageConstruction) {
				this.btnAddModules.Visible = false;
				this.btnSelectModule.Visible = false;
				this.btnConstruction.Visible = this.modulKlimaBodenPlanner.Product.GraphConstruction is ModulKlimaBoden20ConstructionStaffeln;
				this.btnConnections.Visible = false;
				this.btnDeleteConnection.Visible = false;
				this.btnAddAnbindeleitungen.Visible = false;
				this.btnSelectAnbindeleitungen.Visible = false;
				this.sepAnbindeleitungen.Visible = false;
				if (!this.btnConstruction.Checked && !this.btnMove.Checked) {
					this.planPanel.Mode = PlanMode.PM_MOVE;
					this.modulKlimaBodenPlanner.Mode = ModulKlimaBoden20Planner.KlimaBodenMode.KDM_NONE;
					this.UpdateButtons();
				}
			}
		}

		private void tabs_Deselected(object sender, TabControlEventArgs e) {
			this.previousTab = e.TabPage;
		}

        private void UpdateLists(bool updateCircuits, bool updateSubareas, bool updateRows, bool selectLastCircuit)
        {
			List<Circuit> circuits = (this.modulKlimaBodenPlanner.Product.ContainsModules ? this.modulKlimaBodenPlanner.Product.PlannedCircuits : new List<Circuit>());

			int dec = this.newVisible ? 1 : 0;
			this.newVisible = (this.modulKlimaBodenPlanner.Mode == ModulKlimaBoden20Planner.KlimaBodenMode.KDM_LAYOUT_ADD_AREA ||
				this.modulKlimaBodenPlanner.Mode == ModulKlimaBoden20Planner.KlimaBodenMode.KDM_LAYOUT_ADD_AREA_FINISH) &&  this.modulKlimaBodenPlanner.Product.PlannedCircuits.Count < 12;
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
					lstCircuits.Items.Add(Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_NeuerHK);
				}
				if (selectLastCircuit) {
					ignoreListChange--;
					if (this.newVisible && lstCircuits.Items.Count > 1) {
						lstCircuits.SelectedIndex = lstCircuits.Items.Count - 2;
					} else {
						lstCircuits.SelectedIndex = lstCircuits.Items.Count - 1;
					}
					ignoreListChange++;
				} else {
					ignoreListChange--;
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
							if (tmp >= lstCircuits.Items.Count) {
								lstCircuits.SelectedIndex = lstCircuits.Items.Count - 1;
							} else {
								lstCircuits.SelectedIndex = -1;
							}
						}
					}
					ignoreListChange++;
				}
				lstCircuits.EndUpdate();
			}
            if (updateSubareas)
            {
                int subAreasCount = (this.lstCircuits.SelectedIndex < this.lstCircuits.Items.Count - dec && this.lstCircuits.SelectedIndex >= 0 ? (circuits[this.lstCircuits.SelectedIndex] as ModulKlimaBoden20Circuit).SubAreas.Count : 0);
                int tmp = (lstSubarea.SelectedIndex == lstSubarea.Items.Count - dec ? subAreasCount : lstSubarea.SelectedIndex);
                lstSubarea.BeginUpdate();
                lstSubarea.Items.Clear();
                for (int i = 1; i <= subAreasCount; i++)
                {
                    lstSubarea.Items.Add(EuroplanRes.PlannedModulKlimaBoden20ProductPanel_Teilflaeche + " " + i.ToString());
                }
                if (this.newVisible)
                {
                    lstSubarea.Items.Add(Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_NeueTeilflaeche);
                }
                if (tmp < lstSubarea.Items.Count)
                {
                    if (this.newVisible && tmp < 0 && lstCircuits.SelectedIndex < lstCircuits.Items.Count - dec && lstCircuits.SelectedIndex >= 0)
                    {
                        tmp = lstSubarea.Items.Count - 1;
                    }
                    lstSubarea.SelectedIndex = tmp;
                }
                else
                {
                    lstSubarea.SelectedIndex = -1;
                }
                lstSubarea.EndUpdate();
            }

            if (updateRows)
            {
                int rowsCount = (this.lstSubarea.SelectedIndex < this.lstSubarea.Items.Count - dec && this.lstSubarea.SelectedIndex >= 0 ? (circuits[this.lstCircuits.SelectedIndex] as ModulKlimaBoden20Circuit).SubAreas[this.lstSubarea.SelectedIndex].Rows.Count : 0);
                int tmp = (lstRows.SelectedIndex == lstRows.Items.Count - dec ? rowsCount : lstRows.SelectedIndex);
                lstRows.BeginUpdate();
                lstRows.Items.Clear();
                for (int i = 1; i <= rowsCount; i++)
                {
                    lstRows.Items.Add(EuroplanRes.PlannedModulKlimaBoden20ProductPanel_Reihe + i.ToString());
                }
                if (this.newVisible)
                {
                    lstRows.Items.Add(Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_NeueReihen);
                }
                if (tmp < lstRows.Items.Count)
                {
                    if (this.newVisible && tmp < 0 && lstSubarea.SelectedIndex < lstSubarea.Items.Count - dec && lstSubarea.SelectedIndex >= 0)
                    {
                        tmp = lstRows.Items.Count - 1;
                    }
                    lstRows.SelectedIndex = tmp;
                }
                else
                {
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

                if (this.lstCircuits.SelectedIndex >= 0 && this.lstCircuits.SelectedIndex < this.lstCircuits.Items.Count - dec)
                {
                    if (this.lstSubarea.SelectedIndex < 0 || sender == this.lstCircuits)
                    {
                        if (this.newVisible)
                        {
                            this.lstSubarea.SelectedIndex = this.lstSubarea.Items.Count - 1;
                        }
                        else
                        {
                            this.lstSubarea.SelectedIndex = -1;
                        }
                    }
                }
                else
                {
                    this.lstSubarea.SelectedIndex = -1;
                }

                if (this.lstSubarea.SelectedIndex >= 0 && this.lstSubarea.SelectedIndex < this.lstSubarea.Items.Count - dec)
                {
                    if (this.lstRows.SelectedIndex < 0 || sender == this.lstSubarea)
                    {
                        if (this.newVisible)
                        {
                            this.lstRows.SelectedIndex = this.lstRows.Items.Count - 1;
                        }
                        else
                        {
                            this.lstRows.SelectedIndex = -1;
                        }
                    }
                }
                else
                {
                    this.lstRows.SelectedIndex = -1;
                }

                if (sender != lstRows)
                {
                    this.UpdateLists(false, sender == lstCircuits, true, false);
                }

                if (this.lstRows.SelectedIndex >= 0 && this.lstRows.SelectedIndex < this.lstRows.Items.Count - dec)
                {
                    this.modulKlimaBodenPlanner.HighlightRow = (this.lstRows.Items.Count - dec > this.lstRows.SelectedIndex ? (this.modulKlimaBodenPlanner.Product.PlannedCircuits[this.lstCircuits.SelectedIndex] as ModulKlimaBoden20Circuit).SubAreas[this.lstSubarea.SelectedIndex].Rows[this.lstRows.SelectedIndex] : null);
                    this.UpdateSelectedModules();
                }
                else if (this.lstSubarea.SelectedIndex >= 0 && this.lstSubarea.SelectedIndex < this.lstSubarea.Items.Count - dec)
                {
                    this.modulKlimaBodenPlanner.HighlightSubArea = (this.lstSubarea.Items.Count - dec > this.lstSubarea.SelectedIndex ? (this.modulKlimaBodenPlanner.Product.PlannedCircuits[this.lstCircuits.SelectedIndex] as ModulKlimaBoden20Circuit).SubAreas[this.lstSubarea.SelectedIndex] : null);
                    this.UpdateSelectedModules();
                }
                else if (this.lstCircuits.SelectedIndex >= 0 && this.lstCircuits.SelectedIndex < this.lstCircuits.Items.Count - dec)
                {
                    this.modulKlimaBodenPlanner.HighlightCircuit = (this.lstCircuits.Items.Count - dec > this.lstCircuits.SelectedIndex ? (this.modulKlimaBodenPlanner.Product.PlannedCircuits[this.lstCircuits.SelectedIndex] as ModulKlimaBoden20Circuit) : null);
                    this.UpdateSelectedModules();
                }
                else
                {
                    this.modulKlimaBodenPlanner.HighlightCircuit = null;
                    this.UpdateSelectedModules();
                }

                numLength.Enabled = GetSelectedRow() != null && GetSelectedRow().List.Count > 0;

                if (GetSelectedRow() != null)
                {
                    this.numLength.Value = (decimal)GetSelectedRow().LengthVerbindeleitungen;
                }

                if (this.lstCircuits.SelectedIndex >= 0 && this.lstCircuits.SelectedIndex < this.lstCircuits.Items.Count - dec)
                {
                    ModulKlimaBoden20Circuit circuit = (this.lstCircuits.Items.Count - dec > this.lstCircuits.SelectedIndex ? (this.modulKlimaBodenPlanner.Product.PlannedCircuits[this.lstCircuits.SelectedIndex] as ModulKlimaBoden20Circuit) : null);
                    if (circuit != null)
                    {
                        btnColor.BackColor = circuit.CircuitColor;
                        btnColor.Enabled = true;
                    }
                    else
                    {
                        btnColor.BackColor = Color.Transparent;
                        btnColor.Enabled = false;
                    }
                }
                else
                {
                    btnColor.BackColor = Color.Transparent;
                    btnColor.Enabled = false;
                }						

				this.ignoreListChange--;
			}
		}

        private KlimaFlaechenList GetSelectedRow()
        {
            return this.modulKlimaBodenPlanner.HighlightRow;
        }

        private ModulKlimaBoden20Circuit GetSelectedCircuit()
        {
            return this.modulKlimaBodenPlanner.HighlightCircuit;
        }

		private void cmbModulType_SelectedIndexChanged(object sender, EventArgs e) {
			if (cmbOrientation.SelectedItem is KlimaFlaechenModul.ModulOrientationEnum) {
				this.modulKlimaBodenPlanner.NewModulesStartingOrientation = (KlimaFlaechenModul.ModulOrientationEnum)cmbOrientation.SelectedItem;
				this.planPanel.InvalidateGraphics();
			}
		}

		private void UpdateSelectedModules() {
			this.ignoreModuleOrientationChange++;

			if ((this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.modulKlimaBodenPlanner.Mode == ModulKlimaBoden20Planner.KlimaBodenMode.KDM_PICK_MODULE) {
                if (this.modulKlimaBodenPlanner.HighlightCircuit == null && this.modulKlimaBodenPlanner.HighlightSubArea == null && this.modulKlimaBodenPlanner.HighlightRow == null)
                {
					this.lstCircuits.SelectedIndex = -1;
				}
			}

			List<KlimaFlaechenModul> module = this.modulKlimaBodenPlanner.GetAllSelectedModules();

			this.btnInvertDirection.Enabled = module != null && module.Count > 0;

			if (module != null && module.Count > 0) {
				Nullable<KlimaFlaechenModul.ModulOrientationEnum> orientation = null;
				bool orientationOk = true;
				foreach (KlimaFlaechenModul modul in module) {
					if (!orientation.HasValue) {
						orientation = modul.Orientation;
					}
					if (orientation != modul.Orientation) {
						orientationOk = false;
					}
					if (!orientationOk) {
						break;
					}
				}
				if (orientationOk && orientation.HasValue) {
					this.cmbSelectedModuleOrientation.SelectedItem = orientation.Value;
				} else {
					this.cmbSelectedModuleOrientation.SelectedIndex = -1;
				}

				if (orientation.HasValue) {
					if (orientationOk) {
						this.lblOrientationError.Text = "";
						if (this.cmbSelectedModuleOrientation.Items.Count == 3) {
							this.cmbSelectedModuleOrientation.Items.RemoveAt(2);
						}
					} else {
						this.lblOrientationError.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_VerschiedeneAusrichtungen;
						if (this.cmbSelectedModuleOrientation.Items.Count == 2) {
							this.cmbSelectedModuleOrientation.Items.Add(Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_AlleUmdrehen);
						}
					}
					this.cmbSelectedModuleOrientation.Enabled = true;
				} else {
					this.lblTypError.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_KeinModul;
					this.cmbSelectedModuleOrientation.Enabled = false;
				}
			} else {
				this.lblTypError.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_KeinModul;
				this.cmbSelectedModuleOrientation.SelectedIndex = -1;
				this.cmbSelectedModuleOrientation.Enabled = false;
			}

			if (module.Count == 0) {
				this.llHk.Enabled = false;
				this.llHk.Tag = null;
				this.llHk.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_KeinHK;
			} else {
				ModulKlimaBoden20Circuit circuit = null;
				bool circuitOk = true;
				int circuitIndex = 0;
				foreach (KlimaFlaechenModul modul in module) {
					ModulKlimaBoden20Circuit curCircuit = this.modulKlimaBodenPlanner.Product.GetCircuitForModul(modul, out circuitIndex);
					if (circuit != null && circuit != curCircuit) {
						circuitOk = false;
					}
					if (curCircuit == null) {
						circuitOk = false;
					} else {
						circuit = curCircuit;
					}
				}
				if (circuitOk) {
					this.llHk.Tag = circuitIndex;
					circuitIndex++;
					this.llHk.Text = Europlan.Common.EuroplanRes.PlannedModulKlimaDeckeProductPanel_HeizkreisAbkuerzung + circuitIndex.ToString();
					this.llHk.Enabled = true;
				} else {
					this.llHk.Tag = null;
					this.llHk.Enabled = false;
					this.llHk.Text = Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_VerschiendeHK;
				}
			}
			this.ignoreModuleOrientationChange--;
		}

		private int ignoreModuleTypeChange = 0;
		private int ignoreModuleOrientationChange = 0;

		private void cmbSelectedModuleType_SelectedIndexChanged(object sender, EventArgs e) {
			if (ignoreModuleTypeChange == 0) {
				this.changed = true;
				if (this.modulKlimaBodenPlanner.Product == null || this.modulKlimaBodenPlanner.Product.AssociatedRoom == null ||
					this.modulKlimaBodenPlanner.Product.AssociatedRoom.AssociatedPlan == null || this.modulKlimaBodenPlanner.Product.AssociatedRoom.AssociatedPlan.Measure == null) {
					return;
				}
			}
		}

        private void cmbSelectedModuleOrientation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ignoreModuleOrientationChange == 0)
            {
                List<KlimaFlaechenModulVerbindung> linksToUpdate = new List<KlimaFlaechenModulVerbindung>();
                Dictionary<KlimaFlaechenModulVerbindung, KlimaFlaechenList> linksToDelete = new Dictionary<KlimaFlaechenModulVerbindung, KlimaFlaechenList>();
                Dictionary<KlimaFlaechenSubAreaVerbindung, ModulKlimaBoden20Circuit> saLinksToDelete = new Dictionary<KlimaFlaechenSubAreaVerbindung, ModulKlimaBoden20Circuit>();
                this.changed = true;
                int tmp;
                List<KlimaFlaechenModul> modules = this.modulKlimaBodenPlanner.GetAllSelectedModules();
                ModulKlimaBoden20Circuit c;
                bool invertYAxis = this.modulKlimaBodenPlanner.Product.AssociatedRoom.AssociatedPlan.InvertYAxis;
                double measure = this.modulKlimaBodenPlanner.Product.AssociatedRoom.AssociatedPlan.Measure.Value;
                KlimaFlaechenModul next, prev;
                KlimaFlaechenModulVerbindung link;
                KlimaFlaechenSubAreaVerbindung saLink;
                ModulKlimaBoden20SubArea sa;
                KlimaFlaechenList row;
                if (cmbSelectedModuleOrientation.SelectedIndex == 2)
                {
                    foreach (KlimaFlaechenModul modul in modules)
                    {
                        modul.Orientation = (modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT ? KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT : KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT);
                        c = this.modulKlimaBodenPlanner.Product.GetCircuitForModul(modul, out tmp);
                        link = modul.GetOutputLink(c, invertYAxis);
                        next = (link == null ? null : link.End);
                        if (next != null)
                        {
                            if (modules.Contains(next) && link.Vertices.Count == 2)
                            {
                                if (!linksToUpdate.Contains(link))
                                {
                                    linksToUpdate.Add(link);
                                }
                            }
                            else if (!linksToDelete.ContainsKey(link))
                            {
                                sa = c.GetSubareaForModul(modul, out tmp);
                                row = sa.GetRowForModul(modul, out tmp);
                                linksToDelete.Add(link, row);
                            }
                        }
                        link = modul.GetInputLink(c, invertYAxis);
                        prev = (link == null ? null : link.Start);
                        if (prev != null)
                        {
                            if (modules.Contains(prev) && link.Vertices.Count == 2)
                            {
                                if (!linksToUpdate.Contains(link))
                                {
                                    linksToUpdate.Add(link);
                                }
                            }
                            else if (!linksToDelete.ContainsKey(link))
                            {
                                sa = c.GetSubareaForModul(modul, out tmp);
                                row = sa.GetRowForModul(modul, out tmp);
                                linksToDelete.Add(link, row);
                            }
                        }
                        saLink = modul.GetSubareaOutputLink(c, invertYAxis);
                        if (saLink != null && !saLinksToDelete.ContainsKey(saLink))
                        {
                            saLinksToDelete.Add(saLink, c);
                        }
                        saLink = modul.GetSubareaInputLink(c, invertYAxis);
                        if (saLink != null && !saLinksToDelete.ContainsKey(saLink))
                        {
                            saLinksToDelete.Add(saLink, c);
                        }
                    }
                }
                else if (this.cmbSelectedModuleOrientation.SelectedItem is KlimaFlaechenModul.ModulOrientationEnum)
                {
                    foreach (KlimaFlaechenModul modul in modules)
                    {
                        bool orientationChanged = (modul.Orientation != (KlimaFlaechenModul.ModulOrientationEnum)this.cmbSelectedModuleOrientation.SelectedItem);
                        modul.Orientation = (KlimaFlaechenModul.ModulOrientationEnum)this.cmbSelectedModuleOrientation.SelectedItem;
                        if (orientationChanged)
                        {
                            c = this.modulKlimaBodenPlanner.Product.GetCircuitForModul(modul, out tmp);
                            link = modul.GetOutputLink(c, invertYAxis);
                            if (link != null && !linksToDelete.ContainsKey(link))
                            {
                                sa = c.GetSubareaForModul(modul, out tmp);
                                row = sa.GetRowForModul(modul, out tmp);
                                linksToDelete.Add(link, row);
                            }
                            link = modul.GetInputLink(c, invertYAxis);
                            if (link != null && !linksToDelete.ContainsKey(link))
                            {
                                sa = c.GetSubareaForModul(modul, out tmp);
                                row = sa.GetRowForModul(modul, out tmp);
                                linksToDelete.Add(link, row);
                            }
                            saLink = modul.GetSubareaOutputLink(c, invertYAxis);
                            if (saLink != null && !saLinksToDelete.ContainsKey(saLink))
                            {
                                saLinksToDelete.Add(saLink, c);
                            }
                            saLink = modul.GetSubareaInputLink(c, invertYAxis);
                            if (saLink != null && !saLinksToDelete.ContainsKey(saLink))
                            {
                                saLinksToDelete.Add(saLink, c);
                            }
                        }
                    }
                }
                foreach (KeyValuePair<KlimaFlaechenModulVerbindung, KlimaFlaechenList> linkToDelete in linksToDelete)
                {
                    linkToDelete.Value.Links.Remove(linkToDelete.Key);
                }
                foreach (KlimaFlaechenModulVerbindung linkToUpdate in linksToUpdate)
                {
                    linkToUpdate.Vertices[0] = linkToUpdate.Start.GetOutputConnection(measure, invertYAxis, this.modulKlimaBodenPlanner.Product);
                    linkToUpdate.Vertices[1] = linkToUpdate.End.GetInputConnection(measure, invertYAxis, this.modulKlimaBodenPlanner.Product);
                }
                foreach (KeyValuePair<KlimaFlaechenSubAreaVerbindung, ModulKlimaBoden20Circuit> saLinkToDelete in saLinksToDelete)
                {
                    saLinkToDelete.Value.Links.Remove(saLinkToDelete.Key);
                }
                this.UpdateSelectedModules();
                this.planPanel.InvalidateGraphics();
            }           
        }

		private void ModulKlimaBodenPlannerForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ModulKlimaBoden20PlannerForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
		}

		private void ModulKlimaBodenPlannerForm_FormClosing(object sender, FormClosingEventArgs e) {
			this.connectionPlanner.ReGenerateConnectionPipes();

			SettingsKey settings = SettingsFile.Settings["ModulKlimaBoden20PlannerForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			SettingsFile.Update();
		}

		private void llHk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			object hkTag = llHk.Tag;

			if (hkTag != null && this.lstCircuits.Items.Count > (int)hkTag) {
				this.lstCircuits.SelectedIndex = -1;
				this.lstCircuits.SelectedIndex = (int)hkTag;
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
			notifications = ModulKlimaBoden20Product.GlobalNotificationMessage;
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

			double diff = 0;
			if (showHeat) {
				if (showCool) {
					diff = Math.Min(qDiffHeat, qDiffCool);
				} else {
					diff = qDiffHeat;
				}
			} else if (showCool) {
				diff = qDiffCool;

			}
			lblQDiff.Text = Math.Round(diff, 0).ToString("+0;-0");
		}

		private void button1_Click(object sender, EventArgs e) {
			CalculateAndUpdate();
		}

#warning TODO: for each row
		private void numLength_ValueChanged(object sender, EventArgs e) {
			if (this.modulKlimaBodenPlanner.HighlightCircuit != null) {
				//this.modulKlimaBodenPlanner.HighlightCircuit.SonstigeVerbindeleitung = (double)this.numLength.Value;
				this.CalculateAndUpdate();
			}
		}

		private void modulKlimaBodenPlanner_ProjectChanged(object sender) {
			this.CalculateAndUpdate();
			this.changed = true;
		}

		public bool Changed {
			get {
				return this.changed || true;
			}
		}

		private int ignoreRandfries = 0;
		private void numRandfries_ValueChanged(object sender, EventArgs e) {
			if (ignoreRandfries == 0) {
				this.changed = true;
				this.planPanel.InvalidateGraphics();
			}
		}

		private int ignoreBeplankung = 0;

		private void cbBeplankung_CheckedChanged(object sender, EventArgs e) {
			if (ignoreBeplankung == 0) {
				// Akustik is subclass of Glatt!!!
			}
		}

		private int ignoreBeplankungValue = 0;
		private void numBeplankung_ValueChanged(object sender, EventArgs e) {
			if (this.ignoreBeplankungValue == 0) {
				// Akustik is subclass of Glatt!!!
			}
		}

		private void btnColor_Click(object sender, EventArgs e) {
			int dec = this.newVisible ? 1 : 0;
			if (this.lstCircuits.SelectedIndex >= 0 && this.lstCircuits.SelectedIndex < this.lstCircuits.Items.Count - dec) {
				ModulKlimaBoden20Circuit circuit = (this.lstCircuits.Items.Count - dec > this.lstCircuits.SelectedIndex ? (this.modulKlimaBodenPlanner.Product.PlannedCircuits[this.lstCircuits.SelectedIndex] as ModulKlimaBoden20Circuit) : null);
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

		private void btnInvertDirection_Click(object sender, EventArgs e) {
			this.changed = true;
			List<KlimaFlaechenModul> selectedModules = this.modulKlimaBodenPlanner.GetAllSelectedModules();
			List<KlimaFlaechenModul> modulesToInvert = new List<KlimaFlaechenModul>();
            List<IKlimaFlaechenVerbindung> linksToInvert = new List<IKlimaFlaechenVerbindung>();

            Dictionary<IKlimaFlaechenVerbindung, ModulKlimaBoden20Circuit> anbindungen = new Dictionary<IKlimaFlaechenVerbindung, ModulKlimaBoden20Circuit>();

			foreach (KlimaFlaechenModul m1 in selectedModules) {
				int tmp;
				ModulKlimaBoden20Circuit c = this.modulKlimaBodenPlanner.Product.GetCircuitForModul(m1, out tmp);
#warning TODO improve
                IKlimaFlaechenVerbindung link = c.GetNextLink(m1);
                /*
				if (link != null) {
					if (!linksToInvert.Contains(link)) {
						linksToInvert.Add(link);
					}
					if (link.End == null) {
						anbindungen[link] = c;
					}
				}
				link = c.GetPreviousLink(m1);
				if (link != null) {
					if (!linksToInvert.Contains(link)) {
						linksToInvert.Add(link);
					}
					if (link.Start == null) {
						anbindungen[link] = c;
					}
				}
				foreach (KlimaFlaechenModul m2 in c.GetAllLinkedModules(m1)) {
					link = c.GetNextLink(m2);
					if (link != null) {
						if (!linksToInvert.Contains(link)) {
							linksToInvert.Add(link);
						}
						if (link.End == null) {
							anbindungen[link] = c;
						}
					}
					link = c.GetPreviousLink(m2);
					if (link != null) {
						if (!linksToInvert.Contains(link)) {
							linksToInvert.Add(link);
						}
						if (link.Start == null) {
							anbindungen[link] = c;
						}
					}
					if (!modulesToInvert.Contains(m2)) {
						modulesToInvert.Add(m2);
					}
				}
                */
			}           

			if (anbindungen.Count > 0) {
				if (MessageBox.Show(Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_RichtungAendernText, Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_RichtungAendernTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) {
					return;
				}
#warning TODO
				foreach (KeyValuePair<IKlimaFlaechenVerbindung, ModulKlimaBoden20Circuit> kvp in anbindungen) {
					//kvp.Value.Links.Remove(kvp.Key);
				}
			}

			foreach (KlimaFlaechenModul modul in modulesToInvert) {
				modul.GraphBottomUp = !modul.GraphBottomUp;
			}
			foreach (KlimaFlaechenModulVerbindung link in linksToInvert) {
				KlimaFlaechenModul tmp = link.Start;
				link.Start = link.End;
				link.End = tmp;
			}
			this.UpdateSelectedModules();
			this.planPanel.InvalidateGraphics();
		}

		private void btnConstruction_Click(object sender, EventArgs e) {
			if (!btnConstruction.Checked) {
				this.SetProductPlanner();
				this.modulKlimaBodenPlanner.Mode = ModulKlimaBoden20Planner.KlimaBodenMode.KDM_CONSTRUCTION;
				this.planPanel.Mode = PlanMode.PM_PLANNER_DRAG;
				this.UpdateButtons();
			}
		}

		private void btnHorizontal_Click(object sender, EventArgs e) {
			this.changed = true;
			this.numRotation.Value = 90;
		}

		private void btnVertical_Click(object sender, EventArgs e) {
			this.changed = true;
			this.numRotation.Value = 0;
		}

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

		private void btnNewHorizontal_Click(object sender, EventArgs e) {
			this.numNewRotation.Value = 90;
		}

		private void btnNewVertical_Click(object sender, EventArgs e) {
			this.numNewRotation.Value = 0;
		}

		private void btnNewRotate_Click(object sender, EventArgs e) {
			decimal rotation = 0;
			if (sender == this.btnNewCcwLarge) {
				rotation = -largeRotate;
			} else if (sender == this.btnNewCcwSmall) {
				rotation = -smallRotate;
			} else if (sender == btnNewCwLarge) {
				rotation = largeRotate;
			} else if (sender == btnNewCwSmall) {
				rotation = smallRotate;
			}
			decimal value = this.numNewRotation.Value + rotation;
			while (value < 0) {
				value += 360;
			}
			while (value >= 360) {
				value -= 360;
			}
			this.numNewRotation.Value = value;
		}

		private void numNewRotation_ValueChanged(object sender, EventArgs e) {
			this.modulKlimaBodenPlanner.NewModulesRotation = (double)this.numNewRotation.Value;
		}

		private void btnAdd_Click(object sender, EventArgs e) {
			ModulKlimaBoden20Circuit newCircuit = this.modulKlimaBodenPlanner.ConfirmNewModules();
			if (newCircuit != null) {
				this.UpdateLists(true, true, true, true);
			}
		}

		private void cmbHorizontal_SelectedIndexChanged(object sender, EventArgs e) {
			this.modulKlimaBodenPlanner.NewModulesYDicht = (ModulKlimaBoden20Planner.VerlegungsAbstand)cmbHorizontal.SelectedIndex;
		}

		private void cmbVertical_SelectedIndexChanged(object sender, EventArgs e) {
			this.modulKlimaBodenPlanner.NewModulesXDicht = (ModulKlimaBoden20Planner.VerlegungsAbstand)cmbVertical.SelectedIndex;
		}

		private void chkSelectReferenceModule_CheckedChanged(object sender, EventArgs e) {
			if (this.chkSelectReferenceModule.Checked && this.modulKlimaBodenPlanner.Mode == ModulKlimaBoden20Planner.KlimaBodenMode.KDM_LAYOUT_ADD_AREA_FINISH) {
				this.planPanel.Mode = PlanMode.PM_PLANNER_CLICK;
				this.modulKlimaBodenPlanner.Mode = ModulKlimaBoden20Planner.KlimaBodenMode.KDM_LAYOUT_ADD_AREA_PICK_REFERENCE;
			} else if (!this.chkSelectReferenceModule.Checked && this.modulKlimaBodenPlanner.Mode == ModulKlimaBoden20Planner.KlimaBodenMode.KDM_LAYOUT_ADD_AREA_PICK_REFERENCE) {
				this.planPanel.Mode = PlanMode.PM_PLANNER_DRAG;
				this.modulKlimaBodenPlanner.Mode = ModulKlimaBoden20Planner.KlimaBodenMode.KDM_LAYOUT_ADD_AREA_FINISH;
			}
		}

		private void modulKlimaBodenPlanner_ModeChanged(object sender, EventArgs e) {
			if (this.modulKlimaBodenPlanner.Mode == ModulKlimaBoden20Planner.KlimaBodenMode.KDM_LAYOUT_ADD_AREA_FINISH) {
				this.numNewRotation.Enabled = false;
				this.btnNewCcwLarge.Enabled = false;
				this.btnNewCcwSmall.Enabled = false;
				this.btnNewCwLarge.Enabled = false;
				this.btnNewCwSmall.Enabled = false;
				this.btnNewHorizontal.Enabled = false;
				this.btnNewVertical.Enabled = false;
				this.btnAdd.Enabled = true;
				this.chkSelectReferenceModule.Enabled = true;
				if (this.planPanel.Mode != PlanMode.PM_PLANNER_DRAG) {
					this.planPanel.Mode = PlanMode.PM_PLANNER_DRAG;
				}
			} else {
				this.numNewRotation.Enabled = true;
				this.btnNewCcwLarge.Enabled = true;
				this.btnNewCcwSmall.Enabled = true;
				this.btnNewCwLarge.Enabled = true;
				this.btnNewCwSmall.Enabled = true;
				this.btnNewHorizontal.Enabled = true;
				this.btnNewVertical.Enabled = true;
			}
			if (this.modulKlimaBodenPlanner.Mode != ModulKlimaBoden20Planner.KlimaBodenMode.KDM_LAYOUT_ADD_AREA_PICK_REFERENCE) {
				if (this.modulKlimaBodenPlanner.Mode != ModulKlimaBoden20Planner.KlimaBodenMode.KDM_LAYOUT_ADD_AREA_FINISH) {
					this.btnAdd.Enabled = false;
					this.chkSelectReferenceModule.Enabled = false;
				}
				this.chkSelectReferenceModule.Checked = false;
			}
		}

		private void numRotation_ValueChanged(object sender, EventArgs e) {
			if (ignoreRotation == 0) {
				this.changed = true;
				if (this.modulKlimaBodenPlanner.Product.GraphConstruction is ModulKlimaBoden20ConstructionStaffeln) {
					ModulKlimaBoden20ConstructionStaffeln staffeln = this.modulKlimaBodenPlanner.Product.GraphConstruction as ModulKlimaBoden20ConstructionStaffeln;
					staffeln.RotationRelativeToPlan = (double)this.numRotation.Value;
					this.planPanel.InvalidateGraphics();
				}
			}
		}

		private void numStaffelBreite_ValueChanged(object sender, EventArgs e) {
			if (ignoreStaffelBreite == 0) {
				this.changed = true;
				if (this.modulKlimaBodenPlanner.Product.GraphConstruction is ModulKlimaBoden20ConstructionStaffeln) {
					ModulKlimaBoden20ConstructionStaffeln staffeln = this.modulKlimaBodenPlanner.Product.GraphConstruction as ModulKlimaBoden20ConstructionStaffeln;
					staffeln.StaffelnBreite = (double)this.numStaffelBreite.Value / 1000.0;
					this.planPanel.InvalidateGraphics();
				}
			}
		}

		private void lst_KeyDown(object sender, KeyEventArgs e) {
			this.modulKlimaBodenPlanner.PlannerKeyPress(e.KeyCode);
		}

		private void numAchsAbstand_ValueChanged(object sender, EventArgs e) {
			if (ignoreAchsAbstand == 0) {
				this.changed = true;
				if (this.modulKlimaBodenPlanner.Product.GraphConstruction is ModulKlimaBoden20ConstructionStaffeln) {
					ModulKlimaBoden20ConstructionStaffeln staffeln = this.modulKlimaBodenPlanner.Product.GraphConstruction as ModulKlimaBoden20ConstructionStaffeln;
					staffeln.StaffelnAchsabstand = (double)this.numAchsAbstand.Value / 1000.0;
					this.planPanel.InvalidateGraphics();
				}
			}
		}

		private void rbHolzstaffeln_CheckedChanged(object sender, EventArgs e) {
			if (ignoreConstruction == 0 && this.rbHolzstaffeln.Checked) {
				if (!(this.modulKlimaBodenPlanner.Product.GraphConstruction is ModulKlimaBoden20ConstructionStaffeln)) {
					this.changed = true;
					this.modulKlimaBodenPlanner.Product.GraphConstruction = this.staffeln;
					this.UpdateToolbar(this.tabs.SelectedTab);
					this.UpdateControls();
					this.planPanel.InvalidateGraphics();
				}
			}
		}

		private void rbFrei_CheckedChanged(object sender, EventArgs e) {
			if (ignoreConstruction == 0 && this.rbFrei.Checked) {
				if (!(this.modulKlimaBodenPlanner.Product.GraphConstruction is ModulKlimaBoden20ConstructionFrei)) {
					this.changed = true;
					this.modulKlimaBodenPlanner.Product.GraphConstruction = this.frei;
					this.UpdateToolbar(this.tabs.SelectedTab);
					this.UpdateControls();
					this.planPanel.InvalidateGraphics();
				}
			}
		}

		private void btnConnections_Click(object sender, EventArgs e) {
			if (!this.btnConnections.Checked) {
				this.SetProductPlanner();
				this.modulKlimaBodenPlanner.Mode = ModulKlimaBoden20Planner.KlimaBodenMode.KDM_ADD_CONNECTIONS;
				this.planPanel.Mode = PlanMode.PM_PLANNER_CLICK;
				this.UpdateButtons();
			}
		}

		private void btnDeleteConnection_Click(object sender, EventArgs e) {
			if (!this.btnDeleteConnection.Checked) {
				this.SetProductPlanner();
				this.modulKlimaBodenPlanner.Mode = ModulKlimaBoden20Planner.KlimaBodenMode.KDM_DEL_CONNECTION;
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

        private void modulKlimaBodenPlanner_ListsNeedUpdate(object sender, ModulKlimaBoden20Planner.ListNeedsUpdateEventArgs e)
        {
			this.UpdateLists(true, true, true, e.selectLastCircuit);
		}

        private void modulKlimaBodenPlanner_ModuleSelected(object sender, ModulKlimaBoden20Planner.ModuleSelectedEventArgs e)
        {
			this.UpdateSelectedModules();
		}

		private void modulKlimaBodenPlanner_UpdateNewCount(object sender, ModulKlimaBoden20Planner.UpdateNewCountArgs e) {
			if (e.count == 0) {
				this.lblNewModules.Visible = false;
			} else {
				this.lblNewModules.Text = e.count.ToString() + " " + Europlan.Common.EuroplanRes.ModulKlimaBoden20PlannerForm_NeueModule;
				this.lblNewModules.Visible = true;
			}
		}

        private void btnShowPlanBg_Click(object sender, EventArgs e) {
            Product.ShowPlanInBackground = !Product.ShowPlanInBackground;
            this.btnShowPlanBg.Checked = Product.ShowPlanInBackground;
            this.planPanel.InvalidateGraphics();
        }
	}
}