using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;
using WW.Math.Geometry;
using WW.Math;

namespace Europlan.Common.Products {
	public partial class EcothermPlannerForm : Form {

		private PlannedProduct plannedProduct;

		private bool changed = false;

		private bool cmbLayDistanceContainsAutomatic = true;
		private bool cmbRimTypeContainsAutomatic = true;
		private bool cmbRimTypeContainsNone = false;
		private bool cmbCircuitsContainsAutomatic = true;

		public EcothermPlannerForm(PlannedProduct plannedProduct) {
			InitializeComponent();
			this.plannedProduct = plannedProduct;

			EcothermProduct product = plannedProduct.Product as EcothermProduct;
			if (product.PlannedAreaGraphical.Count == 0) {
				product.PlannedAreaGraphical.AddRange(product.AssociatedRoom.RoomCoordinates);
				product.PlannedFloorArea = (float)Math.Round(Math.Abs(new Polygon2D(product.PlannedAreaGraphical).GetArea()) / Math.Pow(product.AssociatedRoom.AssociatedPlan.Measure.Value, 2.0), 2);

				if (product.AssociatedRoom.RoomUnusedAreaCoordinates != null) {
					Polygon2D productPolygon = new Polygon2D(product.PlannedAreaGraphical);
					if (productPolygon.IsClockwise()) {
						productPolygon.Reverse();
					}
					List<Polygon2D> list1 = new List<Polygon2D>();
					list1.Add(productPolygon);
					List<Polygon2D> list2 = new List<Polygon2D>();

					foreach (List<Point2D> unusedArea in product.AssociatedRoom.RoomUnusedAreaCoordinates) {
						Polygon2D unusedAreaPolygon = new Polygon2D(unusedArea);
						if (unusedAreaPolygon.IsClockwise()) {
							unusedAreaPolygon.Reverse();
						}
						list2.Add(unusedAreaPolygon);
					}
					IList<Polygon2D> clippedPolygons = Polygon2D.GetIntersection(list1, list2);
					double area = 0;
					foreach (Polygon2D clippedPolygon in clippedPolygons) {
						area += Math.Round(Math.Abs(clippedPolygon.GetArea()) / Math.Pow(product.AssociatedRoom.AssociatedPlan.Measure.Value, 2.0), 2);
					}
					product.PlannedAreaUnheated = (float)area;
				}

			}

			if (product.TextBoxPosition == Point2D.Zero) {
				Polygon2D polygon = new Polygon2D(product.PlannedAreaGraphical);
				if (polygon.GetCentroid().HasValue) {
					product.TextBoxPosition = polygon.GetCentroid().Value;
				}
			}

			this.eurovalPlanner.Product = product;
			this.SetLanguage();
			this.CalculateAndUpdate();
			this.connectionPlanner.Product = product;
		}

