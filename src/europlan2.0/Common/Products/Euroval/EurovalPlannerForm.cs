using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;

namespace Europlan.Common.Products {
	public partial class EurovalPlannerForm : Form {

		private bool newVisible = false;
		private PlannedProduct plannedProduct;

		private bool changed = false;

		private bool cmbLayDistanceContainsAutomatic = true;
		private bool cmbRimTypeContainsAutomatic = true;
		private bool cmbRimTypeContainsNone = false;
		private bool cmbCircuitsContainsAutomatic = true;

		public EurovalPlannerForm(PlannedProduct plannedProduct) {
			InitializeComponent();
			this.plannedProduct = plannedProduct;

			EurovalProduct product = plannedProduct.Product as EurovalProduct;

			this.eurovalPlanner.Product = product;
			this.SetLanguage();
			this.UpdateToolbar(this.tabs.SelectedTab);
			this.CalculateAndUpdate();
		}

		private void SetLanguage() {
			this.cmbLayDistance.Items.Clear();
			this.cmbRimType.Items.Clear();
			this.cmbCircuits.Items.Clear();

			this.cmbLayDistance.Items.Add(new Europlan.Common.PlannedEurovalProductPanel.LayDistanceItem(null, EuroplanRes.EurovalProduct_Automatisch/*"Automatisch"*/));
			this.cmbLayDistance.Items.Add(new Europlan.Common.PlannedEurovalProductPanel.LayDistanceItem(EurovalProduct.EurovalLayDistance.EV35, EuroplanRes.EurovalProduct_EV35/*"EV35"*/));
			this.cmbLayDistance.Items.Add(new Europlan.Common.PlannedEurovalProductPanel.LayDistanceItem(EurovalProduct.EurovalLayDistance.EV30, EuroplanRes.EurovalProduct_EV30/*"EV30"*/));
			this.cmbLayDistance.Items.Add(new Europlan.Common.PlannedEurovalProductPanel.LayDistanceItem(EurovalProduct.EurovalLayDistance.EV25, EuroplanRes.EurovalProduct_EV25/*"EV25"*/));
			this.cmbLayDistance.Items.Add(new Europlan.Common.PlannedEurovalProductPanel.LayDistanceItem(EurovalProduct.EurovalLayDistance.EV20, EuroplanRes.EurovalProduct_EV20/*"EV20"*/));
			this.cmbLayDistance.Items.Add(new Europlan.Common.PlannedEurovalProductPanel.LayDistanceItem(EurovalProduct.EurovalLayDistance.EV15, EuroplanRes.EurovalProduct_EV15/*"EV15"*/));
			this.cmbLayDistance.Items.Add(new Europlan.Common.PlannedEurovalProductPanel.LayDistanceItem(EurovalProduct.EurovalLayDistance.EV10, EuroplanRes.EurovalProduct_EV10/*"EV10"*/));
			this.cmbLayDistance.Items.Add(new Europlan.Common.PlannedEurovalProductPanel.LayDistanceItem(EurovalProduct.EurovalLayDistance.EV5, EuroplanRes.EurovalProduct_EV5/*"EV5"*/));

			this.cmbRimType.Items.Add(new Europlan.Common.PlannedEurovalProductPanel.RimTypeItem(null, EuroplanRes.EurovalProduct_Automatisch/*"Automatisch"*/));
			this.cmbRimType.Items.Add(new Europlan.Common.PlannedEurovalProductPanel.RimTypeItem(EurovalProduct.EurovalRimType.EV15_60, EuroplanRes.EurovalProduct_EV15_60/*"EV15/60"*/));
			this.cmbRimType.Items.Add(new Europlan.Common.PlannedEurovalProductPanel.RimTypeItem(EurovalProduct.EurovalRimType.EV15_120, EuroplanRes.EurovalProduct_EV15_120/*"EV15/120"*/));
			this.cmbRimType.Items.Add(new Europlan.Common.PlannedEurovalProductPanel.RimTypeItem(EurovalProduct.EurovalRimType.EV15_180, EuroplanRes.EurovalProduct_EV15_180/*"EV15/180"*/));
			this.cmbRimType.Items.Add(new Europlan.Common.PlannedEurovalProductPanel.RimTypeItem(EurovalProduct.EurovalRimType.EV10_55, EuroplanRes.EurovalProduct_EV10_55/*"EV10/55"*/));
			this.cmbRimType.Items.Add(new Europlan.Common.PlannedEurovalProductPanel.RimTypeItem(EurovalProduct.EurovalRimType.EV10_110, EuroplanRes.EurovalProduct_EV10_110/*"EV10/110"*/));
			this.cmbRimType.Items.Add(new Europlan.Common.PlannedEurovalProductPanel.RimTypeItem(EurovalProduct.EurovalRimType.EV10_165, EuroplanRes.EurovalProduct_EV10_165/*"EV10/165"*/));
			this.cmbRimType.Items.Add(new Europlan.Common.PlannedEurovalProductPanel.RimTypeItem(EurovalProduct.EurovalRimType.EV5_40, EuroplanRes.EurovalProduct_EV5_40/*"EV5/40"*/));
			this.cmbRimType.Items.Add(new Europlan.Common.PlannedEurovalProductPanel.RimTypeItem(EurovalProduct.EurovalRimType.EV5_80, EuroplanRes.EurovalProduct_EV5_80/*"EV5/80"*/));
			this.cmbRimType.Items.Add(new Europlan.Common.PlannedEurovalProductPanel.RimTypeItem(EurovalProduct.EurovalRimType.EV5_120, EuroplanRes.EurovalProduct_EV5_120/*"EV5/120"*/));

			this.cmbCircuits.Items.Add(EuroplanRes.EurovalProduct_Automatisch/*"Automatisch"*/);
			for (int i = 1; i <= 12; i++) {
				this.cmbCircuits.Items.Add(i.ToString());
			}
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
				//if (this.eurovalPlanner.ContainsNotConfirmedModules) {
				//    DialogResult result = MessageBox.Show("Wollen Sie die neu hinzugefügten Module übernehmen?", "Module übernehmen", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
				//    if (result == DialogResult.Cancel) {
				//        return;
				//    } else if (result == DialogResult.Yes) {
				//        ModulBodenCircuit newCircuit = this.eurovalPlanner.ConfirmNewModules();
				//        if (newCircuit != null) {
				//            this.UpdateLists(true, true);
				//        }
				//    }
				//}
				this.eurovalPlanner.Mode = EurovalPlanner.EurovalMode.EVM_NONE;
				this.planPanel.Mode = PlanMode.PM_MOVE;
				this.UpdateButtons();
			}
		}

