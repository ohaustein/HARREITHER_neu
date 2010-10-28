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
	public partial class ModulKlimaDeckePlanner : Component, IProductPlanner {

		public enum KlimaDeckeMode {
			KDM_NONE,
			KDM_CONSTRUCTION,
			KDM_LAYOUT_ADD_AREA
		}

		public ModulKlimaDeckePlanner() {
			InitializeComponent();
		}

		public ModulKlimaDeckePlanner(IContainer container) {
			container.Add(this);

			InitializeComponent();
		}

		private ModulKlimaDeckeProduct product;
		private KlimaDeckeMode mode = KlimaDeckeMode.KDM_NONE;
		private Cursor customCursor = null;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ModulKlimaDeckeProduct Product {
			get { return this.product; }
			set {
				this.product = value;
				if (this.ConnectedPlanPanel != null) {
					if (this.product == null || this.product.AssociatedRoom == null || this.product.AssociatedRoom.AssociatedPlan == null) {
						this.ConnectedPlanPanel.Plan = null;
					} else {
						this.ConnectedPlanPanel.Plan = this.product.AssociatedRoom.AssociatedPlan;
					}
				}
			}
		}

		public KlimaDeckeMode Mode {
			get { return this.mode; }
			set { this.mode = value; }
		}

		#region IProductPlanner Members
		private IPlanPanel connectedPlanPanel;

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IPlanPanel ConnectedPlanPanel {
			get { return this.connectedPlanPanel; }
			set { this.connectedPlanPanel = value; }
		}

		private double breite = 0.1; // meter
		private double abstand = 0.5; // meter

		public void PaintAfterPlanPannel(System.Windows.Forms.PaintEventArgs e, Matrix4D additionalTransformation, Point2D mousePositionInPlan, Point mousePositionInControl) {
			if (this.product != null && this.product.AssociatedRoom != null && this.product.AssociatedRoom.RoomCoordinates != null) {
				GraphicsPath path = new GraphicsPath();
				List<PointF> transformedPoints = new List<PointF>();
				foreach (Point2D point in this.product.AssociatedRoom.RoomCoordinates) {
					Point2D tmp = additionalTransformation.TransformTo2D(point);
					transformedPoints.Add(new PointF((float)tmp.X, (float)tmp.Y));
				}
				path.AddPolygon(transformedPoints.ToArray());
				//e.Graphics.DrawPath(System.Drawing.Pens.Red, path);
				//Region clipEnabled = new Region(path);
				Region clipDisabled = new Region();
				clipDisabled.MakeInfinite();
				clipDisabled.Exclude(path);
				path.Dispose();
				Color c = Color.Black;
				if (this.ConnectedPlanPanel != null && this.ConnectedPlanPanel.ColorMode == ColorMode.CM_BLACK_BG) {
					c = Color.White;
				}
				Brush b = new SolidBrush(c);
				//e.Graphics.FillRegion(b, clipRegion);
				//c = Color.FromArgb(128, Color.Black);
				//b = new SolidBrush(c);
				b = new HatchBrush(HatchStyle.BackwardDiagonal, Color.FromArgb(128, c), Color.FromArgb(112, c));
				e.Graphics.FillRegion(b, clipDisabled);

				if (this.product.AssociatedRoom.AssociatedPlan != null && this.product.AssociatedRoom.AssociatedPlan.Measure.HasValue) {
					if (this.product.GraphConstruction != null) {
						this.product.GraphConstruction.Paint(e.Graphics, this.Mode);
					}
				}

				if (this.layoutAddArea != null) {
					PointF[] drawArea = new PointF[this.layoutAddArea.Count];
					for (int i = 0; i < this.layoutAddArea.Count; i++) {
						Point2D tmp = additionalTransformation.TransformTo2D(this.layoutAddArea[i]);
						drawArea[i] = new PointF((float)tmp.X, (float)tmp.Y);
					}
					e.Graphics.DrawPolygon(Pens.Red, drawArea);

					Matrix3D rotation = Transformation3D.Rotate(-this.product.GraphConstruction.Rotation * Math.PI / 180.0);
					Matrix3D invRotation = rotation.GetInverse();

					Segment2D topSeg = new Segment2D(rotation.Transform(this.layoutAddArea[0]), rotation.Transform(this.layoutAddArea[3]));
					double start = topSeg.Start.Y;
					double end = rotation.Transform(this.layoutAddArea[1]).Y;
					double step = KlimaFlaechenModul.GetModuleHeight(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_30) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
					foreach (PossibleModulRow row in this.product.GraphConstruction.PossibleRows) {
						Point2D borderLeftOrigin = rotation.Transform(row.BorderLeft.Origin);
						borderLeftOrigin.Y = topSeg.Start.Y;
						Point2D borderRighOrigin = rotation.Transform(row.BorderRight.Origin);
						borderLeftOrigin.Y = topSeg.Start.Y;
						if (Line2D.Intersects(new Line2D(borderLeftOrigin, new Vector2D(0, 1)), topSeg) && Line2D.Intersects(new Line2D(borderRighOrigin, new Vector2D(0, 1)), topSeg)) {
							if (this.layoutAddAreaBottomToTop) {
								for (double y = start - step; y > end; y -= step) {
									Point2D tmp = invRotation.Transform(new Point2D(borderLeftOrigin.X, y));
									this.DrawModule(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_30, KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT, new Point2D(tmp.X, tmp.Y), additionalTransformation, e.Graphics);
								}
							} else {
								for (double y = start; y < end - step; y += step) {
									Point2D tmp = invRotation.Transform(new Point2D(borderLeftOrigin.X, y));
									this.DrawModule(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_30, KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT, new Point2D(tmp.X, tmp.Y), additionalTransformation, e.Graphics);
								}
							}
						}
					}

				}
			}
		}

		public bool PlannerClick(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			// TODO
			return false;
		}

		public bool PlannerMouseMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			// TODO
			if (this.Mode == KlimaDeckeMode.KDM_CONSTRUCTION) {
				if (this.product.GraphConstruction != null) {
					if (this.product.GraphConstruction.HitTest(planPoint, pointInControl)) {
						this.customCursor = this.product.GraphConstruction.PickCursor;
						this.ConnectedPlanPanel.PlanCursor = this.product.GraphConstruction.PickCursor;
					} else {
						this.customCursor = Cursors.Default;
						this.ConnectedPlanPanel.PlanCursor = Cursors.Default;
					}
				}
			}
			return false;
		}

		private Point2D layoutAddAreaStart;
		private Polygon2D layoutAddArea = null;
		private bool layoutAddAreaBottomToTop = false;

		public bool PlannerDragStart(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			if (this.Mode == KlimaDeckeMode.KDM_CONSTRUCTION) {
				if (button == MouseButtons.Left && this.product.GraphConstruction != null) {
					this.product.GraphConstruction.StartDrag(planPoint, pointInControl);
				}
			} else if (this.Mode == KlimaDeckeMode.KDM_LAYOUT_ADD_AREA) {
				this.layoutAddAreaStart = planPoint;
			}
			return false;
		}

		public bool PlannerDragMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			if (this.Mode == KlimaDeckeMode.KDM_CONSTRUCTION) {
				if (button == MouseButtons.Left && this.product.GraphConstruction != null) {
					this.product.GraphConstruction.MoveDrag(planPoint, pointInControl);
					return true;
				}
			} else if (this.Mode == KlimaDeckeMode.KDM_LAYOUT_ADD_AREA) {
				if (button == MouseButtons.Left && this.product.GraphConstruction != null) {
					Matrix3D matrix = Transformation3D.Rotate(-this.product.GraphConstruction.Rotation * Math.PI / 180.0);
					Point2D rotatedP1 = matrix.Transform(layoutAddAreaStart);
					Point2D rotatedP3 = matrix.Transform(planPoint);
					this.layoutAddAreaBottomToTop = (rotatedP1.Y > rotatedP3.Y);
					Point2D rotatedP2 = new Point2D(rotatedP1.X, rotatedP3.Y);
					Point2D rotatedP4 = new Point2D(rotatedP3.X, rotatedP1.Y);
					matrix = matrix.GetInverse();
					//Point2D p2 = matrix.Transform(rotatedP2);
					//Point2D p4 = matrix.Transform(rotatedP4);
					layoutAddArea = new Polygon2D();
					layoutAddArea.Add(layoutAddAreaStart);
					layoutAddArea.Add(matrix.Transform(rotatedP2));
					layoutAddArea.Add(planPoint);
					layoutAddArea.Add(matrix.Transform(rotatedP4));
					return true;
				}
			}
			return false;
		}

		public bool PlannerDragEnd(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			// TODO
			if (this.layoutAddArea != null) {
				this.layoutAddArea = null;
				return true;
			}
			return false;
		}

		public Cursor CustomCursor {
			get { return this.customCursor; }
		}

		public bool PlannerKeyPress(Keys key) {
			return false;
		}

		public void DrawModule(KlimaFlaechenModul.ModulTypeEnum type, KlimaFlaechenModul.ModulOrientationEnum orientation, Point2D position, Matrix4D additionalTransformation, Graphics g) {
			if (this.product == null || this.product.GraphConstruction == null ||
				this.product.AssociatedRoom == null || this.product.AssociatedRoom.AssociatedPlan == null ||
				this.product.AssociatedRoom.AssociatedPlan.Measure == null) {
				return;
			}
			additionalTransformation = additionalTransformation * Transformation4D.Translation(position.X, position.Y, 0);
			additionalTransformation = additionalTransformation * Transformation4D.RotateZ(this.product.GraphConstruction.Rotation * Math.PI / 180.0);

			double height = KlimaFlaechenModul.GetModuleHeight(type) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
			double width = KlimaFlaechenModul.GetModuleWidth(type) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;

			Point2D topLeft2D = additionalTransformation.TransformTo2D(new Point2D(0, 0));
			Point2D topRight2D = additionalTransformation.TransformTo2D(new Point2D(width, 0));
			Point2D bottomRight2D = additionalTransformation.TransformTo2D(new Point2D(width, height));
			Point2D bottomLeft2D = additionalTransformation.TransformTo2D(new Point2D(0, height));

			PointF topLeft = new PointF((float)topLeft2D.X, (float)topLeft2D.Y);
			PointF topRight = new PointF((float)topRight2D.X, (float)topRight2D.Y);
			PointF bottomRight = new PointF((float)bottomRight2D.X, (float)bottomRight2D.Y);
			PointF bottomLeft = new PointF((float)bottomLeft2D.X, (float)bottomLeft2D.Y);

			Color c = Color.FromArgb(128, 0, 255, 0);
			Pen p = new Pen(c);
			Brush b = new SolidBrush(Color.FromArgb(64, c));
			if (orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT) {
				g.FillPolygon(b, new PointF[] { topLeft, topRight, bottomRight, bottomLeft });
				g.DrawLines(p, new PointF[] { topLeft, topRight, bottomRight, bottomLeft, topRight });
			} else {
				g.FillPolygon(b, new PointF[] { topLeft, topRight, bottomRight, bottomLeft });
				g.DrawLines(p, new PointF[] { topRight, topLeft, topRight, bottomRight, topLeft });
			}
		}
		#endregion
	}
}
