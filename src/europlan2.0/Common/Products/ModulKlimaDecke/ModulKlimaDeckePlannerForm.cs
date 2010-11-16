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

		private int ignoreRotation = 0;

		public ModulKlimaDeckePlannerForm(ModulKlimaDeckeProduct product) {
			InitializeComponent();
			this.modulKlimaBodenPlanner.Product = product;
			if (product.GraphConstruction == null) {
				product.GraphConstruction = new ModulKlimaDeckeConstructionGlatt();
				product.GraphConstruction.Planner = this.modulKlimaBodenPlanner;
			}
			if (product.GraphConstruction == null) {
				product.GraphConstruction = new ModulKlimaDeckeConstructionGlatt();
			}
			this.glatt = product.GraphConstruction as ModulKlimaDeckeConstructionGlatt;
			this.akustik = product.GraphConstruction as ModulKlimaDeckeConstructionAkustik;
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
			} else {
				this.akustik.Planner = this.modulKlimaBodenPlanner;
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

			this.UpdateLists(true, true, true);

			if (product.ContainsModules) {
				this.tabs.SelectedTab = this.pageLayout;
			} else {
				this.tabs.SelectedTab = this.pageConstruction;
			}
			this.UpdateToolbar(this.tabs.SelectedTab);
			this.UpdateButtons();
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
			this.grpNewModules.Visible = this.btnAddModules.Checked;
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

		private TabPage previousTab = null;

		private void tabs_Selecting(object sender, TabControlCancelEventArgs e) {
			if (previousTab == this.pageLayout && e.TabPage == this.pageConstruction) {
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

		private void modulKlimaBodenPlanner_ListsNeedUpdate(object sender, EventArgs e) {
			this.UpdateLists(true, true, true);
		}

		private void UpdateLists(bool updateCircuits, bool updateSubareas, bool updateRows) {
			List<Circuit> circuits = (this.modulKlimaBodenPlanner.Product.ContainsModules ? this.modulKlimaBodenPlanner.Product.PlannedCircuits : new List<Circuit>());

			ignoreListChange++;
			if (updateCircuits) {
				int circuitsCount = circuits.Count;
				int tmp = (lstCircuits.SelectedIndex == lstCircuits.Items.Count - 1 ? circuitsCount : lstCircuits.SelectedIndex);
				lstCircuits.BeginUpdate();
				lstCircuits.Items.Clear();
				for (int i = 1; i <= circuitsCount; i++) {
					lstCircuits.Items.Add(EuroplanRes.PlannedModulKlimaDeckeProductPanel_HeizkreisAbkuerzung + i.ToString());
				}
				lstCircuits.Items.Add("neuer HK");
				lstCircuits.SelectedIndex = tmp;
				lstCircuits.EndUpdate();
			}

			if (updateSubareas) {
				int subAreasCount = (this.lstCircuits.SelectedIndex < this.lstCircuits.Items.Count - 1 && this.lstCircuits.SelectedIndex >= 0 ? (circuits[this.lstCircuits.SelectedIndex] as ModulDeckeCircuit).SubAreas.Count : 0);
				int tmp = (lstSubarea.SelectedIndex == lstSubarea.Items.Count - 1 ? subAreasCount : lstSubarea.SelectedIndex);
				lstSubarea.BeginUpdate();
				lstSubarea.Items.Clear();
				for (int i = 1; i <= subAreasCount; i++) {
					lstSubarea.Items.Add(EuroplanRes.PlannedModulKlimaDeckeProductPanel_Teilflaeche + i.ToString());
				}
				lstSubarea.Items.Add("neue Teilfläche");
				lstSubarea.SelectedIndex = tmp;
				lstSubarea.EndUpdate();
			}

			if (updateRows) {
				int rowsCount = (this.lstSubarea.SelectedIndex < this.lstSubarea.Items.Count - 1 && this.lstSubarea.SelectedIndex >= 0 ? (circuits[this.lstCircuits.SelectedIndex] as ModulDeckeCircuit).SubAreas[this.lstSubarea.SelectedIndex].Rows.Count : 0);
				int tmp = (lstRows.SelectedIndex == lstRows.Items.Count - 1 ? rowsCount : lstRows.SelectedIndex);
				lstRows.BeginUpdate();
				lstRows.Items.Clear();
				for (int i = 1; i <= rowsCount; i++) {
					lstRows.Items.Add(EuroplanRes.PlannedModulKlimaDeckeProductPanel_Reihe + i.ToString());
				}
				lstRows.Items.Add("neue Reihe");
				lstRows.SelectedIndex = tmp;
				lstRows.EndUpdate();
			}
			ignoreListChange--;
		}

		private int ignoreListChange = 0;
		private void lstCircuits_SelectedIndexChanged(object sender, EventArgs e) {
			if (this.ignoreListChange == 0) {
				this.ignoreListChange++;
				if (this.lstCircuits.SelectedIndex >= 0 && this.lstCircuits.SelectedIndex < this.lstCircuits.Items.Count - 1) {
					if (this.lstSubarea.SelectedIndex < 0 || sender == this.lstCircuits) {
						this.lstSubarea.SelectedIndex = this.lstSubarea.Items.Count - 1;
					}
				} else {
					this.lstSubarea.SelectedIndex = -1;
				}

				if (this.lstSubarea.SelectedIndex >= 0 && this.lstSubarea.SelectedIndex < this.lstSubarea.Items.Count - 1) {
					if (this.lstRows.SelectedIndex < 0 || sender == this.lstSubarea) {
						this.lstRows.SelectedIndex = this.lstRows.Items.Count - 1;
					}
				} else {
					this.lstRows.SelectedIndex = -1;
				}

				if (sender != lstRows) {
					this.UpdateLists(false, sender == lstCircuits, true);
				}

				if (this.lstRows.SelectedIndex >= 0 && this.lstRows.SelectedIndex < this.lstRows.Items.Count - 1) {
					this.modulKlimaBodenPlanner.HighlightRow = (this.lstRows.Items.Count - 1 > this.lstRows.SelectedIndex ? (this.modulKlimaBodenPlanner.Product.PlannedCircuits[this.lstCircuits.SelectedIndex] as ModulDeckeCircuit).SubAreas[this.lstSubarea.SelectedIndex].Rows[this.lstRows.SelectedIndex] : null);
					List<KlimaFlaechenModul> modules = new List<KlimaFlaechenModul>();
					if (this.modulKlimaBodenPlanner.HighlightRow != null) {
						foreach (KlimaFlaechenModul modul in this.modulKlimaBodenPlanner.HighlightRow.List) {
							modules.Add(modul);
						}
					}
					this.UpdateSelectedModuleTypeAndOrientation(modules);
				} else if (this.lstSubarea.SelectedIndex >= 0 && this.lstSubarea.SelectedIndex < this.lstSubarea.Items.Count - 1) {
					this.modulKlimaBodenPlanner.HighlightSubArea = (this.lstSubarea.Items.Count - 1 > this.lstSubarea.SelectedIndex ? (this.modulKlimaBodenPlanner.Product.PlannedCircuits[this.lstCircuits.SelectedIndex] as ModulDeckeCircuit).SubAreas[this.lstSubarea.SelectedIndex] : null);
					List<KlimaFlaechenModul> modules = new List<KlimaFlaechenModul>();
					if (this.modulKlimaBodenPlanner.HighlightSubArea != null) {
						foreach (KlimaFlaechenList row in this.modulKlimaBodenPlanner.HighlightSubArea.Rows) {
							foreach (KlimaFlaechenModul modul in row.List) {
								modules.Add(modul);
							}
						}
					}
					this.UpdateSelectedModuleTypeAndOrientation(modules);
				} else if (this.lstCircuits.SelectedIndex >= 0 && this.lstCircuits.SelectedIndex < this.lstCircuits.Items.Count - 1) {
					this.modulKlimaBodenPlanner.HighlightCircuit = (this.lstCircuits.Items.Count - 1 > this.lstCircuits.SelectedIndex ? (this.modulKlimaBodenPlanner.Product.PlannedCircuits[this.lstCircuits.SelectedIndex] as ModulDeckeCircuit) : null);
					List<KlimaFlaechenModul> modules = new List<KlimaFlaechenModul>();
					if (this.modulKlimaBodenPlanner.HighlightCircuit != null) {
						foreach (ModulDeckeSubArea subArea in this.modulKlimaBodenPlanner.HighlightCircuit.SubAreas) {
							foreach (KlimaFlaechenList row in subArea.Rows) {
								foreach (KlimaFlaechenModul modul in row.List) {
									modules.Add(modul);
								}
							}
						}
					}
					this.UpdateSelectedModuleTypeAndOrientation(modules);
				} else {
					this.modulKlimaBodenPlanner.HighlightCircuit = null;
					this.UpdateSelectedModuleTypeAndOrientation(null);
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
			if (e.modul != null) {
				/*this.cmbSelectedModuleType.SelectedItem = e.modul.ModulType;
				this.cmbSelectedModuleOrientation.SelectedItem = e.modul.Orientation;*/
				List<KlimaFlaechenModul> modules = new List<KlimaFlaechenModul>();
				modules.Add(e.modul);
				this.UpdateSelectedModuleTypeAndOrientation(modules);
			} else {
				this.UpdateSelectedModuleTypeAndOrientation(e.modules);
				/*Nullable<KlimaFlaechenModul.ModulTypeEnum> typ = null;
				bool typOk = true;
				Nullable<KlimaFlaechenModul.ModulOrientationEnum> orientation = null;
				bool orientationOk = true;
				foreach (KlimaFlaechenModul modul in e.modules) {
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
				}*/

			}
		}

		private void UpdateSelectedModuleTypeAndOrientation(List<KlimaFlaechenModul> modules) {
			this.ignoreModuleOrientationChange++;
			this.ignoreModuleTypeChange++;
			if (modules != null && modules.Count > 0) {
				Nullable<KlimaFlaechenModul.ModulTypeEnum> typ = null;
				bool typOk = true;
				Nullable<KlimaFlaechenModul.ModulOrientationEnum> orientation = null;
				bool orientationOk = true;
				foreach (KlimaFlaechenModul modul in modules) {
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
		}

		private int ignoreModuleTypeChange = 0;
		private int ignoreModuleOrientationChange = 0;

		private void cmbSelectedModuleType_SelectedIndexChanged(object sender, EventArgs e) {
			if (ignoreModuleTypeChange == 0) {
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
				this.UpdateSelectedModuleTypeAndOrientation(modules);
				this.planPanel.InvalidateGraphics();
				if (nonChanged) {
					MessageBox.Show("Es konnte kein Modul geändert werden, da nicht genug Platz zur Verfügugn steht", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
				} else if (!allChanged) {
					MessageBox.Show("Es konnten nicht alle Module geändert werden, da nicht genug Platz zur Verfügugn steht", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
		}

		private void cmbSelectedModuleOrientation_SelectedIndexChanged(object sender, EventArgs e) {
			if (ignoreModuleOrientationChange == 0) {
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
				this.UpdateSelectedModuleTypeAndOrientation(modules);
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
	}
}