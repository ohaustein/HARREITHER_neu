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
using System.Xml.Serialization;
using WW.Cad.Model;
using WW.Cad.Model.Tables;
using WW.Cad.Model.Entities;

namespace Europlan.Common {
	public partial class HithermDrawer : Component, IProductPlanner {

		public HithermDrawer() {
			InitializeComponent();
		}

		public HithermDrawer(IContainer container) {
			container.Add(this);
			InitializeComponent();
		}

		private HithermProduct product;
		private bool highlightRoomCoordinates = true;
		private bool drawExpansionGaps = true;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public HithermProduct Product {
			get { return this.product; }
			set {
				this.product = value;
				if (this.ConnectedPlanPanel != null) {
					if (this.product == null || this.product.AssociatedRoom == null || this.product.AssociatedRoom.AssociatedPlan == null) {
						this.ConnectedPlanPanel.Plan = null;
					} else {
						this.ConnectedPlanPanel.Plan = this.product.AssociatedRoom.AssociatedPlan;
						Room room = this.product.AssociatedRoom;
						if (room.PlanSettingX.HasValue &&
							room.PlanSettingY.HasValue &&
							room.PlanSettingScale.HasValue &&
							room.PlanSettingAngle.HasValue) {
							this.ConnectedPlanPanel.SetPlanTransformations(room.PlanSettingScale.Value, room.PlanSettingX.Value, room.PlanSettingY.Value, room.PlanSettingAngle.Value);
						}
					}
				}
				this.connectionDrawer.Product = this.product;
				//this.connectionDrawer.Floor = (this.product != null && this.product.AssociatedRoom != null) ? this.product.AssociatedRoom.AssociatedFloor : null;
			}
		}

		#region IProductPlanner Members
		private IPlanPanel connectedPlanPanel;

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IPlanPanel ConnectedPlanPanel {
			get { return this.connectedPlanPanel; }
			set { this.connectedPlanPanel = value; }
		}

		public void PaintAfterPlanPannel(System.Windows.Forms.PaintEventArgs e, Matrix4D additionalTransformation, Point2D mousePositionInPlan, Point mousePositionInControl) {
			this.PaintAfterPlanPannel(e.Graphics, additionalTransformation, mousePositionInPlan, mousePositionInControl, false);
		}

		public void PaintAfterPlanPannel(Graphics g, Matrix4D additionalTransformation, Point2D mousePositionInPlan, Point mousePositionInControl, bool export) {
			if (this.product != null && this.product.AssociatedRoom != null && this.product.AssociatedRoom.RoomCoordinates != null) {

				if (!export) {
					// paint distributors in floor
					Floor floor = this.product.AssociatedRoom.AssociatedFloor;
					double measure = floor.AssociatedPlan.Measure.Value;
					bool invertYAxis = floor.AssociatedPlan.InvertYAxis;
					Region oldClip = g.Clip;
					Region newClip = new Region();
					newClip.MakeInfinite();
					g.Clip = newClip;
					foreach (Distributor dist in this.product.AssociatedRoom.AssociatedFloor.GetAllAvailableDistributors(true)) {
						dist.Draw(g, additionalTransformation, measure, invertYAxis, floor);
					}
					g.Clip = oldClip;

					// paint anbindeleitungen
					this.connectionDrawer.Paint(g, additionalTransformation);
				}
				
				GraphicsPath path = new GraphicsPath();
				List<PointF> transformedPoints = new List<PointF>();
				foreach (Point2D point in this.product.AssociatedRoom.RoomCoordinates) {
					Point2D tmp = additionalTransformation.TransformTo2D(point);
					transformedPoints.Add(new PointF((float)tmp.X, (float)tmp.Y));
				}
				path.AddPolygon(transformedPoints.ToArray());
				Region clipDisabled = new Region();
				clipDisabled.MakeInfinite();
				clipDisabled.Exclude(path);
				Color c = Color.Black;
				if (this.ConnectedPlanPanel != null && this.ConnectedPlanPanel.ColorMode == ColorMode.CM_BLACK_BG) {
					c = Color.White;
				}
				Brush b = new SolidBrush(c);
				b = new HatchBrush(System.Drawing.Drawing2D.HatchStyle.BackwardDiagonal, Color.FromArgb(128, c), Color.FromArgb(112, c));

				if (highlightRoomCoordinates) {
					// gray out all except the room
					g.FillRegion(b, clipDisabled);
				}


				if (this.product.AssociatedRoom.RoomCoordinates.Count > 2) {
					GraphicsPath fillPath = new GraphicsPath();
					fillPath.StartFigure();
					PointF[] array = new PointF[this.product.AssociatedRoom.RoomCoordinates.Count];
					int i = 0;
					foreach (Point2D point in this.product.AssociatedRoom.RoomCoordinates) {
						Point2D tmp = additionalTransformation.TransformTo2D(point);
						array[i++] = new PointF((float)tmp.X, (float)tmp.Y);
					}
					fillPath.AddPolygon(array);
					fillPath.CloseFigure();
					c = Color.FromArgb(64, Color.Blue);
					b = new SolidBrush(c);
					//g.FillPath(b, fillPath);
					g.DrawPath(new Pen(b), fillPath);
					fillPath.Dispose();
				}

				if (drawExpansionGaps) {
					foreach (Segment2D expansionGap in this.Product.AssociatedRoom.AssociatedFloor.ExpansionGaps) {
						Point2D start = additionalTransformation.TransformTo2D(expansionGap.Start);
						Point2D end = additionalTransformation.TransformTo2D(expansionGap.End);
						g.DrawLine(Pens.Blue, (float)start.X, (float)start.Y, (float)end.X, (float)end.Y);
					}
				}

				path.Dispose();
			}
		}

		public bool PlannerClick(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			return false;
		}

		public bool PlannerMouseMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			return false;
		}

		public bool PlannerDragStart(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			return false;
		}

		public bool PlannerDragMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			return false;
		}

		public bool PlannerDragEnd(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			return false;
		}

		public bool PlannerKeyPress(Keys key) {
			return false;
		}
		#endregion

		[DefaultValue(true)]
		public bool HighlightRoomCoordinates {
			get { return this.highlightRoomCoordinates; }
			set { this.highlightRoomCoordinates = value; }
		}

		[DefaultValue(true)]
		public bool DrawExpansionGaps {
			get { return this.drawExpansionGaps; }
			set { this.drawExpansionGaps = value; }
		}

		public Cursor CustomCursor {
			get { return null; }
		}
	}
}
