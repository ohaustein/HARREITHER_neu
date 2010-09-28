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
	public partial class PlanPanel : UserControl, IPlanPanel {

		public enum PlanTypeEnum {
			PT_CAD,
			PT_IMAGE
		}

		private Plan plan = null;
		private IPlanPanel panel = null;
		private PlanMode tmpMode = PlanMode.PM_MOVE;
		private IProductPlanner tmpPlanner = null;

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
						tmpPlanner = this.panel.ProductPlanner;
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
					} else {
						this.imagePanel.Visible = false;
						this.cadPanel.Visible = false;
						this.cadOptions.Visible = false;
						this.panel = null;
					}
					if (this.panel != null) {
						this.panel.Mode = tmpMode;
						this.panel.ProductPlanner = tmpPlanner;
					}
				}
			}
		}


		#region IPlanPanel Members

		public IProductPlanner ProductPlanner {
			get { return this.panel == null ? tmpPlanner : this.panel.ProductPlanner; }
			set {
				if (this.panel != null) {
					this.panel.ProductPlanner = value;
				} else {
					tmpPlanner = value;
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
		#endregion

		private void cadPanelOptions_InvalidateNeeded(object sender, EventArgs e) {
			if (this.cadPanel.Visible) {
				this.cadPanel.RecreateDrawables();
			}
		}
	}
}
