using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;

namespace Europlan.Common.Products {
	public partial class ModulKlimaBodenPlannerForm : Form {

		private ModulKlimaBodenConstructionFrei frei = null;
		private ModulKlimaBodenConstructionStaffeln staffeln = null;

		private int ignoreRotation = 0;
		private bool newVisible = false;
		private PlannedProduct plannedProduct;

		private bool changed = false;

		public ModulKlimaBodenPlannerForm(PlannedProduct plannedProduct) {
			InitializeComponent();
			this.plannedProduct = plannedProduct;

			this.cmbOrientation.Items.Add(KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT);
			this.cmbOrientation.Items.Add(KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT);
			this.cmbOrientation.SelectedIndex = 0;

			this.cmbHorizontal.Items.Add("modulierend");
			this.cmbHorizontal.Items.Add("dicht");
			this.cmbHorizontal.SelectedIndex = 0;

			this.cmbVertical.Items.Add("modulierend");
			this.cmbVertical.Items.Add("dicht");
			this.cmbVertical.SelectedIndex = 0;

			this.cmbConnections.Items.Add("Längsseite");
			this.cmbConnections.Items.Add("Breitseite");
			this.cmbConnections.SelectedIndex = 0;

			ModulKlimaBodenProduct product = plannedProduct.Product as ModulKlimaBodenProduct;

			this.modulKlimaBodenPlanner.Product = product;
			if (product.GraphConstruction == null) {
				product.GraphConstruction = new ModulKlimaBodenConstructionFrei();
			}
			this.frei = product.GraphConstruction as ModulKlimaBodenConstructionFrei;
			this.staffeln = product.GraphConstruction as ModulKlimaBodenConstructionStaffeln;

			if (this.frei == null) {
				this.frei = new ModulKlimaBodenConstructionFrei();
				this.frei.Planner = this.modulKlimaBodenPlanner;
				this.frei.RotationRelativeToPlan = 0;
			} else {
				this.frei.Planner = this.modulKlimaBodenPlanner;
			}
			this.frei.RecalculateSchienen();

			if (this.staffeln == null) {
				this.staffeln = new ModulKlimaBodenConstructionStaffeln();
				this.staffeln.Planner = this.modulKlimaBodenPlanner;
				this.staffeln.RotationRelativeToPlan = 0;
			} else {
				this.staffeln.Planner = this.modulKlimaBodenPlanner;
			}
			this.staffeln.RecalculateSchienen();

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
				if (this.modulKlimaBodenPlanner.ContainsNotConfirmedModules) {
					DialogResult result = MessageBox.Show("Wollen Sie die neu hinzugefügten Module übernehmen?", "Module übernehmen", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
					if (result == DialogResult.Cancel) {
						return;
					} else if (result == DialogResult.Yes) {
						ModulBodenCircuit newCircuit = this.modulKlimaBodenPlanner.ConfirmNewModules();
						if (newCircuit != null) {
							this.UpdateLists(true, true);
						}
					}
				}
				this.modulKlimaBodenPlanner.Mode = ModulKlimaBodenPlanner.KlimaBodenMode.KDM_NONE;
				this.planPanel.Mode = PlanMode.PM_MOVE;
				this.UpdateButtons();
			}
		}

		private void btnAddModules_Click(object sender, EventArgs e) {
			if (!btnAddModules.Checked) {
				this.modulKlimaBodenPlanner.Mode = ModulKlimaBodenPlanner.KlimaBodenMode.KDM_LAYOUT_ADD_AREA;
				this.planPanel.Mode = PlanMode.PM_PLANNER_DRAG;
				this.UpdateButtons();
			}
		}