		private void SetLanguage() {
			this.cmbLayDistance.Items.Clear();
			this.cmbRimType.Items.Clear();
			this.cmbCircuits.Items.Clear();

			this.cmbLayDistance.Items.Add(new Europlan.Common.PlannedEcothermProductPanel.LayDistanceItem(null, EuroplanRes.EcothermProduct_Automatisch/*"Automatisch"*/));
			this.cmbLayDistance.Items.Add(new Europlan.Common.PlannedEcothermProductPanel.LayDistanceItem(EcothermProduct.EcothermLayDistance.EV35, EuroplanRes.EcothermProduct_EV35/*"EV35"*/));
			this.cmbLayDistance.Items.Add(new Europlan.Common.PlannedEcothermProductPanel.LayDistanceItem(EcothermProduct.EcothermLayDistance.EV30, EuroplanRes.EcothermProduct_EV30/*"EV30"*/));
			this.cmbLayDistance.Items.Add(new Europlan.Common.PlannedEcothermProductPanel.LayDistanceItem(EcothermProduct.EcothermLayDistance.EV25, EuroplanRes.EcothermProduct_EV25/*"EV25"*/));
			this.cmbLayDistance.Items.Add(new Europlan.Common.PlannedEcothermProductPanel.LayDistanceItem(EcothermProduct.EcothermLayDistance.EV20, EuroplanRes.EcothermProduct_EV20/*"EV20"*/));
			this.cmbLayDistance.Items.Add(new Europlan.Common.PlannedEcothermProductPanel.LayDistanceItem(EcothermProduct.EcothermLayDistance.EV15, EuroplanRes.EcothermProduct_EV15/*"EV15"*/));
			this.cmbLayDistance.Items.Add(new Europlan.Common.PlannedEcothermProductPanel.LayDistanceItem(EcothermProduct.EcothermLayDistance.EV10, EuroplanRes.EcothermProduct_EV10/*"EV10"*/));
			this.cmbLayDistance.Items.Add(new Europlan.Common.PlannedEcothermProductPanel.LayDistanceItem(EcothermProduct.EcothermLayDistance.EV5, EuroplanRes.EcothermProduct_EV5/*"EV5"*/));

			this.cmbRimType.Items.Add(new Europlan.Common.PlannedEcothermProductPanel.RimTypeItem(null, EuroplanRes.EcothermProduct_Automatisch/*"Automatisch"*/));
			this.cmbRimType.Items.Add(new Europlan.Common.PlannedEcothermProductPanel.RimTypeItem(EcothermProduct.EcothermRimType.EV15_60, EuroplanRes.EcothermProduct_EV15_60/*"EV15/60"*/));
			this.cmbRimType.Items.Add(new Europlan.Common.PlannedEcothermProductPanel.RimTypeItem(EcothermProduct.EcothermRimType.EV15_120, EuroplanRes.EcothermProduct_EV15_120/*"EV15/120"*/));
			this.cmbRimType.Items.Add(new Europlan.Common.PlannedEcothermProductPanel.RimTypeItem(EcothermProduct.EcothermRimType.EV15_180, EuroplanRes.EcothermProduct_EV15_180/*"EV15/180"*/));
			this.cmbRimType.Items.Add(new Europlan.Common.PlannedEcothermProductPanel.RimTypeItem(EcothermProduct.EcothermRimType.EV10_55, EuroplanRes.EcothermProduct_EV10_55/*"EV10/55"*/));
			this.cmbRimType.Items.Add(new Europlan.Common.PlannedEcothermProductPanel.RimTypeItem(EcothermProduct.EcothermRimType.EV10_110, EuroplanRes.EcothermProduct_EV10_110/*"EV10/110"*/));
			this.cmbRimType.Items.Add(new Europlan.Common.PlannedEcothermProductPanel.RimTypeItem(EcothermProduct.EcothermRimType.EV10_165, EuroplanRes.EcothermProduct_EV10_165/*"EV10/165"*/));
			this.cmbRimType.Items.Add(new Europlan.Common.PlannedEcothermProductPanel.RimTypeItem(EcothermProduct.EcothermRimType.EV5_40, EuroplanRes.EcothermProduct_EV5_40/*"EV5/40"*/));
			this.cmbRimType.Items.Add(new Europlan.Common.PlannedEcothermProductPanel.RimTypeItem(EcothermProduct.EcothermRimType.EV5_80, EuroplanRes.EcothermProduct_EV5_80/*"EV5/80"*/));
			this.cmbRimType.Items.Add(new Europlan.Common.PlannedEcothermProductPanel.RimTypeItem(EcothermProduct.EcothermRimType.EV5_120, EuroplanRes.EcothermProduct_EV5_120/*"EV5/120"*/));

			this.cmbCircuits.Items.Add(EuroplanRes.EcothermProduct_Automatisch/*"Automatisch"*/);
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
				this.eurovalPlanner.Mode = EcothermPlanner.PipeProductMode.EVM_NONE;
				this.planPanel.Mode = PlanMode.PM_MOVE;
				this.UpdateButtons();
			}
		}

		private void btnDefineArea_Click(object sender, EventArgs e) {
			if (!btnDefineArea.Checked) {
				this.SetProductPlanner();
				this.eurovalPlanner.Mode = EcothermPlanner.PipeProductMode.EVM_ADD_AREA;
				this.planPanel.Mode = PlanMode.PM_PLANNER_CLICK;
				this.UpdateButtons();
			}
		}