		private void btnAddRz_Click(object sender, EventArgs e) {
			if (!btnAddRz.Checked) {
				this.eurovalPlanner.Mode = EurovalPlanner.EurovalMode.EVM_ADD_RZ;
				this.planPanel.Mode = PlanMode.PM_PLANNER_CLICK;
				this.UpdateButtons();
			}
		}

		private void btnDelRz_Click(object sender, EventArgs e) {
			if (!btnDelRz.Checked) {
				this.eurovalPlanner.Mode = EurovalPlanner.EurovalMode.EVM_DEL_RZ;
				this.planPanel.Mode = PlanMode.PM_PLANNER_CLICK;
				this.UpdateButtons();
			}
		}

		//private void btnAddModules_Click(object sender, EventArgs e) {
		//    if (!btnAddModules.Checked) {
		//        this.eurovalPlanner.Mode = EurovalPlanner.KlimaBodenMode.KDM_LAYOUT_ADD_AREA;
		//        this.planPanel.Mode = PlanMode.PM_PLANNER_DRAG;
		//        this.UpdateButtons();
		//    }
		//}

		//private void btnSelectModule_Click(object sender, EventArgs e) {
		//    if (!btnSelectModule.Checked) {
		//        if (this.eurovalPlanner.ContainsNotConfirmedModules) {
		//            DialogResult result = MessageBox.Show("Wollen Sie die neu hinzugefügten Module übernehmen?", "Module übernehmen", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
		//            if (result == DialogResult.Cancel) {
		//                return;
		//            } else if (result == DialogResult.Yes) {
		//                ModulBodenCircuit newCircuit = this.eurovalPlanner.ConfirmNewModules();
		//                if (newCircuit != null) {
		//                    this.UpdateLists(true, true);
		//                }
		//            }
		//        }
		//        this.eurovalPlanner.Mode = EurovalPlanner.KlimaBodenMode.KDM_PICK_MODULE;
		//        this.planPanel.Mode = PlanMode.PM_PLANNER_DRAG;
		//        this.UpdateButtons();
		//    }
		//}

