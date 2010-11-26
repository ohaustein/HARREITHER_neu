using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;

namespace Europlan.Common.Products {
	public partial class ModulKlimaDeckePlannerForm : Form {

		private ModulKlimaDeckeConstructionGlatt glatt = null;
		private ModulKlimaDeckeConstructionAkustik akustik = null;
		private ModulKlimaDeckeConstructionKassette kassette = null;

		private int ignoreRotation = 0;
		private bool newVisible = false;
		private PlannedProduct plannedProduct;

		private bool changed = false;

		public ModulKlimaDeckePlannerForm(PlannedProduct plannedProduct) {
			InitializeComponent();
			this.plannedProduct = plannedProduct;
			this.cbAutomaticOrientation.Checked = this.modulKlimaBodenPlanner.AutomaticOrientation;
			this.cbAutomaticRows.Checked = this.modulKlimaBodenPlanner.AutomaticRows;
			ModulKlimaDeckeProduct product = plannedProduct.Product as ModulKlimaDeckeProduct;

			ModulKlimaDeckeProduct.ModulCeilingConstructionEnum constrType = (ModulKlimaDeckeProduct.ModulCeilingConstructionEnum)ModulKlimaDeckeProduct.ConfigModulCeilingConstruction;

			this.modulKlimaBodenPlanner.Product = product;
			if (product.GraphConstruction == null) {
				product.GraphConstruction = new ModulKlimaDeckeConstructionGlatt();
				product.GraphConstruction.Planner = this.modulKlimaBodenPlanner;
			}
			if (product.GraphConstruction == null) {
				product.GraphConstruction = (constrType == ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.KASSETTENDECKE ? (ModulKlimaDeckeConstruction)new ModulKlimaDeckeConstructionKassette() : (ModulKlimaDeckeConstruction)new ModulKlimaDeckeConstructionGlatt());
			}
			this.glatt = product.GraphConstruction as ModulKlimaDeckeConstructionGlatt;
			this.akustik = product.GraphConstruction as ModulKlimaDeckeConstructionAkustik;
			this.kassette = product.GraphConstruction as ModulKlimaDeckeConstructionKassette;
			if (this.glatt == null) {
				this.glatt = new ModulKlimaDeckeConstructionGlatt();
				this.glatt.Planner = this.modulKlimaBodenPlanner;
				this.glatt.RotationRelativeToPlan = 0;
			} else {
				this.glatt.Planner = this.modulKlimaBodenPlanner;
			}
			this.glatt.RecalculateSchienen();

			if (this.akustik == null) {
				this.akustik = new ModulKlimaDeckeConstructionAkustik();
				this.akustik.Planner = this.modulKlimaBodenPlanner;
				this.akustik.RotationRelativeToPlan = 0;
			} else {
				this.akustik.Planner = this.modulKlimaBodenPlanner;
			}
			this.numRandfries.Value = (decimal)Math.Round(this.akustik.Randfries * 100.0);
			this.akustik.RecalculateSchienen();

			if (this.kassette == null) {
				this.kassette = new ModulKlimaDeckeConstructionKassette();
				this.kassette.Planner = this.modulKlimaBodenPlanner;
				this.kassette.RotationRelativeToPlan = 0;
			} else {
				this.kassette.Planner = this.modulKlimaBodenPlanner;
			}
			this.kassette.RecalculateSchienen();

			if (constrType == ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.C_PROFIL) {
				this.rbGlatt.Visible = true;
				this.rbAkustik.Visible = true;
				this.rbKassetten.Visible = false;
				this.glatt.SchienenBreite = 0.065;
				this.akustik.SchienenBreite = 0.065;
			} else if (constrType == ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.HOLZSTAFFEL) {
				this.rbGlatt.Visible = true;
				this.rbAkustik.Visible = true;
				this.rbKassetten.Visible = false;
				this.glatt.SchienenBreite = 0.045;
				this.akustik.SchienenBreite = 0.045;
			} else if (constrType == ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.KASSETTENDECKE) {
				this.rbGlatt.Visible = false;
				this.rbAkustik.Visible = false;
				this.rbKassetten.Visible = true;
			}

			if (product.GraphConstruction is ModulKlimaDeckeConstructionGlatt) {
				this.rbGlatt.Checked = true;
			} else if (product.GraphConstruction is ModulKlimaDeckeConstructionAkustik) {
				this.rbAkustik.Checked = true;
			} else if (product.GraphConstruction is ModulKlimaDeckeConstructionKassette) {
				this.rbKassetten.Checked = true;
			}

			this.cmbOrientation.Items.Add(KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT);
			this.cmbOrientation.Items.Add(KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT);
			this.cmbOrientation.SelectedIndex = 0;
			this.cmbSelectedModuleOrientation.Items.Add(KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT);
			this.cmbSelectedModuleOrientation.Items.Add(KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT);
			this.cmbSelectedModuleOrientation.SelectedIndex = -1;
			this.cmbSelectedModuleOrientation.Enabled = false;

			this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_80_30);
			this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_30);
			this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_120_30);
			this.cmbModulType.SelectedIndex = 2;
			this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_80_30);
			this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_30);
			this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_120_30);
			this.cmbSelectedModuleType.SelectedIndex = -1;
			this.cmbSelectedModuleType.Enabled = false;
			this.lblTypError.Text = "Kein Modul ausgewählt";

			this.UpdateLists(true, true, true, false);

			if (product.ContainsModules) {
				this.tabs.SelectedTab = this.pageLayout;
			} else {
				this.tabs.SelectedTab = this.pageConstruction;
			}
			this.UpdateToolbar(this.tabs.SelectedTab);
			this.UpdateButtons();
			this.UpdateControls();
			this.CalculateAndUpdate();
		}

		private void UpdateControls() {
			if (this.modulKlimaBodenPlanner.Product.GraphConstruction is ModulKlimaDeckeConstructionAkustik) {
				rbAkustik.Checked = true;
			} else if (this.modulKlimaBodenPlanner.Product.GraphConstruction is ModulKlimaDeckeConstructionGlatt) {
				rbGlatt.Checked = true;
			} else if (this.modulKlimaBodenPlanner.Product.GraphConstruction is ModulKlimaDeckeConstructionKassette) {
				rbKassetten.Checked = true;
			}
			if (this.rbGlatt.Checked) {
				ignoreRotation++;
				this.lblRandfries.Visible = false;
				this.numRandfries.Visible = false;
				this.lblRandfriesUnit.Visible = false;
				this.grpModulSerie.Visible = true;
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
			} else if (this.rbAkustik.Checked) {
				ignoreRotation++;
				this.lblRandfries.Visible = true;
				this.numRandfries.Visible = true;
				this.lblRandfriesUnit.Visible = true;
				this.grpModulSerie.Visible = true;
				this.grpModulSerie.Visible = true;
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
			} else if (this.rbKassetten.Checked) {
				ignoreRotation++;
				this.lblRandfries.Visible = false;
				this.numRandfries.Visible = false;
				this.lblRandfriesUnit.Visible = false;
				this.grpModulSerie.Visible = false;
				this.numRotation.Value = (decimal)this.kassette.RotationRelativeToPlan;
				if (!this.rbSerie40.Checked) {
					this.rbSerie40.Checked = true;
				}
				ignoreRotation--;
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

		private void btnAddModules_Click(object sender, EventArgs e) {
			if (!btnAddModules.Checked) {
				this.modulKlimaBodenPlanner.Mode = ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_LAYOUT_ADD_AREA;
				this.planPanel.Mode = PlanMode.PM_PLANNER_DRAG;
				this.UpdateButtons();
			}
		}

		private void btnSelectModule_Click(object sender, EventArgs e) {
			if (!btnSelectModule.Checked) {
				this.modulKlimaBodenPlanner.Mode = ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_PICK_MODULE;
				this.planPanel.Mode = PlanMode.PM_PLANNER_DRAG;
				this.UpdateButtons();
			}
		}

		private void UpdateButtons() {
			if (this.planPanel.Mode == PlanMode.PM_MOVE) {
				this.btnMove.Checked = true;
				this.btnConstruction.Checked = false;
				this.btnAddModules.Checked = false;
				this.btnSelectModule.Checked = false;
			} else if ((this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.modulKlimaBodenPlanner.Mode == ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_CONSTRUCTION) {
				this.btnMove.Checked = false;
				this.btnConstruction.Checked = true;
				this.btnAddModules.Checked = false;
				this.btnSelectModule.Checked = false;
			} else if ((this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.modulKlimaBodenPlanner.Mode == ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_LAYOUT_ADD_AREA) {
				this.btnMove.Checked = false;
				this.btnConstruction.Checked = false;
				this.btnAddModules.Checked = true;
				this.btnSelectModule.Checked = false;
			} else if ((this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.modulKlimaBodenPlanner.Mode == ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_PICK_MODULE) {
				this.btnMove.Checked = false;
				this.btnConstruction.Checked = false;
				this.btnAddModules.Checked = false;
				this.btnSelectModule.Checked = true;
			} else {
				this.btnMove.Checked = false;
				this.btnConstruction.Checked = false;
				this.btnAddModules.Checked = false;
				this.btnSelectModule.Checked = false;
			}
			this.grpSelectedModules.Visible = this.btnSelectModule.Checked;
			this.grpSelection.Visible = this.btnSelectModule.Checked;
			this.grpNewModules.Visible = this.btnAddModules.Checked;
			this.grpAutomatic.Visible = this.btnAddModules.Checked;
			if ((this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.modulKlimaBodenPlanner.Mode == ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_LAYOUT_ADD_AREA) {
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
			return this.modulKlimaBodenPlanner.HighlightRow;
		}

		private void rbGlatt_CheckedChanged(object sender, EventArgs e) {
			if (rbGlatt.Checked && this.modulKlimaBodenPlanner.Product.GraphConstruction != this.glatt) {
				this.modulKlimaBodenPlanner.Product.GraphConstruction = this.glatt;
				this.changed = true;
				this.UpdateControls();
				this.planPanel.InvalidateGraphics();
			}
		}

		private void rbAkustik_CheckedChanged(object sender, EventArgs e) {
			if (rbAkustik.Checked && this.modulKlimaBodenPlanner.Product.GraphConstruction != this.akustik) {
				this.modulKlimaBodenPlanner.Product.GraphConstruction = this.akustik;
				this.changed = true;
				this.UpdateControls();
				this.planPanel.InvalidateGraphics();
			}
		}

		private void rbKassetten_CheckedChanged(object sender, EventArgs e) {
			if (rbKassetten.Checked && this.modulKlimaBodenPlanner.Product.GraphConstruction != this.kassette) {
				this.modulKlimaBodenPlanner.Product.GraphConstruction = this.kassette;
				this.changed = true;
				this.UpdateControls();
				this.planPanel.InvalidateGraphics();
			}
		}

		private void numAusrichtung_ValueChanged(object sender, EventArgs e) {
			if (ignoreRotation == 0) {
				this.changed = true;
				if (this.modulKlimaBodenPlanner.Product.GraphConstruction is ModulKlimaDeckeConstructionAkustik) {
					ModulKlimaDeckeConstructionAkustik akustik = this.modulKlimaBodenPlanner.Product.GraphConstruction as ModulKlimaDeckeConstructionAkustik;
					akustik.RotationRelativeToPlan = (double)this.numRotation.Value;
					this.planPanel.InvalidateGraphics();
				} else if (this.modulKlimaBodenPlanner.Product.GraphConstruction is ModulKlimaDeckeConstructionGlatt) {
					ModulKlimaDeckeConstructionGlatt glatt = this.modulKlimaBodenPlanner.Product.GraphConstruction as ModulKlimaDeckeConstructionGlatt;
					glatt.RotationRelativeToPlan = (double)this.numRotation.Value;
					this.planPanel.InvalidateGraphics();
				} else if (this.modulKlimaBodenPlanner.Product.GraphConstruction is ModulKlimaDeckeConstructionKassette) {
					ModulKlimaDeckeConstructionKassette kassette = this.modulKlimaBodenPlanner.Product.GraphConstruction as ModulKlimaDeckeConstructionKassette;
					kassette.RotationRelativeToPlan = (double)this.numRotation.Value;
					this.planPanel.InvalidateGraphics();
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
				value += 180;
			}
			while (value >= 180) {
				value -= 180;
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
				if (this.modulKlimaBodenPlanner.Product.ContainsModules) {
					if (MessageBox.Show("Wenn Sie die Konstruktion ändern wollen, werden alle bereits verplanten Module gelöscht!", "Bestätigen", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK) {
						e.Cancel = true;
					} else {
						this.modulKlimaBodenPlanner.Product.PlannedCircuits.Clear();
						this.modulKlimaBodenPlanner.Product.PlannedCircuits.Add(new ModulDeckeCircuit());
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
				if (!this.btnAddModules.Checked && !this.btnSelectModule.Checked && !this.btnMove.Checked) {
					this.planPanel.Mode = PlanMode.PM_MOVE;
					this.modulKlimaBodenPlanner.Mode = ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_NONE;
					this.UpdateButtons();
				}
			} else if (tabPage == this.pageConstruction) {
				this.btnAddModules.Visible = false;
				this.btnSelectModule.Visible = false;
				this.btnConstruction.Visible = true;
				if (!this.btnConstruction.Checked && !this.btnMove.Checked) {
					this.planPanel.Mode = PlanMode.PM_MOVE;
					this.modulKlimaBodenPlanner.Mode = ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_NONE;
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
			List<Circuit> circuits = (this.modulKlimaBodenPlanner.Product.ContainsModules ? this.modulKlimaBodenPlanner.Product.PlannedCircuits : new List<Circuit>());

			int dec = this.newVisible ? 1 : 0;
			this.newVisible = this.modulKlimaBodenPlanner.Mode == ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_LAYOUT_ADD_AREA;
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
					this.modulKlimaBodenPlanner.HighlightRow = (this.lstRows.Items.Count - dec > this.lstRows.SelectedIndex ? (this.modulKlimaBodenPlanner.Product.PlannedCircuits[this.lstCircuits.SelectedIndex] as ModulDeckeCircuit).SubAreas[this.lstSubarea.SelectedIndex].Rows[this.lstRows.SelectedIndex] : null);
					/*List<KlimaFlaechenModul> modules = new List<KlimaFlaechenModul>();
					if (this.modulKlimaBodenPlanner.HighlightRow != null) {
						foreach (KlimaFlaechenModul modul in this.modulKlimaBodenPlanner.HighlightRow.List) {
							modules.Add(modul);
						}
					}*/
					this.UpdateSelectedModules();
				} else if (this.lstSubarea.SelectedIndex >= 0 && this.lstSubarea.SelectedIndex < this.lstSubarea.Items.Count - dec) {
					this.modulKlimaBodenPlanner.HighlightSubArea = (this.lstSubarea.Items.Count - dec > this.lstSubarea.SelectedIndex ? (this.modulKlimaBodenPlanner.Product.PlannedCircuits[this.lstCircuits.SelectedIndex] as ModulDeckeCircuit).SubAreas[this.lstSubarea.SelectedIndex] : null);
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
					this.modulKlimaBodenPlanner.HighlightCircuit = (this.lstCircuits.Items.Count - dec > this.lstCircuits.SelectedIndex ? (this.modulKlimaBodenPlanner.Product.PlannedCircuits[this.lstCircuits.SelectedIndex] as ModulDeckeCircuit) : null);
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
					this.modulKlimaBodenPlanner.HighlightCircuit = null;
					this.UpdateSelectedModules();
				}

				numLength.Enabled = GetSelectedRow() != null && GetSelectedRow().List.Count > 0;

				if (GetSelectedRow() != null) {
					this.numLength.Value = (decimal)GetSelectedRow().LengthVerbindeleitungen;
				}

				this.ignoreListChange--;
			}
		}

		private void cmbModulType_SelectedIndexChanged(object sender, EventArgs e) {
			if (this.cmbModulType.SelectedItem is KlimaFlaechenModul.ModulTypeEnum) {
				this.modulKlimaBodenPlanner.ModuleTypeToAdd = (KlimaFlaechenModul.ModulTypeEnum)this.cmbModulType.SelectedItem;
			}
			if (this.cmbOrientation.SelectedItem is KlimaFlaechenModul.ModulOrientationEnum) {
				this.modulKlimaBodenPlanner.StartingOrientation = (KlimaFlaechenModul.ModulOrientationEnum)this.cmbOrientation.SelectedItem;
			}
		}

		private void modulKlimaBodenPlanner_ModuleSelected(object sender, ModulKlimaDeckePlanner.ModuleSelectedEventArgs e) {
			this.UpdateSelectedModules();
		}

		private void UpdateSelectedModules() {
			this.ignoreModuleOrientationChange++;
			this.ignoreModuleTypeChange++;

			if ((this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.modulKlimaBodenPlanner.Mode == ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_PICK_MODULE) {
				if (this.modulKlimaBodenPlanner.HighlightCircuit == null && this.modulKlimaBodenPlanner.HighlightSubArea == null && this.modulKlimaBodenPlanner.HighlightRow == null) {
					this.lstCircuits.SelectedIndex = -1;
				}
			}
			List<KlimaFlaechenModul> module = this.modulKlimaBodenPlanner.GetAllSelectedModules();
			
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
					ModulDeckeCircuit curCircuit = this.modulKlimaBodenPlanner.Product.GetCircuitForModul(modul, out circuitIndex);
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
			if (ignoreModuleTypeChange == 0) {
				this.changed = true;
				if (this.modulKlimaBodenPlanner.Product == null || this.modulKlimaBodenPlanner.Product.AssociatedRoom == null ||
					this.modulKlimaBodenPlanner.Product.AssociatedRoom.AssociatedPlan == null || this.modulKlimaBodenPlanner.Product.AssociatedRoom.AssociatedPlan.Measure == null) {
					return;
				}
				List<KlimaFlaechenModul> modules = this.modulKlimaBodenPlanner.GetAllSelectedModules();
				bool allChanged = true;
				bool nonChanged = true;
				if (this.cmbSelectedModuleType.SelectedItem is KlimaFlaechenModul.ModulTypeEnum) {
					foreach (KlimaFlaechenModul modul in modules) {
						PossibleModulLane lane = this.modulKlimaBodenPlanner.Product.GraphConstruction.PossibleLanes[modul.GraphLane];
						if (lane.ModuleChangesPossible(modul, (KlimaFlaechenModul.ModulTypeEnum)this.cmbSelectedModuleType.SelectedItem, modul.GraphPositionInLan, this.modulKlimaBodenPlanner.Product.AssociatedRoom.AssociatedPlan.Measure.Value, this.modulKlimaBodenPlanner.Product)) {
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

		private void cmbSelectedModuleOrientation_SelectedIndexChanged(object sender, EventArgs e) {
			if (ignoreModuleOrientationChange == 0) {
				this.changed = true;
				List<KlimaFlaechenModul> modules = this.modulKlimaBodenPlanner.GetAllSelectedModules();
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

		private void cbAutomaticOrientation_CheckedChanged(object sender, EventArgs e) {
			this.modulKlimaBodenPlanner.AutomaticOrientation = this.cbAutomaticOrientation.Checked;
		}

		private void cbAutomaticRows_CheckedChanged(object sender, EventArgs e) {
			this.modulKlimaBodenPlanner.AutomaticRows = this.cbAutomaticRows.Checked;
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
			if (ignoreListChange == 0 && GetSelectedRow() != null) {
				this.changed = true;
				GetSelectedRow().LengthVerbindeleitungen = (double)this.numLength.Value;
				CalculateAndUpdate();
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
			if (ignoreRandfries == 0) {
				this.changed = true;
				this.akustik.Randfries = ((double)numRandfries.Value) / 100.0;
				this.planPanel.InvalidateGraphics();
			}
		}
	}
}