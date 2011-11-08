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
	public partial class PdfRegionPicker : Component, IPlanner {

		public enum PdfRegionPickerMode {
			DPM_NONE,
			DPM_PICK_REGION,
		}

		public PdfRegionPicker() {
			InitializeComponent();
		}

		public PdfRegionPicker(IContainer container) {
			container.Add(this);

			InitializeComponent();
		}

		private PdfRegionPickerMode mode = PdfRegionPickerMode.DPM_NONE;
		private bool unsavedChanges = false;

		private Nullable<Point2D> startPoint = null;
		private Nullable<Point2D> curPoint = null;
		private Nullable<Point2D> endPoint = null;

		public PdfRegionPickerMode Mode {
			get { return this.mode; }
			set {
				if (value == PdfRegionPickerMode.DPM_PICK_REGION) {
					/*Nullable<Distributor.GraphicalRepresentation> gpToDelete = null;
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
					}*/
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
			set {
				this.connectedPlanPanel = value;
				if (this.connectedPlanPanel != null && this.connectedPlanPanel.Plan != null && this.connectedPlanPanel.Plan is TempImagePlan) {
					this.startPoint = new Point2D(0, 0);
					this.endPoint = (this.connectedPlanPanel.Plan as TempImagePlan).GetImageSize();
				} else {
					this.startPoint = null;
					this.endPoint = null;
				}
			}
		}

		public void PaintAfterPlanPannel(System.Windows.Forms.PaintEventArgs e, Matrix4D additionalTransformation, Point2D mousePositionInPlan, Point mousePositionInControl) {
			this.PaintAfterPlanPannel(e.Graphics, additionalTransformation, mousePositionInPlan, mousePositionInControl);
		}

		internal void PaintAfterPlanPannel(Graphics g, Matrix4D additionalTransformation, Point2D mousePositionInPlan, Point mousePositionInControl) {
			g.SmoothingMode = SmoothingMode.AntiAlias;
			Pen pen = Pens.Green;
			//Pen otherPen = new Pen(Color.FromArgb(128, Color.Red));
			Brush brush = new SolidBrush(Color.FromArgb(32, Color.Green));

			if (this.startPoint.HasValue) {
				if (this.endPoint.HasValue) {
					Point2D start = additionalTransformation.TransformTo2D(this.startPoint.Value);
					Point2D end = additionalTransformation.TransformTo2D(this.endPoint.Value);
					float x = (float)((start.X < end.X) ? start.X : end.X);
					float y = (float)((start.Y < end.Y) ? start.Y : end.Y);
					float width = (float)Math.Abs(end.X - start.X);
					float height = (float)Math.Abs(end.Y - start.Y);
					g.FillRectangle(brush, x, y, width, height);
					g.DrawRectangle(pen, x, y, width, height);
				} else if (this.curPoint.HasValue) {
					Point2D start = additionalTransformation.TransformTo2D(this.startPoint.Value);
					Point2D end = additionalTransformation.TransformTo2D(this.curPoint.Value);
					float x = (float)((start.X < end.X) ? start.X : end.X);
					float y = (float)((start.Y < end.Y) ? start.Y : end.Y);
					float width = (float)Math.Abs(end.X - start.X);
					float height = (float)Math.Abs(end.Y - start.Y);
					g.FillRectangle(brush, x, y, width, height);
					g.DrawRectangle(pen, x, y, width, height);
				}
			}

			/*double width = distributor.Width * this.floor.AssociatedPlan.Measure.Value;
			double height = distributor.Height * this.floor.AssociatedPlan.Measure.Value;

			Point2D leftBottom = Point2D.Zero;
			Point2D leftTop = Point2D.Zero;
			Point2D rightTop = Point2D.Zero;
			Point2D rightBottom = Point2D.Zero;

			if (DrawOtherDistributorsInPlan) {
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
			}

			if (this.Mode == PdfRegionPickerMode.DPM_PICK_REGION) {
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
			}*/
		}

		public bool PlannerClick(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			/*if (this.Mode == PdfRegionPickerMode.DPM_PICK_REGION && button == MouseButtons.Left) {
				Distributor.GraphicalRepresentation gp = new Distributor.GraphicalRepresentation();
				gp.position = planPoint;
				gp.rotation = this.RotationInclPlan;
				gp.isOnThisFloor = distributor.AssociatedFloor == this.Floor;
				gp.floorId = this.Floor.Id;
				distributor.GraphicalRepresentations.Add(gp);
				this.ConnectedPlanPanel.Mode = PlanMode.PM_MOVE;
				this.Mode = PdfRegionPickerMode.DPM_NONE;
				if (ModeChanged != null) {
					this.ModeChanged(this, EventArgs.Empty);
				}
				return true;
			}*/
			return false;
		}

		public bool PlannerMouseMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			/*if (this.Mode == PdfRegionPickerMode.DPM_PICK_REGION) {
				return true;
			}*/
			return false;
		}

		public bool PlannerDragStart(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			// TODO
			this.startPoint = planPoint;
			this.endPoint = null;
			return false;
		}

		public bool PlannerDragMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			// TODO
			this.curPoint = planPoint;
			return !this.endPoint.HasValue;
		}

		public bool PlannerDragEnd(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			// TODO
			this.endPoint = planPoint;
			this.curPoint = null;
			return false;
		}
		#endregion

		public bool UnsavedChanges {
			get { return this.unsavedChanges; }
		}

		public Cursor CustomCursor {
			get {
				if (this.mode == PdfRegionPickerMode.DPM_PICK_REGION) {
					return Cursors.Cross;
				} else {
					return null;
				}
			}
		}

		public event EventHandler ModeChanged;

		public bool PlannerKeyPress(Keys key) {
			/*if (key == Keys.Escape && this.Mode == PdfRegionPickerMode.DPM_PICK_REGION) {
				this.Mode = PdfRegionPickerMode.DPM_NONE;
				this.ConnectedPlanPanel.Mode = PlanMode.PM_MOVE;
				if (this.ModeChanged != null) {
					this.ModeChanged(this, EventArgs.Empty);
				}
				return true;
			}*/
			return false;
		}

		public Nullable<Point2D> TopLeft {
			get {
				if (this.startPoint.HasValue && this.endPoint.HasValue) {
					return new Point2D((float)(Math.Min(this.startPoint.Value.X, this.endPoint.Value.X)), (float)(Math.Min(this.startPoint.Value.Y, this.endPoint.Value.Y)));
				} else {
					return null;
				}
			}
		}

		public Nullable<Point2D> BottomRight {
			get {
				if (this.startPoint.HasValue && this.endPoint.HasValue) {
					return new Point2D((float)(Math.Max(this.startPoint.Value.X, this.endPoint.Value.X)), (float)(Math.Max(this.startPoint.Value.Y, this.endPoint.Value.Y)));
				} else {
					return null;
				}
			}
		}
	}
}