		private void btnSelectModule_Click(object sender, EventArgs e) {
			if (!btnSelectModule.Checked) {
				if (this.modulKlimaBodenPlanner.ContainsNotConfirmedModules) {
					DialogResult result = MessageBox.Show("Wollen Sie die neu hinzugefügten Module übernehmen?", "Module übernehmen", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
					if (result == DialogResult.Cancel) {
						return;
					} else if (result == DialogResult.Yes) {
						ModulBodenCircuit newCircuit = this.modulKlimaBodenPlanner.ConfirmNewModules();
						if (newCircuit != null) {
							this.UpdateLists(true, true);
						}
					}
				}
				this.modulKlimaBodenPlanner.Mode = ModulKlimaBodenPlanner.KlimaBodenMode.KDM_PICK_MODULE;
				this.planPanel.Mode = PlanMode.PM_PLANNER_DRAG;
				this.UpdateButtons();
			}
		}

		private void UpdateButtons() {
			if (this.planPanel.Mode == PlanMode.PM_MOVE) {
				this.btnMove.Checked = true;
				this.btnAddModules.Checked = false;
				this.btnSelectModule.Checked = false;
			} else if ((this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.modulKlimaBodenPlanner.Mode == ModulKlimaBodenPlanner.KlimaBodenMode.KDM_LAYOUT_ADD_AREA) {
				this.btnMove.Checked = false;
				this.btnAddModules.Checked = true;
				this.btnSelectModule.Checked = false;
			} else if ((this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.modulKlimaBodenPlanner.Mode == ModulKlimaBodenPlanner.KlimaBodenMode.KDM_PICK_MODULE) {
				this.btnMove.Checked = false;
				this.btnAddModules.Checked = false;
				this.btnSelectModule.Checked = true;
			} else {
				this.btnMove.Checked = false;
				this.btnAddModules.Checked = false;
				this.btnSelectModule.Checked = false;
			}
			this.grpSelectedModules.Visible = this.btnSelectModule.Checked;
			this.grpSelection.Visible = this.btnSelectModule.Checked;
			this.grpNewModules.Visible = this.btnAddModules.Checked;
			if ((this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.modulKlimaBodenPlanner.Mode == ModulKlimaBodenPlanner.KlimaBodenMode.KDM_LAYOUT_ADD_AREA) {
				if (!this.newVisible) {
					this.newVisible = true;
					this.ignoreListChange++;
					lstCircuits.Items.Add("neuer HK");
					this.ignoreListChange--;
				}
			} else {
				if (this.newVisible) {
					this.newVisible = false;
					this.ignoreListChange++;
					this.lstCircuits.Items.RemoveAt(this.lstCircuits.Items.Count - 1);
					this.ignoreListChange--;
				}
			}
			this.UpdateLists(true, false);
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
					if (MessageBox.Show("Wenn Sie die Konstruktion ändern wollen, werden alle bereits verplanten Module gelöscht!", "Bestätigen", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK) {
						e.Cancel = true;
					} else {
						this.modulKlimaBodenPlanner.Product.PlannedCircuits.Clear();
						this.modulKlimaBodenPlanner.Product.PlannedCircuits.Add(new ModulBodenCircuit());
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
				this.btnAddModules.Visible = true;
				this.btnSelectModule.Visible = true;
				this.btnConstruction.Visible = false;
				if (!this.btnAddModules.Checked && !this.btnSelectModule.Checked && !this.btnMove.Checked) {
					this.planPanel.Mode = PlanMode.PM_MOVE;
					this.modulKlimaBodenPlanner.Mode = ModulKlimaBodenPlanner.KlimaBodenMode.KDM_NONE;
					this.UpdateButtons();
				}
			} else if (tabPage == this.pageConstruction) {
				this.btnAddModules.Visible = false;
				this.btnSelectModule.Visible = false;
				this.btnConstruction.Visible = true;
				if (!this.btnConstruction.Checked && !this.btnMove.Checked) {
					this.planPanel.Mode = PlanMode.PM_MOVE;
					this.modulKlimaBodenPlanner.Mode = ModulKlimaBodenPlanner.KlimaBodenMode.KDM_NONE;
					this.UpdateButtons();
				}
			}
		}

		private void tabs_Deselected(object sender, TabControlEventArgs e) {
			this.previousTab = e.TabPage;
		}

		private void UpdateLists(bool updateCircuits, bool selectLastCircuit) {
			List<Circuit> circuits = (this.modulKlimaBodenPlanner.Product.ContainsModules ? this.modulKlimaBodenPlanner.Product.PlannedCircuits : new List<Circuit>());

			int dec = this.newVisible ? 1 : 0;
			this.newVisible = this.modulKlimaBodenPlanner.Mode == ModulKlimaBodenPlanner.KlimaBodenMode.KDM_LAYOUT_ADD_AREA ||
				this.modulKlimaBodenPlanner.Mode == ModulKlimaBodenPlanner.KlimaBodenMode.KDM_LAYOUT_ADD_AREA_FINISH;
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

			ignoreListChange--;
			//this.lstCircuits_SelectedIndexChanged(this.lstRows, EventArgs.Empty);
		}

		private int ignoreListChange = 0;
		private void lstCircuits_SelectedIndexChanged(object sender, EventArgs e) {
			if (this.ignoreListChange == 0) {
				this.ignoreListChange++;
				int dec = this.newVisible ? 1 : 0;
				this.UpdateSelectedModules();

				if (this.lstCircuits.SelectedIndex >= 0 && this.lstCircuits.SelectedIndex < this.lstCircuits.Items.Count - dec) {
					ModulBodenCircuit circuit = (this.lstCircuits.Items.Count - dec > this.lstCircuits.SelectedIndex ? (this.modulKlimaBodenPlanner.Product.PlannedCircuits[this.lstCircuits.SelectedIndex] as ModulBodenCircuit) : null);
					this.modulKlimaBodenPlanner.HighlightCircuit = circuit;
					if (circuit != null) {
						btnColor.BackColor = circuit.CircuitColor;
						btnColor.Enabled = true;
					} else {
						btnColor.BackColor = Color.Transparent;
						btnColor.Enabled = false;
					}
				} else {
					this.modulKlimaBodenPlanner.HighlightCircuit = null;
					btnColor.BackColor = Color.Transparent;
					btnColor.Enabled = false;
				}

				this.ignoreListChange--;
			}
		}

		private void cmbModulType_SelectedIndexChanged(object sender, EventArgs e) {
			if (cmbOrientation.SelectedItem is KlimaFlaechenModul.ModulOrientationEnum) {
				this.modulKlimaBodenPlanner.NewModulesStartingOrientation = (KlimaFlaechenModul.ModulOrientationEnum)cmbOrientation.SelectedItem;
			}
		}

		private void UpdateSelectedModules() {
			this.ignoreModuleOrientationChange++;

			if ((this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.modulKlimaBodenPlanner.Mode == ModulKlimaBodenPlanner.KlimaBodenMode.KDM_PICK_MODULE) {
				if (this.modulKlimaBodenPlanner.HighlightCircuit == null) {
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
						this.lblOrientationError.Text = "Verschiedene Ausrichtungen ausgewählt";
						if (this.cmbSelectedModuleOrientation.Items.Count == 2) {
							this.cmbSelectedModuleOrientation.Items.Add("Alle umdrehen");
						}
					}
					this.cmbSelectedModuleOrientation.Enabled = true;
				} else {
					this.lblTypError.Text = "Kein Modul ausgewählt";
					this.cmbSelectedModuleOrientation.Enabled = false;
				}
			} else {
				this.lblTypError.Text = "Kein Modul ausgewählt";
				this.cmbSelectedModuleOrientation.SelectedIndex = -1;
				this.cmbSelectedModuleOrientation.Enabled = false;
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

		private void cmbSelectedModuleOrientation_SelectedIndexChanged(object sender, EventArgs e) {
			if (ignoreModuleOrientationChange == 0) {
				this.changed = true;
			}
		}

		private void ModulKlimaBodenPlannerForm_Load(object sender, EventArgs e) {
			//SettingsKey settings = SettingsFile.Settings["ModulKlimaBodenPlannerForm"];
			//this.Location = settings.GetPoint("Location", this.Location);
			//this.Size = settings.GetSize("Size", this.Size);
		}

		private void ModulKlimaBodenPlannerForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ModulKlimaBodenPlannerForm"];
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
			notifications = ModulKlimaBodenProduct.GlobalNotificationMessage;
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
				ModulBodenCircuit circuit = (this.lstCircuits.Items.Count - dec > this.lstCircuits.SelectedIndex ? (this.modulKlimaBodenPlanner.Product.PlannedCircuits[this.lstCircuits.SelectedIndex] as ModulBodenCircuit) : null);
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
		}

		private void btnInvertDirection_Click(object sender, EventArgs e) {
		}

		private void btnConstruction_Click(object sender, EventArgs e) {
			if (!btnConstruction.Checked) {
				this.modulKlimaBodenPlanner.Mode = ModulKlimaBodenPlanner.KlimaBodenMode.KDM_CONSTRUCTION;
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
			ModulBodenCircuit newCircuit = this.modulKlimaBodenPlanner.ConfirmNewModules();
			if (newCircuit != null) {
				this.UpdateLists(true, true);
			}
		}

		private void cmbHorizontal_SelectedIndexChanged(object sender, EventArgs e) {
			this.modulKlimaBodenPlanner.NewModulesXDicht = cmbHorizontal.SelectedIndex == 1;
		}

		private void cmbVertical_SelectedIndexChanged(object sender, EventArgs e) {
			this.modulKlimaBodenPlanner.NewModulesYDicht = cmbVertical.SelectedIndex == 1;
		}

		private void cmbConnections_SelectedIndexChanged(object sender, EventArgs e) {
			this.modulKlimaBodenPlanner.NewModulesConnectHorizontal = cmbConnections.SelectedIndex == 1;
		}

		private void chkSelectReferenceModule_CheckedChanged(object sender, EventArgs e) {
			if (this.chkSelectReferenceModule.Checked && this.modulKlimaBodenPlanner.Mode == ModulKlimaBodenPlanner.KlimaBodenMode.KDM_LAYOUT_ADD_AREA_FINISH) {
				this.planPanel.Mode = PlanMode.PM_PLANNER_CLICK;
				this.modulKlimaBodenPlanner.Mode = ModulKlimaBodenPlanner.KlimaBodenMode.KDM_LAYOUT_ADD_AREA_PICK_REFERENCE;
			} else if (!this.chkSelectReferenceModule.Checked && this.modulKlimaBodenPlanner.Mode == ModulKlimaBodenPlanner.KlimaBodenMode.KDM_LAYOUT_ADD_AREA_PICK_REFERENCE) {
				this.planPanel.Mode = PlanMode.PM_PLANNER_DRAG;
				this.modulKlimaBodenPlanner.Mode = ModulKlimaBodenPlanner.KlimaBodenMode.KDM_LAYOUT_ADD_AREA_FINISH;
			}
		}

		private void modulKlimaBodenPlanner_ModeChanged(object sender, EventArgs e) {
			if (this.modulKlimaBodenPlanner.Mode == ModulKlimaBodenPlanner.KlimaBodenMode.KDM_LAYOUT_ADD_AREA_FINISH && this.planPanel.Mode != PlanMode.PM_PLANNER_DRAG) {
				this.planPanel.Mode = PlanMode.PM_PLANNER_DRAG;
			}
		}

		private void modulKlimaBodenPlanner_ModuleSelected(object sender, EventArgs e) {

		}
	}
}