		private void btnAddReduced_Click(object sender, EventArgs e) {
			if (!btnAddReduced.Checked) {
				this.SetProductPlanner();
				this.eurovalPlanner.Mode = EcothermPlanner.PipeProductMode.EVM_ADD_RED;
				this.planPanel.Mode = PlanMode.PM_PLANNER_CLICK;
				this.UpdateButtons();
			}
		}

		private void btnDelReduced_Click(object sender, EventArgs e) {
			if (!btnDelReduced.Checked) {
				this.SetProductPlanner();
				this.eurovalPlanner.Mode = EcothermPlanner.PipeProductMode.EVM_DEL_RED;
				this.planPanel.Mode = PlanMode.PM_PLANNER_CLICK;
				this.UpdateButtons();
			}
		}

		private void btnAddRz_Click(object sender, EventArgs e) {
			if (!btnAddRz.Checked) {
				this.SetProductPlanner();
				this.eurovalPlanner.Mode = EcothermPlanner.PipeProductMode.EVM_ADD_RZ;
				this.planPanel.Mode = PlanMode.PM_PLANNER_CLICK;
				this.UpdateButtons();
			}
		}

		private void btnDelRz_Click(object sender, EventArgs e) {
			if (!btnDelRz.Checked) {
				this.SetProductPlanner();
				this.eurovalPlanner.Mode = EcothermPlanner.PipeProductMode.EVM_DEL_RZ;
				this.planPanel.Mode = PlanMode.PM_PLANNER_CLICK;
				this.UpdateButtons();
			}
		}

		private void btnSetText_Click(object sender, EventArgs e) {
			if (!btnSetText.Checked) {
				this.SetProductPlanner();
				this.eurovalPlanner.Mode = EcothermPlanner.PipeProductMode.EVM_SET_TEXT;
				this.planPanel.Mode = PlanMode.PM_PLANNER_CLICK;
				this.UpdateButtons();
			}
		}

		private void btnFontPlus_Click(object sender, EventArgs e) {
			this.eurovalPlanner.Product.TextBoxFontSize = this.eurovalPlanner.Product.TextBoxFontSize + 1;
			this.planPanel.InvalidateGraphics();
		}

