using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using WW.Math;

namespace Europlan.Common {
	public partial class ModulKlimaDeckePlanner : Component, IProductPlanner {

		public enum KlimaDeckeMode {
			KDM_NONE,
			KDM_CONSTRUCTION,
			KDM_LAYOUT
		}

		/*public struct ConstructionParameters {
			float 
		}*/

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
				/*double minX = double.MaxValue;
				double maxX = double.MinValue;
				double minY = double.MaxValue;
				double maxY = double.MinValue;*/
				foreach (Point2D point in this.product.AssociatedRoom.RoomCoordinates) {
					/*if (point.X < minX) {
						minX = point.X;
					}
					if (point.X > maxX) {
						maxX = point.X;
					}
					if (point.Y < minY) {
						minY = point.Y;
					}
					if (point.Y > maxY) {
						maxY = point.Y;
					}*/
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
					//e.Graphics.Clip = clipEnabled;
					/*double curPos = minX;
					double increment = (breite + abstand) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
					while (curPos < maxX) {
						curPos += increment;
						Point2D paintP1 = additionalTransformation.TransformTo2D(new Point2D(curPos, minY));
						Point2D paintP2 = additionalTransformation.TransformTo2D(new Point2D(curPos + breite * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, maxY));
						float x = (float)Math.Min(paintP1.X, paintP2.X);
						float y = (float)Math.Min(paintP1.Y, paintP2.Y) - 1;
						float width = (float)Math.Abs(paintP1.X - paintP2.X);
						float height = (float)Math.Abs(paintP1.Y - paintP2.Y) + 2;
						c = Color.Red;
						b = new HatchBrush(HatchStyle.DiagonalCross, c, Color.FromArgb(0, c));
						e.Graphics.DrawRectangle(new Pen(c), x, y, width, height);
						e.Graphics.FillRectangle(b, x, y, width, height);
						//e.Graphics.FillRectangle(b, (float)paintP1.X, (float)paintP1.Y, (float)paintP2.X, (float)paintP2.Y);
					}*/
					if (this.product.GraphConstruction != null) {
						this.product.GraphConstruction.Paint(e.Graphics/*, minX, maxX, minY, maxY, this.product.AssociatedRoom.AssociatedPlan.Measure.Value, additionalTransformation*/);
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
			if (this.product.GraphConstruction != null) {
				if (this.product.GraphConstruction.HitTest(planPoint, pointInControl)) {
					this.customCursor = this.product.GraphConstruction.PickCursor;
					this.ConnectedPlanPanel.PlanCursor = this.product.GraphConstruction.PickCursor;
				} else {
					this.customCursor = Cursors.Default;
					this.ConnectedPlanPanel.PlanCursor = Cursors.Default;
				}
			}
			return false;
		}

		public bool PlannerDragStart(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			if (button == MouseButtons.Left && this.product.GraphConstruction != null) {
				this.product.GraphConstruction.StartDrag(planPoint, pointInControl);
			}
			return false;
		}

		public bool PlannerDragMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			if (button == MouseButtons.Left && this.product.GraphConstruction != null) {
				this.product.GraphConstruction.MoveDrag(planPoint, pointInControl);
				return true;
			}
			return false;
		}

		public bool PlannerDragEnd(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			// TODO
			return false;
		}

		public Cursor CustomCursor {
			get { return this.customCursor; }
		}

		public bool PlannerKeyPress(Keys key) {
			return false;
		}
		#endregion
	}
}
