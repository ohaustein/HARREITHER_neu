using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using WW.Cad.Model;
using WW.Math;

namespace Europlan.Common {
	public partial class PlanPanel : UserControl, IPlanPanel, IProductPlanner {

		public enum PlanTypeEnum {
			PT_CAD,
			PT_IMAGE
		}

		private Plan plan = null;
		private IPlanPanel panel = null;
		private PlanMode tmpMode = PlanMode.PM_MOVE;
		private IProductPlanner productPlanner = null;
		private IPlanPanel connectedPlanPanel;

		public PlanPanel() {
			InitializeComponent();
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public PlanTypeEnum PlanType {
			get { return this.plan is CadPlan ? PlanTypeEnum.PT_CAD : PlanTypeEnum.PT_IMAGE; }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Plan Plan {
			get { return this.plan; }
			set {
				if (this.plan != value)  {
					if (this.panel != null) {
						tmpMode = this.panel.Mode;
						tmpCursor = this.panel.PlanCursor;
					}
					this.plan = value;
					if (this.plan is CadPlan) {
						DxfModel model = (plan as CadPlan).LoadModel();
						this.cadPanelOptions.Plan = plan;
						this.cadPanel.Plan = plan;
						this.cadPanel.PlanScale = (plan as CadPlan).Scale;
						this.cadPanel.PlanTranslation = new Vector2D((plan as CadPlan).TranslationX, (plan as CadPlan).TranslationY);
						this.panel = this.cadPanel;
						this.imagePanel.Visible = false;
						this.cadPanel.Visible = true;
						this.cadPanelOptions.Visible = true;
						this.cadOptions.Visible = true;
						this.imagePanel.ProductPlanner = null;
						this.cadPanel.ProductPlanner = this;
					} else if (this.plan is ImagePlan) {
						this.imagePanel.Plan = plan;
						this.imagePanel.Plan = plan;
						if ((plan as ImagePlan).Scale.HasValue) {
							this.imagePanel.PlanScale = (plan as ImagePlan).Scale.Value;
						}
						this.imagePanel.PlanTranslation = new Vector2D((plan as ImagePlan).XPos, (plan as ImagePlan).YPos);
						this.panel = this.imagePanel;
						this.cadPanel.Visible = false;
						this.cadOptions.Visible = false;
						this.imagePanel.Visible = true;
						this.cadPanel.ProductPlanner = null;
						this.imagePanel.ProductPlanner = this;
					} else {
						this.imagePanel.Visible = false;
						this.cadPanel.Visible = false;
						this.cadOptions.Visible = false;
						this.panel = null;
						this.cadPanel.ProductPlanner = null;
						this.imagePanel.ProductPlanner = null;
					}
					if (this.panel != null) {
						this.panel.Mode = tmpMode;
						this.panel.PlanCursor = tmpCursor;
					}
				}
			}
		}


		#region IPlanPanel Members

		public IProductPlanner ProductPlanner {
			get { return this.productPlanner; }
			set {
				if (this.productPlanner != null) {
					this.productPlanner.ConnectedPlanPanel = null;
				}
				if (value != null && value.ConnectedPlanPanel != null) {
					value = null;
				}
				this.productPlanner = value;
				if (this.productPlanner != null) {
					this.productPlanner.ConnectedPlanPanel = this;
				}
				if (this.panel != null) {
					this.panel.InvalidateGraphics();
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double PlanScale {
			get { return this.panel == null ? 1.0 : this.panel.PlanScale; }
			set {
				if (this.panel != null) {
					this.panel.PlanScale = value;
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public WW.Math.Vector2D PlanTranslation {
			get { return this.panel == null ? new Vector2D() : this.panel.PlanTranslation; }
			set {
				if (this.panel != null) {
					this.panel.PlanTranslation = value;
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public PlanMode Mode {
			get { return this.panel == null ? tmpMode : this.panel.Mode; }
			set {
				if (this.panel != null) {
					this.panel.Mode = value;
				} else {
					this.tmpMode = value;
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public WW.Math.Matrix4D PlanTransformation {
			get { return this.panel == null ? Matrix4D.Identity : this.panel.PlanTransformation; }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ColorMode ColorMode {
			get { return this.panel == null ? ColorMode.CM_WHITE_BG : this.panel.ColorMode; }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ModifierKey ModifierKey {
			get { return this.panel == null ? ModifierKey.MK_NONE : this.panel.ModifierKey; }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool SupportsSnap {
			get { return this.panel == null ? false : this.panel.SupportsSnap; }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public void AddScale(double addedScale, WW.Math.Point2D? center) {
			if (this.panel != null) {
				this.panel.AddScale(addedScale, center);
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool UnsavedChanges {
			get { return this.panel == null ? false : this.panel.UnsavedChanges; }
		}

		public void InvalidateGraphics() {
			if (this.panel != null) {
				this.panel.InvalidateGraphics();
			}
		}
		#endregion

		private void cadPanelOptions_InvalidateNeeded(object sender, EventArgs e) {
			if (this.cadPanel.Visible) {
				this.cadPanel.RecreateDrawables();
			}
		}

		#region IProductPlanner Members
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IPlanPanel ConnectedPlanPanel {
			get { return this.connectedPlanPanel; }
			set { this.connectedPlanPanel = value; }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Cursor CustomCursor {
			get { return this.ProductPlanner != null ? this.ProductPlanner.CustomCursor : null; }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public void PaintAfterPlanPannel(PaintEventArgs e, Matrix4D additionalTransformation, Point2D mousePositionInPlan, Point mousePositionInControl) {
			if (this.ProductPlanner != null) {
				this.ProductPlanner.PaintAfterPlanPannel(e, additionalTransformation, mousePositionInPlan, mousePositionInControl);
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool PlannerClick(Point2D planPoint, Point pointInControl, MouseButtons button) {
			if (this.ProductPlanner != null) {
				return this.ProductPlanner.PlannerClick(planPoint, pointInControl, button);
			}
			return false;
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool PlannerMouseMove(Point2D planPoint, Point pointInControl, MouseButtons button) {
			if (this.ProductPlanner != null) {
				return this.ProductPlanner.PlannerMouseMove(planPoint, pointInControl, button);
			}
			return false;
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool PlannerDragStart(Point2D planPoint, Point pointInControl, MouseButtons button) {
			if (this.ProductPlanner != null) {
				return this.ProductPlanner.PlannerDragStart(planPoint, pointInControl, button);
			}
			return false;
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool PlannerDragMove(Point2D planPoint, Point pointInControl, MouseButtons button) {
			if (this.ProductPlanner != null) {
				return this.ProductPlanner.PlannerDragMove(planPoint, pointInControl, button);
			}
			return false;
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool PlannerDragEnd(Point2D planPoint, Point pointInControl, MouseButtons button) {
			if (this.ProductPlanner != null) {
				return this.ProductPlanner.PlannerDragEnd(planPoint, pointInControl, button);
			}
			return false;
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool PlannerKeyPress(Keys key) {
			if (this.ProductPlanner != null) {
				return this.ProductPlanner.PlannerKeyPress(key);
			}
			return false;
		}

		private Cursor tmpCursor = Cursors.Default;

		public Cursor PlanCursor {
			get { return this.panel != null ? this.panel.PlanCursor : tmpCursor; }
			set {
				if (this.panel != null) {
					this.panel.PlanCursor = value;
				} else {
					this.tmpCursor = value;
				}
			}
		}
		#endregion
	}
}