		private void btnFontMinus_Click(object sender, EventArgs e) {
			this.eurovalPlanner.Product.TextBoxFontSize = Math.Max(this.eurovalPlanner.Product.TextBoxFontSize - 1, 3.0f);
			this.planPanel.InvalidateGraphics();
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
			EcothermProduct product = plannedProduct.Product as EcothermProduct;
			this.btnAddReduced.Enabled = product.PlannedAreaGraphical.Count > 0;
			this.btnDelReduced.Enabled = product.PlannedAreaGraphical.Count > 0;
			this.btnAddRz.Enabled = product.PlannedAreaGraphical.Count > 0;
			this.btnDelRz.Enabled = product.PlannedAreaGraphical.Count > 0;
			this.btnSetText.Enabled = product.PlannedAreaGraphical.Count > 0;
			if (this.planPanel.Mode == PlanMode.PM_MOVE) {
				this.btnMove.Checked = true;
				this.btnDefineArea.Checked = false;
				this.btnAddReduced.Checked = false;
				this.btnDelReduced.Checked = false;
				this.btnAddRz.Checked = false;
				this.btnDelRz.Checked = false;
				this.btnSetText.Checked = false;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = false;
			} else if (this.planPanel.ProductPlanner == this.eurovalPlanner && (this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.eurovalPlanner.Mode == EcothermPlanner.PipeProductMode.EVM_ADD_AREA) {
				this.btnMove.Checked = false;
				this.btnDefineArea.Checked = true;
				this.btnAddReduced.Checked = false;
				this.btnDelReduced.Checked = false;
				this.btnAddRz.Checked = false;
				this.btnDelRz.Checked = false;
				this.btnSetText.Checked = false;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = false;
			} else if (this.planPanel.ProductPlanner == this.eurovalPlanner && (this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.eurovalPlanner.Mode == EcothermPlanner.PipeProductMode.EVM_ADD_RED) {
				this.btnMove.Checked = false;
				this.btnDefineArea.Checked = false;
				this.btnAddReduced.Checked = true;
				this.btnDelReduced.Checked = false;
				this.btnAddRz.Checked = false;
				this.btnDelRz.Checked = false;
				this.btnSetText.Checked = false;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = false;
			} else if (this.planPanel.ProductPlanner == this.eurovalPlanner && (this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.eurovalPlanner.Mode == EcothermPlanner.PipeProductMode.EVM_DEL_RED) {
				this.btnMove.Checked = false;
				this.btnDefineArea.Checked = false;
				this.btnAddReduced.Checked = false;
				this.btnDelReduced.Checked = true;
				this.btnAddRz.Checked = false;
				this.btnDelRz.Checked = false;
				this.btnSetText.Checked = false;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = false;
			} else if (this.planPanel.ProductPlanner == this.eurovalPlanner && (this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.eurovalPlanner.Mode == EcothermPlanner.PipeProductMode.EVM_ADD_RZ) {
				this.btnMove.Checked = false;
				this.btnDefineArea.Checked = false;
				this.btnAddReduced.Checked = false;
				this.btnDelReduced.Checked = false;
				this.btnAddRz.Checked = true;
				this.btnDelRz.Checked = false;
				this.btnSetText.Checked = false;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = false;
			} else if (this.planPanel.ProductPlanner == this.eurovalPlanner && (this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.eurovalPlanner.Mode == EcothermPlanner.PipeProductMode.EVM_DEL_RZ) {
				this.btnMove.Checked = false;
				this.btnDefineArea.Checked = false;
				this.btnAddReduced.Checked = false;
				this.btnDelReduced.Checked = false;
				this.btnAddRz.Checked = false;
				this.btnDelRz.Checked = true;
				this.btnSetText.Checked = false;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = false;
			} else if (this.planPanel.ProductPlanner == this.eurovalPlanner && (this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.eurovalPlanner.Mode == EcothermPlanner.PipeProductMode.EVM_SET_TEXT) {
				this.btnMove.Checked = false;
				this.btnDefineArea.Checked = false;
				this.btnAddReduced.Checked = false;
				this.btnDelReduced.Checked = false;
				this.btnAddRz.Checked = false;
				this.btnDelRz.Checked = false;
				this.btnSetText.Checked = true;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = false;
			} else if (this.planPanel.ProductPlanner == this.connectionPlanner && (this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.connectionPlanner.Mode == ConnectionPlanner.ConnectionMode.KDM_ADD_CONNECTION) {
				this.btnMove.Checked = false;
				this.btnDefineArea.Checked = false;
				this.btnAddReduced.Checked = false;
				this.btnDelReduced.Checked = false;
				this.btnAddRz.Checked = false;
				this.btnDelRz.Checked = false;
				this.btnAddAnbindeleitungen.Checked = true;
				this.btnSelectAnbindeleitungen.Checked = false;
			} else if (this.planPanel.ProductPlanner == this.connectionPlanner && (this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.connectionPlanner.Mode == ConnectionPlanner.ConnectionMode.KDM_SELECT_CONNECTION) {
				this.btnMove.Checked = false;
				this.btnDefineArea.Checked = false;
				this.btnAddReduced.Checked = false;
				this.btnDelReduced.Checked = false;
				this.btnAddRz.Checked = false;
				this.btnDelRz.Checked = false;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = true;
			} else {
				this.btnMove.Checked = false;
				this.btnDefineArea.Checked = false;
				this.btnAddReduced.Checked = false;
				this.btnDelReduced.Checked = false;
				this.btnAddRz.Checked = false;
				this.btnDelRz.Checked = false;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = false;
			}
		}

		private void EcothermPlannerForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["EcothermPlannerForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
		}

		private void EcothermPlannerForm_FormClosing(object sender, FormClosingEventArgs e) {
			this.connectionPlanner.ReGenerateConnectionPipes();

			SettingsKey settings = SettingsFile.Settings["EcothermPlannerForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			SettingsFile.Update();
		}

		private void CalculateAndUpdate() {
			EcothermProduct evProduct = this.plannedProduct.Product as EcothermProduct;
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

			bool showHeat = this.plannedProduct.RequestedHeatLoad > 0 && evProduct.PlannedLayDistance != EcothermProduct.EcothermLayDistance.NONE;
			bool showCool = this.plannedProduct.RequestedCoolLoad > 0 && evProduct.PlannedLayDistance != EcothermProduct.EcothermLayDistance.NONE;
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

			this.rbCalculateHeat.Enabled = evProduct.CalculateMode == Product.CalculateModeEnum.HEAT || evProduct.CalculateMode == Product.CalculateModeEnum.HEAT_AND_COOL;
			this.rbCalculateCool.Enabled = evProduct.CalculateMode == Product.CalculateModeEnum.HEAT || evProduct.CalculateMode == Product.CalculateModeEnum.HEAT_AND_COOL;
			this.rbCalculateBoth.Enabled = evProduct.CalculateMode == Product.CalculateModeEnum.HEAT_AND_COOL;
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
					this.cmbLayDistance.Items.Insert(0, new Europlan.Common.PlannedEcothermProductPanel.LayDistanceItem(null, EuroplanRes.EcothermProduct_Automatisch/*"Automatisch"*/));
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
					this.cmbRimType.Items.Insert(0, new Europlan.Common.PlannedEcothermProductPanel.RimTypeItem(null, EuroplanRes.EcothermProduct_Automatisch/*"Automatisch"*/));
				} else {
					this.cmbRimType.Items.RemoveAt(0);
					this.cmbRimTypeContainsNone = false;
				}
			}
			if (this.cmbRimTypeContainsNone != newCmbRimTypeContainsNone) {
				this.cmbRimTypeContainsNone = newCmbRimTypeContainsNone;
				if (this.cmbRimTypeContainsNone) {
					this.cmbRimType.Items.Insert(0, new Europlan.Common.PlannedEcothermProductPanel.RimTypeItem(null, ""));
				} else {
					this.cmbRimType.Items.RemoveAt(0);
				}
			}

			this.cmbLayDistance.SelectedItem = new Europlan.Common.PlannedEcothermProductPanel.LayDistanceItem(evProduct.RequestedLayDistance, "");
			this.cmbRimType.SelectedItem = new Europlan.Common.PlannedEcothermProductPanel.RimTypeItem(this.cmbRimType.Enabled ? evProduct.RequestedRimType : null, "");
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
					case EcothermProduct.EcothermLayDistance.EV5:
						this.lblRimVaHeat.Text = EuroplanRes.EcothermProduct_EV5; //"EV5";
						this.lblRimVaCool.Text = EuroplanRes.EcothermProduct_EV5; //"EV5";
						this.lblRimVa.Text = EuroplanRes.EcothermProduct_EV5 + "/" + evProduct.PlannedRimWidth.ToString(); //"EV5";
						break;
					case EcothermProduct.EcothermLayDistance.EV10:
						this.lblRimVaHeat.Text = EuroplanRes.EcothermProduct_EV10; //"EV10";
						this.lblRimVaCool.Text = EuroplanRes.EcothermProduct_EV10; //"EV10";
						this.lblRimVa.Text = EuroplanRes.EcothermProduct_EV10 + "/" + evProduct.PlannedRimWidth.ToString(); //"EV10";
						break;
					case EcothermProduct.EcothermLayDistance.EV15:
						this.lblRimVaHeat.Text = EuroplanRes.EcothermProduct_EV15; //"EV15";
						this.lblRimVaCool.Text = EuroplanRes.EcothermProduct_EV15; //"EV15";
						this.lblRimVa.Text = EuroplanRes.EcothermProduct_EV15 + "/" + evProduct.PlannedRimWidth.ToString(); //"EV15";
						break;
					case EcothermProduct.EcothermLayDistance.EV20:
						this.lblRimVaHeat.Text = EuroplanRes.EcothermProduct_EV20; //"EV20";
						this.lblRimVaCool.Text = EuroplanRes.EcothermProduct_EV20; //"EV20";
						this.lblRimVa.Text = EuroplanRes.EcothermProduct_EV20 + "/" + evProduct.PlannedRimWidth.ToString(); //"EV20";
						break;
					case EcothermProduct.EcothermLayDistance.EV25:
						this.lblRimVaHeat.Text = EuroplanRes.EcothermProduct_EV25; //"EV25";
						this.lblRimVaCool.Text = EuroplanRes.EcothermProduct_EV25; //"EV25";
						this.lblRimVa.Text = EuroplanRes.EcothermProduct_EV25 + "/" + evProduct.PlannedRimWidth.ToString(); //"EV25";
						break;
					case EcothermProduct.EcothermLayDistance.EV30:
						this.lblRimVaHeat.Text = EuroplanRes.EcothermProduct_EV30; //"EV30";
						this.lblRimVaCool.Text = EuroplanRes.EcothermProduct_EV30; //"EV30";
						this.lblRimVa.Text = EuroplanRes.EcothermProduct_EV30 + "/" + evProduct.PlannedRimWidth.ToString(); //"EV30";
						break;
					case EcothermProduct.EcothermLayDistance.EV35:
						this.lblRimVaHeat.Text = EuroplanRes.EcothermProduct_EV35; //"EV35";
						this.lblRimVaCool.Text = EuroplanRes.EcothermProduct_EV35; //"EV35";
						this.lblRimVa.Text = EuroplanRes.EcothermProduct_EV35 + "/" + evProduct.PlannedRimWidth.ToString(); //"EV35";
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
					case EcothermProduct.EcothermLayDistance.EV5:
						this.lblResidenceVaHeat.Text = EuroplanRes.EcothermProduct_EV5; //"EV5";
						this.lblResidenceVaCool.Text = EuroplanRes.EcothermProduct_EV5; //"EV5";
						this.lblResidenceVa.Text = EuroplanRes.EcothermProduct_EV5; //"EV5";
						break;
					case EcothermProduct.EcothermLayDistance.EV10:
						this.lblResidenceVaHeat.Text = EuroplanRes.EcothermProduct_EV10; //"EV10";
						this.lblResidenceVaCool.Text = EuroplanRes.EcothermProduct_EV10; //"EV10";
						this.lblResidenceVa.Text = EuroplanRes.EcothermProduct_EV10; //"EV10";
						break;
					case EcothermProduct.EcothermLayDistance.EV15:
						this.lblResidenceVaHeat.Text = EuroplanRes.EcothermProduct_EV15; //"EV15";
						this.lblResidenceVaCool.Text = EuroplanRes.EcothermProduct_EV15; //"EV15";
						this.lblResidenceVa.Text = EuroplanRes.EcothermProduct_EV15; //"EV15";
						break;
					case EcothermProduct.EcothermLayDistance.EV20:
						this.lblResidenceVaHeat.Text = EuroplanRes.EcothermProduct_EV20; //"EV20";
						this.lblResidenceVaCool.Text = EuroplanRes.EcothermProduct_EV20; //"EV20";
						this.lblResidenceVa.Text = EuroplanRes.EcothermProduct_EV20; //"EV20";
						break;
					case EcothermProduct.EcothermLayDistance.EV25:
						this.lblResidenceVaHeat.Text = EuroplanRes.EcothermProduct_EV25; //"EV25";
						this.lblResidenceVaCool.Text = EuroplanRes.EcothermProduct_EV25; //"EV25";
						this.lblResidenceVa.Text = EuroplanRes.EcothermProduct_EV25; //"EV25";
						break;
					case EcothermProduct.EcothermLayDistance.EV30:
						this.lblResidenceVaHeat.Text = EuroplanRes.EcothermProduct_EV30; //"EV30";
						this.lblResidenceVaCool.Text = EuroplanRes.EcothermProduct_EV30; //"EV30";
						this.lblResidenceVa.Text = EuroplanRes.EcothermProduct_EV30; //"EV30";
						break;
					case EcothermProduct.EcothermLayDistance.EV35:
						this.lblResidenceVaHeat.Text = EuroplanRes.EcothermProduct_EV35; //"EV35";
						this.lblResidenceVaCool.Text = EuroplanRes.EcothermProduct_EV35; //"EV35";
						this.lblResidenceVa.Text = EuroplanRes.EcothermProduct_EV35; //"EV35";
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
		}

		private void rbCalculationType_CheckedChanged(object sender, EventArgs e) {
			this.plannedProduct.CalculateHeat = this.rbCalculateHeat.Checked || this.rbCalculateBoth.Checked;
			this.plannedProduct.CalculateCool = this.rbCalculateCool.Checked || this.rbCalculateBoth.Checked;
			this.CalculateAndUpdate();
		}

		private void cmbLayDistance_SelectedIndexChanged(object sender, EventArgs e) {
			(this.plannedProduct.Product as EcothermProduct).RequestedLayDistance = (this.cmbLayDistance.SelectedItem as Europlan.Common.PlannedEcothermProductPanel.LayDistanceItem).layDistance;
			this.CalculateAndUpdate();
		}

		private void cmbRimType_SelectedIndexChanged(object sender, EventArgs e) {
			(this.plannedProduct.Product as EcothermProduct).RequestedRimType = (this.cmbRimType.SelectedItem as Europlan.Common.PlannedEcothermProductPanel.RimTypeItem).rimType;
			this.CalculateAndUpdate();
		}

		private void cmbCircuits_SelectedIndexChanged(object sender, EventArgs e) {
			if (this.cmbCircuits.SelectedIndex + (this.cmbCircuitsContainsAutomatic ? 0 : 1) != this.eurovalPlanner.Product.RequestedCircuits || ((this.cmbCircuitsContainsAutomatic && this.cmbCircuits.SelectedIndex == 0) != (this.eurovalPlanner.Product.RequestedCircuits == null))) {
				if (this.eurovalPlanner.Product.Connections != null && this.eurovalPlanner.Product.Connections.Count > 0) {
					if (MessageBox.Show("Die Anzahl der Heizkreise von Systemen die bereits an einen Verteiler angeschlossen sind kann nicht mehr geändert werden. Wollen Sie die Anbindeleitungen des Systems löschen?", "Anbindeleitungen löschen", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) {
						this.cmbCircuits.SelectedIndex = this.eurovalPlanner.Product.RequestedCircuits.Value + (this.cmbCircuitsContainsAutomatic ? 0 : 1);
						return;
					}
					this.eurovalPlanner.Product.Connections.Clear();
				}
				if (this.cmbCircuits.SelectedIndex >= (this.cmbCircuitsContainsAutomatic ? 1 : 0)) {
					(this.plannedProduct.Product as EcothermProduct).RequestedCircuits = this.cmbCircuits.SelectedIndex + (this.cmbCircuitsContainsAutomatic ? 0 : 1);
				} else {
					(this.plannedProduct.Product as EcothermProduct).RequestedCircuits = null;
				}
				this.CalculateAndUpdate();
			}
		}

		private void numCorners_ValueChanged(object sender, EventArgs e) {
			(this.plannedProduct.Product as EcothermProduct).PlannedRimCorners = (int)this.numCorners.Value;
			this.CalculateAndUpdate();
		}

		private void SetProductPlanner() {
			if (this.planPanel.ProductPlanner != this.eurovalPlanner) {
				double scale = this.planPanel.PlanScale;
				Vector2D translation = this.planPanel.PlanTranslation;
				this.planPanel.ProductPlanner = this.eurovalPlanner;
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

		private void connectionPlanner_AnbindeleitungAdded(object sender, EventArgs e) {
			if (this.cmbCircuitsContainsAutomatic && this.cmbCircuits.SelectedIndex == 0) {
				this.eurovalPlanner.Product.RequestedCircuits = this.eurovalPlanner.Product.PlannedCircuitCount;
				this.cmbCircuits.SelectedIndex = this.eurovalPlanner.Product.PlannedCircuitCount;
			}
			this.connectionPlanner.ReGenerateConnectionPipes();
			this.CalculateAndUpdate();
		}
	}
}