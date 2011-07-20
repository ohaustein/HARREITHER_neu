using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using WW.Math;
using WW.Math.Geometry;

namespace Europlan.Common {
	public partial class DistributorPositioner : Component, IPlanner {

		public enum DistributorPositionerMode {
			DPM_NONE,
			DPM_POSITION,
		}

		public DistributorPositioner() {
			InitializeComponent();
		}

		public DistributorPositioner(IContainer container) {
			container.Add(this);

			InitializeComponent();
		}

		private DistributorPositionerMode mode = DistributorPositionerMode.DPM_NONE;
		private Distributor distributor;
		private Floor floor = null;
		private double rotation = 0.0;
		private bool unsavedChanges = false;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Distributor Distributor {
			get { return this.distributor; }
			set {
				this.distributor = value;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Floor Floor {
			get { return this.floor; }
			set { this.floor = value; }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double Rotation {
			get { return this.rotation; }
			set { this.rotation = value; }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double RotationInclPlan {
			get { return this.rotation - this.floor.AssociatedPlan.Rotation; }
		}

		public DistributorPositionerMode Mode {
			get { return this.mode; }
			set {
				if (value == DistributorPositionerMode.DPM_POSITION) {
					Nullable<Distributor.GraphicalRepresentation> gpToDelete = null;
					foreach (Distributor.GraphicalRepresentation gp in distributor.GraphicalRepresentations) {
						if (gp.floorId == this.floor.Id) {
							gpToDelete = gp;
							break;
						}
					}
					if (gpToDelete.HasValue) {
						// TODO: check if there are already products graphically connected to this distributor
						distributor.GraphicalRepresentations.Remove(gpToDelete.Value);
						this.ConnectedPlanPanel.InvalidateGraphics();
					}
				}
				this.mode = value;
			}
		}

		#region IProductPlanner Members
		private IPlanPanel connectedPlanPanel;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IPlanPanel ConnectedPlanPanel {
			get { return this.connectedPlanPanel; }
			set { this.connectedPlanPanel = value; }
		}

		public void PaintAfterPlanPannel(System.Windows.Forms.PaintEventArgs e, Matrix4D additionalTransformation, Point2D mousePositionInPlan, Point mousePositionInControl) {
			this.PaintAfterPlanPannel(e.Graphics, additionalTransformation, mousePositionInPlan, mousePositionInControl);
		}

		internal void PaintAfterPlanPannel(Graphics g, Matrix4D additionalTransformation, Point2D mousePositionInPlan, Point mousePositionInControl) {
			g.SmoothingMode = SmoothingMode.AntiAlias;
			Pen pen = Pens.Red;
			Pen otherPen = new Pen(Color.FromArgb(128, Color.Red));
			Brush otherBrush = new SolidBrush(Color.FromArgb(128, Color.Red));
			double width = distributor.Width * this.floor.AssociatedPlan.Measure.Value;
			double height = distributor.Height * this.floor.AssociatedPlan.Measure.Value;

			Point2D leftBottom = Point2D.Zero;
			Point2D leftTop = Point2D.Zero;
			Point2D rightTop = Point2D.Zero;
			Point2D rightBottom = Point2D.Zero;

			Matrix4D transformation = additionalTransformation;

			foreach (Distributor d in floor.Distributors) {
				if (d.Id != this.distributor.Id) {
					foreach (Distributor.GraphicalRepresentation gp in d.GraphicalRepresentations) {
						if (gp.floorId == this.floor.Id) {
							if (this.ConnectedPlanPanel.Plan is CadPlan) {
								transformation = additionalTransformation * Transformation4D.Translation(gp.position.X, gp.position.Y, 0);
								transformation = transformation * Transformation4D.RotateZ(-gp.rotation * Math.PI / 180.0);
								transformation = transformation * Transformation4D.Translation(-gp.position.X, -gp.position.Y, 0);

								leftBottom = transformation.TransformTo2D(gp.position);
								leftTop = transformation.TransformTo2D(new Point2D(gp.position.X, gp.position.Y + height));
								rightTop = transformation.TransformTo2D(new Point2D(gp.position.X + width, gp.position.Y + height));
								rightBottom = transformation.TransformTo2D(new Point2D(gp.position.X + width, gp.position.Y));
							} else {
								transformation = additionalTransformation * Transformation4D.Translation(gp.position.X, gp.position.Y, 0);
								transformation = transformation * Transformation4D.RotateZ(gp.rotation * Math.PI / 180.0);
								transformation = transformation * Transformation4D.Translation(-gp.position.X, -gp.position.Y, 0);

								leftBottom = transformation.TransformTo2D(gp.position);
								leftTop = transformation.TransformTo2D(new Point2D(gp.position.X, gp.position.Y - height));
								rightTop = transformation.TransformTo2D(new Point2D(gp.position.X + width, gp.position.Y - height));
								rightBottom = transformation.TransformTo2D(new Point2D(gp.position.X + width, gp.position.Y));
							}
							g.DrawLine(otherPen, (float)leftBottom.X, (float)leftBottom.Y, (float)rightBottom.X, (float)rightBottom.Y);
							g.DrawLine(otherPen, (float)rightBottom.X, (float)rightBottom.Y, (float)rightTop.X, (float)rightTop.Y);
							g.DrawLine(otherPen, (float)rightTop.X, (float)rightTop.Y, (float)leftTop.X, (float)leftTop.Y);
							g.DrawLine(otherPen, (float)leftTop.X, (float)leftTop.Y, (float)leftBottom.X, (float)leftBottom.Y);
							g.FillPolygon(otherBrush, new PointF[] { new PointF((float)leftBottom.X, (float)leftBottom.Y), new PointF((float)rightBottom.X, (float)rightBottom.Y), new PointF((float)rightTop.X, (float)rightTop.Y) });
							break;
						}
					}
				}
			}

			if (this.Mode == DistributorPositionerMode.DPM_POSITION) {
				if (this.ConnectedPlanPanel.Plan is CadPlan) {
					additionalTransformation = additionalTransformation * Transformation4D.Translation(mousePositionInPlan.X, mousePositionInPlan.Y, 0);
					additionalTransformation = additionalTransformation * Transformation4D.RotateZ(-this.RotationInclPlan * Math.PI / 180.0);
					additionalTransformation = additionalTransformation * Transformation4D.Translation(-mousePositionInPlan.X, -mousePositionInPlan.Y, 0);

					leftBottom = additionalTransformation.TransformTo2D(new Point2D(mousePositionInPlan.X, mousePositionInPlan.Y));
					leftTop = additionalTransformation.TransformTo2D(new Point2D(mousePositionInPlan.X, mousePositionInPlan.Y + height));
					rightTop = additionalTransformation.TransformTo2D(new Point2D(mousePositionInPlan.X + width, mousePositionInPlan.Y + height));
					rightBottom = additionalTransformation.TransformTo2D(new Point2D(mousePositionInPlan.X + width, mousePositionInPlan.Y));
				} else {
					additionalTransformation = additionalTransformation * Transformation4D.Translation(mousePositionInPlan.X, mousePositionInPlan.Y, 0);
					additionalTransformation = additionalTransformation * Transformation4D.RotateZ(this.RotationInclPlan * Math.PI / 180.0);
					additionalTransformation = additionalTransformation * Transformation4D.Translation(-mousePositionInPlan.X, -mousePositionInPlan.Y, 0);

					leftBottom = additionalTransformation.TransformTo2D(new Point2D(mousePositionInPlan.X, mousePositionInPlan.Y));
					leftTop = additionalTransformation.TransformTo2D(new Point2D(mousePositionInPlan.X, mousePositionInPlan.Y - height));
					rightTop = additionalTransformation.TransformTo2D(new Point2D(mousePositionInPlan.X + width, mousePositionInPlan.Y - height));
					rightBottom = additionalTransformation.TransformTo2D(new Point2D(mousePositionInPlan.X + width, mousePositionInPlan.Y));
				}
				g.DrawLine(pen, (float)leftBottom.X, (float)leftBottom.Y, (float)rightBottom.X, (float)rightBottom.Y);
				g.DrawLine(pen, (float)rightBottom.X, (float)rightBottom.Y, (float)rightTop.X, (float)rightTop.Y);
				g.DrawLine(pen, (float)rightTop.X, (float)rightTop.Y, (float)leftTop.X, (float)leftTop.Y);
				g.DrawLine(pen, (float)leftTop.X, (float)leftTop.Y, (float)leftBottom.X, (float)leftBottom.Y);
				g.FillPolygon(Brushes.Red, new PointF[] { new PointF((float)leftBottom.X, (float)leftBottom.Y), new PointF((float)rightBottom.X, (float)rightBottom.Y), new PointF((float)rightTop.X, (float)rightTop.Y) });
			} else {
				foreach (Distributor.GraphicalRepresentation gp in distributor.GraphicalRepresentations) {
					if (gp.floorId == this.floor.Id) {
						if (this.ConnectedPlanPanel.Plan is CadPlan) {
							additionalTransformation = additionalTransformation * Transformation4D.Translation(gp.position.X, gp.position.Y, 0);
							additionalTransformation = additionalTransformation * Transformation4D.RotateZ(-gp.rotation * Math.PI / 180.0);
							additionalTransformation = additionalTransformation * Transformation4D.Translation(-gp.position.X, -gp.position.Y, 0);

							leftBottom = additionalTransformation.TransformTo2D(gp.position);
							leftTop = additionalTransformation.TransformTo2D(new Point2D(gp.position.X, gp.position.Y + height));
							rightTop = additionalTransformation.TransformTo2D(new Point2D(gp.position.X + width, gp.position.Y + height));
							rightBottom = additionalTransformation.TransformTo2D(new Point2D(gp.position.X + width, gp.position.Y));
						} else {
							additionalTransformation = additionalTransformation * Transformation4D.Translation(gp.position.X, gp.position.Y, 0);
							additionalTransformation = additionalTransformation * Transformation4D.RotateZ(gp.rotation * Math.PI / 180.0);
							additionalTransformation = additionalTransformation * Transformation4D.Translation(-gp.position.X, -gp.position.Y, 0);

							leftBottom = additionalTransformation.TransformTo2D(gp.position);
							leftTop = additionalTransformation.TransformTo2D(new Point2D(gp.position.X, gp.position.Y - height));
							rightTop = additionalTransformation.TransformTo2D(new Point2D(gp.position.X + width, gp.position.Y - height));
							rightBottom = additionalTransformation.TransformTo2D(new Point2D(gp.position.X + width, gp.position.Y));
						}
						g.DrawLine(pen, (float)leftBottom.X, (float)leftBottom.Y, (float)rightBottom.X, (float)rightBottom.Y);
						g.DrawLine(pen, (float)rightBottom.X, (float)rightBottom.Y, (float)rightTop.X, (float)rightTop.Y);
						g.DrawLine(pen, (float)rightTop.X, (float)rightTop.Y, (float)leftTop.X, (float)leftTop.Y);
						g.DrawLine(pen, (float)leftTop.X, (float)leftTop.Y, (float)leftBottom.X, (float)leftBottom.Y);
						g.FillPolygon(Brushes.Red, new PointF[] { new PointF((float)leftBottom.X, (float)leftBottom.Y), new PointF((float)rightBottom.X, (float)rightBottom.Y), new PointF((float)rightTop.X, (float)rightTop.Y) });
						break;
					}
				}
			}
		}

		public bool PlannerClick(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			if (this.Mode == DistributorPositionerMode.DPM_POSITION && button == MouseButtons.Left) {
				Distributor.GraphicalRepresentation gp = new Distributor.GraphicalRepresentation();
				gp.position = planPoint;
				gp.rotation = this.RotationInclPlan;
				gp.isOnThisFloor = distributor.AssociatedFloor == this.Floor;
				gp.floorId = this.Floor.Id;
				distributor.GraphicalRepresentations.Add(gp);
				this.ConnectedPlanPanel.Mode = PlanMode.PM_MOVE;
				this.Mode = DistributorPositionerMode.DPM_NONE;
				if (ModeChanged != null) {
					this.ModeChanged(this, EventArgs.Empty);
				}
				return true;
			}
			return false;
		}

		public bool PlannerMouseMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			if (this.Mode == DistributorPositionerMode.DPM_POSITION) {
				return true;
			}
			return false;
		}

		public bool PlannerDragStart(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			// TODO
			return false;
		}

		public bool PlannerDragMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			// TODO
			return false;
		}

		public bool PlannerDragEnd(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			// TODO
			return false;
		}
		#endregion

		public bool UnsavedChanges {
			get { return this.unsavedChanges; }
		}

		public Cursor CustomCursor {
			get {
				if (this.mode == DistributorPositionerMode.DPM_POSITION) {
					return Cursors.Cross;
				} else {
					return null;
				}
			}
		}

		public event EventHandler ModeChanged;

		public bool PlannerKeyPress(Keys key) {
			if (key == Keys.Escape && this.Mode == DistributorPositionerMode.DPM_POSITION) {
				this.Mode = DistributorPositionerMode.DPM_NONE;
				this.ConnectedPlanPanel.Mode = PlanMode.PM_MOVE;
				if (this.ModeChanged != null) {
					this.ModeChanged(this, EventArgs.Empty);
				}
				return true;
			}
			return false;
		}

	}
}
