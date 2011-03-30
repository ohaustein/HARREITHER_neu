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

		private ModulKlimaBodenConstructionFrei frei = null;
		private ModulKlimaBodenConstructionStaffeln staffeln = null;

		private int ignoreRotation = 0;
		private bool newVisible = false;
		private PlannedProduct plannedProduct;

		private bool changed = false;

		public EurovalPlannerForm(PlannedProduct plannedProduct) {
			InitializeComponent();
			this.plannedProduct = plannedProduct;

			EurovalProduct product = plannedProduct.Product as EurovalProduct;

			this.eurovalPlanner.Product = product;

			this.UpdateToolbar(this.tabs.SelectedTab);
			this.CalculateAndUpdate();
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
				//this.eurovalPlanner.Mode = EurovalPlanner.KlimaBodenMode.KDM_NONE;
				//this.planPanel.Mode = PlanMode.PM_MOVE;
				//this.UpdateButtons();
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
				//this.btnAddModules.Checked = false;
				//this.btnSelectModule.Checked = false;
			} else if ((this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.eurovalPlanner.Mode == EurovalPlanner.EurovalMode.KDM_LAYOUT_ADD_AREA) {
				this.btnMove.Checked = false;
				//this.btnAddModules.Checked = true;
				//this.btnSelectModule.Checked = false;
			} else {
				this.btnMove.Checked = false;
				//this.btnAddModules.Checked = false;
				//this.btnSelectModule.Checked = false;
			}
			//this.grpSelectedModules.Visible = this.btnSelectModule.Checked;
			//this.grpSelection.Visible = this.btnSelectModule.Checked;
			//this.grpNewModules.Visible = this.btnAddModules.Checked;
			if ((this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.eurovalPlanner.Mode == EurovalPlanner.EurovalMode.KDM_LAYOUT_ADD_AREA) {
				if (!this.newVisible) {
					this.newVisible = true;
					this.ignoreListChange++;
					//lstCircuits.Items.Add("neuer HK");
					this.ignoreListChange--;
				}
			} else {
				if (this.newVisible) {
					this.newVisible = false;
					this.ignoreListChange++;
					//this.lstCircuits.Items.RemoveAt(this.lstCircuits.Items.Count - 1);
					this.ignoreListChange--;
				}
			}
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

		private void UpdateLists(bool updateCircuits, bool selectLastCircuit) {
			//List<Circuit> circuits = (this.eurovalPlanner.Product.ContainsModules ? this.eurovalPlanner.Product.PlannedCircuits : new List<Circuit>());

			//int dec = this.newVisible ? 1 : 0;
			//this.newVisible = this.eurovalPlanner.Mode == EurovalPlanner.KlimaBodenMode.KDM_LAYOUT_ADD_AREA ||
			//    this.eurovalPlanner.Mode == EurovalPlanner.KlimaBodenMode.KDM_LAYOUT_ADD_AREA_FINISH;
			//ignoreListChange++;

			//if (updateCircuits) {
			//    int circuitsCount = circuits.Count;
			//    int tmp = (lstCircuits.SelectedIndex == lstCircuits.Items.Count - dec ? circuitsCount : lstCircuits.SelectedIndex);
			//    lstCircuits.BeginUpdate();
			//    lstCircuits.Items.Clear();
			//    for (int i = 1; i <= circuitsCount; i++) {
			//        lstCircuits.Items.Add(EuroplanRes.PlannedModulKlimaDeckeProductPanel_HeizkreisAbkuerzung + i.ToString());
			//    }
			//    if (this.newVisible) {
			//        lstCircuits.Items.Add("neuer HK");
			//    }
			//    if (selectLastCircuit) {
			//        ignoreListChange--;
			//        if (this.newVisible && lstCircuits.Items.Count > 1) {
			//            lstCircuits.SelectedIndex = lstCircuits.Items.Count - 2;
			//        } else {
			//            lstCircuits.SelectedIndex = lstCircuits.Items.Count - 1;
			//        }
			//        ignoreListChange++;
			//    } else {
			//        if (tmp < lstCircuits.Items.Count && tmp >= 0) {
			//            lstCircuits.SelectedIndex = tmp;
			//        } else {
			//            if (this.newVisible) {
			//                if (tmp < 0 && lstCircuits.Items.Count > 1) {
			//                    lstCircuits.SelectedIndex = lstCircuits.Items.Count - 2;
			//                } else {
			//                    lstCircuits.SelectedIndex = lstCircuits.Items.Count - 1;
			//                }
			//            } else {
			//                lstCircuits.SelectedIndex = -1;
			//            }
			//        }
			//    }
			//    lstCircuits.EndUpdate();
			//}

			//ignoreListChange--;
			//this.lstCircuits_SelectedIndexChanged(this.lstRows, EventArgs.Empty);
		}

		private int ignoreListChange = 0;
		//private void lstCircuits_SelectedIndexChanged(object sender, EventArgs e) {
		//    if (this.ignoreListChange == 0) {
		//        this.ignoreListChange++;
		//        int dec = this.newVisible ? 1 : 0;

		//        if (this.lstCircuits.SelectedIndex >= 0 && this.lstCircuits.SelectedIndex < this.lstCircuits.Items.Count - dec) {
		//            ModulBodenCircuit circuit = (this.lstCircuits.Items.Count - dec > this.lstCircuits.SelectedIndex ? (this.eurovalPlanner.Product.PlannedCircuits[this.lstCircuits.SelectedIndex] as ModulBodenCircuit) : null);
		//            this.eurovalPlanner.HighlightCircuit = circuit;
		//            if (circuit != null) {
		//                btnColor.BackColor = circuit.CircuitColor;
		//                btnColor.Enabled = true;
		//            } else {
		//                btnColor.BackColor = Color.Transparent;
		//                btnColor.Enabled = false;
		//            }
		//        } else {
		//            this.eurovalPlanner.HighlightCircuit = null;
		//            btnColor.BackColor = Color.Transparent;
		//            btnColor.Enabled = false;
		//        }
		//        this.UpdateSelectedModules();
		//        numLength.Enabled = this.eurovalPlanner.HighlightCircuit != null;
		//        if (numLength.Enabled) {
		//            this.numLength.Value = (decimal)this.eurovalPlanner.HighlightCircuit.SonstigeVerbindeleitung;
		//        } else {
		//            this.numLength.Text = "";
		//        }

		//        this.ignoreListChange--;
		//    }
		//}

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

		private void eurovalPlanner_ListsNeedUpdate(object sender, EventArgs e) {
		//    this.UpdateLists(true, false);
		}

	}
}