		private void UpdateButtons() {
			if (this.planPanel.Mode == PlanMode.PM_MOVE) {
				this.btnMove.Checked = true;
				this.btnAddRz.Checked = false;
				this.btnDelRz.Checked = false;
				//this.btnAddModules.Checked = false;
				//this.btnSelectModule.Checked = false;
			} else if ((this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.eurovalPlanner.Mode == EurovalPlanner.EurovalMode.EVM_ADD_RZ) {
				this.btnMove.Checked = false;
				this.btnAddRz.Checked = true;
				this.btnDelRz.Checked = false;
				//this.btnAddModules.Checked = true;
				//this.btnSelectModule.Checked = false;
			} else if ((this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.eurovalPlanner.Mode == EurovalPlanner.EurovalMode.EVM_DEL_RZ) {
				this.btnMove.Checked = false;
				this.btnAddRz.Checked = false;
				this.btnDelRz.Checked = true;
				//this.btnAddModules.Checked = true;
				//this.btnSelectModule.Checked = false;
			} else {
				this.btnMove.Checked = false;
				this.btnAddRz.Checked = false;
				this.btnDelRz.Checked = false;
				//this.btnAddModules.Checked = false;
				//this.btnSelectModule.Checked = false;
			}
			//this.grpSelectedModules.Visible = this.btnSelectModule.Checked;
			//this.grpSelection.Visible = this.btnSelectModule.Checked;
			//this.grpNewModules.Visible = this.btnAddModules.Checked;
			//if ((this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.eurovalPlanner.Mode == EurovalPlanner.EurovalMode.KDM_LAYOUT_ADD_AREA) {
			//    if (!this.newVisible) {
			//        this.newVisible = true;
			//        //this.ignoreListChange++;
			//        //lstCircuits.Items.Add("neuer HK");
			//        //this.ignoreListChange--;
			//    }
			//} else {
			//    if (this.newVisible) {
			//        this.newVisible = false;
			//        //this.ignoreListChange++;
			//        //this.lstCircuits.Items.RemoveAt(this.lstCircuits.Items.Count - 1);
			//        //this.ignoreListChange--;
			//    }
			//}
			//this.UpdateLists(true, false);
		}

		private TabPage previousTab = null;

		private void tabs_Selecting(object sender, TabControlCancelEventArgs e) {
			//if ((previousTab == this.pageLayout || previousTab == this.pageCalculations) && e.TabPage == this.pageConstruction) {
			//    if (this.eurovalPlanner.Product.ContainsModules || this.eurovalPlanner.ContainsNotConfirmedModules) {
			//        if (MessageBox.Show("Wenn Sie die Konstruktion ändern wollen, werden alle bereits verplanten Module gelöscht!", "Bestätigen", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK) {
			//            e.Cancel = true;
			//        } else {
			//            this.eurovalPlanner.HighlightCircuit = null;
			//            this.eurovalPlanner.Product.PlannedCircuits.Clear();
			//            this.eurovalPlanner.Product.PlannedCircuits.Add(new ModulBodenCircuit());
			//        }
			//    }
			//}
			//if (!e.Cancel) {
			//    this.UpdateToolbar(e.TabPage);
			//    this.planPanel.InvalidateGraphics();
			//}
		}

		private void UpdateToolbar(TabPage tabPage) {
			//if (tabPage == this.pageLayout) {
			//    this.btnAddModules.Visible = true;
			//    this.btnSelectModule.Visible = true;
			//    this.btnConstruction.Visible = false;
			//    if (!this.btnAddModules.Checked && !this.btnSelectModule.Checked && !this.btnMove.Checked) {
			//        this.planPanel.Mode = PlanMode.PM_MOVE;
			//        this.eurovalPlanner.Mode = EurovalPlanner.KlimaBodenMode.KDM_NONE;
			//        this.UpdateButtons();
			//    }
			//} else if (tabPage == this.pageConstruction) {
			//    this.btnAddModules.Visible = false;
			//    this.btnSelectModule.Visible = false;
			//    this.btnConstruction.Visible = false;
			//    if (!this.btnConstruction.Checked && !this.btnMove.Checked) {
			//        this.planPanel.Mode = PlanMode.PM_MOVE;
			//        this.eurovalPlanner.Mode = EurovalPlanner.KlimaBodenMode.KDM_NONE;
			//        this.UpdateButtons();
			//    }
			//}
		}

		private void tabs_Deselected(object sender, TabControlEventArgs e) {
			this.previousTab = e.TabPage;
		}

		private void EurovalPlannerForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["EurovalPlannerForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
		}

		private void EurovalPlannerForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["EurovalPlannerForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			SettingsFile.Update();
		}

		private void CalculateAndUpdate() {
			EurovalProduct evProduct = this.plannedProduct.Product as EurovalProduct;
			evProduct.ConfigureProduct(this.plannedProduct.RequestedHeatLoad, this.plannedProduct.RequestedCoolLoad, this.plannedProduct.CalculateHeat, this.plannedProduct.CalculateCool, false);
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

			bool showHeat = this.plannedProduct.RequestedHeatLoad > 0 && evProduct.PlannedLayDistance != EurovalProduct.EurovalLayDistance.NONE;
			bool showCool = this.plannedProduct.RequestedCoolLoad > 0 && evProduct.PlannedLayDistance != EurovalProduct.EurovalLayDistance.NONE;
			bool showRim = evProduct.PlannedAreaRim > 0;
			bool showResidence = true;
			bool complete = evProduct.PlannedCalculationComplete;

			this.lblQSollHeat.Visible = showHeat;
			this.lblQkSollHeat.Visible = showHeat;
			this.lblQfbhHeat.Visible = showHeat;
			this.lblQRestHeat.Visible = showHeat;
			this.lblRimVaHeat.Visible = showHeat && showRim && complete;
			this.lblRimBHeat.Visible = showHeat && showRim && complete;
			this.lblRimTfbHeat.Visible = showHeat && showRim && complete;
			this.lblRimQHeat.Visible = showHeat && showRim && complete;
			this.lblResidenceVaHeat.Visible = showHeat && showResidence && complete;
			this.lblResidenceAHeat.Visible = showHeat && showResidence && complete;
			this.lblResidenceTfbHeat.Visible = showHeat && showResidence && complete;
			this.lblResidenceQHeat.Visible = showHeat && showResidence && complete;
			this.lblConnectionAHeat.Visible = showHeat;
			this.lblConnectionQHeat.Visible = showHeat;
			this.lblCircuitCountHeat.Visible = showHeat && complete;
			this.lblPipeLengthHeat.Visible = showHeat && complete;
			this.lblMhHeat.Visible = showHeat && complete;
			this.lblDeltaPHeat.Visible = showHeat && complete;
			this.lblSpreizungHeat.Visible = showHeat && complete;

			this.lblQSollCool.Visible = showCool;
			this.lblQkSollCool.Visible = showCool;
			this.lblQfbhCool.Visible = showCool;
			this.lblQRestCool.Visible = showCool;
			this.lblRimVaCool.Visible = showCool && showRim && complete;
			this.lblRimBCool.Visible = showCool && showRim && complete;
			this.lblRimTfbCool.Visible = showCool && showRim && complete;
			this.lblRimQCool.Visible = showCool && showRim && complete;
			this.lblResidenceVaCool.Visible = showCool && showResidence && complete;
			this.lblResidenceACool.Visible = showCool && showResidence && complete;
			this.lblResidenceTfbCool.Visible = showCool && showResidence && complete;
			this.lblResidenceQCool.Visible = showCool && showResidence && complete;
			this.lblConnectionACool.Visible = showCool;
			this.lblConnectionQCool.Visible = showCool;
			this.lblCircuitCountCool.Visible = showCool && complete;
			this.lblPipeLengthCool.Visible = showCool && complete;
			this.lblMhCool.Visible = showCool && complete;
			this.lblDeltaPCool.Visible = showCool && complete;
			this.lblSpreizungCool.Visible = showCool && complete;

			this.rbCalculateHeat.Enabled = this.plannedProduct.RequestedHeatLoad > 0;
			this.rbCalculateCool.Enabled = this.plannedProduct.RequestedCoolLoad > 0;
			this.rbCalculateBoth.Enabled = this.plannedProduct.RequestedHeatLoad > 0 && this.plannedProduct.RequestedCoolLoad > 0;
			this.cmbRimType.Enabled = evProduct.PlannedRimLength > 0;

			// disable the following controls if the product is a connection
			this.rbCalculateHeat.Enabled = this.rbCalculateHeat.Enabled && !evProduct.PlannedProductIsConnection;
			this.rbCalculateCool.Enabled = this.rbCalculateCool.Enabled && !evProduct.PlannedProductIsConnection;
			this.rbCalculateBoth.Enabled = this.rbCalculateBoth.Enabled && !evProduct.PlannedProductIsConnection;
			this.cmbLayDistance.Enabled = !evProduct.PlannedProductIsConnection;
			this.cmbRimType.Enabled = this.cmbRimType.Enabled && !evProduct.PlannedProductIsConnection;
			this.cmbCircuits.Enabled = !evProduct.PlannedProductIsConnection;

			bool newCmbCircuitsContainsAutomatic = !evProduct.ManualMode;
			bool newCmbLayDistanceContainsAutomatic = !evProduct.ManualMode;
			bool newCmbRimTypeContainsAutomatic = !evProduct.ManualMode && cmbRimType.Enabled;
			bool newCmbRimTypeContainsNone = !this.cmbRimType.Enabled;

			if (this.cmbCircuitsContainsAutomatic != newCmbCircuitsContainsAutomatic) {
				this.cmbCircuitsContainsAutomatic = newCmbCircuitsContainsAutomatic;
				if (this.cmbCircuitsContainsAutomatic) {
					this.cmbCircuits.Items.Insert(0, "Automatisch");
				} else {
					this.cmbCircuits.Items.RemoveAt(0);
				}
			}

			if (this.cmbLayDistanceContainsAutomatic != newCmbLayDistanceContainsAutomatic) {
				this.cmbLayDistanceContainsAutomatic = newCmbLayDistanceContainsAutomatic;
				if (this.cmbLayDistanceContainsAutomatic) {
					this.cmbLayDistance.Items.Insert(0, new Europlan.Common.PlannedEurovalProductPanel.LayDistanceItem(null, EuroplanRes.EurovalProduct_Automatisch/*"Automatisch"*/));
				} else {
					this.cmbLayDistance.Items.RemoveAt(0);
				}
			}

			if (this.cmbRimTypeContainsAutomatic != newCmbRimTypeContainsAutomatic) {
				this.cmbRimTypeContainsAutomatic = newCmbRimTypeContainsAutomatic;
				if (this.cmbRimTypeContainsAutomatic) {
					if (this.cmbRimTypeContainsNone) {
						this.cmbRimTypeContainsNone = false;
						newCmbRimTypeContainsNone = false;
						this.cmbRimType.Items.RemoveAt(0);
					}
					this.cmbRimType.Items.Insert(0, new Europlan.Common.PlannedEurovalProductPanel.RimTypeItem(null, EuroplanRes.EurovalProduct_Automatisch/*"Automatisch"*/));
				} else {
					this.cmbRimType.Items.RemoveAt(0);
					this.cmbRimTypeContainsNone = false;
				}
			}
			if (this.cmbRimTypeContainsNone != newCmbRimTypeContainsNone) {
				this.cmbRimTypeContainsNone = newCmbRimTypeContainsNone;
				if (this.cmbRimTypeContainsNone) {
					this.cmbRimType.Items.Insert(0, new Europlan.Common.PlannedEurovalProductPanel.RimTypeItem(null, ""));
				} else {
					this.cmbRimType.Items.RemoveAt(0);
				}
			}

			this.cmbLayDistance.SelectedItem = new Europlan.Common.PlannedEurovalProductPanel.LayDistanceItem(evProduct.RequestedLayDistance, "");
			this.cmbRimType.SelectedItem = new Europlan.Common.PlannedEurovalProductPanel.RimTypeItem(this.cmbRimType.Enabled ? evProduct.RequestedRimType : null, "");
			if (evProduct.RequestedCircuits != null) {
				this.cmbCircuits.SelectedIndex = evProduct.RequestedCircuits.Value - 1 + (this.cmbCircuitsContainsAutomatic ? 1 : 0);
			} else {
				this.cmbCircuits.SelectedIndex = 0;
			}

			// General
			this.lblQSollHeat.Text = Math.Round(this.plannedProduct.RequestedHeatLoad, 2).ToString();
			this.lblQSollCool.Text = Math.Round(this.plannedProduct.RequestedCoolLoad, 2).ToString();
			this.lblQkSollHeat.Text = Math.Round(this.plannedProduct.RequestedHeatLoadPerSqM, 2).ToString();
			this.lblQkSollCool.Text = this.plannedProduct.PlannedArea.HasValue ? Math.Round(this.plannedProduct.RequestedCoolLoad / this.plannedProduct.PlannedArea.Value, 2).ToString() : "0";
			this.lblQfbhHeat.Text = Math.Round(evProduct.PlannedHeatLoad, 2).ToString();
			this.lblQfbhCool.Text = Math.Round(evProduct.PlannedCoolLoad, 2).ToString();
			double qRestHeat = evProduct.PlannedHeatLoad - this.plannedProduct.RequestedHeatLoad;
			double qRestCool = evProduct.PlannedCoolLoad - this.plannedProduct.RequestedCoolLoad;
			this.lblQRestHeat.Text = Math.Round(qRestHeat, 2).ToString("+0.00;-0.00");
			this.lblQRestCool.Text = Math.Round(qRestCool, 2).ToString("+0.00;-0.00");

			// Randzone
			if (complete && evProduct.PlannedRimType.HasValue) {
				switch (evProduct.PlannedRimLayDistance) {
					case EurovalProduct.EurovalLayDistance.EV5:
						this.lblRimVaHeat.Text = EuroplanRes.EurovalProduct_EV5; //"EV5";
						this.lblRimVaCool.Text = EuroplanRes.EurovalProduct_EV5; //"EV5";
						this.lblRimVa.Text = EuroplanRes.EurovalProduct_EV5 + "/" + evProduct.PlannedRimWidth.ToString(); //"EV5";
						break;
					case EurovalProduct.EurovalLayDistance.EV10:
						this.lblRimVaHeat.Text = EuroplanRes.EurovalProduct_EV10; //"EV10";
						this.lblRimVaCool.Text = EuroplanRes.EurovalProduct_EV10; //"EV10";
						this.lblRimVa.Text = EuroplanRes.EurovalProduct_EV10 + "/" + evProduct.PlannedRimWidth.ToString(); //"EV10";
						break;
					case EurovalProduct.EurovalLayDistance.EV15:
						this.lblRimVaHeat.Text = EuroplanRes.EurovalProduct_EV15; //"EV15";
						this.lblRimVaCool.Text = EuroplanRes.EurovalProduct_EV15; //"EV15";
						this.lblRimVa.Text = EuroplanRes.EurovalProduct_EV15 + "/" + evProduct.PlannedRimWidth.ToString(); //"EV15";
						break;
					case EurovalProduct.EurovalLayDistance.EV20:
						this.lblRimVaHeat.Text = EuroplanRes.EurovalProduct_EV20; //"EV20";
						this.lblRimVaCool.Text = EuroplanRes.EurovalProduct_EV20; //"EV20";
						this.lblRimVa.Text = EuroplanRes.EurovalProduct_EV20 + "/" + evProduct.PlannedRimWidth.ToString(); //"EV20";
						break;
					case EurovalProduct.EurovalLayDistance.EV25:
						this.lblRimVaHeat.Text = EuroplanRes.EurovalProduct_EV25; //"EV25";
						this.lblRimVaCool.Text = EuroplanRes.EurovalProduct_EV25; //"EV25";
						this.lblRimVa.Text = EuroplanRes.EurovalProduct_EV25 + "/" + evProduct.PlannedRimWidth.ToString(); //"EV25";
						break;
					case EurovalProduct.EurovalLayDistance.EV30:
						this.lblRimVaHeat.Text = EuroplanRes.EurovalProduct_EV30; //"EV30";
						this.lblRimVaCool.Text = EuroplanRes.EurovalProduct_EV30; //"EV30";
						this.lblRimVa.Text = EuroplanRes.EurovalProduct_EV30 + "/" + evProduct.PlannedRimWidth.ToString(); //"EV30";
						break;
					case EurovalProduct.EurovalLayDistance.EV35:
						this.lblRimVaHeat.Text = EuroplanRes.EurovalProduct_EV35; //"EV35";
						this.lblRimVaCool.Text = EuroplanRes.EurovalProduct_EV35; //"EV35";
						this.lblRimVa.Text = EuroplanRes.EurovalProduct_EV35 + "/" + evProduct.PlannedRimWidth.ToString(); //"EV35";
						break;
					default:
						this.lblRimVaHeat.Text = "--";
						this.lblRimVaCool.Text = "--";
						this.lblRimVa.Text = "--";
						break;
				}
				this.lblRimBHeat.Text = evProduct.PlannedRimWidth.ToString();
				this.lblRimBCool.Text = evProduct.PlannedRimWidth.ToString();
				this.lblRimTfbHeat.Text = Math.Round(evProduct.PlannedFloorTemperatureHeatRim, 1).ToString();
				this.lblRimTfbCool.Text = Math.Round(evProduct.PlannedFloorTemperatureCoolRim, 1).ToString();
				this.lblRimQHeat.Text = Math.Round(evProduct.PlannedHeatLoadRim, 0).ToString();
				this.lblRimQCool.Text = Math.Round(evProduct.PlannedCoolLoadRim, 0).ToString();
			} else {
				this.lblRimVaHeat.Text = "";
				this.lblRimVaCool.Text = "";
				this.lblRimVa.Text = "--";
				this.lblRimBHeat.Text = "";
				this.lblRimBCool.Text = "";
				this.lblRimTfbHeat.Text = "";
				this.lblRimTfbCool.Text = "";
				this.lblRimQHeat.Text = "";
				this.lblRimQCool.Text = "";
			}

			// Aufenthaltszone
			if (complete && evProduct.PlannedLayDistance.HasValue) {
				switch (evProduct.PlannedLayDistance) {
					case EurovalProduct.EurovalLayDistance.EV5:
						this.lblResidenceVaHeat.Text = EuroplanRes.EurovalProduct_EV5; //"EV5";
						this.lblResidenceVaCool.Text = EuroplanRes.EurovalProduct_EV5; //"EV5";
						this.lblResidenceVa.Text = EuroplanRes.EurovalProduct_EV5; //"EV5";
						break;
					case EurovalProduct.EurovalLayDistance.EV10:
						this.lblResidenceVaHeat.Text = EuroplanRes.EurovalProduct_EV10; //"EV10";
						this.lblResidenceVaCool.Text = EuroplanRes.EurovalProduct_EV10; //"EV10";
						this.lblResidenceVa.Text = EuroplanRes.EurovalProduct_EV10; //"EV10";
						break;
					case EurovalProduct.EurovalLayDistance.EV15:
						this.lblResidenceVaHeat.Text = EuroplanRes.EurovalProduct_EV15; //"EV15";
						this.lblResidenceVaCool.Text = EuroplanRes.EurovalProduct_EV15; //"EV15";
						this.lblResidenceVa.Text = EuroplanRes.EurovalProduct_EV15; //"EV15";
						break;
					case EurovalProduct.EurovalLayDistance.EV20:
						this.lblResidenceVaHeat.Text = EuroplanRes.EurovalProduct_EV20; //"EV20";
						this.lblResidenceVaCool.Text = EuroplanRes.EurovalProduct_EV20; //"EV20";
						this.lblResidenceVa.Text = EuroplanRes.EurovalProduct_EV20; //"EV20";
						break;
					case EurovalProduct.EurovalLayDistance.EV25:
						this.lblResidenceVaHeat.Text = EuroplanRes.EurovalProduct_EV25; //"EV25";
						this.lblResidenceVaCool.Text = EuroplanRes.EurovalProduct_EV25; //"EV25";
						this.lblResidenceVa.Text = EuroplanRes.EurovalProduct_EV25; //"EV25";
						break;
					case EurovalProduct.EurovalLayDistance.EV30:
						this.lblResidenceVaHeat.Text = EuroplanRes.EurovalProduct_EV30; //"EV30";
						this.lblResidenceVaCool.Text = EuroplanRes.EurovalProduct_EV30; //"EV30";
						this.lblResidenceVa.Text = EuroplanRes.EurovalProduct_EV30; //"EV30";
						break;
					case EurovalProduct.EurovalLayDistance.EV35:
						this.lblResidenceVaHeat.Text = EuroplanRes.EurovalProduct_EV35; //"EV35";
						this.lblResidenceVaCool.Text = EuroplanRes.EurovalProduct_EV35; //"EV35";
						this.lblResidenceVa.Text = EuroplanRes.EurovalProduct_EV35; //"EV35";
						break;
					default:
						this.lblResidenceVaHeat.Text = "--";
						this.lblResidenceVaCool.Text = "--";
						this.lblResidenceVa.Text = "--";
						break;
				}
				this.lblResidenceAHeat.Text = evProduct.PlannedAreaResidence.ToString();
				this.lblResidenceACool.Text = evProduct.PlannedAreaResidence.ToString();
				this.lblResidenceTfbHeat.Text = Math.Round(evProduct.PlannedFloorTemperatureHeatResidence, 1).ToString();
				this.lblResidenceTfbCool.Text = Math.Round(evProduct.PlannedFloorTemperatureCoolResidence, 1).ToString();
				this.lblResidenceQHeat.Text = Math.Round(evProduct.PlannedHeatLoadResidence, 0).ToString();
				this.lblResidenceQCool.Text = Math.Round(evProduct.PlannedCoolLoadResidence, 0).ToString();
			} else {
				this.lblResidenceVaHeat.Text = "";
				this.lblResidenceVaCool.Text = "";
				this.lblResidenceVa.Text = "--";
				this.lblResidenceAHeat.Text = "";
				this.lblResidenceACool.Text = "";
				this.lblResidenceTfbHeat.Text = "";
				this.lblResidenceTfbCool.Text = "";
				this.lblResidenceQHeat.Text = "";
				this.lblResidenceQCool.Text = "";
			}

			// anbindung
			this.lblConnectionAHeat.Text = evProduct.PlannedRemoveArea.ToString();
			this.lblConnectionACool.Text = evProduct.PlannedRemoveArea.ToString();
			this.lblConnectionQHeat.Text = Math.Round(evProduct.PlannedHeatLoadAnbindung, 0).ToString();
			this.lblConnectionQCool.Text = Math.Round(evProduct.PlannedCoolLoadAnbindung, 0).ToString();

			// heizkreis
			this.lblCircuitCountHeat.Text = evProduct.PlannedCircuitCount.ToString();
			this.lblCircuitCountCool.Text = evProduct.PlannedCircuitCount.ToString();
			this.lblCircuitCount.Text = (complete ? evProduct.PlannedCircuitCount.ToString() : "--");
			this.lblPipeLengthHeat.Text = Math.Round(evProduct.PlannedPipeLengthPerCircuit, 1).ToString();
			this.lblPipeLengthCool.Text = Math.Round(evProduct.PlannedPipeLengthPerCircuit, 1).ToString();
			this.lblMhHeat.Text = Math.Round(evProduct.PlannedMaxMhHeat, 1).ToString();
			this.lblMhCool.Text = Math.Round(evProduct.PlannedMaxMhCool, 1).ToString();
			this.lblDeltaPHeat.Text = Math.Round(evProduct.PlannedDeltaRhoHeat, 1).ToString();
			this.lblDeltaPCool.Text = Math.Round(evProduct.PlannedDeltaRhoCool, 1).ToString(); ;
			this.lblSpreizungHeat.Text = Math.Round(evProduct.PlannedSpreizungHeat, 1).ToString();
			this.lblSpreizungCool.Text = Math.Round(evProduct.PlannedSpreizungCool, 1).ToString();

			this.numCorners.Enabled = evProduct.PlannedRimLength > 0;
			this.numRim.Value = Math.Round((decimal)evProduct.PlannedRimLength, 2);
			this.numCorners.Value = (decimal)evProduct.PlannedRimCorners;

			this.planPanel.InvalidateGraphics();
		}

		private void europlanPlanner_ProjectChanged(object sender) {
			this.CalculateAndUpdate();
			this.changed = true;
		}

		public bool Changed {
			get {
				// TODO remove this when the cahnged flag is properly implemented
				return true;
				return this.changed; 
			}
		}

		private void eurovalPlanner_ModeChanged(object sender, EventArgs e) {
			UpdateButtons();
		//    if (this.eurovalPlanner.Mode == EurovalPlanner.KlimaBodenMode.KDM_LAYOUT_ADD_AREA_FINISH) {
		//        this.numNewRotation.Enabled = false;
		//        this.btnNewCcwLarge.Enabled = false;
		//        this.btnNewCcwSmall.Enabled = false;
		//        this.btnNewCwLarge.Enabled = false;
		//        this.btnNewCwSmall.Enabled = false;
		//        this.btnNewHorizontal.Enabled = false;
		//        this.btnNewVertical.Enabled = false;
		//        this.btnAdd.Enabled = true;
		//        this.chkSelectReferenceModule.Enabled = true;
		//        if (this.planPanel.Mode != PlanMode.PM_PLANNER_DRAG) {
		//            this.planPanel.Mode = PlanMode.PM_PLANNER_DRAG;
		//        }
		//    } else {
		//        this.numNewRotation.Enabled = true;
		//        this.btnNewCcwLarge.Enabled = true;
		//        this.btnNewCcwSmall.Enabled = true;
		//        this.btnNewCwLarge.Enabled = true;
		//        this.btnNewCwSmall.Enabled = true;
		//        this.btnNewHorizontal.Enabled = true;
		//        this.btnNewVertical.Enabled = true;
		//    }
		//    if (this.eurovalPlanner.Mode != EurovalPlanner.KlimaBodenMode.KDM_LAYOUT_ADD_AREA_PICK_REFERENCE) {
		//        if (this.eurovalPlanner.Mode != EurovalPlanner.KlimaBodenMode.KDM_LAYOUT_ADD_AREA_FINISH) {
		//            this.btnAdd.Enabled = false;
		//            this.chkSelectReferenceModule.Enabled = false;
		//        }
		//        this.chkSelectReferenceModule.Checked = false;
		//    }
		}

		private void rbCalculationType_CheckedChanged(object sender, EventArgs e) {
			this.plannedProduct.CalculateHeat = this.rbCalculateHeat.Checked || this.rbCalculateBoth.Checked;
			this.plannedProduct.CalculateCool = this.rbCalculateCool.Checked || this.rbCalculateBoth.Checked;
			this.CalculateAndUpdate();
		}

		private void cmbLayDistance_SelectedIndexChanged(object sender, EventArgs e) {
			(this.plannedProduct.Product as EurovalProduct).RequestedLayDistance = (this.cmbLayDistance.SelectedItem as Europlan.Common.PlannedEurovalProductPanel.LayDistanceItem).layDistance;
			this.CalculateAndUpdate();
		}

		private void cmbRimType_SelectedIndexChanged(object sender, EventArgs e) {
			(this.plannedProduct.Product as EurovalProduct).RequestedRimType = (this.cmbRimType.SelectedItem as Europlan.Common.PlannedEurovalProductPanel.RimTypeItem).rimType;
			this.CalculateAndUpdate();
		}

		private void cmbCircuits_SelectedIndexChanged(object sender, EventArgs e) {
			if (this.cmbCircuits.SelectedIndex >= (this.cmbCircuitsContainsAutomatic ? 1 : 0)) {
				(this.plannedProduct.Product as EurovalProduct).RequestedCircuits = this.cmbCircuits.SelectedIndex + (this.cmbCircuitsContainsAutomatic ? 0 : 1);
			} else {
				(this.plannedProduct.Product as EurovalProduct).RequestedCircuits = null;
			}
			this.CalculateAndUpdate();
		}

		private void numCorners_ValueChanged(object sender, EventArgs e) {
			(this.plannedProduct.Product as EurovalProduct).PlannedRimCorners = (int)this.numCorners.Value;
			this.CalculateAndUpdate();
		}

	}
}